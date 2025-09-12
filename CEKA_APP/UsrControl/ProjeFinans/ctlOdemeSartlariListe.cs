using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.ProjeFinans;
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
        private readonly IOdemeSartlariService _odemeSartlariService;
        private readonly IOdemeHareketleriService _odemeHareketleriService;
        public ctlOdemeSartlariListe(IOdemeSartlariService odemeSartlariService, IOdemeHareketleriService odemeHareketleriService)
        {
            InitializeComponent();

            _odemeSartlariService = odemeSartlariService ?? throw new ArgumentNullException(nameof(odemeSartlariService));
            _odemeHareketleriService = odemeHareketleriService ?? throw new ArgumentNullException(nameof(odemeHareketleriService));

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
            try
            {
                List<OdemeSartlari> odemeSekilleri = _odemeSartlariService.GetOdemeBilgileri();
                dataGridOdemeSartlari.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                // Filtreleme işlemini `FiltreleOdemeBilgileri`'nden döndürülen DataTable yerine doğrudan liste üzerinden yapın.
                dataGridOdemeSartlari.DataSource = null; // Eski veri kaynağını temizle
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
            dataGridOdemeSartlari.Refresh();

            if (sonucTablo.Rows.Count == 0)
            {
                MessageBox.Show("Arama kriterlerine uygun veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ConfigureDataGridColumns()
        {
            if (dataGridOdemeSartlari.Columns.Count == 0)
            {
                return;
            }

            dataGridOdemeSartlari.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            var columnOrder = new List<string>
    {
        "projeNo",
        "musteriAdi",
        "projeAciklama",
        "kilometreTasiAdi",
        "siralama",
        "oran",
        "tutar",
        "paraBirimi",
        "tahminiTarih",
        "gerceklesenTarih",
        "odemeAciklama",
        "teminatMektubu",
        "teminatDurumu",
        "durum",
        "faturaNo",
        "kalanTutar",
        "odemeTarihi",
        "odemeSapmasi",
        "projeId",
        "odemeId",
        "kilometreTasiId",
        "status"
    };

            dataGridOdemeSartlari.Columns.Clear();
            foreach (var columnName in columnOrder)
            {
                if (!dataGridOdemeSartlari.Columns.Contains(columnName))
                {
                    var column = new DataGridViewTextBoxColumn
                    {
                        Name = columnName,
                        DataPropertyName = columnName,
                        Visible = true
                    };
                    dataGridOdemeSartlari.Columns.Add(column);
                }
                else
                {
                    dataGridOdemeSartlari.Columns[columnName].Visible = true;
                    dataGridOdemeSartlari.Columns[columnName].DataPropertyName = columnName;
                }

                dataGridOdemeSartlari.Columns[columnName].DisplayIndex = columnOrder.IndexOf(columnName);

                switch (columnName)
                {
                    case "projeNo":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Proje No";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "musteriAdi":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Müşteri Adı";
                        dataGridOdemeSartlari.Columns[columnName].Width = 200;
                        break;
                    case "projeAciklama":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Proje Açıklama";
                        dataGridOdemeSartlari.Columns[columnName].Width = 200;
                        break;
                    case "kilometreTasiAdi":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Kilometre Taşı";
                        dataGridOdemeSartlari.Columns[columnName].Width = 250;
                        break;
                    case "siralama":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Sıra";
                        dataGridOdemeSartlari.Columns[columnName].Width = 80;
                        break;
                    case "oran":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Oran(%)";
                        dataGridOdemeSartlari.Columns[columnName].Width = 100;
                        break;
                    case "tutar":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Tutar";
                        dataGridOdemeSartlari.Columns[columnName].Width = 120;
                        break;
                    case "paraBirimi":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Para Birimi";
                        dataGridOdemeSartlari.Columns[columnName].Width = 120;
                        break;
                    case "tahminiTarih":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Tahmini Tarih";
                        dataGridOdemeSartlari.Columns[columnName].DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "gerceklesenTarih":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Gerçekleşen Tarih";
                        dataGridOdemeSartlari.Columns[columnName].DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "odemeAciklama":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Ödeme Açıklama";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "teminatMektubu":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Teminat Mektubu Var Mı?";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "teminatDurumu":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Teminat Durumu";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "durum":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Durum";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "kalanTutar":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Kalan Tutar";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "odemeTarihi":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Ödeme Tarihi";
                        dataGridOdemeSartlari.Columns[columnName].DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "faturaNo":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Fatura No";
                        dataGridOdemeSartlari.Columns[columnName].Width = 150;
                        break;
                    case "odemeSapmasi":
                        dataGridOdemeSartlari.Columns[columnName].HeaderText = "Ödeme Sapması (Gün)";
                        dataGridOdemeSartlari.Columns[columnName].Width = 100;
                        break;
                    case "projeId":
                        dataGridOdemeSartlari.Columns[columnName].Visible = false;
                        break;
                    case "odemeId":
                        dataGridOdemeSartlari.Columns[columnName].Visible = false;
                        break;
                    case "kilometreTasiId":
                        dataGridOdemeSartlari.Columns[columnName].Visible = false;
                        break;
                    case "status":
                        dataGridOdemeSartlari.Columns[columnName].Visible = false;
                        break;
                }
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
                            DefaultCellStyle = col.DefaultCellStyle != null ? new DataGridViewCellStyle(col.DefaultCellStyle) : null,
                            DataPropertyName = col.DataPropertyName
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
                sonFiltreKriterleri.Clear();
                foreach (var kutu in filtreKutulari)
                {
                    if (!string.IsNullOrEmpty(kutu.Value.Text.Trim()))
                    {
                        sonFiltreKriterleri[kutu.Key] = kutu.Value.Text.Trim();
                    }
                }
                return _odemeSartlariService.FiltreleOdemeBilgileri(filtreKutulari, dataGridOdemeSartlari);
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

            List<OdemeHareketleri> hareketler = _odemeHareketleriService.GetOdemeHareketleriByOdemeId(odemeId);

            if (hareketler != null && hareketler.Count > 0)
            {
                frmOdemeHareketleri odemeHareketleriForm = new frmOdemeHareketleri(hareketler);
                odemeHareketleriForm.ShowDialog();
            }
        }

        private void dataGridOdemeSartlari_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridOdemeSartlari.Columns[e.ColumnIndex].Name == "odemeSapmasi")
            {
                var value = dataGridOdemeSartlari.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (value != null && value != DBNull.Value)
                {
                    if (int.TryParse(value.ToString(), out int sapma))
                    {
                        if (sapma > 0)
                        {
                            e.Value = $"+{sapma}";
                            e.CellStyle.ForeColor = System.Drawing.Color.Green;
                        }
                        else if (sapma < 0)
                        {
                            e.Value = $"{sapma}";
                            e.CellStyle.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            e.Value = "0";
                            e.CellStyle.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
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