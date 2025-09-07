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
        public List<Sevkiyat> GetSevkiyatByProje(int projeId)
        {
            var result = new List<Sevkiyat>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"SELECT ps.projeId, ps.paketId, psp.paketAdi, ps.sevkId, ps.tasimaBilgileri, ps.satisSipNo, ps.irsaliyeNo, ps.aracSevkTarihi, ps.agirlik, ps.faturaToplami, ps.faturaNo, ps.status, ps.aracSira
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
                                projeId = reader.GetInt32(0),
                                paketId = reader.GetInt32(1),
                                paketAdi = reader.GetString(2),
                                sevkId = reader.GetString(3),
                                tasimaBilgileri = reader.IsDBNull(4) ? null : reader.GetString(4),
                                satisSiparisNo = reader.IsDBNull(5) ? null : reader.GetString(5),
                                irsaliyeNo = reader.IsDBNull(6) ? null : reader.GetString(6),
                                aracSevkTarihi = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                agirlik = reader.GetDecimal(8),
                                faturaToplami = reader.IsDBNull(9) ? (decimal?)null : reader.GetDecimal(9),
                                faturaNo = reader.IsDBNull(10) ? null : reader.GetString(10),
                                status = reader.GetString(11),
                                aracSira = reader.GetInt32(12)
                            });
                        }
                    }
                }
            }
            return result;
        }

        public void SevkiyatKaydet(Sevkiyat sevkiyat, SqlTransaction transaction)
        {
            var connection = transaction.Connection;
            string query = @"INSERT INTO ProjeFinans_Sevkiyat 
                             (projeId, sevkId, paketId, tasimaBilgileri, satisSipNo, irsaliyeNo, aracSevkTarihi, agirlik, faturaToplami, faturaNo, aracSira, status)
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
                cmd.ExecuteNonQuery();
            }
        }

        public void SevkiyatGuncelle(Sevkiyat sevkiyat, SqlTransaction transaction)
        {
            var connection = transaction.Connection;
            string query = @"UPDATE ProjeFinans_Sevkiyat SET 
                                paketId = @paketId, 
                                tasimaBilgileri = @tasimaBilgileri, 
                                satisSipNo = @satisSipNo, 
                                irsaliyeNo = @irsaliyeNo, 
                                aracSevkTarihi = @aracSevkTarihi, 
                                agirlik = @agirlik, 
                                faturaToplami = @faturaToplami, 
                                faturaNo = @faturaNo, 
                                status = @status
                             WHERE projeId = @projeId AND sevkId = @sevkId AND aracSira = @aracSira";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", sevkiyat.projeId);
                cmd.Parameters.AddWithValue("@sevkId", sevkiyat.sevkId);
                cmd.Parameters.AddWithValue("@aracSira", sevkiyat.aracSira);
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

        public bool SevkiyatSilBySevkiyatId(int projeId, string sevkiyatId, int aracSira, SqlTransaction transaction)
        {
            var connection = transaction.Connection;
            string query = "DELETE FROM ProjeFinans_Sevkiyat WHERE projeId = @projeId AND sevkId = @sevkId AND aracSira = @aracSira";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@sevkId", sevkiyatId.Trim());
                cmd.Parameters.AddWithValue("@aracSira", aracSira);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool SevkiyatSil(int projeId, SqlTransaction transaction)
        {
            var connection = transaction.Connection;
            string query = "DELETE FROM ProjeFinans_Sevkiyat WHERE projeId = @projeId";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}