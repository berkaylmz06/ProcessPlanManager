using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_TeminatMektuplariData
    {
        public bool TeminatMektubuKaydet(TeminatMektuplari mektup)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string sql = @"
                        INSERT INTO ProjeFinans_TeminatMektuplari
                        (mektupNo, musteriNo, musteriAdi, paraBirimi, banka, mektupTuru, tutar, vadeTarihi, iadeTarihi, komisyonTutari, komisyonOrani, komisyonVadesi, projeNo, kilometreTasiId)
                        VALUES
                        (@mektupNo, @musteriNo, @musteriAdi, @paraBirimi, @banka, @mektupTuru, @tutar, @vadeTarihi, @iadeTarihi, @komisyonTutari, @komisyonOrani, @komisyonVadesi, @projeNo, @kilometreTasiId)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@mektupNo", mektup.mektupNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@musteriNo", mektup.musteriNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@musteriAdi", mektup.musteriAdi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@paraBirimi", mektup.paraBirimi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@banka", mektup.banka ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@mektupTuru", mektup.mektupTuru ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tutar", mektup.tutar);
                        command.Parameters.AddWithValue("@vadeTarihi", mektup.vadeTarihi.HasValue ? (object)mektup.vadeTarihi : DBNull.Value);
                        command.Parameters.AddWithValue("@iadeTarihi", mektup.iadeTarihi);
                        command.Parameters.AddWithValue("@komisyonTutari", mektup.komisyonTutari);
                        command.Parameters.AddWithValue("@komisyonOrani", mektup.komisyonOrani);
                        command.Parameters.AddWithValue("@komisyonVadesi", mektup.komisyonVadesi);
                        command.Parameters.AddWithValue("@projeNo", string.IsNullOrEmpty(mektup.projeNo) ? "-" : mektup.projeNo);
                        command.Parameters.AddWithValue("@kilometreTasiId", mektup.kilometreTasiId);

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

        public List<TeminatMektuplari> GetTeminatMektuplari()
        {
            List<TeminatMektuplari> teminatMektuplari = new List<TeminatMektuplari>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            t.mektupNo, 
                            t.musteriNo, 
                            t.musteriAdi,
                            k.kilometreTasiAdi,
                            t.paraBirimi, 
                            t.banka, 
                            t.mektupTuru, 
                            t.tutar, 
                            t.vadeTarihi, 
                            t.iadeTarihi, 
                            t.komisyonTutari, 
                            t.komisyonOrani, 
                            t.komisyonVadesi, 
                            t.projeNo,
                            t.kilometreTasiId
                        FROM ProjeFinans_TeminatMektuplari t
                        LEFT JOIN ProjeFinans_KilometreTaslari k ON t.kilometreTasiId = k.kilometreTasiId
                        ORDER BY t.mektupNo";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TeminatMektuplari mektup = new TeminatMektuplari
                                {
                                    mektupNo = reader["mektupNo"] as string ?? "",
                                    musteriNo = reader["musteriNo"] as string ?? "",
                                    musteriAdi = reader["musteriAdi"] as string ?? "",
                                    kilometreTasiAdi = reader["kilometreTasiAdi"] as string ?? "",
                                    paraBirimi = reader["paraBirimi"] as string ?? "",
                                    banka = reader["banka"] as string ?? "",
                                    mektupTuru = reader["mektupTuru"] as string ?? "",
                                    tutar = reader.GetDecimal(reader.GetOrdinal("tutar")),
                                    vadeTarihi = !reader.IsDBNull(reader.GetOrdinal("vadeTarihi")) ? (DateTime?)reader.GetDateTime(reader.GetOrdinal("vadeTarihi")) : null,
                                    iadeTarihi = reader.GetDateTime(reader.GetOrdinal("iadeTarihi")),
                                    komisyonTutari = reader.GetDecimal(reader.GetOrdinal("komisyonTutari")),
                                    komisyonOrani = reader.GetDecimal(reader.GetOrdinal("komisyonOrani")),
                                    komisyonVadesi = reader.GetInt32(reader.GetOrdinal("komisyonVadesi")),
                                    projeNo = reader["projeNo"] as string ?? "",
                                    kilometreTasiId = reader.GetInt32(reader.GetOrdinal("kilometreTasiId"))
                                };
                                teminatMektuplari.Add(mektup);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Teminat mektupları getirilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
            return teminatMektuplari;
        }

        public DataTable FiltreleTeminatMektuplari(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    string baseQuery = @"
                        SELECT 
                            t.mektupNo,
                            t.musteriNo,
                            t.musteriAdi,
                            k.kilometreTasiAdi,
                            t.paraBirimi,
                            t.banka,
                            t.mektupTuru,
                            t.tutar,
                            t.vadeTarihi,
                            t.iadeTarihi,
                            t.komisyonTutari,
                            t.komisyonOrani,
                            t.komisyonVadesi,
                            t.projeNo,
                            t.kilometreTasiId
                        FROM ProjeFinans_TeminatMektuplari t
                        LEFT JOIN ProjeFinans_KilometreTaslari k ON t.kilometreTasiId = k.kilometreTasiId
                        WHERE 1=1";

                    var conditions = new List<string>();
                    var parameters = new List<SqlParameter>();
                    int paramIndex = 0;

                    foreach (var kutu in filtreKutulari)
                    {
                        string hamDeger = kutu.Value.Text.Trim();
                        if (string.IsNullOrEmpty(hamDeger))
                        {
                            continue;
                        }

                        string dataGridHeader = kutu.Key.Replace("_Baslangic", "").Replace("_Bitis", "");
                        string columnName = dataGrid.Columns.Cast<DataGridViewColumn>()
                                .FirstOrDefault(c => NormalizeColumnName(c.HeaderText) == NormalizeColumnName(dataGridHeader))?.Name;

                        if (string.IsNullOrEmpty(columnName))
                        {
                            MessageBox.Show($"Sütun bulunamadı: {kutu.Key}", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        string paramName = $"@p{paramIndex++}";
                        string condition = string.Empty;

                        if (kutu.Key.EndsWith("_Baslangic") || kutu.Key.EndsWith("_Bitis"))
                        {
                            if (DateTime.TryParse(hamDeger, out DateTime tarihDegeri))
                            {
                                if (kutu.Key.EndsWith("_Baslangic"))
                                {
                                    conditions.Add($"t.{columnName} >= {paramName}");
                                    parameters.Add(new SqlParameter(paramName, SqlDbType.DateTime) { Value = tarihDegeri.Date });
                                }
                                else if (kutu.Key.EndsWith("_Bitis"))
                                {
                                    conditions.Add($"t.{columnName} < {paramName}");
                                    parameters.Add(new SqlParameter(paramName, SqlDbType.DateTime) { Value = tarihDegeri.Date.AddDays(1) });
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Geçersiz tarih değeri: {hamDeger}");
                            }
                        }
                        else
                        {
                            switch (columnName)
                            {
                                case "mektupNo":
                                case "musteriNo":
                                case "musteriAdi":
                                case "kilometreTasiAdi":
                                case "paraBirimi":
                                case "banka":
                                case "mektupTuru":
                                case "projeNo":
                                    string prefix = (columnName == "kilometreTasiAdi") ? "k" : "t";
                                    condition = $"LOWER({prefix}.{columnName}) LIKE {paramName}";
                                    parameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar) { Value = $"%{hamDeger.ToLower()}%" });
                                    break;
                                case "tutar":
                                case "komisyonTutari":
                                case "komisyonOrani":
                                    if (decimal.TryParse(hamDeger, out decimal deger))
                                    {
                                        condition = $"t.{columnName} = {paramName}";
                                        parameters.Add(new SqlParameter(paramName, SqlDbType.Decimal) { Value = deger });
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Geçersiz {columnName} değeri: {hamDeger}");
                                    }
                                    break;
                                case "komisyonVadesi":
                                    if (int.TryParse(hamDeger, out int vade))
                                    {
                                        condition = $"t.{columnName} = {paramName}";
                                        parameters.Add(new SqlParameter(paramName, SqlDbType.Int) { Value = vade });
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Geçersiz {columnName} değeri: {hamDeger}");
                                    }
                                    break;
                                default:
                                    Console.WriteLine($"Bilinmeyen sütun: {columnName}");
                                    break;
                            }

                            if (!string.IsNullOrEmpty(condition))
                            {
                                conditions.Add(condition);
                            }
                        }
                    }

                    if (conditions.Any())
                    {
                        baseQuery += " AND " + string.Join(" AND ", conditions);
                    }

                    baseQuery += " ORDER BY t.mektupNo";

                    using (var cmd = new SqlCommand(baseQuery, connection))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());

                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Hata detayı: {ex.ToString()}");
                return null;
            }
        }
        public bool MektupNoVarMi(string mektupNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM ProjeFinans_TeminatMektuplari WHERE mektupNo = @mektupNo";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@mektupNo", mektupNo ?? (object)DBNull.Value);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Mektup numarası kontrol edilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        public void MektupGuncelle(string eskiMektupNo, TeminatMektuplari guncelMektup)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE ProjeFinans_TeminatMektuplari
                        SET musteriNo = @yeniMusteriNo,
                            musteriAdi = @musteriAdi,
                            paraBirimi = @paraBirimi,
                            banka = @banka,
                            mektupTuru = @mektupTuru,
                            tutar = @tutar,
                            vadeTarihi = @vadeTarihi,
                            iadeTarihi = @iadeTarihi,
                            komisyonTutari = @komisyonTutari,
                            komisyonOrani = @komisyonOrani,
                            komisyonVadesi = @komisyonVadesi,
                            projeNo = @projeNo,
                            kilometreTasiId = @kilometreTasiId
                        WHERE mektupNo = @eskiMektupNo";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@yeniMusteriNo", guncelMektup.musteriNo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@musteriAdi", guncelMektup.musteriAdi ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@paraBirimi", guncelMektup.paraBirimi ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@banka", guncelMektup.banka ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@mektupTuru", guncelMektup.mektupTuru ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tutar", guncelMektup.tutar);
                        cmd.Parameters.AddWithValue("@vadeTarihi", guncelMektup.vadeTarihi.HasValue ? (object)guncelMektup.vadeTarihi : DBNull.Value);
                        cmd.Parameters.AddWithValue("@iadeTarihi", guncelMektup.iadeTarihi);
                        cmd.Parameters.AddWithValue("@komisyonTutari", guncelMektup.komisyonTutari);
                        cmd.Parameters.AddWithValue("@komisyonOrani", guncelMektup.komisyonOrani);
                        cmd.Parameters.AddWithValue("@komisyonVadesi", guncelMektup.komisyonVadesi);
                        cmd.Parameters.AddWithValue("@projeNo", string.IsNullOrEmpty(guncelMektup.projeNo) ? "-" : guncelMektup.projeNo);
                        cmd.Parameters.AddWithValue("@kilometreTasiId", guncelMektup.kilometreTasiId);
                        cmd.Parameters.AddWithValue("@eskiMektupNo", eskiMektupNo ?? (object)DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Mektup güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        public void MektupSil(string mektupNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM ProjeFinans_TeminatMektuplari WHERE mektupNo = @mektupNo";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@mektupNo", mektupNo ?? (object)DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Mektup silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private string NormalizeColumnName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return columnName;
            return columnName.Replace("ı", "i").Replace("İ", "I").Replace("ş", "s").Replace("Ş", "S")
                            .Replace("ğ", "g").Replace("Ğ", "G").Replace("ü", "u").Replace("Ü", "U")
                            .Replace("ç", "c").Replace("Ç", "C").ToLower();
        }
    }
}