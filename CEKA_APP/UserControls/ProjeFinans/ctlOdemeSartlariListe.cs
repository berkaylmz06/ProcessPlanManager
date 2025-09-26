using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Properties;
using Microsoft.Extensions.DependencyInjection;
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
        private ToolStripMenuItem tsmiSutunSiralama;

        private readonly IServiceProvider _serviceProvider;
        private IOdemeSartlariService _odemeSartlariService => _serviceProvider.GetRequiredService<IOdemeSartlariService>();
        private IOdemeHareketleriService _odemeHareketleriService => _serviceProvider.GetRequiredService<IOdemeHareketleriService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();

        public ctlOdemeSartlariListe(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DataGridViewHelper.StilUygulaProjeFinans(dataGridOdemeSartlari);

            this.cmsOdemeSartlari = new ContextMenuStrip();
            tsmiAra = new ToolStripMenuItem
            {
                Name = "tsmiAra",
                Text = "Ara"
            };
            tsmiAra.Click += new EventHandler(tsmiAra_Click);

            tsmiSutunSiralama = new ToolStripMenuItem
            {
                Name = "tsmiSutunSiralama",
                Text = "Sütun Sıralama"
            };
            tsmiSutunSiralama.Click += new EventHandler(tsmiSutunSiralama_Click);

            this.cmsOdemeSartlari.Items.Add(tsmiAra);
            this.cmsOdemeSartlari.Items.Add(tsmiSutunSiralama);
            this.dataGridOdemeSartlari.ContextMenuStrip = this.cmsOdemeSartlari;

            dataGridOdemeSartlari.AutoGenerateColumns = true;
        }

        private void ctlOdemeSartlariListe_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Ödeme Şartları Liste";
            LoadOdemeSartlariToDataGridView();
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

                dataGridOdemeSartlari.DataSource = null; 
                dataGridOdemeSartlari.DataSource = odemeSekilleri;

                ConfigureDataGridColumns();
                dataGridOdemeSartlari.ScrollBars = ScrollBars.Both;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödeme şartları bilgileri yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void ConfigureDataGridColumns()
        {
            if (dataGridOdemeSartlari.Columns.Count == 0)
            {
                return;
            }

            dataGridOdemeSartlari.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            var sutunSirasi = Settings.Default.SutunSirasiOdemeSartlari != null ? StringCollectionToList(Settings.Default.SutunSirasiOdemeSartlari) : VarsayilanSutunSirasiniAl();

            dataGridOdemeSartlari.Columns.Clear();
            foreach (var sutunAdi in sutunSirasi)
            {
                if (!dataGridOdemeSartlari.Columns.Contains(sutunAdi))
                {
                    var sutun = new DataGridViewTextBoxColumn
                    {
                        Name = sutunAdi,
                        DataPropertyName = sutunAdi,
                        Visible = true 
                    };
                    dataGridOdemeSartlari.Columns.Add(sutun);
                }
                else
                {
                    dataGridOdemeSartlari.Columns[sutunAdi].Visible = true;
                    dataGridOdemeSartlari.Columns[sutunAdi].DataPropertyName = sutunAdi;
                }

                dataGridOdemeSartlari.Columns[sutunAdi].DisplayIndex = sutunSirasi.IndexOf(sutunAdi);

                switch (sutunAdi)
                {
                    case "projeNo":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Proje No";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "musteriNo":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Müşteri No";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 200;
                        break;
                    case "musteriAdi":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Müşteri Adı";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 200;
                        break;
                    case "projeAciklama":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Proje Açıklama";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 200;
                        break;
                    case "kilometreTasiAdi":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Kilometre Taşı";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 250;
                        break;
                    case "siralama":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Sıra";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 80;
                        break;
                    case "oran":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Oran(%)";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 100;
                        break;
                    case "tutar":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Tutar";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 120;
                        break;
                    case "paraBirimi":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Para Birimi";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 120;
                        break;
                    case "tahminiTarih":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Tahmini Tarih";
                        dataGridOdemeSartlari.Columns[sutunAdi].DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "gerceklesenTarih":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Gerçekleşen Tarih";
                        dataGridOdemeSartlari.Columns[sutunAdi].DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "odemeAciklama":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Ödeme Açıklama";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "teminatMektubu":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Teminat Mektubu Var Mı?";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "teminatDurumu":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Teminat Durumu";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "durum":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Durum";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "kalanTutar":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Kalan Tutar";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "odemeTarihi":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Ödeme Tarihi";
                        dataGridOdemeSartlari.Columns[sutunAdi].DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "odemeSapmasi":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Ödeme Sapması";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "faturaNo":
                        dataGridOdemeSartlari.Columns[sutunAdi].HeaderText = "Fatura No";
                        dataGridOdemeSartlari.Columns[sutunAdi].Width = 150;
                        break;
                    case "projeId":
                        dataGridOdemeSartlari.Columns[sutunAdi].Visible = false;
                        break;
                    case "odemeId":
                        dataGridOdemeSartlari.Columns[sutunAdi].Visible = false;
                        break;
                    case "kilometreTasiId":
                        dataGridOdemeSartlari.Columns[sutunAdi].Visible = false;
                        break;
                    case "status":
                        dataGridOdemeSartlari.Columns[sutunAdi].Visible = false;
                        break;
                }
            }
        }
        private void tsmiAra_Click(object sender, EventArgs e)
        {
            string baseSql = _odemeSartlariService.GetOdemeBilgileriQuery();
            var frm = new frmAra(
                dataGridOdemeSartlari,
                _tabloFiltreleService,
                (dt) =>
                {
                    dataGridOdemeSartlari.DataSource = dt;
                    ConfigureDataGridColumns();
                },
                baseSql,
                _serviceProvider,
                detayEkle: false
            );

            frm.ShowDialog();
        }


        private void tsmiSutunSiralama_Click(object sender, EventArgs e)
        {
            var mevcutSira = Settings.Default.SutunSirasiOdemeSartlari != null ? StringCollectionToList(Settings.Default.SutunSirasiOdemeSartlari) : VarsayilanSutunSirasiniAl();
            var gorunurSutunlar = mevcutSira.Where(sutun =>
                dataGridOdemeSartlari.Columns.Contains(sutun) &&
                dataGridOdemeSartlari.Columns[sutun].Visible).ToList();

            frmSutunSiralama sutunSiralamaFormu = new frmSutunSiralama(gorunurSutunlar);
            if (sutunSiralamaFormu.ShowDialog() == DialogResult.OK)
            {
                var yeniSira = sutunSiralamaFormu.SutunSirasiniAl();
                var tamSira = VarsayilanSutunSirasiniAl();
                var gorunmezSutunlar = tamSira.Except(yeniSira).ToList();
                yeniSira.AddRange(gorunmezSutunlar);

                Settings.Default.SutunSirasiOdemeSartlari = new System.Collections.Specialized.StringCollection();
                Settings.Default.SutunSirasiOdemeSartlari.AddRange(yeniSira.ToArray());
                Settings.Default.Save();
                LoadOdemeSartlariToDataGridView();
            }
        }
        private List<string> StringCollectionToList(System.Collections.Specialized.StringCollection collection)
        {
            var list = new List<string>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    list.Add(item);
                }
            }
            return list;
        }
        private List<string> VarsayilanSutunSirasiniAl()
        {
            return new List<string>
            {
                "projeNo", "musteriNo", "musteriAdi", "projeAciklama", "kilometreTasiAdi", "siralama",
                "oran", "tutar", "paraBirimi", "tahminiTarih", "gerceklesenTarih",
                "odemeAciklama", "teminatMektubu", "teminatDurumu", "durum",
                "faturaNo", "kalanTutar", "odemeTarihi",
                "projeId", "odemeId", "kilometreTasiId", "status"
            };
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
    }
}