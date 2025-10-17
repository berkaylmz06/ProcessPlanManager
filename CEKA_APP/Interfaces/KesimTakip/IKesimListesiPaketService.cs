using System;
using System.Data;
using System.Windows.Forms;

namespace CEKA_APP.Interfaces.KesimTakip
{
    public interface IKesimListesiPaketService
    {
        bool SaveKesimDataPaket(string olusturan, string kesimId, int kesilecekPlanSayisi, int toplamPlanTekrari, DateTime eklemeTarihi, int en, int boy);
        bool KesimListesiPaketSil(string kesimId);
        string GetKesimListesiPaketQuery();
        DataTable GetKesimListesiPaket();
        bool KesimListesiPaketKontrolluDusme(string kesimId, int kesilenMiktar, out string hataMesaji);
        void VerileriYenile(DataGridView data);
        bool KesimIdVarMi(string kesimId);
        string GetKesimListesiPaketSureQuery();
        DataTable GetKesimListesiPaketSure();
        bool KesimListesiPaketIptalEt(string kesimId, string iptalNedeni);
    }
}
