using CEKA_APP.Abstracts.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class ProjeIliskiRepository: IProjeIliskiRepository
    {
        public bool AltProjeEkle(SqlConnection connection, SqlTransaction transaction, int ustProjeId, int altProjeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            const string checkSql = @"
                SELECT COUNT(*)
                FROM ProjeFinans_ProjeIliski
                WHERE ustProjeId = @ustProjeId AND altProjeId = @altProjeId";

            using (var checkCommand = new SqlCommand(checkSql, connection, transaction))
            {
                checkCommand.Parameters.AddWithValue("@ustProjeId", ustProjeId);
                checkCommand.Parameters.AddWithValue("@altProjeId", altProjeId);
                int count = (int)checkCommand.ExecuteScalar();
                if (count > 0)
                {
                    return true;
                }
            }

            const string sql = @"
                INSERT INTO ProjeFinans_ProjeIliski 
                    (ustProjeId, altProjeId)
                VALUES 
                    (@ustProjeId, @altProjeId);";

            using (var command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@ustProjeId", ustProjeId);
                command.Parameters.AddWithValue("@altProjeId", altProjeId);
                command.ExecuteNonQuery();
            }

            return true;
        }
        public bool CheckAltProje(SqlConnection connection, int projeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            const string sql = @"
                SELECT COUNT(*)
                FROM ProjeFinans_ProjeIliski
                WHERE AltProjeId = @projeId";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@projeId", projeId);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        public List<int> GetAltProjeler(SqlConnection connection, int projeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            var altProjeler = new List<int>();
            const string sql = @"
                SELECT AltProjeId
                FROM ProjeFinans_ProjeIliski
                WHERE UstProjeId = @projeId";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@projeId", projeId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            altProjeler.Add(reader.GetInt32(0));
                        }
                    }
                }
            }

            return altProjeler;
        }
        public int? GetUstProjeId(SqlConnection connection, int altProjeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            const string sql = @"
                SELECT UstProjeId
                FROM ProjeFinans_ProjeIliski
                WHERE AltProjeId = @altProjeId";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@altProjeId", altProjeId);
                object result = command.ExecuteScalar();
                return result != null && result != DBNull.Value
                    ? Convert.ToInt32(result)
                    : (int?)null;
            }
        }
        public bool AltProjeSil(SqlConnection connection, SqlTransaction transaction, int ustProjeId, int altProjeId)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            const string sql = @"
                DELETE FROM ProjeFinans_ProjeIliski 
                WHERE ustProjeId = @ustProjeId AND altProjeId = @altProjeId";

            using (var command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@ustProjeId", ustProjeId);
                command.Parameters.AddWithValue("@altProjeId", altProjeId);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
