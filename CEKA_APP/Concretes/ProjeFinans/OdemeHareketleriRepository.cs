using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.ProjeFinans
{
    public class OdemeHareketleriRepository:IOdemeHareketleriRepository
    {
        public bool SaveOdemeHareketi(OdemeHareketleri odemeHareketi, SqlTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
            }

            var sql = "INSERT INTO ProjeFinans_OdemeHareketleri (odemeId, odemeMiktari, odemeTarihi, odemeAciklama) VALUES (@odemeId, @odemeMiktari, @odemeTarihi, @odemeAciklama)";

            using (var command = new SqlCommand(sql, transaction.Connection, transaction))
            {
                command.Parameters.Add("@odemeId", SqlDbType.Int).Value = odemeHareketi.odemeId;
                command.Parameters.Add("@odemeMiktari", SqlDbType.Decimal).Value = odemeHareketi.odemeMiktari;
                command.Parameters.Add("@odemeTarihi", SqlDbType.Date).Value = odemeHareketi.odemeTarihi;
                command.Parameters.Add("@odemeAciklama", SqlDbType.NVarChar, 500).Value = odemeHareketi.odemeAciklama;

                command.ExecuteNonQuery();
                return true;
            }
        }
        public List<OdemeHareketleri> GetOdemeHareketleriByOdemeId(int odemeId)
        {
            var odemeHareketleriList = new List<OdemeHareketleri>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT odemeHareketId, odemeId, odemeMiktari, odemeTarihi, odemeAciklama FROM ProjeFinans_OdemeHareketleri WHERE odemeId = @odemeId ORDER BY odemeTarihi DESC";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@odemeId", odemeId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            odemeHareketleriList.Add(new OdemeHareketleri
                            {
                                odemeHareketId = Convert.ToInt32(reader["odemeHareketId"]),
                                odemeId = Convert.ToInt32(reader["odemeId"]),
                                odemeMiktari = Convert.ToDecimal(reader["odemeMiktari"]),
                                odemeTarihi = Convert.ToDateTime(reader["odemeTarihi"]),
                                odemeAciklama = reader["odemeAciklama"].ToString(),
                            });
                        }
                    }
                }
            }
            return odemeHareketleriList;
        }
        public void DeleteOdemeHareketleriByOdemeIds(List<int> odemeIds, SqlTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

            if (odemeIds == null || odemeIds.Count == 0)
            {
                return;
            }

            string createTempTableQuery = @"
                CREATE TABLE #TempOdemeIds (odemeId INT);
            ";
            using (var createCmd = new SqlCommand(createTempTableQuery, transaction.Connection, transaction))
            {
                createCmd.ExecuteNonQuery();
            }

            using (var bulkCopy = new SqlBulkCopy(transaction.Connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = "#TempOdemeIds";
                bulkCopy.ColumnMappings.Add("odemeId", "odemeId");

                DataTable dt = new DataTable();
                dt.Columns.Add("odemeId", typeof(int));
                foreach (var id in odemeIds)
                {
                    dt.Rows.Add(id);
                }
                bulkCopy.WriteToServer(dt);
            }

            string deleteQuery = @"
                DELETE FROM ProjeFinans_OdemeHareketleri
                WHERE odemeId IN (SELECT odemeId FROM #TempOdemeIds);
            ";
            using (var deleteCmd = new SqlCommand(deleteQuery, transaction.Connection, transaction))
            {
                deleteCmd.ExecuteNonQuery();
            }

            string dropTempTableQuery = "DROP TABLE #TempOdemeIds;";
            using (var dropCmd = new SqlCommand(dropTempTableQuery, transaction.Connection, transaction))
            {
                dropCmd.ExecuteNonQuery();
            }
        }
    }
}
