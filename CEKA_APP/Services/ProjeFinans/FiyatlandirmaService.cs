using CEKA_APP.Abstracts;
using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Services.ProjeFinans
{
    public class FiyatlandirmaService : IFiyatlandirmaService
    {
        private readonly IFiyatlandirmaRepository _fiyalandirmaRepository;

        public FiyatlandirmaService(IFiyatlandirmaRepository fiyalandirmaRepository)
        {
            _fiyalandirmaRepository = fiyalandirmaRepository ?? throw new ArgumentNullException(nameof(fiyalandirmaRepository));
        }
        public bool FiyatlandirmaGuncelle(Fiyatlandirma fiyat)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _fiyalandirmaRepository.FiyatlandirmaGuncelle(fiyat, transaction);
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

        public bool FiyatlandirmaKaydet(Fiyatlandirma fiyat)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _fiyalandirmaRepository.FiyatlandirmaKaydet(fiyat, transaction);
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

        public bool FiyatlandirmaSil(int projeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _fiyalandirmaRepository.FiyatlandirmaSil(projeId, transaction);
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

        public bool FiyatlandirmaSilById(int projeId, int fiyatlandirmaKalemId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _fiyalandirmaRepository.FiyatlandirmaSilById(projeId, fiyatlandirmaKalemId, transaction);
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

        public List<Fiyatlandirma> GetFiyatlandirmaByProje(int projeId)
        {
            if (projeId <= 0)
                throw new ArgumentException("Proje ID geçerli olmalıdır.", nameof(projeId));

            return _fiyalandirmaRepository.GetFiyatlandirmaByProje(projeId);
        }

        public (decimal toplamBedel, List<int> eksikFiyatlandirmaProjeler) GetToplamBedel(int projeId, List<int> altProjeler = null)
        {
            if (projeId <= 0)
                throw new ArgumentException("Proje ID geçerli olmalıdır.", nameof(projeId));

            return _fiyalandirmaRepository.GetToplamBedel(projeId, altProjeler);
        }
    }
}
