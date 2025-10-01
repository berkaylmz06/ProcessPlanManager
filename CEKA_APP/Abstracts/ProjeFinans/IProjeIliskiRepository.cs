using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IProjeIliskiRepository
    {
        bool AltProjeEkle(SqlConnection connection, SqlTransaction transaction, int ustProjeId, int altProjeId);
        bool CheckAltProje(SqlConnection connection, int projeId);
        List<int> GetAltProjeler(SqlConnection connection, int projeId);
        int? GetUstProjeId(SqlConnection connection, int altProjeId);
        bool AltProjeSil(SqlConnection connection, SqlTransaction transaction, int ustProjeId, int altProjeId);
    }
}
