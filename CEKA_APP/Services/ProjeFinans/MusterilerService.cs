using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Concretes.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Services.ProjeFinans
{
    public class MusterilerService : IMusterilerService
    {
        private readonly IMusterilerRepository _musterilerRepository;

        public MusterilerService(IMusterilerRepository musterilerRepository)
        {
            _musterilerRepository = musterilerRepository ?? throw new ArgumentNullException(nameof(musterilerRepository));
        }
        public DataTable FiltreleMusteriBilgileri(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid)
        {
            try
            {
                return _musterilerRepository.FiltreleMusteriBilgileri(filtreKutulari,dataGrid);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Müşteriler alınırken hata oluştu.", ex); throw;
            }
        }

        public Musteriler GetMusteriByMusteriNo(string musteriNo)
        {
            try
            {
                return _musterilerRepository.GetMusteriByMusteriNo(musteriNo);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Müşteriler alınırken hata oluştu.", ex); throw;
            }
        }

        public List<Musteriler> GetMusteriler()
        {
            try
            {
                return _musterilerRepository.GetMusteriler();

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Müşteriler alınırken hata oluştu.", ex); throw;
            }
        }

        public void MusteriKaydet(Musteriler musteri)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _musterilerRepository.MusteriKaydet(musteri, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool MusteriNoVarMi(string musteriNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _musterilerRepository.MusteriNoVarMi(musteriNo, transaction);
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

        public string NormalizeColumnName(string columnName)
        {
            try
            {
                return NormalizeColumnName(columnName);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Müşteriler alınırken hata oluştu.", ex); throw;
            }
        }

        public void TumMusterileriSil()
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _musterilerRepository.TumMusterileriSil(transaction);
                        transaction.Commit();
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
