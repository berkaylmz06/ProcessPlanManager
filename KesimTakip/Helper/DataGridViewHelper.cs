using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KesimTakip.Helper
{
    public class DataGridViewHelper
    {
        public static void StilUygula(DataGridView dgv)
        {
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.ScrollBars = ScrollBars.Both;
            dgv.AllowUserToResizeRows = false;

            dgv.BorderStyle = BorderStyle.None;
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

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
    }
}
