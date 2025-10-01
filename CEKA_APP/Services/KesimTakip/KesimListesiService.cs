using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Entitys;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using System;
using System.Collections.Generic;
using System.Data;

namespace CEKA_APP.Services.KesimTakip
{
    public class KesimListesiService : IKesimListesiService
    {
        private readonly IKesimListesiRepository _kesimListesiRepository;
        private readonly IDataBaseService _dataBaseService;

        public KesimListesiService(IKesimListesiRepository kesimListesiRepository, IDataBaseService dataBaseService)
        {
            _kesimListesiRepository = kesimListesiRepository ?? throw new ArgumentNullException(nameof(kesimListesiRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public DataTable GetirKesimListesi(string kesimId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimListesiRepository.GetirKesimListesi(connection, kesimId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kesim listesi alınırken hata oluştu.", ex);
            }
        }

        public List<KesimListesi> GetKesimListesi()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimListesiRepository.GetKesimListesi(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kesim listesi alınırken hata oluştu.", ex);
            }
        }

        public bool KesimListesiSil(int id)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimListesiRepository.KesimListesiSil(connection, transaction, id);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim listesi silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool KesimListesiSilByKesimId(string kesimId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimListesiRepository.KesimListesiSilByKesimId(connection, transaction, kesimId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim listesi silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void SaveKesimData(string olusturan, string kesimId, string projeno, string malzeme, string kalite, string[] kaliplar, string[] pozlar, decimal[] adetler, DateTime eklemeTarihi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _kesimListesiRepository.SaveKesimData(connection, transaction, olusturan, kesimId, projeno, malzeme, kalite, kaliplar, pozlar, adetler, eklemeTarihi);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim eklenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
