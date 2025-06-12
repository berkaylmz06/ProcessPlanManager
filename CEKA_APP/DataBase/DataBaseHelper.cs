using System;
using System.Data.SqlClient;

namespace CEKA_APP.DataBase
{
    class DataBaseHelper
    {
        private static readonly string connectionString = "Server=SQLSERVER;Database=CEKA_APP;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
