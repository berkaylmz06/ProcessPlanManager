using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.Sistem
{
    public interface IKullaniciHareketLogService
    {
        void LogEkle(int kullaniciId, string islemTuru, string sayfaAdi, string ekBilgi = "");
        DataTable GetKullaniciLog();
    }
}
