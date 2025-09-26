using CEKA_APP.Abstracts.Sistem;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.Sistem
{
    public class SorunBildirimleriRepository : ISorunBildirimleriRepository
    {
        public bool SorunBildirimEkle(SqlConnection connection, SqlTransaction transaction, string olusturan, string sorun, DateTime sistemSaat)
        {
            string sql = @"
        INSERT INTO SorunBildirimleri (kullanici, bildiri, bildirizamani) 
        VALUES (@kullanici, @bildiri, @bildirizamani)";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@kullanici", olusturan ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@bildiri", sorun ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@bildirizamani", sistemSaat);

                command.ExecuteNonQuery();
            }

            return true;
        }

        public List<SorunBildirimleri> GetSorunlar(SqlConnection connection)
        {
            var sorunlar = new List<SorunBildirimleri>();
            string query = "SELECT id, kullanici, bildiri, bildirizamani, okundu FROM [SorunBildirimleri]";

            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sorunlar.Add(new SorunBildirimleri
                        {
                            id = reader.GetInt32(0),
                            kullanici = reader.GetString(1),
                            bildiri = reader.GetString(2),
                            bildirizamani = reader.GetString(3), 
                            okundu = reader.GetString(4),
                        });
                    }
                }
            }
            return sorunlar;
        }
        public void UpdateOkunduDurumu(SqlConnection connection, SqlTransaction transaction, int id)
        {
            string query = "SELECT okundu FROM [SorunBildirimleri] WHERE id = @id";
            string okunduDurumu = "";

            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@id", id);
                okunduDurumu = command.ExecuteScalar()?.ToString();
            }

            if (okunduDurumu != "okundu")
            {
                string updateQuery = "UPDATE [SorunBildirimleri] SET okundu = 'okundu' WHERE id = @id";
                using (var command = new SqlCommand(updateQuery, connection, transaction))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
