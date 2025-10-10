using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
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
        private readonly IServiceProvider _serviceProvider;
        private IOdemeSartlariService _odemeSartlariService => _serviceProvider.GetRequiredService<IOdemeSartlariService>();
        private IOdemeHareketleriService _odemeHareketleriService => _serviceProvider.GetRequiredService<IOdemeHareketleriService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();

        public ctlOdemeSartlariListe(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DataGridViewHelper.StilUygulaProjeFinans(dataGridOdemeSartlari);

            dataGridOdemeSartlari.AutoGenerateColumns = false;
        }

        private void ctlOdemeSartlariListe_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Ödeme Şartları Liste";
            LoadOdemeSartlariToDataGridView();
            DataGridViewSettingsManager.LoadColumnWidths(dataGridOdemeSartlari, "SutunGenislikOdemeSartlariListe");
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
                dataGridOdemeSartlari.DataSource = new SiralabilirBindingList<OdemeSartlari>(odemeSekilleri);

                ConfigureDataGridColumns();
                
                dataGridOdemeSartlari.ScrollBars = ScrollBars.Both;
                DataGridViewSettingsManager.LoadColumnWidths(dataGridOdemeSartlari, "SutunGenislikOdemeSartlariListe");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödeme şartları bilgileri yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ConfigureDataGridColumns()
        {
            var varsayilanSutunlar = VarsayilanSutunSirasiniAl();

            var sutunSirasi = Settings.Default.SutunSirasiOdemeSartlari != null
                ? StringCollectionToList(Settings.Default.SutunSirasiOdemeSartlari)
                : varsayilanSutunlar.ToList();

            foreach (var sutun in varsayilanSutunlar)
            {
                if (!sutunSirasi.Contains(sutun))
                {
                    sutunSirasi.Add(sutun); 
                }
            }

            sutunSirasi.RemoveAll(sutun => !varsayilanSutunlar.Contains(sutun));

            Settings.Default.SutunSirasiOdemeSartlari = new System.Collections.Specialized.StringCollection();
            Settings.Default.SutunSirasiOdemeSartlari.AddRange(sutunSirasi.ToArray());
            Settings.Default.Save();

            dataGridOdemeSartlari.SuspendLayout();
            try
            {
                for (int i = 0; i < sutunSirasi.Count; i++)
                {
                    var sutunAdi = sutunSirasi[i];
                    DataGridViewColumn col;

                    if (!dataGridOdemeSartlari.Columns.Contains(sutunAdi))
                    {
                        col = new DataGridViewTextBoxColumn
                        {
                            Name = sutunAdi,
                            DataPropertyName = sutunAdi,
                            Visible = true,
                            MinimumWidth = 50
                        };
                        dataGridOdemeSartlari.Columns.Add(col);
                    }
                    else
                    {
                        col = dataGridOdemeSartlari.Columns[sutunAdi];
                        col.DataPropertyName = sutunAdi;
                        col.Visible = true;
                        col.MinimumWidth = 50;
                    }

                    switch (sutunAdi)
                    {
                        case "projeNo": col.HeaderText = "Proje No"; break;
                        case "musteriNo": col.HeaderText = "Müşteri No"; break;
                        case "musteriAdi": col.HeaderText = "Müşteri Adı"; break;
                        case "projeAciklama": col.HeaderText = "Proje Açıklama"; break;
                        case "kilometreTasiAdi": col.HeaderText = "Kilometre Taşı"; break;
                        case "siralama": col.HeaderText = "Sıra"; break;
                        case "oran": col.HeaderText = "Oran(%)"; break;
                        case "tutar": col.HeaderText = "Tutar"; break;
                        case "paraBirimi": col.HeaderText = "Para Birimi"; break;
                        case "tahminiTarih":
                            col.HeaderText = "Tahmini Tarih";
                            col.DefaultCellStyle.Format = "dd.MM.yyyy";
                            break;
                        case "gerceklesenTarih":
                            col.HeaderText = "Gerçekleşen Tarih";
                            col.DefaultCellStyle.Format = "dd.MM.yyyy";
                            break;
                        case "odemeAciklama": col.HeaderText = "Ödeme Açıklama"; break;
                        case "teminatMektubu": col.HeaderText = "Teminat Mektubu Var Mı?"; break;
                        case "teminatDurumu": col.HeaderText = "Teminat Durumu"; break;
                        case "durum": col.HeaderText = "Durum"; break;
                        case "kalanTutar": col.HeaderText = "Kalan Tutar"; break;
                        case "odemeTarihi":
                            col.HeaderText = "Ödeme Tarihi";
                            col.DefaultCellStyle.Format = "dd.MM.yyyy";
                            break;
                        case "odemeSapmasi": col.HeaderText = "Ödeme Sapması"; break;
                        case "faturaNo": col.HeaderText = "Fatura No"; break;
                        case "projeId":
                        case "odemeId":
                        case "kilometreTasiId":
                        case "status":
                            col.Visible = false; break;
                    }

                    col.DisplayIndex = i;
                }

                foreach (DataGridViewColumn c in dataGridOdemeSartlari.Columns)
                {
                    if (!sutunSirasi.Contains(c.Name))
                        c.Visible = false;
                }
            }
            finally
            {
                dataGridOdemeSartlari.ResumeLayout();
            }

            DataGridViewSettingsManager.LoadColumnWidths(dataGridOdemeSartlari, "SutunGenislikOdemeSartlariListe");
        }
        private void dataGridOdemeSartlari_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridViewSettingsManager.SaveColumnWidths(dataGridOdemeSartlari, "SutunGenislikOdemeSartlariListe");
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
                    DataGridViewSettingsManager.LoadColumnWidths(dataGridOdemeSartlari, "SutunGenislikOdemeSartlariListe");
                },
                baseSql,
                _serviceProvider
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
                "projeId", "odemeId", "kilometreTasiId", "status", "odemeSapmasi"
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