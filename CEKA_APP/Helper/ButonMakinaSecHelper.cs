using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CEKA_APP.Helper
{
    public static class ButonMakinaSecHelper
    {
        public static void ButonSekli(Button clickedButton, List<Button> allButtons)
        {
            foreach (var button in allButtons)
            {
                ButonNormal(button);
            }

            clickedButton.BackColor = ColorTranslator.FromHtml("#E67E22");
            clickedButton.Font = new System.Drawing.Font(clickedButton.Font.FontFamily, 12);
            clickedButton.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            clickedButton.FlatStyle = FlatStyle.Flat;
            clickedButton.FlatAppearance.BorderSize = 1;
            clickedButton.Tag = true;

            clickedButton.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#2C3E50");
            clickedButton.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#34495E");
            clickedButton.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#2C3E50");

        }

        public static void NötrStilUygula(List<Button> allButtons)
        {
            foreach (var button in allButtons)
            {
                ButonNormal(button);
            }
        }

        private static void ButonNormal(Button button)
        {
            button.BackColor = ColorTranslator.FromHtml("#2C3E50");
            button.Font = new System.Drawing.Font(button.Font.FontFamily, 10);
            button.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            button.Tag = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BDC3C7");
            button.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#2C3E50");
            button.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#2C3E50");

        }

        
    }
}
