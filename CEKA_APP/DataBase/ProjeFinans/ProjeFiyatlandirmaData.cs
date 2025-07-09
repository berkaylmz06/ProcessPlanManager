using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFiyatlandirmaData
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
                    if (altProjeler != null && altProjeler.Any())
                    {
                        foreach (var altProje in altProjeler)
                        {
                            string query = @"
                                SELECT SUM(teklifBirimMiktar * teklifBirimFiyat) as ToplamTeklif
                                FROM ProjeFinans_Fiyatlandirma
                                WHERE projeNo = @projeNo AND teklifBirimMiktar IS NOT NULL AND teklifBirimFiyat IS NOT NULL";
                            using (var cmd = new SqlCommand(query, connection))
                            {
                                cmd.Parameters.AddWithValue("@projeNo", altProje);
                                object result = cmd.ExecuteScalar();
                                if (result != DBNull.Value && result != null)
                                {
                                    toplamBedel += Convert.ToDecimal(result);
                                }
                                else
                                {
                                    eksikFiyatlandirmaProjeler.Add(altProje);
                                }
                            }
                        }
                    }
                    else
                    {
                        string query = @"
                            SELECT SUM(teklifBirimMiktar * teklifBirimFiyat) as ToplamTeklif
                            FROM ProjeFinans_Fiyatlandirma
                            WHERE projeNo = @projeNo AND teklifBirimMiktar IS NOT NULL AND teklifBirimFiyat IS NOT NULL";
                        using (var cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@projeNo", projeNo);
                            object result = cmd.ExecuteScalar();
                            if (result != DBNull.Value && result != null)
                            {
                                toplamBedel += Convert.ToDecimal(result);
                            }
                            else
                            {
                                eksikFiyatlandirmaProjeler.Add(projeNo);
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
    }
}