using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.ProjeFinans
{
    public class OdemeHareketleri
    {
        public int odemeHareketId { get; set; }
        public int odemeId { get; set; }
        public decimal odemeMiktari { get; set; }
        public DateTime odemeTarihi { get; set; }
        public string odemeAciklama { get; set; }
    }
}
