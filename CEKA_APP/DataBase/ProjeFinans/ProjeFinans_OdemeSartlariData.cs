using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
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

                    // Check if the record already exists and retrieve odemeTarihi
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

                    // Determine if we need to update odemeTarihi or not based on its existence
                    if (existingOdemeTarihi != null || !string.IsNullOrWhiteSpace(faturaNo)) // Assuming a record exists if either existingOdemeTarihi is not null or we have a faturaNo to update
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
                        // If no existing record and no odemeTarihi is provided, you might want to insert a new one
                        // or handle this as a different case.
                        // For this specific error, let's assume we are always updating, or the INSERT logic needs to be separate.
                        // The original code was a bit convoluted with its logic. Let's simplify.
                        // Let's first check for existence with COUNT(*) and then decide on INSERT or UPDATE.
                        // This is a cleaner approach.
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
                            // If record exists, perform UPDATE. The previous UPDATE query will work.
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
                            // If no record exists, perform INSERT.
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
                        p.musteriAdi,
                        pro.aciklama  AS projeAciklama,   
                        o.kilometreTasiId,
                        k.kilometreTasiAdi,
                        o.siralama,
                        o.oran,
                        o.tutar,
                        p.paraBirimi,
                        o.tahminiTarih,
                        o.gerceklesenTarih,
                        o.aciklama   AS odemeAciklama,  
                        o.teminatMektubu,
                        o.teminatDurumu,
                        o.durum,
                        o.faturaNo,
                        o.kalanTutar,
                        o.odemeTarihi
                    FROM ProjeFinans_OdemeSartlari o
                    JOIN ProjeFinans_KilometreTaslari k ON o.kilometreTasiId = k.kilometreTasiId
                    JOIN ProjeFinans_ProjeKutuk p ON p.projeNo = o.projeNo
                    JOIN ProjeFinans_Projeler pro ON p.projeNo = pro.projeNo;
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

        public DataTable FiltreleOdemeBilgileri(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    string baseQuery = @"
                SELECT
                    o.odemeId,
                    o.projeNo,
                    p.musteriAdi,
                    pro.aciklama AS projeAciklama,
                    k.kilometreTasiAdi AS kilometreTasiAdi,
                    o.siralama,
                    o.oran,
                    o.tutar,
                    p.paraBirimi,
                    o.tahminiTarih,
                    o.gerceklesenTarih,
                    o.aciklama AS odemeAciklama,
                    o.teminatMektubu,
                    o.teminatDurumu,
                    o.durum,
                    o.kalanTutar,
                    o.odemeTarihi,
                    o.faturaNo,
                    CASE 
                        WHEN o.gerceklesenTarih IS NOT NULL AND o.odemeTarihi IS NOT NULL 
                        THEN DATEDIFF(DAY, o.gerceklesenTarih, o.odemeTarihi)
                        ELSE NULL 
                    END AS OdemeSapmasi
                FROM ProjeFinans_OdemeSartlari o
                JOIN ProjeFinans_KilometreTaslari k ON o.kilometreTasiId = k.kilometreTasiId
                JOIN ProjeFinans_ProjeKutuk p ON p.projeNo = o.projeNo
                JOIN ProjeFinans_Projeler pro ON pro.projeNo = p.projeNo
                WHERE 1=1";

                    var conditions = new List<string>();
                    var parameters = new List<SqlParameter>();
                    int paramIndex = 0;

                    // Tarih filtrelerini tutmak için bir Dictionary
                    var dateFilters = new Dictionary<string, (string start, string end)>();

                    foreach (var kutu in filtreKutulari)
                    {
                        string hamDeger = kutu.Value.Text.Trim();
                        if (string.IsNullOrEmpty(hamDeger))
                        {
                            continue;
                        }

                        // Tarih başlangıç/bitiş filtrelerini topla
                        if (kutu.Key.EndsWith("_Baslangic"))
                        {
                            string baseName = kutu.Key.Replace("_Baslangic", "");
                            if (!dateFilters.ContainsKey(baseName))
                            {
                                dateFilters[baseName] = (start: hamDeger, end: null);
                            }
                            else
                            {
                                dateFilters[baseName] = (start: hamDeger, end: dateFilters[baseName].end);
                            }
                        }
                        else if (kutu.Key.EndsWith("_Bitis"))
                        {
                            string baseName = kutu.Key.Replace("_Bitis", "");
                            if (!dateFilters.ContainsKey(baseName))
                            {
                                dateFilters[baseName] = (start: null, end: hamDeger);
                            }
                            else
                            {
                                dateFilters[baseName] = (start: dateFilters[baseName].start, end: hamDeger);
                            }
                        }
                        else
                        {
                            // Diğer filtreler için mevcut mantık
                            string columnName = dataGrid.Columns.Cast<DataGridViewColumn>()
                                .FirstOrDefault(c => NormalizeColumnName(c.HeaderText) == NormalizeColumnName(kutu.Key) ||
                                                    NormalizeColumnName(c.Name) == NormalizeColumnName(kutu.Key))?.Name;

                            if (string.IsNullOrEmpty(columnName))
                            {
                                MessageBox.Show($"Sütun bulunamadı: {kutu.Key}", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                continue;
                            }

                            string condition = string.Empty;

                            switch (columnName)
                            {
                                case "projeNo":
                                    condition = $"o.projeNo LIKE @p{paramIndex}";
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.NVarChar) { Value = $"%{hamDeger}%" });
                                    paramIndex++;
                                    break;
                                case "musteriAdi":
                                    condition = $"p.musteriAdi LIKE @p{paramIndex}";
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.NVarChar) { Value = $"%{hamDeger}%" });
                                    paramIndex++;
                                    break;
                                case "projeAciklama":
                                    condition = $"pro.aciklama LIKE @p{paramIndex}";
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.NVarChar) { Value = $"%{hamDeger}%" });
                                    paramIndex++;
                                    break;
                                case "kilometreTasiAdi":
                                    condition = $"k.kilometreTasiAdi LIKE @p{paramIndex}";
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.NVarChar) { Value = $"%{hamDeger}%" });
                                    paramIndex++;
                                    break;
                                case "siralama":
                                    if (int.TryParse(hamDeger, out int siralama))
                                    {
                                        condition = $"o.siralama = @p{paramIndex}";
                                        parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Int) { Value = siralama });
                                        paramIndex++;
                                    }
                                    break;
                                case "oran":
                                    if (decimal.TryParse(hamDeger, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal oran))
                                    {
                                        condition = $"ABS(o.oran - @p{paramIndex}) < 0.01";
                                        parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Decimal) { Value = oran });
                                        paramIndex++;
                                    }
                                    break;
                                case "tutar":
                                    if (decimal.TryParse(hamDeger, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal tutar))
                                    {
                                        condition = $"ABS(o.tutar - @p{paramIndex}) < 0.01";
                                        parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Decimal) { Value = tutar });
                                        paramIndex++;
                                    }
                                    break;
                                case "paraBirimi":
                                    condition = $"p.paraBirimi LIKE @p{paramIndex}";
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.NVarChar) { Value = $"%{hamDeger}%" });
                                    paramIndex++;
                                    break;
                                case "odemeAciklama":
                                    condition = $"o.aciklama LIKE @p{paramIndex}";
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.NVarChar) { Value = $"%{hamDeger}%" });
                                    paramIndex++;
                                    break;
                                case "teminatMektubu":
                                    bool? teminat = null;
                                    if (hamDeger.Contains("evet") || hamDeger.Contains("true") || hamDeger == "1" || hamDeger.Contains("var"))
                                        teminat = true;
                                    else if (hamDeger.Contains("hayır") || hamDeger.Contains("false") || hamDeger == "0" || hamDeger.Contains("yok"))
                                        teminat = false;
                                    if (teminat.HasValue)
                                    {
                                        condition = $"o.teminatMektubu = @p{paramIndex}";
                                        parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Bit) { Value = teminat.Value });
                                        paramIndex++;
                                    }
                                    break;
                                case "teminatDurumu":
                                    condition = $"o.teminatDurumu LIKE @p{paramIndex}";
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.NVarChar) { Value = $"%{hamDeger}%" });
                                    paramIndex++;
                                    break;
                                case "durum":
                                    condition = $"o.durum LIKE @p{paramIndex}";
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.NVarChar) { Value = $"%{hamDeger}%" });
                                    paramIndex++;
                                    break;
                                case "kalanTutar":
                                    if (decimal.TryParse(hamDeger, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal kalanTutar))
                                    {
                                        condition = $"ABS(o.kalanTutar - @p{paramIndex}) < 0.01";
                                        parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Decimal) { Value = kalanTutar });
                                        paramIndex++;
                                    }
                                    break;
                                case "faturaNo":
                                    condition = $"o.faturaNo LIKE @p{paramIndex}";
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.NVarChar) { Value = $"%{hamDeger}%" });
                                    paramIndex++;
                                    break;
                                case "OdemeSapmasi": // Yeni eklenen, hesaplanmış sütun için
                                    if (hamDeger.Equals("-", StringComparison.OrdinalIgnoreCase))
                                    {
                                        condition = "(o.gerceklesenTarih IS NULL OR o.odemeTarihi IS NULL)";
                                    }
                                    else if (int.TryParse(hamDeger.Replace("+", "").Replace("-", ""), out int sapma))
                                    {
                                        if (hamDeger.StartsWith("+"))
                                        {
                                            condition = $"o.gerceklesenTarih IS NOT NULL AND o.odemeTarihi IS NOT NULL AND DATEDIFF(DAY, o.gerceklesenTarih, o.odemeTarihi) = @p{paramIndex}";
                                            parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Int) { Value = -sapma });
                                            paramIndex++;
                                        }
                                        else if (hamDeger.StartsWith("-"))
                                        {
                                            condition = $"o.gerceklesenTarih IS NOT NULL AND o.odemeTarihi IS NOT NULL AND DATEDIFF(DAY, o.gerceklesenTarih, o.odemeTarihi) = @p{paramIndex}";
                                            parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Int) { Value = sapma });
                                            paramIndex++;
                                        }
                                        else
                                        {
                                            condition = $"o.gerceklesenTarih IS NOT NULL AND o.odemeTarihi IS NOT NULL AND ABS(DATEDIFF(DAY, o.gerceklesenTarih, o.odemeTarihi)) = @p{paramIndex}";
                                            parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Int) { Value = sapma });
                                            paramIndex++;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Geçersiz ödeme sapması girişi: {hamDeger}");
                                        continue;
                                    }
                                    break;
                            }

                            if (!string.IsNullOrEmpty(condition))
                            {
                                conditions.Add(condition);
                            }
                        }
                    }

                    // Tarih filtrelerini sorguya ekle
                    foreach (var filter in dateFilters)
                    {
                        string columnHeader = filter.Key;
                        string dbColumnName = string.Empty;

                        if (columnHeader.Contains("Tahmini Tarih")) dbColumnName = "o.tahminiTarih";
                        else if (columnHeader.Contains("Gerçekleşen Tarih")) dbColumnName = "o.gerceklesenTarih";
                        else if (columnHeader.Contains("Ödeme Tarihi")) dbColumnName = "o.odemeTarihi";

                        if (!string.IsNullOrEmpty(dbColumnName))
                        {
                            string startDate = filter.Value.start;
                            string endDate = filter.Value.end;

                            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                            {
                                if (DateTime.TryParse(startDate, out DateTime dStart) && DateTime.TryParse(endDate, out DateTime dEnd))
                                {
                                    conditions.Add($"CAST({dbColumnName} AS DATE) BETWEEN @p{paramIndex} AND @p{paramIndex + 1}");
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Date) { Value = dStart.Date });
                                    parameters.Add(new SqlParameter($"@p{paramIndex + 1}", SqlDbType.Date) { Value = dEnd.Date });
                                    paramIndex += 2;
                                }
                            }
                            else if (!string.IsNullOrEmpty(startDate))
                            {
                                if (DateTime.TryParse(startDate, out DateTime dStart))
                                {
                                    conditions.Add($"CAST({dbColumnName} AS DATE) >= @p{paramIndex}");
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Date) { Value = dStart.Date });
                                    paramIndex++;
                                }
                            }
                            else if (!string.IsNullOrEmpty(endDate))
                            {
                                if (DateTime.TryParse(endDate, out DateTime dEnd))
                                {
                                    conditions.Add($"CAST({dbColumnName} AS DATE) <= @p{paramIndex}");
                                    parameters.Add(new SqlParameter($"@p{paramIndex}", SqlDbType.Date) { Value = dEnd.Date });
                                    paramIndex++;
                                }
                            }
                        }
                    }

                    if (conditions.Any())
                    {
                        baseQuery += " AND " + string.Join(" AND ", conditions);
                    }

                    using (var cmd = new SqlCommand(baseQuery, connection))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            if (dt.Columns.Contains("OdemeSapmasi"))
                            {
                                dt.Columns.Add("OdemeSapmasiFormatted", typeof(string));
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row["OdemeSapmasi"] != DBNull.Value)
                                    {
                                        int val = Convert.ToInt32(row["OdemeSapmasi"]);
                                        row["OdemeSapmasiFormatted"] = val > 0 ? $"-{val}" : val < 0 ? $"+{Math.Abs(val)}" : "0";
                                    }
                                    else
                                    {
                                        row["OdemeSapmasiFormatted"] = "-";
                                    }
                                }
                                dt.Columns.Remove("OdemeSapmasi");
                                dt.Columns["OdemeSapmasiFormatted"].ColumnName = "OdemeSapmasi";
                            }

                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
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