using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.DataBase;
using System;
using System.Data.SqlClient;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace CEKA_APP.Concretes.KesimTakip
{
    public class IdUreticiRepository: IIdUreticiRepository
    {
        public int GetSiradakiId(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            string query = @"
        SELECT COALESCE(SonId, 0) + 1 
        FROM IdUretici 
        WHERE TabloAdi = 'KesimListesi'";

            using (var cmd = new SqlCommand(query, connection))
            {
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public bool SiradakiIdKaydet(SqlConnection connection, SqlTransaction transaction, int id)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string query = @"
IF EXISTS (SELECT 1 FROM IdUretici WHERE TabloAdi = 'KesimListesi')
BEGIN
    UPDATE IdUretici SET SonId = @id WHERE TabloAdi = 'KesimListesi'
END
ELSE
BEGIN
    INSERT INTO IdUretici (TabloAdi, SonId) VALUES ('KesimListesi', @id)
END";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                return true;
            }
        }
    }
}
