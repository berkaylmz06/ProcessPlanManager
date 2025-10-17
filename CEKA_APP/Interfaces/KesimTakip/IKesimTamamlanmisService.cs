using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CEKA_APP.Interfaces.KesimTakip
{
    public interface IKesimTamamlanmisService
    {
        int TablodanKesimTamamlanmisEkleme(string kesimYapan, string kesimId, int kesilmisPlanSayisi, string kesilenLot, int kullanilanMalzemeEn, int kullanilanMalzemeBoy);
        DataTable GetKesimListesTamamlanmis();
        string GetKesimListesTamamlanmisQuery();
        bool YanUrunDetayEkleme(int kesimTamamlanmisId, int yanUrunEn, int yanUrunBoy, int adet);
    }
}
