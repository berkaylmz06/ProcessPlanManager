using CEKA_APP.UserControls.KesimTakip;
using CEKA_APP.UserControls.ProjeTakip;
using CEKA_APP.UsrControl.ProjeFinans;

namespace CEKA_APP.UsrControl.Interfaces
{
    public interface IUserControlFactory
    {
        ctlOdemeSartlari CreateOdemeSartlariControl();
        ctlOdemeSartlariListe CreateOdemeSartlariListeControl();
        ctlProjeKutuk CreateProjeKutukControl();
        ctlProjeFiyatlandirma CreateProjeFiyatlandirmaControl();
        ctlSevkiyat CreateSevkiyatControl();
        ctlMusteriler CreateMusterilerControl();
        ctlTeminatMektuplari CreateTeminatMektuplariControl();
        ctlProjeBilgileri CreateProjeBilgileriControl();
        ctlTakipTakvimi CreateTakipTakvimiControl();
        ctlKesimYonetimi CreateKesimYonetimiControl();


        ctlKesimPlaniEkle CreateKesimPlaniEkleControl();
        ctlKesimDetaylari CreateKesimDetaylariControl();
        ctlKesimYap CreateKesimYapControl();
        ctlProjeOgeleri CreateProjeOgeleriControl();
        ctlYerlesimPlaniBilgi CreateYerlesimPlaniBilgiControl();
        ctlYapilanKesimleriGor CreateYapilanKesimleriGorControl();


        ctlAutoCadAktarim CreateAutoCadAktarimControl();
        ctlKarsilastirmaTablosu CreateKarsilastirmaTablosuService();


        ctlSistemHareketleri CreateSistemHareketleriService();
        ctlKullaniciAyarlari CreateKullaniciAyarlariService();
        ctlSorunlar CreateSorunlarService();


        ctlProjeKarti CreateProjeKartiControl();
    }
}
