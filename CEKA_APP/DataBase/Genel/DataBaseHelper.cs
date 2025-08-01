using System;
using System.Data.SqlClient;

namespace CEKA_APP.DataBase
{
    class DataBaseHelper
    {
        //private static readonly string connectionString ="Server=192.168.2.22;Database=CEKA_APP;User Id=sa;Password=DOibg544;TrustServerCertificate=True;";
        private static readonly string connectionString = "Server=IFS-DEVELOPER\\SQLEXPRESS;Database=CEKA_APP;Integrated Security=True;TrustServerCertificate=True;";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
