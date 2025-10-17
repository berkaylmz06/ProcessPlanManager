using CEKA_APP.Entitys.KesimTakip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.KesimTakip
{
    public interface IKesimSureRepository
    {
        int Baslat(SqlConnection connection, SqlTransaction transaction, string kesimId, string kesimYapan, string lotNo);
        void Durdur(SqlConnection connection, SqlTransaction transaction, int sureId, int toplamSaniye);
        void Bitir(SqlConnection connection, SqlTransaction transaction, int sureId, int toplamSaniye);
        void Delete(SqlConnection connection, SqlTransaction transaction, int sureId);
        KesimSure GetBySureId(SqlConnection connection, SqlTransaction transaction, int sureId);
        void GuncelleToplamSure(SqlConnection connection, SqlTransaction transaction, int sureId, int toplamSaniye);
        void IptalEt(SqlConnection connection, SqlTransaction transaction, int sureId, int toplamSaniye);
        DataTable GetirKesimHareketVeSure(SqlConnection connection, string kesimId);
        List<(string KesimId, string LotNo, int En, int Boy, int ToplamSureSaniye, string KesimYapan)> GetirDevamEdenKesimler(SqlConnection connection, SqlTransaction transaction);
        int GetirSureId(SqlConnection connection, SqlTransaction transaction, string kesimId);
    }
}
