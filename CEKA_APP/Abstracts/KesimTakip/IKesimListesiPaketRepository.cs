using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CEKA_APP.Abstracts.KesimTakip
{
    public interface IKesimListesiPaketRepository
    {
        bool SaveKesimDataPaket(SqlConnection connection, SqlTransaction transaction, string olusturan, string kesimId, int kesilecekPlanSayisi, int toplamPlanTekrari, DateTime eklemeTarihi);
        bool KesimListesiPaketSil(SqlConnection connection, SqlTransaction transaction, string kesimId);
        DataTable GetKesimListesiPaket(SqlConnection connection);
        string GetKesimListesiPaketQuery();
        bool KesimListesiPaketKontrolluDusme(SqlConnection connection, SqlTransaction transaction, string kesimId, int kesilenMiktar, out string hataMesaji);
        void VerileriYenile(SqlConnection connection, DataGridView data);
        bool KesimIdVarMi(SqlConnection connection, string kesimId);
        string GetKesimListesiPaketSureQuery();
        DataTable GetKesimListesiPaketSure(SqlConnection connection);
        bool KesimListesiPaketIptalEt(SqlConnection connection, SqlTransaction transaction, string kesimId, string iptalNedeni);
    }
}
