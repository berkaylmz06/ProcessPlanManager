using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IFiyatlandirmaKalemleriRepository
    {
        List<(int Id, string Adi, string Birimi, DateTime Tarih)> GetFiyatlandirmaKalemleri(SqlConnection connection);
        int FiyatlandirmaKalemleriEkle(SqlConnection connection, SqlTransaction transaction, string kalemAdi, string kalemBirimi);
        FiyatlandirmaKalem GetFiyatlandirmaKalemByAdi(SqlConnection connection, string kalemAdi);
    }
}
