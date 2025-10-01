using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Concretes.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.Genel;
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
        private readonly IDataBaseService _dataBaseService;

        public KilometreTaslariService(IKilometreTaslariRepository kilometreTaslariRepository, IDataBaseService dataBaseService)
        {
            _kilometreTaslariRepository = kilometreTaslariRepository ?? throw new ArgumentNullException(nameof(kilometreTaslariRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public int FiyatlandirmaKilometreTasiEkle(string kilometreTasiAdi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _kilometreTaslariRepository.FiyatlandirmaKilometreTasiEkle(connection, transaction, kilometreTasiAdi);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kilometre taşı eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public List<(int Id, string Adi, DateTime Tarih)> GetFiyatlandirmaKilometreTasi()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kilometreTaslariRepository.GetFiyatlandirmaKilometreTasi(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kilometre taşları alınırken hata oluştu.", ex);
            }
        }

        public int GetFiyatlandirmaKilometreTasiIdByAdi(string kilometreTasiAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kilometreTaslariRepository.GetFiyatlandirmaKilometreTasiIdByAdi(connection, kilometreTasiAdi);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kilometre taşları alınırken hata oluştu.", ex);
            }
        }

        public string GetKilometreTasiAdi(int kilometreTasiId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kilometreTaslariRepository.GetKilometreTasiAdi(connection, kilometreTasiId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kilometre taş adı alınırken hata oluştu.", ex);
            }
        }

        public List<string> GetKilometreTasiAdlariByIds(List<int> kilometreTasiIds)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kilometreTaslariRepository.GetKilometreTasiAdlariByIds(connection, kilometreTasiIds);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kilometre taşı adları alınırken hata oluştu.", ex);
            }
        }

        public int GetKilometreTasiId(string kilometreTasiAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    int sonuc = _kilometreTaslariRepository.GetKilometreTasiId(connection, kilometreTasiAdi);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kilometre taşı id alınırken hata oluştu.", ex);
            }
        }
    }
}
