using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.Genel
{
    public interface IDataBaseRepository
    {
        SqlConnection GetConnection();
    }
}
