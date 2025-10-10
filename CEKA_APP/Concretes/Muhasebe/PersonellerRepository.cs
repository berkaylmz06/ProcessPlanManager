using CEKA_APP.Abstracts.Muhasebe;
using CEKA_APP.Entitys.Muhasebe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Concretes.Muhasebe
{
    public class PersonellerRepository : IPersonellerRepository
    {
        public void PersonelEkle(SqlConnection connection, SqlTransaction transaction, string adSoyad, string kullaniciAdi, string departman, string telefon, bool aktif)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
                INSERT INTO Personeller (adSoyad, kullaniciAdi, departman, telefon, aktif, kayitTarihi)
                VALUES (@adSoyad, @kullaniciAdi, @departman, @telefon, @aktif, GETDATE());
            ";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@adSoyad", adSoyad);
                cmd.Parameters.AddWithValue("@kullaniciAdi", (object)kullaniciAdi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@departman", departman);
                cmd.Parameters.AddWithValue("@telefon", telefon);
                cmd.Parameters.AddWithValue("@aktif", aktif);
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetAllPersonel(SqlConnection connection, SqlTransaction transaction)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "SELECT * FROM Personeller ORDER BY adSoyad";
            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public void UpdatePersonel(SqlConnection connection, SqlTransaction transaction, int personelId, string adSoyad, string kullaniciAdi, string departman, string telefon, bool aktif)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
                UPDATE Personeller
                SET adSoyad = @adSoyad,
                    kullaniciAdi = @kullaniciAdi,
                    departman = @departman,
                    telefon = @telefon,
                    aktif = @aktif,
                    kayitTarihi = kayitTarihi -- değişmesin
                WHERE personelId = @personelId;
            ";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@personelId", personelId);
                cmd.Parameters.AddWithValue("@adSoyad", adSoyad);
                cmd.Parameters.AddWithValue("@kullaniciAdi", (object)kullaniciAdi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@departman", departman);
                cmd.Parameters.AddWithValue("@telefon", telefon);
                cmd.Parameters.AddWithValue("@aktif", aktif);
                cmd.ExecuteNonQuery();
            }
        }

        public void PersonelSil(SqlConnection connection, SqlTransaction transaction, int personelId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "DELETE FROM Personeller WHERE personelId = @personelId";
            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@personelId", personelId);
                cmd.ExecuteNonQuery();
            }
        }
        public List<Personeller> GetPersonelOperator(SqlConnection connection, SqlTransaction transaction)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "SELECT * FROM Muhasebe_Personeller WHERE departman = 'Operatör'";
            List<Personeller> list = new List<Personeller>();

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var personel = new Personeller
                        {
                            personelId = reader.GetInt32(reader.GetOrdinal("personelId")),
                            adSoyad = reader.GetString(reader.GetOrdinal("adSoyad")),
                            kullaniciAdi = reader["kullaniciAdi"] as string,
                            departman = reader.GetString(reader.GetOrdinal("departman")),
                            telefon = reader.GetString(reader.GetOrdinal("telefon")),
                            aktif = reader.GetBoolean(reader.GetOrdinal("aktif")),
                            kayitTarihi = reader.GetDateTime(reader.GetOrdinal("kayitTarihi"))
                        };

                        list.Add(personel);
                    }
                }
            }

            return list;
        }

    }
}
