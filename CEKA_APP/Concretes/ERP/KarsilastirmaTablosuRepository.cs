using CEKA_APP.Abstracts.ERP;
using CEKA_APP.DataBase;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CEKA_APP.Concretes.ERP
{
    public class KarsilastirmaTablosuRepository : IKarsilastirmaTablosuRepository
    {
        public string GetIfsCodeByAutoCadCodeKalite(SqlConnection connection, string cekaCode)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            var command = new SqlCommand("SELECT IfsCode FROM KarsilastirmaTablosuKalite WHERE CekaCode = @CekaCode", connection);
            command.Parameters.AddWithValue("@CekaCode", cekaCode);

            var result = command.ExecuteScalar();
            return result?.ToString();
        }

        public string GetAutoCadCodeByIfsCodeKalite(SqlConnection connection, string ifsCode)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            var command = new SqlCommand("SELECT CekaCode FROM KarsilastirmaTablosuKalite WHERE IfsCode = @IfsCode", connection);
            command.Parameters.AddWithValue("@IfsCode", ifsCode);

            var result = command.ExecuteScalar();
            return result?.ToString();
        }
        public string GetIfsCodeByKesimCode(SqlConnection connection, string KesimCode)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            var command = new SqlCommand("SELECT IfsCode FROM KarsilastirmaTablosuKesim WHERE KesimCode = @KesimCode", connection);
            command.Parameters.AddWithValue("@KesimCode", KesimCode);

            var result = command.ExecuteScalar();
            return result?.ToString();
        }

        public void SaveKarsilastirmaKalite(SqlConnection connection, SqlTransaction transaction, string cekaCode, string ifsCode, string aciklama = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var checkCommand = new SqlCommand(
                "SELECT COUNT(*) FROM KarsilastirmaTablosuKalite WHERE CekaCode = @CekaCode",
                connection, transaction);
            checkCommand.Parameters.AddWithValue("@CekaCode", cekaCode);
            int count = (int)checkCommand.ExecuteScalar();

            if (count > 0)
            {
                throw new Exception($"Bu Çeka Kodu zaten mevcut: {cekaCode}");
            }

            var command = new SqlCommand(
                "INSERT INTO KarsilastirmaTablosuKalite (CekaCode, IfsCode, Aciklama) VALUES (@CekaCode, @IfsCode, @Aciklama)",
                connection, transaction);
            command.Parameters.AddWithValue("@CekaCode", cekaCode);
            command.Parameters.AddWithValue("@IfsCode", ifsCode);
            command.Parameters.AddWithValue("@Aciklama", (object)aciklama ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public string GetIfsCodeByAutoCadCodeMalzeme(SqlConnection connection, string autoCadCode)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            var command = new SqlCommand("SELECT IfsCode FROM KarsilastirmaTablosuMalzeme WHERE AutoCadCode = @AutoCadCode", connection);
            command.Parameters.AddWithValue("@AutoCadCode", autoCadCode);

            var result = command.ExecuteScalar();
            return result?.ToString();
        }
        public void SaveKarsilastirmaMalzeme(SqlConnection connection, SqlTransaction transaction, string autoCadCode, string ifsCode, string aciklama = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var checkCommand = new SqlCommand(
                "SELECT COUNT(*) FROM KarsilastirmaTablosuMalzeme WHERE AutoCadCode = @AutoCadCode",
                connection, transaction);
            checkCommand.Parameters.AddWithValue("@AutoCadCode", autoCadCode);
            int count = (int)checkCommand.ExecuteScalar();

            if (count > 0)
            {
                throw new Exception($"Bu AutoCAD Kodu zaten mevcut: {autoCadCode}");
            }

            var command = new SqlCommand(
                "INSERT INTO KarsilastirmaTablosuMalzeme (AutoCadCode, IfsCode, Aciklama) VALUES (@AutoCadCode, @IfsCode, @Aciklama)",
                connection, transaction);
            command.Parameters.AddWithValue("@AutoCadCode", autoCadCode);
            command.Parameters.AddWithValue("@IfsCode", ifsCode);
            command.Parameters.AddWithValue("@Aciklama", (object)aciklama ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public string GetIfsCodeByAutoCadCodeKesim(SqlConnection connection, string kesimCode, out string hataMesaji)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            hataMesaji = null;

            using (var command = new SqlCommand("SELECT IfsCode FROM KarsilastirmaTablosuKesim WHERE KesimCode = @KesimCode", connection))
            {
                command.Parameters.AddWithValue("@KesimCode", kesimCode);
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    return result.ToString();
                }
            }

            using (var command = new SqlCommand("SELECT IfsCode FROM KarsilastirmaTablosuMalzeme WHERE AutoCadCode = @AutoCadCode", connection))
            {
                command.Parameters.AddWithValue("@AutoCadCode", kesimCode);
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    return result.ToString();
                }
            }

            hataMesaji = $"Malzeme '{kesimCode}' için ne KarsilastirmaTablosuKesim ne de KarsilastirmaTablosuMalzeme tablosunda eşleşme bulunamadı.";
            return null;
        }

        public void SaveKarsilastirmaKesim(SqlConnection connection, SqlTransaction transaction, string kesimCode, string ifsCode, string aciklama = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            using (var checkCommand = new SqlCommand(
                "SELECT COUNT(*) FROM KarsilastirmaTablosuKesim WHERE KesimCode = @KesimCode",
                connection, transaction))
            {
                checkCommand.Parameters.AddWithValue("@KesimCode", kesimCode);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    throw new Exception($"Bu Kesim Kodu zaten mevcut: {kesimCode}");
                }
            }

            using (var command = new SqlCommand(
                "INSERT INTO KarsilastirmaTablosuKesim (KesimCode, IfsCode, Aciklama) VALUES (@KesimCode, @IfsCode, @Aciklama)",
                connection, transaction))
            {
                command.Parameters.AddWithValue("@KesimCode", kesimCode);
                command.Parameters.AddWithValue("@IfsCode", ifsCode);
                command.Parameters.AddWithValue("@Aciklama", (object)aciklama ?? DBNull.Value);

                command.ExecuteNonQuery();
            }
        }

        public DataTable GetAllKaliteKarsilastirmalari(SqlConnection connection)
        {
            DataTable tablo = new DataTable();

            using (var command = new SqlCommand("SELECT id, CekaCode, IfsCode, Aciklama FROM KarsilastirmaTablosuKalite ORDER BY CekaCode", connection))
            {
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(tablo);
                }
            }

            return tablo;
        }

        public DataTable GetAllMalzemeKarsilastirmalari(SqlConnection connection)
        {
            DataTable tablo = new DataTable();

            string sorgu = "SELECT id, AutoCadCode, IfsCode, Aciklama FROM KarsilastirmaTablosuMalzeme ORDER BY AutoCadCode";

            using (var command = new SqlCommand(sorgu, connection))
            {
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(tablo);
                }
            }

            return tablo;
        }
        public DataTable GetAllKesimKarsilastirmalari(SqlConnection connection)
        {
            DataTable tablo = new DataTable();

            string sorgu = "SELECT id, KesimCode, IfsCode, Aciklama FROM KarsilastirmaTablosuKesim ORDER BY KesimCode";

            using (var command = new SqlCommand(sorgu, connection))
            {
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(tablo);
                }
            }

            return tablo;
        }
        public void SilKarsilastirmaKaydi(SqlConnection connection, SqlTransaction transaction, string tableName, int id)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (!new[] { "KarsilastirmaTablosuKalite", "KarsilastirmaTablosuMalzeme", "KarsilastirmaTablosuKesim" }.Contains(tableName))
                throw new ArgumentException("Geçersiz tablo adı.", nameof(tableName));

            using (var command = new SqlCommand($"DELETE FROM {tableName} WHERE id = @Id", connection, transaction))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
