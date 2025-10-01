using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.ERP
{
    public interface IAutoCadAktarimRepository
    {
        void SaveAutoCadData(SqlConnection connection, SqlTransaction transaction, string projeAdi, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, Guid yuklemeId, decimal netAgirlik, int grupAdet);
        List<AutoCadAktarimDetay> GetAutoCadKayitlari(SqlConnection connection, string projeAdi);
        DataTable GruplariGetir(SqlConnection connection, string projeAdi);
        DataTable UstGruplariGetir(SqlConnection connection, string projeAdi);
        DataTable MalzemeleriGetir(SqlConnection connection, string projeAdi, string grupAdi);
        DataTable MalzemeDetaylariniGetir(SqlConnection connection, string projeAdi, string grupAdi, string malzemeKod);
        void ProjeEkle(SqlConnection connection, SqlTransaction transaction, string projeAdi, string aciklama, DateTime olusturmaTarihi);
        void StandartGrupEkle(SqlConnection connection, SqlTransaction transaction, string grupNo);
        List<string> GetirStandartGruplarListe(SqlConnection connection);
        bool GetirStandartGruplar(SqlConnection connection, string kalip);
        void StandartGrupSil(SqlConnection connection, SqlTransaction transaction, string grupNo);
        void GrupEkleGuncelle(SqlConnection connection, SqlTransaction transaction, string projeNo, string grupAdi, string eskiGrupAdi, string ustGrupAdi, Guid? yuklemeId = null, int takimCarpani = 1);
        void UstGrupEkleGuncelle(SqlConnection connection, SqlTransaction transaction, string projeNo, string ustGrupAdi, Guid? yuklemeId, int takimCarpani, string eskiUstGrupAdi = null);
        void GrupSil(SqlConnection connection, SqlTransaction transaction, string projeAdi, string grupAdi);
        void UstGrupSil(SqlConnection connection, SqlTransaction transaction, string projeAdi, string ustGrupAdi, Guid yuklemeId);
        void MalzemeEkleGuncelle(SqlConnection connection, SqlTransaction transaction, string projeNo, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, decimal agirlik, Guid? yuklemeId = null, string eskiMalzemeKod = null);
        void MalzemeSil(SqlConnection connection, SqlTransaction transaction, string projeAdi, string grupAdi, string malzemeKod);
        void UpdateTakimCarpaniVeAltGruplar(SqlConnection connection, SqlTransaction transaction, string projeAdi, string ustGrup, Guid? yuklemeId, int takimCarpani);
        void UpdateTakimCarpani(SqlConnection connection, SqlTransaction transaction, string projeAdi, string grupAdi, int takimCarpani);
        (bool, int) AdetGetir(SqlConnection connection, string kalite, string malzeme, string kalip, string proje, decimal girilenAdet);
        (bool isValid, int toplamAdetIfs, int toplamAdetYuklenen) KontrolAdeta(SqlConnection connection, string kalite, string malzeme, string kalip, string proje, int girilenAdet);
        void BaglaProjeVeGrup(SqlConnection connection, SqlTransaction transaction, string projeAdi, string secilenGrup);
        decimal GetNetAgirlik(SqlConnection connection, string kalite, string malzeme, string kalipPoz, string proje);
    }
}
