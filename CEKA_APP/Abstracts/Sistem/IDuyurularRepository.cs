using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.Sistem
{
    public interface IDuyurularRepository
    {
        bool DuyuruEkle(string olusturan, string duyuru, DateTime sistemSaat, SqlConnection connection, SqlTransaction transaction);
        Duyurular GetSonDuyuru(SqlConnection connection);
    }
}
