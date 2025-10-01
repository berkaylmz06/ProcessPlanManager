using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using System;

namespace CEKA_APP.Services.KesimTakip
{
    public class IdUreticiService : IIdUreticiService
    {
        private readonly IIdUreticiRepository _idUreticiRepository;
        private readonly IDataBaseService _dataBaseService;

        public IdUreticiService(IIdUreticiRepository idUreticiRepository, IDataBaseService dataBaseService)
        {
            _idUreticiRepository = idUreticiRepository ?? throw new ArgumentNullException(nameof(idUreticiRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public int GetSiradakiId()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _idUreticiRepository.GetSiradakiId(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Sıradaki Id alınırken hata oluştu.", ex);
            }
        }

        public bool SiradakiIdKaydet(int id)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _idUreticiRepository.SiradakiIdKaydet(connection, transaction, id);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Id eklenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
