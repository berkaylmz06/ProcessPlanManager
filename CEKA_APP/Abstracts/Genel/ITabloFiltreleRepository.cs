using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.Genel
{
    public interface ITabloFiltreleRepository
    {
        DataTable GetFilteredData(SqlConnection connection, string baseSql, Dictionary<string, object> filters);
    }
}
