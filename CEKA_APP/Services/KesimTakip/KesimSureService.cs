using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Concretes.KesimTakip;
using CEKA_APP.Entitys.KesimTakip;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Services.KesimTakip
{
    public class KesimSureService : IKesimSureService
    {
        private readonly IKesimSureRepository _kesimSureRepository;
        private readonly IDataBaseService _dataBaseService;

        public KesimSureService(IKesimSureRepository kesimSureRepository, IDataBaseService dataBaseService)
        {
            _kesimSureRepository = kesimSureRepository ?? throw new ArgumentNullException(nameof(kesimSureRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public int Baslat(string kesimId, string kesimYapan, string lotNo)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _kesimSureRepository.Baslat(connection, transaction, kesimId, kesimYapan, lotNo);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim süresi eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void Bitir(int sureId, int toplamSaniye)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _kesimSureRepository.Bitir(connection, transaction, sureId, toplamSaniye);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim süresi bitirilirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void Delete(int sureId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _kesimSureRepository.Delete(connection, transaction, sureId);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim süresi silinirken hata oluştu.", ex);
                    }
                }
            }
        }
        public void Durdur(int sureId, int toplamSaniye)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _kesimSureRepository.Durdur(connection, transaction, sureId, toplamSaniye);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim süresi durdurulurken hata oluştu.", ex);
                    }
                }
            }
        }

        public KesimSure GetBySureId(int sureId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        KesimSure sonuc = _kesimSureRepository.GetBySureId(connection, transaction, sureId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim süresi alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public List<(string KesimId, string LotNo, int En, int Boy, int ToplamSureSaniye, string KesimYapan)> GetirDevamEdenKesimler()
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        transaction.Commit();
                        return _kesimSureRepository.GetirDevamEdenKesimler(connection, transaction);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim süresi alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public DataTable GetirKesimHareketVeSure(string kesimId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimSureRepository.GetirKesimHareketVeSure(connection, kesimId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kesim süresi alınırken hata oluştu.", ex);
            }
        }

        public int GetirSureId(string kesimId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _kesimSureRepository.GetirSureId(connection, transaction, kesimId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim süresi alınırken hata oluştu.", ex);
                    }
                }
            }
        }

        public void GuncelleToplamSure(int sureId, int toplamSaniye)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _kesimSureRepository.GuncelleToplamSure(connection, transaction, sureId, toplamSaniye);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim süresi güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void IptalEt(int sureId, int toplamSaniye)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _kesimSureRepository.IptalEt(connection, transaction, sureId, toplamSaniye);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim iptal edilirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
