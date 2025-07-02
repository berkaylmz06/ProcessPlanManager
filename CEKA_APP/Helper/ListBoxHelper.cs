using System.Drawing;
using System.Windows.Forms;

namespace CEKA_APP.Helper
{
    public static class ListBoxHelper
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
                if (e.Index < 0 || e.Index >= listBox.Items.Count) return;

                var lb = sender as ListBox;
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                e.Graphics.FillRectangle(
                    new SolidBrush(selected ? ColorTranslator.FromHtml("#51A3FF") : Color.White),
                    e.Bounds);

                string text = lb.Items[e.Index].ToString();
                int unlemGenislik = 20;
                Rectangle textRect = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width - unlemGenislik - 5, e.Bounds.Height);

                TextRenderer.DrawText(
                    e.Graphics,
                    text,
                    lb.Font,
                    textRect,
                    selected ? Color.WhiteSmoke : Color.Black,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

                if (lb.Items[e.Index] is CEKA_APP.Entitys.KesimDetaylari veri && veri.ekBilgi)
                {
                    Rectangle unlemRect = new Rectangle(e.Bounds.Right - unlemGenislik - 5, e.Bounds.Top, unlemGenislik, e.Bounds.Height);
                    TextRenderer.DrawText(
                        e.Graphics,
                        "!",
                        new Font("Arial", 12, FontStyle.Bold),
                        unlemRect,
                        Color.Red,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
                }
            };
        }
    }
}