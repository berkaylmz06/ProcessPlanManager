using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IFinansProjelerRepository
    {
        bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, SqlTransaction transaction);
        bool UpdateProjeFinans(int projeId, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, SqlTransaction transaction, out bool degisiklikVar);
        ProjeBilgi GetProjeBilgileri(int projeId);
        bool ProjeSil(int projeId, SqlTransaction transaction);
        int? GetProjeIdByNo(string projeNo);
        string GetProjeNoById(int projeId);
        ProjeBilgi GetProjeBilgileriByNo(string projeNo);
    }
}
