using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.ProjeTakip
{
    public class ProjeKarti
    {
        public int projeKartId { get; set; }
        public int projeId { get; set; }
        public string projeNo { get; set; }
        public string projeAdi { get; set; }
        public DateTime projeBasTarihi { get; set; }
        public DateTime projeBitisTarihi { get; set; }
        public string musteriNo { get; set; }
        public string musteriAdi { get; set; }
        public string projeMuhendisi { get; set; }
        public bool refProjeVarMi { get; set; }
        public string refProje { get; set; }

    }
}
