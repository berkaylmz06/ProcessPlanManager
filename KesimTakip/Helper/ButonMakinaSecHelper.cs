using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace KesimTakip.Helper
{
    public static class ButonMakinaSecHelper
    {
        public static void ButonSekli(Button clickedButton, List<Button> allButtons)
        {
            foreach (var button in allButtons)
            {
                button.BackColor = System.Drawing.Color.FromArgb(224, 224, 224); // Açık gri renk
                button.Font = new System.Drawing.Font(button.Font.FontFamily, 10); // Yazı boyutunu büyüttük
                button.ForeColor = System.Drawing.Color.Black; // Yazı rengini siyah yaptık
                button.Tag = false;

                // Varsayılan buton stilini temizle
                button.FlatStyle = FlatStyle.Standard;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(180, 180, 180); // Düşük kontrast kenarlık rengi
                button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
                button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSkyBlue;

                // Başlangıçta tüm butonlar için yuvarlatılmış köşeleri ayarlayalım
                SetRoundedCorners(button, 10); // 10 px'lik yuvarlaklık
            }

            // Tıklanan buton için özel stil
            clickedButton.BackColor = System.Drawing.Color.LightSkyBlue;
            clickedButton.Font = new System.Drawing.Font(clickedButton.Font.FontFamily, 12); // Yazı boyutunu biraz daha büyüttük
            clickedButton.ForeColor = System.Drawing.Color.White; // Yazı rengini beyaz yaptık
            clickedButton.FlatStyle = FlatStyle.Flat;
            clickedButton.FlatAppearance.BorderSize = 0;
            clickedButton.Tag = true;

            // Seçili butonun stilini değiştirelim
            clickedButton.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            clickedButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
            clickedButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;

            // Seçili buton için yuvarlatılmış köşeler ekleyelim
            SetRoundedCorners(clickedButton, 10); // 10 px'lik yuvarlaklık
        }

        // Yuvarlatılmış köşe uygulama fonksiyonu
        private static void SetRoundedCorners(Button button, int cornerRadius)
        {
            var path = new GraphicsPath();
            path.StartFigure();

            // Üst sol köşe
            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            // Üst sağ köşe
            path.AddArc(button.Width - cornerRadius - 1, 0, cornerRadius, cornerRadius, 270, 90);
            // Alt sağ köşe
            path.AddArc(button.Width - cornerRadius - 1, button.Height - cornerRadius - 1, cornerRadius, cornerRadius, 0, 90);
            // Alt sol köşe
            path.AddArc(0, button.Height - cornerRadius - 1, cornerRadius, cornerRadius, 90, 90);

            path.CloseFigure();

            // Butonun şeklini region ile ayarlıyoruz
            button.Region = new Region(path);
        }
    }
}
