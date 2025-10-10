using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using System;
using System.Data;
using System.Windows.Forms;

namespace CEKA_APP.Services.KesimTakip
{
    public class KesimListesiPaketService : IKesimListesiPaketService
    {
        private readonly IKesimListesiPaketRepository _kesimListesiPaketRepository;
        private readonly IDataBaseService _dataBaseService;

        public KesimListesiPaketService(IKesimListesiPaketRepository kesimListesiPaketRepository, IDataBaseService dataBaseService)
        {
            _kesimListesiPaketRepository = kesimListesiPaketRepository ?? throw new ArgumentNullException(nameof(kesimListesiPaketRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public DataTable GetKesimListesiPaket()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimListesiPaketRepository.GetKesimListesiPaket(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kesim listesi alınırken hata oluştu.", ex);
            }
        }

        public string GetKesimListesiPaketQuery()
        {
           return _kesimListesiPaketRepository.GetKesimListesiPaketQuery();
        }

        public DataTable GetKesimListesiPaketSure()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimListesiPaketRepository.GetKesimListesiPaketSure(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kesim listesi alınırken hata oluştu.", ex);
            }
        }

        public string GetKesimListesiPaketSureQuery()
        {
            return _kesimListesiPaketRepository.GetKesimListesiPaketSureQuery();
        }

        public bool KesimIdVarMi(string kesimId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    bool sonuc = _kesimListesiPaketRepository.KesimIdVarMi(connection, kesimId);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Id alınırken hata oluştu.", ex);
            }
        }

        public bool KesimListesiPaketIptalEt(string kesimId, string iptalNedeni)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimListesiPaketRepository.KesimListesiPaketIptalEt(connection, transaction, kesimId, iptalNedeni);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim paket iptal edilirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool KesimListesiPaketKontrolluDusme(string kesimId, int kesilenMiktar, out string hataMesaji)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimListesiPaketRepository.KesimListesiPaketKontrolluDusme(connection, transaction, kesimId, kesilenMiktar, out hataMesaji);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Paketten düşerken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool KesimListesiPaketSil(string kesimId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimListesiPaketRepository.KesimListesiPaketSil(connection, transaction, kesimId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Paket silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool SaveKesimDataPaket(string olusturan, string kesimId, int kesilecekPlanSayisi, int toplamPlanTekrari, DateTime eklemeTarihi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimListesiPaketRepository.SaveKesimDataPaket(connection, transaction, olusturan, kesimId, kesilecekPlanSayisi, toplamPlanTekrari, eklemeTarihi);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Paket eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public void VerileriYenile(DataGridView data)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    _kesimListesiPaketRepository.VerileriYenile(connection, data);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Veriler alınırken hata oluştu.", ex);
            }
        }
    }
}
