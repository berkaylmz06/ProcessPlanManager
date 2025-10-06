using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class TeminatMektuplariRepository : ITeminatMektuplariRepository
    {
        public bool TeminatMektubuKaydet(SqlConnection connection, SqlTransaction transaction, TeminatMektuplari mektup)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string sql = @"
        INSERT INTO ProjeFinans_TeminatMektuplari
        (mektupNo, musteriNo, musteriAdi, paraBirimi, banka, mektupTuru, tutar, vadeTarihi, iadeTarihi, komisyonTutari, komisyonOrani, komisyonVadesi, projeId, kilometreTasiId)
        VALUES
        (@mektupNo, @musteriNo, @musteriAdi, @paraBirimi, @banka, @mektupTuru, @tutar, @vadeTarihi, @iadeTarihi, @komisyonTutari, @komisyonOrani, @komisyonVadesi, @projeId, @kilometreTasiId)";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@mektupNo", mektup.mektupNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@musteriNo", mektup.musteriNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@musteriAdi", mektup.musteriAdi ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@paraBirimi", mektup.paraBirimi ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@banka", mektup.banka ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@mektupTuru", mektup.mektupTuru ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@tutar", mektup.tutar);
                command.Parameters.AddWithValue("@vadeTarihi", mektup.vadeTarihi.HasValue ? (object)mektup.vadeTarihi.Value : DBNull.Value);
                command.Parameters.AddWithValue("@iadeTarihi", mektup.iadeTarihi);
                command.Parameters.AddWithValue("@komisyonTutari", mektup.komisyonTutari);
                command.Parameters.AddWithValue("@komisyonOrani", mektup.komisyonOrani);
                command.Parameters.AddWithValue("@komisyonVadesi", mektup.komisyonVadesi);
                command.Parameters.AddWithValue("@projeId", mektup.projeId.HasValue ? (object)mektup.projeId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@kilometreTasiId", mektup.kilometreTasiId);

                command.ExecuteNonQuery();
            }

            return true;
        }

        public void MektupGuncelle(SqlConnection connection, SqlTransaction transaction, string eskiMektupNo, TeminatMektuplari guncelMektup)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

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
            projeId = @projeId
        WHERE mektupNo = @eskiMektupNo";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@yeniMusteriNo", guncelMektup.musteriNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@musteriAdi", guncelMektup.musteriAdi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@paraBirimi", guncelMektup.paraBirimi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@banka", guncelMektup.banka ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@mektupTuru", guncelMektup.mektupTuru ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@tutar", guncelMektup.tutar);
                cmd.Parameters.AddWithValue("@vadeTarihi", guncelMektup.vadeTarihi.HasValue ? (object)guncelMektup.vadeTarihi.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@iadeTarihi", guncelMektup.iadeTarihi);
                cmd.Parameters.AddWithValue("@komisyonTutari", guncelMektup.komisyonTutari);
                cmd.Parameters.AddWithValue("@komisyonOrani", guncelMektup.komisyonOrani);
                cmd.Parameters.AddWithValue("@komisyonVadesi", guncelMektup.komisyonVadesi);
                cmd.Parameters.AddWithValue("@projeId", guncelMektup.projeId.HasValue ? (object)guncelMektup.projeId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@eskiMektupNo", eskiMektupNo ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        public void MektupSil(SqlConnection connection, SqlTransaction transaction, string mektupNo)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string query = "DELETE FROM ProjeFinans_TeminatMektuplari WHERE mektupNo = @mektupNo";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@mektupNo", mektupNo ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
        public bool MektupNoVarMi(SqlConnection connection, string mektupNo)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            string query = "SELECT COUNT(1) FROM ProjeFinans_TeminatMektuplari WHERE mektupNo = @mektupNo";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@mektupNo", mektupNo ?? (object)DBNull.Value);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        public string GetTeminatMektuplariQuery()
        {
            return @"
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
            p.projeNo,
            t.kilometreTasiId,
            t.projeId
        FROM ProjeFinans_TeminatMektuplari t
        LEFT JOIN ProjeFinans_KilometreTaslari k ON t.kilometreTasiId = k.kilometreTasiId
        LEFT JOIN ProjeFinans_Projeler p ON p.projeId = t.projeId";
        }

        public List<TeminatMektuplari> GetTeminatMektuplari(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            var teminatMektuplari = new List<TeminatMektuplari>();

            using (var cmd = new SqlCommand(GetTeminatMektuplariQuery(), connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var mektup = new TeminatMektuplari
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
                            kilometreTasiId = reader.GetInt32(reader.GetOrdinal("kilometreTasiId")),
                            projeId = !reader.IsDBNull(reader.GetOrdinal("projeId")) ? (int?)reader.GetInt32(reader.GetOrdinal("projeId")) : null
                        };
                        teminatMektuplari.Add(mektup);
                    }
                }
            }

            return teminatMektuplari;
        }
        public void UpdateKilometreTasiAdi(SqlConnection connection, SqlTransaction transaction, string mektupNo, int kilometreTasiId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            if (string.IsNullOrEmpty(mektupNo))
                throw new ArgumentNullException(nameof(mektupNo));

            string query = @"
        UPDATE ProjeFinans_TeminatMektuplari
        SET kilometreTasiId = @kilometreTasiId
        WHERE mektupNo = @mektupNo";

            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kilometreTasiId", kilometreTasiId);
                cmd.Parameters.AddWithValue("@mektupNo", mektupNo);

                cmd.ExecuteNonQuery();
            }
        }
    }
}