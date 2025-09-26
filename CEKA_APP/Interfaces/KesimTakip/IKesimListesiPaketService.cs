using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CEKA_APP.Interfaces.KesimTakip
{
    public interface IKesimListesiPaketService
    {
        bool SaveKesimDataPaket(string olusturan, string kesimId, int kesilecekPlanSayisi, int toplamPlanTekrari, DateTime eklemeTarihi);
        bool KesimListesiPaketSil(string kesimId);
        string GetKesimListesiPaketQuery();
        DataTable GetKesimListesiPaket();
        bool KesimListesiPaketKontrolluDusme(string kesimId, int kesilenMiktar, out string hataMesaji);
        void VerileriYenile(DataGridView data);
        bool KesimIdVarMi(string kesimId);
    }
}
