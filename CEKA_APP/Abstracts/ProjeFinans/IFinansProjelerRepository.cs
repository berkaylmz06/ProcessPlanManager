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
        bool ProjeEkleProjeFinans(SqlConnection connection, SqlTransaction transaction, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi);
        bool UpdateProjeFinans(SqlConnection connection, SqlTransaction transaction, int projeId, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, out bool degisiklikVar);
        ProjeBilgi GetProjeBilgileri(SqlConnection connection, SqlTransaction transaction, int projeId);
        bool ProjeSil(SqlConnection connection, SqlTransaction transaction, int projeId);
        int? GetProjeIdByNo(SqlConnection connection, SqlTransaction transaction, string projeNo);
        string GetProjeNoById(SqlConnection connection, int projeId);
        ProjeBilgi GetProjeBilgileriByNo(SqlConnection connection, SqlTransaction transaction, string projeNo);
    }
}
