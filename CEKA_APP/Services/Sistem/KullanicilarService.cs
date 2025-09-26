using CEKA_APP.Abstracts.Sistem;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.Sistem;
using System;
using System.Data;

namespace CEKA_APP.Services.Sistem
{
    public class KullanicilarService : IKullanicilarService
    {
        private readonly IKullanicilarRepository _kullanicilarRepository;
        private readonly IDataBaseService _dataBaseService;

        public KullanicilarService(IKullanicilarRepository kullanicilarRepository, IDataBaseService dataBaseService)
        {
            _kullanicilarRepository = kullanicilarRepository ?? throw new ArgumentNullException(nameof(kullanicilarRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public int GetKullaniciIdByKullaniciAdi(string kullaniciAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    int sonuc = _kullanicilarRepository.GetKullaniciIdByKullaniciAdi(connection,kullaniciAdi);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kullanıcı Id alınırken hata oluştu.", ex);
            }
        }

        public DataTable GetKullaniciListesi()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    DataTable sonuc = _kullanicilarRepository.GetKullaniciListesi(connection);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kullanıcı listesi alınırken hata oluştu.", ex);
            }
        }

        public Kullanicilar GirisYap(string kullaniciAdi, string sifre)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Kullanicilar sonuc = _kullanicilarRepository.GirisYap(connection, transaction, kullaniciAdi, sifre);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Giriş yapılırken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool KullaniciAdiVarMi(string kullaniciAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    bool sonuc = _kullanicilarRepository.KullaniciAdiVarMi(connection, kullaniciAdi);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kullanıcı adı alınırken hata oluştu.", ex);
            }
        }

        public Kullanicilar KullaniciBilgiGetir(string kullaniciAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    Kullanicilar sonuc = _kullanicilarRepository.KullaniciBilgiGetir(connection, kullaniciAdi);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kullanıcı bilgileri alınırken hata oluştu.", ex);
            }
        }

        public void KullaniciEkle(Kullanicilar kullanici)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _kullanicilarRepository.KullaniciEkle(connection, transaction, kullanici);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kullanıcı eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool KullaniciGuncelle(Kullanicilar kullanici)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kullanicilarRepository.KullaniciGuncelle(connection, transaction, kullanici);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kullanıcı güncellerken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool KullaniciGuncelleKullaniciBilgi(Kullanicilar kullanici)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kullanicilarRepository.KullaniciGuncelleKullaniciBilgi(connection, transaction, kullanici);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kullanıcı güncellerken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool KullaniciSil(string kullaniciAdi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kullanicilarRepository.KullaniciSil(connection, transaction, kullaniciAdi);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kullanıcı silinirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
