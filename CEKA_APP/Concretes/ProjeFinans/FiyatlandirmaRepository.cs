using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class FiyatlandirmaRepository : IFiyatlandirmaRepository
    {
        public List<Fiyatlandirma> GetFiyatlandirmaByProje(SqlConnection connection, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var result = new List<Fiyatlandirma>();
            string query = @"
    SELECT 
        f.fiyatlandirmaId,
        f.projeId,
        f.fiyatlandirmaKalemId,
        f.teklifBirimMiktar,
        f.teklifBirimFiyat,
        f.gerceklesenBirimMiktar,
        f.gerceklesenBirimFiyat,
        f.teklifDovizKodu,
        f.teklifKuru,
        k.kalemAdi,
        k.kalemBirimi
    FROM ProjeFinans_Fiyatlandirma f
    LEFT JOIN ProjeFinans_FiyatlandirmaKalemleri k
        ON f.fiyatlandirmaKalemId = k.fiyatlandirmaKalemId
    WHERE f.projeId = @projeId";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var fiyat = new Fiyatlandirma
                        {
                            fiyatlandirmaId = Convert.ToInt32(reader["fiyatlandirmaId"]),
                            projeId = Convert.ToInt32(reader["projeId"]),
                            fiyatlandirmaKalemId = Convert.ToInt32(reader["fiyatlandirmaKalemId"]),
                            teklifBirimMiktar = Convert.ToDecimal(reader["teklifBirimMiktar"]),
                            teklifBirimFiyat = Convert.ToDecimal(reader["teklifBirimFiyat"]),
                            gerceklesenBirimMiktar = Convert.ToDecimal(reader["gerceklesenBirimMiktar"]),
                            gerceklesenBirimFiyat = Convert.ToDecimal(reader["gerceklesenBirimFiyat"]),
                            teklifDovizKodu = reader.IsDBNull(reader.GetOrdinal("teklifDovizKodu")) ? "TL" : reader["teklifDovizKodu"].ToString(),
                            teklifKuru = reader.IsDBNull(reader.GetOrdinal("teklifKuru")) ? null : (decimal?)Convert.ToDecimal(reader["teklifKuru"]),
                            kalem = new FiyatlandirmaKalem
                            {
                                fiyatlandirmaKalemId = Convert.ToInt32(reader["fiyatlandirmaKalemId"]),
                                kalemAdi = reader.IsDBNull(reader.GetOrdinal("kalemAdi")) ? null : reader["kalemAdi"].ToString(),
                                kalemBirimi = reader.IsDBNull(reader.GetOrdinal("kalemBirimi")) ? null : reader["kalemBirimi"].ToString()
                            }
                        };

                        result.Add(fiyat);
                    }
                }
            }

            return result;
        }
        public bool FiyatlandirmaKaydet(SqlConnection connection, SqlTransaction transaction, Fiyatlandirma fiyat)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
        INSERT INTO ProjeFinans_Fiyatlandirma 
        (projeId, fiyatlandirmaKalemId, teklifBirimMiktar, teklifBirimFiyat, gerceklesenBirimMiktar, gerceklesenBirimFiyat, teklifDovizKodu, teklifKuru)
        VALUES (@projeId, @fiyatlandirmaKalemId, @teklifBirimMiktar, @teklifBirimFiyat, @gerceklesenBirimMiktar, @gerceklesenBirimFiyat, @teklifDovizKodu, @teklifKuru)";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", fiyat.projeId);
                cmd.Parameters.AddWithValue("@fiyatlandirmaKalemId", fiyat.fiyatlandirmaKalemId);
                cmd.Parameters.AddWithValue("@teklifBirimMiktar", fiyat.teklifBirimMiktar);
                cmd.Parameters.AddWithValue("@teklifBirimFiyat", fiyat.teklifBirimFiyat);
                cmd.Parameters.AddWithValue("@gerceklesenBirimMiktar", fiyat.gerceklesenBirimMiktar);
                cmd.Parameters.AddWithValue("@gerceklesenBirimFiyat", fiyat.gerceklesenBirimFiyat);
                cmd.Parameters.AddWithValue("@teklifDovizKodu", fiyat.teklifDovizKodu ?? "TL");
                cmd.Parameters.AddWithValue("@teklifKuru", (object)fiyat.teklifKuru ?? DBNull.Value);

                cmd.ExecuteNonQuery();
            }

            return true;
        }
        public bool FiyatlandirmaGuncelle(SqlConnection connection, SqlTransaction transaction, Fiyatlandirma fiyat)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
        UPDATE ProjeFinans_Fiyatlandirma
        SET teklifBirimMiktar = @teklifBirimMiktar,
            teklifBirimFiyat = @teklifBirimFiyat,
            gerceklesenBirimMiktar = @gerceklesenBirimMiktar,
            gerceklesenBirimFiyat = @gerceklesenBirimFiyat,
            teklifDovizKodu = @teklifDovizKodu,
            teklifKuru = @teklifKuru
        WHERE projeId = @projeId AND fiyatlandirmaKalemId = @fiyatlandirmaKalemId";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", fiyat.projeId);
                cmd.Parameters.AddWithValue("@fiyatlandirmaKalemId", fiyat.fiyatlandirmaKalemId);
                cmd.Parameters.AddWithValue("@teklifBirimMiktar", fiyat.teklifBirimMiktar);
                cmd.Parameters.AddWithValue("@teklifBirimFiyat", fiyat.teklifBirimFiyat);
                cmd.Parameters.AddWithValue("@gerceklesenBirimMiktar", fiyat.gerceklesenBirimMiktar);
                cmd.Parameters.AddWithValue("@gerceklesenBirimFiyat", fiyat.gerceklesenBirimFiyat);
                cmd.Parameters.AddWithValue("@teklifDovizKodu", fiyat.teklifDovizKodu ?? "TL");
                cmd.Parameters.AddWithValue("@teklifKuru", (object)fiyat.teklifKuru ?? DBNull.Value);

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public bool FiyatlandirmaSil(SqlConnection connection, SqlTransaction transaction, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "DELETE FROM ProjeFinans_Fiyatlandirma WHERE projeId = @projeId";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public bool FiyatlandirmaSilById(SqlConnection connection, SqlTransaction transaction, int projeId, int fiyatlandirmaKalemId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "DELETE FROM ProjeFinans_Fiyatlandirma WHERE projeId = @projeId AND fiyatlandirmaKalemId = @fiyatlandirmaKalemId";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@fiyatlandirmaKalemId", fiyatlandirmaKalemId);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
        public (decimal toplamBedel, List<int> eksikFiyatlandirmaProjeler) GetToplamBedel(SqlConnection connection, int projeId, List<int> altProjeler = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            decimal toplamBedel = 0;
            var eksikFiyatlandirmaProjeler = new List<int>();

            var projelerToControl = new List<int>();
            if (altProjeler != null && altProjeler.Any())
            {
                projelerToControl.AddRange(altProjeler);
            }

            if (!projelerToControl.Contains(projeId))
            {
                projelerToControl.Add(projeId);
            }

            foreach (var proje in projelerToControl)
            {
                string checkQuery = "SELECT COUNT(*) FROM ProjeFinans_Fiyatlandirma WHERE projeId = @projeId";
                using (var checkCmd = new SqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@projeId", proje);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count == 0)
                    {
                        eksikFiyatlandirmaProjeler.Add(proje);
                        continue;
                    }
                }

                string sumQuery = @"
            SELECT SUM(teklifBirimMiktar * teklifBirimFiyat) as ToplamTeklif
            FROM ProjeFinans_Fiyatlandirma
            WHERE projeId = @projeId AND teklifBirimMiktar IS NOT NULL AND teklifBirimFiyat IS NOT NULL";
                using (var sumCmd = new SqlCommand(sumQuery, connection))
                {
                    sumCmd.Parameters.AddWithValue("@projeId", proje);
                    object result = sumCmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        toplamBedel += Convert.ToDecimal(result);
                    }
                    else
                    {
                        eksikFiyatlandirmaProjeler.Add(proje);
                    }
                }
            }

            return (toplamBedel, eksikFiyatlandirmaProjeler);
        }
    }
}
