using System;

namespace CEKA_APP.Entitys
{
    public class AutoCadAktarimDetay
    {
        public string Proje { get; set; }
        public int UstGrupId { get; set; }
        public string UstGrup { get; set; }
        public string Grup { get; set; }
        public int GrupId { get; set; }
        public string MalzemeKod { get; set; }
        public int Adet { get; set; }
        public string MalzemeAd { get; set; }
        public string Kalite { get; set; }
        public Guid? YuklemeId { get; set; }
        public int OrjinalAdet { get; set; }
        public decimal NetAgirlik { get; set; }
        public int GrupAdet { get; set; }
    }
}
