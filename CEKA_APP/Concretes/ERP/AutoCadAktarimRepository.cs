using CEKA_APP.Abstracts.ERP;
using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.ERP
{
    public class AutoCadAktarimRepository : IAutoCadAktarimRepository
    {
        public void SaveAutoCadData(SqlConnection connection, SqlTransaction transaction, string projeAdi, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, Guid yuklemeId, decimal netAgirlik, int takimCarpani)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            int projeId;
            string projeQuery = "SELECT projeId FROM Projeler WHERE projeAdi = @projeAdi";
            using (var cmd = new SqlCommand(projeQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    string insertProje = "INSERT INTO Projeler (projeAdi, olusturmaTarihi) VALUES (@projeAdi, @tarih); SELECT SCOPE_IDENTITY();";
                    using (var insertCmd = new SqlCommand(insertProje, connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                        insertCmd.Parameters.AddWithValue("@tarih", DateTime.Now);
                        projeId = Convert.ToInt32(insertCmd.ExecuteScalar());
                    }
                }
                else
                {
                    projeId = Convert.ToInt32(result);
                }
            }

            string ustGrup = grupAdi.Length >= 3 ? grupAdi.Substring(0, 3) : grupAdi;
            string ustGrupQuery = "SELECT ustGrupId FROM UstGruplar WHERE projeId = @projeId AND ustGrup = @ustGrup AND yuklemeId = @yuklemeId";
            int ustGrupId;
            using (var cmd = new SqlCommand(ustGrupQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                cmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    string insertUstGrup = "INSERT INTO UstGruplar (projeId, ustGrup, yuklemeId, takimCarpani) VALUES (@projeId, @ustGrup, @yuklemeId, 1); SELECT SCOPE_IDENTITY();";
                    using (var insertCmd = new SqlCommand(insertUstGrup, connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@projeId", projeId);
                        insertCmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                        insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                        ustGrupId = Convert.ToInt32(insertCmd.ExecuteScalar());
                    }
                }
                else
                {
                    ustGrupId = Convert.ToInt32(result);
                }
            }

            int grupId;
            string grupQuery = "SELECT grupId FROM Gruplar WHERE projeId = @projeId AND grupAdi = @grupAdi AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
            using (var cmd = new SqlCommand(grupQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                cmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    string insertGrup = "INSERT INTO Gruplar (projeId, grupAdi, yuklemeId, takimCarpani, ustGrupId) VALUES (@projeId, @grupAdi, @yuklemeId, @takimCarpani, @ustGrupId); SELECT SCOPE_IDENTITY();";
                    using (var insertCmd = new SqlCommand(insertGrup, connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@projeId", projeId);
                        insertCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                        insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                        insertCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                        insertCmd.Parameters.AddWithValue("@ustGrupId", ustGrupId);
                        grupId = Convert.ToInt32(insertCmd.ExecuteScalar());
                    }
                }
                else
                {
                    grupId = Convert.ToInt32(result);
                    string updateTakimQuery = "UPDATE Gruplar SET takimCarpani = @takimCarpani, ustGrupId = @ustGrupId WHERE grupId = @grupId";
                    using (var updateCmd = new SqlCommand(updateTakimQuery, connection, transaction))
                    {
                        updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                        updateCmd.Parameters.AddWithValue("@ustGrupId", ustGrupId);
                        updateCmd.Parameters.AddWithValue("@grupId", grupId);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(malzemeAd))
            {
                string insertMalzeme = @"
            INSERT INTO Malzemeler (grupId, malzemeKod, adet, malzemeAd, kalite, yuklemeId, orjinalAdet, netAgirlik)
            VALUES (@grupId, @malzemeKod, @adet, @malzemeAd, @kalite, @yuklemeId, @orjinalAdet, @netAgirlik)";
                using (var insertCmd = new SqlCommand(insertMalzeme, connection, transaction))
                {
                    insertCmd.Parameters.AddWithValue("@grupId", grupId);
                    insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                    insertCmd.Parameters.AddWithValue("@adet", adet);
                    insertCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                    insertCmd.Parameters.AddWithValue("@orjinalAdet", adet);
                    insertCmd.Parameters.AddWithValue("@netAgirlik", netAgirlik);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }
        public List<AutoCadAktarimDetay> GetAutoCadKayitlari(SqlConnection connection, string projeAdi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            List<AutoCadAktarimDetay> list = new List<AutoCadAktarimDetay>();

            string query = @"
        -- Malzeme kayıtları
        SELECT
            p.projeAdi AS proje,
            ug.ustGrup,
            ug.ustGrupId,
            g.grupAdi AS grup,
            g.grupId,
            m.malzemeKod,
            m.yuklemeId
        FROM Malzemeler m
        JOIN Gruplar g ON m.grupId = g.grupId
        JOIN UstGruplar ug ON g.ustGrupId = ug.ustGrupId
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi)
        UNION
        -- Grup kayıtları (malzemesiz)
        SELECT
            p.projeAdi AS proje,
            ug.ustGrup,
            ug.ustGrupId,
            g.grupAdi AS grup,
            g.grupId,
            NULL AS malzemeKod,
            g.yuklemeId
        FROM Gruplar g
        JOIN UstGruplar ug ON g.ustGrupId = ug.ustGrupId
        JOIN Projeler p ON g.projeId = p.projeId
        LEFT JOIN Malzemeler m ON m.grupId = g.grupId
        WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi) AND m.malzemeId IS NULL
        UNION
        -- Üst Grup kayıtları
        SELECT
            p.projeAdi AS proje,
            ug.ustGrup,
            ug.ustGrupId,
            NULL AS grup,
            NULL AS grupId,
            NULL AS malzemeKod,
            ug.yuklemeId
        FROM UstGruplar ug
        JOIN Projeler p ON ug.projeId = p.projeId
        LEFT JOIN Gruplar g ON g.ustGrupId = ug.ustGrupId
        WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi) AND g.grupId IS NULL
        UNION
        -- Proje kayıtları (grupsuz ve malzemesiz)
        SELECT
            p.projeAdi AS proje,
            NULL AS ustGrup,
            NULL AS ustGrupId,
            NULL AS grup,
            NULL AS grupId,
            NULL AS malzemeKod,
            NULL AS yuklemeId
        FROM Projeler p
        LEFT JOIN UstGruplar ug ON ug.projeId = p.projeId
        WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi) AND ug.ustGrupId IS NULL
        ORDER BY proje, ustGrup, grup, malzemeKod";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new AutoCadAktarimDetay
                        {
                            Proje = reader["proje"].ToString(),
                            UstGrup = reader["ustGrup"] != DBNull.Value ? reader["ustGrup"].ToString() : null,
                            UstGrupId = reader["ustGrupId"] != DBNull.Value ? (int)reader["ustGrupId"] : 0,
                            Grup = reader["grup"] != DBNull.Value ? reader["grup"].ToString() : null,
                            GrupId = reader["grupId"] != DBNull.Value ? (int)reader["grupId"] : 0,
                            MalzemeKod = reader["malzemeKod"] != DBNull.Value ? reader["malzemeKod"].ToString() : null,
                            YuklemeId = reader["yuklemeId"] != DBNull.Value ? Guid.Parse(reader["yuklemeId"].ToString()) : (Guid?)null
                        });
                    }
                }
            }

            return list;
        }


        public DataTable GruplariGetir(SqlConnection connection, string projeAdi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            DataTable tablo = new DataTable();

            string sorgu = @"
        SELECT DISTINCT p.projeAdi, ug.ustGrup, ug.ustGrupId, g.grupAdi, g.grupId, g.yuklemeId, g.takimCarpani
        FROM Gruplar g
        JOIN UstGruplar ug ON g.ustGrupId = ug.ustGrupId
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE p.projeAdi = @projeAdi
        ORDER BY ug.ustGrup, g.grupAdi";

            using (var komut = new SqlCommand(sorgu, connection))
            {
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);

                using (var adaptor = new SqlDataAdapter(komut))
                {
                    adaptor.Fill(tablo);
                }
            }

            return tablo;
        }
        public DataTable UstGruplariGetir(SqlConnection connection, string projeAdi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            DataTable tablo = new DataTable();

            string sorgu = @"
        SELECT DISTINCT
            P.projeAdi,
            UG.ustGrup AS ustGrupAdi,
            UG.ustGrupId,
            UG.takimCarpani,
            UG.yuklemeId
        FROM UstGruplar UG
        JOIN Projeler P ON UG.projeId = P.projeId
        WHERE P.projeAdi = @projeAdi
        ORDER BY UG.ustGrup";

            using (var komut = new SqlCommand(sorgu, connection))
            {
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);

                using (var adaptor = new SqlDataAdapter(komut))
                {
                    adaptor.Fill(tablo);
                }
            }
            return tablo;
        }

        public DataTable MalzemeleriGetir(SqlConnection connection, string projeAdi, string grupAdi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            DataTable tablo = new DataTable();

            string sorgu = @"
        SELECT p.projeAdi, g.grupAdi, m.malzemeKod, m.malzemeAd, m.adet, m.kalite, m.yuklemeId, m.orjinalAdet, m.netAgirlik
        FROM Malzemeler m
        JOIN Gruplar g ON m.grupId = g.grupId
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi
        ORDER BY m.malzemeKod";

            using (var komut = new SqlCommand(sorgu, connection))
            {
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                komut.Parameters.AddWithValue("@grupAdi", grupAdi);

                using (var adaptor = new SqlDataAdapter(komut))
                {
                    adaptor.Fill(tablo);
                }
            }
            return tablo;
        }


        public DataTable MalzemeDetaylariniGetir(SqlConnection connection, string projeAdi, string grupAdi, string malzemeKod)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            DataTable tablo = new DataTable();

            string sorgu = @"
        SELECT p.projeAdi, g.grupAdi, m.malzemeKod, m.malzemeAd, m.adet, m.kalite, m.yuklemeId, m.orjinalAdet, m.netAgirlik
        FROM Malzemeler m
        JOIN Gruplar g ON m.grupId = g.grupId
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi AND m.malzemeKod = @malzemeKod";

            using (var komut = new SqlCommand(sorgu, connection))
            {
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                komut.Parameters.AddWithValue("@grupAdi", grupAdi);
                komut.Parameters.AddWithValue("@malzemeKod", malzemeKod);

                using (var adaptor = new SqlDataAdapter(komut))
                {
                    adaptor.Fill(tablo);
                }
            }

            return tablo;
        }
        public void ProjeEkle(SqlConnection connection, SqlTransaction transaction, string projeAdi, string aciklama, DateTime olusturmaTarihi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            using (var komut = new SqlCommand(@"
        INSERT INTO Projeler (projeAdi, olusturmaTarihi, aciklama)
        VALUES (@projeAdi, @olusturmaTarihi, @aciklama)", connection, transaction))
            {
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                komut.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                komut.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? DBNull.Value : (object)aciklama);

                komut.ExecuteNonQuery();
            }
        }

        public void StandartGrupEkle(SqlConnection connection, SqlTransaction transaction, string grupNo)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sorgu = @"
        INSERT INTO StandartGruplar (grupNo, olusturmaTarihi)
        VALUES (@grupNo, GETDATE())";

            using (var komut = new SqlCommand(sorgu, connection, transaction))
            {
                komut.Parameters.AddWithValue("@grupNo", grupNo);
                komut.ExecuteNonQuery();
            }
        }

        public List<string> GetirStandartGruplarListe(SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var gruplar = new List<string>();
            string query = "SELECT grupNo FROM StandartGruplar ORDER BY grupNo";

            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string grupNo = reader["grupNo"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(grupNo))
                        gruplar.Add(grupNo);
                }
            }

            return gruplar;
        }
        public bool GetirStandartGruplar(SqlConnection connection, string kalip)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = "SELECT COUNT(*) FROM StandartGruplar WHERE grupNo = @grupNo";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@grupNo", kalip);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        public void StandartGrupSil(SqlConnection connection, SqlTransaction transaction, string grupNo)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sorgu = "DELETE FROM StandartGruplar WHERE grupNo = @grupNo";

            using (var komut = new SqlCommand(sorgu, connection, transaction))
            {
                komut.Parameters.AddWithValue("@grupNo", grupNo);
                komut.ExecuteNonQuery();
            }
        }

        public void GrupEkleGuncelle(SqlConnection connection, SqlTransaction transaction, string projeNo, string grupAdi, string eskiGrupAdi, string ustGrupAdi, Guid? yuklemeId = null, int takimCarpani = 1)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (string.IsNullOrEmpty(ustGrupAdi)) throw new ArgumentNullException(nameof(ustGrupAdi));

            int projeId = GetProjeId(connection, transaction, projeNo);

            string ustGrupQuery = "SELECT ustGrupId FROM UstGruplar WHERE projeId = @projeId AND ustGrup = @ustGrup AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
            int? ustGrupId = null;
            using (var cmd = new SqlCommand(ustGrupQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@ustGrup", ustGrupAdi);
                cmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    ustGrupId = Convert.ToInt32(result);
                }
                else
                {
                    string insertUstGrup = "INSERT INTO UstGruplar (projeId, ustGrup, yuklemeId, takimCarpani) VALUES (@projeId, @ustGrup, @yuklemeId, 1); SELECT SCOPE_IDENTITY();";
                    using (var insertCmd = new SqlCommand(insertUstGrup, connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@projeId", projeId);
                        insertCmd.Parameters.AddWithValue("@ustGrup", ustGrupAdi);
                        insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                        ustGrupId = Convert.ToInt32(insertCmd.ExecuteScalar());
                    }
                }
            }

            string checkQuery = "SELECT COUNT(*) FROM Gruplar WHERE projeId = @projeId AND grupAdi = @grupAdi AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
            using (var checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@projeId", projeId);
                checkCmd.Parameters.AddWithValue("@grupAdi", eskiGrupAdi ?? grupAdi);
                checkCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0 && !string.IsNullOrEmpty(eskiGrupAdi))
                {
                    string updateQuery = "UPDATE Gruplar SET grupAdi = @yeniGrupAdi, yuklemeId = @yuklemeId, ustGrupId = @ustGrupId, takimCarpani = @takimCarpani WHERE projeId = @projeId AND grupAdi = @eskiGrupAdi AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
                    using (var updateCmd = new SqlCommand(updateQuery, connection, transaction))
                    {
                        updateCmd.Parameters.AddWithValue("@yeniGrupAdi", grupAdi);
                        updateCmd.Parameters.AddWithValue("@projeId", projeId);
                        updateCmd.Parameters.AddWithValue("@eskiGrupAdi", eskiGrupAdi);
                        updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@ustGrupId", ustGrupId.Value);
                        updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                        updateCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string insertQuery = "INSERT INTO Gruplar (projeId, grupAdi, yuklemeId, takimCarpani, ustGrupId) VALUES (@projeId, @grupAdi, @yuklemeId, @takimCarpani, @ustGrupId)";
                    using (var insertCmd = new SqlCommand(insertQuery, connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@projeId", projeId);
                        insertCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                        insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                        insertCmd.Parameters.AddWithValue("@ustGrupId", ustGrupId.Value);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public void UstGrupEkleGuncelle(SqlConnection connection, SqlTransaction transaction, string projeNo, string ustGrupAdi, Guid? yuklemeId, int takimCarpani, string eskiUstGrupAdi = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            int projeId = GetProjeId(connection, transaction, projeNo);

            int? ustGrupId = null;
            string checkQuery = "SELECT ustGrupId FROM UstGruplar WHERE projeId = @projeId AND ustGrup = @ustGrupAdi AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
            using (var checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@projeId", projeId);
                checkCmd.Parameters.AddWithValue("@ustGrupAdi", eskiUstGrupAdi ?? ustGrupAdi);
                checkCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                var result = checkCmd.ExecuteScalar();

                if (result != null)
                {
                    ustGrupId = Convert.ToInt32(result);
                }
            }

            if (ustGrupId.HasValue && !string.IsNullOrEmpty(eskiUstGrupAdi))
            {
                string updateQuery = @"UPDATE UstGruplar
                                   SET ustGrup = @yeniUstGrupAdi, takimCarpani = @takimCarpani
                                   WHERE projeId = @projeId AND ustGrup = @eskiUstGrupAdi AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
                using (var updateCmd = new SqlCommand(updateQuery, connection, transaction))
                {
                    updateCmd.Parameters.AddWithValue("@yeniUstGrupAdi", ustGrupAdi);
                    updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                    updateCmd.Parameters.AddWithValue("@projeId", projeId);
                    updateCmd.Parameters.AddWithValue("@eskiUstGrupAdi", eskiUstGrupAdi);
                    updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                    updateCmd.ExecuteNonQuery();
                }
            }
            else
            {
                string insertQuery = "INSERT INTO UstGruplar (projeId, ustGrup, yuklemeId, takimCarpani) VALUES (@projeId, @ustGrupAdi, @yuklemeId, @takimCarpani); SELECT SCOPE_IDENTITY();";
                using (var insertCmd = new SqlCommand(insertQuery, connection, transaction))
                {
                    insertCmd.Parameters.AddWithValue("@projeId", projeId);
                    insertCmd.Parameters.AddWithValue("@ustGrupAdi", ustGrupAdi);
                    insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                    ustGrupId = Convert.ToInt32(insertCmd.ExecuteScalar());
                }

                string insertGrupQuery = "INSERT INTO Gruplar (projeId, grupAdi, yuklemeId, takimCarpani, ustGrupId) VALUES (@projeId, @grupAdi, @yuklemeId, @takimCarpani, @ustGrupId)";
                using (var insertGrupCmd = new SqlCommand(insertGrupQuery, connection, transaction))
                {
                    insertGrupCmd.Parameters.AddWithValue("@projeId", projeId);
                    insertGrupCmd.Parameters.AddWithValue("@grupAdi", ustGrupAdi);
                    insertGrupCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                    insertGrupCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                    insertGrupCmd.Parameters.AddWithValue("@ustGrupId", ustGrupId.Value);
                    insertGrupCmd.ExecuteNonQuery();
                }
            }

        }
        public void GrupSil(SqlConnection connection, SqlTransaction transaction, string projeAdi, string grupAdi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string checkQuery = @"
        SELECT g.grupId
        FROM Gruplar g
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi";

            using (var checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                checkCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                var result = checkCmd.ExecuteScalar();
                if (result == null)
                {
                    throw new Exception($"Grup bulunamadı: projeAdi={projeAdi}, grupAdi={grupAdi}");
                }
            }

            string sorgu = @"
        DELETE m
        FROM Malzemeler m
        JOIN Gruplar g ON m.grupId = g.grupId
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi;

        DELETE g
        FROM Gruplar g
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi";

            using (var komut = new SqlCommand(sorgu, connection, transaction))
            {
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                komut.Parameters.AddWithValue("@grupAdi", grupAdi);
                komut.ExecuteNonQuery();
            }
        }

        public void UstGrupSil(SqlConnection connection, SqlTransaction transaction, string projeAdi, string ustGrupAdi, Guid yuklemeId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string checkQuery = @"
        SELECT UG.ustGrupId
        FROM UstGruplar UG
        JOIN Projeler P ON UG.projeId = P.projeId
        WHERE P.projeAdi = @projeAdi AND UG.ustGrup = @ustGrupAdi AND UG.yuklemeId = @yuklemeId";

            using (var checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                checkCmd.Parameters.AddWithValue("@ustGrupAdi", ustGrupAdi);
                checkCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                var result = checkCmd.ExecuteScalar();
                if (result == null)
                {
                    throw new Exception($"Üst Grup bulunamadı: projeAdi={projeAdi}, ustGrupAdi={ustGrupAdi}, yuklemeId={yuklemeId}");
                }
            }

            string deleteRelatedDataQuery = @"
        DELETE M
        FROM Malzemeler M
        JOIN Gruplar G ON M.grupId = G.grupId
        JOIN Projeler P ON G.projeId = P.projeId
        WHERE P.projeAdi = @projeAdi AND G.yuklemeId = @yuklemeId;

        DELETE G
        FROM Gruplar G
        JOIN Projeler P ON G.projeId = P.projeId
        WHERE P.projeAdi = @projeAdi AND G.yuklemeId = @yuklemeId;

        DELETE UG
        FROM UstGruplar UG
        JOIN Projeler P ON UG.projeId = P.projeId
        WHERE P.projeAdi = @projeAdi AND UG.ustGrup = @ustGrupAdi AND UG.yuklemeId = @yuklemeId;";

            using (var komut = new SqlCommand(deleteRelatedDataQuery, connection, transaction))
            {
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                komut.Parameters.AddWithValue("@ustGrupAdi", ustGrupAdi);
                komut.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                komut.ExecuteNonQuery();
            }
        }

        public void MalzemeEkleGuncelle(SqlConnection connection, SqlTransaction transaction, string projeNo, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, decimal agirlik, Guid? yuklemeId = null, string eskiMalzemeKod = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            int projeId = GetProjeId(connection, transaction, projeNo);
            int grupId = GetGrupId(connection, transaction, projeId, grupAdi);

            string checkQuery = "SELECT COUNT(*) FROM Malzemeler WHERE grupId = @grupId AND malzemeKod = @malzemeKod";
            using (var checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@grupId", grupId);
                checkCmd.Parameters.AddWithValue("@malzemeKod", eskiMalzemeKod ?? malzemeKod);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0 && !string.IsNullOrEmpty(eskiMalzemeKod))
                {
                    string updateQuery = @"UPDATE Malzemeler
                                   SET malzemeKod = @yeniMalzemeKod, adet = @adet, malzemeAd = @malzemeAd, kalite = @kalite, yuklemeId = @yuklemeId, netAgirlik = @netAgirlik
                                   WHERE grupId = @grupId AND malzemeKod = @eskiMalzemeKod";
                    using (var updateCmd = new SqlCommand(updateQuery, connection, transaction))
                    {
                        updateCmd.Parameters.AddWithValue("@yeniMalzemeKod", malzemeKod);
                        updateCmd.Parameters.AddWithValue("@adet", adet);
                        updateCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@grupId", grupId);
                        updateCmd.Parameters.AddWithValue("@eskiMalzemeKod", eskiMalzemeKod);
                        updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@netAgirlik", agirlik);
                        updateCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string insertQuery = @"INSERT INTO Malzemeler (grupId, malzemeKod, adet, malzemeAd, kalite, yuklemeId, orjinalAdet, netAgirlik)
                                   VALUES (@grupId, @malzemeKod, @adet, @malzemeAd, @kalite, @yuklemeId, @orjinalAdet, @netAgirlik)";
                    using (var insertCmd = new SqlCommand(insertQuery, connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@grupId", grupId);
                        insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                        insertCmd.Parameters.AddWithValue("@adet", adet);
                        insertCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@orjinalAdet", adet);
                        insertCmd.Parameters.AddWithValue("@netAgirlik", agirlik);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public void MalzemeSil(SqlConnection connection, SqlTransaction transaction, string projeAdi, string grupAdi, string malzemeKod)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string sorgu = @"
        DELETE m
        FROM Malzemeler m
        JOIN Gruplar g ON m.grupId = g.grupId
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi AND m.malzemeKod = @malzemeKod";

            using (var komut = new SqlCommand(sorgu, connection, transaction))
            {
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                komut.Parameters.AddWithValue("@grupAdi", grupAdi);
                komut.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                komut.ExecuteNonQuery();
            }
        }

        public void UpdateTakimCarpaniVeAltGruplar(SqlConnection connection, SqlTransaction transaction, string projeAdi, string ustGrup, Guid? yuklemeId, int takimCarpani)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            int projeId = GetProjeId(connection, transaction, projeAdi);

            string ustGrupUpdateQuery = @"
        UPDATE UstGruplar
        SET takimCarpani = @takimCarpani
        WHERE projeId = @projeId
        AND ustGrup = @ustGrup
        AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
            using (var updateCmd = new SqlCommand(ustGrupUpdateQuery, connection, transaction))
            {
                updateCmd.Parameters.AddWithValue("@projeId", projeId);
                updateCmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                updateCmd.ExecuteNonQuery();
            }

            string grupUpdateQuery = @"
        UPDATE Gruplar
        SET takimCarpani = @takimCarpani
        WHERE projeId = @projeId
        AND grupAdi LIKE @ustGrup + '-%'
        AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
            using (var updateCmd = new SqlCommand(grupUpdateQuery, connection, transaction))
            {
                updateCmd.Parameters.AddWithValue("@projeId", projeId);
                updateCmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                updateCmd.ExecuteNonQuery();
            }

            string malzemeUpdateQuery = @"
        UPDATE Malzemeler
        SET adet = m.orjinalAdet * @takimCarpani
        FROM Malzemeler m
        JOIN Gruplar g ON m.grupId = g.grupId
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE p.projeAdi = @projeAdi
        AND g.grupAdi LIKE @ustGrup + '-%'
        AND (@yuklemeId IS NULL OR m.yuklemeId = @yuklemeId)";
            using (var updateCmd = new SqlCommand(malzemeUpdateQuery, connection, transaction))
            {
                updateCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                updateCmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                updateCmd.ExecuteNonQuery();
            }
        }

        public void UpdateTakimCarpani(SqlConnection connection, SqlTransaction transaction, string projeAdi, string grupAdi, int takimCarpani)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            int projeId = GetProjeId(connection, transaction, projeAdi);
            int grupId = GetGrupId(connection, transaction, projeId, grupAdi);

            string updateQuery = @"
        UPDATE Gruplar
        SET takimCarpani = @takimCarpani
        WHERE projeId = @projeId AND grupId = @grupId;

        UPDATE Malzemeler
        SET adet = m.orjinalAdet * @takimCarpani
        FROM Malzemeler m
        JOIN Gruplar g ON m.grupId = g.grupId
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi";

            using (var updateCmd = new SqlCommand(updateQuery, connection, transaction))
            {
                updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                updateCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                updateCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                updateCmd.Parameters.AddWithValue("@projeId", projeId);
                updateCmd.Parameters.AddWithValue("@grupId", grupId);
                updateCmd.ExecuteNonQuery();
            }
        }

        private int GetProjeId(SqlConnection connection, SqlTransaction transaction, string projeNo)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = "SELECT projeId FROM Projeler WHERE projeAdi = @projeAdi";
            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeAdi", projeNo);
                var result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    throw new Exception($"Proje bulunamadı: {projeNo}");

                return Convert.ToInt32(result);
            }
        }
        private int GetGrupId(SqlConnection connection, SqlTransaction transaction, int projeId, string grupAdi)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            const string query = @"
        SELECT grupId
        FROM Gruplar
        WHERE projeId = @projeId
          AND grupAdi = @grupAdi";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@grupAdi", grupAdi);

                var result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    throw new Exception($"Grup bulunamadı: projeId={projeId}, grupAdi={grupAdi}");

                return Convert.ToInt32(result);
            }
        }
        public (bool, int) AdetGetir(SqlConnection connection, string kalite, string malzeme, string kalip, string proje, decimal girilenAdet)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = @"
SELECT SUM(m.adet * COALESCE(g.takimCarpani, 1) * COALESCE(ug.takimCarpani, 1)) AS ToplamAdet
FROM Malzemeler m
JOIN Gruplar g ON m.grupID = g.grupID
JOIN Projeler p ON g.projeID = p.projeID
LEFT JOIN UstGruplar ug ON g.ustGrupID = ug.ustGrupID
WHERE m.kalite = @Kalite
  AND m.malzemeAd = @malzemeAd
  AND m.malzemeKod = @malzemeKod
  AND p.projeAdi = @ProjeAdi";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Kalite", kalite);
                command.Parameters.AddWithValue("@malzemeAd", malzeme);
                command.Parameters.AddWithValue("@malzemeKod", kalip);
                command.Parameters.AddWithValue("@ProjeAdi", proje);

                var result = command.ExecuteScalar();
                int toplamAdet = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                return (girilenAdet <= toplamAdet, toplamAdet);
            }
        }

        public (bool isValid, int toplamAdetIfs, int toplamAdetYuklenen) KontrolAdeta(SqlConnection connection, string kalite, string malzeme, string kalip, string proje, int girilenAdet)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string[] kalipParcalari = kalip.Split('-');
            string originalKalip = kalip;

            if (kalipParcalari.Length >= 3 && GetirStandartGruplar(connection, kalip))
            {
                kalip = $"{kalipParcalari[0]}-00-{kalipParcalari[2]}";
            }

            string query = @"
SELECT
    SUM(m.adet * COALESCE(g.takimCarpani, 1) * COALESCE(ug.takimCarpani, 1)) AS ToplamAdetIfs
FROM Malzemeler m
JOIN Gruplar g ON m.grupID = g.grupID
JOIN Projeler p ON g.projeID = p.projeID
LEFT JOIN UstGruplar ug ON g.ustGrupID = ug.ustGrupID
WHERE m.kalite = @Kalite
  AND m.malzemeAd = @malzemeAd
  AND m.malzemeKod = @malzemeKod
  AND p.projeAdi = @ProjeAdi;

SELECT SUM(KD.toplamAdet) AS ToplamAdetYuklenen
FROM KesimDetaylari KD
WHERE KD.kalite = @Kalite
  AND KD.malzeme = @malzemeAd
  AND KD.malzemeKod LIKE @malzemeKodLike
  AND KD.proje = @ProjeAdi";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Kalite", kalite);
                command.Parameters.AddWithValue("@malzemeAd", malzeme);
                command.Parameters.AddWithValue("@malzemeKod", kalip);
                command.Parameters.AddWithValue("@ProjeAdi", proje);

                string malzemeKodLike = kalipParcalari.Length >= 3 && GetirStandartGruplar(connection, kalip)
                    ? $"{kalipParcalari[0]}-[0-9][0-9]-{kalipParcalari[2]}" 
                    : $"{kalipParcalari[0]}-{kalipParcalari[1]}-{kalipParcalari[2]}"; 
                command.Parameters.AddWithValue("@malzemeKodLike", malzemeKodLike);

                using (var reader = command.ExecuteReader())
                {
                    int toplamAdetIfs = 0;
                    int toplamAdetYuklenen = 0;

                    if (reader.Read())
                    {
                        toplamAdetIfs = reader["ToplamAdetIfs"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetIfs"]) : 0;
                    }

                    reader.NextResult();

                    if (reader.Read())
                    {
                        toplamAdetYuklenen = reader["ToplamAdetYuklenen"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetYuklenen"]) : 0;
                    }

                    bool isValid = girilenAdet + toplamAdetYuklenen <= toplamAdetIfs;

                    return (isValid, toplamAdetIfs, toplamAdetYuklenen);
                }
            }
        }
        public void BaglaProjeVeGrup(SqlConnection connection, SqlTransaction transaction, string projeAdi, string secilenGrup)
        {

            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            string checkProjeQuery = "SELECT COUNT(*) FROM Projeler WHERE projeAdi = @projeAdi";
            using (var checkCmd = new SqlCommand(checkProjeQuery, connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                int projeCount = (int)checkCmd.ExecuteScalar();
                if (projeCount == 0)
                {
                    throw new ApplicationException("Girilen proje numarası mevcut değil.");
                }
            }

            Guid yeniYuklemeId = Guid.NewGuid();
            UstGrupEkleGuncelle(connection, transaction, projeAdi, secilenGrup, yeniYuklemeId, 1, null);
        }
        public decimal GetNetAgirlik(SqlConnection connection, string kalite, string malzeme, string kalipPoz, string proje)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = @"
        SELECT m.netAgirlik
        FROM Malzemeler m
        JOIN Gruplar g ON m.grupId = g.grupId
        JOIN Projeler p ON g.projeId = p.projeId
        WHERE m.kalite = @Kalite
          AND m.malzemeAd = @MalzemeAd
          AND m.malzemeKod = @MalzemeKod
          AND p.projeAdi = @ProjeAdi";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Kalite", kalite);
                command.Parameters.AddWithValue("@MalzemeAd", malzeme);
                command.Parameters.AddWithValue("@MalzemeKod", kalipPoz);
                command.Parameters.AddWithValue("@ProjeAdi", proje);

                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }
    }
}
