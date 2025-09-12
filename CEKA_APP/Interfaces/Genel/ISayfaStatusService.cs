using CEKA_APP.Entitys.Genel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.Genel
{
    public interface ISayfaStatusService
    {
        int Insert(SayfaStatus status);
        SayfaStatus Get(int sayfaId, int projeId);
        void Update(SayfaStatus status);
        void Delete(int sayfaStatusId);
        bool IsAllAltProjelerSayfa4Kapali(int projeId);
        bool IsSayfa3Kapali(int projeId);
        List<string> GetNedenTamamlanmadiByProjeId(int projeId);
    }
}
