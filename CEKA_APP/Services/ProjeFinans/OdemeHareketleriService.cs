using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Services.ProjeFinans
{
    public class OdemeHareketleriService : IOdemeHareketleriService
    {
        private readonly IOdemeHareketleriRepository _odemeHareketleriRepository;
        private readonly IDataBaseService _dataBaseService;

        public OdemeHareketleriService(IOdemeHareketleriRepository odemeHareketleriRepository, IDataBaseService dataBaseService)
        {
            _odemeHareketleriRepository = odemeHareketleriRepository ?? throw new ArgumentNullException(nameof(odemeHareketleriRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public void DeleteOdemeHareketleriByOdemeIds(List<int> odemeIds)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                         _odemeHareketleriRepository.DeleteOdemeHareketleriByOdemeIds(connection, transaction, odemeIds);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ödeme hareketleri silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public List<OdemeHareketleri> GetOdemeHareketleriByOdemeId(int odemeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _odemeHareketleriRepository.GetOdemeHareketleriByOdemeId(connection, odemeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ödeme hareketleri alınırken hata oluştu.", ex);
            }
        }

        public bool SaveOdemeHareketi(OdemeHareketleri odemeHareketi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _odemeHareketleriRepository.SaveOdemeHareketi(connection, transaction, odemeHareketi);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ödeme hareketleri kaydedilirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
