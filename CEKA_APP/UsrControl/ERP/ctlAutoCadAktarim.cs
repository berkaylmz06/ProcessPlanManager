using CEKA_APP.Abstracts;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CEKA_APP.UsrControl
{
    public partial class ctlAutoCadAktarim : UserControl
    {
        private List<AutoCadAktarim> tumParcalar;
        private IFormArayuzu _formArayuzu;
        public ctlAutoCadAktarim()
        {
            InitializeComponent();

            DataGridViewHelper.StilUygula(dataGridIslenmisXml);
            DataGridViewHelper.StilUygula(dataGridXmlCiktisi);
        }
        public void FormArayuzuAyarla(IFormArayuzu formArayuzu)
        {
            _formArayuzu = formArayuzu;
        }
        private void ctlAutoCadAktarim_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "AutoCad Aktarım";
            btnAktar.Enabled = false; 
        }
        private void btnXmlDosyaSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "XML Dosyası|*.xml";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string xmlPath = ofd.FileName;
                    try
                    {
                        tumParcalar = ParcalariOku(xmlPath);
                        dataGridXmlCiktisi.DataSource = tumParcalar;
                        btnAktar.Enabled = true; 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnAktar.Enabled = false; 
                    }
                }
            }
            tabloDuzenle();
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            string filtre = txtAra.Text.ToLower();

            if (tumParcalar == null)
            {
                MessageBox.Show("XML okutması yapılmamış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridXmlCiktisi.DataSource = null;
                btnAktar.Enabled = false;
                return;
            }

            if (string.IsNullOrEmpty(filtre))
            {
                dataGridXmlCiktisi.DataSource = tumParcalar;
            }
            else
            {
                var filtrelenmisParcalar = tumParcalar.Where(p =>
                    (p.Grup?.ToLower().Contains(filtre) ?? false) ||
                    (p.PozNo?.ToLower().Contains(filtre) ?? false) ||
                    (p.Ad?.ToLower().Contains(filtre) ?? false) ||
                    (p.Kalite?.ToLower().Contains(filtre) ?? false)
                ).ToList();
                dataGridXmlCiktisi.DataSource = filtrelenmisParcalar;
            }
        }
        private void OzetTabloyuGuncelle()
        {
            var guncelParcalar = dataGridXmlCiktisi.DataSource as List<AutoCadAktarim>;
            if (guncelParcalar == null) return;

            var project = txtProjeNo.Text.Trim();
            var projeKodu = string.IsNullOrEmpty(project) ? "Bilinmiyor" : project;

            List<string> hataMesajlari = new List<string>();

            var ozetParcalar = guncelParcalar
                .Where(p => !string.IsNullOrEmpty(p.Ad) && !string.IsNullOrEmpty(p.Kalite))
                .Select(p =>
                {
                    string formattedPozNo = p.PozNo.Length == 1 ? $"0{p.PozNo}" : p.PozNo;

                    string ifsMalzemeAd = KarsilastirmaTablosuData.GetIfsCodeByAutoCadCodeMalzeme(p.Ad);
                    if (string.IsNullOrEmpty(ifsMalzemeAd))
                    {
                        ifsMalzemeAd = p.Ad;
                    }

                    return new AutoCadAktarimDetay
                    {
                        Proje = projeKodu,
                        Grup = p.Grup,
                        MalzemeKod = $"{p.Grup.Substring(0, 3)}-00-{formattedPozNo}",
                        Adet = p.Adet * p.GrupAdet,
                        MalzemeAd = ifsMalzemeAd,
                        Kalite = p.Kalite
                    };
                })
                .OrderBy(p => p.Grup)
                .ThenBy(p => p.MalzemeKod)
                .ToList();

            if (hataMesajlari.Count > 0)
            {
                string hataMesaji = "Aşağıdaki uyarilar bulundu:\n\n" + string.Join("\n", hataMesajlari);
                MessageBox.Show(hataMesaji, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnAktar.Enabled = false; 
            }

            dataGridIslenmisXml.DataSource = ozetParcalar;
        }

        private List<AutoCadAktarim> ParcalariOku(string xmlPath)
        {
            var liste = new List<AutoCadAktarim>();
            var doc = XDocument.Load(xmlPath);

            foreach (var mainpart in doc.Descendants("mainpart"))
            {
                string grupNo = mainpart.Attribute("num")?.Value ?? "Bilinmiyor";
                int grupAdet = int.TryParse(mainpart.Attribute("quantity")?.Value, out int qty) ? qty : 1;

                liste.Add(new AutoCadAktarim
                {
                    Grup = grupNo,
                    PozNo = "",
                    Adet = grupAdet,
                    Ad = "",
                    Kalite = "",
                    NetAgirlik = null,
                    GrupAdet = grupAdet
                });

                foreach (var sp in mainpart.Descendants("singlepart"))
                {
                    var part = sp.Element("part");
                    if (part == null)
                    {
                        Console.WriteLine($"Uyarı: singlepart (num: {sp.Attribute("num")?.Value}) içinde part elementi bulunamadı.");
                        continue;
                    }

                    var parca = new AutoCadAktarim
                    {
                        Grup = grupNo,
                        PozNo = sp.Attribute("num")?.Value ?? "Bilinmiyor",
                        Adet = int.TryParse(sp.Attribute("quantity")?.Value, out int adet) ? adet : 1,
                        Ad = part.Attribute("name")?.Value ?? "Bilinmiyor",
                        Kalite = part.Element("material")?.Attribute("name")?.Value ?? "Bilinmiyor",
                        NetAgirlik = double.TryParse(part.Element("exactWeight")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double net) ? net / 1000.0 : (double?)null,
                        GrupAdet = grupAdet
                    };

                    liste.Add(parca);
                }
            }

            return liste;
        }

        private void btnHazirla_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeNo.Text))
            {
                MessageBox.Show("Lütfen proje bilgisi giriniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnAktar.Enabled = false; 
                return;
            }

            string projeNo = txtProjeNo.Text.Trim();
            var guncelParcalar = dataGridXmlCiktisi.DataSource as List<AutoCadAktarim>;
            if (guncelParcalar == null)
            {
                MessageBox.Show("Parça listesi boş!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnAktar.Enabled = false; 
                return;
            }

            var mevcutGruplar = AutoCadAktarimData.GruplariGetir(projeNo);
            var yeniGruplar = guncelParcalar.Select(p => string.Join("-", p.Grup.Split('-').Take(2))).Distinct().ToList();
            var kesisenGruplar = mevcutGruplar.AsEnumerable()
                .Select(row => string.Join("-", row["grupAdi"].ToString().Split('-').Take(2)))
                .Intersect(yeniGruplar)
                .ToList();

            if (kesisenGruplar.Any())
            {
                MessageBox.Show($"Bu proje için zaten mevcut olan gruplar: {string.Join(", ", kesisenGruplar)}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnAktar.Enabled = false;
                return;
            }

            OzetTabloyuGuncelle();
            btnAktar.Enabled = true;
        }
        public void tabloDuzenle()
        {
            if (dataGridXmlCiktisi.Columns.Contains("GrupAdet"))
                dataGridXmlCiktisi.Columns["GrupAdet"].Visible = false;
        }

        private void btnAktar_Click(object sender, EventArgs e)
        {
            if (dataGridIslenmisXml == null || dataGridIslenmisXml.Rows.Count == 0)
            {
                MessageBox.Show("Kaydedilecek veri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnAktar.Enabled = false;
                return;
            }

            DialogResult result = MessageBox.Show("Verileri yüklemek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            string proje = "";
            HashSet<string> uniqueGruplar = new HashSet<string>();

            foreach (DataGridViewRow row in dataGridIslenmisXml.Rows)
            {
                if (row.IsNewRow) continue;

                proje = row.Cells["Proje"].Value?.ToString() ?? "";
                string grup = row.Cells["Grup"].Value?.ToString() ?? "";
                string malzemeKod = row.Cells["MalzemeKod"].Value?.ToString() ?? "";
                int adet = 0;
                int.TryParse(row.Cells["Adet"].Value?.ToString(), out adet);
                string malzemeAd = row.Cells["MalzemeAd"].Value?.ToString() ?? "";
                string kalite = row.Cells["Kalite"].Value?.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(proje) && !string.IsNullOrWhiteSpace(grup))
                {
                    AutoCadAktarimData.SaveAutoCadData(proje, grup, malzemeKod, adet, malzemeAd, kalite);
                    uniqueGruplar.Add(string.Join("-", grup.Split('-').Take(2))); // XXX-XX formatı
                }
            }

            string gruplar = string.Join(", ", uniqueGruplar);

            var userController = new LogEkle(_formArayuzu.lblSistemKullaniciMetinAl());
            userController.LogYap("PaftaYuklemesiYapildi", "AutoCad Aktarım", $"Kullanıcı {proje} numaralı projeye {gruplar} grupları yükledi.");
            MessageBox.Show("Veriler kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnAktar.Enabled = false; 
        }
    }
}