using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl.ProjeFinans
{
    public partial class ctlOdemeSartlariListe : UserControl
    {
        private ToolStripMenuItem tsmiAra;
        private Dictionary<string, string> sonFiltreKriterleri = new Dictionary<string, string>();

        public ctlOdemeSartlariListe()
        {
            InitializeComponent();
            DataGridViewHelper.StilUygulaProjeFinans(dataGridOdemeSartlari);

            this.cmsOdemeSartlari = new ContextMenuStrip();
            tsmiAra = new ToolStripMenuItem
            {
                Name = "tsmiAra",
                Text = "Ara"
            };
            tsmiAra.Click += new EventHandler(tsmiAra_Click);
            this.cmsOdemeSartlari.Items.Add(tsmiAra);
            this.dataGridOdemeSartlari.ContextMenuStrip = this.cmsOdemeSartlari;

            dataGridOdemeSartlari.AutoGenerateColumns = true;
        }

        private void ctlOdemeSartlariListe_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Ödeme Şartları Liste";
            LoadOdemeSartlariToDataGridView();
            dataGridOdemeSartlari.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridOdemeSartlari_CellFormatting);
            dataGridOdemeSartlari.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridOdemeSartlari_DataBindingComplete);
        }

        private void dataGridOdemeSartlari_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridOdemeSartlari.Rows.Count > 0)
            {
                dataGridOdemeSartlari.ClearSelection();
                dataGridOdemeSartlari.CurrentCell = null;
            }
        }

        private void dataGridOdemeSartlari_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridOdemeSartlari.ClearSelection();
                dataGridOdemeSartlari.Rows[e.RowIndex].Selected = true;
                dataGridOdemeSartlari.CurrentCell = dataGridOdemeSartlari.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        public void LoadOdemeSartlariToDataGridView()
        {
            ProjeFinans_OdemeSartlariData odemeSekilleriData = new ProjeFinans_OdemeSartlariData();
            try
            {
                List<OdemeSartlari> odemeSekilleri = odemeSekilleriData.GetOdemeBilgileri();
                dataGridOdemeSartlari.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridOdemeSartlari.DataSource = odemeSekilleri;
                ConfigureDataGridColumns();
                dataGridOdemeSartlari.ScrollBars = ScrollBars.Both;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödeme şartları bilgileri yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuncelleDataGrid(DataTable sonucTablo)
        {
            dataGridOdemeSartlari.DataSource = null;
            dataGridOdemeSartlari.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridOdemeSartlari.DataSource = sonucTablo;

            ConfigureDataGridColumns();

            dataGridOdemeSartlari.ScrollBars = ScrollBars.Both;

            if (sonucTablo.Rows.Count == 0)
            {
                MessageBox.Show("Arama kriterlerine uygun veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConfigureDataGridColumns()
        {
            dataGridOdemeSartlari.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            if (dataGridOdemeSartlari.Columns.Contains("odemeId"))
                dataGridOdemeSartlari.Columns["odemeId"].Visible = false;
            if (dataGridOdemeSartlari.Columns.Contains("kilometreTasiId"))
                dataGridOdemeSartlari.Columns["kilometreTasiId"].Visible = false;

            if (dataGridOdemeSartlari.Columns.Contains("projeNo"))
            {
                dataGridOdemeSartlari.Columns["projeNo"].HeaderText = "Proje No";
                dataGridOdemeSartlari.Columns["projeNo"].DisplayIndex = 0;
                dataGridOdemeSartlari.Columns["projeNo"].Width = 150;
            }
            if (dataGridOdemeSartlari.Columns.Contains("musteriAdi"))
            {
                dataGridOdemeSartlari.Columns["musteriAdi"].HeaderText = "Müşteri Adı";
                dataGridOdemeSartlari.Columns["musteriAdi"].DisplayIndex = 1;
                dataGridOdemeSartlari.Columns["musteriAdi"].Width = 200;
            }
            if (dataGridOdemeSartlari.Columns.Contains("kilometreTasiAdi"))
            {
                dataGridOdemeSartlari.Columns["kilometreTasiAdi"].HeaderText = "Kilometre Taşı";
                dataGridOdemeSartlari.Columns["kilometreTasiAdi"].DisplayIndex = 2;
                dataGridOdemeSartlari.Columns["kilometreTasiAdi"].Width = 250;
            }
            if (dataGridOdemeSartlari.Columns.Contains("siralama"))
            {
                dataGridOdemeSartlari.Columns["siralama"].HeaderText = "Sıra";
                dataGridOdemeSartlari.Columns["siralama"].DisplayIndex = 3;
                dataGridOdemeSartlari.Columns["siralama"].Width = 80;
            }
            if (dataGridOdemeSartlari.Columns.Contains("oran"))
            {
                dataGridOdemeSartlari.Columns["oran"].HeaderText = "Oran(%)";
                dataGridOdemeSartlari.Columns["oran"].DisplayIndex = 4;
                dataGridOdemeSartlari.Columns["oran"].Width = 100;
            }
            if (dataGridOdemeSartlari.Columns.Contains("tutar"))
            {
                dataGridOdemeSartlari.Columns["tutar"].HeaderText = "Tutar";
                dataGridOdemeSartlari.Columns["tutar"].DisplayIndex = 5;
                dataGridOdemeSartlari.Columns["tutar"].Width = 120;
            }
            if (dataGridOdemeSartlari.Columns.Contains("tahminiTarih"))
            {
                dataGridOdemeSartlari.Columns["tahminiTarih"].HeaderText = "Tahmini Tarih";
                dataGridOdemeSartlari.Columns["tahminiTarih"].DisplayIndex = 6;
                dataGridOdemeSartlari.Columns["tahminiTarih"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dataGridOdemeSartlari.Columns["tahminiTarih"].Width = 150;
            }
            if (dataGridOdemeSartlari.Columns.Contains("gerceklesenTarih"))
            {
                dataGridOdemeSartlari.Columns["gerceklesenTarih"].HeaderText = "Gerçekleşen Tarih";
                dataGridOdemeSartlari.Columns["gerceklesenTarih"].DisplayIndex = 7;
                dataGridOdemeSartlari.Columns["gerceklesenTarih"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dataGridOdemeSartlari.Columns["gerceklesenTarih"].Width = 150;
            }
            if (dataGridOdemeSartlari.Columns.Contains("aciklama"))
            {
                dataGridOdemeSartlari.Columns["aciklama"].HeaderText = "Açıklama";
                dataGridOdemeSartlari.Columns["aciklama"].DisplayIndex = 8;
                dataGridOdemeSartlari.Columns["aciklama"].Width = 250;
            }
            if (dataGridOdemeSartlari.Columns.Contains("teminatMektubu"))
            {
                dataGridOdemeSartlari.Columns["teminatMektubu"].HeaderText = "Teminat Mektubu Var Mı?";
                dataGridOdemeSartlari.Columns["teminatMektubu"].DisplayIndex = 9;
                dataGridOdemeSartlari.Columns["teminatMektubu"].Width = 180;
            }
            if (dataGridOdemeSartlari.Columns.Contains("teminatDurumu"))
            {
                dataGridOdemeSartlari.Columns["teminatDurumu"].HeaderText = "Teminat Durumu";
                dataGridOdemeSartlari.Columns["teminatDurumu"].DisplayIndex = 10;
                dataGridOdemeSartlari.Columns["teminatDurumu"].Width = 180;
            }
            if (dataGridOdemeSartlari.Columns.Contains("durum"))
            {
                dataGridOdemeSartlari.Columns["durum"].HeaderText = "Durum";
                dataGridOdemeSartlari.Columns["durum"].DisplayIndex = 11;
                dataGridOdemeSartlari.Columns["durum"].Width = 120;
            }
            if (dataGridOdemeSartlari.Columns.Contains("kalanTutar"))
            {
                dataGridOdemeSartlari.Columns["kalanTutar"].HeaderText = "Kalan Tutar";
                dataGridOdemeSartlari.Columns["kalanTutar"].DisplayIndex = 12;
                dataGridOdemeSartlari.Columns["kalanTutar"].Width = 120;
            }
            if (dataGridOdemeSartlari.Columns.Contains("odemeTarihi"))
            {
                dataGridOdemeSartlari.Columns["odemeTarihi"].HeaderText = "Ödeme Tarihi";
                dataGridOdemeSartlari.Columns["odemeTarihi"].DisplayIndex = 13;
                dataGridOdemeSartlari.Columns["odemeTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dataGridOdemeSartlari.Columns["odemeTarihi"].Width = 150;
            }
            if (dataGridOdemeSartlari.Columns.Contains("faturaNo"))
            {
                dataGridOdemeSartlari.Columns["faturaNo"].HeaderText = "Fatura No";
                dataGridOdemeSartlari.Columns["faturaNo"].DisplayIndex = 14;
                dataGridOdemeSartlari.Columns["faturaNo"].Width = 120;
            }
            if (dataGridOdemeSartlari.Columns.Contains("OdemeSapmasi"))
            {
                dataGridOdemeSartlari.Columns["OdemeSapmasi"].HeaderText = "Ödeme Sapması (Gün)";
                dataGridOdemeSartlari.Columns["OdemeSapmasi"].DisplayIndex = 15;
                dataGridOdemeSartlari.Columns["OdemeSapmasi"].Width = 150;
            }
        }

        private void tsmiAra_Click(object sender, EventArgs e)
        {
            using (var tempGrid = new DataGridView())
            {
                foreach (DataGridViewColumn col in dataGridOdemeSartlari.Columns)
                {
                    if (col.Visible)
                    {
                        var clonedColumn = new DataGridViewColumn(col.CellTemplate)
                        {
                            Name = col.Name,
                            HeaderText = col.HeaderText,
                            Visible = true,
                            Width = col.Width,
                            DisplayIndex = col.DisplayIndex,
                            DefaultCellStyle = col.DefaultCellStyle != null ? new DataGridViewCellStyle(col.DefaultCellStyle) : null
                        };
                        tempGrid.Columns.Add(clonedColumn);
                    }
                }
                frmAra araForm = new frmAra(
                    tempGrid.Columns,
                    FiltreleOdemeSartlari,
                    GuncelleDataGrid,
                    false,
                    sonFiltreKriterleri
                );
                araForm.ShowDialog();
            }
        }

        private DataTable FiltreleOdemeSartlari(Dictionary<string, TextBox> filtreKutulari)
        {
            try
            {
                ProjeFinans_OdemeSartlariData odemeSartlariData = new ProjeFinans_OdemeSartlariData();
                sonFiltreKriterleri.Clear();
                foreach (var kutu in filtreKutulari)
                {
                    if (!string.IsNullOrEmpty(kutu.Value.Text.Trim()))
                    {
                        sonFiltreKriterleri[kutu.Key] = kutu.Value.Text.Trim();
                    }
                }
                return odemeSartlariData.FiltreleOdemeBilgileri(filtreKutulari, dataGridOdemeSartlari);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void dataGridOdemeSartlari_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridOdemeSartlari.RowCount)
            {
                return;
            }

            if (!dataGridOdemeSartlari.Columns.Contains("odemeId"))
            {
                MessageBox.Show("odemeId sütunu bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var odemeIdCell = dataGridOdemeSartlari.Rows[e.RowIndex].Cells["odemeId"].Value;
            if (odemeIdCell == null || odemeIdCell == DBNull.Value)
            {
                MessageBox.Show("Seçilen satırda geçerli bir 'odemeId' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(odemeIdCell.ToString(), out int odemeId))
            {
                MessageBox.Show($"Seçilen satırda 'odemeId' geçersiz: {odemeIdCell}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var odemeHareketleriData = new ProjeFinans_OdemeHareketleriData();
            List<OdemeHareketleri> hareketler = odemeHareketleriData.GetOdemeHareketleriByOdemeId(odemeId);

            if (hareketler != null && hareketler.Count > 0)
            {
                frmOdemeHareketleri odemeHareketleriForm = new frmOdemeHareketleri(hareketler);
                odemeHareketleriForm.ShowDialog();
            }
        }

        private void dataGridOdemeSartlari_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridOdemeSartlari.Columns[e.ColumnIndex].Name == "OdemeSapmasi")
            {
                var row = dataGridOdemeSartlari.Rows[e.RowIndex];
                var gerceklesenTarih = row.Cells["gerceklesenTarih"].Value;
                var odemeTarihi = row.Cells["odemeTarihi"].Value;

                if (gerceklesenTarih != null && odemeTarihi != null && gerceklesenTarih != DBNull.Value && odemeTarihi != DBNull.Value)
                {
                    try
                    {
                        DateTime gerceklesen = Convert.ToDateTime(gerceklesenTarih);
                        DateTime odeme = Convert.ToDateTime(odemeTarihi);
                        int sapma = (odeme - gerceklesen).Days;

                        if (sapma > 0)
                        {
                            e.Value = $"-{sapma}";
                            e.CellStyle.ForeColor = System.Drawing.Color.Red;
                        }
                        else if (sapma < 0)
                        {
                            e.Value = $"+{Math.Abs(sapma)}";
                            e.CellStyle.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            e.Value = "0";
                            e.CellStyle.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    catch
                    {
                        e.Value = "-";
                        e.CellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
                else
                {
                    e.Value = "-";
                    e.CellStyle.ForeColor = System.Drawing.Color.Black;
                }
            }
        }
    }
}