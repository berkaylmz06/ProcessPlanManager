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
    public class FinansProjelerRepository: IFinansProjelerRepository
    {
        public bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, SqlTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            using (SqlCommand komut = new SqlCommand(@"
        INSERT INTO ProjeFinans_Projeler (projeNo, aciklama, projeAdi, olusturmaTarihi)
        VALUES (@projeNo, @aciklama, @projeAdi, @olusturmaTarihi)", transaction.Connection, transaction))
            {
                komut.Parameters.AddWithValue("@projeNo", projeNo);
                komut.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                komut.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                komut.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);

                komut.ExecuteNonQuery();
                return true;
            }
        }


        public bool UpdateProjeFinans(int projeId, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, SqlTransaction transaction, out bool degisiklikVar)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            ProjeBilgi mevcutBilgi = GetProjeBilgileri(projeId);
            if (mevcutBilgi == null)
            {
                degisiklikVar = false;
                return false;
            }

            degisiklikVar =
                (mevcutBilgi.ProjeNo ?? "") != (projeNo ?? "") ||
                (mevcutBilgi.Aciklama ?? "") != (aciklama ?? "") ||
                (mevcutBilgi.ProjeAdi ?? "") != (projeAdi ?? "") ||
                mevcutBilgi.OlusturmaTarihi != olusturmaTarihi;

            if (!degisiklikVar)
            {
                return true;
            }

            string sorgu = @"
        UPDATE ProjeFinans_Projeler
        SET projeNo = @projeNo,
            aciklama = @aciklama, 
            projeAdi = @projeAdi, 
            olusturmaTarihi = @olusturmaTarihi
        WHERE projeId = @projeId";

            using (SqlCommand command = new SqlCommand(sorgu, transaction.Connection, transaction))
            {
                command.Parameters.AddWithValue("@projeId", projeId);
                command.Parameters.AddWithValue("@projeNo", projeNo);
                command.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                command.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                command.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                command.ExecuteNonQuery();
                return true;
            }
        }

        public ProjeBilgi GetProjeBilgileri(int projeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string sql = @"
                SELECT projeNo, projeAdi, aciklama, olusturmaTarihi
                FROM ProjeFinans_Projeler
                WHERE projeId = @projeId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@projeId", projeId);
                    Console.WriteLine($"SQL Sorgusu için projeId: '{projeId}'");
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Kayıt bulundu.");
                            return new ProjeBilgi
                            {
                                ProjeNo = reader.IsDBNull(0) ? null : reader.GetString(0),
                                ProjeAdi = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Aciklama = reader.IsDBNull(2) ? null : reader.GetString(2),
                                OlusturmaTarihi = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3)
                            };
                        }
                        else
                        {
                            Console.WriteLine("Kayıt bulunamadı.");
                        }
                    }
                }
                return null;
            }
        }
        public bool ProjeSil(int projeId, SqlTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = "DELETE FROM ProjeFinans_Projeler WHERE projeId = @projeId";
            using (SqlCommand command = new SqlCommand(sql, transaction.Connection, transaction))
            {
                command.Parameters.AddWithValue("@projeId", projeId);
                command.ExecuteNonQuery();
            }
            return true;
        }

        public int? GetProjeIdByNo(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT projeId FROM ProjeFinans_Projeler WHERE projeNo = @projeNo";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeNo", projeNo);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            return null;
        }
        public string GetProjeNoById(int projeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT projeNo FROM ProjeFinans_Projeler WHERE projeId = @projeId";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeId", projeId);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                }
            }
            return null;
        }
        public ProjeBilgi GetProjeBilgileriByNo(string projeNo)
        {
            int? projeId = GetProjeIdByNo(projeNo);
            return projeId.HasValue ? GetProjeBilgileri(projeId.Value) : null;
        }
    }
}
