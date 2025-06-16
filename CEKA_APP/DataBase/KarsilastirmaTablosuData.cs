using System;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.DataBase
{
    public class KarsilastirmaTablosuData
    {
        // Kalite için IFS kodunu getir
        public static string GetIfsCodeByAutoCadCodeKalite(string cekaCode)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT IfsCode FROM KarsilastirmaTablosuKalite WHERE CekaCode = @CekaCode", connection);
                    command.Parameters.AddWithValue("@CekaCode", cekaCode);

                    var result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }

        // Kalite kaydı ekle
        public static void SaveKarsilastirmaKalite(string cekaCode, string ifsCode, string aciklama = null)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();

                    // CekaCode'un zaten var olup olmadığını kontrol et
                    var checkCommand = new SqlCommand("SELECT COUNT(*) FROM KarsilastirmaTablosuKalite WHERE CekaCode = @CekaCode", connection);
                    checkCommand.Parameters.AddWithValue("@CekaCode", cekaCode);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        throw new Exception($"Bu Çeka Kodu zaten mevcut: {cekaCode}");
                    }

                    // Yeni kaydı ekle
                    var command = new SqlCommand(
                        "INSERT INTO KarsilastirmaTablosuKalite (CekaCode, IfsCode, Aciklama) VALUES (@CekaCode, @IfsCode, @Aciklama)",
                        connection);
                    command.Parameters.AddWithValue("@CekaCode", cekaCode);
                    command.Parameters.AddWithValue("@IfsCode", ifsCode);
                    command.Parameters.AddWithValue("@Aciklama", (object)aciklama ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                throw;
            }
        }

        // Malzeme için IFS kodunu getir
        public static string GetIfsCodeByAutoCadCodeMalzeme(string autoCadCode)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT IfsCode FROM KarsilastirmaTablosuMalzeme WHERE AutoCadCode = @AutoCadCode", connection);
                    command.Parameters.AddWithValue("@AutoCadCode", autoCadCode);

                    var result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }

        // Malzeme kaydı ekle
        public static void SaveKarsilastirmaMalzeme(string autoCadCode, string ifsCode, string aciklama = null)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();

                    // AutoCadCode'un zaten var olup olmadığını kontrol et
                    var checkCommand = new SqlCommand("SELECT COUNT(*) FROM KarsilastirmaTablosuMalzeme WHERE AutoCadCode = @AutoCadCode", connection);
                    checkCommand.Parameters.AddWithValue("@AutoCadCode", autoCadCode);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        throw new Exception($"Bu AutoCAD Kodu zaten mevcut: {autoCadCode}");
                    }

                    // Yeni kaydı ekle
                    var command = new SqlCommand(
                        "INSERT INTO KarsilastirmaTablosuMalzeme (AutoCadCode, IfsCode, Aciklama) VALUES (@AutoCadCode, @IfsCode, @Aciklama)",
                        connection);
                    command.Parameters.AddWithValue("@AutoCadCode", autoCadCode);
                    command.Parameters.AddWithValue("@IfsCode", ifsCode);
                    command.Parameters.AddWithValue("@Aciklama", (object)aciklama ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                throw;
            }
        }

        // Kesim için IFS kodunu getir
        public static string GetIfsCodeByAutoCadCodeKesim(string kesimCode)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT IfsCode FROM KarsilastirmaTablosuKesim WHERE KesimCode = @KesimCode", connection);
                    command.Parameters.AddWithValue("@KesimCode", kesimCode);

                    var result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }

        // Kesim kaydı ekle
        public static void SaveKarsilastirmaKesim(string kesimCode, string ifsCode, string aciklama = null)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();

                    // KesimCode'un zaten var olup olmadığını kontrol et
                    var checkCommand = new SqlCommand("SELECT COUNT(*) FROM KarsilastirmaTablosuKesim WHERE KesimCode = @KesimCode", connection);
                    checkCommand.Parameters.AddWithValue("@KesimCode", kesimCode);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        throw new Exception($"Bu Kesim Kodu zaten mevcut: {kesimCode}");
                    }

                    // Yeni kaydı ekle
                    var command = new SqlCommand(
                        "INSERT INTO KarsilastirmaTablosuKesim (KesimCode, IfsCode, Aciklama) VALUES (@KesimCode, @IfsCode, @Aciklama)",
                        connection);
                    command.Parameters.AddWithValue("@KesimCode", kesimCode);
                    command.Parameters.AddWithValue("@IfsCode", ifsCode);
                    command.Parameters.AddWithValue("@Aciklama", (object)aciklama ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                throw;
            }
        }

        // Tüm kalite karşılaştırmalarını getir (sıralı)
        public static DataTable GetAllKaliteKarsilastirmalari()
        {
            DataTable tablo = new DataTable();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = "SELECT id, CekaCode, IfsCode, Aciklama FROM KarsilastirmaTablosuKalite ORDER BY CekaCode";
                using (var command = new SqlCommand(sorgu, conn))
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tablo);
                    }
                }
            }
            return tablo;
        }

        // Tüm malzeme karşılaştırmalarını getir (sıralı)
        public static DataTable GetAllMalzemeKarsilastirmalari()
        {
            DataTable tablo = new DataTable();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = "SELECT id, AutoCadCode, IfsCode, Aciklama FROM KarsilastirmaTablosuMalzeme ORDER BY AutoCadCode";
                using (var command = new SqlCommand(sorgu, conn))
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tablo);
                    }
                }
            }
            return tablo;
        }

        // Tüm kesim karşılaştırmalarını getir (sıralı)
        public static DataTable GetAllKesimKarsilastirmalari()
        {
            DataTable tablo = new DataTable();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = "SELECT id, KesimCode, IfsCode, Aciklama FROM KarsilastirmaTablosuKesim ORDER BY KesimCode";
                using (var command = new SqlCommand(sorgu, conn))
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(tablo);
                    }
                }
            }
            return tablo;
        }
    }
}