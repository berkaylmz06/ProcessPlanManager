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
            _odemeHareketleriRepo = odemeHareketleriRepo ?? throw new ArgumentNullException(nameof(odemeHareketleriRepo), "_odemeHareketleriRepo null olamaz.");
        }
        public void SaveOrUpdateOdemeBilgi(OdemeSartlari odemeSartlari, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");


            string checkQuery = "SELECT odemeTarihi FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
            DateTime? existingOdemeTarihi = null;
            using (var checkCmd = new SqlCommand(checkQuery, transaction.Connection, transaction))
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
                        kalanTutar = @kalanTutar";


                if (updateOdemeTarihi)
                    query += ", odemeTarihi = @odemeTarihi";

                query += " WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
            }
            else
            {
                string countQuery = "SELECT COUNT(*) FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
                int recordCount = 0;
                using (var countCmd = new SqlCommand(countQuery, transaction.Connection, transaction))
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
                            kalanTutar = @kalanTutar";


                    if (updateOdemeTarihi)
                        query += ", odemeTarihi = @odemeTarihi";

                    query += " WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
                }
                else
                {

                    query = @"
                        INSERT INTO ProjeFinans_OdemeSartlari
                        (projeId, kilometreTasiId, siralama, oran, tutar, tahminiTarih, gerceklesenTarih, aciklama, teminatMektubu, teminatDurumu, faturaNo, durum, kalanTutar, odemeTarihi)
                        VALUES (@projeId, @kilometreTasiId, @siralama, @oran, @tutar, @tahminiTarih, @gerceklesenTarih, @aciklama, @teminatMektubu, @teminatDurumu, @faturaNo, @durum, @kalanTutar, @odemeTarihi)";

                }
            }

            using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.Add("@projeId", SqlDbType.Int).Value = odemeSartlari.projeId;
                cmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = odemeSartlari.kilometreTasiId;
                cmd.Parameters.Add("@siralama", SqlDbType.Int).Value = odemeSartlari.siralama;
                cmd.Parameters.Add("@oran", SqlDbType.Decimal).Value = odemeSartlari.oran;
                cmd.Parameters.Add("@tutar", SqlDbType.Decimal).Value = odemeSartlari.tutar;
                cmd.Parameters.Add("@tahminiTarih", SqlDbType.DateTime2).Value = odemeSartlari.tahminiTarih ?? (object)DBNull.Value;
                cmd.Parameters.Add("@gerceklesenTarih", SqlDbType.DateTime2).Value = odemeSartlari.gerceklesenTarih ?? (object)DBNull.Value;
                cmd.Parameters.Add("@aciklama", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(odemeSartlari.odemeAciklama) ? (object)DBNull.Value : odemeSartlari.odemeAciklama;
                cmd.Parameters.Add("@teminatMektubu", SqlDbType.Bit).Value = odemeSartlari.teminatMektubu;
                cmd.Parameters.Add("@teminatDurumu", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(odemeSartlari.teminatDurumu) ? (object)DBNull.Value : odemeSartlari.teminatDurumu;
                cmd.Parameters.Add("@faturaNo", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(odemeSartlari.faturaNo) ? (object)DBNull.Value : odemeSartlari.faturaNo;
                cmd.Parameters.Add("@durum", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(odemeSartlari.durum) ? (object)DBNull.Value : odemeSartlari.durum;
                cmd.Parameters.Add("@kalanTutar", SqlDbType.Decimal).Value = odemeSartlari.kalanTutar;

                if (query.Contains("@odemeTarihi"))
                    cmd.Parameters.Add("@odemeTarihi", SqlDbType.DateTime2).Value = odemeSartlari.odemeTarihi ?? (object)DBNull.Value;

                cmd.ExecuteNonQuery();
            }
        }
        public string GetFaturaNo(int projeId, int kilometreTasiId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                try
                {
                    string query = "SELECT faturaNo FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@projeId", projeId);
                        cmd.Parameters.AddWithValue("@kilometreTasiId", kilometreTasiId);
                        var result = cmd.ExecuteScalar();
                        return result != null && result != DBNull.Value ? result.ToString() : null;
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<OdemeSartlari> GetOdemeBilgileri()
        {
            var odemeBilgileriList = new List<OdemeSartlari>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                try
                {
                    string query = @"
            SELECT
                o.odemeId,
                o.projeId,
                CASE 
                    WHEN pi.ustProjeId IS NOT NULL AND pUst.musteriAdi IS NOT NULL 
                        THEN pUst.musteriAdi
                    ELSE p.musteriAdi
                END AS musteriAdi,
                CASE 
                    WHEN pi.ustProjeId IS NOT NULL AND proUst.aciklama IS NOT NULL 
                        THEN proUst.aciklama
                    ELSE pro.aciklama
                END AS projeAciklama,
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
                o.odemeTarihi
            FROM ProjeFinans_OdemeSartlari o
            LEFT JOIN ProjeFinans_KilometreTaslari k ON o.kilometreTasiId = k.kilometreTasiId
            LEFT JOIN ProjeFinans_ProjeKutuk p ON p.projeId = o.projeId
            LEFT JOIN ProjeFinans_Projeler pro ON o.projeId = pro.projeId
            LEFT JOIN ProjeFinans_ProjeIliski pi ON o.projeId = pi.altProjeId
            LEFT JOIN ProjeFinans_ProjeKutuk pUst ON pi.ustProjeId = pUst.projeId
            LEFT JOIN ProjeFinans_Projeler proUst ON pi.ustProjeId = proUst.projeId;
            ";

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
                                    teminatMektubu = reader.GetBoolean(reader.GetOrdinal("teminatMektubu")),
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
                }
                finally
                {
                    connection.Close();
                }
            }
            return odemeBilgileriList;
        }

        public List<OdemeSartlari> GetOdemeBilgileriByProjeId(int projeId)
        {
            var odemeBilgileriList = new List<OdemeSartlari>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                try
                {
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
                                    teminatMektubu = reader.GetBoolean(reader.GetOrdinal("teminatMektubu")),
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
                }
                finally
                {
                    connection.Close();
                }
            }
            return odemeBilgileriList;
        }

        public OdemeSartlari GetOdemeBilgi(string projeNo, int kilometreTasiId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();

                try
                {
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
                                    teminatMektubu = reader.GetBoolean(reader.GetOrdinal("teminatMektubu")),
                                    teminatDurumu = reader.IsDBNull(reader.GetOrdinal("teminatDurumu")) ? null : reader.GetString(reader.GetOrdinal("teminatDurumu")),
                                    durum = reader.IsDBNull(reader.GetOrdinal("durum")) ? null : reader.GetString(reader.GetOrdinal("durum")),
                                    faturaNo = reader.IsDBNull(reader.GetOrdinal("faturaNo")) ? null : reader.GetString(reader.GetOrdinal("faturaNo")),
                                    kalanTutar = reader.GetDecimal(reader.GetOrdinal("kalanTutar")),
                                    odemeTarihi = reader.IsDBNull(reader.GetOrdinal("odemeTarihi")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("odemeTarihi")),
                                };
                            }
                            return null;
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public OdemeSartlari GetOdemeBilgiByOdemeId(int odemeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();

                try
                {
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
                                    teminatMektubu = reader.GetBoolean(reader.GetOrdinal("teminatMektubu")),
                                    teminatDurumu = reader.IsDBNull(reader.GetOrdinal("teminatDurumu")) ? null : reader.GetString(reader.GetOrdinal("teminatDurumu")),
                                    durum = reader.IsDBNull(reader.GetOrdinal("durum")) ? null : reader.GetString(reader.GetOrdinal("durum")),
                                    faturaNo = reader.IsDBNull(reader.GetOrdinal("faturaNo")) ? null : reader.GetString(reader.GetOrdinal("faturaNo")),
                                    kalanTutar = reader.GetDecimal(reader.GetOrdinal("kalanTutar")),
                                    odemeTarihi = reader.IsDBNull(reader.GetOrdinal("odemeTarihi")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("odemeTarihi")),
                                };
                            }
                            return null;
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void DeleteOdemeBilgi(int projeId, int kilometreTasiId, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

            string query = "DELETE FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId AND kilometreTasiId = @kilometreTasiId";
            using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@kilometreTasiId", kilometreTasiId);
                cmd.ExecuteNonQuery();
            }
        }

        public bool UpdateFaturaNo(int odemeId, string faturaNo, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

            string query = "UPDATE ProjeFinans_OdemeSartlari SET faturaNo = @faturaNo WHERE odemeId = @odemeId";
            using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@odemeId", odemeId);
                cmd.Parameters.AddWithValue("@faturaNo", string.IsNullOrEmpty(faturaNo) ? (object)DBNull.Value : faturaNo);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool OdemeSartlariSil(int projeId, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            if (transaction.Connection == null)
                throw new InvalidOperationException("Transaction bağlantısı null.");
            if (_odemeHareketleriRepo == null)
                throw new InvalidOperationException("_odemeHareketleriRepo başlatılmamış.");

            Console.WriteLine($"OdemeSartlariSil başladı, projeId: {projeId}");
            List<int> odemeIds = new List<int>();
            string selectQuery = "SELECT odemeId FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId";
            using (var selectCmd = new SqlCommand(selectQuery, transaction.Connection, transaction))
            {
                selectCmd.Parameters.AddWithValue("@projeId", projeId);
                using (var reader = selectCmd.ExecuteReader())
                {
                    Console.WriteLine("Reader oluşturuldu, veri var mı: " + reader.HasRows);
                    while (reader.Read())
                        odemeIds.Add(reader.GetInt32(0));
                }
            }
            Console.WriteLine($"Bulunan odemeId sayısı: {odemeIds.Count}");

            Console.WriteLine("DeleteOdemeHareketleriByOdemeIds çağrılıyor...");
            _odemeHareketleriRepo.DeleteOdemeHareketleriByOdemeIds(odemeIds, transaction);
            Console.WriteLine("DeleteOdemeHareketleriByOdemeIds tamamlandı.");

            string deleteQuery = "DELETE FROM ProjeFinans_OdemeSartlari WHERE projeId = @projeId";
            using (var deleteCmd = new SqlCommand(deleteQuery, transaction.Connection, transaction))
            {
                deleteCmd.Parameters.AddWithValue("@projeId", projeId);
                int rowsAffected = deleteCmd.ExecuteNonQuery();
                Console.WriteLine($"Silinen satır sayısı: {rowsAffected}");
            }

            return true;
        }
        public DataTable FiltreleOdemeBilgileri(Dictionary<string, TextBox> filtreKriterleri, DataGridView dataGrid)
        {
            var data = GetOdemeBilgileri();
            DataTable dt = new DataTable();
            if (data.Any())
            {
                dt = ToDataTableWithOdemeSapmasi(data);
            }

            List<string> filterExpressions = new List<string>();
            CultureInfo trCulture = CultureInfo.GetCultureInfo("tr-TR");

            foreach (var kvp in filtreKriterleri)
            {
                string headerText = kvp.Key;
                string value = kvp.Value.Text.Trim();

                if (string.IsNullOrWhiteSpace(value))
                    continue;

                string normalizedHeader = NormalizeColumnName(headerText.Replace("_Baslangic", "").Replace("_Bitis", ""));
                var column = dataGrid.Columns.Cast<DataGridViewColumn>()
                    .FirstOrDefault(c => NormalizeColumnName(c.HeaderText) == normalizedHeader);
                if (column == null || !dt.Columns.Contains(column.DataPropertyName))
                    continue;

                string dataTableColumnName = column.DataPropertyName;
                Type columnType = dt.Columns[dataTableColumnName].DataType;

                if (columnType == typeof(DateTime))
                {
                    if (DateTime.TryParse(value, trCulture, DateTimeStyles.None, out DateTime tarihDegeri))
                    {
                        if (headerText.EndsWith("_Baslangic"))
                        {
                            filterExpressions.Add($"{dataTableColumnName} >= #{tarihDegeri:MM/dd/yyyy}#");
                        }
                        else if (headerText.EndsWith("_Bitis"))
                        {
                            filterExpressions.Add($"{dataTableColumnName} < #{tarihDegeri.AddDays(1):MM/dd/yyyy}#");
                        }
                        else
                        {
                            filterExpressions.Add($"{dataTableColumnName} = #{tarihDegeri:MM/dd/yyyy}#");
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (dataTableColumnName == "odemeSapmasi")
                {
                    string op = "=";
                    string numericValue = value;

                    if (value.StartsWith(">="))
                    {
                        op = ">=";
                        numericValue = value.Substring(2).Trim();
                    }
                    else if (value.StartsWith("<="))
                    {
                        op = "<=";
                        numericValue = value.Substring(2).Trim();
                    }
                    else if (value.StartsWith(">"))
                    {
                        op = ">";
                        numericValue = value.Substring(1).Trim();
                    }
                    else if (value.StartsWith("<"))
                    {
                        op = "<";
                        numericValue = value.Substring(1).Trim();
                    }
                    else if (value.StartsWith("="))
                    {
                        op = "=";
                        numericValue = value.Substring(1).Trim();
                    }
                    else
                    {
                        numericValue = value;
                    }

                    if (int.TryParse(numericValue, out int val))
                    {
                        filterExpressions.Add($"{dataTableColumnName} {op} {val}");
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (columnType == typeof(string))
                {
                    if (dataTableColumnName == "teminatMektubu")
                    {
                        string searchValue = value.ToLower();
                        if (searchValue.Contains("%"))
                        {
                            string likeValue = searchValue.Replace("'", "''");
                            filterExpressions.Add($"{dataTableColumnName} LIKE '{likeValue}'");
                        }
                        else
                        {
                            if (searchValue == "var" || searchValue == "yok")
                            {
                                filterExpressions.Add($"{dataTableColumnName} = '{searchValue.Replace("'", "''")}'");
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (value.Contains("%"))
                        {
                            string likeValue = value.Replace("'", "''");
                            filterExpressions.Add($"{dataTableColumnName} LIKE '{likeValue}'");
                        }
                        else
                        {
                            filterExpressions.Add($"{dataTableColumnName} = '{value.Replace("'", "''")}'");
                        }
                    }
                }
                else if (columnType == typeof(decimal) || columnType == typeof(double))
                {
                    if (value.Contains("%"))
                    {
                        string likeValue = value.Replace("%", "[%]").Replace("'", "''");
                        filterExpressions.Add($"CONVERT({dataTableColumnName}, 'System.String') LIKE '{likeValue}'");
                    }
                    else
                    {
                        string op = "=";
                        string numericValue = value;

                        if (value.StartsWith(">="))
                        {
                            op = ">=";
                            numericValue = value.Substring(2).Trim();
                        }
                        else if (value.StartsWith("<="))
                        {
                            op = "<=";
                            numericValue = value.Substring(2).Trim();
                        }
                        else if (value.StartsWith(">"))
                        {
                            op = ">";
                            numericValue = value.Substring(1).Trim();
                        }
                        else if (value.StartsWith("<"))
                        {
                            op = "<";
                            numericValue = value.Substring(1).Trim();
                        }
                        else if (value.StartsWith("="))
                        {
                            op = "=";
                            numericValue = value.Substring(1).Trim();
                        }
                        else
                        {
                            numericValue = value;
                        }

                        if (decimal.TryParse(numericValue, NumberStyles.Any, trCulture, out decimal decimalValue))
                        {
                            filterExpressions.Add($"{dataTableColumnName} {op} {decimalValue.ToString(CultureInfo.InvariantCulture)}");
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else if (columnType == typeof(int))
                {
                    string op = "=";
                    string numericValue = value;

                    if (value.StartsWith(">="))
                    {
                        op = ">=";
                        numericValue = value.Substring(2).Trim();
                    }
                    else if (value.StartsWith("<="))
                    {
                        op = "<=";
                        numericValue = value.Substring(2).Trim();
                    }
                    else if (value.StartsWith(">"))
                    {
                        op = ">";
                        numericValue = value.Substring(1).Trim();
                    }
                    else if (value.StartsWith("<"))
                    {
                        op = "<";
                        numericValue = value.Substring(1).Trim();
                    }
                    else if (value.StartsWith("="))
                    {
                        op = "=";
                        numericValue = value.Substring(1).Trim();
                    }
                    else
                    {
                        numericValue = value;
                    }

                    if (int.TryParse(numericValue, out int intValue))
                    {
                        filterExpressions.Add($"{dataTableColumnName} {op} {intValue}");
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            if (filterExpressions.Any())
            {
                string finalFilterExpression = string.Join(" AND ", filterExpressions);
                dt.DefaultView.RowFilter = finalFilterExpression;
                return dt.DefaultView.ToTable();
            }

            return dt;
        }

        public DataTable ToDataTableWithOdemeSapmasi(List<OdemeSartlari> data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("odemeId", typeof(int));
            dt.Columns.Add("projeId", typeof(int));
            dt.Columns.Add("musteriAdi", typeof(string));
            dt.Columns.Add("projeAciklama", typeof(string));
            dt.Columns.Add("kilometreTasiId", typeof(int));
            dt.Columns.Add("kilometreTasiAdi", typeof(string));
            dt.Columns.Add("siralama", typeof(int));
            dt.Columns.Add("oran", typeof(decimal));
            dt.Columns.Add("tutar", typeof(decimal));
            dt.Columns.Add("paraBirimi", typeof(string));
            dt.Columns.Add("tahminiTarih", typeof(DateTime));
            dt.Columns.Add("gerceklesenTarih", typeof(DateTime));
            dt.Columns.Add("odemeAciklama", typeof(string));
            dt.Columns.Add("teminatMektubu", typeof(string));
            dt.Columns.Add("teminatDurumu", typeof(string));
            dt.Columns.Add("durum", typeof(string));
            dt.Columns.Add("faturaNo", typeof(string));
            dt.Columns.Add("kalanTutar", typeof(decimal));
            dt.Columns.Add("odemeTarihi", typeof(DateTime));
            dt.Columns.Add("odemeSapmasi", typeof(int));
            dt.Columns.Add("statu", typeof(string));

            foreach (var item in data)
            {
                var row = dt.NewRow();
                row["odemeId"] = item.odemeId;
                row["projeId"] = item.projeId;
                row["musteriAdi"] = item.musteriAdi ?? (object)DBNull.Value;
                row["projeAciklama"] = item.projeAciklama ?? (object)DBNull.Value;
                row["kilometreTasiId"] = item.kilometreTasiId;
                row["kilometreTasiAdi"] = item.kilometreTasiAdi ?? (object)DBNull.Value;
                row["siralama"] = item.siralama;
                row["oran"] = item.oran;
                row["tutar"] = item.tutar;
                row["paraBirimi"] = item.paraBirimi ?? (object)DBNull.Value;
                row["tahminiTarih"] = item.tahminiTarih.HasValue ? item.tahminiTarih.Value : (object)DBNull.Value;
                row["gerceklesenTarih"] = item.gerceklesenTarih.HasValue ? item.gerceklesenTarih.Value : (object)DBNull.Value;
                row["odemeAciklama"] = item.odemeAciklama ?? (object)DBNull.Value;
                row["teminatMektubu"] = item.teminatMektubu ? "Var" : "Yok";
                row["teminatDurumu"] = item.teminatDurumu ?? (object)DBNull.Value;
                row["durum"] = item.durum ?? (object)DBNull.Value;
                row["faturaNo"] = item.faturaNo ?? (object)DBNull.Value;
                row["kalanTutar"] = item.kalanTutar;
                row["odemeTarihi"] = item.odemeTarihi.HasValue ? item.odemeTarihi.Value : (object)DBNull.Value;
                row["odemeSapmasi"] = item.odemeSapmasi;
                row["statu"] = item.statu ?? (object)DBNull.Value;

                dt.Rows.Add(row);
            }

            return dt;
        }

        public string NormalizeColumnName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return columnName;

            return columnName.Replace("ı", "i").Replace("İ", "I").Replace("ş", "s").Replace("Ş", "S")
                            .Replace("ğ", "g").Replace("Ğ", "G").Replace("ü", "u").Replace("Ü", "U")
                            .Replace("ç", "c").Replace("Ç", "C").Replace("ö", "o").Replace("Ö", "O")
                            .ToLower();
        }
    }
}