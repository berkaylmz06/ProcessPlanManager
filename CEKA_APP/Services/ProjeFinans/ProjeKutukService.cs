using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Services.ProjeFinans
{
    public class ProjeKutukService : IProjeKutukService
    {
        private readonly IProjeKutukRepository _projeKutukRepository;

        public ProjeKutukService(IProjeKutukRepository projeKutukRepository)
        {
            _projeKutukRepository = projeKutukRepository ?? throw new ArgumentNullException(nameof(projeKutukRepository));
        }
        public bool AltProjeEkle(int ustProjeId, int altProjeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.AltProjeEkle(ustProjeId, altProjeId, transaction);
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

        public ProjeBilgi GetProjeBilgileri(string projeNo)
        {
            try
            {
                return _projeKutukRepository.GetProjeBilgileri(projeNo);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje NO alınırken hata oluştu.", ex);
            }
        }

        public ProjeKutuk GetProjeKutukStatus(int projeId)
        {
            try
            {
                return _projeKutukRepository.GetProjeKutukStatus(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public string GetProjeParaBirimi(int projeId)
        {
            try
            {
                return _projeKutukRepository.GetProjeParaBirimi(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public bool IsFaturalamaSekliTekil(int projeId)
        {
            try
            {
                return _projeKutukRepository.IsFaturalamaSekliTekil(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.ProjeEkleProjeFinans(projeNo, aciklama, projeAdi, olusturmaTarihi, transaction);
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

        public bool ProjeFiyatlandirmaEkle(string projeNo, decimal fiyat)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.ProjeFiyatlandirmaEkle(projeNo, fiyat, transaction);
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

        public ProjeKutuk ProjeKutukAra(int projeId)
        {
            try
            {
                return _projeKutukRepository.ProjeKutukAra(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public bool ProjeKutukEkle(ProjeKutuk kutuk)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.ProjeKutukEkle(kutuk, transaction);
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

        public bool ProjeKutukGuncelle(ProjeKutuk yeniKutuk)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.ProjeKutukGuncelle(transaction, yeniKutuk);
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

        public bool ProjeKutukSil(int projeId, List<int> altProjeIds)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.ProjeKutukSil(transaction, projeId, altProjeIds);
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

        public bool UpdateProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.UpdateProjeFinans(projeNo, aciklama, projeAdi, olusturmaTarihi, transaction);
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

        public bool UpdateProjeKutukDurum(int projeId, bool? montajTamamlandiMi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeKutukRepository.UpdateProjeKutukDurum(projeId, montajTamamlandiMi, transaction);
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

        public void UpdateToplamBedel(string projeNo, decimal toplamBedel)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _projeKutukRepository.UpdateToplamBedel(transaction, projeNo, toplamBedel);
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

        public (bool HasRelated, List<string> Details) HasRelatedRecords(int projeId, List<int> altProjeler)
        {
            try
            {
                return _projeKutukRepository.HasRelatedRecords(projeId, altProjeler);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Veriler alınırken hata oluştu.", ex);
            }
        }
    }
}
