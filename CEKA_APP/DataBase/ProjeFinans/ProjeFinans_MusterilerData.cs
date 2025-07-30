using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_MusterilerData
    {
        public void MusteriKaydet(Musteriler musteri)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO ProjeFinans_Musteriler
                        (musteriNo, musteriAdi, vergiNo, vergiDairesi, adres , musteriMensei, doviz)
                        VALUES (@musteriNo, @musteriAdi, @vergiNo, @vergiDairesi, @adres, @musteriMensei, @doviz)";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@musteriNo", musteri.musteriNo);
                        cmd.Parameters.AddWithValue("@musteriAdi", musteri.musteriAdi);
                        cmd.Parameters.AddWithValue("@vergiNo", musteri.vergiNo);
                        cmd.Parameters.AddWithValue("@vergiDairesi", musteri.vergiDairesi);
                        cmd.Parameters.AddWithValue("@adres", musteri.adres);
                        cmd.Parameters.AddWithValue("@musteriMensei", musteri.musteriMensei);
                        cmd.Parameters.AddWithValue("@doviz", musteri.doviz);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Müşteri kaydı eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        public bool MusteriNoVarMi(string musteriNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM ProjeFinans_Musteriler WHERE musteriNo = @musteriNo";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@musteriNo", musteriNo);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Müşteri numarası kontrol edilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
        public Musteriler GetMusteriByMusteriNo(string musteriNo)
        {
            Musteriler musteri = null;
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT musteriNo, musteriAdi, vergiNo, vergiDairesi, adres , musteriMensei, doviz FROM ProjeFinans_Musteriler WHERE musteriNo = @musteriNo";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@musteriNo", musteriNo);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                musteri = new Musteriler();
                                musteri.musteriNo = reader["musteriNo"] as string;
                                musteri.musteriAdi = reader["musteriAdi"] as string;
                                musteri.vergiNo = reader["vergiNo"] as string;
                                musteri.vergiDairesi = reader["vergiDairesi"] as string;
                                musteri.adres = reader["adres"] as string;
                                musteri.musteriMensei = reader["musteriMensei"] as string;
                                musteri.doviz = reader["doviz"] as string;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Müşteri bilgileri getirilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
            return musteri;
        }
        public List<Musteriler> GetMusteriler()
        {
            List<Musteriler> musterilerListesi = new List<Musteriler>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT musteriNo, musteriAdi, vergiNo, vergiDairesi, adres , musteriMensei, doviz FROM ProjeFinans_Musteriler ORDER BY musteriAdi";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Musteriler musteri = new Musteriler();
                                musteri.musteriNo = reader["musteriNo"] as string;
                                musteri.musteriAdi = reader["musteriAdi"] as string;
                                musteri.vergiNo = reader["vergiNo"] as string;
                                musteri.vergiDairesi = reader["vergiDairesi"] as string;
                                musteri.adres = reader["adres"] as string;
                                musteri.musteriMensei = reader["musteriMensei"] as string;
                                musteri.doviz = reader["doviz"] as string;

                                musterilerListesi.Add(musteri);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Müşteriler getirilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
            return musterilerListesi;
        }
       
        public void TumMusterileriSil()
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM ProjeFinans_Musteriler";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Tüm müşteriler silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
    }
}