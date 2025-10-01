using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface ISevkiyatPaketleriService
    {
        List<(int Id, string Adi, DateTime Tarih)> GetPaketler();
        int PaketEkle(string paketAdi);
        int GetPaketIdByAdi(string paketAdi);
    }
}
