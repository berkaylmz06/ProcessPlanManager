using CEKA_APP.Abstracts.Sistem;
using CEKA_APP.Concretes.Sistem;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.Sistem;
using CEKA_APP.Services.Genel;
using System;
using System.Data;

namespace CEKA_APP.Services.Sistem
{
    public class KullaniciHareketLogService : IKullaniciHareketLogService
    {
        private readonly IKullaniciHareketLogRepository _kullaniciHareketLogRepository;
        private readonly IDataBaseService _dataBaseService;
        public KullaniciHareketLogService(IKullaniciHareketLogRepository kullaniciHareketLogRepository, IDataBaseService dataBaseService)
        {
            _kullaniciHareketLogRepository = kullaniciHareketLogRepository ?? throw new ArgumentNullException(nameof(kullaniciHareketLogRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public DataTable GetKullaniciLog()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kullaniciHareketLogRepository.GetKullaniciLog(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kullanıcı log alınırken hata oluştu.", ex);
            }
        }

        public void LogEkle(int kullaniciId, string islemTuru, string sayfaAdi, string ekBilgi = "")
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _kullaniciHareketLogRepository.LogEkle(connection, transaction, kullaniciId, islemTuru, sayfaAdi, ekBilgi);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Log eklenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
