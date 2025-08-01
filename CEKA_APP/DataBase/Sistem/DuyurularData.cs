using CEKA_APP.Entitys;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CEKA_APP.DataBase
{
    public class DuyurularData
    {
        public bool DuyuruEkle(string olusturan, string duyuru, DateTime sistemSaat)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "INSERT INTO [Duyurular] (kullanici, duyuru, duyuruZamani) VALUES (@kullanici, @duyuru, @duyuruZamani)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add("@kullanici", SqlDbType.NVarChar).Value = olusturan ?? (object)DBNull.Value;
                        command.Parameters.Add("@duyuru", SqlDbType.NVarChar).Value = duyuru ?? (object)DBNull.Value;
                        command.Parameters.Add("@duyuruZamani", SqlDbType.DateTime2).Value = sistemSaat;

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

        public Duyurular GetSonDuyuru()
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT id, kullanici, duyuru, duyuruZamani FROM Duyurular ORDER BY duyuruZamani DESC, id DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Duyurular
                                {
                                    id = reader.GetInt32(0),
                                    kullanici = reader.GetString(1),
                                    duyuru = reader.GetString(2),
                                    duyuruZamani = reader.GetDateTime(3)
                                };
                            }
                        }
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Duyuru alınırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }
    }
}
