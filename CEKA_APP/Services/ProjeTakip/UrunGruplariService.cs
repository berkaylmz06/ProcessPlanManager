using CEKA_APP.Abstracts.ProjeTakip;
using CEKA_APP.Entitys.ProjeTakip;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeTakip;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Services.ProjeTakip
{
    public class UrunGruplariService : IUrunGruplariService
    {
        private readonly IUrunGruplariRepository _urunGruplariRepository;
        private readonly IDataBaseService _dataBaseService;

        public UrunGruplariService(IUrunGruplariRepository urunGruplariRepository, IDataBaseService dataBaseService)
        {
            _urunGruplariRepository = urunGruplariRepository ?? throw new ArgumentNullException(nameof(urunGruplariRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public List<UrunGruplari> GetUrunGrubuBilgileri()
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        List<UrunGruplari> sonuc = _urunGruplariRepository.GetUrunGrubuBilgileri(connection, transaction);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ürün grupları alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public string GetUrunGrubuBilgileriQuery()
        {
            return _urunGruplariRepository.GetUrunGrubuBilgileriQuery();
        }

        public bool UrunGrubuEkle(string urunGrubu, string urunGrubuAdi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _urunGruplariRepository.UrunGrubuEkle(connection, transaction, urunGrubu, urunGrubuAdi);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ürün grupları kaydedilirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool UrunGrubuGuncelle(int urunGrubuId, string urunGrubu, string urunGrubuAdi, out bool degisiklikVar)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _urunGruplariRepository.UrunGrubuGuncelle(connection, transaction, urunGrubuId, urunGrubu, urunGrubuAdi, out degisiklikVar);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ürün grupları güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool UrunGrubuSil(int urunGrubuId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _urunGruplariRepository.UrunGrubuSil(connection, transaction, urunGrubuId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Ürün grubu silinirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
