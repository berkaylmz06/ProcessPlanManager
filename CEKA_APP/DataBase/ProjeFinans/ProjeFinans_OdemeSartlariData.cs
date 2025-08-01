using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization; // CultureInfo için eklendi

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_OdemeSartlariData
    {
        public void SaveOrUpdateOdemeBilgi(string projeNo, int kilometreTasiId, int siralama, string oran, string tutar, string tahminiTarih, string gerceklesenTarih, string aciklama, bool teminatMektubu, string teminatDurumu, string durum, string faturaNo, string kalanTutar)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM ProjeFinans_OdemeSartlari WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                    using (var checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = projeNo?.Trim() ?? throw new ArgumentNullException(nameof(projeNo));
                        checkCmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = kilometreTasiId;
                        int existingCount = (int)checkCmd.ExecuteScalar();

                        string query;
                        if (existingCount > 0)
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
                            kalanTutar = @kalanTutar
                        WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                        }
                        else
                        {
                            query = @"
                        INSERT INTO ProjeFinans_OdemeSartlari
                        (projeNo, kilometreTasiId, siralama, oran, tutar, tahminiTarih, gerceklesenTarih, aciklama, teminatMektubu, teminatDurumu, faturaNo, durum, kalanTutar)
                        VALUES (@projeNo, @kilometreTasiId, @siralama, @oran, @tutar, @tahminiTarih, @gerceklesenTarih, @aciklama, @teminatMektubu, @teminatDurumu, @faturaNo, @durum, @kalanTutar)";
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

                            cmd.ExecuteNonQuery();
                        }
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
                        return result != null ? result.ToString() : null;
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
                        o.kalanTutar    
                    FROM ProjeFinans_OdemeSartlari o
                    JOIN ProjeFinans_KilometreTaslari k ON o.kilometreTasiId = k.kilometreTasiId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var odemeSekilleri = new CEKA_APP.Entitys.ProjeFinans.OdemeSartlari
                            {
                                odemeId = Convert.ToInt32(reader["odemeId"]),
                                projeNo = reader["projeNo"].ToString(),
                                kilometreTasiId = Convert.ToInt32(reader["kilometreTasiId"]),
                                kilometreTasiAdi = reader["kilometreTasiAdi"].ToString(),
                                siralama = Convert.ToInt32(reader["siralama"]),
                                oran = Convert.ToDecimal(reader["oran"]),
                                tutar = Convert.ToDecimal(reader["tutar"]),
                                aciklama = reader["aciklama"].ToString(),
                                teminatMektubu = Convert.ToBoolean(reader["teminatMektubu"]),
                                teminatDurumu = reader["teminatDurumu"] != DBNull.Value ? reader["teminatDurumu"].ToString() : null,
                                durum = reader["durum"].ToString(),
                                faturaNo = reader["faturaNo"] != DBNull.Value ? reader["faturaNo"].ToString() : null,
                                kalanTutar = reader["kalanTutar"] != DBNull.Value ? Convert.ToDecimal(reader["kalanTutar"]) : 0
                            };

                            if (reader["tahminiTarih"] != DBNull.Value)
                                odemeSekilleri.tahminiTarih = Convert.ToDateTime(reader["tahminiTarih"]);
                            else
                                odemeSekilleri.tahminiTarih = null;

                            if (reader["gerceklesenTarih"] != DBNull.Value)
                                odemeSekilleri.gerceklesenTarih = Convert.ToDateTime(reader["gerceklesenTarih"]);
                            else
                                odemeSekilleri.gerceklesenTarih = null;

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
                o.kalanTutar
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
                                aciklama = reader["aciklama"].ToString(),
                                teminatMektubu = Convert.ToBoolean(reader["teminatMektubu"]),
                                teminatDurumu = reader["teminatDurumu"] != DBNull.Value ? reader["teminatDurumu"].ToString() : null,
                                durum = reader["durum"].ToString(),
                                faturaNo = reader["faturaNo"] != DBNull.Value ? reader["faturaNo"].ToString() : null,
                                kalanTutar = reader["kalanTutar"] != DBNull.Value ? Convert.ToDecimal(reader["kalanTutar"]) : Convert.ToDecimal(reader["tutar"])
                            };

                            if (reader["tahminiTarih"] != DBNull.Value)
                                odemeSekilleri.tahminiTarih = Convert.ToDateTime(reader["tahminiTarih"]);
                            else
                                odemeSekilleri.tahminiTarih = null;

                            if (reader["gerceklesenTarih"] != DBNull.Value)
                                odemeSekilleri.gerceklesenTarih = Convert.ToDateTime(reader["gerceklesenTarih"]);
                            else
                                odemeSekilleri.gerceklesenTarih = null;

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
                        o.kalanTutar
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
                                aciklama = reader["aciklama"].ToString(),
                                teminatMektubu = Convert.ToBoolean(reader["teminatMektubu"]),
                                teminatDurumu = reader["teminatDurumu"] != DBNull.Value ? reader["teminatDurumu"].ToString() : null,
                                durum = reader["durum"].ToString(),
                                faturaNo = reader["faturaNo"] != DBNull.Value ? reader["faturaNo"].ToString() : null,
                                kalanTutar = reader["kalanTutar"] != DBNull.Value ? Convert.ToDecimal(reader["kalanTutar"]) : 0
                            };

                            if (reader["tahminiTarih"] != DBNull.Value)
                                odemeSekilleri.tahminiTarih = Convert.ToDateTime(reader["tahminiTarih"]);
                            else
                                odemeSekilleri.tahminiTarih = null;

                            if (reader["gerceklesenTarih"] != DBNull.Value)
                                odemeSekilleri.gerceklesenTarih = Convert.ToDateTime(reader["gerceklesenTarih"]);
                            else
                                odemeSekilleri.gerceklesenTarih = null;

                            return odemeSekilleri;
                        }
                        return null;
                    }
                }
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
        public dynamic GetOdemeBilgiByOdemeId(int odemeId)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM ProjeFinans_OdemeSartlari WHERE odemeId = @odemeId";
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
                                    kalanTutar = reader.IsDBNull(reader.GetOrdinal("kalanTutar")) ? 0 : reader.GetDecimal(reader.GetOrdinal("kalanTutar"))
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
    }
}