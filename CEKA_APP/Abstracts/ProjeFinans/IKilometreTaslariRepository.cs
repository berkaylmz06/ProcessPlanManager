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
        List<(int Id, string Adi, DateTime Tarih)> GetFiyatlandirmaKilometreTasi();
        int FiyatlandirmaKilometreTasiEkle(string kilometreTasiAdi, SqlTransaction transaction);
        int GetFiyatlandirmaKilometreTasiIdByAdi(string kilometreTasiAdi);
        string GetKilometreTasiAdi(int kilometreTasiId);
        int GetKilometreTasiId(string kilometreTasiAdi);
        List<string> GetKilometreTasiAdlariByIds(List<int> kilometreTasiIds);

    }
}
