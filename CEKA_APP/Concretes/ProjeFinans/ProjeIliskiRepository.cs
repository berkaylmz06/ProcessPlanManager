using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class ProjeIliskiRepository: IProjeIliskiRepository
    {
        public bool AltProjeEkle(int ustProjeId, int altProjeId, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            string sql = @"
            INSERT INTO ProjeFinans_ProjeIliski 
            (UstProjeId, AltProjeId)
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


        public bool CheckAltProje(int projeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {

                connection.Open();
                string sql = @"
                SELECT COUNT(*)
                FROM ProjeFinans_ProjeIliski
                WHERE AltProjeId = @projeId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@projeId", projeId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public List<int> GetAltProjeler(int projeId)
        {
            var altProjeler = new List<int>();

            using (var connection = DataBaseHelper.GetConnection())
            {

                connection.Open();
                string sql = @"
                SELECT AltProjeId
                FROM ProjeFinans_ProjeIliski
                WHERE UstProjeId = @projeId OR AltProjeId = @projeId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@projeId", projeId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                int altProjeId = reader.GetInt32(0);
                                altProjeler.Add(altProjeId);
                            }
                        }
                    }
                }
            }
            return altProjeler;
        }

        public int? GetUstProjeId(int altProjeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string sql = @"
                SELECT UstProjeId
                FROM ProjeFinans_ProjeIliski
                WHERE AltProjeId = @altProjeId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@altProjeId", altProjeId);
                    object result = command.ExecuteScalar();
                    return result != DBNull.Value && result != null
                        ? Convert.ToInt32(result)
                        : (int?)null;
                }
            }
        }
    }
}
