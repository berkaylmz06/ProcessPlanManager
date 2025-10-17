using CEKA_APP.Abstracts;
using CEKA_APP.Forms;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Properties;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlYapilanKesimleriGor : UserControl
    {
        private IKullaniciAdiOgren _kullaniciAdi;
        private readonly IServiceProvider _serviceProvider;

        private IKesimListesiService _kesimListesiService => _serviceProvider.GetRequiredService<IKesimListesiService>();
        private IKesimTamamlanmisService _kesimTamamlanmisService => _serviceProvider.GetRequiredService<IKesimTamamlanmisService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();
        private IKesimSureService _kesimSureService => _serviceProvider.GetRequiredService<IKesimSureService>();

        public ctlYapilanKesimleriGor(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DataGridViewHelper.StilUygula(dataGridViewTamamlanmisKesimListesi);
            DataGridViewHelper.StilUygula(dataGridTamamlanmisDetay);
            DataGridViewHelper.StilUygula(dataGridViewTamamlanmisHareket);

            LoadKesimListesiToDataGridView();
            ConfigureDataGridColumns(dataGridViewTamamlanmisKesimListesi);
            DataGridViewSettingsManager.LoadColumnWidths(dataGridViewTamamlanmisKesimListesi, "SutunGenislikYapilanKesimler");

            tabloDuzenleDetay();
            tabloDuzenleHareket();
        }

        private void dataGridViewTamamlanmisKesimListesi_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTest = dataGridViewTamamlanmisKesimListesi.HitTest(e.X, e.Y);
                if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
                {
                    contextMenuStrip1.Show(dataGridViewTamamlanmisKesimListesi, e.Location);
                }
            }
        }

        private void tsmiSutunSiralama_Click(object sender, EventArgs e)
        {
            var mevcutSira = Settings.Default.SutunSirasiYapilanKesimler != null
                ? StringCollectionToList(Settings.Default.SutunSirasiYapilanKesimler)
                : GetVarsayilanSutunSirasiniKesimListesi();
            var gorunurSutunlar = mevcutSira.Where(sutun =>
                dataGridViewTamamlanmisKesimListesi.Columns.Contains(sutun) &&
                dataGridViewTamamlanmisKesimListesi.Columns[sutun].Visible).ToList();

            frmSutunSiralama sutunSiralamaFormu = new frmSutunSiralama(gorunurSutunlar);
            if (sutunSiralamaFormu.ShowDialog() == DialogResult.OK)
            {
                var yeniSira = sutunSiralamaFormu.SutunSirasiniAl();
                var tamSira = GetVarsayilanSutunSirasiniKesimListesi();
                var gorunmezSutunlar = tamSira.Except(yeniSira).ToList();
                yeniSira.AddRange(gorunmezSutunlar);

                Settings.Default.SutunSirasiYapilanKesimler = new StringCollection();
                Settings.Default.SutunSirasiYapilanKesimler.AddRange(yeniSira.ToArray());
                Settings.Default.Save();
                ConfigureDataGridColumns(dataGridViewTamamlanmisKesimListesi);
                DataGridViewSettingsManager.LoadColumnWidths(dataGridViewTamamlanmisKesimListesi, "SutunGenislikYapilanKesimler");
            }
        }

        private void ctlYapilanKesimleriGor_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Yapılan Kesimleri Gör";
        }

        public void FormKullaniciAdiGetir(IKullaniciAdiOgren kullaniciAdi)
        {
            _kullaniciAdi = kullaniciAdi;
        }

        private void LoadKesimListesiToDataGridView()
        {
            try
            {
                DataTable dt = _kesimTamamlanmisService.GetKesimListesTamamlanmis();
                dataGridViewTamamlanmisKesimListesi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewTamamlanmisKesimListesi.DataSource = dt;
                dataGridViewTamamlanmisKesimListesi.ScrollBars = ScrollBars.Both;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kesim listesi yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridColumns(DataGridView dataGrid)
        {
            var varsayilanSutunlar = GetVarsayilanSutunSirasiniKesimListesi();
            var sutunSirasi = Settings.Default.SutunSirasiYapilanKesimler != null
                ? StringCollectionToList(Settings.Default.SutunSirasiYapilanKesimler)
                : varsayilanSutunlar.ToList();

            foreach (var sutun in varsayilanSutunlar)
            {
                if (!sutunSirasi.Contains(sutun))
                {
                    sutunSirasi.Add(sutun);
                }
            }

            sutunSirasi.RemoveAll(sutun => !varsayilanSutunlar.Contains(sutun));

            Settings.Default.SutunSirasiYapilanKesimler = new StringCollection();
            Settings.Default.SutunSirasiYapilanKesimler.AddRange(sutunSirasi.ToArray());
            Settings.Default.Save();

            dataGrid.SuspendLayout();
            try
            {
                for (int i = 0; i < sutunSirasi.Count; i++)
                {
                    var sutunAdi = sutunSirasi[i];
                    DataGridViewColumn col;

                    if (!dataGrid.Columns.Contains(sutunAdi))
                    {
                        col = new DataGridViewTextBoxColumn
                        {
                            Name = sutunAdi,
                            DataPropertyName = sutunAdi,
                            Visible = true,
                            MinimumWidth = 50
                        };
                        dataGrid.Columns.Add(col);
                    }
                    else
                    {
                        col = dataGrid.Columns[sutunAdi];
                        col.DataPropertyName = sutunAdi;
                        col.Visible = true;
                        col.MinimumWidth = 50;
                    }

                    switch (sutunAdi)
                    {
                        case "kesimYapan": col.HeaderText = "Kesim Yapan Kullanıcı"; break;
                        case "kesimId": col.HeaderText = "Kesim ID"; break;
                        case "kesilmisPlanSayisi": col.HeaderText = "Kesilmiş Plan Sayısı"; break;
                        case "kesilenLot": col.HeaderText = "Kesilen Lot"; break;
                        case "kullanilanMalzemeEn": col.HeaderText = "Kullanılan Malzeme En"; break;
                        case "kullanilanMalzemeBoy": col.HeaderText = "Kullanılan Malzeme Boy"; break;
                        case "yanUrunEn": col.HeaderText = "Yan Ürün En"; break;
                        case "yanUrunBoy": col.HeaderText = "Yan Ürün Boy"; break;
                    }

                    col.DisplayIndex = i;
                }

                foreach (DataGridViewColumn c in dataGrid.Columns)
                {
                    if (!sutunSirasi.Contains(c.Name))
                        c.Visible = false;
                }
            }
            finally
            {
                dataGrid.ResumeLayout();
            }

            DataGridViewSettingsManager.LoadColumnWidths(dataGrid, "SutunGenislikYapilanKesimler");
        }

        private List<string> GetVarsayilanSutunSirasiniKesimListesi()
        {
            return new List<string>
            {
                "kesimId", "kesimYapan", "kesilmisPlanSayisi", "kesilenLot",
                "kullanilanMalzemeEn", "kullanilanMalzemeBoy", "yanUrunEn", "yanUrunBoy"
            };
        }

        private List<string> StringCollectionToList(StringCollection collection)
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

        private void tabloDuzenleDetay()
        {
            if (dataGridTamamlanmisDetay.Columns.Contains("olusturan"))
                dataGridTamamlanmisDetay.Columns["olusturan"].HeaderText = "Planı Oluşturan";
            if (dataGridTamamlanmisDetay.Columns.Contains("kesimId"))
                dataGridTamamlanmisDetay.Columns["kesimId"].HeaderText = "Kesim ID";
            if (dataGridTamamlanmisDetay.Columns.Contains("projeNo"))
                dataGridTamamlanmisDetay.Columns["projeNo"].HeaderText = "Proje No";
            if (dataGridTamamlanmisDetay.Columns.Contains("kalite"))
                dataGridTamamlanmisDetay.Columns["kalite"].HeaderText = "Kalite";
            if (dataGridTamamlanmisDetay.Columns.Contains("malzeme"))
                dataGridTamamlanmisDetay.Columns["malzeme"].HeaderText = "Malzeme";
            if (dataGridTamamlanmisDetay.Columns.Contains("kalipNo"))
                dataGridTamamlanmisDetay.Columns["kalipNo"].HeaderText = "Kalıp No";
            if (dataGridTamamlanmisDetay.Columns.Contains("kesilecekPozlar"))
                dataGridTamamlanmisDetay.Columns["kesilecekPozlar"].HeaderText = "Kesilen Pozlar";
            if (dataGridTamamlanmisDetay.Columns.Contains("kpAdetSayilari"))
                dataGridTamamlanmisDetay.Columns["kpAdetSayilari"].HeaderText = "Kesilen Pozların Adet Sayıları";
            if (dataGridTamamlanmisDetay.Columns.Contains("eklemeTarihi"))
            {
                dataGridTamamlanmisDetay.Columns["eklemeTarihi"].HeaderText = "Ekleme Tarihi";
                dataGridTamamlanmisDetay.Columns["eklemeTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
            }
        }

        private void tabloDuzenleHareket()
        {
            if (dataGridViewTamamlanmisHareket.Columns.Contains("kesimYapan"))
                dataGridViewTamamlanmisHareket.Columns["kesimYapan"].HeaderText = "Kesim Yapan Operatör";
            if (dataGridViewTamamlanmisHareket.Columns.Contains("kesimId"))
                dataGridViewTamamlanmisHareket.Columns["kesimId"].HeaderText = "Kesim ID";
            if (dataGridViewTamamlanmisHareket.Columns.Contains("toplamSureSaniye"))
                dataGridViewTamamlanmisHareket.Columns["toplamSureSaniye"].HeaderText = "Toplam Süre (Dakika)";
            if (dataGridViewTamamlanmisHareket.Columns.Contains("baslamaTarihi"))
            {
                dataGridViewTamamlanmisHareket.Columns["baslamaTarihi"].HeaderText = "Başlama Tarihi";
                dataGridViewTamamlanmisHareket.Columns["baslamaTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            }
            if (dataGridViewTamamlanmisHareket.Columns.Contains("bitisTarihi"))
            {
                dataGridViewTamamlanmisHareket.Columns["bitisTarihi"].HeaderText = "Bitiş Tarihi";
                dataGridViewTamamlanmisHareket.Columns["bitisTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            }
            if (dataGridViewTamamlanmisHareket.Columns.Contains("status"))
                dataGridViewTamamlanmisHareket.Columns["status"].HeaderText = "Statü";
        }

        private void dataGridViewTamamlanmisKesimListesi_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridViewSettingsManager.SaveColumnWidths(dataGridViewTamamlanmisKesimListesi, "SutunGenislikYapilanKesimler");
        }
        private void dataGridViewTamamlanmisKesimListesi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var satir = dataGridViewTamamlanmisKesimListesi.Rows[e.RowIndex];
            if (satir.Cells["kesimId"].Value == null) return;

            string kesimId = satir.Cells["kesimId"].Value.ToString();

            try
            {
                var dt = _kesimListesiService.GetirKesimListesi(kesimId);
                dataGridTamamlanmisDetay.DataSource = dt;
                tabloDuzenleDetay();

                var dt1 = _kesimSureService.GetirKesimHareketVeSure(kesimId);

                if (dt1.Columns.Contains("toplamSureSaniye"))
                {
                    foreach (DataRow row in dt1.Rows)
                    {
                        if (row["toplamSureSaniye"] != DBNull.Value && int.TryParse(row["toplamSureSaniye"].ToString(), out int saniye))
                        {
                            decimal dakika = saniye / 60m;
                            row["toplamSureSaniye"] = dakika;
                        }
                    }
                }

                dataGridViewTamamlanmisHareket.DataSource = dt1;
                tabloDuzenleHareket();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridTamamlanmisDetay_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridTamamlanmisDetay.Columns[e.ColumnIndex].Name == "kpAdetSayilari" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal val))
                {
                    e.Value = val.ToString("G29", System.Globalization.CultureInfo.CurrentCulture);
                    e.FormattingApplied = true;
                }
            }
        }

        private void tsmiAra_Click(object sender, EventArgs e)
        {
            string baseSql = _kesimTamamlanmisService.GetKesimListesTamamlanmisQuery();

            var frm = new frmAra(
                dataGridViewTamamlanmisKesimListesi,
                _tabloFiltreleService,
                (dt) =>
                {
                    dataGridViewTamamlanmisKesimListesi.DataSource = dt;
                    ConfigureDataGridColumns(dataGridViewTamamlanmisKesimListesi);
                    DataGridViewSettingsManager.LoadColumnWidths(dataGridViewTamamlanmisKesimListesi, "SutunGenislikYapilanKesimler");
                },
                baseSql,
                _serviceProvider
            );

            frm.ShowDialog();
        }
    }
}