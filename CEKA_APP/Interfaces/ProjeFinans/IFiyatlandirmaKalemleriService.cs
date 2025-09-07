using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IFiyatlandirmaKalemleriService
    {
        List<(int Id, string Adi, string Birimi, DateTime Tarih)> GetFiyatlandirmaKalemleri();
        int FiyatlandirmaKalemleriEkle(string kalemAdi, string kalemBirimi);
        FiyatlandirmaKalem GetFiyatlandirmaKalemByAdi(string kalemAdi);
    }
}
