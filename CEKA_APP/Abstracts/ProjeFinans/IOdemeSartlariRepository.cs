using CEKA_APP.Entitys.ProjeFinans;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IOdemeSartlariRepository
    {
        void SaveOrUpdateOdemeBilgi(SqlConnection connection, SqlTransaction transaction, OdemeSartlari odemeSartlari);
        string GetFaturaNo(SqlConnection connection, int projeId, int kilometreTasiId);
        List<OdemeSartlari> GetOdemeBilgileri(SqlConnection connection);
        List<OdemeSartlari> GetOdemeBilgileriByProjeId(SqlConnection connection, int projeId);
        OdemeSartlari GetOdemeBilgi(SqlConnection connection, string projeNo, int kilometreTasiId);
        OdemeSartlari GetOdemeBilgiByOdemeId(SqlConnection connection, int odemeId);
        void DeleteOdemeBilgi(SqlConnection connection, SqlTransaction transaction, int projeId, int kilometreTasiId);
        bool UpdateFaturaNo(SqlConnection connection, SqlTransaction transaction, int odemeId, string faturaNo);
        bool OdemeSartlariSil(SqlConnection connection, SqlTransaction transaction, int projeId);
        string GetOdemeBilgileriQuery();
    }
}
