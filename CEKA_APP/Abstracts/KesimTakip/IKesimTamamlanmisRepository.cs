using System;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.KesimTakip
{
    public interface IKesimTamamlanmisRepository
    {
        int TablodanKesimTamamlanmisEkleme(SqlConnection connection, SqlTransaction transaction, string kesimYapan, string kesimId, int kesilmisPlanSayisi, string kesilenLot, int kullanilanMalzemeEn, int kullanilanMalzemeBoy);
        DataTable GetKesimListesTamamlanmis(SqlConnection connection);
        string GetKesimListesTamamlanmisQuery();
        bool YanUrunDetayEkleme(SqlConnection connection, SqlTransaction transaction, int kesimTamamlanmisId, int yanUrunEn, int yanUrunBoy, int adet);
    }
}
