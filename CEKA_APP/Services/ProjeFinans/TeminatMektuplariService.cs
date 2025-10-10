using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CEKA_APP.Services.ProjeFinans
{
    public class TeminatMektuplariService : ITeminatMektuplariService
    {
        private readonly ITeminatMektuplariRepository _teminatMektuplariRepository;
        private readonly IDataBaseService _dataBaseService;

        public TeminatMektuplariService(ITeminatMektuplariRepository teminatMektuplariRepository, IDataBaseService dataBaseService)
        {
            _teminatMektuplariRepository = teminatMektuplariRepository ?? throw new ArgumentNullException(nameof(teminatMektuplariRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public TeminatMektuplari GetTeminatMektubuByProjeNo(string projeNo)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _teminatMektuplariRepository.GetTeminatMektubuByProjeNo(connection, projeNo);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Teminat mektupları alınırken hata oluştu.", ex);
            }
        }

        public List<TeminatMektuplari> GetTeminatMektuplari()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _teminatMektuplariRepository.GetTeminatMektuplari(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Teminat mektupları alınırken hata oluştu.", ex);
            }
        }

        public string GetTeminatMektuplariQuery()
        {
            return _teminatMektuplariRepository.GetTeminatMektuplariQuery();
        }

        public void MektupGuncelle(string eskiMektupNo, TeminatMektuplari guncelMektup)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _teminatMektuplariRepository.MektupGuncelle(connection, transaction, eskiMektupNo, guncelMektup);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Mektup güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool MektupNoVarMi(string mektupNo)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _teminatMektuplariRepository.MektupNoVarMi(connection, mektupNo);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Mektup no alınırken hata oluştu.", ex);
            }
        }

        public void MektupSil(string mektupNo)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _teminatMektuplariRepository.MektupSil(connection, transaction, mektupNo);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Mektup silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool TeminatMektubuKaydet(TeminatMektuplari mektup)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _teminatMektuplariRepository.TeminatMektubuKaydet(connection, transaction, mektup);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Mektup kaydedilirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void UpdateKilometreTasiAdi(string mektupNo, int kilometreTasiId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _teminatMektuplariRepository.UpdateKilometreTasiAdi(connection, transaction, mektupNo, kilometreTasiId);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Mektup kilometre taşı güncelenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void UpdateTeminatDurum(string mektupNo, string durum)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _teminatMektuplariRepository.UpdateTeminatDurum(connection, transaction, mektupNo, durum);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Mektup durum güncelenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
