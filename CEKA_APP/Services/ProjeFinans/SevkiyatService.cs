using CEKA_APP.Abstracts;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces;
using CEKA_APP.Interfaces.Genel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class SevkiyatService : ISevkiyatService
    {
        private readonly ISevkiyatRepository _sevkiyatRepository;
        private readonly IDataBaseService _dataBaseService;

        public SevkiyatService(ISevkiyatRepository sevkiyatRepository, IDataBaseService dataBaseService)
        {
            _sevkiyatRepository = sevkiyatRepository ?? throw new ArgumentNullException(nameof(sevkiyatRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public List<Sevkiyat> GetSevkiyatByProje(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _sevkiyatRepository.GetSevkiyatByProje(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Sevkiyatlar alınırken hata oluştu.", ex);
            }
        }

        public int SevkiyatKaydet(Sevkiyat sevkiyat)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int  sonuc = _sevkiyatRepository.SevkiyatKaydet(connection, transaction, sevkiyat);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Sevkiyat kaydedilirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void SevkiyatGuncelle(Sevkiyat sevkiyat)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _sevkiyatRepository.SevkiyatGuncelle(connection, transaction, sevkiyat);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Sevkiyat güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool SevkiyatSilBySevkiyatId(int projeId, int sevkiyatId, int aracSira)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _sevkiyatRepository.SevkiyatSilBySevkiyatId(connection, transaction, projeId, sevkiyatId, aracSira);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Sevkiyat silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool SevkiyatSil(int projeId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _sevkiyatRepository.SevkiyatSil(connection, transaction, projeId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Sevkiyat silinirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}