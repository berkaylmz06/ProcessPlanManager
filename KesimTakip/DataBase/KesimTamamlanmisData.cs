using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KesimTakip.DataBase
{
    class KesimTamamlanmisData
    {
        public static void TablodanKesimTamamlanmisEkleme(string kesimYapan, string kesimId, int kesilmisPlanSayisi, string kesimTarihi, string kesimSaati)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();

                string insertQuery = "INSERT INTO [KesimTamamlanmisPaket] ([kesimYapan], [kesimId], [kesilmisPlanSayisi], [kesimTarihi], [kesimSaati]) " +
                                     "VALUES (@kesimYapan, @kesimId, @kesilmisPlanSayisi, @kesimTarihi, @kesimSaati)";

                using (var insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@kesimYapan", kesimYapan);
                    insertCommand.Parameters.AddWithValue("@kesimId", kesimId);
                    insertCommand.Parameters.AddWithValue("@kesilmisPlanSayisi", kesilmisPlanSayisi);
                    insertCommand.Parameters.AddWithValue("@kesimTarihi", kesimTarihi);
                    insertCommand.Parameters.AddWithValue("@kesimSaati", kesimSaati);

                    insertCommand.ExecuteNonQuery();
                }
            }
        }
        public DataTable GetKesimListesTamamlanmis()
        {
            string query = "SELECT [kesimYapan], [kesimId], [kesilmisPlanSayisi], [kesimTarihi], [kesimSaati] FROM [KesimTamamlanmisPaket]";
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
        public static DataTable KesimTamamlanmisFiltrele(Dictionary<string, TextBox> filtreKutulari)
        {
            var dt = new DataTable();

            Dictionary<string, (string dbColumn, string dataType)> labelToDbColumn = new Dictionary<string, (string, string)>
            {
                 { "Kesim Yapan", ("kesimYapan", "text") },
                 { "Kesim ID", ("[kesimId]", "text") },
                 { "Kesilmiş Plan Sayısı",("[kesilmisPlanSayisi]", "int") },
                 { "Kesim Tarihi", ("[kesimTarihi]", "text") },
                 { "Kesim Saati", ("[kesimSaati]", "text") }
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

            string query = "SELECT * FROM [KesimTamamlanmisPaket]";

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
