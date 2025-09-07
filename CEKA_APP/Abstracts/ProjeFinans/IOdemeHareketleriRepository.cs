using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IOdemeHareketleriRepository
    {
        bool SaveOdemeHareketi(OdemeHareketleri odemeHareketi, SqlTransaction transaction);
        List<OdemeHareketleri> GetOdemeHareketleriByOdemeId(int odemeId);
        void DeleteOdemeHareketleriByOdemeIds(List<int> odemeIds, SqlTransaction transaction);
    }
}
