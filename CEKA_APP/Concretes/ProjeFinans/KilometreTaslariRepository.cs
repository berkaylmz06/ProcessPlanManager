using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class KilometreTaslariRepository: IKilometreTaslariRepository
    {
        public List<(int Id, string Adi, DateTime Tarih)> GetFiyatlandirmaKilometreTasi(SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var kilometreTaslari = new List<(int Id, string Adi, DateTime Tarih)>();

            string query = "SELECT kilometreTasiId, kilometreTasiAdi, olusturmaTarihi FROM ProjeFinans_KilometreTaslari";
            using (var cmd = new SqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    kilometreTaslari.Add((reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2)));
                }
            }

            return kilometreTaslari;
        }
        public int FiyatlandirmaKilometreTasiEkle(SqlConnection connection, SqlTransaction transaction, string kilometreTasiAdi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
        INSERT INTO ProjeFinans_KilometreTaslari (kilometreTasiAdi, olusturmaTarihi) 
        VALUES (@kilometreTasiAdi, @olusturmaTarihi);
        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ";

            using (var cmd = new SqlCommand(query, connection, transaction)) 
            {
                cmd.Parameters.AddWithValue("@kilometreTasiAdi", kilometreTasiAdi);
                cmd.Parameters.AddWithValue("@olusturmaTarihi", DateTime.Now);

                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetFiyatlandirmaKilometreTasiIdByAdi(SqlConnection connection, string kilometreTasiAdi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = "SELECT kilometreTasiId FROM ProjeFinans_KilometreTaslari WHERE kilometreTasiAdi = @kilometreTasiAdi";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@kilometreTasiAdi", kilometreTasiAdi);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public string GetKilometreTasiAdi(SqlConnection connection, int kilometreTasiId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = "SELECT KalemAdi FROM ProjeFinans_FiyatlandirmaKilometreTaslari WHERE FiyatlandirmaKilometreTasiId = @id";
            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", kilometreTasiId);
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? result.ToString() : string.Empty;
            }
        }

        public int GetKilometreTasiId(SqlConnection connection, string kilometreTasiAdi)
        {
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

        public List<string> GetKilometreTasiAdlariByIds(SqlConnection connection, List<int> kilometreTasiIds)
        {
            var kilometreTasiAdlari = new List<string>();
            if (kilometreTasiIds == null || kilometreTasiIds.Count == 0)
            {
                return kilometreTasiAdlari;
            }

            string idList = string.Join(",", kilometreTasiIds);
            string query = $"SELECT kilometreTasiAdi FROM ProjeFinans_KilometreTaslari WHERE kilometreTasiId IN ({idList})";

            using (var cmd = new SqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    kilometreTasiAdlari.Add(reader.GetString(0));
                }
            }

            return kilometreTasiAdlari;
        }
    }
}
