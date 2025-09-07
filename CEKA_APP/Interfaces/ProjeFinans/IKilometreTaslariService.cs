using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IKilometreTaslariService
    {
        List<(int Id, string Adi, DateTime Tarih)> GetFiyatlandirmaKilometreTasi();
        int FiyatlandirmaKilometreTasiEkle(string kilometreTasiAdi);
        int GetFiyatlandirmaKilometreTasiIdByAdi(string kilometreTasiAdi);
        string GetKilometreTasiAdi(int kilometreTasiId);
        int GetKilometreTasiId(string kilometreTasiAdi);
        List<string> GetKilometreTasiAdlariByIds(List<int> kilometreTasiIds);
    }
}
