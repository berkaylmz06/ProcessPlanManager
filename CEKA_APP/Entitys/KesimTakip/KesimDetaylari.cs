using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys
{
    public class KesimDetaylari
    {
        public int Id { get; set; }
        public string kalite { get; set; }
        public string malzeme { get; set; }
        public string malzemeKod{ get; set; }
        public string proje { get; set; }
        public string kesimId { get; set; }
        public decimal  kesilmisAdet { get; set; }
        public decimal kesilecekAdet { get; set; }
        public decimal toplamAdet { get; set; }
        public string Key { get; set; }
        public string poz
        {
            get { return $"{kalite}-{malzeme}-{malzemeKod}-{proje}"; }
        }
        public override string ToString()
        {
            return poz;
        }
        public bool ekBilgi { get; set; }
        public string ekBilgiMesaji { get; set; }
    }

}
