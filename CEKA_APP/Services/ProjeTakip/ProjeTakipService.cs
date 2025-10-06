using CEKA_APP.Abstracts.ProjeTakip;
using CEKA_APP.Entitys.ProjeTakip;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeTakip;
using System;

namespace CEKA_APP.Services.ProjeTakip
{
    public class ProjeTakipService : IProjeTakipService
    {
        private readonly IProjeTakipRepository _projeTakipRepository;
        private readonly IDataBaseService _dataBaseService;

        public ProjeTakipService(IProjeTakipRepository projeTakipRepository, IDataBaseService dataBaseService)
        {
            _projeTakipRepository = projeTakipRepository ?? throw new ArgumentNullException(nameof(projeTakipRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public ProjeKarti ProjeKartiAra(int projeKartId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _projeTakipRepository.ProjeKartiAra(connection, projeKartId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje kartı alınırken hata oluştu.", ex);
            }
        }

        public bool ProjeKartiEkle(ProjeKarti projeKarti)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeTakipRepository.ProjeKartiEkle(connection, transaction, projeKarti);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje kartı eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool ProjeKartiSil(int projeKartId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeTakipRepository.ProjeKartiSil(connection, transaction, projeKartId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Proje kartı silinirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}
