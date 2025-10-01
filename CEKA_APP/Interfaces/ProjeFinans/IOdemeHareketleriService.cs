using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IOdemeHareketleriService
    {
        bool SaveOdemeHareketi(OdemeHareketleri odemeHareketi);
        List<OdemeHareketleri> GetOdemeHareketleriByOdemeId(int odemeId);
        void DeleteOdemeHareketleriByOdemeIds(List<int> odemeIds);
    }
}
