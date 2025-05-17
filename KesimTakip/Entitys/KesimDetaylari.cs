using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesimTakip.Entitys
{
    public class KesimDetaylari
    {
        public int Id { get; set; }
        public string poz { get; set; }
        public string projeNo { get; set; }
        public string kesimId { get; set; }
        public int kesilmisAdet { get; set; }
        public int kesilecekAdet { get; set; }
        public int toplamAdet { get; set; }
        public override string ToString()
        {
            return poz; // Veya istediğiniz başka bir format: $"{PozNo} - {ToplamAdet}"
        }
    }

}
