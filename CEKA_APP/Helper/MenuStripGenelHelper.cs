using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Helper
{
    public static class MenuStripGenelHelper
    {
        public static void StilUygula(MenuStrip menu)
        {
            menu.Renderer = new ToolStripProfessionalRenderer(new MenuStripColorTable());
            menu.BackColor = ColorTranslator.FromHtml("#E67E22");

            foreach (ToolStripMenuItem item in menu.Items)
            {
                item.ForeColor = Color.White;
                item.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                StilAltÖğeleriUygula(item); 
            }
        }
        private static void StilAltÖğeleriUygula(ToolStripMenuItem parent)
        {
            foreach (ToolStripItem subItem in parent.DropDownItems)
            {
                subItem.ForeColor = Color.White;
                subItem.Font = new Font("Segoe UI", 9);
                subItem.BackColor = ColorTranslator.FromHtml("#34495E");

                if (subItem is ToolStripMenuItem subMenuItem && subMenuItem.HasDropDownItems)
                {
                    StilAltÖğeleriUygula(subMenuItem);
                }
            }
        }
    }

    public class MenuStripColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => ColorTranslator.FromHtml("#D35400");
        public override Color MenuItemBorder => Color.Transparent;
        public override Color ToolStripDropDownBackground => ColorTranslator.FromHtml("#E67E22");
        public override Color MenuItemSelectedGradientBegin => ColorTranslator.FromHtml("#D35400");
        public override Color MenuItemSelectedGradientEnd => ColorTranslator.FromHtml("#D35400");
        public override Color MenuItemPressedGradientBegin => ColorTranslator.FromHtml("#E67E22"); 
        public override Color MenuItemPressedGradientMiddle => ColorTranslator.FromHtml("#E67E22");
        public override Color MenuItemPressedGradientEnd => ColorTranslator.FromHtml("#E67E22");
        public override Color ImageMarginGradientBegin => ColorTranslator.FromHtml("#E67E22");
        public override Color ImageMarginGradientMiddle => ColorTranslator.FromHtml("#E67E22");
        public override Color ImageMarginGradientEnd => ColorTranslator.FromHtml("#E67E22");
    }
}
