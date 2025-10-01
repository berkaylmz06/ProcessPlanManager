using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Services.ProjeFinans
{
    public class FiyatlandirmaService : IFiyatlandirmaService
    {
        private readonly IFiyatlandirmaRepository _fiyalandirmaRepository;
        private readonly IDataBaseService _dataBaseService;

        public FiyatlandirmaService(IFiyatlandirmaRepository fiyalandirmaRepository, IDataBaseService dataBaseService)
        {
            _fiyalandirmaRepository = fiyalandirmaRepository ?? throw new ArgumentNullException(nameof(fiyalandirmaRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public bool FiyatlandirmaGuncelle(Fiyatlandirma fiyat)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _fiyalandirmaRepository.FiyatlandirmaGuncelle(connection, transaction, fiyat);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Fiyatlandırma güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool FiyatlandirmaKaydet(Fiyatlandirma fiyat)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _fiyalandirmaRepository.FiyatlandirmaKaydet(connection, transaction, fiyat);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Fiyatlandırma kaydedilirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool FiyatlandirmaSil(int projeId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _fiyalandirmaRepository.FiyatlandirmaSil(connection, transaction, projeId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Fiyatlandırma silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool FiyatlandirmaSilById(int projeId, int fiyatlandirmaKalemId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _fiyalandirmaRepository.FiyatlandirmaSilById(connection, transaction, projeId, fiyatlandirmaKalemId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Fiyatlandırma silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public List<Fiyatlandirma> GetFiyatlandirmaByProje(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _fiyalandirmaRepository.GetFiyatlandirmaByProje(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fiyatlandırma alınırken hata oluştu.", ex);
            }
        }

        public (decimal toplamBedel, List<int> eksikFiyatlandirmaProjeler) GetToplamBedel(int projeId, List<int> altProjeler = null)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _fiyalandirmaRepository.GetToplamBedel(connection, projeId, altProjeler);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Toplam bedel alınırken hata oluştu.", ex);
            }
        }
    }
}
