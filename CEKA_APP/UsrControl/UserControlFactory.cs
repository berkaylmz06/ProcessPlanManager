using CEKA_APP.Interfaces;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Services.ProjeFinans;
using CEKA_APP.UsrControl.Interfaces;
using CEKA_APP.UsrControl.ProjeFinans;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.UsrControl
{
    public class UserControlFactory : IUserControlFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public UserControlFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public ctlMusteriler CreateMusterilerControl()
        {
            var musterilerService = _serviceProvider.GetRequiredService<IMusterilerService>();
            return new ctlMusteriler(musterilerService);
        }

        public ctlOdemeSartlari CreateOdemeSartlariControl()
        {
            var odemeSartlariService = _serviceProvider.GetRequiredService<IOdemeSartlariService>();
            var odemeHareketleriService = _serviceProvider.GetRequiredService<IOdemeHareketleriService>();
            var kilometreTaslariService = _serviceProvider.GetRequiredService<IKilometreTaslariService>();
            var fiyatlandirmaService = _serviceProvider.GetRequiredService<IFiyatlandirmaService>();
            var musterilerService = _serviceProvider.GetRequiredService<IMusterilerService>();
            var finansProjelerService = _serviceProvider.GetRequiredService<IFinansProjelerService>();
            var projeIliskiService = _serviceProvider.GetRequiredService<IProjeIliskiService>();
            var projeKutukService = _serviceProvider.GetRequiredService<IProjeKutukService>();
            var teminatMektuplariService = _serviceProvider.GetRequiredService<ITeminatMektuplariService>();
            var sayfaStatusService = _serviceProvider.GetRequiredService<ISayfaStatusService>();

            return new ctlOdemeSartlari(odemeSartlariService, odemeHareketleriService, kilometreTaslariService, fiyatlandirmaService, musterilerService, finansProjelerService, projeIliskiService, projeKutukService,teminatMektuplariService, sayfaStatusService);
        }

        public ctlOdemeSartlariListe CreateOdemeSartlariListeControl()
        {
            var odemeSartlariService = _serviceProvider.GetRequiredService<IOdemeSartlariService>();
            var odemeHareketleriService = _serviceProvider.GetRequiredService<IOdemeHareketleriService>();

            return new ctlOdemeSartlariListe(odemeSartlariService, odemeHareketleriService);
        }

        public ctlProjeBilgileri CreateProjeBilgileriControl()
        {
            var finansProjelerService = _serviceProvider.GetRequiredService<IFinansProjelerService>();
            var projeKutukService = _serviceProvider.GetRequiredService<IProjeKutukService>();

            return new ctlProjeBilgileri(finansProjelerService, projeKutukService);
        }

        public ctlProjeFiyatlandirma CreateProjeFiyatlandirmaControl()
        {
            var odemeSartlariService = _serviceProvider.GetRequiredService<IOdemeSartlariService>();
            var fiyatlandirmaService = _serviceProvider.GetRequiredService<IFiyatlandirmaService>();
            var fiyatlandirmaKalemleriService = _serviceProvider.GetRequiredService<IFiyatlandirmaKalemleriService>();
            var finansProjelerService = _serviceProvider.GetRequiredService<IFinansProjelerService>();
            var projeIliskiService = _serviceProvider.GetRequiredService<IProjeIliskiService>();
            var projeKutukService = _serviceProvider.GetRequiredService<IProjeKutukService>();

            return new ctlProjeFiyatlandirma(odemeSartlariService, fiyatlandirmaService, fiyatlandirmaKalemleriService, finansProjelerService, projeIliskiService, projeKutukService);
        }

        public ctlProjeKutuk CreateProjeKutukControl()
        {
            var fiyatlandirmaService = _serviceProvider.GetRequiredService<IFiyatlandirmaService>();
            var musterilerService = _serviceProvider.GetRequiredService<IMusterilerService>();
            var finansProjelerService = _serviceProvider.GetRequiredService<IFinansProjelerService>();
            var projeKutukService = _serviceProvider.GetRequiredService<IProjeKutukService>();
            var sayfaStatusService = _serviceProvider.GetRequiredService<ISayfaStatusService>();

            return new ctlProjeKutuk(fiyatlandirmaService, musterilerService, finansProjelerService, projeKutukService, sayfaStatusService);
        }

        public ctlSevkiyat CreateSevkiyatControl()
        {
            var sevkiyatService = _serviceProvider.GetRequiredService<ISevkiyatService>();
            var fiyatlandirmaService = _serviceProvider.GetRequiredService<IFiyatlandirmaService>();
            var sevkiyatPaketleriService = _serviceProvider.GetRequiredService<ISevkiyatPaketleriService>();
            var finansProjelerService = _serviceProvider.GetRequiredService<IFinansProjelerService>();
            var projeIliskiService = _serviceProvider.GetRequiredService<IProjeIliskiService>();
            var projeKutukService = _serviceProvider.GetRequiredService<IProjeKutukService>();
            var sayfaStatusService = _serviceProvider.GetRequiredService<ISayfaStatusService>();

            return new ctlSevkiyat(sevkiyatService, fiyatlandirmaService, sevkiyatPaketleriService, finansProjelerService, projeIliskiService, projeKutukService, sayfaStatusService);
        }

        public ctlTakipTakvimi CreateTakipTakvimiControl()
        {
            var odemeSartlariService = _serviceProvider.GetRequiredService<IOdemeSartlariService>();

            return new ctlTakipTakvimi(odemeSartlariService);
        }

        public ctlTeminatMektuplari CreateTeminatMektuplariControl()
        {
            var musterilerService = _serviceProvider.GetRequiredService<IMusterilerService>();
            var finansProjelerService = _serviceProvider.GetRequiredService<IFinansProjelerService>();
            var projeKutukService = _serviceProvider.GetRequiredService<IProjeKutukService>();
            var teminatMektuplariService = _serviceProvider.GetRequiredService<ITeminatMektuplariService>();

            return new ctlTeminatMektuplari(musterilerService, finansProjelerService,projeKutukService, teminatMektuplariService);
        }
    }
}
