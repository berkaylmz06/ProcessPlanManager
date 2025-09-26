using CEKA_APP.Entitys;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.Genel
{
    public interface IKesimDetaylariRepository
    {
        void SaveKesimDetaylariData(SqlConnection connection, SqlTransaction transaction, string kalite, string malzeme, string malzemeKod, string proje, int kesilecekAdet, int toplamAdet, bool ekBilgi);
        DataTable GetKesimDetaylariBilgi(SqlConnection connection);
        bool PozExists(SqlConnection connection, string kalite, string malzeme, string malzemekod, string proje);

        bool UpdateKesilmisAdet(SqlConnection connection, SqlTransaction transaction, string kalite, string malzeme, string malzemekod, string proje, decimal sondurum);
        List<KesimDetaylari> GetKesimDetaylariBilgileri(SqlConnection connection, string projeAdi = null, string grupAdi = null);
        bool GuncelleKesimDetaylari(SqlConnection connection, SqlTransaction transaction, string kalite, string malzeme, string kalipNo, string kesilecekPozlar, string proje, decimal silinecekAdet = 0, bool tamSilme = false);
        (decimal toplamAdet, decimal kesilmisAdet, decimal kesilecekAdet, List<string> eslesenPozlar) GetAdetlerVeEslesenPozlar(SqlConnection connection, string kalite, string malzeme, string proje, string malzemeKodIlkKisim, string malzemeKodUcuncuKisim);
    }
}
