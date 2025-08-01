using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP
{
    public class Kullanicilar
    {
        public int id { get; set; }
        public string adSoyad { get; set; }
        public string kullaniciAdi { get; set; }
        public string  sifre { get; set; }
        public string kullaniciRol { get; set; }
        public string email { get; set; }
        public override string ToString()
        {
            return adSoyad;
        }
    }
}
