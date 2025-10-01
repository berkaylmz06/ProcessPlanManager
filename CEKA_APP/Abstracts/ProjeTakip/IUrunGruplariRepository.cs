using CEKA_APP.Entitys.ProjeTakip;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.ProjeTakip
{
    public interface IUrunGruplariRepository
    {
        bool UrunGrubuEkle(SqlConnection connection, SqlTransaction transaction, string urunGrubu, string urunGrubuAdi);
        bool UrunGrubuGuncelle(SqlConnection connection, SqlTransaction transaction, int urunGrubuId, string urunGrubu, string urunGrubuAdi, out bool degisiklikVar);
        bool UrunGrubuSil(SqlConnection connection, SqlTransaction transaction, int urunGrubuId);
        string GetUrunGrubuBilgileriQuery();
        List<UrunGruplari> GetUrunGrubuBilgileri(SqlConnection connection, SqlTransaction transaction);
    }
}
