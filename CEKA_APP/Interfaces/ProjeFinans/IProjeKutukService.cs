using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IProjeKutukService
    {
        bool ProjeKutukEkle(ProjeKutuk kutuk);
        bool ProjeFiyatlandirmaEkle(string projeNo, decimal fiyat);
        bool AltProjeEkle(int ustProjeId, int altProjeId);
        bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi);
        bool UpdateProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi);
        ProjeBilgi GetProjeBilgileri(string projeNo);
        ProjeKutuk ProjeKutukAra(int projeId);
        void UpdateToplamBedel(string projeNo, decimal toplamBedel);
        bool IsFaturalamaSekliTekil(int projeId);
        bool HasRelatedRecords(int projeId, List<int> altProjeler);
        bool ProjeKutukGuncelle(ProjeKutuk yeniKutuk);
        bool ProjeKutukSil(string projeNo, List<string> altProjeler);
        string GetProjeParaBirimi(int projeId);
    }
}
