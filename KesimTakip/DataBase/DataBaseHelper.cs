using System;
using System.Data.SqlClient;

namespace KesimTakip.DataBase
{
    class DataBaseHelper
    {
        private static readonly string connectionString = "Server=SQLSERVER;Database=KesimTakip;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
