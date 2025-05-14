using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KesimTakip.DataBase
{
    class KesimListesiPaketData
    {
        public static void SaveKesimDataPaket(string olusturan, string kesimId, int kesilecekPlanSayisi, int toplamPlanTekrari, string eklemeTarihi)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO [KesimListesiPaket] ([olusturan], [kesimId], [kesilecekPlanSayisi], [toplamPlanTekrari], [eklemeTarihi])" +
                                   "VALUES (@olusturan, @kesimId, @kesilecekPlanSayisi, @toplamPlanTekrari, @eklemeTarihi)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@olusturan", olusturan);
                        cmd.Parameters.AddWithValue("@kesimId", kesimId);
                        cmd.Parameters.AddWithValue("@kesilecekPlanSayisi", kesilecekPlanSayisi);
                        cmd.Parameters.AddWithValue("@toplamPlanTekrari", toplamPlanTekrari);
                        cmd.Parameters.AddWithValue("@eklemeTarihi", eklemeTarihi);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        public DataTable GetKesimListesiPaket()
        {
            string query = "SELECT [olusturan], [kesimId], [kesilecekPlanSayisi], [kesilmisPlanSayisi], [toplamPlanTekrari], [eklemeTarihi] FROM [KesimListesiPaket]";
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static bool KesimListesiPaketKesimIdVarsa(int kesimId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM [KesimListesiPaket] WHERE [kesimId] = @kesimId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kesimId", kesimId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public static bool KesimListesiPaketKontrolluDusme(string kesimId, int kesilenMiktar, out string hataMesaji)
        {
            hataMesaji = "";

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();

                string selectQuery = "SELECT [kesilecekPlanSayisi] FROM [KesimListesiPaket] WHERE [kesimId] = @id";
                using (var selectCommand = new SqlCommand(selectQuery, conn))
                {
                    selectCommand.Parameters.AddWithValue("@id", kesimId);
                    var mevcutDegerObj = selectCommand.ExecuteScalar();

                    if (mevcutDegerObj != null && int.TryParse(mevcutDegerObj.ToString(), out int mevcutMiktar))
                    {
                        if (kesilenMiktar > mevcutMiktar)
                        {
                            hataMesaji = $"Girilen miktar ({kesilenMiktar}) mevcut değerden ({mevcutMiktar}) fazla olamaz!";
                            return false;
                        }

                        string updateQuery = "UPDATE [KesimListesiPaket] " +
                                             "SET [kesilecekPlanSayisi] = [kesilecekPlanSayisi] - @azalt, " +
                                             "[kesilmisPlanSayisi] = [kesilmisPlanSayisi] + @arttir " +
                                             "WHERE [kesimId] = @id";

                        using (var updateCommand = new SqlCommand(updateQuery, conn))
                        {
                            updateCommand.Parameters.AddWithValue("@azalt", kesilenMiktar);
                            updateCommand.Parameters.AddWithValue("@arttir", kesilenMiktar);
                            updateCommand.Parameters.AddWithValue("@id", kesimId);

                            updateCommand.ExecuteNonQuery();
                        }

                        return true;
                    }
                    else
                    {
                        hataMesaji = "Mevcut değer alınamadı.";
                        return false;
                    }
                }
            }
        }

        public static void VerileriYenile(DataGridView data)
        {
            string query = "SELECT * FROM [KesimListesiPaket]";
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    data.DataSource = dataTable;
                }
            }
        }

        public static DataTable KesimListesiniPaketFiltrele(Dictionary<string, TextBox> filtreKutulari)
        {
            var dt = new DataTable();

            Dictionary<string, (string dbColumn, string dataType)> labelToDbColumn = new Dictionary<string, (string, string)>
            {
                 { "Planı Oluşturan", ("olusturan", "text") },
                 { "Kesim ID", ("[kesimId]", "int") },
                 { "Kesilecek Plan Sayısı",("[kesilecekPlanSayisi]", "int") },
                 { "Kesilmiş Plan Sayısı",("[kesilmisPlanSayisi]", "int") },
                 { "Toplam Plan Tekrarı",("[toplamPlanTekrari]", "int") },
                 { "Ekleme Tarihi", ("[eklemeTarihi]", "text") }
            };

            List<string> kosullar = new List<string>();

            foreach (var pair in filtreKutulari)
            {
                string labelText = pair.Key;

                if (!labelToDbColumn.ContainsKey(labelText))
                    continue;

                var (dbKolon, veriTuru) = labelToDbColumn[labelText];
                string deger = pair.Value.Text;

                if (!string.IsNullOrWhiteSpace(deger))
                {
                    if (veriTuru == "text")
                    {
                        kosullar.Add($"{dbKolon} LIKE '%{deger.Replace("'", "''")}%'");
                    }
                    else if (veriTuru == "int" && int.TryParse(deger, out int sayi))
                    {
                        kosullar.Add($"{dbKolon} = {sayi}");
                    }
                }
            }

            string query = "SELECT * FROM [KesimListesiPaket]";

            if (kosullar.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", kosullar);
            }

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var da = new SqlDataAdapter(query, conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }
    }
}
