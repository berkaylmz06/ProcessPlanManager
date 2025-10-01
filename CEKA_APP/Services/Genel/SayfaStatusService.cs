using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Entitys.Genel;
using CEKA_APP.Interfaces.Genel;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Services.Genel
{
    public class SayfaStatusService : ISayfaStatusService
    {
        private readonly ISayfaStatusRepository _sayfaStatusRepository;
        private readonly IDataBaseService _dataBaseService;

        public SayfaStatusService(ISayfaStatusRepository sayfaStatusRepository, IDataBaseService dataBaseService)
        {
            _sayfaStatusRepository = sayfaStatusRepository ?? throw new ArgumentNullException(nameof(sayfaStatusRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public void Delete(int sayfaStatusId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _sayfaStatusRepository.Delete(connection, transaction, sayfaStatusId);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Sayfa statü silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public SayfaStatus Get(int sayfaId, int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _sayfaStatusRepository.Get(connection, sayfaId, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Sayfa statü alınırken hata oluştu.", ex);
            }
        }

        public List<string> GetNedenTamamlanmadiByProjeId(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _sayfaStatusRepository.GetNedenTamamlanmadiByProjeId(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Neden tamamlanmadı bilgileri alınırken hata oluştu.", ex);
            }
        }

        public int Insert(SayfaStatus status)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _sayfaStatusRepository.Insert(connection, transaction, status);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Statü eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool IsAllAltProjelerSayfa4Kapali(int projeId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _sayfaStatusRepository.IsAllAltProjelerSayfa4Kapali(connection, transaction, projeId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Sevkiyat proje statüleri alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool IsSayfa3Kapali(int projeId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _sayfaStatusRepository.IsSayfa3Kapali(connection, transaction, projeId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ödeme şartları proje statüleri alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public void Update(SayfaStatus status)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _sayfaStatusRepository.Update(connection, transaction, status);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Statü güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
