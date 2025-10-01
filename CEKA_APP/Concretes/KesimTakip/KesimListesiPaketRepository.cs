using CEKA_APP.Abstracts.KesimTakip;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.KesimTakip
{
    public class KesimListesiPaketRepository :IKesimListesiPaketRepository
    {
        public bool SaveKesimDataPaket(SqlConnection connection, SqlTransaction transaction, string olusturan, string kesimId, int kesilecekPlanSayisi, int toplamPlanTekrari, DateTime eklemeTarihi)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string query = @"
        IF NOT EXISTS (SELECT 1 FROM KesimListesiPaket WHERE kesimId = @kesimId)
        INSERT INTO KesimListesiPaket ([olusturan], [kesimId], [kesilecekPlanSayisi], [toplamPlanTekrari], [eklemeTarihi])
        VALUES (@olusturan, @kesimId, @kesilecekPlanSayisi, @toplamPlanTekrari, @eklemeTarihi)";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@olusturan", olusturan ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@kesimId", kesimId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@kesilecekPlanSayisi", kesilecekPlanSayisi);
                cmd.Parameters.AddWithValue("@toplamPlanTekrari", toplamPlanTekrari);
                cmd.Parameters.AddWithValue("@eklemeTarihi", eklemeTarihi);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool KesimListesiPaketSil(SqlConnection connection, SqlTransaction transaction, string kesimId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string deleteKesimListesiQuery = "DELETE FROM KesimListesi WHERE kesimId = @kesimId";
            using (var cmdKesimListesi = new SqlCommand(deleteKesimListesiQuery, connection, transaction))
            {
                cmdKesimListesi.Parameters.AddWithValue("@kesimId", kesimId ?? (object)DBNull.Value);
                cmdKesimListesi.ExecuteNonQuery();
            }

            string deletePaketQuery = "DELETE FROM KesimListesiPaket WHERE kesimId = @kesimId";
            using (var cmdPaket = new SqlCommand(deletePaketQuery, connection, transaction))
            {
                cmdPaket.Parameters.AddWithValue("@kesimId", kesimId ?? (object)DBNull.Value);
                int rowsAffected = cmdPaket.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public string GetKesimListesiPaketQuery()
        {
            return @"SELECT olusturan, kesimId, kesilecekPlanSayisi, kesilmisPlanSayisi, toplamPlanTekrari, eklemeTarihi FROM KesimListesiPaket";
        }

        public DataTable GetKesimListesiPaket(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            using (var cmd = new SqlCommand(GetKesimListesiPaketQuery(), connection))
            {
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public bool KesimListesiPaketKontrolluDusme(SqlConnection connection, SqlTransaction transaction, string kesimId, int kesilenMiktar, out string hataMesaji)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            hataMesaji = "";

            string selectQuery = "SELECT [kesilecekPlanSayisi] FROM [KesimListesiPaket] WHERE [kesimId] = @id";
            using (var selectCommand = new SqlCommand(selectQuery, connection, transaction))
            {
                selectCommand.Parameters.AddWithValue("@id", kesimId ?? (object)DBNull.Value);
                var mevcutDegerObj = selectCommand.ExecuteScalar();

                if (mevcutDegerObj != null && int.TryParse(mevcutDegerObj.ToString(), out int mevcutMiktar))
                {
                    if (kesilenMiktar > mevcutMiktar)
                    {
                        hataMesaji = $"Girilen miktar ({kesilenMiktar}) mevcut değerden ({mevcutMiktar}) fazla olamaz!";
                        return false;
                    }
                    string updateQuery = @"
                UPDATE [KesimListesiPaket]
                SET 
                    [kesilecekPlanSayisi] = [kesilecekPlanSayisi] - @azalt,
                    [kesilmisPlanSayisi] = [kesilmisPlanSayisi] + @arttir
                WHERE [kesimId] = @id";

                    using (var updateCommand = new SqlCommand(updateQuery, connection, transaction))
                    {
                        updateCommand.Parameters.AddWithValue("@azalt", kesilenMiktar);
                        updateCommand.Parameters.AddWithValue("@arttir", kesilenMiktar);
                        updateCommand.Parameters.AddWithValue("@id", kesimId ?? (object)DBNull.Value);

                        updateCommand.ExecuteNonQuery();
                    }

                    return true;
                }
                else
                {
                    hataMesaji = "Mevcut değer alınamadı.";
                    return false;
                }
            }
        }
        public void VerileriYenile(SqlConnection connection, DataGridView data)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            string query = @"
        SELECT 
            [olusturan], 
            [kesimId], 
            [kesilecekPlanSayisi], 
            [kesilmisPlanSayisi], 
            [toplamPlanTekrari], 
            [eklemeTarihi] 
        FROM [KesimListesiPaket]";

            using (var cmd = new SqlCommand(query, connection))
            {
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    data.DataSource = dataTable;
                }
            }
        }
        public bool KesimIdVarMi(SqlConnection connection, string kesimId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrWhiteSpace(kesimId))
                throw new ArgumentNullException(nameof(kesimId));

            string query = @"
        SELECT COUNT(*) 
        FROM KesimListesiPaket 
        WHERE SUBSTRING(kesimId, 1, CHARINDEX('-', kesimId) - 1) = @KesimId";

            using (var command = new SqlCommand(query, connection))
            {
                string kesimIdSade = kesimId.Split('-')[0];
                command.Parameters.AddWithValue("@KesimId", kesimIdSade ?? (object)DBNull.Value);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
