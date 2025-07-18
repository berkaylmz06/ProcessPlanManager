using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_ProjeIliskiData
    {
        public static bool AltProjeEkle(string ustProjeNo, string altProjeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sql = @"
                        INSERT INTO ProjeFinans_ProjeIliski 
                        (ustProjeNo, altProjeNo)
                        VALUES 
                        (@ustProjeNo, @altProjeNo)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ustProjeNo", ustProjeNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@altProjeNo", altProjeNo ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kayıt eklenemedi: " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public static bool CheckAltProje(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string sql = @"
                SELECT COUNT(*)
                FROM ProjeFinans_ProjeIliski
                WHERE altProjeNo = @projeNo";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                        int count = (int)command.ExecuteScalar();
                        return count > 0; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Alt proje kontrolü sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public static List<string> GetAltProjeler(string projeNo)
        {
            var altProjeler = new List<string>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string sql = @"
                SELECT altProjeNo
                FROM ProjeFinans_ProjeIliski
                WHERE ustProjeNo = @projeNo OR altProjeNo = @projeNo";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string altProjeNo = reader.IsDBNull(0) ? null : reader.GetString(0);
                                if (!string.IsNullOrEmpty(altProjeNo))
                                {
                                    altProjeler.Add(altProjeNo);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Alt projeler alınırken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return altProjeler;
        }
    }
}
