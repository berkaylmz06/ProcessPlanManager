using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.ProjeFinans
{
    public class Fiyatlandirma
    {
        public int fiyatlandirmaId { get; set; }
        public int projeId { get; set; }
        public int fiyatlandirmaKalemId { get; set; }
        public decimal teklifBirimMiktar { get; set; }
        public decimal teklifBirimFiyat { get; set; }
        public decimal gerceklesenBirimMiktar { get; set; }
        public decimal gerceklesenBirimFiyat { get; set; }
        public string teklifDovizKodu { get; set; }
        public decimal? teklifKuru { get; set; }
        public string status { get; set; }

        public FiyatlandirmaKalem kalem { get; set; }
    }
}
