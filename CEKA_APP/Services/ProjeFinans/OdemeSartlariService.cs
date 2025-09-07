using CEKA_APP.Abstracts;
using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.Services.ProjeFinans
{
    public class OdemeSartlariService : IOdemeSartlariService
    {
        private readonly IOdemeSartlariRepository _odemeSartlariRepository;

        public OdemeSartlariService(IOdemeSartlariRepository odemeSartlariRepository)
        {
            _odemeSartlariRepository = odemeSartlariRepository ?? throw new ArgumentNullException(nameof(odemeSartlariRepository), "Ödeme şartları deposu null olamaz.");
        }

        public void SaveOrUpdateOdemeBilgi(OdemeSartlari odemeSartlarin)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _odemeSartlariRepository.SaveOrUpdateOdemeBilgi(odemeSartlarin, transaction);
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

        public string GetFaturaNo(int projeId, int kilometreTasiId)
        {

            if (projeId <= 0)
                throw new ArgumentException("Proje ID geçerli olmalıdır.", nameof(projeId));

            return _odemeSartlariRepository.GetFaturaNo(projeId, kilometreTasiId);
        }

        public List<OdemeSartlari> GetOdemeBilgileri()
        {
            try
            {
                return _odemeSartlariRepository.GetOdemeBilgileri();
            }
            catch (Exception ex)
            {
                throw new Exception("Ödeme bilgileri alınırken hata oluştu.", ex);
            }
        }

        public List<OdemeSartlari> GetOdemeBilgileriByProjeId(int projeId)
        {
            if (projeId <= 0)
                throw new ArgumentException("Proje ID geçerli olmalıdır.", nameof(projeId));

            try
            {
                return _odemeSartlariRepository.GetOdemeBilgileriByProjeId(projeId);
            }
            catch (Exception ex)
            {
                throw new Exception("Proje ID'ye göre ödeme bilgileri alınırken hata oluştu.", ex);
            }
        }

        public OdemeSartlari GetOdemeBilgi(string projeNo, int kilometreTasiId)
        {
            if (string.IsNullOrWhiteSpace(projeNo))
                throw new ArgumentException("Proje numarası geçerli olmalıdır.", nameof(projeNo));
            if (kilometreTasiId <= 0)
                throw new ArgumentException("Kilometre taşı ID geçerli olmalıdır.", nameof(kilometreTasiId));

            try
            {
                return _odemeSartlariRepository.GetOdemeBilgi(projeNo, kilometreTasiId);
            }
            catch (Exception ex)
            {
                throw new Exception("Ödeme bilgisi alınırken hata oluştu.", ex);
            }
        }

        public OdemeSartlari GetOdemeBilgiByOdemeId(int odemeId)
        {
            if (odemeId <= 0)
                throw new ArgumentException("Ödeme ID geçerli olmalıdır.", nameof(odemeId));

            try
            {
                return _odemeSartlariRepository.GetOdemeBilgiByOdemeId(odemeId);
            }
            catch (Exception ex)
            {
                throw new Exception("Ödeme ID'ye göre ödeme bilgisi alınırken hata oluştu.", ex);
            }
        }

        public void DeleteOdemeBilgi(int projeId, int kilometreTasiId)
        {
            if (projeId <= 0)
                throw new ArgumentException("Proje ID geçerli olmalıdır.", nameof(projeId));
            if (kilometreTasiId <= 0)
                throw new ArgumentException("Kilometre taşı ID geçerli olmalıdır.", nameof(kilometreTasiId));

            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _odemeSartlariRepository.DeleteOdemeBilgi(projeId, kilometreTasiId, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Ödeme bilgisi silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool UpdateFaturaNo(int odemeId, string faturaNo)
        {
            if (odemeId <= 0)
                throw new ArgumentException("Ödeme ID geçerli olmalıdır.", nameof(odemeId));

            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool result = _odemeSartlariRepository.UpdateFaturaNo(odemeId, faturaNo, transaction);
                        if (result)
                            transaction.Commit();
                        else
                            transaction.Rollback();

                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Fatura numarası güncellenirken hata oluştu.", ex);
                    }
                }
            }
        }

        public bool OdemeSartlariSil(int projeId)
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
                        if (transaction == null)
                            throw new InvalidOperationException("Transaction başlatılamadı.");
                        bool result = _odemeSartlariRepository.OdemeSartlariSil(projeId, transaction);
                        if (result)
                            transaction.Commit();
                        else
                            transaction.Rollback();

                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Ödeme şartları silinirken hata oluştu.", ex);
                    }
                }
            }
        }

        public DataTable FiltreleOdemeBilgileri(Dictionary<string, TextBox> filtreKriterleri, DataGridView dataGrid)
        {
            if (filtreKriterleri == null)
                throw new ArgumentNullException(nameof(filtreKriterleri), "Filtre kriterleri null olamaz.");
            if (dataGrid == null)
                throw new ArgumentNullException(nameof(dataGrid), "DataGridView null olamaz.");

            try
            {
                return _odemeSartlariRepository.FiltreleOdemeBilgileri(filtreKriterleri, dataGrid);
            }
            catch (Exception ex)
            {
                throw new Exception("Ödeme bilgileri filtrelenirken hata oluştu.", ex);
            }
        }

        public DataTable ToDataTableWithOdemeSapmasi(List<OdemeSartlari> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Veri listesi null olamaz.");

            try
            {
                return _odemeSartlariRepository.ToDataTableWithOdemeSapmasi(data);
            }
            catch (Exception ex)
            {
                throw new Exception("Veri tablosuna dönüştürülürken hata oluştu.", ex);
            }
        }

        public string NormalizeColumnName(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("Sütun adı boş olamaz.", nameof(columnName));

            try
            {
                return _odemeSartlariRepository.NormalizeColumnName(columnName);
            }
            catch (Exception ex)
            {
                throw new Exception("Sütun adı normalleştirilirken hata oluştu.", ex);
            }
        }
    }
}