using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using System;

namespace CEKA_APP.Services.ProjeFinans
{
    public class FinansProjelerService : IFinansProjelerService
    {
        private readonly IFinansProjelerRepository _finansProjelerRepository;
        private readonly IDataBaseService _dataBaseService;

        public FinansProjelerService(IFinansProjelerRepository finansProjelerRepository, IDataBaseService dataBaseService)
        {
            _finansProjelerRepository = finansProjelerRepository ?? throw new ArgumentNullException(nameof(finansProjelerRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public ProjeBilgi GetProjeBilgileri(int projeId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ProjeBilgi sonuc = _finansProjelerRepository.GetProjeBilgileri(connection, transaction, projeId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje bilgileri alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public ProjeBilgi GetProjeBilgileriByNo(string projeNo)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ProjeBilgi sonuc = _finansProjelerRepository.GetProjeBilgileriByNo(connection, transaction, projeNo);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje bilgileri alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public int? GetProjeIdByNo(string projeNo)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int? sonuc = _finansProjelerRepository.GetProjeIdByNo(connection, transaction, projeNo);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje Id alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public string GetProjeNoById(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    string sonuc = _finansProjelerRepository.GetProjeNoById(connection, projeId);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje No alınırken hata oluştu.", ex);
            }
        }

        public bool ProjeEkleProjeFinans(string projeNo, string projeTipi, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _finansProjelerRepository.ProjeEkleProjeFinans(connection, transaction, projeNo, projeTipi, aciklama, projeAdi, olusturmaTarihi);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool ProjeSil(int projeId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _finansProjelerRepository.ProjeSil(connection, transaction, projeId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool UpdateProjeFinans(int projeId, string projeNo, string projeTipi, string aciklama, string projeAdi, DateTime olusturmaTarihi, out bool degisiklikVar)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _finansProjelerRepository.UpdateProjeFinans(connection, transaction, projeId, projeNo, projeTipi, aciklama, projeAdi, olusturmaTarihi, out degisiklikVar);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

    }
}
