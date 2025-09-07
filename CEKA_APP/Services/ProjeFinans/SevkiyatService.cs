using CEKA_APP.Abstracts;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class SevkiyatService : ISevkiyatService
    {
        private readonly ISevkiyatRepository _sevkiyatRepository;

        public SevkiyatService(ISevkiyatRepository sevkiyatRepository)
        {
            _sevkiyatRepository = sevkiyatRepository ?? throw new ArgumentNullException(nameof(sevkiyatRepository));
        }

        public List<Sevkiyat> GetSevkiyatByProje(int projeId)
        {
            if (projeId <= 0)
                throw new ArgumentException("Proje ID geçerli olmalıdır.", nameof(projeId));

            return _sevkiyatRepository.GetSevkiyatByProje(projeId);
        }

        public void SevkiyatKaydet(Sevkiyat sevkiyat)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _sevkiyatRepository.SevkiyatKaydet(sevkiyat, transaction);
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

        public void SevkiyatGuncelle(Sevkiyat sevkiyat)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _sevkiyatRepository.SevkiyatGuncelle(sevkiyat, transaction);
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

        public bool SevkiyatSilBySevkiyatId(int projeId, string sevkiyatId, int aracSira)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool result = _sevkiyatRepository.SevkiyatSilBySevkiyatId(projeId, sevkiyatId, aracSira, transaction);
                        if (result)
                            transaction.Commit();
                        else
                            transaction.Rollback();

                        return result;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool SevkiyatSil(int projeId)
        {
            if (projeId <= 0)
                throw new ArgumentException("Proje ID geçerli olmalıdır.", nameof(projeId));

            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool result = _sevkiyatRepository.SevkiyatSil(projeId, transaction);
                        if (result)
                            transaction.Commit();
                        else
                            transaction.Rollback();

                        return result;
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