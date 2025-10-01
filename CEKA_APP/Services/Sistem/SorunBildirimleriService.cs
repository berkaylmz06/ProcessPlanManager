using CEKA_APP.Abstracts.Sistem;
using CEKA_APP.Entitys;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.Sistem;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Services.Sistem
{
    public class SorunBildirimleriService : ISorunBildirimleriService
    {
        private readonly ISorunBildirimleriRepository _sorunBildirimleriRepository;
        private readonly IDataBaseService _dataBaseService;

        public SorunBildirimleriService(ISorunBildirimleriRepository sorunBildirimleriRepository, IDataBaseService dataBaseService)
        {
            _sorunBildirimleriRepository = sorunBildirimleriRepository ?? throw new ArgumentNullException(nameof(sorunBildirimleriRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public List<SorunBildirimleri> GetSorunlar()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _sorunBildirimleriRepository.GetSorunlar(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Sorunlar alınırken hata oluştu.", ex);
            }
        }

        public bool SorunBildirimEkle(string olusturan, string sorun, DateTime sistemSaat)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _sorunBildirimleriRepository.SorunBildirimEkle(connection, transaction, olusturan, sorun, sistemSaat);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Bildirim eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void UpdateOkunduDurumu(int id)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _sorunBildirimleriRepository.UpdateOkunduDurumu(connection, transaction, id);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Okundu durumu güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
