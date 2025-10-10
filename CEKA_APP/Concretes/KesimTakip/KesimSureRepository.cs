using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Entitys.KesimTakip;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.KesimTakip
{
    public class KesimSureRepository : IKesimSureRepository
    {
        public int Baslat(SqlConnection connection, SqlTransaction transaction, string kesimId, string kesimYapan)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string selectSql = @"SELECT sureId, baslamaTarihi, toplamSureSaniye
                                 FROM KesimSure
                                 WHERE kesimId = @kesimId";

            using (SqlCommand cmd = new SqlCommand(selectSql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kesimId", kesimId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int existingId = (int)reader["sureId"];
                        reader.Close();

                        string updateSql = @"UPDATE KesimSure
                                             SET status = 'Devam Ediyor',
                                                 baslamaTarihi = GETDATE()
                                             WHERE sureId = @sureId";
                        using (SqlCommand updateCmd = new SqlCommand(updateSql, connection, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@sureId", existingId);
                            updateCmd.ExecuteNonQuery();
                        }

                        return existingId;
                    }
                }
            }

            string insertSql = @"INSERT INTO KesimSure 
                                 (kesimId, toplamSureSaniye, baslamaTarihi, status, kesimYapan)
                                 OUTPUT INSERTED.sureId
                                 VALUES (@kesimId, 0, GETDATE(), 'Devam Ediyor', @kesimYapan)";

            using (SqlCommand cmd = new SqlCommand(insertSql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kesimId", kesimId);
                cmd.Parameters.AddWithValue("@kesimYapan", kesimYapan ?? (object)DBNull.Value);
                int newId = (int)cmd.ExecuteScalar();
                return newId;
            }
        }

        public void Durdur(SqlConnection connection, SqlTransaction transaction, int sureId, int toplamSaniye)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = @"
        UPDATE KesimSure
        SET toplamSureSaniye = @toplamSureSaniye,
            status = 'Durduruldu'
        WHERE sureId = @sureId";

            using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sureId", sureId);
                cmd.Parameters.AddWithValue("@toplamSureSaniye", toplamSaniye);
                cmd.ExecuteNonQuery();
            }
        }

        public void Bitir(SqlConnection connection, SqlTransaction transaction, int sureId, int toplamSaniye)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = @"
        UPDATE KesimSure
        SET toplamSureSaniye = @toplamSureSaniye,
            bitisTarihi = GETDATE(),
            status = 'Tamamlandı'
        WHERE sureId = @sureId";

            using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sureId", sureId);
                cmd.Parameters.AddWithValue("@toplamSureSaniye", toplamSaniye);
                cmd.ExecuteNonQuery();
            }
        }
        public void Delete(SqlConnection connection, SqlTransaction transaction, int sureId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = "DELETE FROM KesimSure WHERE sureId = @sureId";
            using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sureId", sureId);
                cmd.ExecuteNonQuery();
            }
        }

        public KesimSure GetBySureId(SqlConnection connection, SqlTransaction transaction, int sureId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = "SELECT * FROM KesimSure WHERE sureId = @sureId";
            using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sureId", sureId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new KesimSure
                        {
                            sureId = (int)reader["sureId"],
                            kesimId = (string)reader["kesimId"],
                            toplamSureSaniye = (int)reader["toplamSureSaniye"],
                            baslamaTarihi = (DateTime)reader["baslamaTarihi"],
                            bitisTarihi = reader["bitisTarihi"] as DateTime?,
                            status = (string)reader["status"],
                            kesimYapan = reader["kesimYapan"] as string
                        };
                    }
                }
            }
            return null;
        }

        public void GuncelleToplamSure(SqlConnection connection, SqlTransaction transaction, int sureId, int toplamSaniye)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = "UPDATE KesimSure SET toplamSureSaniye = @toplamSureSaniye WHERE sureId = @sureId";
            using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@toplamSureSaniye", toplamSaniye);
                cmd.Parameters.AddWithValue("@sureId", sureId);
                cmd.ExecuteNonQuery();
            }
        }
        public void IptalEt(SqlConnection connection, SqlTransaction transaction, int sureId, int toplamSaniye)
        {
            string query = @"
                    UPDATE KesimSure
                    SET status = 'İptal Edildi',
                        toplamSureSaniye = @toplamSureSaniye
                    WHERE sureId = @sureId";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sureId", sureId);
                cmd.Parameters.AddWithValue("@toplamSureSaniye", toplamSaniye);
                cmd.ExecuteNonQuery();
            }
        }
        public DataTable GetirKesimHareketVeSure(SqlConnection connection, string kesimId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            DataTable dt = new DataTable();

            string query = "Select kesimYapan, kesimId, toplamSureSaniye, baslamaTarihi, bitisTarihi, status from KesimSure where kesimId = @kesimId";
            using (var da = new SqlDataAdapter(query, connection))
            {
                da.SelectCommand.Parameters.AddWithValue("@kesimId", kesimId);
                da.Fill(dt);
            }

            return dt;
        }
    }
}
