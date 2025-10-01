using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class FinansProjelerRepository: IFinansProjelerRepository
    {
        public bool ProjeEkleProjeFinans(SqlConnection connection, SqlTransaction transaction, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            using (SqlCommand komut = new SqlCommand(@"
        INSERT INTO ProjeFinans_Projeler (projeNo, aciklama, projeAdi, olusturmaTarihi)
        VALUES (@projeNo, @aciklama, @projeAdi, @olusturmaTarihi)", connection, transaction))
            {
                komut.Parameters.AddWithValue("@projeNo", projeNo);
                komut.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                komut.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                komut.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);

                komut.ExecuteNonQuery();
                return true;
            }
        }

        public bool UpdateProjeFinans(SqlConnection connection, SqlTransaction transaction, int projeId, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, out bool degisiklikVar)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            ProjeBilgi mevcutBilgi = GetProjeBilgileri(connection, transaction, projeId);
            if (mevcutBilgi == null)
            {
                degisiklikVar = false;
                return false;
            }

            Func<string, string> normalize = (s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim();

            string dbProjeNo = normalize(mevcutBilgi.ProjeNo);
            string uiProjeNo = normalize(projeNo);
            string dbProjeAdi = normalize(mevcutBilgi.ProjeAdi);
            string uiProjeAdi = normalize(projeAdi);
            string dbAciklama = normalize(mevcutBilgi.Aciklama);
            string uiAciklama = normalize(aciklama);

            degisiklikVar =
                !string.Equals(dbProjeNo, uiProjeNo, StringComparison.Ordinal) ||
                !string.Equals(dbProjeAdi, uiProjeAdi, StringComparison.Ordinal) ||
                !string.Equals(dbAciklama, uiAciklama, StringComparison.Ordinal) ||
                mevcutBilgi.OlusturmaTarihi.Date != olusturmaTarihi.Date;

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

            using (SqlCommand command = new SqlCommand(sorgu, connection, transaction))
            {
                command.Parameters.AddWithValue("@projeId", projeId);
                command.Parameters.AddWithValue("@projeNo", projeNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                command.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                command.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);

                command.ExecuteNonQuery();
                return true;
            }
        }
        public ProjeBilgi GetProjeBilgileri(SqlConnection connection, SqlTransaction transaction, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = @"
        SELECT projeNo, projeAdi, aciklama, olusturmaTarihi
        FROM ProjeFinans_Projeler
        WHERE projeId = @projeId";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@projeId", projeId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ProjeBilgi
                        {
                            ProjeNo = reader.IsDBNull(0) ? null : reader.GetString(0),
                            ProjeAdi = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Aciklama = reader.IsDBNull(2) ? null : reader.GetString(2),
                            OlusturmaTarihi = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3)
                        };
                    }
                }
            }
            return null;
        }

        public bool ProjeSil(SqlConnection connection, SqlTransaction transaction, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sql = "DELETE FROM ProjeFinans_Projeler WHERE projeId = @projeId";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@projeId", projeId);
                command.ExecuteNonQuery();
            }

            return true;
        }

        public int? GetProjeIdByNo(SqlConnection connection, SqlTransaction transaction,string projeNo)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "SELECT projeId FROM ProjeFinans_Projeler WHERE projeNo = @projeNo";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeNo", projeNo);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }

            return null;
        }

        public string GetProjeNoById(SqlConnection connection, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = "SELECT projeNo FROM ProjeFinans_Projeler WHERE projeId = @projeId";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
            }

            return null;
        }

        public ProjeBilgi GetProjeBilgileriByNo(SqlConnection connection, SqlTransaction transaction, string projeNo)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (string.IsNullOrEmpty(projeNo)) throw new ArgumentNullException(nameof(projeNo));

            int? projeId = GetProjeIdByNo(connection, transaction, projeNo);
            if (projeId.HasValue)
            {
                return GetProjeBilgileri(connection, transaction, projeId.Value);
            }

            return null;
        }
    }
}
