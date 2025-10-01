using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Helper
{
    public class DataGridViewHelper
    {
        public static void StilUygula(DataGridView dgv)
        {
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.ScrollBars = ScrollBars.Both;
            dgv.AllowUserToResizeRows = false;

            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#51A3FF"); 
            dgv.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;

            dgv.BackgroundColor = Color.White;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2C3E50");
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            dgv.RowHeadersVisible = false;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#51A3FF");
            dgv.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;

            dgv.RowTemplate.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

        }
        public static void StilUygulaKullaniciLog(DataGridView dgvLogs)
        {
            dgvLogs.RowTemplate.Height = 32;
            dgvLogs.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvLogs.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgvLogs.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvLogs.EnableHeadersVisualStyles = false;
            dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvLogs.BorderStyle = BorderStyle.None;
            dgvLogs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLogs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dgvLogs.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255);
            dgvLogs.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvLogs, new object[] { true });

        }
        public static void StilUygulaProjeOge(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = true;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 
            dgv.MultiSelect = false; 
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false; 

            dgv.BackgroundColor = ColorTranslator.FromHtml("#F5F6FA");
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false; 

            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2C3E50");
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold); 
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 35; 

            dgv.DefaultCellStyle.BackColor = Color.White; 
            dgv.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#2C3E50"); 
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9); 
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 
            dgv.DefaultCellStyle.Padding = new Padding(5);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E8ECEF");
            dgv.AlternatingRowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 

            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#74B9FF");
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            dgv.GridColor = ColorTranslator.FromHtml("#DCE3E8"); 
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; 

            dgv.AllowUserToOrderColumns = false; 
            dgv.AllowUserToResizeColumns = true; 
            dgv.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2; 

            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.RowTemplate.Height = 30;

            dgv.DefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.BackColor;
            dgv.DefaultCellStyle.SelectionForeColor = dgv.DefaultCellStyle.ForeColor;

            dgv.CellPainting -= Dgv_SeciliSatiraSadeceCerceveCiz;
            dgv.CellPainting += Dgv_SeciliSatiraSadeceCerceveCiz;
        }
        private static void Dgv_SeciliSatiraSadeceCerceveCiz(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var dgv = sender as DataGridView;
                var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                e.Handled = true;
                e.PaintBackground(e.ClipBounds, false);
                e.PaintContent(e.ClipBounds);

                if (dgv.Rows[e.RowIndex].Selected)
                {
                    using (Pen pen = new Pen(ColorTranslator.FromHtml("#2980B9"), 2))
                    {
                        Rectangle rect = e.CellBounds;
                        rect.Width -= 1;
                        rect.Height -= 1;
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                }
            }
        }
       
        public static void StilUygulaProjeFinans(DataGridView dgvMusteriler)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvMusteriler, new object[] { true });

            dgvMusteriler.RowTemplate.Height = 35; 
            dgvMusteriler.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvMusteriler.EnableHeadersVisualStyles = false; 

            dgvMusteriler.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold); 
            dgvMusteriler.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48); 
            dgvMusteriler.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; 
            dgvMusteriler.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None; 
            dgvMusteriler.ColumnHeadersHeight = 40;
            dgvMusteriler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvMusteriler.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvMusteriler.BorderStyle = BorderStyle.None; 
            dgvMusteriler.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; 
            dgvMusteriler.GridColor = Color.FromArgb(230, 230, 230); 
            dgvMusteriler.BackgroundColor = Color.White; 

            dgvMusteriler.DefaultCellStyle.BackColor = Color.White;
            dgvMusteriler.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            dgvMusteriler.DefaultCellStyle.SelectionBackColor = Color.FromArgb(29, 161, 242);
            dgvMusteriler.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvMusteriler.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; 

            dgvMusteriler.ReadOnly = true;
            dgvMusteriler.AllowUserToAddRows = false;
            dgvMusteriler.AllowUserToDeleteRows = false; 
            dgvMusteriler.AllowUserToResizeRows = false; 
            dgvMusteriler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
            dgvMusteriler.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 
        }
        public static void StilUygulaUrunGrubuSecim(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = true;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#34495E");
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 35;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.RowTemplate.Height = 30;

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F0F3F6");
            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#51A3FF");
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            dgv.GridColor = ColorTranslator.FromHtml("#DCE3E8");
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgv.DataBindingComplete += (s, e) =>
            {
                if (dgv.Columns.Count > 0)
                {
                    dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgv.Columns[0].Width = 30;

                    for (int i = 1; i < dgv.Columns.Count; i++)
                        dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            };
        }
    }
}
