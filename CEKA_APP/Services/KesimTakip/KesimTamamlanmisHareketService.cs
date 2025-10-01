using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using System;
using System.Data;

namespace CEKA_APP.Services.KesimTakip
{
    public class KesimTamamlanmisHareketService : IKesimTamamlanmisHareketService
    {
        private readonly IKesimTamamlanmisHareketRepository _kesimTamamlanmisHareketRepository;
        private readonly IDataBaseService _dataBaseService;

        public KesimTamamlanmisHareketService(IKesimTamamlanmisHareketRepository kesimTamamlanmisHareketRepository, IDataBaseService dataBaseService)
        {
            _kesimTamamlanmisHareketRepository = kesimTamamlanmisHareketRepository ?? throw new ArgumentNullException(nameof(kesimTamamlanmisHareketRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public DataTable GetirKesimTamamlanmisHareket(string kesimId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimTamamlanmisHareketRepository.GetirKesimTamamlanmisHareket(connection, kesimId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Hareketler alınırken hata oluştu.", ex);
            }
        }

        public bool TablodanKesimTamamlanmisHareketEkleme(string kesimYapan, string kesimId, int kesilenAdet, DateTime kesimTarihi, TimeSpan kesimSaati)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimTamamlanmisHareketRepository.TablodanKesimTamamlanmisHareketEkleme(connection, transaction, kesimYapan, kesimId, kesilenAdet, kesimTarihi, kesimSaati);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Hareket eklenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
