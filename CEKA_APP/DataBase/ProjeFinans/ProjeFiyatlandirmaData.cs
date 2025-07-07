using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFiyatlandirmaData
    {
        public List<(int fiyatlandirmaKalemId, string kalemAdi, decimal teklifBirimMiktar, decimal teklifBirimFiyat, decimal teklifToplam, decimal gerceklesenBirimMiktar, decimal gerceklesenBirimFiyat, decimal gerceklesenMaliyet)> GetFiyatlandirmaByProje(string projeNo)
        {
            var result = new List<(int, string, decimal, decimal, decimal, decimal, decimal, decimal)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"SELECT f.fiyatlandirmaKalemId, i.kalemAdi, f.teklifBirimMiktar, f.teklifBirimFiyat, f.teklifToplam, f.gerceklesenBirimMiktar, f.gerceklesenBirimFiyat, f.gerceklesenMaliyet
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
                                Convert.ToDecimal(reader["teklifToplam"]),
                                Convert.ToDecimal(reader["gerceklesenBirimMiktar"]),
                                Convert.ToDecimal(reader["gerceklesenBirimFiyat"]),
                                Convert.ToDecimal(reader["gerceklesenMaliyet"])
                            ));
                        }
                    }
                }
            }
            return result;
        }

        public void FiyatlandirmaKaydet(string projeNo, int fiyatlandirmaKalemId, decimal teklifBirimMiktar, decimal teklifBirimFiyat, decimal teklifToplam, decimal gerceklesenBirimMiktar, decimal gerceklesenBirimFiyat, decimal gerceklesenMaliyet)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO ProjeFinans_Fiyatlandirma 
                        (projeNo, fiyatlandirmaKalemId, teklifBirimMiktar, teklifBirimFiyat, teklifToplam, gerceklesenBirimMiktar, gerceklesenBirimFiyat, gerceklesenMaliyet)
                        VALUES (@projeNo, @fiyatlandirmaKalemId, @teklifBirimMiktar, @teklifBirimFiyat, @teklifToplam, @gerceklesenBirimMiktar, @gerceklesenBirimFiyat, @gerceklesenMaliyet)";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@projeNo", projeNo);
                        cmd.Parameters.AddWithValue("@fiyatlandirmaKalemId", fiyatlandirmaKalemId);
                        cmd.Parameters.AddWithValue("@teklifBirimMiktar", teklifBirimMiktar);
                        cmd.Parameters.AddWithValue("@teklifBirimFiyat", teklifBirimFiyat);
                        cmd.Parameters.AddWithValue("@teklifToplam", teklifToplam);
                        cmd.Parameters.AddWithValue("@gerceklesenBirimMiktar", gerceklesenBirimMiktar);
                        cmd.Parameters.AddWithValue("@gerceklesenBirimFiyat", gerceklesenBirimFiyat);
                        cmd.Parameters.AddWithValue("@gerceklesenMaliyet", gerceklesenMaliyet);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex) when (ex.Number == 547) // Foreign key violation
                {
                    MessageBox.Show($"Proje '{projeNo}' için proje bilgileri kaydedilmelidir. Lütfen önce proje bilgilerini kaydedin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fiyatlandırma kaydı eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void FiyatlandirmaGuncelle(string projeNo, int fiyatlandirmaKalemId, decimal teklifBirimMiktar, decimal teklifBirimFiyat, decimal teklifToplam, decimal gerceklesenBirimMiktar, decimal gerceklesenBirimFiyat, decimal gerceklesenMaliyet)
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
                            teklifToplam = @teklifToplam, 
                            gerceklesenBirimMiktar = @gerceklesenBirimMiktar, 
                            gerceklesenBirimFiyat = @gerceklesenBirimFiyat, 
                            gerceklesenMaliyet = @gerceklesenMaliyet
                        WHERE projeNo = @projeNo AND fiyatlandirmaKalemId = @fiyatlandirmaKalemId";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@projeNo", projeNo);
                        cmd.Parameters.AddWithValue("@fiyatlandirmaKalemId", fiyatlandirmaKalemId);
                        cmd.Parameters.AddWithValue("@teklifBirimMiktar", teklifBirimMiktar);
                        cmd.Parameters.AddWithValue("@teklifBirimFiyat", teklifBirimFiyat);
                        cmd.Parameters.AddWithValue("@teklifToplam", teklifToplam);
                        cmd.Parameters.AddWithValue("@gerceklesenBirimMiktar", gerceklesenBirimMiktar);
                        cmd.Parameters.AddWithValue("@gerceklesenBirimFiyat", gerceklesenBirimFiyat);
                        cmd.Parameters.AddWithValue("@gerceklesenMaliyet", gerceklesenMaliyet);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fiyatlandırma güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public decimal GetToplamBedel(string projeNo, List<string> altProjeler = null)
        {
            decimal toplamBedel = 0;
            var projeler = new List<string> { projeNo };
            if (altProjeler != null)
                projeler.AddRange(altProjeler);

            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT SUM(teklifToplam) as ToplamTeklif
                    FROM ProjeFinans_Fiyatlandirma
                    WHERE projeNo IN ({0})";
                string projeNoParams = string.Join(",", projeler.Select((_, index) => $"@projeNo{index}"));
                query = string.Format(query, projeNoParams);

                using (var cmd = new SqlCommand(query, connection))
                {
                    for (int i = 0; i < projeler.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@projeNo{i}", projeler[i]);
                    }
                    object result = cmd.ExecuteScalar();
                    toplamBedel = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
            return toplamBedel;
        }
    }
}