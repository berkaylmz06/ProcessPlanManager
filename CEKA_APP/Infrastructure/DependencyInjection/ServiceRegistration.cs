using CEKA_APP.Abstracts;
using CEKA_APP.Abstracts.ERP;
using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Abstracts.ProjeTakip;
using CEKA_APP.Abstracts.Sistem;
using CEKA_APP.Concretes.ERP;
using CEKA_APP.Concretes.Genel;
using CEKA_APP.Concretes.KesimTakip;
using CEKA_APP.Concretes.ProjeFinans;
using CEKA_APP.Concretes.ProjeTakip;
using CEKA_APP.Concretes.Sistem;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Forms.ProjeTakip;
using CEKA_APP.Interfaces;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Interfaces.ProjeTakip;
using CEKA_APP.Interfaces.Sistem;
using CEKA_APP.Services.ERP;
using CEKA_APP.Services.Genel;
using CEKA_APP.Services.KesimTakip;
using CEKA_APP.Services.ProjeFinans;
using CEKA_APP.Services.ProjeTakip;
using CEKA_APP.Services.Sistem;
using Microsoft.Extensions.DependencyInjection;

namespace CEKA_APP.Infrastructure.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOdemeSartlariRepository, OdemeSartlariRepository>();
            services.AddScoped<IOdemeHareketleriRepository, OdemeHareketleriRepository>();
            services.AddScoped<IOdemeSartlariService, OdemeSartlariService>();
            services.AddScoped<IOdemeHareketleriService, OdemeHareketleriService>();
            services.AddScoped<ISevkiyatRepository, SevkiyatRepository>();
            services.AddScoped<ISevkiyatService, SevkiyatService>();
            services.AddScoped<ISevkiyatPaketleriRepository, SevkiyatPaketleriRepository>();
            services.AddScoped<ISevkiyatPaketleriService, SevkiyatPaketleriService>();
            services.AddScoped<IKilometreTaslariRepository, KilometreTaslariRepository>();
            services.AddScoped<IKilometreTaslariService, KilometreTaslariService>();
            services.AddScoped<IFiyatlandirmaRepository, FiyatlandirmaRepository>();
            services.AddScoped<IFiyatlandirmaService, FiyatlandirmaService>();
            services.AddScoped<IFiyatlandirmaKalemleriRepository, FiyatlandirmaKalemleriRepository>();
            services.AddScoped<IFiyatlandirmaKalemleriService, FiyatlandirmaKalemleriService>();
            services.AddScoped<IMusterilerRepository, MusterilerRepository>();
            services.AddScoped<IMusterilerService, MusterilerService>();
            services.AddScoped<IFinansProjelerRepository, FinansProjelerRepository>();
            services.AddScoped<IFinansProjelerService, FinansProjelerService>();
            services.AddScoped<IProjeKutukRepository, ProjeKutukRepository>();
            services.AddScoped<IProjeKutukService, ProjeKutukService>();
            services.AddScoped<IProjeIliskiRepository, ProjeIliskiRepository>();
            services.AddScoped<IProjeIliskiService, ProjeIliskiService>();
            services.AddScoped<ITeminatMektuplariRepository, TeminatMektuplariRepository>();
            services.AddScoped<ITeminatMektuplariService, TeminatMektuplariService>();
            services.AddScoped<ISayfaStatusRepository, SayfaStatusRepository>();
            services.AddScoped<ISayfaStatusService, SayfaStatusService>();


            services.AddScoped<IIdUreticiRepository, IdUreticiRepository>();
            services.AddScoped<IIdUreticiService, IdUreticiService>();
            services.AddScoped<IKesimDetaylariRepository, KesimDetaylariRepository>();
            services.AddScoped<IKesimDetaylariService, KesimDetaylariService>();
            services.AddScoped<IKesimListesiRepository, KesimListesiRepository>();
            services.AddScoped<IKesimListesiService, KesimListesiService>();
            services.AddScoped<IKesimListesiPaketRepository, KesimListesiPaketRepository>();
            services.AddScoped<IKesimListesiPaketService, KesimListesiPaketService>();
            services.AddScoped<IKesimTamamlanmisRepository, KesimTamamlanmisRepository>();
            services.AddScoped<IKesimTamamlanmisService, KesimTamamlanmisService>();
            services.AddScoped<IKesimTamamlanmisHareketRepository, KesimTamamlanmisHareketRepository>();
            services.AddScoped<IKesimTamamlanmisHareketService, KesimTamamlanmisHareketService>();


            services.AddScoped<IAutoCadAktarimRepository, AutoCadAktarimRepository>();
            services.AddScoped<IAutoCadAktarimService, AutoCadAktarimService>();
            services.AddScoped<IKarsilastirmaTablosuRepository, KarsilastirmaTablosuRepository>();
            services.AddScoped<IKarsilastirmaTablosuService, KarsilastirmaTablosuService>();


            services.AddScoped<IDuyurularRepository, DuyurularRepository>();
            services.AddScoped<IDuyurularService, DuyurularService>();
            services.AddScoped<IKullaniciHareketLogRepository, KullaniciHareketLogRepository>();
            services.AddScoped<IKullaniciHareketLogService, KullaniciHareketLogService>();
            services.AddScoped<IKullanicilarRepository, KullanicilarRepository>();
            services.AddScoped<IKullanicilarService, KullanicilarService>();
            services.AddScoped<ISorunBildirimleriRepository, SorunBildirimleriRepository>();
            services.AddScoped<ISorunBildirimleriService, SorunBildirimleriService>();


            services.AddScoped<IUrunGruplariRepository, UrunGruplariRepository>();
            services.AddScoped<IUrunGruplariService, UrunGruplariService>();


            services.AddSingleton<IDataBaseRepository, DataBaseRepository>();
            services.AddSingleton<IDataBaseService, DataBaseService>();
            services.AddSingleton<ITabloFiltreleRepository, TabloFiltreleRepository>();
            services.AddSingleton<ITabloFiltreleService, TabloFiltreleService>();



            services.AddScoped<frmAnaSayfa>();
            services.AddScoped<frmKullaniciGirisi>();
            services.AddScoped<frmKullaniciAyarlari>();
            services.AddScoped<frmUrunGrubuEkle>();
            return services;
        }
    }
}
