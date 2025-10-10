using CEKA_APP.Entitys.KesimTakip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.KesimTakip
{
    public interface IKesimSureService
    {
        int Baslat(string kesimId, string kesimYapan);
        void Durdur(int sureId, int toplamSaniye);
        void Bitir(int sureId, int toplamSaniye);
        void Delete(int sureId);
        KesimSure GetBySureId(int sureId);
        void GuncelleToplamSure(int sureId, int toplamSaniye);
        void IptalEt(int sureId, int toplamSaniye);
        DataTable GetirKesimHareketVeSure(string kesimId);
    }
}
