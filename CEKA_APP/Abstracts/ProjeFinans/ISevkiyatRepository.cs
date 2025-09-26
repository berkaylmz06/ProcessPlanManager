using CEKA_APP.Entitys.ProjeFinans;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts
{
    public interface ISevkiyatRepository
    {
        List<Sevkiyat> GetSevkiyatByProje(SqlConnection connection, int projeId);
        int SevkiyatKaydet(SqlConnection connection, SqlTransaction transaction, Sevkiyat sevkiyat);
        void SevkiyatGuncelle(SqlConnection connection, SqlTransaction transaction, Sevkiyat sevkiyat);
        bool SevkiyatSilBySevkiyatId(SqlConnection connection, SqlTransaction transaction, int projeId, int sevkiyatId, int aracSira);
        bool SevkiyatSil(SqlConnection connection, SqlTransaction transaction, int projeId);
    }
}