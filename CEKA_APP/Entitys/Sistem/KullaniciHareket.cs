using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys
{
    public class KullaniciHareket
    {
        public int kullaniciId { get; set; }
        public IslemTuru islem { get; set; }
        public string sayfaAdi { get; set; }
        public DateTime tarihSaat { get; set; }
        public string ekBilgi { get; set; }
    }
    public enum IslemTuru
    {
        Giriş,
        Güncelle,
        Sil,
        Ekle,
        KesimPlaniEklendi,
        XmlDosyasiOlusturuldu,
        KesimPlaniKesildi,
        YerlesimPlaniSilindi,
        YerlesimPlaniIcerigiSilindi,
        PaftaYuklemesiYapildi
    }
}
