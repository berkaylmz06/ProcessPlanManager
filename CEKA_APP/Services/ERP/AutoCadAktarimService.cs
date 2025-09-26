using CEKA_APP.Abstracts.ERP;
using CEKA_APP.Entitys;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.Genel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Services.ERP
{
    public class AutoCadAktarimService : IAutoCadAktarimService
    {
        private readonly IAutoCadAktarimRepository _autoCadAktarimRepository;
        private readonly IDataBaseService _dataBaseService;

        public AutoCadAktarimService(IAutoCadAktarimRepository autoCadAktarimRepository, IDataBaseService dataBaseService)
        {
            _autoCadAktarimRepository = autoCadAktarimRepository ?? throw new ArgumentNullException(nameof(autoCadAktarimRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public void BaglaProjeVeGrup(string projeAdi, string secilenGrup)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.BaglaProjeVeGrup(connection, transaction, projeAdi, secilenGrup);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Bağlama sırasında hata oluştu.", ex);
                    }
                }
            }
        }

        public List<AutoCadAktarimDetay> GetAutoCadKayitlari(string projeAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.GetAutoCadKayitlari(connection, projeAdi);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("AutoCad kayıtları alınırken hata oluştu.", ex);
            }
        }

        public bool GetirStandartGruplar(string kalip)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.GetirStandartGruplar(connection, kalip);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Standart gruplar alınırken hata oluştu.", ex);
            }
        }

        public List<string> GetirStandartGruplarListe()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.GetirStandartGruplarListe(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Standart gruplar alınırken hata oluştu.", ex);
            }
        }

        public decimal GetNetAgirlik(string kalite, string malzeme, string kalipPoz, string proje)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.GetNetAgirlik(connection, kalite, malzeme, kalipPoz, proje);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ağırlık alınırken hata oluştu.", ex);
            }
        }

        public void GrupEkleGuncelle(string projeNo, string grupAdi, string eskiGrupAdi, string ustGrupAdi, Guid? yuklemeId = null, int takimCarpani = 1)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.GrupEkleGuncelle(connection, transaction, projeNo, grupAdi, eskiGrupAdi, ustGrupAdi, yuklemeId, takimCarpani);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Gruplar alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public DataTable GruplariGetir(string projeAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.GruplariGetir(connection, projeAdi);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Gruplar alınırken hata oluştu.", ex);
            }
        }

        public void GrupSil(string projeAdi, string grupAdi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.GrupSil(connection, transaction, projeAdi, grupAdi);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Grup silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public (bool, int) AdetGetir(string kalite, string malzeme, string kalip, string proje, decimal girilenAdet)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.AdetGetir(connection, kalite, malzeme, kalip, proje, girilenAdet);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Adetler alınırken hata oluştu.", ex);
            }
        }

        public (bool isValid, int toplamAdetIfs, int toplamAdetYuklenen) KontrolAdeta(string kalite, string malzeme, string kalip, string proje, int girilenAdet)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.KontrolAdeta(connection, kalite, malzeme, kalip, proje, girilenAdet);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Adetler alınırken hata oluştu.", ex);
            }
        }

        public DataTable MalzemeDetaylariniGetir(string projeAdi, string grupAdi, string malzemeKod)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.MalzemeDetaylariniGetir(connection, projeAdi, grupAdi, malzemeKod);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Malzeme detayları alınırken hata oluştu.", ex);
            }
        }

        public void MalzemeEkleGuncelle(string projeNo, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, decimal agirlik, Guid? yuklemeId = null, string eskiMalzemeKod = null)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.MalzemeEkleGuncelle(connection, transaction, projeNo, grupAdi, malzemeKod, adet, malzemeAd, kalite, agirlik, yuklemeId, eskiMalzemeKod);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Malzemeler alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public DataTable MalzemeleriGetir(string projeAdi, string grupAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.MalzemeleriGetir(connection, projeAdi, grupAdi);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Malzemeler alınırken hata oluştu.", ex);
            }
        }

        public void MalzemeSil(string projeAdi, string grupAdi, string malzemeKod)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.MalzemeSil(connection, transaction, projeAdi, grupAdi, malzemeKod);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Malzemeler silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void ProjeEkle(string projeAdi, string aciklama, DateTime olusturmaTarihi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.ProjeEkle(connection, transaction, projeAdi, aciklama, olusturmaTarihi);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void SaveAutoCadData(string projeAdi, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, Guid yuklemeId, decimal netAgirlik, int grupAdet)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.SaveAutoCadData(connection, transaction, projeAdi, grupAdi, malzemeKod, adet, malzemeAd, kalite, yuklemeId, netAgirlik, grupAdet);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("AutoCad verileri kaydedilirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void StandartGrupEkle(string grupNo)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.StandartGrupEkle(connection, transaction, grupNo);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Standart grup eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void StandartGrupSil(string grupNo)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.StandartGrupSil(connection, transaction, grupNo);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Standart grup silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void UpdateTakimCarpani(string projeAdi, string grupAdi, int takimCarpani)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.UpdateTakimCarpani(connection, transaction, projeAdi, grupAdi, takimCarpani);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Çarpım sırasında hata oluştu.", ex);
                    }
                }
            }
        }

        public void UpdateTakimCarpaniVeAltGruplar(string projeAdi, string ustGrup, Guid? yuklemeId, int takimCarpani)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.UpdateTakimCarpaniVeAltGruplar(connection, transaction, projeAdi, ustGrup, yuklemeId, takimCarpani);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Çarpım sırasında hata oluştu.", ex);
                    }
                }
            }
        }

        public void UstGrupEkleGuncelle(string projeNo, string ustGrupAdi, Guid? yuklemeId, int takimCarpani, string eskiUstGrupAdi = null)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.UstGrupEkleGuncelle(connection, transaction, projeNo, ustGrupAdi, yuklemeId, takimCarpani, eskiUstGrupAdi);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Üst grup ekleme sırasında hata oluştu.", ex);
                    }
                }
            }
        }

        public DataTable UstGruplariGetir(string projeAdi)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _autoCadAktarimRepository.UstGruplariGetir(connection, projeAdi);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Üst gruplar alınırken hata oluştu.", ex);
            }
        }

        public void UstGrupSil(string projeAdi, string ustGrupAdi, Guid yuklemeId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _autoCadAktarimRepository.UstGrupSil(connection, transaction, projeAdi, ustGrupAdi, yuklemeId);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Üst grup silinmesi sırasında hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
