using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class OdemeSartlariRepository : IOdemeSartlariRepository
    {
        private readonly IOdemeHareketleriRepository _odemeHareketleriRepo;

        public OdemeSartlariRepository(IOdemeHareketleriRepository odemeHareketleriRepo)
        {
            _odemeHareketleriRepo = odemeHareketleriRepo ?? throw new ArgumentNullException(nameof(odemeHareketleriRepo));
        }
        public void SaveOrUpdateOdemeBilgi(SqlConnection connection, SqlTransaction transaction, OdemeSartlari odemeSartlari)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string checkQuery = "SELECT odemeTarihi FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
            DateTime? existingOdemeTarihi = null;
            using (var checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.Add("@projeId", SqlDbType.Int).Value = odemeSartlari.projeId;
                checkCmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = odemeSartlari.kilometreTasiId;
                var result = checkCmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    existingOdemeTarihi = (DateTime)result;
            }

            bool updateOdemeTarihi = odemeSartlari.odemeTarihi.HasValue;
            string query;

            if (existingOdemeTarihi != null || !string.IsNullOrWhiteSpace(odemeSartlari.faturaNo))
            {
                query = @"
        UPDATE ProjeFinans_OdemeSartlari
        SET siralama = @siralama,
            oran = @oran,
            tutar = @tutar,
            tahminiTarih = @tahminiTarih,
            gerceklesenTarih = @gerceklesenTarih,
            aciklama = @aciklama,
            teminatMektubu = @teminatMektubu,
            teminatDurumu = @teminatDurumu,
            faturaNo = @faturaNo,
            durum = @durum,
            kalanTutar = @kalanTutar,
            status = @status";

                if (updateOdemeTarihi)
                    query += ", odemeTarihi = @odemeTarihi";

                query += " WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
            }
            else
            {
                string countQuery = "SELECT COUNT(*) FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
                int recordCount = 0;
                using (var countCmd = new SqlCommand(countQuery, connection, transaction))
                {
                    countCmd.Parameters.Add("@projeId", SqlDbType.Int).Value = odemeSartlari.projeId;
                    countCmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = odemeSartlari.kilometreTasiId;
                    recordCount = (int)countCmd.ExecuteScalar();
                }

                if (recordCount > 0)
                {
                    query = @"
            UPDATE ProjeFinans_OdemeSartlari
            SET siralama = @siralama,
                oran = @oran,
                tutar = @tutar,
                tahminiTarih = @tahminiTarih,
                gerceklesenTarih = @gerceklesenTarih,
                aciklama = @aciklama,
                teminatMektubu = @teminatMektubu,
                teminatDurumu = @teminatDurumu,
                faturaNo = @faturaNo,
                durum = @durum,
                kalanTutar = @kalanTutar,
                status = @status";

                    if (updateOdemeTarihi)
                        query += ", odemeTarihi = @odemeTarihi";

                    query += " WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
                }
                else
                {
                    query = @"
            INSERT INTO ProjeFinans_OdemeSartlari
            (projeId, kilometreTasiId, siralama, oran, tutar, tahminiTarih, gerceklesenTarih, aciklama, teminatMektubu, teminatDurumu, faturaNo, durum, kalanTutar, odemeTarihi, status)
            VALUES (@projeId, @kilometreTasiId, @siralama, @oran, @tutar, @tahminiTarih, @gerceklesenTarih, @aciklama, @teminatMektubu, @teminatDurumu, @faturaNo, @durum, @kalanTutar, @odemeTarihi, @status)";
                }
            }

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.Add("@projeId", SqlDbType.Int).Value = odemeSartlari.projeId;
                cmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = odemeSartlari.kilometreTasiId;
                cmd.Parameters.Add("@siralama", SqlDbType.Int).Value = odemeSartlari.siralama;
                cmd.Parameters.Add("@oran", SqlDbType.Decimal).Value = odemeSartlari.oran;
                cmd.Parameters.Add("@tutar", SqlDbType.Decimal).Value = odemeSartlari.tutar;
                cmd.Parameters.Add("@tahminiTarih", SqlDbType.DateTime2).Value = odemeSartlari.tahminiTarih ?? (object)DBNull.Value;
                cmd.Parameters.Add("@gerceklesenTarih", SqlDbType.DateTime2).Value = odemeSartlari.gerceklesenTarih ?? (object)DBNull.Value;
                cmd.Parameters.Add("@aciklama", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(odemeSartlari.odemeAciklama) ? (object)DBNull.Value : odemeSartlari.odemeAciklama;
                cmd.Parameters.Add("@teminatMektubu", SqlDbType. NVarChar, 50).Value = string.IsNullOrWhiteSpace(odemeSartlari.teminatMektubu) ? (object)DBNull.Value : odemeSartlari.teminatMektubu;
                cmd.Parameters.Add("@teminatDurumu", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(odemeSartlari.teminatDurumu) ? (object)DBNull.Value : odemeSartlari.teminatDurumu;
                cmd.Parameters.Add("@faturaNo", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(odemeSartlari.faturaNo) ? (object)DBNull.Value : odemeSartlari.faturaNo;
                cmd.Parameters.Add("@durum", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(odemeSartlari.durum) ? (object)DBNull.Value : odemeSartlari.durum;
                cmd.Parameters.Add("@kalanTutar", SqlDbType.Decimal).Value = odemeSartlari.kalanTutar;
                cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(odemeSartlari.status) ? "Başlatıldı" : odemeSartlari.status;

                if (query.Contains("@odemeTarihi"))
                    cmd.Parameters.Add("@odemeTarihi", SqlDbType.DateTime2).Value = odemeSartlari.odemeTarihi ?? (object)DBNull.Value;

                cmd.ExecuteNonQuery();
            }
        }
        public string GetFaturaNo(SqlConnection connection, int projeId, int kilometreTasiId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = "SELECT faturaNo FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@kilometreTasiId", kilometreTasiId);
                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? result.ToString() : null;
            }
        }
        public string GetOdemeBilgileriQuery()
        {
            return @"
 SELECT 
    o.odemeId,
    o.projeId,
    pro.projeNo,
	CASE 
    WHEN pi.ustProjeId IS NOT NULL AND pUst.musteriNo IS NOT NULL 
        THEN pUst.musteriNo 
    ELSE p.musteriNo 
END AS musteriNo,
    CASE 
    WHEN pi.ustProjeId IS NOT NULL AND pUst.musteriAdi IS NOT NULL 
        THEN pUst.musteriAdi 
    ELSE p.musteriAdi 
END AS musteriAdi,
    pro.aciklama AS projeAciklama,
    o.kilometreTasiId,
    k.kilometreTasiAdi,
    o.siralama,
    o.oran,
    o.tutar,
    CASE 
        WHEN pi.ustProjeId IS NOT NULL AND pUst.paraBirimi IS NOT NULL 
            THEN pUst.paraBirimi 
        ELSE p.paraBirimi 
    END AS paraBirimi,
    o.tahminiTarih,
    o.gerceklesenTarih,
    o.aciklama AS odemeAciklama,
    o.teminatMektubu,
    o.teminatDurumu,
    o.durum,
    o.faturaNo,
    o.kalanTutar,
    o.odemeTarihi,
	DATEDIFF(DAY, o.gerceklesenTarih, o.odemeTarihi) AS odemeSapmasi
FROM ProjeFinans_OdemeSartlari o
    LEFT JOIN ProjeFinans_KilometreTaslari k ON o.kilometreTasiId = k.kilometreTasiId
    LEFT JOIN ProjeFinans_ProjeKutuk p ON o.projeId = p.projeId
    LEFT JOIN ProjeFinans_Projeler pro ON o.projeId = pro.projeId
    LEFT JOIN ProjeFinans_ProjeIliski pi ON o.projeId = pi.altProjeId
    LEFT JOIN ProjeFinans_ProjeKutuk pUst ON pi.ustProjeId = pUst.projeId
";
        }

        public List<OdemeSartlari> GetOdemeBilgileri(SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var odemeBilgileriList = new List<OdemeSartlari>();

            using (var cmd = new SqlCommand(GetOdemeBilgileriQuery(), connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var odemeSartlari = new OdemeSartlari
                        {
                            odemeId = reader.GetInt32(reader.GetOrdinal("odemeId")),
                            projeId = reader.GetInt32(reader.GetOrdinal("projeId")),
                            projeNo = reader.IsDBNull(reader.GetOrdinal("projeNo")) ? null : reader.GetString(reader.GetOrdinal("projeNo")),
                            musteriNo = reader.IsDBNull(reader.GetOrdinal("musteriNo")) ? null : reader.GetString(reader.GetOrdinal("musteriNo")),
                            musteriAdi = reader.IsDBNull(reader.GetOrdinal("musteriAdi")) ? null : reader.GetString(reader.GetOrdinal("musteriAdi")),
                            projeAciklama = reader.IsDBNull(reader.GetOrdinal("projeAciklama")) ? null : reader.GetString(reader.GetOrdinal("projeAciklama")),
                            kilometreTasiId = reader.GetInt32(reader.GetOrdinal("kilometreTasiId")),
                            kilometreTasiAdi = reader.IsDBNull(reader.GetOrdinal("kilometreTasiAdi")) ? null : reader.GetString(reader.GetOrdinal("kilometreTasiAdi")),
                            siralama = reader.GetInt32(reader.GetOrdinal("siralama")),
                            oran = reader.GetDecimal(reader.GetOrdinal("oran")),
                            tutar = reader.GetDecimal(reader.GetOrdinal("tutar")),
                            paraBirimi = reader.IsDBNull(reader.GetOrdinal("paraBirimi")) ? null : reader.GetString(reader.GetOrdinal("paraBirimi")),
                            tahminiTarih = reader.IsDBNull(reader.GetOrdinal("tahminiTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("tahminiTarih")),
                            gerceklesenTarih = reader.IsDBNull(reader.GetOrdinal("gerceklesenTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gerceklesenTarih")),
                            odemeAciklama = reader.IsDBNull(reader.GetOrdinal("odemeAciklama")) ? null : reader.GetString(reader.GetOrdinal("odemeAciklama")),
                            teminatMektubu = reader.IsDBNull(reader.GetOrdinal("teminatMektubu")) ? null : reader.GetString(reader.GetOrdinal("teminatMektubu")),
                            teminatDurumu = reader.IsDBNull(reader.GetOrdinal("teminatDurumu")) ? null : reader.GetString(reader.GetOrdinal("teminatDurumu")),
                            durum = reader.IsDBNull(reader.GetOrdinal("durum")) ? null : reader.GetString(reader.GetOrdinal("durum")),
                            faturaNo = reader.IsDBNull(reader.GetOrdinal("faturaNo")) ? null : reader.GetString(reader.GetOrdinal("faturaNo")),
                            kalanTutar = reader.GetDecimal(reader.GetOrdinal("kalanTutar")),
                            odemeTarihi = reader.IsDBNull(reader.GetOrdinal("odemeTarihi")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("odemeTarihi")),
                            odemeSapmasi = reader.IsDBNull(reader.GetOrdinal("odemeSapmasi")) ? 0 : reader.GetInt32(reader.GetOrdinal("odemeSapmasi"))
                        };

                        odemeBilgileriList.Add(odemeSartlari);
                    }
                }
            }

            return odemeBilgileriList;
        }

        public List<OdemeSartlari> GetOdemeBilgileriByProjeId(SqlConnection connection, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var odemeBilgileriList = new List<OdemeSartlari>();
            string query = @"
        SELECT
            o.odemeId,
            o.projeId,
            o.kilometreTasiId,
            k.kilometreTasiAdi AS kilometreTasiAdi,
            o.siralama,
            o.oran,
            o.tutar,
            o.tahminiTarih,
            o.gerceklesenTarih,
            o.aciklama,
            o.teminatMektubu,
            o.teminatDurumu,
            o.durum,
            o.faturaNo,
            o.kalanTutar,
            o.odemeTarihi
        FROM ProjeFinans_OdemeSartlari o
        JOIN ProjeFinans_KilometreTaslari k ON o.kilometreTasiId = k.kilometreTasiId
        WHERE o.projeId = @projeId
        ORDER BY o.siralama";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var odemeSartlari = new OdemeSartlari
                        {
                            odemeId = reader.GetInt32(reader.GetOrdinal("odemeId")),
                            projeId = reader.GetInt32(reader.GetOrdinal("projeId")),
                            kilometreTasiId = reader.GetInt32(reader.GetOrdinal("kilometreTasiId")),
                            kilometreTasiAdi = reader.IsDBNull(reader.GetOrdinal("kilometreTasiAdi")) ? null : reader.GetString(reader.GetOrdinal("kilometreTasiAdi")),
                            siralama = reader.GetInt32(reader.GetOrdinal("siralama")),
                            oran = reader.GetDecimal(reader.GetOrdinal("oran")),
                            tutar = reader.GetDecimal(reader.GetOrdinal("tutar")),
                            tahminiTarih = reader.IsDBNull(reader.GetOrdinal("tahminiTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("tahminiTarih")),
                            gerceklesenTarih = reader.IsDBNull(reader.GetOrdinal("gerceklesenTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gerceklesenTarih")),
                            odemeAciklama = reader.IsDBNull(reader.GetOrdinal("aciklama")) ? null : reader.GetString(reader.GetOrdinal("aciklama")),
                            teminatMektubu = reader.IsDBNull(reader.GetOrdinal("teminatMektubu")) ? null : reader.GetString(reader.GetOrdinal("teminatMektubu")),
                            teminatDurumu = reader.IsDBNull(reader.GetOrdinal("teminatDurumu")) ? null : reader.GetString(reader.GetOrdinal("teminatDurumu")),
                            durum = reader.IsDBNull(reader.GetOrdinal("durum")) ? null : reader.GetString(reader.GetOrdinal("durum")),
                            faturaNo = reader.IsDBNull(reader.GetOrdinal("faturaNo")) ? null : reader.GetString(reader.GetOrdinal("faturaNo")),
                            kalanTutar = reader.GetDecimal(reader.GetOrdinal("kalanTutar")),
                            odemeTarihi = reader.IsDBNull(reader.GetOrdinal("odemeTarihi")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("odemeTarihi")),
                        };

                        odemeBilgileriList.Add(odemeSartlari);
                    }
                }
            }

            return odemeBilgileriList;
        }


        public OdemeSartlari GetOdemeBilgi(SqlConnection connection, string projeNo, int kilometreTasiId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = @"
        SELECT
            o.odemeId,
            o.projeId,
            o.kilometreTasiId,
            k.kilometreTasiAdi AS kilometreTasiAdi,
            o.siralama,
            o.oran,
            o.tutar,
            o.tahminiTarih,
            o.gerceklesenTarih,
            o.aciklama,
            o.teminatMektubu,
            o.teminatDurumu,
            o.durum,
            o.faturaNo,
            o.kalanTutar,
            o.odemeTarihi
        FROM ProjeFinans_OdemeSartlari o
        JOIN ProjeFinans_KilometreTaslari k ON o.kilometreTasiId = k.kilometreTasiId
        JOIN ProjeFinans_Projeler p ON o.projeId = p.projeId
        WHERE p.projeNo = @projeNo AND o.kilometreTasiId = @kilometreTasiId";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@projeNo", projeNo);
                cmd.Parameters.AddWithValue("@kilometreTasiId", kilometreTasiId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OdemeSartlari
                        {
                            odemeId = reader.GetInt32(reader.GetOrdinal("odemeId")),
                            projeId = reader.GetInt32(reader.GetOrdinal("projeId")),
                            kilometreTasiId = reader.GetInt32(reader.GetOrdinal("kilometreTasiId")),
                            kilometreTasiAdi = reader.IsDBNull(reader.GetOrdinal("kilometreTasiAdi")) ? null : reader.GetString(reader.GetOrdinal("kilometreTasiAdi")),
                            siralama = reader.GetInt32(reader.GetOrdinal("siralama")),
                            oran = reader.GetDecimal(reader.GetOrdinal("oran")),
                            tutar = reader.GetDecimal(reader.GetOrdinal("tutar")),
                            tahminiTarih = reader.IsDBNull(reader.GetOrdinal("tahminiTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("tahminiTarih")),
                            gerceklesenTarih = reader.IsDBNull(reader.GetOrdinal("gerceklesenTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gerceklesenTarih")),
                            odemeAciklama = reader.IsDBNull(reader.GetOrdinal("aciklama")) ? null : reader.GetString(reader.GetOrdinal("aciklama")),
                            teminatMektubu = reader.IsDBNull(reader.GetOrdinal("teminatMektubu")) ? null : reader.GetString(reader.GetOrdinal("teminatMektubu")),
                            teminatDurumu = reader.IsDBNull(reader.GetOrdinal("teminatDurumu")) ? null : reader.GetString(reader.GetOrdinal("teminatDurumu")),
                            durum = reader.IsDBNull(reader.GetOrdinal("durum")) ? null : reader.GetString(reader.GetOrdinal("durum")),
                            faturaNo = reader.IsDBNull(reader.GetOrdinal("faturaNo")) ? null : reader.GetString(reader.GetOrdinal("faturaNo")),
                            kalanTutar = reader.GetDecimal(reader.GetOrdinal("kalanTutar")),
                            odemeTarihi = reader.IsDBNull(reader.GetOrdinal("odemeTarihi")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("odemeTarihi")),
                        };
                    }
                }
            }

            return null;
        }


        public OdemeSartlari GetOdemeBilgiByOdemeId(SqlConnection connection, int odemeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = @"
        SELECT
            odemeId,
            projeId,
            kilometreTasiId,
            siralama,
            oran,
            tutar,
            tahminiTarih,
            gerceklesenTarih,
            aciklama,
            teminatMektubu,
            teminatDurumu,
            durum,
            faturaNo,
            kalanTutar,
            odemeTarihi
        FROM ProjeFinans_OdemeSartlari
        WHERE odemeId = @odemeId";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@odemeId", odemeId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OdemeSartlari
                        {
                            odemeId = reader.GetInt32(reader.GetOrdinal("odemeId")),
                            projeId = reader.GetInt32(reader.GetOrdinal("projeId")),
                            kilometreTasiId = reader.GetInt32(reader.GetOrdinal("kilometreTasiId")),
                            siralama = reader.GetInt32(reader.GetOrdinal("siralama")),
                            oran = reader.GetDecimal(reader.GetOrdinal("oran")),
                            tutar = reader.GetDecimal(reader.GetOrdinal("tutar")),
                            tahminiTarih = reader.IsDBNull(reader.GetOrdinal("tahminiTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("tahminiTarih")),
                            gerceklesenTarih = reader.IsDBNull(reader.GetOrdinal("gerceklesenTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gerceklesenTarih")),
                            odemeAciklama = reader.IsDBNull(reader.GetOrdinal("aciklama")) ? null : reader.GetString(reader.GetOrdinal("aciklama")),
                            teminatMektubu = reader.IsDBNull(reader.GetOrdinal("teminatMektubu")) ? null : reader.GetString(reader.GetOrdinal("teminatMektubu")),
                            teminatDurumu = reader.IsDBNull(reader.GetOrdinal("teminatDurumu")) ? null : reader.GetString(reader.GetOrdinal("teminatDurumu")),
                            durum = reader.IsDBNull(reader.GetOrdinal("durum")) ? null : reader.GetString(reader.GetOrdinal("durum")),
                            faturaNo = reader.IsDBNull(reader.GetOrdinal("faturaNo")) ? null : reader.GetString(reader.GetOrdinal("faturaNo")),
                            kalanTutar = reader.GetDecimal(reader.GetOrdinal("kalanTutar")),
                            odemeTarihi = reader.IsDBNull(reader.GetOrdinal("odemeTarihi")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("odemeTarihi")),
                        };
                    }
                }
            }

            return null;
        }
        public void DeleteOdemeBilgi(SqlConnection connection, SqlTransaction transaction, int projeId, int kilometreTasiId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

            int? odemeId = null;

            string selectQuery = "SELECT odemeId FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
            using (var selectCmd = new SqlCommand(selectQuery, connection, transaction))
            {
                selectCmd.Parameters.AddWithValue("@projeId", projeId);
                selectCmd.Parameters.AddWithValue("@kilometreTasiId", kilometreTasiId);
                var result = selectCmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    odemeId = Convert.ToInt32(result);
                }
            }

            if (odemeId.HasValue)
            {
                _odemeHareketleriRepo.DeleteOdemeHareketleriByOdemeIds(connection, transaction, new List<int> { odemeId.Value });
            }

            string deleteQuery = "DELETE FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
            using (var deleteCmd = new SqlCommand(deleteQuery, connection, transaction))
            {
                deleteCmd.Parameters.AddWithValue("@projeId", projeId);
                deleteCmd.Parameters.AddWithValue("@kilometreTasiId", kilometreTasiId);
                deleteCmd.ExecuteNonQuery();
            }
        }

        public bool UpdateFaturaNo(SqlConnection connection, SqlTransaction transaction, int odemeId, string faturaNo)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

            string query = "UPDATE ProjeFinans_OdemeSartlari SET faturaNo = @faturaNo WHERE odemeId = @odemeId";
            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@odemeId", odemeId);
                cmd.Parameters.AddWithValue("@faturaNo", string.IsNullOrEmpty(faturaNo) ? (object)DBNull.Value : faturaNo);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public bool OdemeSartlariSil(SqlConnection connection, SqlTransaction transaction, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (_odemeHareketleriRepo == null) throw new InvalidOperationException("_odemeHareketleriRepo başlatılmamış.");

            List<int> odemeIds = new List<int>();
            string selectQuery = "SELECT odemeId FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId";
            using (var selectCmd = new SqlCommand(selectQuery, connection, transaction))
            {
                selectCmd.Parameters.AddWithValue("@projeId", projeId);
                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                        odemeIds.Add(reader.GetInt32(0));
                }
            }

            _odemeHareketleriRepo.DeleteOdemeHareketleriByOdemeIds(connection, transaction, odemeIds);

            string deleteQuery = "DELETE FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId";
            using (var deleteCmd = new SqlCommand(deleteQuery, connection, transaction))
            {
                deleteCmd.Parameters.AddWithValue("@projeId", projeId);
                deleteCmd.ExecuteNonQuery();
            }

            return true;
        }
        public List<OdemeSartlari> GetAllOdemeBilgileri(SqlConnection connection, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var odemeBilgileriList = new List<OdemeSartlari>();
            int? ustProjeId = null;

            string checkUstProjeQuery = @"
                SELECT ustProjeId 
                FROM ProjeFinans_ProjeIliski 
                WHERE altProjeId = @projeId";
            using (var checkCmd = new SqlCommand(checkUstProjeQuery, connection))
            {
                checkCmd.Parameters.AddWithValue("@projeId", projeId);
                var result = checkCmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    ustProjeId = Convert.ToInt32(result); 
                }
                else
                {
                    ustProjeId = projeId;
                }
            }

            var projeIds = new List<int> { ustProjeId.Value };
            string getAltProjelerQuery = @"
                SELECT altProjeId 
                FROM ProjeFinans_ProjeIliski 
                WHERE ustProjeId = @ustProjeId";
            using (var altProjelerCmd = new SqlCommand(getAltProjelerQuery, connection))
            {
                altProjelerCmd.Parameters.AddWithValue("@ustProjeId", ustProjeId);
                using (var reader = altProjelerCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        projeIds.Add(reader.GetInt32(0));
                    }
                }
            }

            string query = @"
                SELECT
                    o.odemeId,
                    o.projeId,
                    o.kilometreTasiId,
                    k.kilometreTasiAdi AS kilometreTasiAdi,
                    o.siralama,
                    o.oran,
                    o.tutar,
                    o.tahminiTarih,
                    o.gerceklesenTarih,
                    o.aciklama,
                    o.teminatMektubu,
                    o.teminatDurumu,
                    o.durum,
                    o.faturaNo,
                    o.kalanTutar,
                    o.odemeTarihi
                FROM ProjeFinans_OdemeSartlari o
                JOIN ProjeFinans_KilometreTaslari k ON o.kilometreTasiId = k.kilometreTasiId
                WHERE o.projeId IN (" + string.Join(",", projeIds) + @")
                ORDER BY o.projeId, o.siralama";

            using (var cmd = new SqlCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var odemeSartlari = new OdemeSartlari
                        {
                            odemeId = reader.GetInt32(reader.GetOrdinal("odemeId")),
                            projeId = reader.GetInt32(reader.GetOrdinal("projeId")),
                            kilometreTasiId = reader.GetInt32(reader.GetOrdinal("kilometreTasiId")),
                            kilometreTasiAdi = reader.IsDBNull(reader.GetOrdinal("kilometreTasiAdi")) ? null : reader.GetString(reader.GetOrdinal("kilometreTasiAdi")),
                            siralama = reader.GetInt32(reader.GetOrdinal("siralama")),
                            oran = reader.GetDecimal(reader.GetOrdinal("oran")),
                            tutar = reader.GetDecimal(reader.GetOrdinal("tutar")),
                            tahminiTarih = reader.IsDBNull(reader.GetOrdinal("tahminiTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("tahminiTarih")),
                            gerceklesenTarih = reader.IsDBNull(reader.GetOrdinal("gerceklesenTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gerceklesenTarih")),
                            odemeAciklama = reader.IsDBNull(reader.GetOrdinal("aciklama")) ? null : reader.GetString(reader.GetOrdinal("aciklama")),
                            teminatMektubu = reader.IsDBNull(reader.GetOrdinal("teminatMektubu")) ? null : reader.GetString(reader.GetOrdinal("teminatMektubu")),
                            teminatDurumu = reader.IsDBNull(reader.GetOrdinal("teminatDurumu")) ? null : reader.GetString(reader.GetOrdinal("teminatDurumu")),
                            durum = reader.IsDBNull(reader.GetOrdinal("durum")) ? null : reader.GetString(reader.GetOrdinal("durum")),
                            faturaNo = reader.IsDBNull(reader.GetOrdinal("faturaNo")) ? null : reader.GetString(reader.GetOrdinal("faturaNo")),
                            kalanTutar = reader.GetDecimal(reader.GetOrdinal("kalanTutar")),
                            odemeTarihi = reader.IsDBNull(reader.GetOrdinal("odemeTarihi")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("odemeTarihi"))
                        };
                        odemeBilgileriList.Add(odemeSartlari);
                    }
                }
            }

            return odemeBilgileriList;
        }
    }
}