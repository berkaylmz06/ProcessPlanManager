using System.Collections.Generic;
using System.Data;

namespace CEKA_APP.Interfaces.Genel
{
    public interface ITabloFiltreleService
    {
        DataTable GetFilteredData( string baseSql, Dictionary<string, object> filters);
    }
}
