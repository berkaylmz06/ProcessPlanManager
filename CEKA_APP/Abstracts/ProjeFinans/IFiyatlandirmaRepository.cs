using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IFiyatlandirmaRepository
    {
        List<Fiyatlandirma> GetFiyatlandirmaByProje(int projeId);
        bool FiyatlandirmaKaydet(Fiyatlandirma fiyat, SqlTransaction transaction);
        bool FiyatlandirmaGuncelle(Fiyatlandirma fiyat, SqlTransaction transaction);
        bool FiyatlandirmaSil(int projeId, SqlTransaction transaction);
        bool FiyatlandirmaSilById(int projeId, int fiyatlandirmaKalemId, SqlTransaction transaction);
        (decimal toplamBedel, List<int> eksikFiyatlandirmaProjeler) GetToplamBedel(int projeId, List<int> altProjeler = null);
    }
}
