using CEKA_APP.UsrControl.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
