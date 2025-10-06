using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CEKA_APP.Services.ProjeFinans
{
    public class OdemeSartlariService : IOdemeSartlariService
    {
        private readonly IOdemeSartlariRepository _odemeSartlariRepository;
        private readonly IDataBaseService _dataBaseService;

        public OdemeSartlariService(IOdemeSartlariRepository odemeSartlariRepository, IDataBaseService dataBaseService)
        {
            _odemeSartlariRepository = odemeSartlariRepository ?? throw new ArgumentNullException(nameof(odemeSartlariRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public void SaveOrUpdateOdemeBilgi(OdemeSartlari odemeSartlarin)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _odemeSartlariRepository.SaveOrUpdateOdemeBilgi(connection, transaction, odemeSartlarin);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ödeme bilgisi kaydı yapılırken hata oluştu.", ex);
                    }
                }
            }
        }

        public string GetFaturaNo(int projeId, int kilometreTasiId)
        {

            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    string sonuc = _odemeSartlariRepository.GetFaturaNo(connection, projeId, kilometreTasiId);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fatura no alınırken hata oluştu.", ex);
            }
        }

        public List<OdemeSartlari> GetOdemeBilgileri()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _odemeSartlariRepository.GetOdemeBilgileri(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ödeme bilgileri alınırken hata oluştu.", ex);
            }
        }

        public List<OdemeSartlari> GetOdemeBilgileriByProjeId(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _odemeSartlariRepository.GetOdemeBilgileriByProjeId(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ödeme bilgileri alınırken hata oluştu.", ex);
            }
        }

        public OdemeSartlari GetOdemeBilgi(string projeNo, int kilometreTasiId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _odemeSartlariRepository.GetOdemeBilgi(connection, projeNo, kilometreTasiId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ödeme bilgileri alınırken hata oluştu.", ex);
            }
        }

        public OdemeSartlari GetOdemeBilgiByOdemeId(int odemeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _odemeSartlariRepository.GetOdemeBilgiByOdemeId(connection, odemeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ödeme bilgileri alınırken hata oluştu.", ex);
            }
        }

        public void DeleteOdemeBilgi(int projeId, int kilometreTasiId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _odemeSartlariRepository.DeleteOdemeBilgi(connection, transaction, projeId, kilometreTasiId);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ödeme bilgisi silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool UpdateFaturaNo(int odemeId, string faturaNo)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _odemeSartlariRepository.UpdateFaturaNo(connection, transaction, odemeId, faturaNo);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Fatura güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool OdemeSartlariSil(int projeId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _odemeSartlariRepository.OdemeSartlariSil(connection, transaction, projeId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ödeme şartları silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public string GetOdemeBilgileriQuery()
        {
            return _odemeSartlariRepository.GetOdemeBilgileriQuery();
        }

        public List<OdemeSartlari> GetAllOdemeBilgileri(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _odemeSartlariRepository.GetAllOdemeBilgileri(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ödeme bilgileri alınırken hata oluştu.", ex);
            }
        }
    }
}