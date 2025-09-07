using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface ISevkiyatPaketleriRepository
    {
        List<(int Id, string Adi, DateTime Tarih)> GetPaketler();
        int PaketEkle(string paketAdi, SqlTransaction transaction);
        int GetPaketIdByAdi(string paketAdi);
    }
}
