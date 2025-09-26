using CEKA_APP.Abstracts;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlYapilanKesimleriGor : UserControl
    {
        private IKullaniciAdiOgren _kullaniciAdi;
        private readonly IServiceProvider _serviceProvider;

        private IKesimListesiService _kesimListesiService => _serviceProvider.GetRequiredService<IKesimListesiService>();
        private IKesimTamamlanmisService _kesimTamamlanmisService => _serviceProvider.GetRequiredService<IKesimTamamlanmisService>();
        private IKesimTamamlanmisHareketService _kesimTamamlanmisHareketService => _serviceProvider.GetRequiredService<IKesimTamamlanmisHareketService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();


        public ctlYapilanKesimleriGor(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DataTable dt = _kesimTamamlanmisService.GetKesimListesTamamlanmis();

            dataGridViewTamamlanmisKesimListesi.DataSource = dt;

            DataGridViewHelper.StilUygula(dataGridViewTamamlanmisKesimListesi);
            DataGridViewHelper.StilUygula(dataGridTamamlanmisDetay);
            DataGridViewHelper.StilUygula(dataGridViewTamamlanmisHareket);

            tabloDuzenle();
        }

        private void ctlYapilanKesimleriGor_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Yapılan Kesimleri Gör";
        }

        public void FormKullaniciAdiGetir(IKullaniciAdiOgren kullaniciAdi)
        {
            _kullaniciAdi = kullaniciAdi;
        }

        public void tabloDuzenle()
        {
            if (dataGridViewTamamlanmisKesimListesi.Columns.Contains("kesimYapan"))
                dataGridViewTamamlanmisKesimListesi.Columns["kesimYapan"].HeaderText = "Kesim Yapan";

            if (dataGridViewTamamlanmisKesimListesi.Columns.Contains("kesimId"))
                dataGridViewTamamlanmisKesimListesi.Columns["kesimId"].HeaderText = "Kesim ID";

            if (dataGridViewTamamlanmisKesimListesi.Columns.Contains("kesilmisPlanSayisi"))
                dataGridViewTamamlanmisKesimListesi.Columns["kesilmisPlanSayisi"].HeaderText = "Kesilmiş Plan Sayısı";

            if (dataGridViewTamamlanmisKesimListesi.Columns.Contains("kesilenLot"))
                dataGridViewTamamlanmisKesimListesi.Columns["kesilenLot"].HeaderText = "Kesilen Lot";

            if (dataGridViewTamamlanmisKesimListesi.Columns.Contains("kesimTarihi"))
                dataGridViewTamamlanmisKesimListesi.Columns["kesimTarihi"].HeaderText = "Kesim Tarihi";

            if (dataGridViewTamamlanmisKesimListesi.Columns.Contains("kesimSaati"))
                dataGridViewTamamlanmisKesimListesi.Columns["kesimSaati"].HeaderText = "Kesim Saati";
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string baseSql = _kesimTamamlanmisService.GetKesimListesTamamlanmisQuery();

            var frm = new frmAra(
                dataGridViewTamamlanmisKesimListesi,
                _tabloFiltreleService,
                (dt) => { dataGridViewTamamlanmisKesimListesi.DataSource = dt; },
                baseSql,
                _serviceProvider,
                detayEkle: true
            );

            frm.ShowDialog();
        }

        private void dataGridViewTamamlanmisKesimListesi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var satir = dataGridViewTamamlanmisKesimListesi.Rows[e.RowIndex];
                if (satir.Cells["kesimId"].Value != null)
                {
                    string kesimId = satir.Cells["kesimId"].Value.ToString();

                    try
                    {
                        var dt = _kesimListesiService.GetirKesimListesi(kesimId);

                        dataGridTamamlanmisDetay.DataSource = dt;
                        dataGridTamamlanmisDetay.Columns[0].Visible = false;
                        dataGridTamamlanmisDetay.Columns[1].HeaderText = "Planı Oluşturan";
                        dataGridTamamlanmisDetay.Columns[2].HeaderText = "Kesim ID";
                        dataGridTamamlanmisDetay.Columns[3].HeaderText = "Proje No";
                        dataGridTamamlanmisDetay.Columns[4].HeaderText = "Kalite";
                        dataGridTamamlanmisDetay.Columns[5].HeaderText = "Malzeme";
                        dataGridTamamlanmisDetay.Columns[6].HeaderText = "Kalıp No";
                        dataGridTamamlanmisDetay.Columns[7].HeaderText = "Kesilen Pozlar";
                        dataGridTamamlanmisDetay.Columns[8].HeaderText = "Kesilen Pozların Adet Sayıları";
                        dataGridTamamlanmisDetay.Columns[9].HeaderText = "Ekleme Tarihi";

                        dataGridTamamlanmisDetay.CellFormatting += (s, ev) =>
                        {
                            if (ev.ColumnIndex == dataGridTamamlanmisDetay.Columns[8].Index && ev.Value != null)
                            {
                                if (decimal.TryParse(ev.Value.ToString(), out decimal val))
                                {
                                    ev.Value = val.ToString("G29", System.Globalization.CultureInfo.CurrentCulture);
                                    ev.FormattingApplied = true;
                                }
                            }
                        };

                        var dt1 = _kesimTamamlanmisHareketService.GetirKesimTamamlanmisHareket(kesimId);

                        dataGridViewTamamlanmisHareket.DataSource = dt1;
                        dataGridViewTamamlanmisHareket.Columns[0].Visible = false;
                        dataGridViewTamamlanmisHareket.Columns[1].HeaderText = "Kesim Yapan";
                        dataGridViewTamamlanmisHareket.Columns[2].HeaderText = "Kesim ID";
                        dataGridViewTamamlanmisHareket.Columns[3].HeaderText = "Kesilen Adet";
                        dataGridViewTamamlanmisHareket.Columns[4].HeaderText = "Kesim Tarihi";
                        dataGridViewTamamlanmisHareket.Columns[5].HeaderText = "Kesim Saati";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}