using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data;

namespace CEKA_APP.Interfaces.KesimTakip
{
    public interface IKesimListesiService
    {
        void SaveKesimData(string olusturan, string kesimId, string projeno, string malzeme, string kalite, string[] kaliplar, string[] pozlar, decimal[] adetler, DateTime eklemeTarihi);
        List<KesimListesi> GetKesimListesi();
        DataTable GetirKesimListesi(string kesimId);
        bool KesimListesiSil(int id);
        bool KesimListesiSilByKesimId(string kesimId);
    }
}
