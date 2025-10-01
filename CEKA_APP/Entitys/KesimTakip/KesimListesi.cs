using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys
{
    public class KesimListesi
    {
        public int id { get; set; }
        public string olusturan { get; set; }
        public string kesimId { get; set; }
        public string projeNo { get; set; }
        public string kalite { get; set; }
        public string malzeme { get; set; }
        public string kalipNo { get; set; }
        public string kesilecekPozlar { get; set; }
        public string kpAdetSayilari { get; set; }
        public DateTime eklemeTarihi { get; set; }
    }
}
