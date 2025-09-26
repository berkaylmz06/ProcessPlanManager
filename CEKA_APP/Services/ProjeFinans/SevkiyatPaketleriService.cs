using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Services.ProjeFinans
{
    public class SevkiyatPaketleriService : ISevkiyatPaketleriService
    {
        private readonly ISevkiyatPaketleriRepository _sevkiyatPaketleriRepository;
        private readonly IDataBaseService _dataBaseService;

        public SevkiyatPaketleriService(ISevkiyatPaketleriRepository sevkiyatPaketleriRepository, IDataBaseService dataBaseService)
        {
            _sevkiyatPaketleriRepository = sevkiyatPaketleriRepository ?? throw new ArgumentNullException(nameof(sevkiyatPaketleriRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public int GetPaketIdByAdi(string paketAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    int sonuc = _sevkiyatPaketleriRepository.GetPaketIdByAdi(connection, paketAdi);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Paketler id alınırken hata oluştu.", ex);
            }
        }

        public List<(int Id, string Adi, DateTime Tarih)> GetPaketler()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _sevkiyatPaketleriRepository.GetPaketler(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Paketler alınırken hata oluştu.", ex);
            }
        }

        public int PaketEkle(string paketAdi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _sevkiyatPaketleriRepository.PaketEkle(connection, transaction, paketAdi);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Paket eklenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
