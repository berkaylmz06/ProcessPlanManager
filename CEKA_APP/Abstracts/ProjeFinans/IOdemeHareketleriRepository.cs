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
        bool SaveOdemeHareketi(SqlConnection connection, SqlTransaction transaction, OdemeHareketleri odemeHareketi);
        List<OdemeHareketleri> GetOdemeHareketleriByOdemeId(SqlConnection connection, int odemeId);
        void DeleteOdemeHareketleriByOdemeIds(SqlConnection connection, SqlTransaction transaction, List<int> odemeIds);
    }
}
