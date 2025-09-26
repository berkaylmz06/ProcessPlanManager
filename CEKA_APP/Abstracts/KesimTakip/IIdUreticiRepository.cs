using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.KesimTakip
{
    public interface IIdUreticiRepository
    {
        int GetSiradakiId(SqlConnection connection);
        bool SiradakiIdKaydet(SqlConnection connection, SqlTransaction transaction, int id);
    }
}
