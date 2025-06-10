using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using KesimTakip.UsrControl;

namespace KesimTakip.DataBase
{
    class KesimListesiPaketData
    {
        public static bool SaveKesimDataPaket(string olusturan, string kesimId, int kesilecekPlanSayisi, int toplamPlanTekrari, DateTime eklemeTarihi)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"IF NOT EXISTS (SELECT 1 FROM KesimListesiPaket WHERE kesimId = @kesimId)
                            INSERT INTO KesimListesiPaket ([olusturan], [kesimId], [kesilecekPlanSayisi], [toplamPlanTekrari], [eklemeTarihi])
                            VALUES (@olusturan, @kesimId, @kesilecekPlanSayisi, @toplamPlanTekrari, @eklemeTarihi)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@olusturan", SqlDbType.NVarChar).Value = olusturan;
                        cmd.Parameters.Add("@kesimId", SqlDbType.NVarChar).Value = kesimId;
                        cmd.Parameters.Add("@kesilecekPlanSayisi", SqlDbType.Int).Value = kesilecekPlanSayisi;
                        cmd.Parameters.Add("@toplamPlanTekrari", SqlDbType.Int).Value = toplamPlanTekrari;
                        cmd.Parameters.Add("@eklemeTarihi", SqlDbType.DateTime).Value = eklemeTarihi;

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
                 { "Kesim ID", ("[kesimId]", "text") },
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
        public static bool KesimIdVarMi(string kesimId)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM KesimListesiPaket 
        WHERE SUBSTRING(KesimId, 1, CHARINDEX('-', KesimId) - 1) = @KesimId";

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var command = new SqlCommand(query, conn))
                {
                    string kesimIdSade = kesimId.Split('-')[0];
                    command.Parameters.AddWithValue("@KesimId", kesimIdSade);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
