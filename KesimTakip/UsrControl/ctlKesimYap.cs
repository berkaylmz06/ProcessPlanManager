using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KesimTakip.DataBase;
using KesimTakip.Helper;

namespace KesimTakip.UsrControl
{
    public partial class ctlKesimYap : UserControl
    {
        public static string kesimYapanKullanici;
        public DataGridView AssociatedDataGridView { get; set; }
        public ctlKesimYap(string kesimYapan)
        {
            InitializeComponent();

            kesimYapanKullanici = kesimYapan;
            KesimListesiPaketData kesimdatas = new KesimListesiPaketData();
            DataTable dt = kesimdatas.GetKesimListesiPaket();

            dataGridKesimListesi.DataSource = dt;

            DataGridViewHelper.StilUygula(dataGridDetay);
            DataGridViewHelper.StilUygula(dataGridKesimListesi);
            tabloDuzenle();

            VerileriYukle();
        }

        private void dataGridKesimListesi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var satir = dataGridKesimListesi.Rows[e.RowIndex];
                if (satir.Cells["kesimId"].Value != null)
                {
                    string kesimId = satir.Cells["kesimId"].Value.ToString();


                    try
                    {
                        var dt = KesimListesiData.GetirKesimListesi(kesimId);

                        dataGridDetay.DataSource = dt;
                        dataGridDetay.Columns[0].Visible = false;
                        dataGridDetay.Columns[1].HeaderText = "Planı Oluşturan";
                        dataGridDetay.Columns[2].HeaderText = "Kesim ID";
                        dataGridDetay.Columns[3].HeaderText = "Proje No";
                        dataGridDetay.Columns[4].HeaderText = "Kalite";
                        dataGridDetay.Columns[5].HeaderText = "Kalınlık";
                        dataGridDetay.Columns[6].HeaderText = "Kalıp No";
                        dataGridDetay.Columns[7].HeaderText = "Kesilen Pozlar";
                        dataGridDetay.Columns[8].HeaderText = "Kesilen Pozların Adet Sayıları";
                        dataGridDetay.Columns[9].HeaderText = "Ekleme Tarihi";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void VerileriYukle()
        {
            KesimListesiPaketData kesimdatas = new KesimListesiPaketData();
            DataTable dt = kesimdatas.GetKesimListesiPaket();

            dt.Columns.Add("Detay", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["Detay"] = "Detay Görmek İçin Tıklayınız.";
            }

            dataGridKesimListesi.DataSource = dt;
        }


        public void tabloDuzenle()
        {
            if (dataGridKesimListesi.Columns.Contains("id"))
                dataGridKesimListesi.Columns["id"].Visible = false;

            if (dataGridKesimListesi.Columns.Contains("olusturan"))
                dataGridKesimListesi.Columns["olusturan"].HeaderText = "Planı Oluşturan";

            if (dataGridKesimListesi.Columns.Contains("kesimId"))
                dataGridKesimListesi.Columns["kesimId"].HeaderText = "Kesim ID";

            if (dataGridKesimListesi.Columns.Contains("kesilecekPlanSayisi"))
                dataGridKesimListesi.Columns["kesilecekPlanSayisi"].HeaderText = "Kesilecek Plan Sayısı";

            if (dataGridKesimListesi.Columns.Contains("kesilmisPlanSayisi"))
                dataGridKesimListesi.Columns["kesilmisPlanSayisi"].HeaderText = "Kesilmiş Plan Sayısı";

            if (dataGridKesimListesi.Columns.Contains("toplamPlanTekrari"))
                dataGridKesimListesi.Columns["toplamPlanTekrari"].HeaderText = "Toplam Plan Tekrarı";

            if (dataGridKesimListesi.Columns.Contains("eklemeTarihi"))
                dataGridKesimListesi.Columns["eklemeTarihi"].HeaderText = "Ekleme Tarihi";

            if (dataGridKesimListesi.Columns.Contains("Detay"))
                dataGridKesimListesi.Columns["Detay"].HeaderText = "Detay";
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            AssociatedDataGridView = dataGridKesimListesi;
            frmAra araForm = new frmAra(AssociatedDataGridView.Columns);
            araForm.ShowDialog();
        }

        private void btnPaketKes_Click(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            string tarih = currentDateTime.ToString("dd.MM.yyyy");
            string saat = currentDateTime.ToString("HH:mm:ss");

            if (dataGridKesimListesi.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridKesimListesi.SelectedRows[0];

                try
                {
                    var olusturan = selectedRow.Cells["olusturan"].Value?.ToString();
                    string kesimId = selectedRow.Cells["kesimId"].Value?.ToString() ?? "0";

                    if (string.IsNullOrEmpty(txtKesilecekPlanSayisi.Text) || !int.TryParse(txtKesilecekPlanSayisi.Text, out int kesilmisPlanSayisi) || kesilmisPlanSayisi <= 0)
                    {
                        MessageBox.Show("Kesilecek Plan Sayısını geçerli bir değer ile doldurun.");
                        return;
                    }

                    if (string.IsNullOrEmpty(olusturan) || kesilmisPlanSayisi <= 0)
                    {
                        MessageBox.Show("Eksik veri. Lütfen tüm gerekli alanları doldurun.");
                        return;
                    }

                    string hata;
                    bool sonuc = KesimListesiPaketData.KesimListesiPaketKontrolluDusme(kesimId, kesilmisPlanSayisi, out hata);

                    if (!sonuc)
                    {
                        MessageBox.Show(hata);
                    }
                    else
                    {
                        KesimTamamlanmisData.TablodanKesimTamamlanmisEkleme(olusturan, kesimId, kesilmisPlanSayisi, tarih, saat);
                        MessageBox.Show("Kesim başarıyla tamamlandı.");
                        KesimListesiPaketData.VerileriYenile(dataGridKesimListesi);
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Kesim ID'si geçersiz.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.");
            }
        }
    }
}
