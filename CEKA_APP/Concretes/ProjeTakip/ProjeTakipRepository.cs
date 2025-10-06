using CEKA_APP.Abstracts.ProjeTakip;
using CEKA_APP.Entitys.ProjeTakip;
using System;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.ProjeTakip
{
    public class ProjeTakipRepository : IProjeTakipRepository
    {
        public bool ProjeKartiEkle(SqlConnection connection, SqlTransaction transaction, ProjeKarti projeKarti)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string projeNo = projeKarti.projeNo.Trim();
            string kok = projeNo.Split('.')[0].Trim();

            string checkSql = @"
        SELECT COUNT(*) 
        FROM ProjeTakip_ProjeKarti 
        WHERE LEFT(TRIM(projeNo), 5) = @kok";

            using (SqlCommand checkCommand = new SqlCommand(checkSql, connection, transaction))
            {
                checkCommand.Parameters.AddWithValue("@kok", kok);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    return false;
                }
            }

            string sql = @"
        INSERT INTO ProjeTakip_ProjeKarti 
        (projeId, projeNo, projeAdi, projeBasTarihi, projeBitisTarihi, musteriNo, musteriAdi, projeMuhendisi, refProjeVarMi, refProje)
        VALUES 
        (@projeId, @projeNo, @projeAdi, @projeBasTarihi, @projeBitisTarihi, @musteriNo, @musteriAdi, @projeMuhendisi, @refProjeVarMi, @refProje)";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@projeKartId", projeKarti.projeKartId);
                command.Parameters.AddWithValue("@projeId", projeKarti.projeId);
                command.Parameters.AddWithValue("@projeNo", projeKarti.projeNo);
                command.Parameters.AddWithValue("@projeAdi", projeKarti.projeAdi);
                command.Parameters.AddWithValue("@projeBasTarihi", projeKarti.projeBasTarihi);
                command.Parameters.AddWithValue("@projeBitisTarihi", projeKarti.projeBitisTarihi);
                command.Parameters.AddWithValue("@musteriNo", projeKarti.musteriNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@musteriAdi", projeKarti.musteriAdi ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@projeMuhendisi", projeKarti.projeMuhendisi ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@refProjeVarMi", projeKarti.refProjeVarMi);
                command.Parameters.AddWithValue("@refProje", string.IsNullOrEmpty(projeKarti.refProje) ? (object)DBNull.Value : projeKarti.refProje);

                command.ExecuteNonQuery();
            }

            return true;
        }
        public ProjeKarti ProjeKartiAra(SqlConnection connection, int projeKartId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string sql = @"
        SELECT 
            projeId, projeNo, projeAdi, projeBasTarihi, projeBitisTarihi, musteriNo, musteriAdi, projeMuhendisi, refProjeVarMi, refProje FROM ProjeTakip_ProjeKarti
        WHERE projeKartId = @projeKartId";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@projeKartId", projeKartId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var projeTakip = new ProjeKarti
                        {
                            projeId = reader.GetInt32(0),
                            projeNo = reader.IsDBNull(1) ? null : reader.GetString(1),
                            projeAdi = reader.IsDBNull(2) ? null : reader.GetString(2),
                            projeBasTarihi = reader.IsDBNull(7) ? DateTime.Now : reader.GetDateTime(7),
                            projeBitisTarihi = reader.IsDBNull(7) ? DateTime.Now : reader.GetDateTime(7),
                            musteriNo = reader.IsDBNull(3) ? null : reader.GetString(3),
                            musteriAdi = reader.IsDBNull(4) ? null : reader.GetString(4),
                            projeMuhendisi = reader.IsDBNull(4) ? null : reader.GetString(4),
                            refProjeVarMi = reader.IsDBNull(5) ? false : reader.GetBoolean(5),
                            refProje = reader.IsDBNull(6) ? "0" : reader.GetString(6),
                        };

                        reader.Close();

                        return projeTakip;
                    }
                }
            }

            return null;
        }
        public bool ProjeKartiSil(SqlConnection connection, SqlTransaction transaction, int projeKartId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = "DELETE FROM ProjeTakip_ProjeKarti WHERE projeKartId = @projeKartId";
            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@projeKartId", projeKartId);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
