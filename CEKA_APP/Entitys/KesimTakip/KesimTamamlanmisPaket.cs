using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys
{
    public class KesimTamamlanmisPaket
    {
        public int id { get; set; }
        public string kesimYapan { get; set; }
        public int kesimId { get; set; }
        public int kesilmisPlanSayisi { get; set; }
        public string kesilenLot { get; set; }
        public DateTime kesimTarihi { get; set; }
        public TimeSpan kesimSaati { get; set; }
    }
}
