using CEKA_APP.Abstracts;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();
        private IKesimSureService _kesimSureService => _serviceProvider.GetRequiredService<IKesimSureService>();


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
                dataGridViewTamamlanmisKesimListesi.Columns["kesimYapan"].HeaderText = "Kesim Yapan Kulanıcı";

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
                _serviceProvider
            );

            frm.ShowDialog();
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
                    dataGridTamamlanmisDetay.Columns["eklemeTarihi"].HeaderText = "Ekleme Tarihi";

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

                if (dataGridViewTamamlanmisHareket.Columns.Contains("kesimYapan"))
                    dataGridViewTamamlanmisHareket.Columns["kesimYapan"].HeaderText = "Kesim Yapan Operatör";
                if (dataGridViewTamamlanmisHareket.Columns.Contains("kesimId"))
                    dataGridViewTamamlanmisHareket.Columns["kesimId"].HeaderText = "Kesim ID";
                if (dataGridViewTamamlanmisHareket.Columns.Contains("toplamSureSaniye"))
                    dataGridViewTamamlanmisHareket.Columns["toplamSureSaniye"].HeaderText = "Toplam Süre (Dakika)";
                if (dataGridViewTamamlanmisHareket.Columns.Contains("baslamaTarihi"))
                    dataGridViewTamamlanmisHareket.Columns["baslamaTarihi"].HeaderText = "Başlama Tarihi";
                if (dataGridViewTamamlanmisHareket.Columns.Contains("bitisTarihi"))
                    dataGridViewTamamlanmisHareket.Columns["bitisTarihi"].HeaderText = "Bitiş Tarihi";
                if (dataGridViewTamamlanmisHareket.Columns.Contains("status"))
                    dataGridViewTamamlanmisHareket.Columns["status"].HeaderText = "Statü";
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
    }
}