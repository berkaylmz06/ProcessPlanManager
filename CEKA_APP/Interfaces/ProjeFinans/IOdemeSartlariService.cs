using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IOdemeSartlariService
    {
        void SaveOrUpdateOdemeBilgi(OdemeSartlari odemeSartlari);
        string GetFaturaNo(int projeId, int kilometreTasiId);
        List<OdemeSartlari> GetOdemeBilgileri();
        List<OdemeSartlari> GetOdemeBilgileriByProjeId(int projeId);
        OdemeSartlari GetOdemeBilgi(string projeNo, int kilometreTasiId);
        OdemeSartlari GetOdemeBilgiByOdemeId(int odemeId);
        void DeleteOdemeBilgi(int projeId, int kilometreTasiId);
        bool UpdateFaturaNo(int odemeId, string faturaNo);
        bool OdemeSartlariSil(int projeId);
        string GetOdemeBilgileriQuery();
    }
}
