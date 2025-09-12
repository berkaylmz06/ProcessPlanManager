using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys
{
    public class ProjeKutuk
    {
        public int projeKutukId { get; set; }
        public string musteriNo { get; set; }
        public string musteriAdi { get; set; }
        public string isFirsatiNo { get; set; }
        public int projeId { get; set; }
        public string projeNo { get; set; }
        public bool altProjeVarMi { get; set; }
        public string digerProjeIliskisiVarMi { get; set; } 
        public DateTime siparisSozlesmeTarihi { get; set; }
        public decimal toplamBedel { get; set; }
        public string paraBirimi { get; set; }
        public string faturalamaSekli { get; set; }
        public bool nakliyeVarMi { get; set; }
        public List<int> altProjeBilgileri { get; set; }
        public string ustProjeID { get; set; }
        public string altProjeID { get; set; }
        public bool montajTamamlandiMi { get; set; }
        public string status { get; set; }
    }
}
