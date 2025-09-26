using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.ERP
{
    public interface IKarsilastirmaTablosuRepository
    {
        string GetIfsCodeByAutoCadCodeKalite(SqlConnection connection, string cekaCode);
        string GetAutoCadCodeByIfsCodeKalite(SqlConnection connection, string ifsCode);
        string GetIfsCodeByKesimCode(SqlConnection connection, string KesimCode);
        void SaveKarsilastirmaKalite(SqlConnection connection, SqlTransaction transaction, string cekaCode, string ifsCode, string aciklama = null);
        string GetIfsCodeByAutoCadCodeMalzeme(SqlConnection connection, string autoCadCode);
        void SaveKarsilastirmaMalzeme(SqlConnection connection, SqlTransaction transaction, string autoCadCode, string ifsCode, string aciklama = null);
        string GetIfsCodeByAutoCadCodeKesim(SqlConnection connection, string kesimCode, out string hataMesaji);
        void SaveKarsilastirmaKesim(SqlConnection connection, SqlTransaction transaction, string kesimCode, string ifsCode, string aciklama = null);
        DataTable GetAllKaliteKarsilastirmalari(SqlConnection connection);
        DataTable GetAllMalzemeKarsilastirmalari(SqlConnection connection);
        DataTable GetAllKesimKarsilastirmalari(SqlConnection connection);
        void SilKarsilastirmaKaydi(SqlConnection connection, SqlTransaction transaction, string tableName, int id);
    }
}
