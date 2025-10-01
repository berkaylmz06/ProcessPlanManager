using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Entitys;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using System;
using System.Collections.Generic;
using System.Data;

namespace CEKA_APP.Services.KesimTakip
{
    public class KesimDetaylariService : IKesimDetaylariService
    {
        private readonly IKesimDetaylariRepository _kesimDetaylariRepository;
        private readonly IDataBaseService _dataBaseService;

        public KesimDetaylariService(IKesimDetaylariRepository kesimDetaylariRepository, IDataBaseService dataBaseService)
        {
            _kesimDetaylariRepository = kesimDetaylariRepository ?? throw new ArgumentNullException(nameof(kesimDetaylariRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public (decimal toplamAdet, decimal kesilmisAdet, decimal kesilecekAdet, List<string> eslesenPozlar) GetAdetlerVeEslesenPozlar(string kalite, string malzeme, string proje, string malzemeKodIlkKisim, string malzemeKodUcuncuKisim)
        {

            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimDetaylariRepository.GetAdetlerVeEslesenPozlar(connection, kalite, malzeme, proje, malzemeKodIlkKisim, malzemeKodUcuncuKisim);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("eşleşen pozlar ve adetler alınırken hata oluştu.", ex);
            }
        }

        public DataTable GetKesimDetaylariBilgi()
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimDetaylariRepository.GetKesimDetaylariBilgi(connection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kesim detayları alınırken hata oluştu.", ex);
            }
        }

        public List<KesimDetaylari> GetKesimDetaylariBilgileri(string projeAdi = null, string grupAdi = null)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimDetaylariRepository.GetKesimDetaylariBilgileri(connection, projeAdi, grupAdi);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Kesim detayları alınırken hata oluştu.", ex);
            }
        }

        public bool GuncelleKesimDetaylari(string kalite, string malzeme, string kalipNo, string kesilecekPozlar, string proje, decimal silinecekAdet = 0, bool tamSilme = false)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimDetaylariRepository.GuncelleKesimDetaylari(connection, transaction, kalite, malzeme, kalipNo, kesilecekPozlar, proje, silinecekAdet, tamSilme);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim detayları güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool PozExists(string kalite, string malzeme, string malzemekod, string proje)
        {

            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _kesimDetaylariRepository.PozExists(connection, kalite, malzeme, malzemekod, proje);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Pozlar alınırken hata oluştu.", ex);
            }
        }

        public void SaveKesimDetaylariData(string kalite, string malzeme, string malzemeKod, string proje, int kesilecekAdet, int toplamAdet, bool ekBilgi)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _kesimDetaylariRepository.SaveKesimDetaylariData(connection, transaction, kalite, malzeme, malzemeKod, proje, kesilecekAdet, toplamAdet, ekBilgi);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesim detayları eklenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool UpdateKesilmisAdet(string kalite, string malzeme, string malzemekod, string proje, decimal sondurum)
        {
            using (var connection = _dataBaseService.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _kesimDetaylariRepository.UpdateKesilmisAdet(connection, transaction, kalite, malzeme, malzemekod, proje, sondurum);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ApplicationException("Kesilmiş adetler güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }
    }
}