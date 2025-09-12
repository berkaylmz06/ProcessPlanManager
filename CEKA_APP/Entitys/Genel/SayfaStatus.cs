using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.Genel
{
    public class SayfaStatus
    {
        public int sayfaStatusId { get; set; }
        public int sayfaId { get; set; }
        public int projeId { get; set; }
        public bool bilgilerTamamMi { get; set; }
        public string status { get; set; }
        public string nedenTamamlanmadi { get; set; }
    }
}
