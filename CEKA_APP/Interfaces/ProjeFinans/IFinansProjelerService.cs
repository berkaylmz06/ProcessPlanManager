using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IFinansProjelerService
    {
        bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi);
        bool UpdateProjeFinans(int projeId, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, out bool degisiklikVar);
        ProjeBilgi GetProjeBilgileri(int projeId);
        bool ProjeSil(int projeId);
        int? GetProjeIdByNo(string projeNo);
        string GetProjeNoById(int projeId);
        ProjeBilgi GetProjeBilgileriByNo(string projeNo);
    }
}
