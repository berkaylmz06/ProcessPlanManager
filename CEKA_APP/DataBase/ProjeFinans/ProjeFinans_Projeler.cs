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
                catch (Exception)
                {
                    return false; // Hata mesajını UI katmanına bırak
                }
            }
        }

        public static bool UpdateProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, out bool degisiklikVar)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    ProjeBilgi mevcutBilgi = GetProjeBilgileri(projeNo);
                    if (mevcutBilgi == null)
                    {
                        degisiklikVar = false;
                        return false;
                    }

                    degisiklikVar =
                        (mevcutBilgi.Aciklama ?? "") != (aciklama ?? "") ||
                        (mevcutBilgi.ProjeAdi ?? "") != (projeAdi ?? "") ||
                        mevcutBilgi.OlusturmaTarihi != olusturmaTarihi;

                    if (!degisiklikVar)
                    {
                        return true;
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

                        return true;
                    }
                }
                catch (Exception)
                {
                    degisiklikVar = false;
                    return false; // Hata mesajını UI katmanına bırak
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
        public static bool ProjeSil(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "DELETE FROM ProjeFinans_Projeler WHERE projeNo = @projeNo";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@projeNo", projeNo);
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje silinirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
    }
}
