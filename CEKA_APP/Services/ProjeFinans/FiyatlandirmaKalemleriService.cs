using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Concretes.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Services.ProjeFinans
{
    public class FiyatlandirmaKalemleriService : IFiyatlandirmaKalemleriService
    {
        private readonly IFiyatlandirmaKalemleriRepository _fiyatlandirmaKalemleriRepository;

        public FiyatlandirmaKalemleriService(IFiyatlandirmaKalemleriRepository fiyatlandirmaKalemleriRepository)
        {
            _fiyatlandirmaKalemleriRepository = fiyatlandirmaKalemleriRepository ?? throw new ArgumentNullException(nameof(fiyatlandirmaKalemleriRepository));
        }
        public int FiyatlandirmaKalemleriEkle(string kalemAdi, string kalemBirimi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _fiyatlandirmaKalemleriRepository.FiyatlandirmaKalemleriEkle(kalemAdi, kalemBirimi, transaction);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public FiyatlandirmaKalem GetFiyatlandirmaKalemByAdi(string kalemAdi)
        {
            try
            {
                return _fiyatlandirmaKalemleriRepository.GetFiyatlandirmaKalemByAdi(kalemAdi);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fiyatlandırma kalemi alınırken hata oluştu.", ex);
            }
        }

        public List<(int Id, string Adi, string Birimi, DateTime Tarih)> GetFiyatlandirmaKalemleri()
        {
            try
            {
                return _fiyatlandirmaKalemleriRepository.GetFiyatlandirmaKalemleri();

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Fiyatlandırma kalemi alınırken hata oluştu.", ex); throw;
            }
        }
    }
}
