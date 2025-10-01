using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.KesimTakip
{
    public class KesimListesiRepository : IKesimListesiRepository
    {
        public void SaveKesimData(SqlConnection connection, SqlTransaction transaction, string olusturan, string kesimId, string projeno, string malzeme, string kalite, string[] kaliplar, string[] pozlar, decimal[] adetler, DateTime eklemeTarihi)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (pozlar.Length != adetler.Length || pozlar.Length != kaliplar.Length)
                throw new ArgumentException("Pozlar, kalıp numaraları ve adet sayıları eşleşmiyor!");

            for (int i = 0; i < pozlar.Length; i++)
            {
                string temizPoz = pozlar[i].Trim();
                string temizKalip = kaliplar[i].Trim();
                decimal adet = adetler[i];

                if (!string.IsNullOrEmpty(temizPoz) && !string.IsNullOrEmpty(temizKalip))
                {
                    string query = @"INSERT INTO KesimListesi 
                            (olusturan, kesimId, projeNo, malzeme, kalite, kalipNo, kesilecekPozlar, kpAdetSayilari, eklemeTarihi)
                            VALUES 
                            (@olusturan, @kesimId, @projeNo, @malzeme, @kalite, @kalipNo, @kesilecekPozlar, @kpAdetSayilari, @eklemeTarihi)";

                    using (var cmd = new SqlCommand(query, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@olusturan", olusturan ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@kesimId", kesimId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@projeNo", projeno ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@malzeme", malzeme ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@kalipNo", temizKalip ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@kesilecekPozlar", temizPoz ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@kpAdetSayilari", adet);
                        cmd.Parameters.AddWithValue("@eklemeTarihi", eklemeTarihi);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public List<KesimListesi> GetKesimListesi(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            try
            {
                var veriler = new List<KesimListesi>();
                string query = @"
            SELECT 
                olusturan, 
                kesimId, 
                projeNo, 
                kalite, 
                malzeme, 
                kalipNo, 
                kesilecekPozlar, 
                kpAdetSayilari, 
                eklemeTarihi 
            FROM KesimListesi";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            veriler.Add(new KesimListesi
                            {
                                olusturan = reader.GetString(0),
                                kesimId = reader.GetString(1),
                                projeNo = reader.GetString(2),
                                kalite = reader.GetString(3),
                                malzeme = reader.GetString(4),
                                kalipNo = reader.GetString(5),
                                kesilecekPozlar = reader.GetString(6),
                                kpAdetSayilari = reader.GetString(7),
                                eklemeTarihi = reader.GetDateTime(8)
                            });
                        }
                    }
                }

                return veriler;
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetirKesimListesi(SqlConnection connection, string kesimId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            DataTable dt = new DataTable();

            try
            {
                string query = "SELECT * FROM KesimListesi WHERE kesimId = @kesimId";

                using (var da = new SqlDataAdapter(query, connection))
                {
                    da.SelectCommand.Parameters.AddWithValue("@kesimId", kesimId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Veri yüklenirken hata oluştu: " + ex.Message);
            }

            return dt;
        }
        public bool KesimListesiSil(SqlConnection connection, SqlTransaction transaction, int id)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string query = "DELETE FROM KesimListesi WHERE id = @id";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public bool KesimListesiSilByKesimId(SqlConnection connection, SqlTransaction transaction, string kesimId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            if (string.IsNullOrEmpty(kesimId))
                throw new ArgumentNullException(nameof(kesimId));

            try
            {
                string query = "DELETE FROM KesimListesi WHERE kesimId = @kesimId";

                using (var cmd = new SqlCommand(query, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@kesimId", kesimId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"KesimListesi tablosundan kesimId: {kesimId} için veriler silinirken hata oluştu: {ex.Message}");
            }
        }
    }
}
