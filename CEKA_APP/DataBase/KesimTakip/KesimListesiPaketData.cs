using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.DataBase
{
    class KesimListesiPaketData
    {
        public static bool SaveKesimDataPaket(string olusturan, string kesimId, int kesilecekPlanSayisi, int toplamPlanTekrari, DateTime eklemeTarihi)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"IF NOT EXISTS (SELECT 1 FROM KesimListesiPaket WHERE kesimId = @kesimId)
                            INSERT INTO KesimListesiPaket ([olusturan], [kesimId], [kesilecekPlanSayisi], [toplamPlanTekrari], [eklemeTarihi])
                            VALUES (@olusturan, @kesimId, @kesilecekPlanSayisi, @toplamPlanTekrari, @eklemeTarihi)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@olusturan", SqlDbType.NVarChar).Value = olusturan ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@kesimId", SqlDbType.NVarChar).Value = kesimId ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@kesilecekPlanSayisi", SqlDbType.Int).Value = kesilecekPlanSayisi;
                        cmd.Parameters.Add("@toplamPlanTekrari", SqlDbType.Int).Value = toplamPlanTekrari;
                        cmd.Parameters.Add("@eklemeTarihi", SqlDbType.DateTime).Value = eklemeTarihi;

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool KesimListesiPaketSil(string kesimId)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();
                    string deleteKesimListesiQuery = "DELETE FROM KesimListesi WHERE kesimId = @kesimId";
                    using (var cmdKesimListesi = new SqlCommand(deleteKesimListesiQuery, conn))
                    {
                        cmdKesimListesi.Parameters.AddWithValue("@kesimId", kesimId);
                        cmdKesimListesi.ExecuteNonQuery();
                    }

                    string deletePaketQuery = "DELETE FROM KesimListesiPaket WHERE kesimId = @kesimId";
                    using (var cmdPaket = new SqlCommand(deletePaketQuery, conn))
                    {
                        cmdPaket.Parameters.AddWithValue("@kesimId", kesimId);
                        int rowsAffected = cmdPaket.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable GetKesimListesiPaket()
        {
            string query = "SELECT [olusturan], [kesimId], [kesilecekPlanSayisi], [kesilmisPlanSayisi], [toplamPlanTekrari], [eklemeTarihi] FROM [KesimListesiPaket]";
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kesim listesi getirilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        public static bool KesimListesiPaketKontrolluDusme(string kesimId, int kesilenMiktar, out string hataMesaji)
        {
            hataMesaji = "";

            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    string selectQuery = "SELECT [kesilecekPlanSayisi] FROM [KesimListesiPaket] WHERE [kesimId] = @id";
                    using (var selectCommand = new SqlCommand(selectQuery, conn))
                    {
                        selectCommand.Parameters.AddWithValue("@id", kesimId);
                        var mevcutDegerObj = selectCommand.ExecuteScalar();

                        if (mevcutDegerObj != null && int.TryParse(mevcutDegerObj.ToString(), out int mevcutMiktar))
                        {
                            if (kesilenMiktar > mevcutMiktar)
                            {
                                hataMesaji = $"Girilen miktar ({kesilenMiktar}) mevcut değerden ({mevcutMiktar}) fazla olamaz!";
                                return false;
                            }

                            string updateQuery = "UPDATE [KesimListesiPaket] " +
                                                "SET [kesilecekPlanSayisi] = [kesilecekPlanSayisi] - @azalt, " +
                                                "[kesilmisPlanSayisi] = [kesilmisPlanSayisi] + @arttir " +
                                                "WHERE [kesimId] = @id";

                            using (var updateCommand = new SqlCommand(updateQuery, conn))
                            {
                                updateCommand.Parameters.AddWithValue("@azalt", kesilenMiktar);
                                updateCommand.Parameters.AddWithValue("@arttir", kesilenMiktar);
                                updateCommand.Parameters.AddWithValue("@id", kesimId);

                                updateCommand.ExecuteNonQuery();
                            }

                            return true;
                        }
                        else
                        {
                            hataMesaji = "Mevcut değer alınamadı.";
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    hataMesaji = $"Kontrollü düşme işlemi sırasında hata oluştu: {ex.Message}";
                    return false;
                }
            }
        }

        public static void VerileriYenile(DataGridView data)
        {
            string query = "SELECT [olusturan], [kesimId], [kesilecekPlanSayisi], [kesilmisPlanSayisi], [toplamPlanTekrari], [eklemeTarihi] FROM [KesimListesiPaket]";
            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        data.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Veriler yenilenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        public DataTable FiltreleKesimListesiPaket(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    string baseQuery = @"
                        SELECT
                            olusturan,
                            kesimId,
                            kesilecekPlanSayisi,
                            kesilmisPlanSayisi,
                            toplamPlanTekrari,
                            eklemeTarihi
                        FROM KesimListesiPaket
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

                        string sqlColumnName = (columnName.Contains(" ") || columnName.Contains(".")) ? $"[{columnName}]" : columnName;

                        if (kutu.Key.EndsWith("_Baslangic"))
                        {
                            if (DateTime.TryParse(hamDeger, out DateTime baslangicTarihi))
                            {
                                string paramName = $"@p{paramIndex++}";
                                conditions.Add($"{sqlColumnName} >= {paramName}");
                                parameters.Add(new SqlParameter(paramName, SqlDbType.DateTime) { Value = baslangicTarihi.Date });
                            }
                        }
                        else if (kutu.Key.EndsWith("_Bitis"))
                        {
                            if (DateTime.TryParse(hamDeger, out DateTime bitisTarihi))
                            {
                                string paramName = $"@p{paramIndex++}";
                                conditions.Add($"{sqlColumnName} < {paramName}");
                                parameters.Add(new SqlParameter(paramName, SqlDbType.DateTime) { Value = bitisTarihi.Date.AddDays(1) });
                            }
                        }
                        else
                        {
                            string paramName = $"@p{paramIndex++}";
                            string condition = string.Empty;

                            switch (columnName)
                            {
                                case "olusturan":
                                case "kesimId":
                                    condition = $"LOWER({sqlColumnName}) LIKE {paramName}";
                                    parameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar) { Value = "%" + hamDeger.ToLower() + "%" });
                                    break;
                                case "kesilecekPlanSayisi":
                                case "kesilmisPlanSayisi":
                                case "toplamPlanTekrari":
                                    if (int.TryParse(hamDeger, out int deger))
                                    {
                                        condition = $"{sqlColumnName} = {paramName}";
                                        parameters.Add(new SqlParameter(paramName, SqlDbType.Int) { Value = deger });
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Geçersiz sayısal değer: {hamDeger}");
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
        public static bool KesimIdVarMi(string kesimId)
        {
            try
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM KesimListesiPaket 
                    WHERE SUBSTRING(kesimId, 1, CHARINDEX('-', kesimId) - 1) = @KesimId";

                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var command = new SqlCommand(query, conn))
                    {
                        string kesimIdSade = kesimId.Split('-')[0];
                        command.Parameters.AddWithValue("@KesimId", kesimIdSade);

                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kesim ID kontrolü sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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