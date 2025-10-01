using CEKA_APP.Abstracts.Sistem;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.Sistem
{
    public class DuyurularRepository : IDuyurularRepository
    {
        public bool DuyuruEkle(string olusturan, string duyuru, DateTime sistemSaat, SqlConnection connection, SqlTransaction transaction)
        {
            string sql = @"INSERT INTO [Duyurular] (kullanici, duyuru, duyuruZamani) 
                       VALUES (@kullanici, @duyuru, @duyuruZamani)";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.Add("@kullanici", SqlDbType.NVarChar).Value = olusturan ?? (object)DBNull.Value;
                command.Parameters.Add("@duyuru", SqlDbType.NVarChar).Value = duyuru ?? (object)DBNull.Value;
                command.Parameters.Add("@duyuruZamani", SqlDbType.DateTime2).Value = sistemSaat;

                command.ExecuteNonQuery();
            }

            return true;
        }

        public Duyurular GetSonDuyuru(SqlConnection connection)
        {
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
    }
}
