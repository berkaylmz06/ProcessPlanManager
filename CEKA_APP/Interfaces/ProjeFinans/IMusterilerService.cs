using CEKA_APP.Entitys.ProjeFinans;
using System.Collections.Generic;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IMusterilerService
    {
        void MusteriKaydet(Musteriler musteri);
        bool MusteriNoVarMi(string musteriNo);
        Musteriler GetMusteriByMusteriNo(string musteriNo);
        List<Musteriler> GetMusteriler();
        void TumMusterileriSil();
        string GetMusterilerQuery();
        List<Musteriler> GetMusterilerAraFormu();
        string GetMusterilerAraFormuQuery();
    }
}
