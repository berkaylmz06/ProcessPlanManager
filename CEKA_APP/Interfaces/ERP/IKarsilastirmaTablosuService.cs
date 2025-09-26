using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.ERP
{
    public interface IKarsilastirmaTablosuService
    {
        string GetIfsCodeByAutoCadCodeKalite(string cekaCode);
        string GetAutoCadCodeByIfsCodeKalite(string cekaCode);
        string GetIfsCodeByKesimCode(string KesimCode);
        void SaveKarsilastirmaKalite(string cekaCode, string ifsCode, string aciklama = null);
        string GetIfsCodeByAutoCadCodeMalzeme(string autoCadCode);
        void SaveKarsilastirmaMalzeme(string autoCadCode, string ifsCode, string aciklama = null);
        string GetIfsCodeByAutoCadCodeKesim(string kesimCode, out string hataMesaji);
        void SaveKarsilastirmaKesim(string kesimCode, string ifsCode, string aciklama = null);
        DataTable GetAllKaliteKarsilastirmalari();
        DataTable GetAllMalzemeKarsilastirmalari();
        DataTable GetAllKesimKarsilastirmalari();
        void SilKarsilastirmaKaydi(string tableName, int id);
    }
}
