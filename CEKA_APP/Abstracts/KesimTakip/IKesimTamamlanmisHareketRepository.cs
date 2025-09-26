using System;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.KesimTakip
{
    public interface IKesimTamamlanmisHareketRepository
    {
        bool TablodanKesimTamamlanmisHareketEkleme(SqlConnection connection, SqlTransaction transaction, string kesimYapan, string kesimId, int kesilenAdet, DateTime kesimTarihi, TimeSpan kesimSaati);
        DataTable GetirKesimTamamlanmisHareket(SqlConnection connection, string kesimId);
    }
}
