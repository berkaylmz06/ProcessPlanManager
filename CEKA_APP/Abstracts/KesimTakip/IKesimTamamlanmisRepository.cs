using System;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.KesimTakip
{
    public interface IKesimTamamlanmisRepository
    {
        bool TablodanKesimTamamlanmisEkleme(SqlConnection connection, SqlTransaction transaction, string kesimYapan, string kesimId, int kesilmisPlanSayisi, DateTime kesimTarihi, TimeSpan kesimSaati, string kesilenLot);
        DataTable GetKesimListesTamamlanmis(SqlConnection connection);
        string GetKesimListesTamamlanmisQuery();
    }
}
