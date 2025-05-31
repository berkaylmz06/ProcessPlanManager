using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesimTakip.Entitys
{
    public class AutoCadAktarim
    {
        public string Grup { get; set; }
        public string PozNo { get; set; }
        public int Adet { get; set; }
        public string Ad { get; set; }
        public string Kalite { get; set; }
        public double? NetAgirlik { get; set; }
        public double? ToplamAgirlik
        {
            get { return NetAgirlik.HasValue ? (double?)Math.Round(Adet * NetAgirlik.Value, 2) : null; }
        }
        public int GrupAdet { get; set; }
    }
}
