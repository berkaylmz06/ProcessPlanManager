using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class MusterilerRepository: IMusterilerRepository
    {
        public void MusteriKaydet(SqlConnection connection, SqlTransaction transaction, Musteriler musteri)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "INSERT INTO ProjeFinans_Musteriler (musteriNo, musteriAdi, vergiNo, vergiDairesi, adres, musteriMensei, doviz) VALUES (@musteriNo, @musteriAdi, @vergiNo, @vergiDairesi, @adres, @musteriMensei, @doviz)";

            using (var cmd = new SqlCommand(query, connection, transaction))
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
        public bool MusteriNoVarMi(SqlConnection connection, string musteriNo)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            string query = "SELECT COUNT(1) FROM ProjeFinans_Musteriler WHERE musteriNo = @musteriNo";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@musteriNo", musteriNo ?? (object)DBNull.Value);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        public Musteriler GetMusteriByMusteriNo(SqlConnection connection, string musteriNo)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            Musteriler musteri = null;
            string query = "SELECT musteriNo, musteriAdi, vergiNo, vergiDairesi, adres, musteriMensei, doviz FROM ProjeFinans_Musteriler WHERE musteriNo = @musteriNo";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@musteriNo", musteriNo ?? (object)DBNull.Value);
                using (var reader = cmd.ExecuteReader())
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
            return musteri;
        }
        public string GetMusterilerQuery() 
        {
            return @"SELECT musteriNo, musteriAdi, vergiNo, vergiDairesi, adres, musteriMensei, doviz FROM ProjeFinans_Musteriler";
        } 

        public List<Musteriler> GetMusteriler(SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            List<Musteriler> musterilerListesi = new List<Musteriler>();

            using (var cmd = new SqlCommand(GetMusterilerQuery(), connection))
            using (var reader = cmd.ExecuteReader())
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

            return musterilerListesi;
        }
        public void TumMusterileriSil(SqlConnection connection, SqlTransaction transaction)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "DELETE FROM ProjeFinans_Musteriler";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public string GetMusterilerAraFormuQuery()
        {
            return @"SELECT musteriNo, musteriAdi FROM ProjeFinans_Musteriler";
        }

        public List<Musteriler> GetMusterilerAraFormu(SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            List<Musteriler> musterilerListesi = new List<Musteriler>();

            using (var cmd = new SqlCommand(GetMusterilerQuery(), connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Musteriler musteri = new Musteriler
                    {
                        musteriNo = reader["musteriNo"] as string,
                        musteriAdi = reader["musteriAdi"] as string,
                    };
                    musterilerListesi.Add(musteri);
                }
            }

            return musterilerListesi;
        }
    }
}
