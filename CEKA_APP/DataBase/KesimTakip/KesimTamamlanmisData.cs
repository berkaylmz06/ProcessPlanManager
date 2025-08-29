using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.DataBase
{
    class KesimTamamlanmisData
    {
        public static bool TablodanKesimTamamlanmisEkleme(string kesimYapan, string kesimId, int kesilmisPlanSayisi, DateTime kesimTarihi, TimeSpan kesimSaati, string kesilenLot)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();

                    string query = @"
    IF EXISTS (SELECT 1 FROM [KesimTamamlanmisPaket] WHERE [kesimId] = @kesimId)
    BEGIN
        UPDATE [KesimTamamlanmisPaket]
        SET 
            [kesilmisPlanSayisi] = [kesilmisPlanSayisi] + @kesilmisPlanSayisi,
            [kesimYapan] = @kesimYapan,
            [kesimTarihi] = @kesimTarihi,
            [kesimSaati] = @kesimSaati,
            [kesilenLot] = @kesilenLot
        WHERE [kesimId] = @kesimId
    END
    ELSE
    BEGIN
        INSERT INTO [KesimTamamlanmisPaket] 
            ([kesimYapan], [kesimId], [kesilmisPlanSayisi], [kesimTarihi], [kesimSaati], [kesilenLot])
        VALUES 
            (@kesimYapan, @kesimId, @kesilmisPlanSayisi, @kesimTarihi, @kesimSaati, @kesilenLot)
    END";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kesimYapan", kesimYapan ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@kesimId", kesimId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@kesilmisPlanSayisi", kesilmisPlanSayisi);
                        command.Parameters.AddWithValue("@kesimTarihi", kesimTarihi);
                        command.Parameters.AddWithValue("@kesimSaati", kesimSaati);
                        command.Parameters.AddWithValue("@kesilenLot", kesilenLot ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kesim kaydı eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public DataTable GetKesimListesTamamlanmis()
        {
            string query = "SELECT [kesimYapan], [kesimId], [kesilmisPlanSayisi], [kesilenLot], [kesimTarihi], [kesimSaati] FROM [KesimTamamlanmisPaket]";
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

        public DataTable FiltreleKesimBilgileri(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    string baseQuery = @"
                        SELECT
                            kesimYapan,
                            kesimId,
                            kesilmisPlanSayisi,
                            kesilenLot,
                            kesimTarihi,
                            kesimSaati
                        FROM KesimTamamlanmisPaket
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

                        if (kutu.Key.EndsWith("_Baslangic"))
                        {
                            if (DateTime.TryParse(hamDeger, out DateTime baslangicTarihi))
                            {
                                string paramName = $"@p{paramIndex++}";
                                conditions.Add($"{columnName} >= {paramName}");
                                parameters.Add(new SqlParameter(paramName, SqlDbType.DateTime) { Value = baslangicTarihi.Date });
                            }
                        }
                        else if (kutu.Key.EndsWith("_Bitis"))
                        {
                            if (DateTime.TryParse(hamDeger, out DateTime bitisTarihi))
                            {
                                string paramName = $"@p{paramIndex++}";
                                conditions.Add($"{columnName} < {paramName}");
                                parameters.Add(new SqlParameter(paramName, SqlDbType.DateTime) { Value = bitisTarihi.Date.AddDays(1) });
                            }
                        }
                        else
                        {
                            string paramName = $"@p{paramIndex++}";
                            string condition = string.Empty;

                            switch (columnName)
                            {
                                case "kesimYapan":
                                    condition = $"LOWER(kesimYapan) LIKE {paramName}";
                                    parameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar) { Value = "%" + hamDeger.ToLower() + "%" });
                                    break;
                                case "kesimId":
                                    condition = $"LOWER(kesimId) LIKE {paramName}";
                                    parameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar) { Value = "%" + hamDeger.ToLower() + "%" });
                                    break;
                                case "kesilmisPlanSayisi":
                                    if (int.TryParse(hamDeger, out int kesilmisPlanSayisi))
                                    {
                                        condition = $"kesilmisPlanSayisi = {paramName}";
                                        parameters.Add(new SqlParameter(paramName, SqlDbType.Int) { Value = kesilmisPlanSayisi });
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Geçersiz kesilmiş plan sayısı değeri: {hamDeger}");
                                    }
                                    break;
                                case "kesilenLot":
                                    condition = $"LOWER(kesilenLot) LIKE {paramName}";
                                    parameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar) { Value = "%" + hamDeger.ToLower() + "%" });
                                    break;
                                case "kesimSaati":
                                    if (TimeSpan.TryParse(hamDeger, out TimeSpan kesimSaati))
                                    {
                                        condition = $"kesimSaati = {paramName}";
                                        parameters.Add(new SqlParameter(paramName, SqlDbType.Time) { Value = kesimSaati });
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Geçersiz kesim saati değeri: {hamDeger}");
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
        private string NormalizeColumnName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return columnName;
            return columnName.Replace("ı", "i").Replace("İ", "I").Replace("ş", "s").Replace("Ş", "S")
                            .Replace("ğ", "g").Replace("Ğ", "G").Replace("ü", "u").Replace("Ü", "U")
                            .Replace("ç", "c").Replace("Ç", "C").ToLower();
        }
    }
}