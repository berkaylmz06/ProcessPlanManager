using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IKilometreTaslariRepository
    {
        List<(int Id, string Adi, DateTime Tarih)> GetFiyatlandirmaKilometreTasi(SqlConnection connection);
        int FiyatlandirmaKilometreTasiEkle(SqlConnection connection, SqlTransaction transaction, string kilometreTasiAdi);
        int GetFiyatlandirmaKilometreTasiIdByAdi(SqlConnection connection, string kilometreTasiAdi);
        string GetKilometreTasiAdi(SqlConnection connection, int kilometreTasiId);
        int GetKilometreTasiId(SqlConnection connection, string kilometreTasiAdi);
        List<string> GetKilometreTasiAdlariByIds(SqlConnection connection, List<int> kilometreTasiIds);
    }
}
