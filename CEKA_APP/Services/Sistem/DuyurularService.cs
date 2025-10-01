using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Abstracts.Sistem;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Entitys.Genel;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.Sistem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Services.Sistem
{
    public class DuyurularService : IDuyurularService
    {
        private readonly IDuyurularRepository _duyurularRepository;
        private readonly IDataBaseService _dataBaseService;

        public DuyurularService(IDuyurularRepository duyurularRepository, IDataBaseService dataBaseService)
        {
            _duyurularRepository = duyurularRepository ?? throw new ArgumentNullException(nameof(duyurularRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }
        public bool DuyuruEkle(string olusturan, string duyuru, DateTime sistemSaat)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _duyurularRepository.DuyuruEkle(olusturan, duyuru, sistemSaat, connection, transaction);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Duyuru eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public Duyurular GetSonDuyuru()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    Duyurular sonuc = _duyurularRepository.GetSonDuyuru(connection);
                    return sonuc;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Duyuru alınırken hata oluştu.", ex);
            }
        }
    }
}
