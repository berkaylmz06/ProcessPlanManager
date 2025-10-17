using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.KesimTakip
{
    public class KesimSure
    {
        public int sureId { get; set; }
        public string kesimId { get; set; }
        public int toplamSureSaniye { get; set; }
        public DateTime baslamaTarihi { get; set; }
        public DateTime? bitisTarihi { get; set; }
        public string status { get; set; }
        public string kesimYapan { get; set; }
        public string lotNo { get; set; }
    }

}
