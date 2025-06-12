using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys
{
    class KesimListesiPaket
    {
        public int id { get; set; }
        public string olusturan { get; set; }
        public string kesimId { get; set; }
        public int kesilecekPlanSayisi { get; set; }
        public int kesilmisPlanSayisi { get; set; }
        public int  toplamPlanTekrari { get; set; }
        public DateTime eklemeTarihi { get; set; }
    }
}
