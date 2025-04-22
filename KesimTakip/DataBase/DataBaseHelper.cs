using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesimTakip.DataBase
{
    class DataBaseHelper
    {
        private static readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=KesimTakip";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
