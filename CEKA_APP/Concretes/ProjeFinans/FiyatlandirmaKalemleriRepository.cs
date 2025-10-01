using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class FiyatlandirmaKalemleriRepository: IFiyatlandirmaKalemleriRepository
    {
        public List<(int Id, string Adi, string Birimi, DateTime Tarih)> GetFiyatlandirmaKalemleri(SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var fiyatlandirmaKalemleri = new List<(int Id, string Adi, string Birimi, DateTime Tarih)>();
            string query = "SELECT fiyatlandirmaKalemId, kalemAdi, kalemBirimi, olusturmaTarihi FROM ProjeFinans_FiyatlandirmaKalemleri";

            using (var cmd = new SqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    fiyatlandirmaKalemleri.Add((
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetDateTime(3)
                    ));
                }
            }

            return fiyatlandirmaKalemleri;
        }
        public int FiyatlandirmaKalemleriEkle(SqlConnection connection, SqlTransaction transaction, string kalemAdi, string kalemBirimi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
    INSERT INTO ProjeFinans_FiyatlandirmaKalemleri 
        (kalemAdi, kalemBirimi, olusturmaTarihi) 
    VALUES 
        (@kalemAdi, @kalemBirimi, @olusturmaTarihi);
    SELECT SCOPE_IDENTITY();";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kalemAdi", kalemAdi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@kalemBirimi", kalemBirimi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@olusturmaTarihi", DateTime.Now);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public FiyatlandirmaKalem GetFiyatlandirmaKalemByAdi(SqlConnection connection, string kalemAdi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = @"
    SELECT fiyatlandirmaKalemId, kalemAdi, kalemBirimi 
    FROM ProjeFinans_FiyatlandirmaKalemleri 
    WHERE kalemAdi = @kalemAdi";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@kalemAdi", kalemAdi ?? (object)DBNull.Value);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new FiyatlandirmaKalem
                        {
                            fiyatlandirmaKalemId = reader.GetInt32(reader.GetOrdinal("fiyatlandirmaKalemId")),
                            kalemAdi = reader.GetString(reader.GetOrdinal("kalemAdi")),
                            kalemBirimi = reader.GetString(reader.GetOrdinal("kalemBirimi"))
                        };
                    }
                }
            }

            return null;
        }
    }
}
