using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KesimTakip.Abstracts;
using KesimTakip.Concretes;
using KesimTakip.DataBase;
using KesimTakip.Helper;

namespace KesimTakip.UsrControl
{
    public partial class ctlKesimYap : UserControl
    {
        private IKullaniciAdiOgren _kullaniciAdi;
        public ctlKesimYap()
        {
            InitializeComponent();

            KesimListesiPaketData kesimdatas = new KesimListesiPaketData();
            DataTable dt = kesimdatas.GetKesimListesiPaket();

            dataGridKesimListesi.DataSource = dt;

            DataGridViewHelper.StilUygula(dataGridDetay);
            DataGridViewHelper.StilUygula(dataGridKesimListesi);
            tabloDuzenle();

            VerileriYukle();
        }
        public void FormKullaniciAdiGetir(IKullaniciAdiOgren kullaniciAdi)
        {
            _kullaniciAdi = kullaniciAdi;
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
                        dataGridDetay.Columns[5].HeaderText = "Malzeme";
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

            if (!dataGridKesimListesi.Columns.Contains("Detay"))
            {
                DataGridViewTextBoxColumn detayKolon = new DataGridViewTextBoxColumn();
                detayKolon.Name = "Detay";
                detayKolon.HeaderText = "Detay";
                dataGridKesimListesi.Columns.Add(detayKolon);
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            var userController = new UserController(_kullaniciAdi.lblSistemKullaniciMetinAl());
            userController.LogYap("Tabloda Arama", "Kesim Yap", "Kullanıcı Tabloda Arama Yaptı.");


            frmAra frm = new frmAra(
    dataGridKesimListesi.Columns,
    KesimListesiFiltrele,
    AramaSonucuGeldi,true);

            frm.ShowDialog();
        }
        private DataTable KesimListesiFiltrele(Dictionary<string, TextBox> filtreler)
        {
            return KesimListesiPaketData.KesimListesiniPaketFiltrele(filtreler);
        }
        private void AramaSonucuGeldi(DataTable tablo)
        {
            dataGridKesimListesi.DataSource = tablo;
            if (dataGridKesimListesi.Columns.Contains("id"))
                dataGridKesimListesi.Columns["id"].Visible = false;

            tabloDuzenle(); 
        }
        private void btnPaketKes_Click(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now; 
            DateTime tarih = currentDateTime.Date;
            TimeSpan saat = currentDateTime.TimeOfDay;


            if (dataGridKesimListesi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Dikkat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridKesimListesi.SelectedRows[0];

            try
            {
                string kesimId = selectedRow.Cells["kesimId"].Value?.ToString() ?? "0";
                string olusturan = selectedRow.Cells["olusturan"].Value?.ToString();

                if (!int.TryParse(txtKesilecekPlanSayisi.Text, out int carpan) || carpan <= 0)
                {
                    MessageBox.Show("Kesilecek Plan Sayısını geçerli bir pozitif sayı ile doldurun.", "Dikkat!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (string.IsNullOrEmpty(olusturan))
                {
                    MessageBox.Show("Oluşturan bilgisi eksik. Lütfen gerekli alanları doldurun.", "Dikkat!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string hata;
                bool paketSonuc = KesimListesiPaketData.KesimListesiPaketKontrolluDusme(kesimId, carpan, out hata);
                if (!paketSonuc)
                {
                    MessageBox.Show(hata, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool sonuc1 = KesimTamamlanmisData.TablodanKesimTamamlanmisEkleme(olusturan, kesimId, carpan, tarih, saat);
                bool sonuc2 = KesimTamamlanmisHareket.TablodanKesimTamamlanmisHareketEkleme(olusturan, kesimId, carpan, tarih, saat);

                if (sonuc1 && sonuc2)
                {
                    var userController = new UserController(_kullaniciAdi.lblSistemKullaniciMetinAl());
                    userController.LogYap("KesimPlaniKesildi", "Kesim Yap", $"Kullanıcı {kesimId} numaralı kesim planından {txtKesilecekPlanSayisi.Text} adet kesimini tamamladı.");
                }
                else
                {
                    MessageBox.Show("Kayıt işlemi sırasında hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                var dt = KesimListesiData.GetirKesimListesi(kesimId);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("KesimListesi tablosunda ilgili kesimId bulunamadı.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                StringBuilder pozVeSondurumMesaj = new StringBuilder();

                foreach (DataRow row in dt.Rows)
                {
                    string kalite = row["kalite"].ToString();
                    string malzeme = row["malzeme"].ToString();
                    string kalipNo = row["kalipNo"].ToString();
                    string poz = row["kesilecekPozlar"].ToString();
                    string proje = row["projeNo"].ToString();
                    string adetSatır = row["kpAdetSayilari"].ToString();

                    if (!int.TryParse(adetSatır, out int kpAdet))
                    {
                        MessageBox.Show("Veritabanındaki bazı adet değerleri geçerli değil.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int sondurum = kpAdet * carpan;
                    string pozbilgileri = $"{kalite}-{malzeme}-{kalipNo}-{poz}-{proje}";
                    pozVeSondurumMesaj.AppendLine($"Poz: {pozbilgileri}, Sondurum: {sondurum}");

                    if (!KesimDetaylariData.PozExists(pozbilgileri))
                    {
                        MessageBox.Show($"Poz: {pozbilgileri} KesimDetaylari tablosunda bulunamadı.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    bool updateSuccess = KesimDetaylariData.UpdateKesilmisAdet(pozbilgileri, sondurum);
                    if (!updateSuccess)
                    {
                        MessageBox.Show($"Poz: {pozbilgileri} için kesilmisAdet veya kesilecekAdet güncellenemedi. Kesilecek adet yetersiz olabilir.\nSondurum: {sondurum}",
                            "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MessageBox.Show("Kesim başarıyla tamamlandı.",
                    "Başarılı!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                KesimListesiPaketData.VerileriYenile(dataGridKesimListesi);
                tabloDuzenle();
                VerileriYukle();
            }
            catch (FormatException)
            {
                MessageBox.Show("Kesim ID'si geçersiz.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
