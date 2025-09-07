using CEKA_APP.Abstracts;
using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Concretes.ProjeFinans;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Interfaces;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Services.ProjeFinans;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            services.AddScoped<frmAnaSayfa>();
            services.AddScoped<frmKullaniciGirisi>();
            return services;
        }
    }
}
