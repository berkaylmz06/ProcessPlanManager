using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_FiyatlandirmaData
    {
        public List<(int fiyatlandirmaKalemId, string kalemAdi, decimal teklifBirimMiktar, decimal teklifBirimFiyat, decimal gerceklesenBirimMiktar, decimal gerceklesenBirimFiyat)> GetFiyatlandirmaByProje(string projeNo)
        {
            var result = new List<(int, string, decimal, decimal, decimal, decimal)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"SELECT f.fiyatlandirmaKalemId, i.kalemAdi, f.teklifBirimMiktar, f.teklifBirimFiyat, f.gerceklesenBirimMiktar, f.gerceklesenBirimFiyat
                                FROM ProjeFinans_Fiyatlandirma f
                                JOIN ProjeFinans_FiyatlandirmaKalemleri i ON f.fiyatlandirmaKalemId = i.fiyatlandirmaKalemId
                                WHERE f.projeNo = @projeNo";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeNo", projeNo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add((
                                Convert.ToInt32(reader["fiyatlandirmaKalemId"]),
                                reader["kalemAdi"].ToString(),
                                Convert.ToDecimal(reader["teklifBirimMiktar"]),
                                Convert.ToDecimal(reader["teklifBirimFiyat"]),
                                Convert.ToDecimal(reader["gerceklesenBirimMiktar"]),
                                Convert.ToDecimal(reader["gerceklesenBirimFiyat"])
                            ));
                        }
                    }
                }
            }
            return result;
        }

        public void FiyatlandirmaKaydet(string projeNo, int fiyatlandirmaKalemId, decimal teklifBirimMiktar, decimal teklifBirimFiyat, decimal gerceklesenBirimMiktar, decimal gerceklesenBirimFiyat)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO ProjeFinans_Fiyatlandirma 
                        (projeNo, fiyatlandirmaKalemId, teklifBirimMiktar, teklifBirimFiyat, gerceklesenBirimMiktar, gerceklesenBirimFiyat)
                        VALUES (@projeNo, @fiyatlandirmaKalemId, @teklifBirimMiktar, @teklifBirimFiyat, @gerceklesenBirimMiktar, @gerceklesenBirimFiyat)";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@projeNo", projeNo);
                        cmd.Parameters.AddWithValue("@fiyatlandirmaKalemId", fiyatlandirmaKalemId);
                        cmd.Parameters.AddWithValue("@teklifBirimMiktar", teklifBirimMiktar);
                        cmd.Parameters.AddWithValue("@teklifBirimFiyat", teklifBirimFiyat);
                        cmd.Parameters.AddWithValue("@gerceklesenBirimMiktar", gerceklesenBirimMiktar);
                        cmd.Parameters.AddWithValue("@gerceklesenBirimFiyat", gerceklesenBirimFiyat);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex) when (ex.Number == 547)
                {
                    MessageBox.Show($"Proje '{projeNo}' için proje bilgileri kaydedilmelidir. Lütfen önce proje bilgilerini kaydedin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fiyatlandırma kaydı eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void FiyatlandirmaGuncelle(string projeNo, int fiyatlandirmaKalemId, decimal teklifBirimMiktar, decimal teklifBirimFiyat, decimal gerceklesenBirimMiktar, decimal gerceklesenBirimFiyat)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE ProjeFinans_Fiyatlandirma 
                        SET teklifBirimMiktar = @teklifBirimMiktar, 
                            teklifBirimFiyat = @teklifBirimFiyat, 
                            gerceklesenBirimMiktar = @gerceklesenBirimMiktar, 
                            gerceklesenBirimFiyat = @gerceklesenBirimFiyat
                        WHERE projeNo = @projeNo AND fiyatlandirmaKalemId = @fiyatlandirmaKalemId";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@projeNo", projeNo);
                        cmd.Parameters.AddWithValue("@fiyatlandirmaKalemId", fiyatlandirmaKalemId);
                        cmd.Parameters.AddWithValue("@teklifBirimMiktar", teklifBirimMiktar);
                        cmd.Parameters.AddWithValue("@teklifBirimFiyat", teklifBirimFiyat);
                        cmd.Parameters.AddWithValue("@gerceklesenBirimMiktar", gerceklesenBirimMiktar);
                        cmd.Parameters.AddWithValue("@gerceklesenBirimFiyat", gerceklesenBirimFiyat);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fiyatlandırma güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public (decimal toplamBedel, List<string> eksikFiyatlandirmaProjeler) GetToplamBedel(string projeNo, List<string> altProjeler = null)
        {
            decimal toplamBedel = 0;
            var eksikFiyatlandirmaProjeler = new List<string>();

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    var projelerToControl = new List<string>();
                    if (altProjeler != null && altProjeler.Any())
                    {
                        projelerToControl = altProjeler;
                    }
                    else
                    {
                        projelerToControl.Add(projeNo);
                    }

                    foreach (var proje in projelerToControl)
                    {
                        // Önce proje için herhangi bir fiyatlandırma kaydının olup olmadığını kontrol et
                        string checkQuery = "SELECT COUNT(*) FROM ProjeFinans_Fiyatlandirma WHERE projeNo = @projeNo";
                        using (var checkCmd = new SqlCommand(checkQuery, connection))
                        {
                            checkCmd.Parameters.AddWithValue("@projeNo", proje);
                            int count = (int)checkCmd.ExecuteScalar();
                            if (count == 0)
                            {
                                eksikFiyatlandirmaProjeler.Add(proje);
                                continue;
                            }
                        }

                        // Eğer kayıt varsa, toplam bedeli hesapla
                        string sumQuery = @"
                            SELECT SUM(teklifBirimMiktar * teklifBirimFiyat) as ToplamTeklif
                            FROM ProjeFinans_Fiyatlandirma
                            WHERE projeNo = @projeNo AND teklifBirimMiktar IS NOT NULL AND teklifBirimFiyat IS NOT NULL";
                        using (var sumCmd = new SqlCommand(sumQuery, connection))
                        {
                            sumCmd.Parameters.AddWithValue("@projeNo", proje);
                            object result = sumCmd.ExecuteScalar();
                            if (result != DBNull.Value && result != null)
                            {
                                toplamBedel += Convert.ToDecimal(result);
                            }
                            else
                            {
                                // Eğer kayıt olmasına rağmen toplam bedel NULL dönüyorsa, bu da bir hata durumudur.
                                eksikFiyatlandirmaProjeler.Add(proje);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Toplam bedel hesaplanırken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return (toplamBedel, eksikFiyatlandirmaProjeler);
        }

        public bool FiyatlandirmaSil(string projeNo)
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
                            string query = @"
                        DELETE FROM ProjeFinans_Fiyatlandirma
                        WHERE projeNo = @projeNo";
                            using (var cmd = new SqlCommand(query, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Fiyatlandırma kayıtları silinirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}