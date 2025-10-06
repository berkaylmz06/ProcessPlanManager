using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Services.ProjeFinans
{
    public class ProjeKutukService : IProjeKutukService
    {
        private readonly IProjeKutukRepository _projeKutukRepository;
        private readonly IDataBaseService _dataBaseService;

        public ProjeKutukService(IProjeKutukRepository projeKutukRepository, IDataBaseService dataBaseService)
        {
            _projeKutukRepository = projeKutukRepository ?? throw new ArgumentNullException(nameof(projeKutukRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public ProjeKutuk GetProjeKutukStatus(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _projeKutukRepository.GetProjeKutukStatus(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje kütük bilgileri alınırken hata oluştu.", ex);
            }
        }

        public string GetProjeParaBirimi(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _projeKutukRepository.GetProjeParaBirimi(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje para birimi alınırken hata oluştu.", ex);
            }
        }

        public bool IsFaturalamaSekliTekil(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _projeKutukRepository.IsFaturalamaSekliTekil(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Faturalama şekli alınırken hata oluştu.", ex);
            }
        }

        public ProjeKutuk ProjeKutukAra(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _projeKutukRepository.ProjeKutukAra(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kütük bilgileri alınırken hata oluştu.", ex);
            }
        }

        public bool ProjeKutukEkle(ProjeKutuk kutuk)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.ProjeKutukEkle(connection, transaction, kutuk);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje kütük eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool ProjeKutukGuncelle(ProjeKutuk yeniKutuk)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.ProjeKutukGuncelle(connection, transaction, yeniKutuk);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje kütük güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool ProjeKutukSil(int projeId, List<int> altProjeIds)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.ProjeKutukSil(connection, transaction, projeId, altProjeIds);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje kütük silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool UpdateProjeKutukDurum(int projeId, bool? montajTamamlandiMi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.UpdateProjeKutukDurum(connection, transaction, projeId, montajTamamlandiMi);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje kütük statü güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void UpdateToplamBedel(string projeNo, decimal toplamBedel)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _projeKutukRepository.UpdateToplamBedel(connection, transaction, projeNo, toplamBedel);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Toplam bedel alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public (bool HasRelated, List<string> Details) HasRelatedRecords(int projeId, List<int> altProjeler)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _projeKutukRepository.HasRelatedRecords(connection, projeId, altProjeler);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kayıtlar alınırken hata oluştu.", ex);
            }
        }

        public bool ProjeNoKontrol(string projeNo)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _projeKutukRepository.ProjeNoKontrol(connection, projeNo);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kayıtlar alınırken hata oluştu.", ex);
            }
        }
    }
}
