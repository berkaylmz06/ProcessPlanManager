using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KesimTakip.DataBase;

namespace KesimTakip.Helper
{
    public class LogEkle
    {
        private readonly int _kullaniciId;
        private readonly KullanicilarData _kullaniciService;

        public LogEkle(string kullaniciAdi)
        {
            if(string.IsNullOrEmpty(kullaniciAdi))
                throw new ArgumentException("Kullanıcı adı boş olamaz.", nameof(kullaniciAdi));

            _kullaniciService = new KullanicilarData();
            try
            {
                _kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(kullaniciAdi);
            }
            catch (Exception ex)
            {
                throw new Exception("Kullanıcı ID'si alınamadı.", ex);
            }
        }
        public void LogYap(string islemTuru, string sayfaAdi, string ekBilgi = "")
        {
            if (string.IsNullOrEmpty(islemTuru) || string.IsNullOrEmpty(sayfaAdi))
                throw new ArgumentException("İşlem türü ve sayfa adı boş olamaz.");

            try
            {
                KullaniciHareketLogData.LogEkle(_kullaniciId, islemTuru, sayfaAdi, ekBilgi);
            }
            catch (Exception ex)
            {
                throw new Exception("Log ekleme sırasında bir hata oluştu.", ex);
            }
        }
    }

}
