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
using System.Windows.Forms;

namespace CEKA_APP.Services.ProjeFinans
{
    public class OdemeHareketleriService : IOdemeHareketleriService
    {
        private readonly IOdemeHareketleriRepository _odemeHareketleriRepository;

        public OdemeHareketleriService(IOdemeHareketleriRepository odemeHareketleriRepository)
        {
            _odemeHareketleriRepository = odemeHareketleriRepository ?? throw new ArgumentNullException(nameof(odemeHareketleriRepository), "Ödeme şartları deposu null olamaz.");
        }
        public void DeleteOdemeHareketleriByOdemeIds(List<int> odemeIds)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _odemeHareketleriRepository.DeleteOdemeHareketleriByOdemeIds(odemeIds, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"SQL Hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }
            }
        }

        public List<OdemeHareketleri> GetOdemeHareketleriByOdemeId(int odemeId)
        {
            if (odemeId <= 0)
                throw new ArgumentException("Odeme ID geçerli olmalıdır.", nameof(odemeId));

            try
            {
                return _odemeHareketleriRepository.GetOdemeHareketleriByOdemeId(odemeId);
            }
            catch (Exception ex)
            {
                throw new Exception("Odeme ID'ye göre ödeme bilgileri alınırken hata oluştu.", ex);
            }
        }

        public bool SaveOdemeHareketi(OdemeHareketleri odemeHareketi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _odemeHareketleriRepository.SaveOdemeHareketi(odemeHareketi, transaction);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"SQL Hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }
            }
        }
    }
}
