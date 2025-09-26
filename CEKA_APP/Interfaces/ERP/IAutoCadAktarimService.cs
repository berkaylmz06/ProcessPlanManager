using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Interfaces.ERP
{
    public interface IAutoCadAktarimService
    {
        void SaveAutoCadData(string projeAdi, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, Guid yuklemeId, decimal netAgirlik, int grupAdet);
        List<AutoCadAktarimDetay> GetAutoCadKayitlari(string projeAdi);
        DataTable GruplariGetir(string projeAdi);
        DataTable UstGruplariGetir(string projeAdi);
        DataTable MalzemeleriGetir(string projeAdi, string grupAdi);
        DataTable MalzemeDetaylariniGetir(string projeAdi, string grupAdi, string malzemeKod);
        void ProjeEkle(string projeAdi, string aciklama, DateTime olusturmaTarihi);
        void StandartGrupEkle(string grupNo);
        List<string> GetirStandartGruplarListe();
        bool GetirStandartGruplar(string kalip);
        void StandartGrupSil(string grupNo);
        void GrupEkleGuncelle(string projeNo, string grupAdi, string eskiGrupAdi, string ustGrupAdi, Guid? yuklemeId = null, int takimCarpani = 1);
        void UstGrupEkleGuncelle(string projeNo, string ustGrupAdi, Guid? yuklemeId, int takimCarpani, string eskiUstGrupAdi = null);
        void GrupSil(string projeAdi, string grupAdi);
        void UstGrupSil(string projeAdi, string ustGrupAdi, Guid yuklemeId);
        void MalzemeEkleGuncelle(string projeNo, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, decimal agirlik, Guid? yuklemeId = null, string eskiMalzemeKod = null);
        void MalzemeSil(string projeAdi, string grupAdi, string malzemeKod);
        void UpdateTakimCarpaniVeAltGruplar(string projeAdi, string ustGrup, Guid? yuklemeId, int takimCarpani);
        void UpdateTakimCarpani(string projeAdi, string grupAdi, int takimCarpani);
        (bool, int) AdetGetir(string kalite, string malzeme, string kalip, string proje, decimal girilenAdet);
        (bool isValid, int toplamAdetIfs, int toplamAdetYuklenen) KontrolAdeta(string kalite, string malzeme, string kalip, string proje, int girilenAdet);
        void BaglaProjeVeGrup(string projeAdi, string secilenGrup);
        decimal GetNetAgirlik(string kalite, string malzeme, string kalipPoz, string proje);
    }
}
