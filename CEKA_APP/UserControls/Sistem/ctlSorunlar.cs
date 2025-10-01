using CEKA_APP.Entitys;
using CEKA_APP.Interfaces.Sistem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlSorunlar : UserControl
    {
        private int selectedRowIndex = -1;
        private List<SorunBildirimleri> sorunlar;
        private readonly IServiceProvider _serviceProvider;

        private ISorunBildirimleriService _sorunBildirimleriService => _serviceProvider.GetRequiredService<ISorunBildirimleriService>();
        public ctlSorunlar(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            sorunlar = _sorunBildirimleriService.GetSorunlar();
            dataGridSorunBildirimleri.DataSource = sorunlar;
        }
        private void ctlSorunlar_Load(object sender, EventArgs e)
        {
            List<SorunBildirimleri> sorunlar = _sorunBildirimleriService.GetSorunlar();

            dataGridSorunBildirimleri.DataSource = sorunlar;

            dataGridSorunBildirimleri.Columns[0].Visible = false;
            dataGridSorunBildirimleri.Columns[1].Visible = true;
            dataGridSorunBildirimleri.Columns[2].Visible = false;
            dataGridSorunBildirimleri.Columns[3].Visible = false;
            dataGridSorunBildirimleri.Columns[4].Visible = false;

            dataGridSorunBildirimleri.Columns[1].HeaderText = "Bildiri Ve Durum Bilgisi";

            dataGridSorunBildirimleri.ReadOnly = true;
            dataGridSorunBildirimleri.AllowUserToAddRows = false;
            dataGridSorunBildirimleri.AllowUserToDeleteRows = false;
            dataGridSorunBildirimleri.DefaultCellStyle.SelectionBackColor = dataGridSorunBildirimleri.DefaultCellStyle.BackColor;
            dataGridSorunBildirimleri.DefaultCellStyle.SelectionForeColor = dataGridSorunBildirimleri.DefaultCellStyle.ForeColor;

            ctlBaslik1.Baslik = "Sorunlar";
        }
        private void dataGridSorunBildirimleri_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedRowIndex = e.RowIndex;

                var selectedRow = dataGridSorunBildirimleri.Rows[e.RowIndex];

                int id = Convert.ToInt32(selectedRow.Cells["id"].Value);
                string kullanici = selectedRow.Cells["kullanici"].Value?.ToString();
                string bildiri = selectedRow.Cells["bildiri"].Value?.ToString();
                string bildirizamani = selectedRow.Cells["bildirizamani"].Value?.ToString();
                string okundu = selectedRow.Cells["okundu"].Value?.ToString();

                txtBildiriYapanKullanici.Text = kullanici;
                txtSorunBildirimi.Text = bildiri;
                txtBildiriZamani.Text = bildirizamani;

                if (okundu != "okundu")
                {
                    _sorunBildirimleriService.UpdateOkunduDurumu(id);
                }

                List<SorunBildirimleri> sorunlar = _sorunBildirimleriService.GetSorunlar();
                dataGridSorunBildirimleri.DataSource = sorunlar;

                if (selectedRowIndex >= 0 && selectedRowIndex < dataGridSorunBildirimleri.Rows.Count)
                {
                    dataGridSorunBildirimleri.Rows[selectedRowIndex].Selected = true;
                }
            }
        }
        private void dataGridSorunBildirimleri_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                string kullanici = dataGridSorunBildirimleri.Rows[e.RowIndex].Cells["kullanici"].Value?.ToString() ?? "";
                string okundu = dataGridSorunBildirimleri.Rows[e.RowIndex].Cells["okundu"].Value?.ToString() ?? "";

                e.Value = $"{kullanici} bir bildiri yayınladı - Durum: {(okundu == "okundu" ? "Okundu" : "Okunmadı")}";
            }
        }
    }
}
