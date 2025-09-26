using CEKA_APP.Abstracts.ERP;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.Genel;
using System;
using System.Data;

namespace CEKA_APP.Services.ERP
{
    public class KarsilastirmaTablosuService : IKarsilastirmaTablosuService
    {
        private readonly IKarsilastirmaTablosuRepository _karsilastirmaTablosuRepository;
        private readonly IDataBaseService _dataBaseService;

        public KarsilastirmaTablosuService(IKarsilastirmaTablosuRepository karsilastirmaTablosuRepository, IDataBaseService dataBaseService)
        {
            _karsilastirmaTablosuRepository = karsilastirmaTablosuRepository ?? throw new ArgumentNullException(nameof(karsilastirmaTablosuRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public DataTable GetAllKaliteKarsilastirmalari()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _karsilastirmaTablosuRepository.GetAllKaliteKarsilastirmalari(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kalite karşılaştırmaları alınırken hata oluştu.", ex);
            }
        }

        public DataTable GetAllKesimKarsilastirmalari()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _karsilastirmaTablosuRepository.GetAllKesimKarsilastirmalari(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kesim karşılaştırmaları alınırken hata oluştu.", ex);
            }
        }

        public DataTable GetAllMalzemeKarsilastirmalari()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _karsilastirmaTablosuRepository.GetAllMalzemeKarsilastirmalari(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Malzeme karşılaştırmaları alınırken hata oluştu.", ex);
            }
        }

        public string GetAutoCadCodeByIfsCodeKalite(string cekaCode)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _karsilastirmaTablosuRepository.GetAutoCadCodeByIfsCodeKalite(connection,cekaCode);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("AutoCad kodu alınırken hata oluştu.", ex);
            }
        }

        public string GetIfsCodeByAutoCadCodeKalite(string cekaCode)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _karsilastirmaTablosuRepository.GetIfsCodeByAutoCadCodeKalite(connection,cekaCode);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ifs kodu alınırken hata oluştu.", ex);
            }
        }

        public string GetIfsCodeByAutoCadCodeKesim(string kesimCode, out string hataMesaji)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _karsilastirmaTablosuRepository.GetIfsCodeByAutoCadCodeKesim(connection, kesimCode,out hataMesaji);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ifs kodu alınırken hata oluştu.", ex);
            }
        }

        public string GetIfsCodeByAutoCadCodeMalzeme(string autoCadCode)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _karsilastirmaTablosuRepository.GetIfsCodeByAutoCadCodeMalzeme(connection, autoCadCode);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ifs kodu alınırken hata oluştu.", ex);
            }
        }

        public string GetIfsCodeByKesimCode(string KesimCode)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _karsilastirmaTablosuRepository.GetIfsCodeByKesimCode(connection, KesimCode);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ifs kodu alınırken hata oluştu.", ex);
            }
        }

        public void SaveKarsilastirmaKalite(string cekaCode, string ifsCode, string aciklama = null)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _karsilastirmaTablosuRepository.SaveKarsilastirmaKalite(connection, transaction, cekaCode, ifsCode, aciklama);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kalite kodu eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void SaveKarsilastirmaKesim(string kesimCode, string ifsCode, string aciklama = null)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _karsilastirmaTablosuRepository.SaveKarsilastirmaKesim(connection, transaction, kesimCode, kesimCode, aciklama);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim kodu eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void SaveKarsilastirmaMalzeme(string autoCadCode, string ifsCode, string aciklama = null)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _karsilastirmaTablosuRepository.SaveKarsilastirmaMalzeme(connection, transaction, autoCadCode, ifsCode,aciklama);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Malzeme kodu eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void SilKarsilastirmaKaydi(string tableName, int id)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _karsilastirmaTablosuRepository.SilKarsilastirmaKaydi(connection, transaction, tableName, id);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Karşılaştırma kaydı silinirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
