// ProjeFinans_SevkiyatData.cs
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
        public List<(string paketAdi, string sevkId, string tasimaBilgileri, string satisSipNo, string irsaliyeNo, DateTime aracSevkTarihi, decimal agirlik, decimal faturaToplami, string faturaNo)> GetSevkiyatByProje(string projeNo)
        {
            var result = new List<(string, string, string, string, string, DateTime, decimal, decimal, string)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"SELECT psp.paketAdi, ps.sevkId, ps.tasimaBilgileri, ps.satisSipNo, ps.irsaliyeNo, ps.aracSevkTarihi, ps.agirlik, ps.faturaToplami, ps.faturaNo
                               FROM ProjeFinans_Sevkiyat ps
                               JOIN ProjeFinans_SevkiyatPaketleri psp ON ps.paketId = psp.paketId
                               WHERE ps.projeNo = @projeNo";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeNo", projeNo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add((
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


        public void SevkiyatKaydet(string projeNo, string sevkId, int paketId, string tasimaBilgileri, string satisSipNo, string irsaliyeNo, DateTime aracSevkTarihi, decimal agirlik, decimal faturaToplami, string faturaNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO ProjeFinans_Sevkiyat
                        (projeNo, sevkId, paketId, tasimaBilgileri, satisSipNo, irsaliyeNo, aracSevkTarihi, agirlik, faturaToplami, faturaNo)
                        VALUES (@projeNo, @sevkId, @paketId, @tasimaBilgileri, @satisSipNo, @irsaliyeNo, @aracSevkTarihi, @agirlik, @faturaToplami, @faturaNo)";
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
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sevkiyat kaydı eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SevkiyatGuncelle(string projeNo, string sevkId, int paketId, string tasimaBilgileri, string satisSipNo, string irsaliyeNo, DateTime aracSevkTarihi, decimal agirlik, decimal faturaToplami, string faturaNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE ProjeFinans_Sevkiyat
                        SET paketId = @paketId,  -- PaketId de güncellenebilir
                            tasimaBilgileri = @tasimaBilgileri,
                            satisSipNo = @satisSipNo,
                            irsaliyeNo = @irsaliyeNo,
                            aracSevkTarihi = @aracSevkTarihi,
                            agirlik = @agirlik,
                            faturaToplami = @faturaToplami,
                            faturaNo = @faturaNo
                        WHERE projeNo = @projeNo AND sevkId = @sevkId"; // Güncelleme koşulu sevkId olmalı
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@projeNo", projeNo);
                        cmd.Parameters.AddWithValue("@sevkId", sevkId); // WHERE koşulu için
                        cmd.Parameters.AddWithValue("@paketId", paketId); // SET için
                        cmd.Parameters.AddWithValue("@tasimaBilgileri", tasimaBilgileri);
                        cmd.Parameters.AddWithValue("@satisSipNo", satisSipNo);
                        cmd.Parameters.AddWithValue("@irsaliyeNo", irsaliyeNo);
                        cmd.Parameters.AddWithValue("@aracSevkTarihi", aracSevkTarihi);
                        cmd.Parameters.AddWithValue("@agirlik", agirlik);
                        cmd.Parameters.AddWithValue("@faturaToplami", faturaToplami);
                        cmd.Parameters.AddWithValue("@faturaNo", faturaNo);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sevkiyat bilgisi güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}