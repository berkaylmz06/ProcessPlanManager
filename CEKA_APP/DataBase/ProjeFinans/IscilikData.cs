using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class IscilikData
    {
        

        public List<(int Id, string Adi, DateTime Tarih)> GetIscilikler()
        {
            var iscilikler = new List<(int Id, string Adi, DateTime Tarih)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT iscilikId, iscilikAdi, olusturmaTarihi FROM ProjeFinans_Iscilikler";
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        iscilikler.Add((reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2)));
                    }
                }
                connection.Close();
            }
            return iscilikler;
        }

        public int IscilikEkle(string iscilikAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO ProjeFinans_Iscilikler (iscilikAdi, olusturmaTarihi) VALUES (@iscilikAdi, @olusturmaTarihi)";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@iscilikAdi", iscilikAdi);
                    cmd.Parameters.AddWithValue("@olusturmaTarihi", DateTime.Now);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
