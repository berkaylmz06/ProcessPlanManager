using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Concretes.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Services.ProjeFinans
{
    public class KilometreTaslariService : IKilometreTaslariService
    {
        private readonly IKilometreTaslariRepository _kilometreTaslariRepository;

        public KilometreTaslariService(IKilometreTaslariRepository kilometreTaslariRepository)
        {
            _kilometreTaslariRepository = kilometreTaslariRepository ?? throw new ArgumentNullException(nameof(kilometreTaslariRepository));
        }
        public int FiyatlandirmaKilometreTasiEkle(string kilometreTasiAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _kilometreTaslariRepository.FiyatlandirmaKilometreTasiEkle(kilometreTasiAdi, transaction);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"SQL Hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }
            }
        }

        public List<(int Id, string Adi, DateTime Tarih)> GetFiyatlandirmaKilometreTasi()
        {
            return _kilometreTaslariRepository.GetFiyatlandirmaKilometreTasi();
        }

        public int GetFiyatlandirmaKilometreTasiIdByAdi(string kilometreTasiAdi)
        {
            return _kilometreTaslariRepository.GetFiyatlandirmaKilometreTasiIdByAdi(kilometreTasiAdi);
        }

        public string GetKilometreTasiAdi(int kilometreTasiId)
        {
            return _kilometreTaslariRepository.GetKilometreTasiAdi(kilometreTasiId);
        }

        public List<string> GetKilometreTasiAdlariByIds(List<int> kilometreTasiIds)
        {
            return _kilometreTaslariRepository.GetKilometreTasiAdlariByIds(kilometreTasiIds);
        }

        public int GetKilometreTasiId(string kilometreTasiAdi)
        {
            return _kilometreTaslariRepository.GetKilometreTasiId(kilometreTasiAdi);
        }
    }
}
