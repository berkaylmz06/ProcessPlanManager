using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class ProjeKutukRepository : IProjeKutukRepository
    {
        private readonly IProjeIliskiRepository _projeIliskiRepository;

        public ProjeKutukRepository(IProjeIliskiRepository projeIliskiRepository)
        {
            _projeIliskiRepository = projeIliskiRepository ?? throw new ArgumentNullException(nameof(projeIliskiRepository));
        }
        public bool ProjeKutukEkle(ProjeKutuk kutuk, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string projeNo = kutuk.projeNo.Trim();
            string kok = projeNo.Split('.')[0].Trim();


            string checkSql = @"
            SELECT COUNT(*) 
            FROM ProjeFinans_ProjeKutuk 
            WHERE LEFT(TRIM(projeNo), 5) = @kok";

            using (SqlCommand checkCommand = new SqlCommand(checkSql, transaction.Connection, transaction))
            {
                checkCommand.Parameters.AddWithValue("@kok", kok);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    return false;
                }
            }

            string sql = @"
            INSERT INTO ProjeFinans_ProjeKutuk 
            (projeId, musteriNo, musteriAdi, isFirsatiNo, projeNo, altProjeVarMi, digerProjeIliskisiVarMi, siparisSozlesmeTarihi, paraBirimi, toplamBedel, faturalamaSekli, nakliyeVarMi)
            VALUES 
            (@projeId, @musteriNo, @musteriAdi, @isFirsatiNo, @projeNo, @altProjeVarMi, @digerProjeIliskisiVarMi, @siparisSozlesmeTarihi, @paraBirimi, @toplamBedel, @faturalamaSekli, @nakliyeVarMi)";

            using (SqlCommand command = new SqlCommand(sql, transaction.Connection, transaction))
            {
                command.Parameters.AddWithValue("@projeId", kutuk.projeId);
                command.Parameters.AddWithValue("@musteriNo", kutuk.musteriNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@musteriAdi", kutuk.musteriAdi ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@isFirsatiNo", kutuk.isFirsatiNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@projeNo", kutuk.projeNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@altProjeVarMi", kutuk.altProjeVarMi);
                command.Parameters.AddWithValue("@digerProjeIliskisiVarMi", string.IsNullOrEmpty(kutuk.digerProjeIliskisiVarMi) ? (object)DBNull.Value : kutuk.digerProjeIliskisiVarMi);
                command.Parameters.AddWithValue("@siparisSozlesmeTarihi", kutuk.siparisSozlesmeTarihi);
                command.Parameters.AddWithValue("@paraBirimi", kutuk.paraBirimi ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@toplamBedel", kutuk.toplamBedel);
                command.Parameters.AddWithValue("@faturalamaSekli", kutuk.faturalamaSekli ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@nakliyeVarMi", kutuk.nakliyeVarMi);

                command.ExecuteNonQuery();
            }
            return true;
        }
        public bool ProjeFiyatlandirmaEkle(string projeNo, decimal fiyat, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string checkSorgu = "SELECT COUNT(*) FROM ProjeFinans WHERE projeNo = @projeNo";
            using (SqlCommand checkKomut = new SqlCommand(checkSorgu, transaction.Connection, transaction))
            {
                checkKomut.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                int count = (int)checkKomut.ExecuteScalar();

                string sorgu;
                if (count > 0)
                {
                    sorgu = @"
                    UPDATE ProjeFinans
                    SET fiyat = @fiyat
                    WHERE projeNo = @projeNo";
                }
                else
                {
                    sorgu = @"
                    INSERT INTO ProjeFinans (projeNo, fiyat)
                    VALUES (@projeNo, @fiyat)";
                }

                using (SqlCommand komut = new SqlCommand(sorgu, transaction.Connection, transaction))
                {
                    komut.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                    komut.Parameters.AddWithValue("@fiyat", fiyat);
                    int affectedRows = komut.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
        }

        public bool AltProjeEkle(int ustProjeId, int altProjeId, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string sql = @"
            INSERT INTO ProjeFinans_ProjeIliski 
            (ustProjeId, altProjeId)
            VALUES 
            (@ustProjeId, @altProjeId)";

            using (SqlCommand command = new SqlCommand(sql, transaction.Connection, transaction))
            {
                command.Parameters.AddWithValue("@ustProjeId", ustProjeId);
                command.Parameters.AddWithValue("@altProjeId", altProjeId);
                command.ExecuteNonQuery();
            }

            return true;
        }

        public bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string sorgu = @"
            INSERT INTO ProjeFinans_Projeler (projeNo, aciklama, projeAdi, olusturmaTarihi)
            VALUES (@projeNo, @aciklama, @projeAdi, @olusturmaTarihi)";

            using (SqlCommand komut = new SqlCommand(sorgu, transaction.Connection, transaction))
            {
                komut.Parameters.AddWithValue("@projeNo", projeNo);
                komut.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                komut.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                komut.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                komut.ExecuteNonQuery();
            }

            return true;
        }


        public bool UpdateProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            ProjeBilgi mevcutBilgi = GetProjeBilgileri(projeNo);
            if (mevcutBilgi == null)
            {
                return false;
            }

            bool degisiklikVar =
                (mevcutBilgi.Aciklama ?? "") != (aciklama ?? "") ||
                (mevcutBilgi.ProjeAdi ?? "") != (projeAdi ?? "") ||
                mevcutBilgi.OlusturmaTarihi != olusturmaTarihi;

            if (!degisiklikVar)
            {
                return false;
            }

            string sorgu = @"
            UPDATE ProjeFinans_Projeler
            SET aciklama = @aciklama, 
                projeAdi = @projeAdi, 
                olusturmaTarihi = @olusturmaTarihi
            WHERE projeNo = @projeNo";

            using (SqlCommand command = new SqlCommand(sorgu, transaction.Connection, transaction))
            {
                command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                command.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                command.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                command.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                command.ExecuteNonQuery();

                return true;
            }
        }


        public ProjeBilgi GetProjeBilgileri(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string sql = @"
                        SELECT projeNo, projeAdi, aciklama, olusturmaTarihi
                        FROM ProjeFinans_Projeler
                        WHERE TRIM(projeNo) = @projeNo";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
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
        }

        public ProjeKutuk ProjeKutukAra(int projeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string sql = @"
                SELECT 
                    projeKutukId, projeId, musteriNo, musteriAdi, isFirsatiNo, projeNo,
                    altProjeVarMi, digerProjeIliskisiVarMi, siparisSozlesmeTarihi, paraBirimi, toplamBedel, faturalamaSekli, nakliyeVarMi, status
                FROM ProjeFinans_ProjeKutuk
                WHERE projeId = @projeId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@projeId", projeId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var kutuk = new ProjeKutuk
                            {
                                projeKutukId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                projeId = reader.GetInt32(1),
                                musteriNo = reader.IsDBNull(2) ? null : reader.GetString(2),
                                musteriAdi = reader.IsDBNull(3) ? null : reader.GetString(3),
                                isFirsatiNo = reader.IsDBNull(4) ? null : reader.GetString(4),
                                projeNo = reader.IsDBNull(5) ? null : reader.GetString(5),
                                altProjeVarMi = reader.IsDBNull(6) ? false : reader.GetBoolean(6),
                                digerProjeIliskisiVarMi = reader.IsDBNull(7) ? "0" : reader.GetString(7),
                                siparisSozlesmeTarihi = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8),
                                paraBirimi = reader.IsDBNull(9) ? null : reader.GetString(9),
                                toplamBedel = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10),
                                faturalamaSekli = reader.IsDBNull(11) ? null : reader.GetString(11),
                                nakliyeVarMi = reader.IsDBNull(12) ? false : reader.GetBoolean(12),
                                status = reader.IsDBNull(13) ? null : reader.GetString(13),
                                altProjeBilgileri = new List<int>()
                            };

                            reader.Close();

                            if (kutuk.altProjeVarMi)
                            {
                                string altProjeSql = @"
                                SELECT altProjeId
                                FROM ProjeFinans_ProjeIliski
                                WHERE ustProjeId = @projeId";
                                using (SqlCommand altCommand = new SqlCommand(altProjeSql, connection))
                                {
                                    altCommand.Parameters.AddWithValue("@projeId", projeId);
                                    using (var altReader = altCommand.ExecuteReader())
                                    {
                                        while (altReader.Read())
                                        {
                                            kutuk.altProjeBilgileri.Add(altReader.IsDBNull(0) ? 0 : altReader.GetInt32(0));
                                        }
                                    }
                                }
                            }

                            return kutuk;
                        }
                    }
                }
                return null;
            }
        }
        public void UpdateToplamBedel(SqlTransaction transaction, string projeNo, decimal toplamBedel)
        {
            string query = @"
            UPDATE ProjeFinans_ProjeKutuk
            SET toplamBedel = @toplamBedel
            WHERE projeNo = @projeNo";

            using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeNo", projeNo);
                cmd.Parameters.AddWithValue("@toplamBedel", toplamBedel);
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsFaturalamaSekliTekil(int projeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string sql = @"
                        SELECT faturalamaSekli
                        FROM ProjeFinans_ProjeKutuk
                        WHERE projeId = @projeId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@projeId", projeId);
                    var result = command.ExecuteScalar();
                    return result != null && result.ToString().Trim().ToLower() == "tekil";
                }
            }
        }
        public bool HasRelatedRecords(int projeId, List<int> altProjeler)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();

                var projeler = new List<int> { projeId };
                if (altProjeler != null && altProjeler.Any())
                {
                    projeler.AddRange(altProjeler);
                }

                var ilgiliTablolar = new List<string>();

                string fiyatlandirmaSql = @"
                        SELECT COUNT(*) 
                        FROM ProjeFinans_Fiyatlandirma 
                        WHERE projeId IN ({0})";

                string odemeSartlariSql = @"
                        SELECT COUNT(*) 
                        FROM ProjeFinans_OdemeSartlari 
                        WHERE projeId IN ({0})";

                string sevkiyatSql = @"
                        SELECT COUNT(*) 
                        FROM ProjeFinans_Sevkiyat 
                        WHERE projeId IN ({0})";

                var parameters = projeler.Select((p, i) => "@p" + i).ToList();
                string inClause = string.Join(",", parameters);
                fiyatlandirmaSql = string.Format(fiyatlandirmaSql, inClause);
                odemeSartlariSql = string.Format(odemeSartlariSql, inClause);
                sevkiyatSql = string.Format(sevkiyatSql, inClause);

                using (SqlCommand command = new SqlCommand(fiyatlandirmaSql, connection))
                {
                    for (int i = 0; i < projeler.Count; i++)
                    {
                        command.Parameters.AddWithValue("@p" + i, projeler[i]);
                    }
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        ilgiliTablolar.Add("Fiyatlandırma");
                    }
                }

                using (SqlCommand command = new SqlCommand(odemeSartlariSql, connection))
                {
                    for (int i = 0; i < projeler.Count; i++)
                    {
                        command.Parameters.AddWithValue("@p" + i, projeler[i]);
                    }
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        ilgiliTablolar.Add("Ödeme Şartları");
                    }
                }

                using (SqlCommand command = new SqlCommand(sevkiyatSql, connection))
                {
                    for (int i = 0; i < projeler.Count; i++)
                    {
                        command.Parameters.AddWithValue("@p" + i, projeler[i]);
                    }
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        ilgiliTablolar.Add("Sevkiyat");
                    }
                }

                if (ilgiliTablolar.Any())
                {
                    return true;
                }

                return false;
            }
        }

        public bool ProjeKutukGuncelle(SqlTransaction transaction, ProjeKutuk yeniKutuk)
        {
            string yeniProjeNo = yeniKutuk.projeNo.Trim();
            string kok = yeniProjeNo.Split('.')[0].Trim();

            string checkSql = @"
            SELECT COUNT(*) 
            FROM ProjeFinans_ProjeKutuk 
            WHERE LEFT(TRIM(projeNo), 5) = @kok AND TRIM(projeNo) <> @projeNo";

            using (SqlCommand checkCommand = new SqlCommand(checkSql, transaction.Connection, transaction))
            {
                checkCommand.Parameters.AddWithValue("@kok", kok);
                checkCommand.Parameters.AddWithValue("@projeNo", yeniProjeNo);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    return false;
                }
            }

            string sql = @"
            UPDATE ProjeFinans_ProjeKutuk 
            SET projeId = @projeId,
                musteriNo = @musteriNo, 
                musteriAdi = @musteriAdi, 
                isFirsatiNo = @isFirsatiNo, 
                altProjeVarMi = @altProjeVarMi, 
                digerProjeIliskisiVarMi = @digerProjeIliskisiVarMi, 
                siparisSozlesmeTarihi = @siparisSozlesmeTarihi, 
                paraBirimi = @paraBirimi, 
                toplamBedel = @toplamBedel, 
                faturalamaSekli = @faturalamaSekli, 
                nakliyeVarMi = @nakliyeVarMi
            WHERE projeNo = @projeNo";

            using (SqlCommand command = new SqlCommand(sql, transaction.Connection, transaction))
            {
                command.Parameters.AddWithValue("@projeId", yeniKutuk.projeId);
                command.Parameters.AddWithValue("@musteriNo", yeniKutuk.musteriNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@musteriAdi", yeniKutuk.musteriAdi ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@isFirsatiNo", yeniKutuk.isFirsatiNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@projeNo", yeniKutuk.projeNo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@altProjeVarMi", yeniKutuk.altProjeVarMi);
                command.Parameters.AddWithValue("@digerProjeIliskisiVarMi", string.IsNullOrEmpty(yeniKutuk.digerProjeIliskisiVarMi) ? (object)DBNull.Value : yeniKutuk.digerProjeIliskisiVarMi);
                command.Parameters.AddWithValue("@siparisSozlesmeTarihi", yeniKutuk.siparisSozlesmeTarihi);
                command.Parameters.AddWithValue("@paraBirimi", yeniKutuk.paraBirimi ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@toplamBedel", yeniKutuk.toplamBedel);
                command.Parameters.AddWithValue("@faturalamaSekli", yeniKutuk.faturalamaSekli ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@nakliyeVarMi", yeniKutuk.nakliyeVarMi);
                command.ExecuteNonQuery();
            }

            if (yeniKutuk.altProjeVarMi)
            {
                string altProjeSilSql = @"
                DELETE FROM ProjeFinans_ProjeIliski 
                WHERE ustProjeId = @projeNo";

                using (SqlCommand command = new SqlCommand(altProjeSilSql, transaction.Connection, transaction))
                {
                    command.Parameters.AddWithValue("@projeNo", yeniKutuk.projeNo.Trim());
                    command.ExecuteNonQuery();
                }

                foreach (var altProje in yeniKutuk.altProjeBilgileri)
                {
                    string altProjeEkleSql = @"
                    INSERT INTO ProjeFinans_ProjeIliski 
                    (ustProjeId, altProjeId)
                    VALUES 
                    (@ustProjeId, @altProjeId)";

                    using (SqlCommand command = new SqlCommand(altProjeEkleSql, transaction.Connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ustProjeId", yeniKutuk.projeNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@altProjeId", altProje != 0 ? (object)altProje : DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }

            return true;
        }
        public bool ProjeKutukSil(SqlTransaction transaction, string projeNo, List<string> altProjeler)
        {
            var projeler = new List<string> { projeNo };
            if (altProjeler != null && altProjeler.Any())
            {
                projeler.AddRange(altProjeler);
            }

            var parameters = projeler.Select((p, i) => "@p" + i).ToList();
            string inClause = string.Join(",", parameters);

            string projeKutukSilSql = $"DELETE FROM ProjeFinans_ProjeKutuk WHERE projeNo IN ({inClause})";
            using (SqlCommand command = new SqlCommand(projeKutukSilSql, transaction.Connection, transaction))
            {
                for (int i = 0; i < projeler.Count; i++)
                    command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
                command.ExecuteNonQuery();
            }

            string altProjeIliskiSilSql = $"DELETE FROM ProjeFinans_ProjeIliski WHERE ustProjeId IN ({inClause}) OR altProjeId IN ({inClause})";
            using (SqlCommand command = new SqlCommand(altProjeIliskiSilSql, transaction.Connection, transaction))
            {
                for (int i = 0; i < projeler.Count; i++)
                    command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
                command.ExecuteNonQuery();
            }

            string projeFinansSilSql = $"DELETE FROM ProjeFinans_Projeler WHERE projeNo IN ({inClause})";
            using (SqlCommand command = new SqlCommand(projeFinansSilSql, transaction.Connection, transaction))
            {
                for (int i = 0; i < projeler.Count; i++)
                    command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
                command.ExecuteNonQuery();
            }

            return true;
        }

        //public static bool ProjeKutukSil(string projeNo, List<string> altProjeler)
        //{
        //    using (var connection = DataBaseHelper.GetConnection())
        //    {
        //        try
        //        {
        //            connection.Open();

        //            using (var transaction = connection.BeginTransaction())
        //            {
        //                try
        //                {
        //                    var projeler = new List<string> { projeNo };
        //                    if (altProjeler != null && altProjeler.Any())
        //                    {
        //                        projeler.AddRange(altProjeler);
        //                    }

        //                    var parameters = projeler.Select((p, i) => "@p" + i).ToList();
        //                    string inClause = string.Join(",", parameters);

        //                    string projeKutukSilSql = @"
        //                        DELETE FROM ProjeFinans_ProjeKutuk 
        //                        WHERE projeNo IN ({0})";
        //                    projeKutukSilSql = string.Format(projeKutukSilSql, inClause);

        //                    using (SqlCommand command = new SqlCommand(projeKutukSilSql, connection, transaction))
        //                    {
        //                        for (int i = 0; i < projeler.Count; i++)
        //                        {
        //                            command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
        //                        }
        //                        command.ExecuteNonQuery();
        //                    }

        //                    string altProjeIliskiSilSql = @"
        //                        DELETE FROM ProjeFinans_ProjeIliski 
        //                        WHERE ustProjeId IN ({0}) OR altProjeId IN ({0})";
        //                    altProjeIliskiSilSql = string.Format(altProjeIliskiSilSql, inClause);

        //                    using (SqlCommand command = new SqlCommand(altProjeIliskiSilSql, connection, transaction))
        //                    {
        //                        for (int i = 0; i < projeler.Count; i++)
        //                        {
        //                            command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
        //                        }
        //                        command.ExecuteNonQuery();
        //                    }

        //                    string projeFinansSilSql = @"
        //                        DELETE FROM ProjeFinans_Projeler 
        //                        WHERE projeNo IN ({0})";
        //                    projeFinansSilSql = string.Format(projeFinansSilSql, inClause);

        //                    using (SqlCommand command = new SqlCommand(projeFinansSilSql, connection, transaction))
        //                    {
        //                        for (int i = 0; i < projeler.Count; i++)
        //                        {
        //                            command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
        //                        }
        //                        command.ExecuteNonQuery();
        //                    }

        //                    transaction.Commit();
        //                    return true;
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    MessageBox.Show($"Proje silinirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return false;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Bağlantı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //}
        public string GetProjeParaBirimi(int projeId)
        {
            int? ustProjeId = _projeIliskiRepository.GetUstProjeId(projeId);
            int hedefProjeId = ustProjeId.HasValue ? ustProjeId.Value : projeId;

            ProjeKutuk kutuk = ProjeKutukAra(hedefProjeId);
            return kutuk?.paraBirimi;
        }
    }
}
