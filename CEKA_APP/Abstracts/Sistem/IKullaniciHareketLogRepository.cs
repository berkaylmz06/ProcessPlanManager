using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.Sistem
{
    public interface IKullaniciHareketLogRepository
    {
        void LogEkle(SqlConnection connection, SqlTransaction transaction, int kullaniciId, string islemTuru, string sayfaAdi, string ekBilgi = "");
        DataTable GetKullaniciLog(SqlConnection connection);
    }
}
