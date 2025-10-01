using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IProjeIliskiService
    {
        bool AltProjeEkle(int ustProjeId, int altProjeId);
        bool CheckAltProje(int projeId);
        List<int> GetAltProjeler(int projeId);
        int? GetUstProjeId(int altProjeId);
        bool AltProjeSil(int ustProjeId, int altProjeId);
    }
}
