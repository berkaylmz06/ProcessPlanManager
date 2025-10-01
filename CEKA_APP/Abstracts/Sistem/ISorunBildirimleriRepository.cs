using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.Sistem
{
    public interface ISorunBildirimleriRepository
    {
        bool SorunBildirimEkle(SqlConnection connection, SqlTransaction transaction, string olusturan, string sorun, DateTime sistemSaat);
        List<SorunBildirimleri> GetSorunlar(SqlConnection connection);
        void UpdateOkunduDurumu(SqlConnection connection, SqlTransaction transaction, int id);
    }
}
