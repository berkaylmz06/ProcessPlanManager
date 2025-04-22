using KesimTakip.Entitys;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KesimTakip.DataBase
{
    class SorunBildirimleriData
    {
        public bool SorunBildirimEkle(string olusturan, string sorun, string sistemSaat)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "INSERT INTO \"SorunBildirimleri\" (kullanici, bildiri, bildirizamani) VALUES (@kullanici, @bildiri, @bildirizamani)";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@kullanici", olusturan);
                        command.Parameters.AddWithValue("@bildiri", sorun);
                        command.Parameters.AddWithValue("@bildirizamani", sistemSaat);

                        command.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Veritabanına veri eklenirken hata oluştu: {ex.Message}");
                    return false;
                }
            }
        }
        public static List<SorunBildirimleri> GetSorunlar()
        {
            var sorunlar = new List<SorunBildirimleri>();
            string query = "SELECT id, kullanici, bildiri, bildirizamani, okundu FROM \"SorunBildirimleri\"";

            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
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
            }
            return sorunlar;
        }
        public static void UpdateOkunduDurumu(int id)
        {
            string query = "SELECT okundu FROM \"SorunBildirimleri\" WHERE id = @id";
            string okunduDurumu = "";

            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    okunduDurumu = command.ExecuteScalar()?.ToString();
                }
            }

            if (okunduDurumu != "okundu")
            {
                string updateQuery = "UPDATE \"SorunBildirimleri\" SET okundu = 'okundu' WHERE id = @id";
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
