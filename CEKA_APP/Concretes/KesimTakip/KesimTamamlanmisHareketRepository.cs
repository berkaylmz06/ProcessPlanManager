using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.DataBase;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.KesimTakip
{
    public class KesimTamamlanmisHareketRepository : IKesimTamamlanmisHareketRepository
    {
        public bool TablodanKesimTamamlanmisHareketEkleme(SqlConnection connection, SqlTransaction transaction, string kesimYapan, string kesimId, int kesilenAdet, DateTime kesimTarihi, TimeSpan kesimSaati)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string query = @"
        INSERT INTO [KesimTamamlanmisHareket] 
            ([kesimYapan], [kesimId], [kesilenAdet], [kesimTarihi], [kesimSaati])
        VALUES 
            (@kesimYapan, @kesimId, @kesilenAdet, @kesimTarihi, @kesimSaati)";

            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@kesimYapan", kesimYapan ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@kesimId", kesimId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@kesilenAdet", kesilenAdet);
                command.Parameters.AddWithValue("@kesimTarihi", kesimTarihi);
                command.Parameters.AddWithValue("@kesimSaati", kesimSaati);

                command.ExecuteNonQuery();
            }

            return true;
        }
        public DataTable GetirKesimTamamlanmisHareket(SqlConnection connection, string kesimId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            DataTable dt = new DataTable();

            string query = "SELECT * FROM KesimTamamlanmisHareket WHERE kesimId = @kesimId";

            using (var da = new SqlDataAdapter(query, connection))
            {
                da.SelectCommand.Parameters.AddWithValue("@kesimId", kesimId);
                da.Fill(dt);
            }

            return dt;
        }

    }
}
