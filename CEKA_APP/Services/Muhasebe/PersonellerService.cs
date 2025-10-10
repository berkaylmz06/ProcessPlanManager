using CEKA_APP.Abstracts.Muhasebe;
using CEKA_APP.Entitys.Muhasebe;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.Muhasebe;
using System;
using System.Collections.Generic;
using System.Data;

namespace CEKA_APP.Services.Muhasebe
{
    public class PersonellerService : IPersonellerService
    {
        private readonly IPersonellerRepository _personellerRepository;
        private readonly IDataBaseService _dataBaseService;
        public PersonellerService(IPersonellerRepository personellerRepo, IDataBaseService dataBaseService)
        {
            _personellerRepository = personellerRepo ?? throw new ArgumentNullException(nameof(personellerRepo));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public DataTable GetAllPersonel()
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DataTable sonuc = _personellerRepository.GetAllPersonel(connection, transaction);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Personeller alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public List<Personeller> GetPersonelOperator()
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        List<Personeller> sonuc = _personellerRepository.GetPersonelOperator(connection, transaction);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Operatörler alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public void PersonelEkle(string adSoyad, string kullaniciAdi, string departman, string telefon, bool aktif)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _personellerRepository.PersonelEkle(connection, transaction, adSoyad, kullaniciAdi, departman, telefon, aktif);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Personel eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void PersonelSil(int personelId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _personellerRepository.PersonelSil(connection, transaction, personelId);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Personel silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void UpdatePersonel(int personelId, string adSoyad, string kullaniciAdi, string departman, string telefon, bool aktif)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _personellerRepository.UpdatePersonel(connection, transaction, personelId, adSoyad, kullaniciAdi, departman, telefon, aktif);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Personel güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
