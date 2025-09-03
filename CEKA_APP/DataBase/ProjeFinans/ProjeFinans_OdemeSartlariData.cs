using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_OdemeSartlariData
    {
        public void SaveOrUpdateOdemeBilgi(string projeNo, int kilometreTasiId, int siralama, string oran, string tutar, string tahminiTarih, string gerceklesenTarih, string aciklama, bool teminatMektubu, string teminatDurumu, string durum, string faturaNo, string kalanTutar, string odemeTarihi = null)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT odemeTarihi FROM ProjeFinans_OdemeSartlari WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                    DateTime? existingOdemeTarihi = null;
                    using (var checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = projeNo?.Trim() ?? throw new ArgumentNullException(nameof(projeNo));
                        checkCmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = kilometreTasiId;

                        var result = checkCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            existingOdemeTarihi = (DateTime)result;
                        }
                    }

                    string query;
                    bool updateOdemeTarihi = !string.IsNullOrWhiteSpace(odemeTarihi);

                    if (existingOdemeTarihi != null || !string.IsNullOrWhiteSpace(faturaNo))
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
                        {
                            query += ", odemeTarihi = @odemeTarihi";
                        }

                        query += " WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                    }
                    else
                    {
                        string countQuery = "SELECT COUNT(*) FROM ProjeFinans_OdemeSartlari WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                        int recordCount = 0;
                        using (var countCmd = new SqlCommand(countQuery, connection))
                        {
                            countCmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = projeNo?.Trim() ?? throw new ArgumentNullException(nameof(projeNo));
                            countCmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = kilometreTasiId;
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
                            {
                                query += ", odemeTarihi = @odemeTarihi";
                            }

                            query += " WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                        }
                        else
                        {
                            query = @"
                                INSERT INTO ProjeFinans_OdemeSartlari
                                (projeNo, kilometreTasiId, siralama, oran, tutar, tahminiTarih, gerceklesenTarih, aciklama, teminatMektubu, teminatDurumu, faturaNo, durum, kalanTutar, odemeTarihi)
                                VALUES (@projeNo, @kilometreTasiId, @siralama, @oran, @tutar, @tahminiTarih, @gerceklesenTarih, @aciklama, @teminatMektubu, @teminatDurumu, @faturaNo, @durum, @kalanTutar, @odemeTarihi)";
                        }
                    }

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = projeNo?.Trim() ?? throw new ArgumentNullException(nameof(projeNo));
                        cmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = kilometreTasiId;
                        cmd.Parameters.Add("@siralama", SqlDbType.Int).Value = siralama;
                        cmd.Parameters.Add("@oran", SqlDbType.Decimal).Value = decimal.Parse(oran, CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@tutar", SqlDbType.Decimal).Value = decimal.Parse(tutar, CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@tahminiTarih", SqlDbType.DateTime2).Value = string.IsNullOrWhiteSpace(tahminiTarih) ? (object)DBNull.Value : DateTime.Parse(tahminiTarih);
                        cmd.Parameters.Add("@gerceklesenTarih", SqlDbType.DateTime2).Value = string.IsNullOrWhiteSpace(gerceklesenTarih) ? (object)DBNull.Value : DateTime.Parse(gerceklesenTarih);
                        cmd.Parameters.Add("@aciklama", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(aciklama) ? (object)DBNull.Value : aciklama;
                        cmd.Parameters.Add("@teminatMektubu", SqlDbType.Bit).Value = teminatMektubu;
                        cmd.Parameters.Add("@teminatDurumu", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(teminatDurumu) ? (object)DBNull.Value : teminatDurumu;
                        cmd.Parameters.Add("@faturaNo", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(faturaNo) ? (object)DBNull.Value : faturaNo;
                        cmd.Parameters.Add("@durum", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(durum) ? (object)DBNull.Value : durum;
                        cmd.Parameters.Add("@kalanTutar", SqlDbType.Decimal).Value = string.IsNullOrWhiteSpace(kalanTutar) ? (object)DBNull.Value : decimal.Parse(kalanTutar, CultureInfo.InvariantCulture);

                        if (query.Contains("@odemeTarihi"))
                        {
                            cmd.Parameters.Add("@odemeTarihi", SqlDbType.DateTime2).Value = string.IsNullOrWhiteSpace(odemeTarihi) ? (object)DBNull.Value : DateTime.Parse(odemeTarihi);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ödeme bilgisi kaydedilirken/güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        public string GetFaturaNo(string projeNo, int kilometreTasiId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT faturaNo FROM ProjeFinans_OdemeSartlari WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = projeNo?.Trim() ?? throw new ArgumentNullException(nameof(projeNo));
                        cmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = kilometreTasiId;
                        var result = cmd.ExecuteScalar();
                        return result != null && result != DBNull.Value ? result.ToString() : null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fatura numarası alınırken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public List<OdemeSartlari> GetOdemeBilgileri()
        {
            var odemeBilgileriList = new List<OdemeSartlari>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT
                        o.odemeId,
                        o.projeNo,
                        CASE 
                            WHEN pi.ustProjeNo IS NOT NULL AND pUst.musteriAdi IS NOT NULL 
                                THEN pUst.musteriAdi
                            ELSE p.musteriAdi
                        END AS musteriAdi,
                        CASE 
                            WHEN pi.ustProjeNo IS NOT NULL AND proUst.aciklama IS NOT NULL 
                                THEN proUst.aciklama
                            ELSE pro.aciklama
                        END AS projeAciklama,
                        o.kilometreTasiId,
                        k.kilometreTasiAdi,
                        o.siralama,
                        o.oran,
                        o.tutar,
                        CASE 
                            WHEN pi.ustProjeNo IS NOT NULL AND pUst.paraBirimi IS NOT NULL 
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
                    LEFT JOIN ProjeFinans_ProjeKutuk p ON p.projeNo = o.projeNo
                    LEFT JOIN ProjeFinans_Projeler pro ON o.projeNo = pro.projeNo
                    LEFT JOIN ProjeFinans_ProjeIliski pi ON o.projeNo = pi.altProjeNo
                    LEFT JOIN ProjeFinans_ProjeKutuk pUst ON pi.ustProjeNo = pUst.projeNo
                    LEFT JOIN ProjeFinans_Projeler proUst ON pi.ustProjeNo = proUst.projeNo;
                    ";

                using (var cmd = new SqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var odemeSekilleri = new OdemeSartlari
                            {
                                odemeId = Convert.ToInt32(reader["odemeId"]),
                                projeNo = reader["projeNo"].ToString(),
                                musteriAdi = reader["musteriAdi"].ToString(),
                                projeAciklama = reader["projeAciklama"] != DBNull.Value ? reader["projeAciklama"].ToString() : null,
                                kilometreTasiId = Convert.ToInt32(reader["kilometreTasiId"]),
                                kilometreTasiAdi = reader["kilometreTasiAdi"].ToString(),
                                siralama = Convert.ToInt32(reader["siralama"]),
                                oran = Convert.ToDecimal(reader["oran"]),
                                tutar = Convert.ToDecimal(reader["tutar"]),
                                paraBirimi = reader["paraBirimi"].ToString(),
                                odemeAciklama = reader["odemeAciklama"] != DBNull.Value ? reader["odemeAciklama"].ToString() : null,
                                teminatMektubu = Convert.ToBoolean(reader["teminatMektubu"]),
                                teminatDurumu = reader["teminatDurumu"] != DBNull.Value ? reader["teminatDurumu"].ToString() : null,
                                durum = reader["durum"] != DBNull.Value ? reader["durum"].ToString() : null,
                                faturaNo = reader["faturaNo"] != DBNull.Value ? reader["faturaNo"].ToString() : null,
                                kalanTutar = reader["kalanTutar"] != DBNull.Value ? Convert.ToDecimal(reader["kalanTutar"]) : 0,
                                odemeTarihi = reader["odemeTarihi"] != DBNull.Value ? Convert.ToDateTime(reader["odemeTarihi"]) : (DateTime?)null,
                                tahminiTarih = reader["tahminiTarih"] != DBNull.Value ? Convert.ToDateTime(reader["tahminiTarih"]) : (DateTime?)null,
                                gerceklesenTarih = reader["gerceklesenTarih"] != DBNull.Value ? Convert.ToDateTime(reader["gerceklesenTarih"]) : (DateTime?)null
                            };

                            odemeBilgileriList.Add(odemeSekilleri);
                        }
                    }
                }
            }
            return odemeBilgileriList;
        }

        public List<OdemeSartlari> GetOdemeBilgileriByProjeNo(string projeNo)
        {
            var odemeBilgileriList = new List<OdemeSartlari>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT
                        o.odemeId,
                        o.projeNo,
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
                    WHERE o.projeNo = @projeNo
                    ORDER BY o.siralama";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeNo", projeNo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var odemeSekilleri = new OdemeSartlari
                            {
                                odemeId = Convert.ToInt32(reader["odemeId"]),
                                projeNo = reader["projeNo"].ToString(),
                                kilometreTasiId = Convert.ToInt32(reader["kilometreTasiId"]),
                                kilometreTasiAdi = reader["kilometreTasiAdi"].ToString(),
                                siralama = Convert.ToInt32(reader["siralama"]),
                                oran = Convert.ToDecimal(reader["oran"]),
                                tutar = Convert.ToDecimal(reader["tutar"]),
                                tahminiTarih = reader["tahminiTarih"] != DBNull.Value ? Convert.ToDateTime(reader["tahminiTarih"]) : (DateTime?)null,
                                gerceklesenTarih = reader["gerceklesenTarih"] != DBNull.Value ? Convert.ToDateTime(reader["gerceklesenTarih"]) : (DateTime?)null,
                                odemeAciklama = reader["aciklama"] != DBNull.Value ? reader["aciklama"].ToString() : null,
                                teminatMektubu = Convert.ToBoolean(reader["teminatMektubu"]),
                                teminatDurumu = reader["teminatDurumu"] != DBNull.Value ? reader["teminatDurumu"].ToString() : null,
                                durum = reader["durum"] != DBNull.Value ? reader["durum"].ToString() : null,
                                faturaNo = reader["faturaNo"] != DBNull.Value ? reader["faturaNo"].ToString() : null,
                                kalanTutar = reader["kalanTutar"] != DBNull.Value ? Convert.ToDecimal(reader["kalanTutar"]) : Convert.ToDecimal(reader["tutar"]),
                                odemeTarihi = reader["odemeTarihi"] != DBNull.Value ? Convert.ToDateTime(reader["odemeTarihi"]) : (DateTime?)null
                            };

                            odemeBilgileriList.Add(odemeSekilleri);
                        }
                    }
                }
            }
            return odemeBilgileriList;
        }

        public OdemeSartlari GetOdemeBilgi(string projeNo, int kilometreTasiId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT
                        o.odemeId,
                        o.projeNo,
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
                    WHERE o.projeNo = @projeNo AND o.kilometreTasiId = @kilometreTasiId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeNo", projeNo);
                    cmd.Parameters.AddWithValue("@kilometreTasiId", kilometreTasiId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var odemeSekilleri = new OdemeSartlari
                            {
                                odemeId = Convert.ToInt32(reader["odemeId"]),
                                projeNo = reader["projeNo"].ToString(),
                                kilometreTasiId = Convert.ToInt32(reader["kilometreTasiId"]),
                                kilometreTasiAdi = reader["kilometreTasiAdi"].ToString(),
                                siralama = Convert.ToInt32(reader["siralama"]),
                                oran = Convert.ToDecimal(reader["oran"]),
                                tutar = Convert.ToDecimal(reader["tutar"]),
                                tahminiTarih = reader["tahminiTarih"] != DBNull.Value ? Convert.ToDateTime(reader["tahminiTarih"]) : (DateTime?)null,
                                gerceklesenTarih = reader["gerceklesenTarih"] != DBNull.Value ? Convert.ToDateTime(reader["gerceklesenTarih"]) : (DateTime?)null,
                                odemeAciklama = reader["aciklama"] != DBNull.Value ? reader["aciklama"].ToString() : null,
                                teminatMektubu = Convert.ToBoolean(reader["teminatMektubu"]),
                                teminatDurumu = reader["teminatDurumu"] != DBNull.Value ? reader["teminatDurumu"].ToString() : null,
                                durum = reader["durum"] != DBNull.Value ? reader["durum"].ToString() : null,
                                faturaNo = reader["faturaNo"] != DBNull.Value ? reader["faturaNo"].ToString() : null,
                                kalanTutar = reader["kalanTutar"] != DBNull.Value ? Convert.ToDecimal(reader["kalanTutar"]) : 0,
                                odemeTarihi = reader["odemeTarihi"] != DBNull.Value ? Convert.ToDateTime(reader["odemeTarihi"]) : (DateTime?)null
                            };

                            return odemeSekilleri;
                        }
                        return null;
                    }
                }
            }
        }

        public dynamic GetOdemeBilgiByOdemeId(int odemeId)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT
                            odemeId,
                            projeNo,
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
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@odemeId", odemeId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new
                                {
                                    odemeId = reader.GetInt32(reader.GetOrdinal("odemeId")),
                                    projeNo = reader.GetString(reader.GetOrdinal("projeNo")),
                                    kilometreTasiId = reader.GetInt32(reader.GetOrdinal("kilometreTasiId")),
                                    siralama = reader.GetInt32(reader.GetOrdinal("siralama")),
                                    oran = reader.GetDecimal(reader.GetOrdinal("oran")),
                                    tutar = reader.GetDecimal(reader.GetOrdinal("tutar")),
                                    tahminiTarih = reader.IsDBNull(reader.GetOrdinal("tahminiTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("tahminiTarih")),
                                    gerceklesenTarih = reader.IsDBNull(reader.GetOrdinal("gerceklesenTarih")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gerceklesenTarih")),
                                    aciklama = reader.IsDBNull(reader.GetOrdinal("aciklama")) ? null : reader.GetString(reader.GetOrdinal("aciklama")),
                                    teminatMektubu = reader.IsDBNull(reader.GetOrdinal("teminatMektubu")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("teminatMektubu")),
                                    teminatDurumu = reader.IsDBNull(reader.GetOrdinal("teminatDurumu")) ? null : reader.GetString(reader.GetOrdinal("teminatDurumu")),
                                    durum = reader.IsDBNull(reader.GetOrdinal("durum")) ? null : reader.GetString(reader.GetOrdinal("durum")),
                                    faturaNo = reader.IsDBNull(reader.GetOrdinal("faturaNo")) ? null : reader.GetString(reader.GetOrdinal("faturaNo")),
                                    kalanTutar = reader.IsDBNull(reader.GetOrdinal("kalanTutar")) ? 0 : reader.GetDecimal(reader.GetOrdinal("kalanTutar")),
                                    odemeTarihi = reader.IsDBNull(reader.GetOrdinal("odemeTarihi")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("odemeTarihi"))
                                };
                            }
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ödeme bilgisi alınırken hata: {ex.Message}");
            }
        }

        public void DeleteOdemeBilgi(string projeNo, int kilometreTasiId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM ProjeFinans_OdemeSartlari WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = projeNo;
                        cmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = kilometreTasiId;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ödeme bilgisi silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        public bool UpdateFaturaNo(int odemeId, string faturaNo)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE ProjeFinans_OdemeSartlari SET faturaNo = @faturaNo WHERE odemeId = @odemeId";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@odemeId", SqlDbType.Int).Value = odemeId;
                        command.Parameters.Add("@faturaNo", SqlDbType.NVarChar, 50).Value = string.IsNullOrEmpty(faturaNo) ? (object)DBNull.Value : faturaNo;
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show($"Ödeme ID: {odemeId} için kayıt bulunamadı. Fatura numarası güncellenemedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fatura numarası güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool OdemeSartlariSil(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            List<int> odemeIds = new List<int>();
                            string selectQuery = @"
                                SELECT odemeId 
                                FROM ProjeFinans_OdemeSartlari 
                                WHERE projeNo = @projeNo";
                            using (var selectCmd = new SqlCommand(selectQuery, connection, transaction))
                            {
                                selectCmd.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                                using (var reader = selectCmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        odemeIds.Add(reader.GetInt32(0));
                                    }
                                }
                            }

                            var odemeHareketleriData = new ProjeFinans_OdemeHareketleriData();
                            if (!odemeHareketleriData.DeleteOdemeHareketleriByOdemeIds(odemeIds))
                            {
                                throw new Exception("Ödeme hareketleri silinirken hata oluştu.");
                            }

                            string deleteQuery = @"
                                DELETE FROM ProjeFinans_OdemeSartlari
                                WHERE projeNo = @projeNo";
                            using (var deleteCmd = new SqlCommand(deleteQuery, connection, transaction))
                            {
                                deleteCmd.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                                deleteCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Ödeme şartları silinirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bağlantı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        public DataTable FiltreleOdemeBilgileri(Dictionary<string, TextBox> filtreKriterleri, DataGridView dataGrid)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Arama sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        private DataTable ToDataTableWithOdemeSapmasi(List<OdemeSartlari> data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("odemeId", typeof(int));
            dt.Columns.Add("projeNo", typeof(string));
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

            foreach (var item in data)
            {
                var row = dt.NewRow();
                row["odemeId"] = item.odemeId;
                row["projeNo"] = item.projeNo;
                row["musteriAdi"] = item.musteriAdi;
                row["projeAciklama"] = item.projeAciklama ?? (object)DBNull.Value;
                row["kilometreTasiId"] = item.kilometreTasiId;
                row["kilometreTasiAdi"] = item.kilometreTasiAdi;
                row["siralama"] = item.siralama;
                row["oran"] = item.oran;
                row["tutar"] = item.tutar;
                row["paraBirimi"] = item.paraBirimi;
                row["tahminiTarih"] = item.tahminiTarih ?? (object)DBNull.Value;
                row["gerceklesenTarih"] = item.gerceklesenTarih ?? (object)DBNull.Value;
                row["odemeAciklama"] = item.odemeAciklama ?? (object)DBNull.Value;
                row["teminatMektubu"] = item.teminatMektubu ? "Var" : "Yok";
                row["teminatDurumu"] = item.teminatDurumu ?? (object)DBNull.Value;
                row["durum"] = item.durum ?? (object)DBNull.Value;
                row["faturaNo"] = item.faturaNo ?? (object)DBNull.Value;
                row["kalanTutar"] = item.kalanTutar;
                row["odemeTarihi"] = item.odemeTarihi ?? (object)DBNull.Value;

                int sapma = 0;
                if (item.gerceklesenTarih.HasValue && item.odemeTarihi.HasValue)
                {
                    sapma = (item.gerceklesenTarih.Value - item.odemeTarihi.Value).Days;
                }
                row["odemeSapmasi"] = sapma;

                dt.Rows.Add(row);
            }

            return dt;
        }
        private string NormalizeColumnName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return columnName;
            return columnName.Replace("ı", "i").Replace("İ", "I").Replace("ş", "s").Replace("Ş", "S")
                            .Replace("ğ", "g").Replace("Ğ", "G").Replace("ü", "u").Replace("Ü", "U")
                            .Replace("ç", "c").Replace("Ç", "C").Replace("ö", "o").Replace("Ö", "O")
                            .ToLower();
        }
    }
}
