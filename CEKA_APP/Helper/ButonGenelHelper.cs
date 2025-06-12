using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Helper
{
    public static class ButonGenelHelper
    {
        public static void StilUygula(Button btn)
        {
            btn.BackColor = ColorTranslator.FromHtml("#2C3E50"); 

            btn.ForeColor = ColorTranslator.FromHtml("#ECF0F1");

            btn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#E67E22");

            btn.FlatAppearance.BorderSize = 1;

            btn.FlatStyle = FlatStyle.Flat;

            btn.MouseEnter += (sender, e) =>
            {
                btn.BackColor = ColorTranslator.FromHtml("#34495E");
            };
            btn.MouseLeave += (sender, e) =>
            {
                btn.BackColor = ColorTranslator.FromHtml("#2C3E50"); 
            };
        }
        public static void TuruncuZeminButonStilUygula(Button btn)
        {
            btn.BackColor = ColorTranslator.FromHtml("#E67E22");

            btn.ForeColor = ColorTranslator.FromHtml("#2C3E50");

            btn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#2C3E50");

            btn.FlatAppearance.BorderSize = 1;

            btn.FlatStyle = FlatStyle.Flat;

            btn.MouseEnter += (sender, e) =>
            {
                btn.BackColor = ColorTranslator.FromHtml("#D35400"); 
            };
            btn.MouseLeave += (sender, e) =>
            {
                btn.BackColor = ColorTranslator.FromHtml("#E67E22");
            };
        }
        public static void KullaniciEkleButonAyari(Button btn)
        {
            btn.BackColor = ColorTranslator.FromHtml("#3498DB");

            btn.ForeColor = ColorTranslator.FromHtml("#FFFFFF");

            btn.FlatStyle = FlatStyle.Flat;

            btn.FlatAppearance.BorderSize = 0;
            
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            
            btn.Cursor = Cursors.Hand;
            
            btn.TabStop = false;
        }
    }
}
