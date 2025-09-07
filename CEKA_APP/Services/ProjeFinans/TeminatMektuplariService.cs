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
    public class TeminatMektuplariService : ITeminatMektuplariService
    {
        private readonly ITeminatMektuplariRepository _teminatMektuplariRepository;

        public TeminatMektuplariService(ITeminatMektuplariRepository teminatMektuplariRepository)
        {
            _teminatMektuplariRepository = teminatMektuplariRepository ?? throw new ArgumentNullException(nameof(teminatMektuplariRepository));
        }
        public DataTable FiltreleTeminatMektuplari(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid)
        {
            try
            {
                return _teminatMektuplariRepository.FiltreleTeminatMektuplari(filtreKutulari, dataGrid);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Teminat mektubu alınırken hata oluştu.", ex);
            }
        }

        public List<TeminatMektuplari> GetTeminatMektuplari()
        {
            try
            {
                return _teminatMektuplariRepository.GetTeminatMektuplari();

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Teminat mektubu alınırken hata oluştu.", ex);
            }
        }

        public void MektupGuncelle(string eskiMektupNo, TeminatMektuplari guncelMektup)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _teminatMektuplariRepository.MektupGuncelle(eskiMektupNo, guncelMektup, transaction);
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

        public bool MektupNoVarMi(string mektupNo)
        {
            try
            {
                return _teminatMektuplariRepository.MektupNoVarMi(mektupNo);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Mektub NO alınırken hata oluştu.", ex);
            }
        }

        public void MektupSil(string mektupNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _teminatMektuplariRepository.MektupSil(mektupNo, transaction);
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

        public string NormalizeColumnName(string columnName)
        {
            try
            {
                return _teminatMektuplariRepository.NormalizeColumnName(columnName);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Teminat mektubu alınırken hata oluştu.", ex);
            }
        }

        public bool TeminatMektubuKaydet(TeminatMektuplari mektup)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _teminatMektuplariRepository.TeminatMektubuKaydet(mektup, transaction);
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
