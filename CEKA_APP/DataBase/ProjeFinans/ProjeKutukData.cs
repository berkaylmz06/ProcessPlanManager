using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.DataBase
{
    public class ProjeKutukData
    {
        public static bool ProjeKutukEkle(ProjeKutuk kutuk)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sql = @"
                        INSERT INTO ProjeFinans_ProjeKutuk 
                        (musteriNo, musteriAdi, teklifNo, isFirsatiNo, projeNo, altProjeVarMi, digerProjeIliskisiVarMi, siparisSozlesmeTarihi, toplamBedel, nakliyeVarMi)
                        VALUES 
                        (@musteriNo, @musteriAdi, @teklifNo, @isFirsatiNo, @projeNo, @altProjeVarMi, @digerProjeIliskisiVarMi, @siparisSozlesmeTarihi, @toplamBedel, @nakliyeVarMi)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@musteriNo", kutuk.musteriNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@musteriAdi", kutuk.musteriAdi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@teklifNo", kutuk.teklifNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@isFirsatiNo", kutuk.isFirsatiNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@projeNo", kutuk.projeNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@altProjeVarMi", kutuk.altProjeVarMi);
                        command.Parameters.AddWithValue("@digerProjeIliskisiVarMi", string.IsNullOrEmpty(kutuk.digerProjeIliskisiVarMi) ? (object)DBNull.Value : kutuk.digerProjeIliskisiVarMi);
                        command.Parameters.AddWithValue("@siparisSozlesmeTarihi", kutuk.siparisSozlesmeTarihi);
                        command.Parameters.AddWithValue("@toplamBedel", kutuk.toplamBedel);
                        command.Parameters.AddWithValue("@nakliyeVarMi", kutuk.nakliyeVarMi);
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

        public static bool ProjeEkleProjeFinans(string projeAdi, string aciklama = null)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    string checkSql = @"
                        SELECT COUNT(*) 
                        FROM ProjeFinans_Projeler 
                        WHERE projeNo = @projeNo";

                    using (SqlCommand checkCommand = new SqlCommand(checkSql, conn))
                    {
                        checkCommand.Parameters.AddWithValue("@projeNo", projeAdi);
                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show($"Proje No '{projeAdi}' zaten kayıtlı.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    string sorgu = @"
                        INSERT INTO ProjeFinans_Projeler (projeNo, olusturmaTarihi, aciklama)
                        VALUES (@projeNo, GETDATE(), @aciklama)";

                    using (SqlCommand komut = new SqlCommand(sorgu, conn))
                    {
                        komut.Parameters.AddWithValue("@projeNo", projeAdi);
                        komut.Parameters.AddWithValue("@aciklama", aciklama != null ? (object)aciklama : DBNull.Value);
                        komut.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje eklenemedi: {ex.Message}", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        public static ProjeKutuk ProjeKutukAra(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string sql = @"
                        SELECT 
                            projeKutukId, musteriNo, musteriAdi, teklifNo, isFirsatiNo, projeNo,
                            altProjeVarMi, digerProjeIliskisiVarMi, siparisSozlesmeTarihi, toplamBedel, nakliyeVarMi
                        FROM ProjeFinans_ProjeKutuk
                        WHERE TRIM(projeNo) = @projeNo";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var kutuk = new ProjeKutuk
                                {
                                    projeKutukId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    musteriNo = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    musteriAdi = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    teklifNo = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    isFirsatiNo = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    projeNo = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    altProjeVarMi = reader.IsDBNull(6) ? false : reader.GetBoolean(6),
                                    digerProjeIliskisiVarMi = reader.IsDBNull(7) ? "0" : reader.GetString(7),
                                    siparisSozlesmeTarihi = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8),
                                    toplamBedel = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                                    nakliyeVarMi = reader.IsDBNull(10) ? false : reader.GetBoolean(10),
                                    altProjeBilgileri = new List<string>()
                                };

                                reader.Close();

                                if (kutuk.altProjeVarMi)
                                {
                                    string altProjeSql = @"
                                        SELECT altProjeNo
                                        FROM ProjeFinans_ProjeIliski
                                        WHERE ustProjeNo = @projeNo";
                                    using (SqlCommand altCommand = new SqlCommand(altProjeSql, connection))
                                    {
                                        altCommand.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                                        using (var altReader = altCommand.ExecuteReader())
                                        {
                                            while (altReader.Read())
                                            {
                                                kutuk.altProjeBilgileri.Add(altReader.IsDBNull(0) ? "" : altReader.GetString(0));
                                            }
                                        }
                                    }
                                }

                                return kutuk;
                            }
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje aranırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }
    }
}