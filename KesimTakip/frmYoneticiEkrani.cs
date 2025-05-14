using KesimTakip.DataBase;
using KesimTakip.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KesimTakip
{
    public partial class frmYoneticiEkrani : Form
    {
        private List<SorunBildirimleri> sorunlar;

        public frmYoneticiEkrani()
        {
            InitializeComponent();

            List<KesimListesi> products = KesimListesiData.GetKesimListesi();
            dataGridKesimListesi.DataSource = products;

            tabloDuzeni();

            sorunlar = SorunBildirimleriData.GetSorunlar();
            dataGridSorunBildirimleri.DataSource = sorunlar;

            PanelGoster(panelAnaSayfa);
        }
        private void PanelGoster(Panel target)
        {
            panelAnaSayfa.Visible = false;
            panelKullaniciAyar.Visible = false;
            panelSorunBildirimleri.Visible = false;
            panelKesimDetaylari.Visible = false;

            target.Visible = true;
        }
        public void tabloDuzeni()
        {
            dataGridKesimListesi.Columns[0].Visible = false;
            dataGridKesimListesi.Columns[1].HeaderText = "Oluşturan";
            dataGridKesimListesi.Columns[2].HeaderText = "Kesim ID";
            dataGridKesimListesi.Columns[3].HeaderText = "Proje No";
            dataGridKesimListesi.Columns[4].HeaderText = "Kalite";
            dataGridKesimListesi.Columns[5].HeaderText = "Kalınlık";
            dataGridKesimListesi.Columns[6].HeaderText = "Kalıp No";
            dataGridKesimListesi.Columns[7].HeaderText = "Kesilen Pozlar";
            dataGridKesimListesi.Columns[8].HeaderText = "Kesilen Pozların Adet Sayıları";
            dataGridKesimListesi.Columns[9].HeaderText = "Kesim Tarihi";
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnKullaniciEkle_Click(object sender, EventArgs e)
        {
            frmKullaniciEkle kulEkle = new frmKullaniciEkle();
            kulEkle.ShowDialog();
        }
        private void kullanıcıAyarlarıToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PanelGoster(panelKullaniciAyar);
        }

        private void anaSayfaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelGoster(panelAnaSayfa);
        }

        private void sorunBildirimeriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelGoster(panelSorunBildirimleri);
        }

        private int selectedRowIndex = -1;
        private void frmYoneticiEkrani_Load(object sender, EventArgs e)
        {
            List<SorunBildirimleri> sorunlar = SorunBildirimleriData.GetSorunlar();

            dataGridSorunBildirimleri.DataSource = sorunlar;

            dataGridSorunBildirimleri.Columns[0].Visible = false;
            dataGridSorunBildirimleri.Columns[1].Visible = true;
            dataGridSorunBildirimleri.Columns[2].Visible = false;
            dataGridSorunBildirimleri.Columns[3].Visible = false;
            dataGridSorunBildirimleri.Columns[4].Visible = false;

            dataGridSorunBildirimleri.Columns[1].HeaderText = "Bildiri Ve Durum Bilgisi";

            dataGridSorunBildirimleri.CellFormatting += dataGridSorunBildirimleri_CellFormatting;

            dataGridSorunBildirimleri.ReadOnly = true;
            dataGridSorunBildirimleri.AllowUserToAddRows = false;
            dataGridSorunBildirimleri.AllowUserToDeleteRows = false;
            dataGridSorunBildirimleri.DefaultCellStyle.SelectionBackColor = dataGridSorunBildirimleri.DefaultCellStyle.BackColor;
            dataGridSorunBildirimleri.DefaultCellStyle.SelectionForeColor = dataGridSorunBildirimleri.DefaultCellStyle.ForeColor;

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
                    SorunBildirimleriData.UpdateOkunduDurumu(id);
                }

                List<SorunBildirimleri> sorunlar = SorunBildirimleriData.GetSorunlar();
                dataGridSorunBildirimleri.DataSource = sorunlar;

                if (selectedRowIndex >= 0 && selectedRowIndex < dataGridSorunBildirimleri.Rows.Count)
                {
                    dataGridSorunBildirimleri.Rows[selectedRowIndex].Selected = true;
                }
            }
        }

        private void btnKullaniciAyarlari_Click(object sender, EventArgs e)
        {
            PanelGoster(panelKullaniciAyar);
        }



        private void btnSorunlar_Click(object sender, EventArgs e)
        {
            PanelGoster(panelSorunBildirimleri);
        }

        private void btnKesimDetaylari_Click(object sender, EventArgs e)
        {
            PanelGoster(panelKesimDetaylari);
        }
    }
}
