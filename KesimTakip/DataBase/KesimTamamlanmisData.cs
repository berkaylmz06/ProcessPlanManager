using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KesimTakip.DataBase
{
    class KesimTamamlanmisData
    {
        public static bool TablodanKesimTamamlanmisEkleme(string kesimYapan, string kesimId, int kesilmisPlanSayisi, DateTime kesimTarihi, TimeSpan kesimSaati)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();

                    string query = @"
            IF EXISTS (SELECT 1 FROM [KesimTamamlanmisPaket] WHERE [kesimId] = @kesimId)
            BEGIN
                UPDATE [KesimTamamlanmisPaket]
                SET 
                    [kesilmisPlanSayisi] = [kesilmisPlanSayisi] + @kesilmisPlanSayisi,
                    [kesimYapan] = @kesimYapan,
                    [kesimTarihi] = @kesimTarihi,
                    [kesimSaati] = @kesimSaati
                WHERE [kesimId] = @kesimId
            END
            ELSE
            BEGIN
                INSERT INTO [KesimTamamlanmisPaket] 
                    ([kesimYapan], [kesimId], [kesilmisPlanSayisi], [kesimTarihi], [kesimSaati])
                VALUES 
                    (@kesimYapan, @kesimId, @kesilmisPlanSayisi, @kesimTarihi, @kesimSaati)
            END";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kesimYapan", kesimYapan);
                        command.Parameters.AddWithValue("@kesimId", kesimId);
                        command.Parameters.AddWithValue("@kesilmisPlanSayisi", kesilmisPlanSayisi);
                        command.Parameters.AddWithValue("@kesimTarihi", kesimTarihi);
                        command.Parameters.AddWithValue("@kesimSaati", kesimSaati);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
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
