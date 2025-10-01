using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IFiyatlandirmaService
    {
        List<Fiyatlandirma> GetFiyatlandirmaByProje(int projeId);
        bool FiyatlandirmaKaydet(Fiyatlandirma fiyat);
        bool FiyatlandirmaGuncelle(Fiyatlandirma fiyat);
        bool FiyatlandirmaSil(int projeId);
        bool FiyatlandirmaSilById(int projeId, int fiyatlandirmaKalemId);
        (decimal toplamBedel, List<int> eksikFiyatlandirmaProjeler) GetToplamBedel(int projeId, List<int> altProjeler = null);
    }
}
