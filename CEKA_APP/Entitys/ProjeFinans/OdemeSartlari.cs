﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.ProjeFinans
{
    public class OdemeSartlari
    {
        public int odemeId { get; set; } 
        public string projeNo { get; set; }
        public int kilometreTasiId { get; set; }
        public string kilometreTasiAdi { get; set; } 
        public int siralama { get; set; }
        public decimal oran { get; set; }
        public decimal tutar { get; set; }
        public DateTime? tahminiTarih{ get; set; }
        public DateTime? gerceklesenTarih { get; set; } 
        public string aciklama { get; set; }
        public bool teminatMektubu { get; set; }
        public string teminatDurumu { get; set; }
        public string durum { get; set; }
        public decimal odenenTutar { get; set; } 
        public decimal kalanTutar { get; set; }
        public string faturaNo { get; set; }
    }
}
