using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.Sistem
{
    public interface IKullanicilarRepository
    {
        Kullanicilar GirisYap(SqlConnection connection, SqlTransaction transaction, string kullaniciAdi, string sifre);
        DataTable GetKullaniciListesi(SqlConnection connection);
        void KullaniciEkle(SqlConnection connection, SqlTransaction transaction, Kullanicilar kullanici);
        bool KullaniciAdiVarMi(SqlConnection connection, string kullaniciAdi);
        bool KullaniciSil(SqlConnection connection, SqlTransaction transaction, string kullaniciAdi);
        bool KullaniciGuncelle(SqlConnection connection, SqlTransaction transaction, Kullanicilar kullanici);
        int GetKullaniciIdByKullaniciAdi(SqlConnection connection, string kullaniciAdi);
        Kullanicilar KullaniciBilgiGetir(SqlConnection connection, string kullaniciAdi);
        bool KullaniciGuncelleKullaniciBilgi(SqlConnection connection, SqlTransaction transaction, Kullanicilar kullanici);
    }
}
