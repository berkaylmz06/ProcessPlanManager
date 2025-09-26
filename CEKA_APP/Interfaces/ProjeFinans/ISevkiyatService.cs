using CEKA_APP.Entitys.ProjeFinans;
using System.Collections.Generic;

namespace CEKA_APP.Interfaces
{
    public interface ISevkiyatService
    {
        List<Sevkiyat> GetSevkiyatByProje(int projeId);
        int SevkiyatKaydet(Sevkiyat sevkiyat);
        void SevkiyatGuncelle(Sevkiyat sevkiyat);
        bool SevkiyatSilBySevkiyatId(int projeId, int sevkiyatId, int aracSira);
        bool SevkiyatSil(int projeId);
    }
}