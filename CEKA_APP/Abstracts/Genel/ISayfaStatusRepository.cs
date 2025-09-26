using CEKA_APP.Entitys.Genel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.Genel
{
    public interface ISayfaStatusRepository
    {
        int Insert(SqlConnection connection, SqlTransaction transaction, SayfaStatus status);
        SayfaStatus Get(SqlConnection connection, int sayfaId, int projeId);
        void Update(SqlConnection connection, SqlTransaction transaction, SayfaStatus status);
        void Delete(SqlConnection connection, SqlTransaction transaction, int sayfaStatusId);
        bool IsAllAltProjelerSayfa4Kapali(SqlConnection connection, SqlTransaction transaction, int projeId);
        bool IsSayfa3Kapali(SqlConnection connection, SqlTransaction transaction, int projeId);
        List<string> GetNedenTamamlanmadiByProjeId(SqlConnection connection, int projeId);
    }
}
