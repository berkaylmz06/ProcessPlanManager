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
                        (musteriNo, musteriAdi, musteriMensei, vergiNo, vergiDairesi, adres, olusturmaTarihi)
                        VALUES (@musteriNo, @musteriAdi, @musteriMensei, @vergiNo, @vergiDairesi, @adres, GETDATE())";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@musteriNo", musteri.musteriNo);
                        cmd.Parameters.AddWithValue("@musteriAdi", musteri.musteriAdi);
                        cmd.Parameters.AddWithValue("@musteriMensei", musteri.musteriMensei);
                        cmd.Parameters.AddWithValue("@vergiNo", musteri.vergiNo);
                        cmd.Parameters.AddWithValue("@vergiDairesi", musteri.vergiDairesi);
                        cmd.Parameters.AddWithValue("@adres", musteri.adres);

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
                    string query = "SELECT musteriNo, musteriAdi, musteriMensei, vergiNo, vergiDairesi, adres, olusturmaTarihi FROM ProjeFinans_Musteriler WHERE musteriNo = @musteriNo";
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
                                musteri.musteriMensei = reader["musteriMensei"] as string;
                                musteri.vergiNo = reader["vergiNo"] as string;
                                musteri.vergiDairesi = reader["vergiDairesi"] as string;
                                musteri.adres = reader["adres"] as string;
                                musteri.olusturmaTarihi = reader.GetDateTime(reader.GetOrdinal("olusturmaTarihi"));
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
                    string query = "SELECT musteriNo, musteriAdi, musteriMensei, vergiNo, vergiDairesi, adres, olusturmaTarihi FROM ProjeFinans_Musteriler ORDER BY musteriAdi";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Musteriler musteri = new Musteriler();
                                musteri.musteriNo = reader["musteriNo"] as string;
                                musteri.musteriAdi = reader["musteriAdi"] as string;
                                musteri.musteriMensei = reader["musteriMensei"] as string;
                                musteri.vergiNo = reader["vergiNo"] as string;
                                musteri.vergiDairesi = reader["vergiDairesi"] as string;
                                musteri.adres = reader["adres"] as string;
                                musteri.olusturmaTarihi = reader.GetDateTime(reader.GetOrdinal("olusturmaTarihi"));

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
        public void MusteriGuncelle(string eskiMusteriNo, Musteriler guncelMusteri)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE ProjeFinans_Musteriler
                        SET musteriNo = @yeniMusteriNo,
                            musteriAdi = @musteriAdi,
                            musteriMensei = @musteriMensei,
                            vergiNo = @vergiNo,
                            vergiDairesi = @vergiDairesi,
                            adres = @adres
                        WHERE musteriNo = @eskiMusteriNo";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@yeniMusteriNo", guncelMusteri.musteriNo);
                        cmd.Parameters.AddWithValue("@musteriAdi", guncelMusteri.musteriAdi);
                        cmd.Parameters.AddWithValue("@musteriMensei", guncelMusteri.musteriMensei);
                        cmd.Parameters.AddWithValue("@vergiNo", guncelMusteri.vergiNo);
                        cmd.Parameters.AddWithValue("@vergiDairesi", guncelMusteri.vergiDairesi);
                        cmd.Parameters.AddWithValue("@adres", guncelMusteri.adres);
                        cmd.Parameters.AddWithValue("@eskiMusteriNo", eskiMusteriNo); 

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Müşteri güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
        public void MusteriSil(string musteriNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM ProjeFinans_Musteriler WHERE musteriNo = @musteriNo";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@musteriNo", musteriNo);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Müşteri silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
    }
}