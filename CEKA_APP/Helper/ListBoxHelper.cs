using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Helper
{
    public class ListBoxHelper
    {
        public static void StilUygula(ListBox listBox)
        {
            listBox.BorderStyle = BorderStyle.None;
            listBox.BackColor = Color.White;
            listBox.ForeColor = Color.Black;
            listBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            listBox.ItemHeight = 22;
            listBox.IntegralHeight = false;

            listBox.DrawMode = DrawMode.OwnerDrawFixed;
            listBox.DrawItem += (sender, e) =>
            {
                if (e.Index < 0) return;

                var lb = sender as ListBox;
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                e.Graphics.FillRectangle(
                    new SolidBrush(selected ? ColorTranslator.FromHtml("#51A3FF") : Color.White),
                    e.Bounds);

                TextRenderer.DrawText(
                    e.Graphics,
                    lb.Items[e.Index].ToString(),
                    lb.Font,
                    e.Bounds,
                    selected ? Color.WhiteSmoke : Color.Black,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            };
        }
    }
}
