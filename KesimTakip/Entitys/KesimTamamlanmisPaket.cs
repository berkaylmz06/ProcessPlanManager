using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesimTakip.Entitys
{
    public class KesimTamamlanmisPaket
    {
        public int id { get; set; }
        public string kesimYapan { get; set; }
        public int kesimId { get; set; }
        public int kesilmisPlanSayisi { get; set; }
        public string kesimTarihi { get; set; }
        public string kesimSaati { get; set; }
    }
}
