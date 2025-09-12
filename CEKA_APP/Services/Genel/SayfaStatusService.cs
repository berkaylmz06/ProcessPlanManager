using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys.Genel;
using CEKA_APP.Interfaces.Genel;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Services.Genel
{
    public class SayfaStatusService : ISayfaStatusService
    {
        private readonly ISayfaStatusRepository _sayfaStatusRepository;

        public SayfaStatusService(ISayfaStatusRepository sayfaStatusRepository)
        {
            _sayfaStatusRepository = sayfaStatusRepository ?? throw new ArgumentNullException(nameof(sayfaStatusRepository));
        }
        public void Delete(int sayfaStatusId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _sayfaStatusRepository.Delete(sayfaStatusId, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public SayfaStatus Get(int sayfaId, int projeId)
        {
            try
            {
                return _sayfaStatusRepository.Get(sayfaId, projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Sayfa Statüsü alınırken hata oluştu.", ex);
            }
        }

        public List<string> GetNedenTamamlanmadiByProjeId(int projeId)
        {

            try
            {
                return _sayfaStatusRepository.GetNedenTamamlanmadiByProjeId(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Sayfa Statüsü alınırken hata oluştu.", ex);
            }
        }

        public int Insert(SayfaStatus status)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _sayfaStatusRepository.Insert(status, transaction);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool IsAllAltProjelerSayfa4Kapali(int projeId)
        {
            try
            {
                return _sayfaStatusRepository.IsAllAltProjelerSayfa4Kapali(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public bool IsSayfa3Kapali(int projeId)
        {
            try
            {
                return _sayfaStatusRepository.IsSayfa3Kapali(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public void Update(SayfaStatus status)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _sayfaStatusRepository.Update(status, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
