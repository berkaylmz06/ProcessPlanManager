using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class FiyatlandirmaKilometreTaslariData
    {
        public List<(int Id, string Adi, DateTime Tarih)> GetFiyatlandirmaKilometreTasi()
        {
            var kilometreTaslari = new List<(int Id, string Adi, DateTime Tarih)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT kilometreTasiId, kilometreTasiAdi, olusturmaTarihi FROM ProjeFinans_KilometreTaslari";
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        kilometreTaslari.Add((reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2)));
                    }
                }
                connection.Close();
            }
            return kilometreTaslari;
        }

        public int FiyatlandirmaKilometreTasiEkle(string kilometreTasiAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO ProjeFinans_KilometreTaslari (kilometreTasiAdi, olusturmaTarihi) VALUES (@kilometreTasiAdi, @olusturmaTarihi)";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@kilometreTasiAdi", kilometreTasiAdi);
                    cmd.Parameters.AddWithValue("@olusturmaTarihi", DateTime.Now);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        public int GetFiyatlandirmaKilometreTasiIdByAdi(string kilometreTasiAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT kilometreTasiId FROM ProjeFinans_KilometreTaslari WHERE kilometreTasiAdi = @kilometreTasiAdi";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@kilometreTasiAdi", kilometreTasiAdi);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
        public int GetKilometreTasiId(string kilometreTasiAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string sql = @"
                        SELECT kilometreTasiId
                        FROM ProjeFinans_KilometreTaslari
                        WHERE kilometreTasiAdi = @kilometreTasiAdi";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@kilometreTasiAdi", kilometreTasiAdi.Trim());
                        object result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kilometre taşı ID alınırken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
        }
    }
}
