using CEKA_APP.Interfaces;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Interfaces.Sistem;
using CEKA_APP.UserControls.ProjeTakip;
using CEKA_APP.UsrControl.Interfaces;
using CEKA_APP.UsrControl.ProjeFinans;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CEKA_APP.UsrControl
{
    public class UserControlFactory : IUserControlFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public UserControlFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public ctlAutoCadAktarim CreateAutoCadAktarimControl()
        {
            return new ctlAutoCadAktarim(_serviceProvider);
        }

        public ctlKarsilastirmaTablosu CreateKarsilastirmaTablosuService()
        {
            return new ctlKarsilastirmaTablosu(_serviceProvider);
        }

        public ctlKesimDetaylari CreateKesimDetaylariControl()
        {
            return new ctlKesimDetaylari(_serviceProvider);
        }

        public ctlKesimPlaniEkle CreateKesimPlaniEkleControl()
        {
            return new ctlKesimPlaniEkle(_serviceProvider);
        }

        public ctlKesimYap CreateKesimYapControl()
        {
            return new ctlKesimYap(_serviceProvider);
        }

        public ctlKullaniciAyarlari CreateKullaniciAyarlariService()
        {
            return new ctlKullaniciAyarlari(_serviceProvider);
        }

        public ctlMusteriler CreateMusterilerControl()
        {
            return new ctlMusteriler(_serviceProvider);
        }

        public ctlOdemeSartlari CreateOdemeSartlariControl()
        {
            return new ctlOdemeSartlari(_serviceProvider);
        }

        public ctlOdemeSartlariListe CreateOdemeSartlariListeControl()
        {
            return new ctlOdemeSartlariListe(_serviceProvider);
        }

        public ctlProjeBilgileri CreateProjeBilgileriControl()
        {
            return new ctlProjeBilgileri(_serviceProvider);
        }

        public ctlProjeFiyatlandirma CreateProjeFiyatlandirmaControl()
        {
            return new ctlProjeFiyatlandirma(_serviceProvider);
        }

        public ctlProjeKarti CreateProjeKartiControl()
        {
            return new ctlProjeKarti(_serviceProvider);
        }

        public ctlProjeKutuk CreateProjeKutukControl()
        {
            return new ctlProjeKutuk(_serviceProvider);
        }

        public ctlProjeOgeleri CreateProjeOgeleriControl()
        {
            return new ctlProjeOgeleri(_serviceProvider);
        }

        public ctlSevkiyat CreateSevkiyatControl()
        {
            return new ctlSevkiyat(_serviceProvider);
        }

        public ctlSistemHareketleri CreateSistemHareketleriService()
        {
            return new ctlSistemHareketleri(_serviceProvider);
        }

        public ctlSorunlar CreateSorunlarService()
        {
            return new ctlSorunlar(_serviceProvider);
        }

        public ctlTakipTakvimi CreateTakipTakvimiControl()
        {
            return new ctlTakipTakvimi(_serviceProvider);
        }

        public ctlTeminatMektuplari CreateTeminatMektuplariControl()
        {
            return new ctlTeminatMektuplari(_serviceProvider);
        }

        public ctlYapilanKesimleriGor CreateYapilanKesimleriGorControl()
        {
            return new ctlYapilanKesimleriGor(_serviceProvider);
        }

        public ctlYerlesimPlaniBilgi CreateYerlesimPlaniBilgiControl()
        {
            return new ctlYerlesimPlaniBilgi(_serviceProvider);
        }
    }
}
