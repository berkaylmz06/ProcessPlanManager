using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_OdemeSekilleriData
    {
        public void SaveOrUpdateOdemeBilgi(string projeNo, string kilometreTasiId, int siralama, string oran, string tutar, string tahminiTarih, string gerceklesenTarih, string aciklama, bool teminatMektubu, string teminatDurumu, string durum)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    // Kaydın varlığını kontrol et
                    string checkQuery = "SELECT COUNT(*) FROM ProjeFinans_OdemeSekilleri WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                    using (var checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = projeNo?.Trim() ?? throw new ArgumentNullException(nameof(projeNo));
                        checkCmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = int.Parse(kilometreTasiId);
                        int existingCount = (int)checkCmd.ExecuteScalar();

                        string query;
                        if (existingCount > 0)
                        {
                            // Kayıt varsa güncelle
                            query = @"
                                UPDATE ProjeFinans_OdemeSekilleri
                                SET siralama = @siralama,
                                    oran = @oran,
                                    tutar = @tutar,
                                    tahminiTarih = @tahminiTarih,
                                    gerceklesenTarih = @gerceklesenTarih,
                                    aciklama = @aciklama,
                                    teminatMektubu = @teminatMektubu,
                                    teminatDurumu = @teminatDurumu,
                                    durum = @durum
                                WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
                        }
                        else
                        {
                            // Kayıt yoksa ekle
                            query = @"
                                INSERT INTO ProjeFinans_OdemeSekilleri
                                (projeNo, kilometreTasiId, siralama, oran, tutar, tahminiTarih, gerceklesenTarih, aciklama, teminatMektubu, teminatDurumu, durum)
                                VALUES (@projeNo, @kilometreTasiId, @siralama, @oran, @tutar, @tahminiTarih, @gerceklesenTarih, @aciklama, @teminatMektubu, @teminatDurumu, @durum)";
                        }

                        using (var cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = projeNo?.Trim() ?? throw new ArgumentNullException(nameof(projeNo));
                            cmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = int.Parse(kilometreTasiId);
                            cmd.Parameters.Add("@siralama", SqlDbType.Int).Value = siralama;
                            cmd.Parameters.Add("@oran", SqlDbType.Decimal).Value = decimal.Parse(oran, System.Globalization.CultureInfo.InvariantCulture);
                            cmd.Parameters.Add("@tutar", SqlDbType.Decimal).Value = decimal.Parse(tutar, System.Globalization.CultureInfo.InvariantCulture);
                            cmd.Parameters.Add("@tahminiTarih", SqlDbType.DateTime2).Value = string.IsNullOrWhiteSpace(tahminiTarih) ? (object)DBNull.Value : DateTime.Parse(tahminiTarih);
                            cmd.Parameters.Add("@gerceklesenTarih", SqlDbType.DateTime2).Value = string.IsNullOrWhiteSpace(gerceklesenTarih) ? (object)DBNull.Value : DateTime.Parse(gerceklesenTarih);
                            cmd.Parameters.Add("@aciklama", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(aciklama) ? (object)DBNull.Value : aciklama;
                            cmd.Parameters.Add("@teminatMektubu", SqlDbType.Bit).Value = teminatMektubu;
                            cmd.Parameters.Add("@teminatDurumu", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(teminatDurumu) ? (object)DBNull.Value : teminatDurumu;
                            cmd.Parameters.Add("@durum", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(durum) ? (object)DBNull.Value : durum;

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
        public List<OdemeSekilleri> GetOdemeBilgileri()
        {
            var odemeBilgileriList = new List<OdemeSekilleri>();
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
                        o.durum
                    FROM ProjeFinans_OdemeSekilleri o
                    JOIN ProjeFinans_KilometreTaslari k ON o.kilometreTasiId = k.kilometreTasiId";


                using (var cmd = new SqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var odemeSekilleri = new CEKA_APP.Entitys.ProjeFinans.OdemeSekilleri
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
                                teminatDurumu = reader["teminatMektubu"] != DBNull.Value ? reader["teminatMektubu"].ToString() : null,
                                durum = reader["durum"].ToString()
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
        public List<OdemeSekilleri> GetOdemeBilgileriByProjeNo(string projeNo)
        {
            var odemeBilgileriList = new List<OdemeSekilleri>();
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
                        o.durum
                    FROM ProjeFinans_OdemeSekilleri o
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
                            var odemeSekilleri = new CEKA_APP.Entitys.ProjeFinans.OdemeSekilleri
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
                                durum = reader["durum"].ToString()
                            };

                            if (reader["tahminiTarih"] != DBNull.Value)
                                odemeSekilleri.tahminiTarih = Convert.ToDateTime(reader["tahminiTarih"]);

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

        public void UpdateOrder(List<Tuple<string, int, int>> reorderedData) // Tuple: ProjeNo, KilometreTasiId, NewSiralama
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE ProjeFinans_OdemeSekilleri
                        SET siralama = @newSiralama
                        WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";

                    foreach (var item in reorderedData)
                    {
                        using (var cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.Add("@newSiralama", SqlDbType.Int).Value = item.Item3;
                            cmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = item.Item1;
                            cmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = item.Item2;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sıralama güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
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
                    string query = "DELETE FROM ProjeFinans_OdemeSekilleri WHERE projeNo = @projeNo AND kilometreTasiId = @kilometreTasiId";
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
    }
}