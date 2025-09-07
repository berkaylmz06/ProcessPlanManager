using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IProjeIliskiRepository
    {
        bool AltProjeEkle(int ustProjeId, int altProjeId, SqlTransaction transaction);
        bool CheckAltProje(int projeId);
        List<int> GetAltProjeler(int projeId);
        int? GetUstProjeId(int altProjeId);
    }
}
