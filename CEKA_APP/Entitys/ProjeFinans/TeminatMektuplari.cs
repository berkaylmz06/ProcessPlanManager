using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.ProjeFinans
{
    public class TeminatMektuplari
    {
        public string mektupNo { get; set; }
        public string musteriNo { get; set; }
        public string musteriAdi { get; set; }
        public string kilometreTasiAdi { get; set; } 
        public int? projeId{ get; set; }
        public int kilometreTasiId { get; set; }
        public string paraBirimi { get; set; }
        public decimal tutar { get; set; }
        public string banka { get; set; }
        public string mektupTuru { get; set; }
        public DateTime? vadeTarihi { get; set; }
        public DateTime iadeTarihi { get; set; }
        public decimal komisyonTutari { get; set; }
        public decimal komisyonOrani { get; set; }
        public int komisyonVadesi { get; set; }
        public string projeNo { get; set; }
        public string durum { get; set; }
    }
}
