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
        }
    }
}
