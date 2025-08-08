using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_SevkiyatData
    {
        public List<(int aracSira, string paketAdi, string sevkId, string tasimaBilgileri, string satisSipNo, string irsaliyeNo, DateTime aracSevkTarihi, decimal agirlik, decimal faturaToplami, string faturaNo)> GetSevkiyatByProje(string projeNo)
        {
            var result = new List<(int, string, string, string, string, string, DateTime, decimal, decimal, string)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"SELECT ps.AracSira, psp.paketAdi, ps.sevkId, ps.tasimaBilgileri, ps.satisSipNo, ps.irsaliyeNo, ps.aracSevkTarihi, ps.agirlik, ps.faturaToplami, ps.faturaNo
                               FROM ProjeFinans_Sevkiyat ps
                               JOIN ProjeFinans_SevkiyatPaketleri psp ON ps.paketId = psp.paketId
                               WHERE ps.projeNo = @projeNo ORDER BY ps.AracSira ASC";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeNo", projeNo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add((
                                reader["AracSira"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AracSira"]),
                                reader["paketAdi"].ToString(),
                                reader["sevkId"].ToString(),
                                reader["tasimaBilgileri"].ToString(),
                                reader["satisSipNo"].ToString(),
                                reader["irsaliyeNo"].ToString(),
                                Convert.ToDateTime(reader["aracSevkTarihi"]),
                                Convert.ToDecimal(reader["agirlik"]),
                                Convert.ToDecimal(reader["faturaToplami"]),
                                reader["faturaNo"].ToString()
                            ));
                        }
                    }
                }
            }
            return result;
        }

        public void SevkiyatKaydet(string projeNo, string sevkId, int paketId, string tasimaBilgileri, string satisSipNo, string irsaliyeNo, DateTime aracSevkTarihi, decimal agirlik, decimal faturaToplami, string faturaNo, int aracSira)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO ProjeFinans_Sevkiyat
                        (projeNo, sevkId, paketId, tasimaBilgileri, satisSipNo, irsaliyeNo, aracSevkTarihi, agirlik, faturaToplami, faturaNo, AracSira)
                        VALUES (@projeNo, @sevkId, @paketId, @tasimaBilgileri, @satisSipNo, @irsaliyeNo, @aracSevkTarihi, @agirlik, @faturaToplami, @faturaNo, @aracSira)";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@projeNo", projeNo);
                        cmd.Parameters.AddWithValue("@paketId", paketId);
                        cmd.Parameters.AddWithValue("@sevkId", sevkId);
                        cmd.Parameters.AddWithValue("@tasimaBilgileri", tasimaBilgileri);
                        cmd.Parameters.AddWithValue("@satisSipNo", satisSipNo);
                        cmd.Parameters.AddWithValue("@irsaliyeNo", irsaliyeNo);
                        cmd.Parameters.AddWithValue("@aracSevkTarihi", aracSevkTarihi);
                        cmd.Parameters.AddWithValue("@agirlik", agirlik);
                        cmd.Parameters.AddWithValue("@faturaToplami", faturaToplami);
                        cmd.Parameters.AddWithValue("@faturaNo", faturaNo);
                        cmd.Parameters.AddWithValue("@aracSira", aracSira);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sevkiyat kaydı eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SevkiyatGuncelle(string projeNo, string sevkId, int paketId, string tasimaBilgileri, string satisSipNo, string irsaliyeNo, DateTime aracSevkTarihi, decimal agirlik, decimal faturaToplami, string faturaNo, int aracSira)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE ProjeFinans_Sevkiyat
                        SET paketId = @paketId,
                            tasimaBilgileri = @tasimaBilgileri,
                            satisSipNo = @satisSipNo,
                            irsaliyeNo = @irsaliyeNo,
                            aracSevkTarihi = @aracSevkTarihi,
                            agirlik = @agirlik,
                            faturaToplami = @faturaToplami,
                            faturaNo = @faturaNo,
                            AracSira = @aracSira -- Yeni: AracSira da güncellenebilir
                        WHERE projeNo = @projeNo AND sevkId = @sevkId";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@projeNo", projeNo);
                        cmd.Parameters.AddWithValue("@sevkId", sevkId);
                        cmd.Parameters.AddWithValue("@paketId", paketId);
                        cmd.Parameters.AddWithValue("@tasimaBilgileri", tasimaBilgileri);
                        cmd.Parameters.AddWithValue("@satisSipNo", satisSipNo);
                        cmd.Parameters.AddWithValue("@irsaliyeNo", irsaliyeNo);
                        cmd.Parameters.AddWithValue("@aracSevkTarihi", aracSevkTarihi);
                        cmd.Parameters.AddWithValue("@agirlik", agirlik);
                        cmd.Parameters.AddWithValue("@faturaToplami", faturaToplami);
                        cmd.Parameters.AddWithValue("@faturaNo", faturaNo);
                        cmd.Parameters.AddWithValue("@aracSira", aracSira); 
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sevkiyat bilgisi güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public bool SevkiyatSil(string projeNo)
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
                                DELETE FROM ProjeFinans_Sevkiyat
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
                            MessageBox.Show($"Sevkiyat silinirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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