using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.Sistem
{
    public interface IKullanicilarService
    {
        Kullanicilar GirisYap(string kullaniciAdi, string sifre);
        DataTable GetKullaniciListesi();
        void KullaniciEkle(Kullanicilar kullanici);
        bool KullaniciAdiVarMi(string kullaniciAdi);
        bool KullaniciSil(string kullaniciAdi);
        bool KullaniciGuncelle(Kullanicilar kullanici);
        int GetKullaniciIdByKullaniciAdi(string kullaniciAdi);
        Kullanicilar KullaniciBilgiGetir(string kullaniciAdi);
        bool KullaniciGuncelleKullaniciBilgi(Kullanicilar kullanici);
    }
}
