using System;
using System.Windows.Forms;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using Image = iText.Layout.Element.Image;
using HorizontalAlignment = iText.Layout.Properties.HorizontalAlignment;

namespace CEKA_APP.Forms
{
    public partial class frmFaturaOlustur : Form
    {
        private string kilometreTasiAdi;
        private string tutar;
        private string aciklama;
        private string tarih;
        private readonly string configFilePath = @"\\192.168.2.3\proje\CEKA APP\config.txt"; // Paylaşılan text dosya yolu
        private readonly string sharedFolderPath = @"\\192.168.2.3\proje\CEKA APP\Resources\"; // Paylaşılan resim klasörü

        public frmFaturaOlustur(string kilometreTasiAdi, string tutar, string aciklama, string tarih)
        {
            this.kilometreTasiAdi = kilometreTasiAdi ?? "Bilinmiyor";
            this.tutar = tutar ?? "0";
            this.aciklama = aciklama ?? "";
            this.tarih = tarih ?? "Belirtilmemiş";
            InitializeComponent();
        }

        private void frmFaturaOlustur_Load(object sender, EventArgs e)
        {
            txtKilometreTasiAdi.Text = kilometreTasiAdi;
            txtTutar.Text = tutar;
            txtAciklama.Text = aciklama;
            txtTarih.Text = tarih;
        }

        private void btnPdfOlustur_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.Title = "Faturayı Kaydet";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        GenerateInvoicePdf(saveFileDialog.FileName);
                        MessageBox.Show("Fatura PDF olarak kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"PDF oluşturulurken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void GenerateInvoicePdf(string filePath)
        {
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                writer.SetCompressionLevel(0); // Görüntü sıkıştırmasını kapat
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document document = new Document(pdf))
                {
                    // config.txt dosyasından resim yollarını oku
                    string pdfUstResimYol = "";
                    string pdfAltResimYol = "";
                    try
                    {
                        if (!File.Exists(configFilePath))
                        {
                            // config.txt yoksa oluştur
                            Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));
                            File.WriteAllLines(configFilePath, new[]
                            {
                                $"pdfUstResimYol={Path.Combine(sharedFolderPath, "pdfUst.png")}",
                                $"pdfAltResimYol={Path.Combine(sharedFolderPath, "pdfAlt.png")}"
                            });
                        }

                        var lines = File.ReadAllLines(configFilePath);
                        foreach (var line in lines)
                        {
                            if (line.StartsWith("pdfUstResimYol="))
                                pdfUstResimYol = line.Substring("pdfUstResimYol=".Length).Trim();
                            else if (line.StartsWith("pdfAltResimYol="))
                                pdfAltResimYol = line.Substring("pdfAltResimYol=".Length).Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        document.Add(new Paragraph($"Yapılandırma dosyası okunurken hata oluştu: {ex.Message}"));
                    }

                    // Üst Resim (Tamamen sola dayalı, sağdan 2 cm boşluk, yükseklik 5 cm)
                    try
                    {
                        if (!string.IsNullOrEmpty(pdfUstResimYol) && File.Exists(pdfUstResimYol))
                        {
                            ImageData imageData = ImageDataFactory.Create(pdfUstResimYol);
                            float heightInPoints = 141.75f; // 5 cm = 28.35 punto * 5
                            float maxWidthInPoints = pdf.GetDefaultPageSize().GetWidth() - 56.7f; // Sağdan 2 cm (56.7 punto) boşluk
                            Image resim1 = new Image(imageData)
                                .ScaleToFit(maxWidthInPoints, heightInPoints)
                                .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                                .SetMarginLeft(0f); // Tamamen sola dayalı
                            document.Add(resim1);
                        }
                        else
                        {
                            document.Add(new Paragraph("Üst resim dosyası bulunamadı!"));
                        }
                    }
                    catch (Exception ex)
                    {
                        document.Add(new Paragraph($"Üst resim eklenirken hata oluştu: {ex.Message}"));
                    }

                    document.Add(new Paragraph("FATURA")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20));

                    document.Add(new Paragraph($"Kilometre Taşı: {kilometreTasiAdi}"));
                    document.Add(new Paragraph($"Tutar: {tutar}"));
                    document.Add(new Paragraph($"Açıklama: {aciklama}"));
                    document.Add(new Paragraph($"Tarih: {tarih}"));

                    // Alt Resim (Sayfanın altına ortalı, yükseklik 5 cm, alt kenardan 2 cm yukarıda)
                    try
                    {
                        if (!string.IsNullOrEmpty(pdfAltResimYol) && File.Exists(pdfAltResimYol))
                        {
                            ImageData imageData = ImageDataFactory.Create(pdfAltResimYol);
                            float heightInPoints = 141.75f; // 5 cm = 28.35 punto * 5
                            float maxWidthInPoints = pdf.GetDefaultPageSize().GetWidth() - 56.7f - 56.7f; // Sağ ve sol 2 cm boşluk
                            Image resim2 = new Image(imageData)
                                .ScaleToFit(maxWidthInPoints, heightInPoints)
                                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                                .SetFixedPosition(56.7f, 56.7f); // Sol kenardan 2 cm, alt kenardan 2 cm (56.7 punto)
                            document.Add(resim2);
                        }
                        else
                        {
                            document.Add(new Paragraph("Alt resim dosyası bulunamadı!"));
                        }
                    }
                    catch (Exception ex)
                    {
                        document.Add(new Paragraph($"Alt resim eklenirken hata oluştu: {ex.Message}"));
                    }
                }
            }
        }

        private void btnUstResim_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                openFileDialog.Title = "Üst Resim Seç";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string destPath = Path.Combine(sharedFolderPath, "pdfUst" + Path.GetExtension(openFileDialog.FileName));
                        Directory.CreateDirectory(sharedFolderPath); // Klasör yoksa oluştur
                        File.Copy(openFileDialog.FileName, destPath, true);

                        // config.txt dosyasını güncelle
                        UpdateConfigFile("pdfUstResimYol", destPath);
                        MessageBox.Show("Üst resim güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Üst resim güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void bnAltResim_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                openFileDialog.Title = "Alt Resim Seç";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string destPath = Path.Combine(sharedFolderPath, "pdfAlt" + Path.GetExtension(openFileDialog.FileName));
                        Directory.CreateDirectory(sharedFolderPath); // Klasör yoksa oluştur
                        File.Copy(openFileDialog.FileName, destPath, true);

                        // config.txt dosyasını güncelle
                        UpdateConfigFile("pdfAltResimYol", destPath);
                        MessageBox.Show("Alt resim güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Alt resim güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UpdateConfigFile(string key, string newPath)
        {
            string pdfUstResimYol = Path.Combine(sharedFolderPath, "pdfUst.png");
            string pdfAltResimYol = Path.Combine(sharedFolderPath, "pdfAlt.png");

            // Mevcut config.txt içeriğini oku
            if (File.Exists(configFilePath))
            {
                var lines = File.ReadAllLines(configFilePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("pdfUstResimYol="))
                        pdfUstResimYol = line.Substring("pdfUstResimYol=".Length).Trim();
                    else if (line.StartsWith("pdfAltResimYol="))
                        pdfAltResimYol = line.Substring("pdfAltResimYol=".Length).Trim();
                }
            }

            // Güncellenecek anahtarı değiştir
            if (key == "pdfUstResimYol")
                pdfUstResimYol = newPath;
            else if (key == "pdfAltResimYol")
                pdfAltResimYol = newPath;

            // config.txt dosyasını yeniden yaz
            Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));
            File.WriteAllLines(configFilePath, new[]
            {
                $"pdfUstResimYol={pdfUstResimYol}",
                $"pdfAltResimYol={pdfAltResimYol}"
            });
        }
    }
}