using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IProjeKutukRepository
    {
        bool ProjeKutukEkle(ProjeKutuk kutuk, SqlTransaction transaction);
        bool ProjeFiyatlandirmaEkle(string projeNo, decimal fiyat, SqlTransaction transaction);
        bool AltProjeEkle(int ustProjeId, int altProjeId, SqlTransaction transaction);
        bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, SqlTransaction transaction);
        bool UpdateProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, SqlTransaction transaction);
        ProjeBilgi GetProjeBilgileri(string projeNo);
        ProjeKutuk ProjeKutukAra(int projeId);
        void UpdateToplamBedel(SqlTransaction transaction, string projeNo, decimal toplamBedel);
        bool IsFaturalamaSekliTekil(int projeId);
        bool HasRelatedRecords(int projeId, List<int> altProjeler);
        bool ProjeKutukGuncelle(SqlTransaction transaction, ProjeKutuk yeniKutuk);
        bool ProjeKutukSil(SqlTransaction transaction, string projeNo, List<string> altProjeler);
        string GetProjeParaBirimi(int projeId);
    }
}
