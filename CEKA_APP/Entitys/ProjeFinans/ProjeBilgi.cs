using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.ProjeFinans
{
    public class ProjeBilgi
    {
        public int projeId { get; set; }
        public string ProjeNo { get; set; }
        public string ProjeTipi { get; set; }
        public string ProjeAdi { get; set; }
        public string Aciklama { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }
}
