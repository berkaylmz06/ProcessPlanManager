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

namespace CEKA_APP.Services.ProjeFinans
{
    public class FiyatlandirmaKalemleriService : IFiyatlandirmaKalemleriService
    {
        private readonly IFiyatlandirmaKalemleriRepository _fiyatlandirmaKalemleriRepository;
        private readonly IDataBaseService _dataBaseService;

        public FiyatlandirmaKalemleriService(IFiyatlandirmaKalemleriRepository fiyatlandirmaKalemleriRepository, IDataBaseService dataBaseService)
        {
            _fiyatlandirmaKalemleriRepository = fiyatlandirmaKalemleriRepository ?? throw new ArgumentNullException(nameof(fiyatlandirmaKalemleriRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public int FiyatlandirmaKalemleriEkle(string kalemAdi, string kalemBirimi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _fiyatlandirmaKalemleriRepository.FiyatlandirmaKalemleriEkle(connection, transaction, kalemAdi, kalemBirimi);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Fiyatlandırma kalemi eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public FiyatlandirmaKalem GetFiyatlandirmaKalemByAdi(string kalemAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _fiyatlandirmaKalemleriRepository.GetFiyatlandirmaKalemByAdi(connection, kalemAdi);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fiyatlandırma kalemleri alınırken hata oluştu.", ex);
            }
        }

        public List<(int Id, string Adi, string Birimi, DateTime Tarih)> GetFiyatlandirmaKalemleri()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _fiyatlandirmaKalemleriRepository.GetFiyatlandirmaKalemleri(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fiyatlandırma kalemleri alınırken hata oluştu.", ex);
            }
        }
    }
}
