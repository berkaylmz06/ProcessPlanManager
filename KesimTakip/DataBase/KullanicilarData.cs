using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using KesimTakip.Entitys;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace KesimTakip.DataBase
{
    class KullanicilarData
    {
        public Kullanicilar GirisYap(string kullaniciAdi, string sifre)
        {
            Kullanicilar kullanici = null;

            string sql = @"
                SELECT id, adSoyad, kullaniciAdi, sifre, kullaniciRol, email 
                FROM Kullanicilar 
                WHERE kullaniciAdi = @kullaniciAdi";

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                kullanici = new Kullanicilar
                                {
                                    id = reader.GetInt32(0),
                                    adSoyad = reader.GetString(1),
                                    kullaniciAdi = reader.GetString(2),
                                    sifre = reader.GetString(3),
                                    kullaniciRol = reader.GetString(4),
                                    email = reader.GetString(5)
                                };
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Giriş işlemi sırasında bir hata oluştu.", ex);
                }
            }

            return kullanici;
        }
        public static DataTable GetKullaniciListesi()
        {
            string query = @"
                SELECT id, adSoyad, kullaniciAdi, sifre, kullaniciRol, email 
                FROM Kullanicilar 
                ORDER BY adSoyad";

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Kullanıcı listesi alınırken bir hata oluştu.", ex);
                }
            }
        }
        public static void KullaniciEkle(Kullanicilar kullanici)
        {
            if (kullanici == null || string.IsNullOrEmpty(kullanici.kullaniciAdi) || string.IsNullOrEmpty(kullanici.sifre))
                throw new ArgumentException("Kullanıcı bilgileri eksik.");

            string kontrolQuery = "SELECT COUNT(*) FROM Kullanicilar WHERE kullaniciAdi = @kullaniciAdi OR email = @email";
            string insertQuery = @"
                INSERT INTO Kullanicilar 
                (adSoyad, kullaniciAdi, sifre, kullaniciRol, email) 
                VALUES 
                (@adSoyad, @kullaniciAdi, @sifre, @kullaniciRol, @email)";

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    using (var kontrolCmd = new SqlCommand(kontrolQuery, connection))
                    {
                        kontrolCmd.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);
                        kontrolCmd.Parameters.AddWithValue("@email", kullanici.email ?? (object)DBNull.Value);
                        int mevcutKullanici = (int)kontrolCmd.ExecuteScalar();

                        if (mevcutKullanici > 0)
                            throw new Exception("Bu kullanıcı adı veya e-posta zaten kullanılıyor.");
                    }

                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@adSoyad", kullanici.adSoyad ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);
                        command.Parameters.AddWithValue("@sifre", kullanici.sifre);
                        command.Parameters.AddWithValue("@kullaniciRol", kullanici.kullaniciRol ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@email", kullanici.email ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Kullanıcı ekleme sırasında bir hata oluştu.", ex);
                }
            }
        }
        public static bool KullaniciAdiVarMi(string kullaniciAdi)
        {
            string query = "SELECT COUNT(*) FROM Kullanicilar WHERE kullaniciAdi = @kullaniciAdi";

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Kullanıcı adı kontrolü sırasında bir hata oluştu.", ex);
                }
            }
        }
        public static bool KullaniciSil(string kullaniciAdi)
        {
            string query = "DELETE FROM Kullanicilar WHERE kullaniciAdi = @kullaniciAdi";

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                        int affectedRows = command.ExecuteNonQuery();
                        return affectedRows > 0;
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Kullanıcı silme sırasında bir hata oluştu.", ex);
                }
            }
        }
        public static bool KullaniciGuncelle(Kullanicilar kullanici)
        {
            string selectQuery = @"
                SELECT adSoyad, sifre, kullaniciRol, email 
                FROM Kullanicilar 
                WHERE kullaniciAdi = @kullaniciAdi";
            string updateQuery = @"
                UPDATE Kullanicilar 
                SET adSoyad = @adSoyad, sifre = @sifre, kullaniciRol = @kullaniciRol, email = @email
                WHERE kullaniciAdi = @kullaniciAdi";

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    bool degisiklikVar = false;
                    using (var selectCmd = new SqlCommand(selectQuery, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);

                        using (var reader = selectCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return false;

                            string mevcutAdSoyad = reader["adSoyad"].ToString();
                            string mevcutSifre = reader["sifre"].ToString();
                            string mevcutRol = reader["kullaniciRol"].ToString();
                            string mevcutEmail = reader["email"].ToString();

                            degisiklikVar = mevcutAdSoyad != (kullanici.adSoyad ?? mevcutAdSoyad) ||
                                            mevcutSifre !=(kullanici.sifre ?? mevcutSifre) ||
                                            mevcutRol != (kullanici.kullaniciRol ?? mevcutRol) ||
                                            mevcutEmail != (kullanici.email ?? mevcutEmail);
                        }
                    }

                    if (!degisiklikVar)
                        return false;

                    using (var updateCmd = new SqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@adSoyad", kullanici.adSoyad ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@sifre", kullanici.sifre);
                        updateCmd.Parameters.AddWithValue("@kullaniciRol", kullanici.kullaniciRol ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@email", kullanici.email ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);

                        updateCmd.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Kullanıcı güncelleme sırasında bir hata oluştu.", ex);
                }
            }
        }
        public int GetKullaniciIdByKullaniciAdi(string kullaniciAdi)
        {
            if (string.IsNullOrEmpty(kullaniciAdi))
                throw new ArgumentException("Kullanıcı adı boş olamaz.", nameof(kullaniciAdi));

            string query = "SELECT id FROM Kullanicilar WHERE kullaniciAdi = @kullaniciAdi";

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                        object result = command.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            throw new Exception("Kullanıcı bulunamadı.");
                        }

                        return Convert.ToInt32(result);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Kullanıcı ID'si alınırken bir hata oluştu.", ex);
                }
            }
        }
    }
}
