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
    public class MusterilerService : IMusterilerService
    {
        private readonly IMusterilerRepository _musterilerRepository;
        private readonly IDataBaseService _dataBaseService;

        public MusterilerService(IMusterilerRepository musterilerRepository, IDataBaseService dataBaseService)
        {
            _musterilerRepository = musterilerRepository ?? throw new ArgumentNullException(nameof(musterilerRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public Musteriler GetMusteriByMusteriNo(string musteriNo)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _musterilerRepository.GetMusteriByMusteriNo(connection, musteriNo);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Müşteriler alınırken hata oluştu.", ex);
            }
        }

        public List<Musteriler> GetMusteriler()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _musterilerRepository.GetMusteriler(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Müşteriler alınırken hata oluştu.", ex);
            }
        }

        public List<Musteriler> GetMusterilerAraFormu()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _musterilerRepository.GetMusterilerAraFormu(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Müşteriler alınırken hata oluştu.", ex);
            }
        }

        public string GetMusterilerAraFormuQuery()
        {
            return _musterilerRepository.GetMusterilerAraFormuQuery();
        }

        public string GetMusterilerQuery()
        {
            return _musterilerRepository.GetMusterilerQuery();
        }

        public void MusteriKaydet(Musteriler musteri)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _musterilerRepository.MusteriKaydet(connection, transaction, musteri);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Müşteri kaydı yapılırken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool MusteriNoVarMi(string musteriNo)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    bool sonuc = _musterilerRepository.MusteriNoVarMi(connection, musteriNo);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Müşteri no alınırken hata oluştu.", ex);
            }
        }

        public void TumMusterileriSil()
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _musterilerRepository.TumMusterileriSil(connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Müşteri silinirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
