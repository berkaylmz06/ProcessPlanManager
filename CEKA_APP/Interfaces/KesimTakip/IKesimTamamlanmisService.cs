using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CEKA_APP.Interfaces.KesimTakip
{
    public interface IKesimTamamlanmisService
    {
        bool TablodanKesimTamamlanmisEkleme(string kesimYapan, string kesimId, int kesilmisPlanSayisi, DateTime kesimTarihi, TimeSpan kesimSaati, string kesilenLot);
        DataTable GetKesimListesTamamlanmis();
        string GetKesimListesTamamlanmisQuery();
    }
}
