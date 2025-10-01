using System;
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
        public static void ProjeFinansStillUygula(ListBox listBox)
        {
            if (listBox == null) throw new ArgumentNullException(nameof(listBox));

            listBox.DrawMode = DrawMode.OwnerDrawVariable;
            listBox.HorizontalScrollbar = false;
            listBox.BorderStyle = BorderStyle.None;
            listBox.BackColor = Color.LightGray;
            listBox.ForeColor = Color.Black;
            listBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            listBox.IntegralHeight = false;

            listBox.MeasureItem += (sender, e) =>
            {
                if (e.Index < 0 || e.Index >= listBox.Items.Count) return;
                var text = listBox.Items[e.Index]?.ToString() ?? string.Empty;
                int width = listBox.ClientSize.Width - 5;
                Size textSize = TextRenderer.MeasureText(text, listBox.Font, new Size(width, int.MaxValue), TextFormatFlags.WordBreak);
                e.ItemHeight = textSize.Height + 6;
            };

            listBox.DrawItem += (sender, e) =>
            {
                if (e.Index < 0 || e.Index >= listBox.Items.Count) return;
                var lb = sender as ListBox;
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                // Arka plan
                using (var bgBrush = new SolidBrush(selected ? Color.FromArgb(81, 163, 255) : Color.LightGray))
                {
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);
                }

                // Metin
                string text = lb.Items[e.Index]?.ToString() ?? string.Empty;
                Rectangle textRect = new Rectangle(e.Bounds.Left + 3, e.Bounds.Top + 3, e.Bounds.Width - 6, e.Bounds.Height - 6);
                TextRenderer.DrawText(
                    e.Graphics,
                    text,
                    lb.Font,
                    textRect,
                    selected ? Color.White : Color.Black,
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak);

                // Satır alt çizgisi
                using (var pen = new Pen(Color.Gray)) // çizgi rengi
                {
                    e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
                }
            };

            listBox.Paint += (sender, e) =>
            {
                var lb = sender as ListBox;
                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray), lb.ClientRectangle);
            };
        }
    }
}