using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CEKA_APP.Abstracts;
using CEKA_APP.DataBase;
using CEKA_APP.Helper;

namespace CEKA_APP.UsrControl
{
    public partial class ctlYapilanKesimleriGor : UserControl
    {
        private IKullaniciAdiOgren _kullaniciAdi;
        private Dictionary<string, string> sonFiltreKriterleri = new Dictionary<string, string>(); 

        public ctlYapilanKesimleriGor()
        {
            InitializeComponent();
            KesimTamamlanmisData kesimTamamlanmisDatas = new KesimTamamlanmisData();
            DataTable dt = kesimTamamlanmisDatas.GetKesimListesTamamlanmis();

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

            if (dataGridViewTamamlanmisKesimListesi.Columns.Contains("kesimTarihi"))
                dataGridViewTamamlanmisKesimListesi.Columns["kesimTarihi"].HeaderText = "Kesim Tarihi";

            if (dataGridViewTamamlanmisKesimListesi.Columns.Contains("kesimSaati"))
                dataGridViewTamamlanmisKesimListesi.Columns["kesimSaati"].HeaderText = "Kesim Saati";
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            frmAra frm = new frmAra(
                dataGridViewTamamlanmisKesimListesi.Columns,
                KesimListesiFiltrele,
                AramaSonucuGeldi,
                false,
                sonFiltreKriterleri 
            );

            frm.ShowDialog();
        }

        private DataTable KesimListesiFiltrele(Dictionary<string, TextBox> filtreler)
        {
            sonFiltreKriterleri.Clear();
            foreach (var filtre in filtreler)
            {
                sonFiltreKriterleri[filtre.Key] = filtre.Value.Text.Trim();
            }

            return KesimTamamlanmisData.KesimTamamlanmisFiltrele(filtreler);
        }

        private void AramaSonucuGeldi(DataTable tablo)
        {
            dataGridViewTamamlanmisKesimListesi.DataSource = tablo;
            if (dataGridViewTamamlanmisKesimListesi.Columns.Contains("id"))
                dataGridViewTamamlanmisKesimListesi.Columns["id"].Visible = false;

            tabloDuzenle();
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
                        var dt = KesimListesiData.GetirKesimListesi(kesimId);

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

                        var dt1 = KesimTamamlanmisHareket.GetirKesimTamamlanmisHareket(kesimId);

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