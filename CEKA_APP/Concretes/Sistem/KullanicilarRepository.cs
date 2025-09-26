using CEKA_APP.Abstracts.Sistem;
using CEKA_APP.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Concretes.Sistem
{
    public class KullanicilarRepository : IKullanicilarRepository
    {
        public Kullanicilar GirisYap(SqlConnection connection, SqlTransaction transaction, string kullaniciAdi, string sifre)
        {
            Kullanicilar kullanici = null;

            string sql = @"
        SELECT id, adSoyad, kullaniciAdi, sifre, kullaniciRol, email 
        FROM Kullanicilar 
        WHERE kullaniciAdi = @kullaniciAdi AND sifre = @sifre";

            using (var cmd = new SqlCommand(sql, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@sifre", sifre);

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

            return kullanici;
        }
        public DataTable GetKullaniciListesi(SqlConnection connection)
        {
            string query = @"
        SELECT id, adSoyad, kullaniciAdi, sifre, kullaniciRol, email 
        FROM Kullanicilar 
        ORDER BY adSoyad";

            using (var adapter = new SqlDataAdapter(query, connection))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void KullaniciEkle(SqlConnection connection, SqlTransaction transaction, Kullanicilar kullanici)
        {
            if (kullanici == null || string.IsNullOrEmpty(kullanici.kullaniciAdi) || string.IsNullOrEmpty(kullanici.sifre))
                throw new ArgumentException("Kullanıcı bilgileri eksik.");

            string kontrolQuery = "SELECT COUNT(*) FROM Kullanicilar WHERE kullaniciAdi = @kullaniciAdi OR email = @email";
            string insertQuery = @"
        INSERT INTO Kullanicilar 
        (adSoyad, kullaniciAdi, sifre, email) 
        VALUES 
        (@adSoyad, @kullaniciAdi, @sifre, @email)";

            using (var kontrolCmd = new SqlCommand(kontrolQuery, connection, transaction))
            {
                kontrolCmd.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);
                kontrolCmd.Parameters.AddWithValue("@email", kullanici.email ?? (object)DBNull.Value);
                int mevcutKullanici = (int)kontrolCmd.ExecuteScalar();

                if (mevcutKullanici > 0)
                    throw new Exception("Bu kullanıcı adı veya e-posta zaten kullanılıyor.");
            }

            using (var command = new SqlCommand(insertQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@adSoyad", kullanici.adSoyad ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);
                command.Parameters.AddWithValue("@sifre", kullanici.sifre);
                command.Parameters.AddWithValue("@email", kullanici.email ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        public bool KullaniciAdiVarMi(SqlConnection connection, string kullaniciAdi)
        {
            string query = "SELECT COUNT(*) FROM Kullanicilar WHERE kullaniciAdi = @kullaniciAdi";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        public bool KullaniciSil(SqlConnection connection, SqlTransaction transaction, string kullaniciAdi)
        {
            string query = "DELETE FROM Kullanicilar WHERE kullaniciAdi = @kullaniciAdi";

            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                int affectedRows = command.ExecuteNonQuery();
                return affectedRows > 0;
            }
        }
        public bool KullaniciGuncelle(SqlConnection connection, SqlTransaction transaction, Kullanicilar kullanici)
        {
            string selectQuery = @"
        SELECT adSoyad, sifre, kullaniciRol, email 
        FROM Kullanicilar 
        WHERE kullaniciAdi = @kullaniciAdi";

            string updateQuery = @"
        UPDATE Kullanicilar 
        SET adSoyad = @adSoyad, sifre = @sifre, kullaniciRol = @kullaniciRol, email = @email
        WHERE kullaniciAdi = @kullaniciAdi";

            bool degisiklikVar = false;

            using (var selectCmd = new SqlCommand(selectQuery, connection, transaction))
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
                                     mevcutSifre != (kullanici.sifre ?? mevcutSifre) ||
                                     mevcutRol != (kullanici.kullaniciRol ?? mevcutRol) ||
                                     mevcutEmail != (kullanici.email ?? mevcutEmail);
                }
            }

            if (!degisiklikVar)
                return false;

            using (var updateCmd = new SqlCommand(updateQuery, connection, transaction))
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
        public int GetKullaniciIdByKullaniciAdi(SqlConnection connection, string kullaniciAdi)
        {
            if (string.IsNullOrEmpty(kullaniciAdi))
                throw new ArgumentException("Kullanıcı adı boş olamaz.", nameof(kullaniciAdi));

            string query = "SELECT id FROM Kullanicilar WHERE kullaniciAdi = @kullaniciAdi";

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
        public Kullanicilar KullaniciBilgiGetir(SqlConnection connection, string kullaniciAdi)
        {
            string query = "SELECT k.adSoyad, k.sifre, k.email FROM Kullanicilar k WHERE k.kullaniciAdi = @kullaniciAdi";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Kullanicilar
                        {
                            kullaniciAdi = kullaniciAdi,
                            adSoyad = reader["adSoyad"].ToString(),
                            sifre = reader["sifre"].ToString(),
                            email = reader["email"].ToString()
                        };
                    }
                    return null;
                }
            }
        }
        public bool KullaniciGuncelleKullaniciBilgi(SqlConnection connection, SqlTransaction transaction, Kullanicilar kullanici)
        {
            string query = "UPDATE Kullanicilar SET adSoyad = @adSoyad, sifre = @sifre, email = @email WHERE kullaniciAdi = @kullaniciAdi";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@adSoyad", kullanici.adSoyad ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@sifre", kullanici.sifre ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email", kullanici.email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);

                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows > 0;
            }
        }
    }
}
