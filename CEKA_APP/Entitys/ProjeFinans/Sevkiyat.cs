using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.ProjeFinans
{
    public class Sevkiyat
    {
        public string projeNo { get; set; }
        public int sevkiyatPaketId { get; set; }
        public string paketAdi { get; set; }
        public string sevkId { get; set; }
        public string tasimaBilgileri { get; set; }
        public string satisSiparisNo { get; set; }
        public string irsaliyeNo { get; set; }
        public DateTime? aracSevkTarihi { get; set; }
        public decimal agirlik { get; set; }
        public decimal faturaToplami { get; set; }
        public string faturaNo { get; set; }
    }
}
