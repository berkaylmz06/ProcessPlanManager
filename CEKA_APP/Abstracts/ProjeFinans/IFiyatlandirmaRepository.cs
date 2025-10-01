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
        List<Fiyatlandirma> GetFiyatlandirmaByProje(SqlConnection connection, int projeId);
        bool FiyatlandirmaKaydet(SqlConnection connection, SqlTransaction transaction, Fiyatlandirma fiyat);
        bool FiyatlandirmaGuncelle(SqlConnection connection, SqlTransaction transaction, Fiyatlandirma fiyat);
        bool FiyatlandirmaSil(SqlConnection connection, SqlTransaction transaction, int projeId);
        bool FiyatlandirmaSilById(SqlConnection connection, SqlTransaction transaction, int projeId, int fiyatlandirmaKalemId);
        (decimal toplamBedel, List<int> eksikFiyatlandirmaProjeler) GetToplamBedel(SqlConnection connection, int projeId, List<int> altProjeler = null);
    }
}
