using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Entitys.Genel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CEKA_APP.Concretes.Genel
{
    public class SayfaStatusRepository: ISayfaStatusRepository
    {
        public int Insert(SqlConnection connection, SqlTransaction transaction, SayfaStatus status)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
INSERT INTO SayfaStatus (sayfaId, projeId, bilgilerTamamMi, status, nedenTamamlanmadi)
VALUES (@sayfaId, @projeId, @bilgilerTamamMi, @status, @nedenTamamlanmadi);
SELECT SCOPE_IDENTITY();";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@sayfaId", status.sayfaId);
                cmd.Parameters.AddWithValue("@projeId", status.projeId);
                cmd.Parameters.AddWithValue("@bilgilerTamamMi", status.bilgilerTamamMi);
                cmd.Parameters.AddWithValue("@status", status.status);
                cmd.Parameters.AddWithValue("@nedenTamamlanmadi", status.nedenTamamlanmadi);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public SayfaStatus Get(SqlConnection connection, int sayfaId, int projeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            string query = @"
SELECT sayfaStatusId, sayfaId, projeId, bilgilerTamamMi, status, nedenTamamlanmadi
FROM SayfaStatus
WHERE sayfaId = @sayfaId AND projeId = @projeId";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@sayfaId", sayfaId);
                cmd.Parameters.AddWithValue("@projeId", projeId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new SayfaStatus
                        {
                            sayfaStatusId = reader.GetInt32(reader.GetOrdinal("sayfaStatusId")),
                            sayfaId = reader.GetInt32(reader.GetOrdinal("sayfaId")),
                            projeId = reader.GetInt32(reader.GetOrdinal("projeId")),
                            bilgilerTamamMi = reader.GetBoolean(reader.GetOrdinal("bilgilerTamamMi")),
                            status = reader.GetString(reader.GetOrdinal("status")),
                            nedenTamamlanmadi = reader.IsDBNull(reader.GetOrdinal("nedenTamamlanmadi"))
                                                ? string.Empty
                                                : reader.GetString(reader.GetOrdinal("nedenTamamlanmadi"))
                        };
                    }
                }
            }

            return null;
        }
        public List<string> GetNedenTamamlanmadiByProjeId(SqlConnection connection, int projeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            var sonuc = new List<string>();

            string projeNo = projeId.ToString();
            using (var cmd = new SqlCommand("SELECT projeNo FROM ProjeFinans_Projeler WHERE projeId = @projeId", connection))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    projeNo = result.ToString();
            }

            List<int> altProjeler = new List<int>();
            string altProjeQuery = @"SELECT altProjeId FROM ProjeFinans_ProjeIliski WHERE ustProjeId = @ustProjeId";
            using (var cmd = new SqlCommand(altProjeQuery, connection))
            {
                cmd.Parameters.AddWithValue("@ustProjeId", projeId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        altProjeler.Add(reader.GetInt32(reader.GetOrdinal("altProjeId")));
                    }
                }
            }

            bool ustProjeVeriVar = false;
            using (var cmd = new SqlCommand("SELECT nedenTamamlanmadi FROM SayfaStatus WHERE projeId = @projeId AND sayfaId = 3", connection))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ustProjeVeriVar = true;
                        string veri = reader.IsDBNull(reader.GetOrdinal("nedenTamamlanmadi"))
                            ? string.Empty
                            : reader.GetString(reader.GetOrdinal("nedenTamamlanmadi"));

                        if (!string.IsNullOrEmpty(veri))
                        {
                            foreach (var parca in veri.Split(';').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)))
                            {
                                sonuc.Add($"Ödeme Şartları Proje {projeNo}: {parca}");
                            }
                        }
                    }
                }
            }

            if (!ustProjeVeriVar)
            {
                sonuc.Add($" Proje {projeNo}: Ödeme Şartları bilgileri girilmemiş.");
            }

            if (altProjeler.Any())
            {
                string altProjeIds = string.Join(",", altProjeler);

                var altProjelerDict = new Dictionary<int, string>();
                string altProjelerQuery = $"SELECT projeId, projeNo FROM ProjeFinans_Projeler WHERE projeId IN ({altProjeIds})";
                using (var cmd = new SqlCommand(altProjelerQuery, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int altId = reader.GetInt32(reader.GetOrdinal("projeId"));
                            string altNo = reader.IsDBNull(reader.GetOrdinal("projeNo")) ? altId.ToString() : reader.GetString(reader.GetOrdinal("projeNo"));
                            altProjelerDict.Add(altId, altNo);
                        }
                    }
                }

                string altSayfaQuery = $"SELECT projeId, nedenTamamlanmadi FROM SayfaStatus WHERE projeId IN ({altProjeIds}) AND sayfaId = 4";
                var altProjelerVeriVar = new HashSet<int>();
                using (var cmd = new SqlCommand(altSayfaQuery, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int altId = reader.GetInt32(reader.GetOrdinal("projeId"));
                            altProjelerVeriVar.Add(altId);

                            string veri = reader.IsDBNull(reader.GetOrdinal("nedenTamamlanmadi"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("nedenTamamlanmadi"));

                            string altProjeNo = altProjelerDict.ContainsKey(altId) ? altProjelerDict[altId] : altId.ToString();

                            if (!string.IsNullOrEmpty(veri))
                            {
                                foreach (var parca in veri.Split(';').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)))
                                {
                                    sonuc.Add($"Sevkiyat Proje {altProjeNo}: {parca}");
                                }
                            }
                        }
                    }
                }

                foreach (var altId in altProjeler)
                {
                    if (!altProjelerVeriVar.Contains(altId))
                    {
                        string altProjeNo = altProjelerDict.ContainsKey(altId) ? altProjelerDict[altId] : altId.ToString();
                        sonuc.Add($" Proje {altProjeNo}: Sevkiyat bilgileri girilmemiş.");
                    }
                }
            }

            string montajQuery = @"SELECT COUNT(*) FROM ProjeFinans_ProjeKutuk WHERE projeId = @projeId AND montajTamamlandiMi = 0";
            using (var cmd = new SqlCommand(montajQuery, connection))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                int eksikMontaj = (int)cmd.ExecuteScalar();
                if (eksikMontaj > 0)
                {
                    sonuc.Add($"Proje {projeNo}: Montaj Tamamlanmadı.");
                }
            }

            return sonuc;
        }

        public void Update(SqlConnection connection, SqlTransaction transaction, SayfaStatus status)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
UPDATE SayfaStatus
SET bilgilerTamamMi=@bilgilerTamamMi, status=@status ,nedenTamamlanmadi=@nedenTamamlanmadi
WHERE sayfaStatusId=@id";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@bilgilerTamamMi", status.bilgilerTamamMi);
                cmd.Parameters.AddWithValue("@status", status.status);
                cmd.Parameters.AddWithValue("@nedenTamamlanmadi", status.nedenTamamlanmadi);
                cmd.Parameters.AddWithValue("@id", status.sayfaStatusId);

                cmd.ExecuteNonQuery();
            }
        }
        public void Delete(SqlConnection connection, SqlTransaction transaction, int sayfaStatusId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "DELETE FROM SayfaStatus WHERE sayfaStatusId=@id";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@id", sayfaStatusId);
                cmd.ExecuteNonQuery();
            }
        }
        public bool IsAllAltProjelerSayfa4Kapali(SqlConnection connection, SqlTransaction transaction, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            string altProjelerQuery = @"
SELECT altProjeId
FROM ProjeFinans_ProjeIliski
WHERE ustProjeId = @projeId";

            List<int> altProjeler = new List<int>();
            using (var cmd = new SqlCommand(altProjelerQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        altProjeler.Add(reader.GetInt32(reader.GetOrdinal("altProjeId")));
                    }
                }
            }

            if (!altProjeler.Any())
            {
                return true;
            }

            string sayfaStatusQuery = @"
SELECT COUNT(*) as KapaliCount
FROM SayfaStatus
WHERE projeId IN (" + string.Join(",", altProjeler) + @") 
  AND sayfaId = 4 
  AND status = 'Kapatıldı'";

            using (var cmd = new SqlCommand(sayfaStatusQuery, connection, transaction))
            {
                int kapaliCount = Convert.ToInt32(cmd.ExecuteScalar());
                return kapaliCount == altProjeler.Count;
            }
        }

        public bool IsSayfa3Kapali(SqlConnection connection, SqlTransaction transaction, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            string query = @"
SELECT 1
FROM SayfaStatus
WHERE projeId = @projeId
  AND sayfaId = 3
  AND status = 'Kapatıldı'";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }
    }
}
