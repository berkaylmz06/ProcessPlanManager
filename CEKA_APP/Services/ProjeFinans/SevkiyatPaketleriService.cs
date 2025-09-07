using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Concretes.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Services.ProjeFinans
{
    public class SevkiyatPaketleriService : ISevkiyatPaketleriService
    {
        private readonly ISevkiyatPaketleriRepository _sevkiyatPaketleriRepository;

        public SevkiyatPaketleriService(ISevkiyatPaketleriRepository sevkiyatPaketleriRepository)
        {
            _sevkiyatPaketleriRepository = sevkiyatPaketleriRepository ?? throw new ArgumentNullException(nameof(sevkiyatPaketleriRepository));
        }
        public int GetPaketIdByAdi(string paketAdi)
        {
            try
            {
                return _sevkiyatPaketleriRepository.GetPaketIdByAdi(paketAdi);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Servis Paketleri alınırken hata oluştu.", ex); throw;
            }
        }

        public List<(int Id, string Adi, DateTime Tarih)> GetPaketler()
        {
            try
            {
                return _sevkiyatPaketleriRepository.GetPaketler();

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Servis Paketleri alınırken hata oluştu.", ex); throw;
            }
        }

        public int PaketEkle(string paketAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sonuc = _sevkiyatPaketleriRepository.PaketEkle(paketAdi, transaction);
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
    }
}
