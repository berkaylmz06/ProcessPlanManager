using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CEKA_APP.Services.KesimTakip
{
    public class KesimTamamlanmisService : IKesimTamamlanmisService
    {
        private readonly IKesimTamamlanmisRepository _kesimTamamlanmisRepository;
        private readonly IDataBaseService _dataBaseService;

        public KesimTamamlanmisService(IKesimTamamlanmisRepository kesimTamamlanmisRepository, IDataBaseService dataBaseService)
        {
            _kesimTamamlanmisRepository = kesimTamamlanmisRepository ?? throw new ArgumentNullException(nameof(kesimTamamlanmisRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public DataTable GetKesimListesTamamlanmis()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimTamamlanmisRepository.GetKesimListesTamamlanmis(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Tamamlanmış kesim listesi alınırken hata oluştu.", ex);
            }
        }

        public string GetKesimListesTamamlanmisQuery()
        {
           return _kesimTamamlanmisRepository.GetKesimListesTamamlanmisQuery();
        }

        public bool TablodanKesimTamamlanmisEkleme(string kesimYapan, string kesimId, int kesilmisPlanSayisi, DateTime kesimTarihi, TimeSpan kesimSaati, string kesilenLot)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimTamamlanmisRepository.TablodanKesimTamamlanmisEkleme(connection, transaction, kesimYapan, kesimId, kesilmisPlanSayisi,kesimTarihi, kesimSaati,kesilenLot);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Tamamlanmış kesim eklenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
