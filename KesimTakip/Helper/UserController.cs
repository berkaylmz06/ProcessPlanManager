using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KesimTakip.DataBase;

namespace KesimTakip.Helper
{
    public class UserController
    {
        private string _kullaniciAdi;

        public UserController(string kullaniciAdi)
        {
            _kullaniciAdi = kullaniciAdi;
        }

        public void LogYap(string islemTuru, string sayfaAdi, string ekBilgi = "")
        {
            KullaniciHareketLogData.LogEkle(_kullaniciAdi, islemTuru, sayfaAdi, ekBilgi);
        }
    }

}
