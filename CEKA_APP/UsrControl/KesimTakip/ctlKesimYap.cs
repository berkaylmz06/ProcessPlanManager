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
using CEKA_APP.Concretes;
using CEKA_APP.DataBase;
using CEKA_APP.Helper;

namespace CEKA_APP.UsrControl
{
    public partial class ctlKesimYap : UserControl
    {
        private IKullaniciAdiOgren _kullaniciAdi;
        private Dictionary<string, string> sonFiltreKriterleri = new Dictionary<string, string>();

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

        private void ctlKesimYap_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Kesim Yap";
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

        public void VerileriYukle()
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
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            var userController = new LogEkle(_kullaniciAdi.lblSistemKullaniciMetinAl());
            userController.LogYap("Tabloda Arama", "Kesim Yap", "Kullanıcı Tabloda Arama Yaptı.");

            List<DataGridViewColumn> tempFilteredColumns = new List<DataGridViewColumn>();
            foreach (DataGridViewColumn column in dataGridKesimListesi.Columns)
            {
                if (column.Name != "Detay")
                {
                    DataGridViewColumn newCol = (DataGridViewColumn)column.Clone();
                    tempFilteredColumns.Add(newCol);
                }
            }

            using (DataGridView tempGrid = new DataGridView())
            {
                foreach (var col in tempFilteredColumns)
                {
                    tempGrid.Columns.Add(col);
                }

                frmAra frm = new frmAra(
                    tempGrid.Columns,
                    KesimListesiFiltrele,
                    AramaSonucuGeldi,
                    true,
                    sonFiltreKriterleri
                );

                frm.ShowDialog();
            }
        }

        private DataTable KesimListesiFiltrele(Dictionary<string, TextBox> filtreler)
        {
            try
            {
                KesimListesiPaketData kesimData = new KesimListesiPaketData();
                sonFiltreKriterleri.Clear();
                foreach (var filtre in filtreler)
                {
                    if (!string.IsNullOrEmpty(filtre.Value.Text.Trim()))
                    {
                        sonFiltreKriterleri[filtre.Key] = filtre.Value.Text.Trim();
                    }
                }
                return kesimData.FiltreleKesimListesiPaket(filtreler, dataGridKesimListesi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Hata detayı: {ex.ToString()}");
                return null;
            }
        }

        private void AramaSonucuGeldi(DataTable tablo)
        {
            dataGridKesimListesi.DataSource = tablo;
            if (dataGridKesimListesi.Columns.Contains("id"))
                dataGridKesimListesi.Columns["id"].Visible = false;

            tabloDuzenle();
            // Detay sütununu tekrar ekle
            if (!tablo.Columns.Contains("Detay"))
            {
                tablo.Columns.Add("Detay", typeof(string));
                foreach (DataRow row in tablo.Rows)
                {
                    row["Detay"] = "Detay Görmek İçin Tıklayınız.";
                }
            }
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

                var dt = KesimListesiData.GetirKesimListesi(kesimId);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("KesimListesi tablosunda ilgili kesimId bulunamadı.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                StringBuilder pozVeSondurumMesaj = new StringBuilder();
                StringBuilder hataAyrintilari = new StringBuilder();
                List<string> hataMesajlari = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    string kalite = row["kalite"].ToString();
                    string malzeme = row["malzeme"].ToString();
                    string kalipNo = row["kalipNo"].ToString();
                    string poz = row["kesilecekPozlar"].ToString();
                    string proje = row["projeNo"].ToString();
                    string adetSatır = row["kpAdetSayilari"].ToString();

                    string ifsKalite = KarsilastirmaTablosuData.GetIfsCodeByAutoCadCodeKalite(kalite);
                    if (string.IsNullOrEmpty(ifsKalite))
                    {
                        MessageBox.Show($"Kalite kodu '{kalite}' için eşleşme bulunamadı.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string hataMesaji;
                    string ifsMalzeme = KarsilastirmaTablosuData.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                    if (string.IsNullOrEmpty(ifsMalzeme))
                    {
                        hataMesajlari.Add(hataMesaji);
                        MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!int.TryParse(adetSatır, out int kpAdet))
                    {
                        MessageBox.Show("Veritabanındaki bazı adet değerleri geçerli değil.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string kalipNoPoz = $"{kalipNo}-{poz}";
                    string kalipNoPozForValidation = kalipNoPoz;

                    int tireSayisi = kalipNoPoz.Count(c => c == '-');
                    if (tireSayisi >= 3)
                    {
                        int ucuncuTireIndex = kalipNoPoz.IndexOf('-', kalipNoPoz.IndexOf('-', kalipNoPoz.IndexOf('-') + 1) + 1);
                        kalipNoPozForValidation = kalipNoPoz.Substring(0, ucuncuTireIndex);
                    }

                    int sondurum = kpAdet * carpan;
                    string pozbilgileri = $"{ifsKalite}-{ifsMalzeme}-{kalipNoPozForValidation}-{proje}";
                    pozVeSondurumMesaj.AppendLine($"Poz: {pozbilgileri}, Sondurum: {sondurum}");

                    hataAyrintilari.AppendLine($"Kontrol edilen pozbilgileri: {pozbilgileri}");

                    if (!KesimDetaylariData.PozExists(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje))
                    {
                        MessageBox.Show($"Poz: {pozbilgileri} KesimDetaylari tablosunda bulunamadı.\nAyrıntılar:\n{hataAyrintilari.ToString()}",
                            "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string hata;
                bool paketSonuc = KesimListesiPaketData.KesimListesiPaketKontrolluDusme(kesimId, carpan, out hata);
                if (!paketSonuc)
                {
                    MessageBox.Show(hata, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    string kalite = row["kalite"].ToString();
                    string malzeme = row["malzeme"].ToString();
                    string kalipNo = row["kalipNo"].ToString();
                    string poz = row["kesilecekPozlar"].ToString();
                    string proje = row["projeNo"].ToString();
                    string adetSatır = row["kpAdetSayilari"].ToString();

                    string ifsKalite = KarsilastirmaTablosuData.GetIfsCodeByAutoCadCodeKalite(kalite);
                    if (string.IsNullOrEmpty(ifsKalite))
                    {
                        MessageBox.Show($"Kalite kodu '{kalite}' için eşleşme bulunamadı.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string hataMesaji;
                    string ifsMalzeme = KarsilastirmaTablosuData.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                    if (string.IsNullOrEmpty(ifsMalzeme))
                    {
                        hataMesajlari.Add(hataMesaji);
                        MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int sondurum = int.Parse(adetSatır) * carpan;
                    string kalipNoPoz = $"{kalipNo}-{poz}";
                    string kalipNoPozForValidation = kalipNoPoz;
                    if (kalipNoPoz.Contains("-EK"))
                    {
                        kalipNoPozForValidation = kalipNoPoz.Substring(0, kalipNoPoz.IndexOf("-EK"));
                    }

                    bool updateSuccess = KesimDetaylariData.UpdateKesilmisAdet(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje, sondurum);
                    if (!updateSuccess)
                    {
                        MessageBox.Show($"Poz: {ifsKalite}-{ifsMalzeme}-{kalipNoPozForValidation}-{proje} için kesilmisAdet veya kesilecekAdet güncellenemedi. Kesilecek adet yetersiz olabilir.\nSondurum: {sondurum}",
                            "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                bool sonuc1 = KesimTamamlanmisData.TablodanKesimTamamlanmisEkleme(olusturan, kesimId, carpan, tarih, saat);
                bool sonuc2 = KesimTamamlanmisHareket.TablodanKesimTamamlanmisHareketEkleme(olusturan, kesimId, carpan, tarih, saat);

                if (sonuc1 && sonuc2)
                {
                    var userController = new LogEkle(_kullaniciAdi.lblSistemKullaniciMetinAl());
                    userController.LogYap("KesimPlaniKesildi", "Kesim Yap", $"Kullanıcı {kesimId} numaralı kesim planından {txtKesilecekPlanSayisi.Text} adet kesimini tamamladı.");
                }
                else
                {
                    MessageBox.Show("Kayıt işlemi sırasında hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
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