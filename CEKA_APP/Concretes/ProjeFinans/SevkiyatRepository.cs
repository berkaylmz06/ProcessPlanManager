using CEKA_APP.Abstracts;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class SevkiyatRepository : ISevkiyatRepository
    {
        public List<Sevkiyat> GetSevkiyatByProje(SqlConnection connection, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var result = new List<Sevkiyat>();

            string query = @"
        SELECT ps.sevkiyatId, ps.projeId, ps.paketId, psp.paketAdi, ps.sevkId, ps.tasimaBilgileri, ps.satisSipNo, 
        ps.irsaliyeNo, ps.aracSevkTarihi, ps.agirlik, ps.faturaToplami, ps.faturaNo, ps.status, ps.aracSira
 FROM ProjeFinans_Sevkiyat ps
 JOIN ProjeFinans_SevkiyatPaketleri psp ON ps.paketId = psp.paketId
 WHERE ps.projeId = @projeId
 ORDER BY ps.aracSira ASC";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Sevkiyat
                        {
                            sevkiyatId = reader.GetInt32(0),
                            projeId = reader.GetInt32(1),
                            paketId = reader.GetInt32(2),
                            paketAdi = reader.GetString(3),
                            sevkId = reader.GetString(4),
                            tasimaBilgileri = reader.IsDBNull(5) ? null : reader.GetString(5),
                            satisSiparisNo = reader.IsDBNull(6) ? null : reader.GetString(6),
                            irsaliyeNo = reader.IsDBNull(7) ? null : reader.GetString(7),
                            aracSevkTarihi = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8),
                            agirlik = reader.GetDecimal(9),
                            faturaToplami = reader.IsDBNull(10) ? (decimal?)null : reader.GetDecimal(10), 
                            faturaNo = reader.IsDBNull(11) ? null : reader.GetString(11),
                            status = reader.GetString(12),
                            aracSira = reader.GetInt32(13)
                        });
                    }
                }
            }

            return result;
        }
        public int SevkiyatKaydet(SqlConnection connection, SqlTransaction transaction, Sevkiyat sevkiyat)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
        INSERT INTO ProjeFinans_Sevkiyat 
        (projeId, sevkId, paketId, tasimaBilgileri, satisSipNo, irsaliyeNo, aracSevkTarihi, agirlik, faturaToplami, faturaNo, aracSira, status)
        OUTPUT INSERTED.sevkiyatId
        VALUES
        (@projeId, @sevkId, @paketId, @tasimaBilgileri, @satisSiparisNo, @irsaliyeNo, @aracSevkTarihi, @agirlik, @faturaToplami, @faturaNo, @aracSira, @status)";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", sevkiyat.projeId);
                cmd.Parameters.AddWithValue("@sevkId", sevkiyat.sevkId);
                cmd.Parameters.AddWithValue("@paketId", sevkiyat.paketId);
                cmd.Parameters.AddWithValue("@tasimaBilgileri", (object)sevkiyat.tasimaBilgileri ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@satisSiparisNo", (object)sevkiyat.satisSiparisNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@irsaliyeNo", (object)sevkiyat.irsaliyeNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@aracSevkTarihi", sevkiyat.aracSevkTarihi.HasValue ? (object)sevkiyat.aracSevkTarihi.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@agirlik", sevkiyat.agirlik);
                cmd.Parameters.AddWithValue("@faturaToplami", sevkiyat.faturaToplami.HasValue ? (object)sevkiyat.faturaToplami.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@faturaNo", (object)sevkiyat.faturaNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@aracSira", sevkiyat.aracSira);
                cmd.Parameters.AddWithValue("@status", sevkiyat.status);

                return (int)cmd.ExecuteScalar(); // Yeni sevkiyatId'yi döndür (IDENTITY)
            }
        }
        public void SevkiyatGuncelle(SqlConnection connection, SqlTransaction transaction, Sevkiyat sevkiyat)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
UPDATE ProjeFinans_Sevkiyat SET 
    paketId = @paketId, 
    tasimaBilgileri = @tasimaBilgileri, 
    satisSipNo = @satisSipNo, 
    irsaliyeNo = @irsaliyeNo, 
    aracSevkTarihi = @aracSevkTarihi, 
    agirlik = @agirlik, 
    faturaToplami = @faturaToplami, 
    faturaNo = @faturaNo, 
    status = @status,
    sevkId = @sevkId
WHERE projeId = @projeId AND sevkiyatId = @sevkiyatId";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", sevkiyat.projeId);
                cmd.Parameters.AddWithValue("@sevkiyatId", sevkiyat.sevkiyatId);
                cmd.Parameters.AddWithValue("@sevkId", (object)sevkiyat.sevkId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@paketId", sevkiyat.paketId);
                cmd.Parameters.AddWithValue("@tasimaBilgileri", (object)sevkiyat.tasimaBilgileri ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@satisSipNo", (object)sevkiyat.satisSiparisNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@irsaliyeNo", (object)sevkiyat.irsaliyeNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@aracSevkTarihi", sevkiyat.aracSevkTarihi.HasValue ? (object)sevkiyat.aracSevkTarihi.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@agirlik", sevkiyat.agirlik);
                cmd.Parameters.AddWithValue("@faturaToplami", sevkiyat.faturaToplami.HasValue ? (object)sevkiyat.faturaToplami.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@faturaNo", (object)sevkiyat.faturaNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", sevkiyat.status);

                cmd.ExecuteNonQuery();
            }
        }


        public bool SevkiyatSilBySevkiyatId(SqlConnection connection, SqlTransaction transaction, int projeId, int sevkiyatId, int aracSira)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
        DELETE FROM ProjeFinans_Sevkiyat 
        WHERE projeId = @projeId 
          AND sevkiyatId = @sevkiyatId 
          AND aracSira = @aracSira";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@sevkiyatId", sevkiyatId);
                cmd.Parameters.AddWithValue("@aracSira", aracSira);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool SevkiyatSil(SqlConnection connection, SqlTransaction transaction, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
        DELETE FROM ProjeFinans_Sevkiyat 
        WHERE projeId = @projeId";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}