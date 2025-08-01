using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_Projeler
    {
        public static bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string sorgu = @"
                        INSERT INTO ProjeFinans_Projeler (projeNo, aciklama, projeAdi, olusturmaTarihi)
                        VALUES (@projeNo, @aciklama, @projeAdi, @olusturmaTarihi)";

                    using (SqlCommand komut = new SqlCommand(sorgu, conn))
                    {
                        komut.Parameters.AddWithValue("@projeNo", projeNo);
                        komut.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                        komut.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                        komut.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                        komut.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje finans kaydı eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public static bool UpdateProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    ProjeBilgi mevcutBilgi = GetProjeBilgileri(projeNo);
                    if (mevcutBilgi == null)
                    {
                        MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    bool degisiklikVar =
                        (mevcutBilgi.Aciklama ?? "") != (aciklama ?? "") ||
                        (mevcutBilgi.ProjeAdi ?? "") != (projeAdi ?? "") ||
                        mevcutBilgi.OlusturmaTarihi != olusturmaTarihi;

                    if (!degisiklikVar)
                    {
                        MessageBox.Show("Herhangi bir değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    string sorgu = @"
                        UPDATE ProjeFinans_Projeler
                        SET aciklama = @aciklama, 
                            projeAdi = @projeAdi, 
                            olusturmaTarihi = @olusturmaTarihi
                        WHERE projeNo = @projeNo";

                    using (SqlCommand command = new SqlCommand(sorgu, connection))
                    {
                        command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                        command.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                        command.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                        command.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                        command.ExecuteNonQuery();

                        MessageBox.Show($"Proje '{projeNo}' kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje finans güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public static ProjeBilgi GetProjeBilgileri(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string sql = @"
                SELECT projeNo, projeAdi, aciklama, olusturmaTarihi
                FROM ProjeFinans_Projeler
                WHERE projeNo = @projeNo";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                        Console.WriteLine($"SQL Sorgusu için projeNo: '{projeNo}'");
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Console.WriteLine("Kayıt bulundu.");
                                return new ProjeBilgi
                                {
                                    ProjeNo = reader.IsDBNull(0) ? null : reader.GetString(0),
                                    ProjeAdi = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    Aciklama = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    OlusturmaTarihi = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3)
                                };
                            }
                            else
                            {
                                Console.WriteLine("Kayıt bulunamadı.");
                            }
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                    MessageBox.Show($"Proje bilgileri alınırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }
    }
}
