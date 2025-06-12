using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.DataBase
{
    public class KarsilastirmaTablosuData
    {
        public static string GetIfsCodeByAutoCadCodeKalite(string CekaCode)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT IfsCode FROM KarsilastirmaTablosuKalite WHERE CekaCode = @CekaCode", connection);
                    command.Parameters.AddWithValue("@CekaCode", CekaCode);

                    var result = command.ExecuteScalar();
                    return result?.ToString(); // Eğer eşleşme yoksa null döner
                }
            }
            catch (Exception ex)
            {
                // Hata loglama (isteğe bağlı)
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }
        public static void SaveKarsilastirmaKalite(string CekaCode, string ifsCode, string aciklama = null)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "INSERT INTO KarsilastirmaTablosuKalite (CekaCode, IfsCode, Aciklama) VALUES (@CekaCode, @IfsCode, @Aciklama)",
                        connection);
                    command.Parameters.AddWithValue("@CekaCode", CekaCode);
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
        public static void UpdateKarsilastirmaKalite(int id, string CekaCode, string ifsCode, string aciklama = null)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "UPDATE KarsilastirmaTablosuKalite SET CekaCode = @CekaCode, IfsCode = @IfsCode, Aciklama = @Aciklama WHERE Id = @Id",
                        connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@CekaCode", CekaCode);
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
        public static void SaveKarsilastirmaMalzeme(string autoCadCode, string ifsCode, string aciklama = null)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
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
        public static void UpdateKarsilastirmaMalzeme(int id, string autoCadCode, string ifsCode, string aciklama = null)
        {
            try
            {
                using (SqlConnection connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "UPDATE KarsilastirmaTablosuMalzeme SET AutoCadCode = @AutoCadCode, IfsCode = @IfsCode, Aciklama = @Aciklama WHERE Id = @Id",
                        connection);
                    command.Parameters.AddWithValue("@Id", id);
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
        public static DataTable GetAllKaliteKarsilastirmalari()
        {
            DataTable tablo = new DataTable();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = "SELECT id, CekaCode, IfsCode, Aciklama FROM KarsilastirmaTablosuKalite";
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

        public static DataTable GetAllMalzemeKarsilastirmalari()
        {
            DataTable tablo = new DataTable();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = "SELECT id, AutoCadCode, IfsCode, Aciklama FROM KarsilastirmaTablosuMalzeme";
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
