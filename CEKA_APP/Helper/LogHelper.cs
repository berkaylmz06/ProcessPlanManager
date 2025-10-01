using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEKA_APP.Entitys;

namespace CEKA_APP.Helper
{
    public static class LogHelper
    {
        public static Color GetIslemRenk(IslemTuru islem)
        {
            switch (islem)
            {
                case IslemTuru.Giriş:
                    return Color.DodgerBlue;
                case IslemTuru.Güncelle:
                    return Color.MediumSlateBlue;
                case IslemTuru.Sil:
                    return Color.Red;
                case IslemTuru.Ekle:
                    return Color.SeaGreen;
                case IslemTuru.KesimPlaniEklendi:
                    return Color.Green;
                case IslemTuru.XmlDosyasiOlusturuldu:
                    return Color.Orange;
                case IslemTuru.KesimPlaniKesildi:
                    return Color.Red;
                case IslemTuru.YerlesimPlaniSilindi:
                    return Color.OrangeRed;
                case IslemTuru.YerlesimPlaniIcerigiSilindi:
                    return Color.Tomato;
                case IslemTuru.PaftaYuklemesiYapildi:
                    return Color.BlueViolet;

                default:
                    return Color.Gray;
            }
        }

        public static string GetIslemEtiketi(IslemTuru islem)
        {
            return islem.ToString();
        }
    }
}
