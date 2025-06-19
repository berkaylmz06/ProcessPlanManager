using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFiyatlandirmaData
    {
        public void FiyatlandirmaKaydet(string projeNo, int iscilikId, decimal teklif, decimal maliyet)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO ProjeFinans_Fiyatlandirma (projeNo, iscilikId, teklifToplam, gerceklesenMaliyet) VALUES (@projeNo, @iscilikId, @teklif, @maliyet)";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeNo", projeNo);
                    cmd.Parameters.AddWithValue("@iscilikId", iscilikId);
                    cmd.Parameters.AddWithValue("@teklif", teklif);
                    cmd.Parameters.AddWithValue("@maliyet", maliyet);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void FiyatlandirmaGuncelle(string projeNo, int iscilikId, decimal teklif, decimal maliyet)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "UPDATE ProjeFinans_Fiyatlandirma SET teklifToplam = @teklif, gerceklesenMaliyet = @maliyet WHERE projeNo = @projeNo AND iscilikId = @iscilikId";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeNo", projeNo);
                    cmd.Parameters.AddWithValue("@iscilikId", iscilikId);
                    cmd.Parameters.AddWithValue("@teklif", teklif);
                    cmd.Parameters.AddWithValue("@maliyet", maliyet);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public List<(string IscilikAdi, decimal Teklif, decimal Maliyet)> GetFiyatlandirmaByProje(string projeNo)
        {
            var fiyatlandirmalar = new List<(string IscilikAdi, decimal Teklif, decimal Maliyet)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT i.iscilikAdi, f.teklifToplam, f.gerceklesenMaliyet " +
                              "FROM ProjeFinans_Fiyatlandirma f " +
                              "JOIN ProjeFinans_Iscilikler i ON f.iscilikId = i.iscilikId " +
                              "WHERE f.projeNo = @projeNo";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeNo", projeNo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal teklif = reader["teklifToplam"] != DBNull.Value ? Convert.ToDecimal(reader["teklifToplam"]) : 0;
                            decimal maliyet = reader["gerceklesenMaliyet"] != DBNull.Value ? Convert.ToDecimal(reader["gerceklesenMaliyet"]) : 0;
                            fiyatlandirmalar.Add((reader["iscilikAdi"].ToString(), teklif, maliyet));
                        }
                    }
                }
                connection.Close();
            }
            return fiyatlandirmalar;
        }
    }
}
