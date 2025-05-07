using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KesimTakip.DataBase
{
    class KesimTamamlanmisData
    {
        public static void TablodanKesimTamamlanmisEkleme(string kesimYapan, int kesimId, int kesilmisPlanSayisi, string kesimTarihi, string kesimSaati)
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
    }
}
