using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.KesimTakip
{
    public interface IKesimTamamlanmisHareketService
    {
        bool TablodanKesimTamamlanmisHareketEkleme(string kesimYapan, string kesimId, int kesilenAdet, DateTime kesimTarihi, TimeSpan kesimSaati);
        DataTable GetirKesimTamamlanmisHareket(string kesimId);
    }
}
