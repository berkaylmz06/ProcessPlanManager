using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.Sistem
{
    public interface ISorunBildirimleriService
    {
        bool SorunBildirimEkle(string olusturan, string sorun, DateTime sistemSaat);
        List<SorunBildirimleri> GetSorunlar();
        void UpdateOkunduDurumu(int id);
    }
}
