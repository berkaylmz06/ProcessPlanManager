using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys
{
    public class Duyurular
    {
        public int id { get; set; }
        public string kullanici { get; set; }
        public string duyuru { get; set; }
        public DateTime duyuruZamani { get; set; }
        public DateTime yayinlamaTarihi { get; set; }
    }
}
