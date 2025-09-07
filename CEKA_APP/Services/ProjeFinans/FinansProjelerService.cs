using CEKA_APP.Abstracts.ProjeFinans;
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
    public class FinansProjelerService : IFinansProjelerService
    {
        private readonly IFinansProjelerRepository _finansProjelerRepository;

        public FinansProjelerService(IFinansProjelerRepository finansProjelerRepository)
        {
            _finansProjelerRepository = finansProjelerRepository ?? throw new ArgumentNullException(nameof(finansProjelerRepository));
        }
        public ProjeBilgi GetProjeBilgileri(int projeId)
        {
            try
            {
                return _finansProjelerRepository.GetProjeBilgileri(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public ProjeBilgi GetProjeBilgileriByNo(string projeNo)
        {
            try
            {
                return _finansProjelerRepository.GetProjeBilgileriByNo(projeNo);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje NO alınırken hata oluştu.", ex);
            }
        }

        public int? GetProjeIdByNo(string projeNo)
        {
            try
            {
                return _finansProjelerRepository.GetProjeIdByNo(projeNo);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje NO alınırken hata oluştu.", ex);
            }
        }

        public string GetProjeNoById(int projeId)
        {
            try
            {
                return _finansProjelerRepository.GetProjeNoById(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public bool ProjeEkleProjeFinans(string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _finansProjelerRepository.ProjeEkleProjeFinans(projeNo, aciklama, projeAdi, olusturmaTarihi, transaction);
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

        public bool ProjeSil(int projeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _finansProjelerRepository.ProjeSil(projeId, transaction);
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

        public bool UpdateProjeFinans(int projeId, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi, out bool degisiklikVar)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _finansProjelerRepository.UpdateProjeFinans(projeId, projeNo, aciklama, projeAdi, olusturmaTarihi, transaction, out degisiklikVar);

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
