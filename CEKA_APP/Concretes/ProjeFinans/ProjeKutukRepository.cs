using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class ProjeKutukRepository : IProjeKutukRepository
    {
        private readonly IProjeIliskiRepository _projeIliskiRepository;
        private readonly ISayfaStatusRepository _sayfaStatusRepository;

        public ProjeKutukRepository(IProjeIliskiRepository projeIliskiRepository, ISayfaStatusRepository sayfaStatusRepository)
        {
            _projeIliskiRepository = projeIliskiRepository ?? throw new ArgumentNullException(nameof(projeIliskiRepository));
            _sayfaStatusRepository = sayfaStatusRepository ?? throw new ArgumentNullException(nameof(sayfaStatusRepository));
        }
        public bool ProjeKutukEkle(SqlConnection connection, SqlTransaction transaction, ProjeKutuk kutuk)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string projeNo = kutuk.projeNo.Trim();
            string kok = projeNo.Split('.')[0].Trim();

            string checkSql = @"
        SELECT COUNT(*) 
        FROM ProjeFinans_ProjeKutuk 
        WHERE LEFT(TRIM(projeNo), 5) = @kok";

            using (SqlCommand checkCommand = new SqlCommand(checkSql, connection, transaction))
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
        (projeId, musteriNo, musteriAdi, isFirsatiNo, projeNo, altProjeVarMi, digerProjeIliskisiVarMi, siparisSozlesmeTarihi, paraBirimi, toplamBedel, faturalamaSekli, nakliyeVarMi, montajTamamlandiMi)
        VALUES 
        (@projeId, @musteriNo, @musteriAdi, @isFirsatiNo, @projeNo, @altProjeVarMi, @digerProjeIliskisiVarMi, @siparisSozlesmeTarihi, @paraBirimi, @toplamBedel, @faturalamaSekli, @nakliyeVarMi, @montajTamamlandiMi)";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
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
                command.Parameters.AddWithValue("@montajTamamlandiMi", kutuk.montajTamamlandiMi);

                command.ExecuteNonQuery();
            }

            return true;
        }

        public bool ProjeFiyatlandirmaEkle(SqlConnection connection, SqlTransaction transaction, string projeNo, decimal fiyat)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string checkSorgu = "SELECT COUNT(*) FROM ProjeFinans WHERE projeNo = @projeNo";
            using (SqlCommand checkKomut = new SqlCommand(checkSorgu, connection, transaction))
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

                using (SqlCommand komut = new SqlCommand(sorgu, connection, transaction))
                {
                    komut.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                    komut.Parameters.AddWithValue("@fiyat", fiyat);
                    int affectedRows = komut.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
        }

       

        public bool ProjeEkleProjeFinans(SqlConnection connection, SqlTransaction transaction, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sorgu = @"
        INSERT INTO ProjeFinans_Projeler (projeNo, aciklama, projeAdi, olusturmaTarihi)
        VALUES (@projeNo, @aciklama, @projeAdi, @olusturmaTarihi)";

            using (SqlCommand komut = new SqlCommand(sorgu, connection, transaction))
            {
                komut.Parameters.AddWithValue("@projeNo", projeNo);
                komut.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                komut.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                komut.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                komut.ExecuteNonQuery();
            }

            return true;
        }

        public bool UpdateProjeFinans(SqlConnection connection, SqlTransaction transaction, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            ProjeBilgi mevcutBilgi = GetProjeBilgileri(connection, projeNo);
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

            using (SqlCommand command = new SqlCommand(sorgu, connection, transaction))
            {
                command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                command.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                command.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                command.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                command.ExecuteNonQuery();
            }

            return true;
        }

        public ProjeBilgi GetProjeBilgileri(SqlConnection connection, string projeNo)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

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

        public ProjeKutuk ProjeKutukAra(SqlConnection connection, int projeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string sql = @"
        SELECT 
            projeKutukId, projeId, musteriNo, musteriAdi, isFirsatiNo, projeNo,
            altProjeVarMi, digerProjeIliskisiVarMi, siparisSozlesmeTarihi, paraBirimi, toplamBedel, faturalamaSekli, nakliyeVarMi, montajTamamlandiMi, status
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
                            montajTamamlandiMi = reader.IsDBNull(13) ? false : reader.GetBoolean(13),
                            status = reader.IsDBNull(14) ? null : reader.GetString(14),
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
        public void UpdateToplamBedel(SqlConnection connection, SqlTransaction transaction, string projeNo, decimal toplamBedel)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
        UPDATE ProjeFinans_ProjeKutuk
        SET toplamBedel = @toplamBedel
        WHERE projeNo = @projeNo";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeNo", projeNo);
                cmd.Parameters.AddWithValue("@toplamBedel", toplamBedel);
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsFaturalamaSekliTekil(SqlConnection connection, int projeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

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

        public (bool HasRelated, List<string> Details) HasRelatedRecords(SqlConnection connection, int projeId, List<int> altProjeler)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var details = new List<string>();

            var projeler = new List<int> { projeId };
            if (altProjeler != null && altProjeler.Any())
            {
                projeler.AddRange(altProjeler);
            }

            var parameters = projeler.Select((p, i) => "@p" + i).ToList();
            string inClause = string.Join(",", parameters);

            string fiyatlandirmaSql = $"SELECT COUNT(*) FROM ProjeFinans_Fiyatlandirma WHERE projeId IN ({inClause})";
            using (SqlCommand command = new SqlCommand(fiyatlandirmaSql, connection))
            {
                for (int i = 0; i < projeler.Count; i++)
                {
                    command.Parameters.AddWithValue("@p" + i, projeler[i]);
                }
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    details.Add($"Fiyatlandırma tablosunda {count} kayıt var.");
                }
            }

            string odemeSartlariSql = $"SELECT COUNT(*) FROM ProjeFinans_OdemeSartlari WHERE projeId IN ({inClause})";
            using (SqlCommand command = new SqlCommand(odemeSartlariSql, connection))
            {
                for (int i = 0; i < projeler.Count; i++)
                {
                    command.Parameters.AddWithValue("@p" + i, projeler[i]);
                }
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    details.Add($"Ödeme Şartları tablosunda {count} kayıt var.");
                }
            }

            string sevkiyatSql = $"SELECT COUNT(*) FROM ProjeFinans_Sevkiyat WHERE projeId IN ({inClause})";
            using (SqlCommand command = new SqlCommand(sevkiyatSql, connection))
            {
                for (int i = 0; i < projeler.Count; i++)
                {
                    command.Parameters.AddWithValue("@p" + i, projeler[i]);
                }
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    details.Add($"Sevkiyat tablosunda {count} kayıt var.");
                }
            }

            string teminatSql = $"SELECT COUNT(*) FROM ProjeFinans_TeminatMektuplari WHERE projeId IN ({inClause})";
            using (SqlCommand command = new SqlCommand(teminatSql, connection))
            {
                for (int i = 0; i < projeler.Count; i++)
                {
                    command.Parameters.AddWithValue("@p" + i, projeler[i]);
                }
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    details.Add($"Teminat Mektupları tablosunda {count} kayıt var.");
                }
            }

            bool hasRelated = details.Any();
            return (hasRelated, details);
        }

        public bool ProjeKutukGuncelle(SqlConnection connection, SqlTransaction transaction, ProjeKutuk yeniKutuk)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string yeniProjeNo = yeniKutuk.projeNo.Trim();
            string kok = yeniProjeNo.Split('.')[0].Trim();

            string checkSql = @"
    SELECT COUNT(*) 
    FROM ProjeFinans_ProjeKutuk 
    WHERE LEFT(TRIM(projeNo), 5) = @kok AND TRIM(projeNo) <> @projeNo";

            using (SqlCommand checkCommand = new SqlCommand(checkSql, connection, transaction))
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
        nakliyeVarMi = @nakliyeVarMi,
        montajTamamlandiMi = @montajTamamlandiMi
    WHERE projeNo = @projeNo";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
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
                command.Parameters.AddWithValue("@montajTamamlandiMi", yeniKutuk.montajTamamlandiMi);
                command.ExecuteNonQuery();
            }

            if (yeniKutuk.altProjeVarMi)
            {
                string altProjeSilSql = @"
        DELETE FROM ProjeFinans_ProjeIliski 
        WHERE ustProjeId = @projeId";

                using (SqlCommand command = new SqlCommand(altProjeSilSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@projeId", yeniKutuk.projeId);
                    command.ExecuteNonQuery();
                }

                foreach (var altProje in yeniKutuk.altProjeBilgileri)
                {
                    string altProjeEkleSql = @"
            INSERT INTO ProjeFinans_ProjeIliski 
            (ustProjeId, altProjeId)
            VALUES 
            (@ustProjeId, @altProjeId)";

                    using (SqlCommand command = new SqlCommand(altProjeEkleSql, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ustProjeId", yeniKutuk.projeId);
                        command.Parameters.AddWithValue("@altProjeId", altProje != 0 ? (object)altProje : DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }

            return true;
        }

        public bool ProjeKutukSil(SqlConnection connection, SqlTransaction transaction, int projeId, List<int> altProjeIds)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var projeler = new List<int> { projeId };
            if (altProjeIds != null && altProjeIds.Any())
                projeler.AddRange(altProjeIds);

            var parameters = projeler.Select((p, i) => "@p" + i).ToList();
            string inClause = string.Join(",", parameters);

            string sayfaStatusSilSql = $"DELETE FROM SayfaStatus WHERE projeId IN ({inClause})";
            using (SqlCommand command = new SqlCommand(sayfaStatusSilSql, connection, transaction))
            {
                for (int i = 0; i < projeler.Count; i++)
                    command.Parameters.AddWithValue("@p" + i, projeler[i]);
                command.ExecuteNonQuery();
            }

            string projeKutukSilSql = $"DELETE FROM ProjeFinans_ProjeKutuk WHERE projeId IN ({inClause})";
            using (SqlCommand command = new SqlCommand(projeKutukSilSql, connection, transaction))
            {
                for (int i = 0; i < projeler.Count; i++)
                    command.Parameters.AddWithValue("@p" + i, projeler[i]);
                command.ExecuteNonQuery();
            }

            string altProjeIliskiSilSql =
                $"DELETE FROM ProjeFinans_ProjeIliski WHERE ustProjeId IN ({inClause}) OR altProjeId IN ({inClause})";
            using (SqlCommand command = new SqlCommand(altProjeIliskiSilSql, connection, transaction))
            {
                for (int i = 0; i < projeler.Count; i++)
                    command.Parameters.AddWithValue("@p" + i, projeler[i]);
                command.ExecuteNonQuery();
            }

            string projeFinansSilSql = $"DELETE FROM ProjeFinans_Projeler WHERE projeId IN ({inClause})";
            using (SqlCommand command = new SqlCommand(projeFinansSilSql, connection, transaction))
            {
                for (int i = 0; i < projeler.Count; i++)
                    command.Parameters.AddWithValue("@p" + i, projeler[i]);
                command.ExecuteNonQuery();
            }

            return true;
        }

        public string GetProjeParaBirimi(SqlConnection connection, int projeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            int? ustProjeId = _projeIliskiRepository.GetUstProjeId(connection, projeId);
            int hedefProjeId = ustProjeId ?? projeId;

            ProjeKutuk kutuk = ProjeKutukAra(connection, hedefProjeId);

            return kutuk?.paraBirimi;
        }


        public ProjeKutuk GetProjeKutukStatus(SqlConnection connection, int projeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            const string sql = @"
        SELECT projeId, status
        FROM ProjeFinans_ProjeKutuk
        WHERE projeId = @projeId";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@projeId", SqlDbType.Int).Value = projeId;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ProjeKutuk
                        {
                            projeId = reader.GetInt32(reader.GetOrdinal("projeId")),
                            status = reader.IsDBNull(reader.GetOrdinal("status"))
                                ? "Başlatıldı"
                                : reader.GetString(reader.GetOrdinal("status"))
                        };
                    }
                }
            }

            return null;
        }

        public bool UpdateProjeKutukDurum(SqlConnection connection, SqlTransaction transaction, int projeId, bool? montajTamamlandiMi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string kontrolQuery = @"
        SELECT TOP 1 ustProjeId
        FROM ProjeFinans_ProjeIliski
        WHERE altProjeId = @projeId";

            int? ustProjeId = null;
            using (var cmd = new SqlCommand(kontrolQuery, connection, transaction))
            {
                cmd.Parameters.Add("@projeId", SqlDbType.Int).Value = projeId;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        ustProjeId = reader.GetInt32(reader.GetOrdinal("ustProjeId"));
                }
            }

            int targetProjeId = ustProjeId.HasValue ? ustProjeId.Value : projeId;

            string checkQuery = @"SELECT COUNT(*) FROM ProjeFinans_ProjeKutuk WHERE projeId = @projeId";
            using (var checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.Add("@projeId", SqlDbType.Int).Value = targetProjeId;
                int count = (int)checkCmd.ExecuteScalar();
                if (count == 0)
                    return false;
            }

            bool finalMontajTamamlandiMi;
            if (montajTamamlandiMi.HasValue)
            {
                finalMontajTamamlandiMi = montajTamamlandiMi.Value;
            }
            else
            {
                string currentStatusQuery = @"
            SELECT montajTamamlandiMi 
            FROM ProjeFinans_ProjeKutuk 
            WHERE projeId = @projeId";

                using (var cmd = new SqlCommand(currentStatusQuery, connection, transaction))
                {
                    cmd.Parameters.Add("@projeId", SqlDbType.Int).Value = targetProjeId;
                    var result = cmd.ExecuteScalar();
                    finalMontajTamamlandiMi =
                        (result != null && result != DBNull.Value)
                        ? (bool)result
                        : false;
                }
            }

            bool sayfa3Kapali;
            bool sayfa4Kapali;

            sayfa3Kapali = _sayfaStatusRepository.IsSayfa3Kapali(connection, transaction, targetProjeId);
            sayfa4Kapali = _sayfaStatusRepository.IsAllAltProjelerSayfa4Kapali(connection, transaction, targetProjeId);

            string status = (sayfa3Kapali && sayfa4Kapali && finalMontajTamamlandiMi)
                ? "Kapatıldı"
                : "Başlatıldı";

            string updateQuery = @"
        UPDATE ProjeFinans_ProjeKutuk
        SET status = @status
        WHERE projeId = @projeId";

            using (var cmd = new SqlCommand(updateQuery, connection, transaction))
            {
                cmd.Parameters.Add("@projeId", SqlDbType.Int).Value = targetProjeId;
                cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = status;

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public bool ProjeNoKontrol(SqlConnection connection, string projeNo)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (string.IsNullOrWhiteSpace(projeNo))
                throw new ArgumentNullException(nameof(projeNo));


            const string sql = @"
        SELECT COUNT(*) 
        FROM ProjeFinans_ProjeKutuk 
        WHERE SUBSTRING(ProjeNo, 1, 5) = @ProjeNo";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@ProjeNo", SqlDbType.NVarChar).Value = projeNo;

                int count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
