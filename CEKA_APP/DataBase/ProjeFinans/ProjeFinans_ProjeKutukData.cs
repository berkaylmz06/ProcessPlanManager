using CEKA_APP.DataBase.ProjeFinans;
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

namespace CEKA_APP.DataBase
{
    public class ProjeFinans_ProjeKutukData
    {
        public static bool ProjeKutukEkle(ProjeKutuk kutuk)
        {
            string projeNo = kutuk.projeNo.Trim();
            string kok = projeNo.Split('.')[0].Trim();

            if (!Regex.IsMatch(kok, @"^\d{5}$"))
            {
                MessageBox.Show("Geçersiz kök numarası.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string checkSql = @"
                SELECT COUNT(*) 
                FROM ProjeFinans_ProjeKutuk 
                WHERE LEFT(TRIM(projeNo), 5) = @kok";
                    using (SqlCommand checkCommand = new SqlCommand(checkSql, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@kok", kok);
                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Bu kök numaraya sahip bir proje zaten kayıtlı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    string sql = @"
                INSERT INTO ProjeFinans_ProjeKutuk 
                (musteriNo, musteriAdi, isFirsatiNo, projeNo, altProjeVarMi, digerProjeIliskisiVarMi, siparisSozlesmeTarihi, paraBirimi, toplamBedel, faturalamaSekli, nakliyeVarMi)
                VALUES 
                (@musteriNo, @musteriAdi, @isFirsatiNo, @projeNo, @altProjeVarMi, @digerProjeIliskisiVarMi, @siparisSozlesmeTarihi, @paraBirimi, @toplamBedel, @faturalamaSekli, @nakliyeVarMi)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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
                catch (Exception ex)
                {
                    MessageBox.Show("Kayıt eklenemedi: " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public static bool ProjeFiyatlandirmaEkle(string projeNo, decimal fiyat)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string checkSorgu = "SELECT COUNT(*) FROM ProjeFinans WHERE projeNo = @projeNo";
                    using (SqlCommand checkKomut = new SqlCommand(checkSorgu, conn))
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

                        using (SqlCommand komut = new SqlCommand(sorgu, conn))
                        {
                            komut.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                            komut.Parameters.AddWithValue("@fiyat", fiyat);
                            int affectedRows = komut.ExecuteNonQuery();
                            return affectedRows > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fiyatlandırma kaydı eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        public static bool AltProjeEkle(string ustProjeNo, string altProjeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sql = @"
                        INSERT INTO ProjeFinans_ProjeIliski 
                        (ustProjeNo, altProjeNo)
                        VALUES 
                        (@ustProjeNo, @altProjeNo)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ustProjeNo", ustProjeNo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@altProjeNo", altProjeNo ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kayıt eklenemedi: " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        public static bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string sorgu = @"
                        INSERT INTO ProjeFinans_Projeler (projeNo, aciklama, projeAdi, olusturmaTarihi)
                        VALUES (@projeNo, @aciklama, @projeAdi, @olusturmaTarihi)";

                    using (SqlCommand komut = new SqlCommand(sorgu, conn))
                    {
                        komut.Parameters.AddWithValue("@projeNo", projeNo);
                        komut.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                        komut.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                        komut.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                        komut.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje finans kaydı eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        public static bool UpdateProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    ProjeBilgi mevcutBilgi = GetProjeBilgileri(projeNo);
                    if (mevcutBilgi == null)
                    {
                        MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    bool degisiklikVar =
                        (mevcutBilgi.Aciklama ?? "") != (aciklama ?? "") ||
                        (mevcutBilgi.ProjeAdi ?? "") != (projeAdi ?? "") ||
                        mevcutBilgi.OlusturmaTarihi != olusturmaTarihi;

                    if (!degisiklikVar)
                    {
                        MessageBox.Show("Herhangi bir değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    string sorgu = @"
                        UPDATE ProjeFinans_Projeler
                        SET aciklama = @aciklama, 
                            projeAdi = @projeAdi, 
                            olusturmaTarihi = @olusturmaTarihi
                        WHERE projeNo = @projeNo";

                    using (SqlCommand command = new SqlCommand(sorgu, connection))
                    {
                        command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                        command.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? (object)DBNull.Value : aciklama);
                        command.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                        command.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                        command.ExecuteNonQuery();

                        MessageBox.Show($"Proje '{projeNo}' kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje finans güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        public static ProjeBilgi GetProjeBilgileri(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
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
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje bilgileri alınırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public static ProjeKutuk ProjeKutukAra(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string sql = @"
                        SELECT 
                            projeKutukId, musteriNo, musteriAdi, isFirsatiNo, projeNo,
                            altProjeVarMi, digerProjeIliskisiVarMi, siparisSozlesmeTarihi, paraBirimi, toplamBedel, faturalamaSekli, nakliyeVarMi
                        FROM ProjeFinans_ProjeKutuk
                        WHERE TRIM(projeNo) = @projeNo";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var kutuk = new ProjeKutuk
                                {
                                    projeKutukId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    musteriNo = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    musteriAdi = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    isFirsatiNo = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    projeNo = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    altProjeVarMi = reader.IsDBNull(5) ? false : reader.GetBoolean(5),
                                    digerProjeIliskisiVarMi = reader.IsDBNull(6) ? "0" : reader.GetString(6),
                                    siparisSozlesmeTarihi = reader.IsDBNull(7) ? DateTime.Now : reader.GetDateTime(7),
                                    paraBirimi = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    toplamBedel = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                                    faturalamaSekli = reader.IsDBNull(10) ? null : reader.GetString(10),
                                    nakliyeVarMi = reader.IsDBNull(11) ? false : reader.GetBoolean(11),
                                    altProjeBilgileri = new List<string>()
                                };

                                reader.Close();

                                if (kutuk.altProjeVarMi)
                                {
                                    string altProjeSql = @"
                                        SELECT altProjeNo
                                        FROM ProjeFinans_ProjeIliski
                                        WHERE ustProjeNo = @projeNo";
                                    using (SqlCommand altCommand = new SqlCommand(altProjeSql, connection))
                                    {
                                        altCommand.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                                        using (var altReader = altCommand.ExecuteReader())
                                        {
                                            while (altReader.Read())
                                            {
                                                kutuk.altProjeBilgileri.Add(altReader.IsDBNull(0) ? "" : altReader.GetString(0));
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
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje aranırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }
        public static void UpdateToplamBedel(string projeNo, decimal toplamBedel)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                UPDATE ProjeFinans_ProjeKutuk
                SET toplamBedel = @toplamBedel
                WHERE projeNo = @projeNo";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@projeNo", projeNo);
                        cmd.Parameters.AddWithValue("@toplamBedel", toplamBedel);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Toplam bedel güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static bool IsFaturalamaSekliTekil(string projeNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string sql = @"
                        SELECT faturalamaSekli
                        FROM ProjeFinans_ProjeKutuk
                        WHERE TRIM(projeNo) = @projeNo";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@projeNo", projeNo.Trim());
                        var result = command.ExecuteScalar();
                        return result != null && result.ToString().Trim().ToLower() == "tekil";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Faturalama şekli kontrolü sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public static bool HasRelatedRecords(string projeNo, List<string> altProjeler)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    var projeler = new List<string> { projeNo };
                    if (altProjeler != null && altProjeler.Any())
                    {
                        projeler.AddRange(altProjeler);
                    }

                    var ilgiliTablolar = new List<string>();

                    string fiyatlandirmaSql = @"
                        SELECT COUNT(*) 
                        FROM ProjeFinans_Fiyatlandirma 
                        WHERE projeNo IN ({0})";

                    string odemeSartlariSql = @"
                        SELECT COUNT(*) 
                        FROM ProjeFinans_OdemeSartlari 
                        WHERE projeNo IN ({0})";

                    string sevkiyatSql = @"
                        SELECT COUNT(*) 
                        FROM ProjeFinans_Sevkiyat 
                        WHERE projeNo IN ({0})";

                    var parameters = projeler.Select((p, i) => "@p" + i).ToList();
                    string inClause = string.Join(",", parameters);
                    fiyatlandirmaSql = string.Format(fiyatlandirmaSql, inClause);
                    odemeSartlariSql = string.Format(odemeSartlariSql, inClause);
                    sevkiyatSql = string.Format(sevkiyatSql, inClause);

                    using (SqlCommand command = new SqlCommand(fiyatlandirmaSql, connection))
                    {
                        for (int i = 0; i < projeler.Count; i++)
                        {
                            command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
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
                            command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
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
                            command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
                        }
                        int count = (int)command.ExecuteScalar();
                        if (count > 0)
                        {
                            ilgiliTablolar.Add("Sevkiyat");
                        }
                    }

                    if (ilgiliTablolar.Any())
                    {
                        string hataMesaji = $"Proje veya alt projeleri için aşağıdaki sayfalarda kayıt bulunmaktadır:\n- {string.Join("\n- ", ilgiliTablolar)}\nLütfen önce bu kayıtları siliniz.";
                        MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kayıt kontrolü sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
        }

        public static bool ProjeKutukGuncelle(ProjeKutuk yeniKutuk)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    ProjeKutuk mevcutKutuk = ProjeKutukAra(yeniKutuk.projeNo);
                    if (mevcutKutuk == null)
                    {
                        return false;
                    }

                    bool degisiklikVar =
                        mevcutKutuk.musteriNo != yeniKutuk.musteriNo ||
                        mevcutKutuk.musteriAdi != yeniKutuk.musteriAdi ||
                        mevcutKutuk.isFirsatiNo != yeniKutuk.isFirsatiNo ||
                        mevcutKutuk.altProjeVarMi != yeniKutuk.altProjeVarMi ||
                        mevcutKutuk.digerProjeIliskisiVarMi != yeniKutuk.digerProjeIliskisiVarMi ||
                        mevcutKutuk.siparisSozlesmeTarihi != yeniKutuk.siparisSozlesmeTarihi ||
                        mevcutKutuk.paraBirimi != yeniKutuk.paraBirimi ||
                        mevcutKutuk.toplamBedel != yeniKutuk.toplamBedel ||
                        mevcutKutuk.faturalamaSekli != yeniKutuk.faturalamaSekli ||
                        mevcutKutuk.nakliyeVarMi != yeniKutuk.nakliyeVarMi ||
                        !mevcutKutuk.altProjeBilgileri.OrderBy(x => x).SequenceEqual(yeniKutuk.altProjeBilgileri.OrderBy(x => x));

                    if (!degisiklikVar)
                    {
                        return false;
                    }

                    string yeniProjeNo = yeniKutuk.projeNo.Trim();
                    string kok = yeniProjeNo.Split('.')[0].Trim();

                    string checkSql = @"
                SELECT COUNT(*) 
                FROM ProjeFinans_ProjeKutuk 
                WHERE LEFT(TRIM(projeNo), 5) = @kok AND TRIM(projeNo) <> @projeNo";
                    using (SqlCommand checkCommand = new SqlCommand(checkSql, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@kok", kok);
                        checkCommand.Parameters.AddWithValue("@projeNo", yeniProjeNo);
                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Bu kök numaraya sahip başka bir proje zaten kayıtlı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string sql = @"
                        UPDATE ProjeFinans_ProjeKutuk 
                        SET musteriNo = @musteriNo, 
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

                            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                            {
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
                            WHERE ustProjeNo = @projeNo";
                                using (SqlCommand command = new SqlCommand(altProjeSilSql, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@projeNo", yeniKutuk.projeNo.Trim());
                                    command.ExecuteNonQuery();
                                }

                                foreach (var altProje in yeniKutuk.altProjeBilgileri)
                                {
                                    string altProjeEkleSql = @"
                                INSERT INTO ProjeFinans_ProjeIliski 
                                (ustProjeNo, altProjeNo)
                                VALUES 
                                (@ustProjeNo, @altProjeNo)";
                                    using (SqlCommand command = new SqlCommand(altProjeEkleSql, connection, transaction))
                                    {
                                        command.Parameters.AddWithValue("@ustProjeNo", yeniKutuk.projeNo ?? (object)DBNull.Value);
                                        command.Parameters.AddWithValue("@altProjeNo", altProje ?? (object)DBNull.Value);
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            System.Diagnostics.Debug.WriteLine($"Proje güncellenirken hata: {ex.Message}");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Bağlantı hatası: {ex.Message}");
                    return false;
                }
            }
        }
        public static bool ProjeKutukSil(string projeNo, List<string> altProjeler)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var projeler = new List<string> { projeNo };
                            if (altProjeler != null && altProjeler.Any())
                            {
                                projeler.AddRange(altProjeler);
                            }

                            var parameters = projeler.Select((p, i) => "@p" + i).ToList();
                            string inClause = string.Join(",", parameters);

                            string projeKutukSilSql = @"
                                DELETE FROM ProjeFinans_ProjeKutuk 
                                WHERE projeNo IN ({0})";
                            projeKutukSilSql = string.Format(projeKutukSilSql, inClause);

                            using (SqlCommand command = new SqlCommand(projeKutukSilSql, connection, transaction))
                            {
                                for (int i = 0; i < projeler.Count; i++)
                                {
                                    command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
                                }
                                command.ExecuteNonQuery();
                            }

                            string altProjeIliskiSilSql = @"
                                DELETE FROM ProjeFinans_ProjeIliski 
                                WHERE ustProjeNo IN ({0}) OR altProjeNo IN ({0})";
                            altProjeIliskiSilSql = string.Format(altProjeIliskiSilSql, inClause);

                            using (SqlCommand command = new SqlCommand(altProjeIliskiSilSql, connection, transaction))
                            {
                                for (int i = 0; i < projeler.Count; i++)
                                {
                                    command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
                                }
                                command.ExecuteNonQuery();
                            }

                            string projeFinansSilSql = @"
                                DELETE FROM ProjeFinans_Projeler 
                                WHERE projeNo IN ({0})";
                            projeFinansSilSql = string.Format(projeFinansSilSql, inClause);

                            using (SqlCommand command = new SqlCommand(projeFinansSilSql, connection, transaction))
                            {
                                for (int i = 0; i < projeler.Count; i++)
                                {
                                    command.Parameters.AddWithValue("@p" + i, projeler[i].Trim());
                                }
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Proje silinirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bağlantı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public static string GetProjeParaBirimi(string projeNo)
        {
            string ustProjeNo = ProjeFinans_ProjeIliskiData.GetUstProjeNo(projeNo);
            string hedefProjeNo = !string.IsNullOrEmpty(ustProjeNo) ? ustProjeNo : projeNo;

            ProjeKutuk kutuk = ProjeKutukAra(hedefProjeNo);
            return kutuk.paraBirimi;
        }
    }
}