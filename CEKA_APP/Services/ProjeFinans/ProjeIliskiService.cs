using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;

namespace CEKA_APP.Services.ProjeFinans
{
    public class ProjeIliskiService : IProjeIliskiService
    {
        private readonly IProjeIliskiRepository _projeIliskiRepository;
        private readonly IDataBaseService _dataBaseService;

        public ProjeIliskiService(IProjeIliskiRepository projeIliskiRepository, IDataBaseService dataBaseService)
        {
            _projeIliskiRepository = projeIliskiRepository ?? throw new ArgumentNullException(nameof(projeIliskiRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public bool AltProjeEkle(int ustProjeId, int altProjeId)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeIliskiRepository.AltProjeEkle(connection, transaction, ustProjeId, altProjeId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Alt proje eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool AltProjeSil(int ustProjeId, int altProjeId)
        {

            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeIliskiRepository.AltProjeSil(connection, transaction, ustProjeId, altProjeId);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Alt proje silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool CheckAltProje(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    bool sonuc = _projeIliskiRepository.CheckAltProje(connection, projeId);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Alt projeler alınırken hata oluştu.", ex);
            }
        }

        public List<int> GetAltProjeler(int projeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _projeIliskiRepository.GetAltProjeler(connection, projeId);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Alt projeler alınırken hata oluştu.", ex);
            }
        }

        public int? GetUstProjeId(int altProjeId)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    int? sonuc = _projeIliskiRepository.GetUstProjeId(connection, altProjeId);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Üst proje Id alınırken hata oluştu.", ex);
            }
        }
    }
}
