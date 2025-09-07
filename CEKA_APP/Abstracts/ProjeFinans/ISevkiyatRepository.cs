using CEKA_APP.Entitys.ProjeFinans;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts
{
    public interface ISevkiyatRepository
    {
        List<Sevkiyat> GetSevkiyatByProje(int projeId);
        void SevkiyatKaydet(Sevkiyat sevkiyat, SqlTransaction transaction);
        void SevkiyatGuncelle(Sevkiyat sevkiyat, SqlTransaction transaction);
        bool SevkiyatSilBySevkiyatId(int projeId, string sevkiyatId, int aracSira, SqlTransaction transaction);
        bool SevkiyatSil(int projeId, SqlTransaction transaction);
    }
}