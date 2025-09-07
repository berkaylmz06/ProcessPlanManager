using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class MusterilerRepository: IMusterilerRepository
    {
        public void MusteriKaydet(Musteriler musteri, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string query = @"
        INSERT INTO ProjeFinans_Musteriler
        (musteriNo, musteriAdi, vergiNo, vergiDairesi, adres, musteriMensei, doviz)
        VALUES (@musteriNo, @musteriAdi, @vergiNo, @vergiDairesi, @adres, @musteriMensei, @doviz)";

            using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@musteriNo", musteri.musteriNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@musteriAdi", musteri.musteriAdi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@vergiNo", musteri.vergiNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@vergiDairesi", musteri.vergiDairesi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@adres", musteri.adres ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@musteriMensei", musteri.musteriMensei ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@doviz", musteri.doviz ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }


        public bool MusteriNoVarMi(string musteriNo, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string query = "SELECT COUNT(1) FROM ProjeFinans_Musteriler WHERE musteriNo = @musteriNo";

            using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@musteriNo", musteriNo ?? (object)DBNull.Value);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }


        public Musteriler GetMusteriByMusteriNo(string musteriNo)
        {
            Musteriler musteri = null;
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT musteriNo, musteriAdi, vergiNo, vergiDairesi, adres, musteriMensei, doviz FROM ProjeFinans_Musteriler WHERE musteriNo = @musteriNo";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@musteriNo", musteriNo);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            musteri = new Musteriler
                            {
                                musteriNo = reader["musteriNo"] as string,
                                musteriAdi = reader["musteriAdi"] as string,
                                vergiNo = reader["vergiNo"] as string,
                                vergiDairesi = reader["vergiDairesi"] as string,
                                adres = reader["adres"] as string,
                                musteriMensei = reader["musteriMensei"] as string,
                                doviz = reader["doviz"] as string
                            };
                        }
                    }
                }
            }
            return musteri;
        }

        public List<Musteriler> GetMusteriler()
        {
            List<Musteriler> musterilerListesi = new List<Musteriler>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT musteriNo, musteriAdi, vergiNo, vergiDairesi, adres, musteriMensei, doviz FROM ProjeFinans_Musteriler ORDER BY musteriAdi";
                using (var cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Musteriler musteri = new Musteriler
                            {
                                musteriNo = reader["musteriNo"] as string,
                                musteriAdi = reader["musteriAdi"] as string,
                                vergiNo = reader["vergiNo"] as string,
                                vergiDairesi = reader["vergiDairesi"] as string,
                                adres = reader["adres"] as string,
                                musteriMensei = reader["musteriMensei"] as string,
                                doviz = reader["doviz"] as string
                            };
                            musterilerListesi.Add(musteri);
                        }
                    }
                }
            }
            return musterilerListesi;
        }

        public void TumMusterileriSil(SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string query = "DELETE FROM ProjeFinans_Musteriler";

            using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.ExecuteNonQuery();
            }
        }


        public DataTable FiltreleMusteriBilgileri(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string baseQuery = @"
                        SELECT
                            musteriNo,
                            musteriAdi,
                            vergiDairesi,
                            vergiNo,
                            adres,
                            musteriMensei,
                            doviz
                        FROM ProjeFinans_Musteriler
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


                    string paramName = $"@p{paramIndex++}";
                    string condition = $"LOWER({sqlColumnName}) LIKE {paramName}";
                    conditions.Add(condition);
                    parameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar) { Value = "%" + hamDeger.ToLower() + "%" });
                }

                if (conditions.Any())
                {
                    baseQuery += " AND " + string.Join(" AND ", conditions);
                }

                baseQuery += " ORDER BY musteriAdi";

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

        public string NormalizeColumnName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return columnName;

            return columnName.Replace("ı", "i").Replace("İ", "I").Replace("ş", "s").Replace("Ş", "S")
                            .Replace("ğ", "g").Replace("Ğ", "G").Replace("ü", "u").Replace("Ü", "U")
                            .Replace("ç", "c").Replace("Ç", "C").Replace("ö", "o").Replace("Ö", "O")
                            .ToLower();
        }
    }
}
