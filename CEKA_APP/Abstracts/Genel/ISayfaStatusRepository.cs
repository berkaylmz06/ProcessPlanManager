using CEKA_APP.Entitys.Genel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.Genel
{
    public interface ISayfaStatusRepository
    {
        int Insert(SayfaStatus status, SqlTransaction transaction);
        SayfaStatus Get(int sayfaId, int projeId);
        void Update(SayfaStatus status, SqlTransaction transaction);
        void Delete(int sayfaStatusId, SqlTransaction transaction);
        bool IsAllAltProjelerSayfa4Kapali(int projeId);
        bool IsSayfa3Kapali(int projeId);
        List<string> GetNedenTamamlanmadiByProjeId(int projeId);
    }
}
