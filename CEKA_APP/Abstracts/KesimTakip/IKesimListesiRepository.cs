using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.KesimTakip
{
    public interface IKesimListesiRepository
    {
        void SaveKesimData(SqlConnection connection, SqlTransaction transaction, string olusturan, string kesimId, string projeno, string malzeme, string kalite, string[] kaliplar, string[] pozlar, decimal[] adetler, DateTime eklemeTarihi);        List<KesimListesi> GetKesimListesi(SqlConnection connection);
        DataTable GetirKesimListesi(SqlConnection connection, string kesimId);
        bool KesimListesiSil(SqlConnection connection, SqlTransaction transaction, int id);
        bool KesimListesiSilByKesimId(SqlConnection connection, SqlTransaction transaction, string kesimId);
    }
}
