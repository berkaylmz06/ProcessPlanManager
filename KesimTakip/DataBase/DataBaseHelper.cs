using System;
using System.Data.SqlClient;

namespace KesimTakip.DataBase
{
    class DataBaseHelper
    {
        private static readonly string connectionString = "Server=IFS-DEVELOPER\\SQLEXPRESS;Database=KesimTakip;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
