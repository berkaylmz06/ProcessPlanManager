using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KesimTakip.Helper
{
    public static class ButonMakinaSecHelper
    {
        public static void ButonSekli(Button clickedButton, List<Button> allButtons)
        {
            foreach (var button in allButtons)
            {
                ApplyNeutralStyle(button);
            }

            // Seçili butonun stilini ayarla
            clickedButton.BackColor = System.Drawing.Color.LightSkyBlue;
            clickedButton.Font = new System.Drawing.Font(clickedButton.Font.FontFamily, 12);
            clickedButton.ForeColor = System.Drawing.Color.White;
            clickedButton.FlatStyle = FlatStyle.Flat;
            clickedButton.FlatAppearance.BorderSize = 0;
            clickedButton.Tag = true;

            clickedButton.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            clickedButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
            clickedButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;

            SetRoundedCorners(clickedButton, 10);
        }

        public static void NötrStilUygula(List<Button> allButtons)
        {
            foreach (var button in allButtons)
            {
                ApplyNeutralStyle(button);
            }
        }

        private static void ApplyNeutralStyle(Button button)
        {
            button.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            button.Font = new System.Drawing.Font(button.Font.FontFamily, 10);
            button.ForeColor = System.Drawing.Color.Black;
            button.Tag = false;
            button.FlatStyle = FlatStyle.Standard;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(180, 180, 180);
            button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSkyBlue;

            SetRoundedCorners(button, 10);
        }

        public static void SetRoundedCorners(Button button, int cornerRadius)
        {
            var path = new GraphicsPath();
            path.StartFigure();

            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(button.Width - cornerRadius - 1, 0, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(button.Width - cornerRadius - 1, button.Height - cornerRadius - 1, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(0, button.Height - cornerRadius - 1, cornerRadius, cornerRadius, 90, 90);

            path.CloseFigure();

            button.Region = new Region(path);
        }
    }
}