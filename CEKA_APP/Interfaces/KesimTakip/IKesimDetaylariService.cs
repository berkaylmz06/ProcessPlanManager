using CEKA_APP.Entitys;
using System.Collections.Generic;
using System.Data;

namespace CEKA_APP.Interfaces.KesimTakip
{
    public interface IKesimDetaylariService
    {
        void SaveKesimDetaylariData(string kalite, string malzeme, string malzemeKod, string proje, int kesilecekAdet, int toplamAdet, bool ekBilgi);
        DataTable GetKesimDetaylariBilgi();
        bool PozExists(string kalite, string malzeme, string malzemekod, string proje);

        bool UpdateKesilmisAdet(string kalite, string malzeme, string malzemekod, string proje, decimal sondurum);
        List<KesimDetaylari> GetKesimDetaylariBilgileri(string projeAdi = null, string grupAdi = null);
        bool GuncelleKesimDetaylari(string kalite, string malzeme, string kalipNo, string kesilecekPozlar, string proje, decimal silinecekAdet = 0, bool tamSilme = false);
        (decimal toplamAdet, decimal kesilmisAdet, decimal kesilecekAdet, List<string> eslesenPozlar) GetAdetlerVeEslesenPozlar(
               string kalite, string malzeme, string proje, string malzemeKodIlkKisim, string malzemeKodUcuncuKisim);
    }
}
