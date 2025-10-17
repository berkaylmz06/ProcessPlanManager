using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Entitys.KesimTakip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.KesimTakip
{
    public class KesimSureRepository : IKesimSureRepository
    {
        public int Baslat(SqlConnection connection, SqlTransaction transaction, string kesimId, string kesimYapan, string lotNo)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (string.IsNullOrEmpty(kesimId)) throw new ArgumentNullException(nameof(kesimId));
            if (string.IsNullOrEmpty(lotNo)) throw new ArgumentNullException(nameof(lotNo));

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
                                         baslamaTarihi = GETDATE(),
                                         lotNo = @lotNo
                                     WHERE sureId = @sureId";

                        using (SqlCommand updateCmd = new SqlCommand(updateSql, connection, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@sureId", existingId);
                            updateCmd.Parameters.AddWithValue("@lotNo", lotNo);
                            updateCmd.ExecuteNonQuery();
                        }

                        return existingId;
                    }
                }
            }

            string insertSql = @"INSERT INTO KesimSure 
                         (kesimId, toplamSureSaniye, baslamaTarihi, status, kesimYapan, lotNo)
                         OUTPUT INSERTED.sureId
                         VALUES (@kesimId, 0, GETDATE(), 'Devam Ediyor', @kesimYapan, @lotNo)";

            using (SqlCommand cmd = new SqlCommand(insertSql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kesimId", kesimId);
                cmd.Parameters.AddWithValue("@kesimYapan", kesimYapan ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@lotNo", lotNo);
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
                            kesimYapan = reader["kesimYapan"] as string,
                            lotNo = reader["lotNo"] as string
                        };
                    }
                }
            }
            return null;
        }
        public int GetirSureId(SqlConnection connection, SqlTransaction transaction, string kesimId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = @"
                SELECT TOP 1 sureId 
                FROM KesimSure 
                WHERE kesimId = @kesimId 
                  AND status IN ('Devam Ediyor')";

            using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kesimId", kesimId);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return (int)result;
                }
            }

            return 0;
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

            string query = "Select kesimYapan, kesimId, toplamSureSaniye, baslamaTarihi, bitisTarihi, lotNo, status from KesimSure where kesimId = @kesimId";
            using (var da = new SqlDataAdapter(query, connection))
            {
                da.SelectCommand.Parameters.AddWithValue("@kesimId", kesimId);
                da.Fill(dt);
            }

            return dt;
        }
        public List<(string KesimId, string LotNo, int En, int Boy, int ToplamSureSaniye, string KesimYapan)> GetirDevamEdenKesimler(SqlConnection connection, SqlTransaction transaction)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var devamEdenKesimler = new List<(string KesimId, string LotNo, int En, int Boy, int ToplamSureSaniye, string KesimYapan)>();

            string sql = @"
    SELECT
        kp.kesimId,
        ks.lotNo,
        kp.En,
        kp.Boy,
        ks.toplamSureSaniye,
        ks.kesimYapan
    FROM KesimSure ks
    INNER JOIN KesimListesiPaket kp ON ks.kesimId = kp.kesimId
    WHERE ks.Status IN ('Devam Ediyor')";

            using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string kesimId = reader["kesimId"].ToString();
                        string lotNo = reader["lotNo"].ToString();
                        int en = reader["En"] != DBNull.Value ? Convert.ToInt32(reader["En"]) : 0;
                        int boy = reader["Boy"] != DBNull.Value ? Convert.ToInt32(reader["Boy"]) : 0;
                        int toplamSureSaniye = reader["toplamSureSaniye"] != DBNull.Value ? Convert.ToInt32(reader["toplamSureSaniye"]) : 0;
                        string kesimYapan = reader["kesimYapan"].ToString();

                        devamEdenKesimler.Add((kesimId, lotNo, en, boy, toplamSureSaniye, kesimYapan));
                    }
                }
            }
            return devamEdenKesimler;
        }
    }
}
