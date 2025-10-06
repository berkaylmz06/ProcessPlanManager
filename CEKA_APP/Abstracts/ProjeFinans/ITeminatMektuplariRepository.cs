using CEKA_APP.Entitys.ProjeFinans;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface ITeminatMektuplariRepository
    {
        bool TeminatMektubuKaydet(SqlConnection connection, SqlTransaction transaction, TeminatMektuplari mektup);
        void MektupGuncelle(SqlConnection connection, SqlTransaction transaction, string eskiMektupNo, TeminatMektuplari guncelMektup);
        void MektupSil(SqlConnection connection, SqlTransaction transaction, string mektupNo);
        bool MektupNoVarMi(SqlConnection connection, string mektupNo);
        List<TeminatMektuplari> GetTeminatMektuplari(SqlConnection connection);
        string GetTeminatMektuplariQuery();
        void UpdateKilometreTasiAdi(SqlConnection connection, SqlTransaction transaction, string mektupNo, int kilometreTasiId);
    }
}
