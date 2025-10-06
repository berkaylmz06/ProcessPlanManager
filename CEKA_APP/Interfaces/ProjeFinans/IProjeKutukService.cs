using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IProjeKutukService
    {
        bool ProjeKutukEkle(ProjeKutuk kutuk);
        ProjeKutuk ProjeKutukAra(int projeId);
        void UpdateToplamBedel(string projeNo, decimal toplamBedel);
        bool IsFaturalamaSekliTekil(int projeId);
        (bool HasRelated, List<string> Details) HasRelatedRecords(int projeId, List<int> altProjeler);
        bool ProjeKutukGuncelle(ProjeKutuk yeniKutuk);
        bool ProjeKutukSil(int projeId, List<int> altProjeIds);
        string GetProjeParaBirimi(int projeId);
        bool UpdateProjeKutukDurum(int projeId, bool? montajTamamlandiMi);
        ProjeKutuk GetProjeKutukStatus(int projeId);
        bool ProjeNoKontrol(string projeNo);

    }
}
