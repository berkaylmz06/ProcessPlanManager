using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KesimTakip.DataBase
{
    class KesimListesiPaketData
    {
        public static void SaveKesimDataPaket(string olusturan, int kesimId, int kesilecekPlanSayisi, int toplamPlanTekrari, string eklemeTarihi)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO \"KesimListesiPaket\" (\"olusturan\", \"kesimId\", \"kesilecekPlanSayisi\", \"toplamPlanTekrari\", \"eklemeTarihi\")" +
                               "VALUES (@olusturan, @kesimId, @kesilecekPlanSayisi, @toplamPlanTekrari, @eklemeTarihi)";

                using (var cmd = new NpgsqlCommand(query, conn))
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
        public DataTable GetKesimListesiPaket()
        {
            string query = "SELECT \"olusturan\", \"kesimId\", \"kesilecekPlanSayisi\", \"kesilmisPlanSayisi\", \"toplamPlanTekrari\", \"eklemeTarihi\" FROM \"KesimListesiPaket\"";
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var adapter = new NpgsqlDataAdapter(query, connection))
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
                string query = "SELECT COUNT(1) FROM \"KesimListesiPaket\" WHERE \"kesimId\" = @kesimId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kesimId", kesimId);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        public static bool KesimListesiPaketKontrolluDusme(int kesimId, int kesilenMiktar, out string hataMesaji)
        {
            hataMesaji = "";

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();

                string selectQuery = "SELECT \"kesilecekPlanSayisi\" FROM \"KesimListesiPaket\" WHERE \"kesimId\" = @id";
                using (var selectCommand = new NpgsqlCommand(selectQuery, conn))
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

                        string updateQuery = "UPDATE \"KesimListesiPaket\" " +
                                             "SET \"kesilecekPlanSayisi\" = \"kesilecekPlanSayisi\" - @azalt, " +
                                             "\"kesilmisPlanSayisi\" = \"kesilmisPlanSayisi\" + @arttir " +
                                             "WHERE \"kesimId\" = @id";

                        using (var updateCommand = new NpgsqlCommand(updateQuery, conn))
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
            string query = "SELECT * FROM \"KesimListesiPaket\"";
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
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
                 { "Kesim ID", ("\"kesimId\"", "int") },
                 { "Kesilecek Plan Sayısı",("\"kesilecekPlanSayisi\"", "int") },
                 { "Kesilmiş Plan Sayısı",("\"kesilmisPlanSayisi\"", "int") },
                 { "Toplam Plan Tekrarı",("\"toplamPlanTekrari\"", "int") },
                 { "Ekleme Tarihi", ("\"eklemeTarihi\"", "text") }
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
                        kosullar.Add($"{dbKolon} ILIKE '%{deger.Replace("'", "''")}%'");
                    }
                    else if (veriTuru == "int" && int.TryParse(deger, out int sayi))
                    {
                        kosullar.Add($"{dbKolon} = {sayi}");
                    }
                }
            }

            string query = "SELECT * FROM \"KesimListesiPaket\"";

            if (kosullar.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", kosullar);
            }

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var da = new Npgsql.NpgsqlDataAdapter(query, conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }


    }
}
