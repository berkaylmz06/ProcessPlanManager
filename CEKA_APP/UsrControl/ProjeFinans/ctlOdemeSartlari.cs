using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys;
using CEKA_APP.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using CEKA_APP.Entitys.ProjeFinans; // Add this for SqlCommand and SqlDbType

namespace CEKA_APP.UsrControl
{
    public partial class ctlOdemeSartlari : UserControl
    {
        private List<(string projeNo, int kilometreTasiId)> _pendingDeletions = new List<(string projeNo, int kilometreTasiId)>();

        public ctlOdemeSartlari()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.AutoSize = false;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.MinimumSize = new Size(tableLayoutPanel1.Width, 200);
            tableLayoutPanel1.ColumnCount = 13;
            tableLayoutPanel1.ColumnStyles.Clear();

            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            for (int i = 3; i < 13; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7.5f));
            }

            btnKilometreTasiEkle.Enabled = false;

            tableLayoutPanel1.AllowDrop = false;
        }

        private void LoadOdemeBilgileri(string projeNo)
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.RowCount = 0;
                AddHeaderRow();

                var odemeSekilleriData = new ProjeFinans_OdemeSekilleriData();
                var odemeBilgileri = odemeSekilleriData.GetOdemeBilgileriByProjeNo(projeNo);

                // Filter out items that are marked for pending deletion
                var filteredOdemeBilgileri = odemeBilgileri.Where(odemeBilgi =>
                    !_pendingDeletions.Any(pd => pd.projeNo == projeNo && pd.kilometreTasiId == odemeBilgi.kilometreTasiId)
                )
                // Tahmini Tarihe göre sıralama, null değerleri sona al
                .OrderBy(o => o.tahminiTarih.HasValue ? 0 : 1) // Null olmayanları öne al
                .ThenBy(o => o.tahminiTarih.HasValue ? o.tahminiTarih.Value : DateTime.MaxValue) // Null olmayanları tarihe göre sırala
                .ToList();


                foreach (var odemeBilgi in filteredOdemeBilgileri)
                {
                    int newRowIndex = tableLayoutPanel1.RowCount;
                    tableLayoutPanel1.RowCount++;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                    // Delete Icon PictureBox
                    var pbDelete = new PictureBox
                    {
                        Image = Properties.Resources.copKutusu, // Replace 'DeleteIcon' with your actual icon resource name
                        SizeMode = PictureBoxSizeMode.Zoom, // Adjust as needed (Zoom, StretchImage, CenterImage, Normal)
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        Size = new Size(26, 26), // Keep it small
                        Cursor = Cursors.Hand // Indicate it's clickable
                    };
                    pbDelete.Click += PbDelete_Click; // Attach the new click event
                    pbDelete.Tag = odemeBilgi; // Store the entire object for easy access during deletion
                    tableLayoutPanel1.Controls.Add(pbDelete, 0, newRowIndex); // Column 0

                    var lblKilometreTasiAdi = new Label { Text = odemeBilgi.kilometreTasiAdi, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };

                    var lblOran = new Label { Text = $"%{odemeBilgi.oran:F0}", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };

                    var txtTutar = new TextBox { Text = odemeBilgi.tutar.ToString("N2", System.Globalization.CultureInfo.CurrentCulture), Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle, Enabled = false, BackColor = Color.LightGray };

                    var dtpTahminiTarih = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Margin = new Padding(2), Font = new Font("Segoe UI", 10), Height = 30, MaximumSize = new Size(0, 30), Enabled = false }; // Tahmini Tarih kolonunu pasif yap
                    var dtpGerceklesenTarih = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Margin = new Padding(2), Font = new Font("Segoe UI", 10), Height = 30, MaximumSize = new Size(0, 30) };
                    var rtbAciklama = new RichTextBox { Text = odemeBilgi.aciklama, Dock = DockStyle.Fill, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle, ScrollBars = RichTextBoxScrollBars.Vertical };

                    if (odemeBilgi.tahminiTarih.HasValue)
                    {
                        dtpTahminiTarih.Value = odemeBilgi.tahminiTarih.Value;
                        dtpTahminiTarih.Checked = true;
                    }
                    else
                    {
                        dtpTahminiTarih.Checked = false;
                    }

                    if (odemeBilgi.gerceklesenTarih.HasValue)
                    {
                        dtpGerceklesenTarih.Value = odemeBilgi.gerceklesenTarih.Value;
                        dtpGerceklesenTarih.Checked = true;
                    }
                    else
                    {
                        dtpGerceklesenTarih.Checked = false;
                    }

                    var pnlTeminatMektubu = new Panel { Dock = DockStyle.Fill, Margin = new Padding(2), Height = 30 };
                    var chkTeminatMektubuVar = new CheckBox
                    {
                        Text = "Var",
                        Checked = odemeBilgi.teminatMektubu,
                        Dock = DockStyle.Left,
                        Margin = new Padding(2),
                        Width = 50,
                        Font = new Font("Segoe UI", 10)
                    };
                    var chkTeminatMektubuYok = new CheckBox
                    {
                        Text = "Yok",
                        Checked = !odemeBilgi.teminatMektubu,
                        Dock = DockStyle.Right,
                        Margin = new Padding(2),
                        Width = 50,
                        Font = new Font("Segoe UI", 10)
                    };
                    pnlTeminatMektubu.Controls.Add(chkTeminatMektubuVar);
                    pnlTeminatMektubu.Controls.Add(chkTeminatMektubuYok);

                    var newLblTeminatDurum = new Label
                    {
                        Text = odemeBilgi.teminatMektubu ? "Aktif" : "Pasif",
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(2),
                        ForeColor = odemeBilgi.teminatMektubu ? Color.Green : Color.Red,
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 12, FontStyle.Bold)
                    };

                    var btnMektupGeriAl = new Button
                    {
                        Text = "Geri Al",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Width = 75,
                        Height = 30,
                        Font = new Font("Segoe UI", 10),
                        Enabled = odemeBilgi.teminatMektubu // Initial state based on DB
                    };
                    btnMektupGeriAl.Click += (s, e) =>
                    {
                        var button = s as Button;
                        int rowIdx = tableLayoutPanel1.GetRow(button);
                        var lblTempTeminatDurum = GetLabelAt(rowIdx, 8); // Adjusted column index

                        // İlgili CheckBox'ları bul
                        var pnlTempTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(7, rowIdx) as Panel;
                        var chkTempTeminatMektubuVar = pnlTempTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");
                        var chkTempTeminatMektubuYok = pnlTempTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Yok");

                        if (lblTempTeminatDurum != null)
                        {
                            lblTempTeminatDurum.Text = "Pasif";
                            lblTempTeminatDurum.ForeColor = Color.Red;
                            button.Enabled = false;

                            // Teminat mektubu durumunu CheckBox'lar üzerinde de güncelle
                            if (chkTempTeminatMektubuVar != null) chkTempTeminatMektubuVar.Checked = false;
                            if (chkTempTeminatMektubuYok != null) chkTempTeminatMektubuYok.Checked = true;
                        }
                    };

                    var btnFaturalama = new Button
                    {
                        Text = "Fatura Oluştur",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        AutoSize = true,
                        Height = 30,
                        Font = new Font("Segoe UI", 10)
                    };

                    var lblDurum = new Label
                    {
                        Text = odemeBilgi.durum,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 10)
                    };
                    if (odemeBilgi.durum == "Ödendi")
                    {
                        lblDurum.ForeColor = Color.Green;
                    }
                    else if (odemeBilgi.durum == "Bekliyor")
                    {
                        lblDurum.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblDurum.ForeColor = Color.Black;
                    }

                    // Adjusted column indices due to new delete column at index 0
                    tableLayoutPanel1.Controls.Add(lblKilometreTasiAdi, 1, newRowIndex);
                    tableLayoutPanel1.Controls.Add(lblOran, 2, newRowIndex);
                    tableLayoutPanel1.Controls.Add(txtTutar, 3, newRowIndex);
                    tableLayoutPanel1.Controls.Add(dtpTahminiTarih, 4, newRowIndex);
                    tableLayoutPanel1.Controls.Add(dtpGerceklesenTarih, 5, newRowIndex);
                    tableLayoutPanel1.Controls.Add(rtbAciklama, 6, newRowIndex);

                    tableLayoutPanel1.Controls.Add(pnlTeminatMektubu, 7, newRowIndex);
                    tableLayoutPanel1.Controls.Add(newLblTeminatDurum, 8, newRowIndex);
                    tableLayoutPanel1.Controls.Add(btnMektupGeriAl, 9, newRowIndex);
                    tableLayoutPanel1.Controls.Add(btnFaturalama, 10, newRowIndex);
                    tableLayoutPanel1.Controls.Add(lblDurum, 11, newRowIndex);

                    var btnOdendi = new Button
                    {
                        Text = odemeBilgi.durum == "Ödendi" ? "Bekliyor Yap" : "Ödendi Yap",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        AutoSize = true,
                        Height = 30,
                        Font = new Font("Segoe UI", 10)
                    };
                    btnOdendi.Click += BtnOdendi_Click;
                    tableLayoutPanel1.Controls.Add(btnOdendi, 12, newRowIndex);


                    chkTeminatMektubuVar.CheckedChanged += (s, e) =>
                    {
                        if (chkTeminatMektubuVar.Checked)
                        {
                            chkTeminatMektubuYok.Checked = false;
                            var lblTempTeminatDurum = GetLabelAt(newRowIndex, 8); // Adjusted column index
                            var frm = new frmTeminatMektubuEkle(null);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                if (lblTempTeminatDurum != null)
                                {
                                    lblTempTeminatDurum.Text = "Aktif";
                                    lblTempTeminatDurum.ForeColor = Color.Green;
                                    btnMektupGeriAl.Enabled = true;
                                }
                            }
                            else
                            {
                                chkTeminatMektubuVar.Checked = false;
                                chkTeminatMektubuYok.Checked = true;
                                if (lblTempTeminatDurum != null)
                                {
                                    lblTempTeminatDurum.Text = "Pasif";
                                    lblTempTeminatDurum.ForeColor = Color.Red;
                                    btnMektupGeriAl.Enabled = false;
                                }
                            }
                        }
                    };

                    chkTeminatMektubuYok.CheckedChanged += (s, e) =>
                    {
                        if (chkTeminatMektubuYok.Checked)
                        {
                            chkTeminatMektubuVar.Checked = false;
                            var lblTempTeminatDurum = GetLabelAt(newRowIndex, 8); // Adjusted column index
                            if (lblTempTeminatDurum != null)
                            {
                                lblTempTeminatDurum.Text = "Pasif";
                                lblTempTeminatDurum.ForeColor = Color.Red;
                                btnMektupGeriAl.Enabled = false;
                            }
                        }
                    };


                    btnFaturalama.Click += (s, e) =>
                    {
                        var button = s as Button;
                        int rowIndex = tableLayoutPanel1.GetRow(button);

                        var lblKmTasiAdi = GetLabelAt(rowIndex, 1); // Adjusted column index
                        var txtTutarFatura = GetTextBoxAt(rowIndex, 3); // Adjusted column index
                        var rtbAciklamaFatura = tableLayoutPanel1.GetControlFromPosition(6, rowIndex) as RichTextBox; // Adjusted column index
                        var dtpTahminiTarihFatura = tableLayoutPanel1.GetControlFromPosition(4, rowIndex) as DateTimePicker; // Adjusted column index
                        var dtpGerceklesenTarihFatura = tableLayoutPanel1.GetControlFromPosition(5, rowIndex) as DateTimePicker; // Adjusted column index

                        if (lblKmTasiAdi == null || txtTutarFatura == null || rtbAciklamaFatura == null ||
                            string.IsNullOrWhiteSpace(lblKmTasiAdi.Text) ||
                            string.IsNullOrWhiteSpace(txtTutarFatura.Text) ||
                            string.IsNullOrWhiteSpace(rtbAciklamaFatura.Text))
                        {
                            MessageBox.Show("Seçili satırda gerekli bilgiler eksik veya boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string selectedTarih = "Belirtilmemiş";
                        if (dtpTahminiTarihFatura != null && dtpGerceklesenTarihFatura != null)
                        {
                            if (dtpTahminiTarihFatura.Checked && dtpGerceklesenTarihFatura.Checked)
                            {
                                if (dtpTahminiTarihFatura.Value.Date == dtpGerceklesenTarihFatura.Value.Date)
                                {
                                    selectedTarih = dtpTahminiTarihFatura.Value.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    selectedTarih = dtpGerceklesenTarihFatura.Value.ToString("yyyy-MM-dd");
                                }
                            }
                            else if (dtpTahminiTarihFatura.Checked)
                            {
                                selectedTarih = dtpTahminiTarihFatura.Value.ToString("yyyy-MM-dd");
                            }
                            else if (dtpGerceklesenTarihFatura.Checked)
                            {
                                selectedTarih = dtpGerceklesenTarihFatura.Value.ToString("yyyy-MM-dd");
                            }
                        }

                        var frm = new frmFaturaOlustur(
                            txtTutarFatura.Text,
                            rtbAciklamaFatura.Text,
                            selectedTarih,
                            txtProjeAra.Text.Trim()
                        );
                        frm.ShowDialog();
                    };
                }

                AddBottomSpacer();

                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private void PbDelete_Click(object sender, EventArgs e)
        {
            var pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                int rowIndex = tableLayoutPanel1.GetRow(pictureBox);

                // Başlık satırını (row 0) veya en alttaki boşluk (spacer) satırını silmeye çalışmadığımızdan emin olun
                if (rowIndex <= 0 || rowIndex >= tableLayoutPanel1.RowCount - 1)
                {
                    return;
                }

                var odemeBilgi = pictureBox.Tag as OdemeSekilleri;

                tableLayoutPanel1.SuspendLayout(); // Düzen güncellemelerini duraklat
                try
                {
                    // Case 1: Mevcut bir satırı silme (veritabanından)
                    if (odemeBilgi != null && odemeBilgi.kilometreTasiId != 0)
                    {
                        // Silinmeyi bekleyenler listesine ekle
                        _pendingDeletions.Add((txtProjeAra.Text.Trim(), odemeBilgi.kilometreTasiId));
                        MessageBox.Show("Satır başarıyla silinmek üzere işaretlendi. Kaydet butonuna tıklayarak veritabanından silmeyi onaylayın.", "Silme Onayı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // Case 2: Yeni eklenmiş, henüz kaydedilmemiş bir satırı silme
                    else
                    {
                        // _pendingDeletions'a eklemeye gerek yok, henüz DB'de yok
                        MessageBox.Show("Yeni eklenen satır başarıyla kaldırıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Satırı UI'dan kaldırmak yerine, tüm tabloyu yeniden yükle
                    // Bu, sıralama ve hizalama sorunlarını çözecektir.
                    LoadOdemeBilgileri(txtProjeAra.Text.Trim());

                    // Bir satır kaldırıldıktan sonra toplam yüzdeyi yeniden hesapla
                    btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
                }
                finally
                {
                    tableLayoutPanel1.ResumeLayout(true); // Düzen güncellemelerine devam et ve hemen uygula
                }
            }
        }


        private void btnAra_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen aranacak Proje No girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnKilometreTasiEkle.Enabled = false;
                _pendingDeletions.Clear();
                LoadOdemeBilgileri(string.Empty); // This will now handle AddHeaderRow()
                return;
            }

            var projeNo = txtProjeAra.Text.Trim();
            Console.WriteLine($"Girilen projeNo: '{projeNo}'");

            // Get ProjeBilgi
            CEKA_APP.Entitys.ProjeFinans.ProjeBilgi projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            Console.WriteLine($"projeBilgi: {(projeBilgi != null ? "Bulundu" : "Bulunamadı")}");

            // Get ProjeKutuk
            ProjeKutuk projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
            Console.WriteLine($"projeKutuk: {(projeKutuk != null ? "Bulundu" : "Bulunamadı")}");

            // Check if it's an alt proje
            bool isAltProje = ProjeFinans_ProjeIliskiData.CheckAltProje(projeNo);
            Console.WriteLine($"isAltProje: {isAltProje}");

            if (projeBilgi != null)
            {
                if (isAltProje)
                {
                    var result = MessageBox.Show(
                        "Alt proje için ödeme verisi giriyorsunuz, devam etmek istiyor musunuz?",
                        "Uyarı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        btnKilometreTasiEkle.Enabled = false;
                        _pendingDeletions.Clear();
                        LoadOdemeBilgileri(string.Empty); // This will now handle AddHeaderRow()
                        return;
                    }
                }

                var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
                // Alt projeleri ProjeFinans_ProjeIliski tablosundan al
                var altProjeler = isAltProje ? ProjeFinans_ProjeIliskiData.GetAltProjeler(projeNo) : (projeKutuk != null && projeKutuk.altProjeVarMi ? projeKutuk.altProjeBilgileri?.ToList() ?? new List<string>() : null);

                var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);

                UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler);
                UpdateTutarColumn(toplamBedel);

                _pendingDeletions.Clear();
                LoadOdemeBilgileri(projeNo); // This will now handle AddHeaderRow()

                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            else
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtToplamBedel.Text = "";
                txtToplamBedel.ForeColor = Color.Black;
                btnKilometreTasiEkle.Enabled = false;

                _pendingDeletions.Clear();
                LoadOdemeBilgileri(string.Empty); // This will now handle AddHeaderRow()
            }
        }

        private bool IsAltProje(string projeNo, ProjeKutuk proje)
        {
            if (proje.altProjeVarMi)
                return false;
            return ProjeFinans_ProjeIliskiData.CheckAltProje(projeNo);
        }

        public void UpdateToplamBedelUI(string projeNo, decimal toplamBedel, List<string> eksikFiyatlandirmaProjeler)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler);
                }));
            }
            else
            {
                var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
                if (projeKutuk != null && projeKutuk.altProjeVarMi)
                {
                    if (eksikFiyatlandirmaProjeler.Any())
                    {
                        txtToplamBedel.Text = $"{toplamBedel:F2} (Alt projeler: {string.Join(", ", eksikFiyatlandirmaProjeler)} için fiyatlandırma yok)";
                        txtToplamBedel.ForeColor = Color.Red;
                    }
                    else
                    {
                        txtToplamBedel.Text = toplamBedel.ToString("F2");
                        txtToplamBedel.ForeColor = Color.Black;
                    }
                }
                else
                {
                    if (eksikFiyatlandirmaProjeler.Any() && eksikFiyatlandirmaProjeler.Contains(projeNo))
                    {
                        txtToplamBedel.Text = $"{toplamBedel:F2} ({projeNo} için fiyatlandırma yok)";
                        txtToplamBedel.ForeColor = Color.Red;
                    }
                    else
                    {
                        txtToplamBedel.Text = toplamBedel.ToString("F2");
                        txtToplamBedel.ForeColor = Color.Black;
                    }
                }
            }
        }

        public void UpdateToplamBedelUI(decimal toplamBedel)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateToplamBedelUI(toplamBedel);
                }));
            }
            else
            {
                txtToplamBedel.Text = toplamBedel.ToString("F2");
                txtToplamBedel.ForeColor = Color.Black;
            }
        }

        private void ctlOdemeSartlari_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Ödeme Şartları";
            AddHeaderRow(); // Ensure header is always present on load
            // Removed AddBottomSpacer() here, it will be added by LoadOdemeBilgileri if needed.
        }

        private void AddHeaderRow()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                // Clear all existing controls and row styles
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.RowCount = 0; // Reset row count before adding new rows

                tableLayoutPanel1.RowCount = 1;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                int headerRowIndex = 0;

                string[] headers = {
            "", // New header for the delete icon column
            "Proje Kilometre Taşları",
            "Oran (%)",
            "Tutar",
            "Tahmini Tarih",
            "Gerçekleşen Tarih",
            "Açıklama",
            "Teminat Mektubu",
            "Teminat Durumu",
            "Mektup Geri Al",
            "Faturalama",
            "Durum",
            "Durum Değiştir"
        };

                for (int i = 0; i < headers.Length; i++)
                {
                    var lbl = new Label
                    {
                        Text = headers[i],
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        BackColor = Color.LightGray,
                    };
                    tableLayoutPanel1.Controls.Add(lbl, i, headerRowIndex);
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private void AddBottomSpacer()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                // Check if a spacer row already exists at the end
                // The header is at row 0. Data rows start from row 1.
                // A spacer would be at tableLayoutPanel1.RowCount - 1.
                if (tableLayoutPanel1.RowCount > 1) // At least header and potentially data rows
                {
                    Control lastControlInFirstCol = tableLayoutPanel1.GetControlFromPosition(0, tableLayoutPanel1.RowCount - 1);
                    if (lastControlInFirstCol is Label lbl && string.IsNullOrEmpty(lbl.Text) && lbl.Height == 10)
                    {
                        // Spacer already exists, do nothing
                        return;
                    }
                }

                // If no spacer, or table is empty (only header), add one
                tableLayoutPanel1.RowCount++;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));
                int newSpacerRowIndex = tableLayoutPanel1.RowCount - 1;

                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    var emptyLabel = new Label
                    {
                        Text = "",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        AutoSize = false,
                        Height = 10,
                        Font = new Font("Segoe UI", 8)
                    };
                    tableLayoutPanel1.Controls.Add(emptyLabel, col, newSpacerRowIndex);
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }


        private void AddYeniKilometreTasiSatiri(string kilometreTasiAdi, string oran)
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                string cleanOran = oran.Replace("%", "").Trim();
                if (!decimal.TryParse(cleanOran, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal yeniOran))
                {
                    MessageBox.Show("Geçersiz oran formatı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal mevcutOranToplami = HesaplaOranToplami();
                if (mevcutOranToplami + yeniOran > 100)
                {
                    MessageBox.Show($"Oran toplamı %100'ü aşıyor (Mevcut: {mevcutOranToplami}, Yeni: {yeniOran}). Yeni kilometre taşı eklenemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RemoveBottomSpacer();

                int newRowIndex = tableLayoutPanel1.RowCount;
                tableLayoutPanel1.RowCount++;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                // --- Tüm kontrolleri değişkenlere atayarak oluşturma ---

                // Delete Icon PictureBox (Column 0)
                var pbDelete = new PictureBox
                {
                    Image = Properties.Resources.copKutusu,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(2),
                    Size = new Size(26, 26),
                    Cursor = Cursors.Hand
                };
                pbDelete.Click += PbDelete_Click;

                // Kilometre Taşı Adı (Column 1)
                var lblKilometreTasiAdi = new Label { Text = kilometreTasiAdi, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };

                // Oran (Column 2)
                var lblOran = new Label { Text = $"%{cleanOran}", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };

                // Tutar (Column 3)
                var txtTutar = new TextBox { Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle, Enabled = false, BackColor = Color.LightGray };

                // Tahmini Tarih (Column 4)
                var dtpTahminiTarih = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Margin = new Padding(2), Font = new Font("Segoe UI", 10), Height = 30, MaximumSize = new Size(0, 30) };

                // Gerçekleşen Tarih (Column 5)
                var dtpGerceklesenTarih = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Margin = new Padding(2), Font = new Font("Segoe UI", 10), Height = 30, MaximumSize = new Size(0, 30) };

                // Açıklama (Column 6)
                var rtbAciklama = new RichTextBox { Dock = DockStyle.Fill, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle, ScrollBars = RichTextBoxScrollBars.Vertical };

                // Teminat Mektubu (Column 7)
                var pnlTeminatMektubu = new Panel { Dock = DockStyle.Fill, Margin = new Padding(2), Height = 30 };
                var chkTeminatMektubuVar = new CheckBox { Text = "Var", Checked = false, Dock = DockStyle.Left, Margin = new Padding(2), Width = 50, Font = new Font("Segoe UI", 10) };
                var chkTeminatMektubuYok = new CheckBox { Text = "Yok", Checked = true, Dock = DockStyle.Right, Margin = new Padding(2), Width = 50, Font = new Font("Segoe UI", 10) };
                pnlTeminatMektubu.Controls.Add(chkTeminatMektubuVar);
                pnlTeminatMektubu.Controls.Add(chkTeminatMektubuYok);

                // Teminat Durumu (Column 8)
                var newLblTeminatDurum = new Label { Text = "Pasif", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), ForeColor = Color.Red, AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 12, FontStyle.Bold) };

                // Mektup Geri Al (Column 9)
                var btnMektupGeriAl = new Button { Text = "Geri Al", Dock = DockStyle.Fill, Margin = new Padding(2), AutoSize = false, Width = 75, Height = 30, Font = new Font("Segoe UI", 10), Enabled = false };

                // Faturalama (Column 10)
                var btnFaturalama = new Button { Text = "Fatura Oluştur", Dock = DockStyle.Fill, Margin = new Padding(2), AutoSize = true, Height = 30, Font = new Font("Segoe UI", 10) };

                // Durum (Column 11)
                var lblDurum = new Label { Text = "Bekliyor", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), ForeColor = Color.Red, AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };

                // Durum Değiştir (Column 12)
                var btnOdendi = new Button { Text = "Ödendi Yap", Dock = DockStyle.Fill, Margin = new Padding(2), AutoSize = true, Height = 30, Font = new Font("Segoe UI", 10) };

                // --- Kontrolleri TableLayoutPanel'e Ekleme ---
                tableLayoutPanel1.Controls.Add(pbDelete, 0, newRowIndex);
                tableLayoutPanel1.Controls.Add(lblKilometreTasiAdi, 1, newRowIndex);
                tableLayoutPanel1.Controls.Add(lblOran, 2, newRowIndex);
                tableLayoutPanel1.Controls.Add(txtTutar, 3, newRowIndex);
                tableLayoutPanel1.Controls.Add(dtpTahminiTarih, 4, newRowIndex);
                tableLayoutPanel1.Controls.Add(dtpGerceklesenTarih, 5, newRowIndex);
                tableLayoutPanel1.Controls.Add(rtbAciklama, 6, newRowIndex);
                tableLayoutPanel1.Controls.Add(pnlTeminatMektubu, 7, newRowIndex);
                tableLayoutPanel1.Controls.Add(newLblTeminatDurum, 8, newRowIndex);
                tableLayoutPanel1.Controls.Add(btnMektupGeriAl, 9, newRowIndex);
                tableLayoutPanel1.Controls.Add(btnFaturalama, 10, newRowIndex);
                tableLayoutPanel1.Controls.Add(lblDurum, 11, newRowIndex);
                tableLayoutPanel1.Controls.Add(btnOdendi, 12, newRowIndex);


                // --- Olay Atamaları (Event Handlers) ---
                btnOdendi.Click += BtnOdendi_Click;

                btnMektupGeriAl.Click += (s, e) =>
                {
                    var button = s as Button;
                    int rowIdx = tableLayoutPanel1.GetRow(button);
                    var lblTempTeminatDurum = GetLabelAt(rowIdx, 8);
                    var pnlTempTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(7, rowIdx) as Panel;
                    var chkTempTeminatMektubuVar = pnlTempTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");
                    var chkTempTeminatMektubuYok = pnlTempTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Yok");

                    if (lblTempTeminatDurum != null)
                    {
                        lblTempTeminatDurum.Text = "Pasif";
                        lblTempTeminatDurum.ForeColor = Color.Red;
                        button.Enabled = false;
                        if (chkTempTeminatMektubuVar != null) chkTempTeminatMektubuVar.Checked = false;
                        if (chkTempTeminatMektubuYok != null) chkTempTeminatMektubuYok.Checked = true;
                    }
                };

                chkTeminatMektubuVar.CheckedChanged += (s, e) =>
                {
                    if (chkTeminatMektubuVar.Checked)
                    {
                        chkTeminatMektubuYok.Checked = false;
                        var frm = new frmTeminatMektubuEkle(null);
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            newLblTeminatDurum.Text = "Aktif";
                            newLblTeminatDurum.ForeColor = Color.Green;
                            btnMektupGeriAl.Enabled = true;
                        }
                        else
                        {
                            chkTeminatMektubuVar.Checked = false;
                            chkTeminatMektubuYok.Checked = true;
                        }
                    }
                };

                chkTeminatMektubuYok.CheckedChanged += (s, e) =>
                {
                    if (chkTeminatMektubuYok.Checked)
                    {
                        chkTeminatMektubuVar.Checked = false;
                        newLblTeminatDurum.Text = "Pasif";
                        newLblTeminatDurum.ForeColor = Color.Red;
                        btnMektupGeriAl.Enabled = false;
                    }
                };

                btnFaturalama.Click += (s, e) =>
                {
                    // Bu click olayının içindeki Get*At çağrıları artık doğru kontrolleri bulacaktır.
                    // Bu nedenle bu kısımda değişiklik yapmaya gerek yoktur.
                    // Örnek olarak bırakılmıştır:
                    var button = s as Button;
                    int rowIndex = tableLayoutPanel1.GetRow(button);

                    var lblKmTasiAdiClick = GetLabelAt(rowIndex, 1);
                    var txtTutarFatura = GetTextBoxAt(rowIndex, 3);
                    var rtbAciklamaFatura = GetRichTextBoxAt(rowIndex, 6);
                    var dtpTahminiTarihFatura = GetDateTimePickerAt(rowIndex, 4);
                    var dtpGerceklesenTarihFatura = GetDateTimePickerAt(rowIndex, 5);

                    if (lblKmTasiAdiClick == null || txtTutarFatura == null || rtbAciklamaFatura == null ||
                        string.IsNullOrWhiteSpace(lblKmTasiAdiClick.Text) ||
                        string.IsNullOrWhiteSpace(txtTutarFatura.Text) ||
                        string.IsNullOrWhiteSpace(rtbAciklamaFatura.Text))
                    {
                        MessageBox.Show("Seçili satırda gerekli bilgiler eksik veya boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string selectedTarih = "Belirtilmemiş";
                    if (dtpTahminiTarihFatura != null && dtpGerceklesenTarihFatura != null)
                    {
                        if (dtpTahminiTarihFatura.Checked && dtpGerceklesenTarihFatura.Checked)
                        {
                            selectedTarih = dtpTahminiTarihFatura.Value.Date == dtpGerceklesenTarihFatura.Value.Date
                                ? dtpTahminiTarihFatura.Value.ToString("yyyy-MM-dd")
                                : dtpGerceklesenTarihFatura.Value.ToString("yyyy-MM-dd");
                        }
                        else if (dtpTahminiTarihFatura.Checked)
                        {
                            selectedTarih = dtpTahminiTarihFatura.Value.ToString("yyyy-MM-dd");
                        }
                        else if (dtpGerceklesenTarihFatura.Checked)
                        {
                            selectedTarih = dtpGerceklesenTarihFatura.Value.ToString("yyyy-MM-dd");
                        }
                    }

                    var frm = new frmFaturaOlustur(
                        txtTutarFatura.Text,
                        rtbAciklamaFatura.Text,
                        selectedTarih,
                        txtProjeAra.Text.Trim()
                    );
                    frm.ShowDialog();
                };

                decimal calculatedToplamBedel;
                if (!ParseToplamBedel(out calculatedToplamBedel))
                {
                    calculatedToplamBedel = 1000m;
                }

                AddBottomSpacer();
                UpdateTutarColumn(calculatedToplamBedel);
                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private void RemoveBottomSpacer()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                if (tableLayoutPanel1.RowCount > 1) // Ensure there's at least a header row
                {
                    int bottomSpacerIndex = tableLayoutPanel1.RowCount - 1;
                    // Check if the last row is indeed the spacer
                    var existingSpacerControl = tableLayoutPanel1.GetControlFromPosition(0, bottomSpacerIndex);
                    if (existingSpacerControl is Label lbl && string.IsNullOrEmpty(lbl.Text) && lbl.Height == 10)
                    {
                        for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                        {
                            Control ctl = tableLayoutPanel1.GetControlFromPosition(col, bottomSpacerIndex);
                            if (ctl != null)
                            {
                                tableLayoutPanel1.Controls.Remove(ctl);
                                ctl.Dispose();
                            }
                        }
                        if (tableLayoutPanel1.RowStyles.Count > bottomSpacerIndex)
                        {
                            tableLayoutPanel1.RowStyles.RemoveAt(bottomSpacerIndex);
                        }
                        tableLayoutPanel1.RowCount--;
                    }
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private decimal HesaplaOranToplami()
        {
            decimal oranToplami = 0;
            var culture = new System.Globalization.CultureInfo("tr-TR");

            // Adjusted loop start for data rows (skipping header and spacer)
            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                var lblOran = GetLabelAt(row, 2); // Adjusted column index
                if (lblOran != null && decimal.TryParse(lblOran.Text.Replace("%", "").Trim(),
                    System.Globalization.NumberStyles.Any, culture, out decimal oran))
                {
                    oranToplami += oran;
                }
            }
            return oranToplami;
        }

        private bool ParseToplamBedel(out decimal toplamBedel)
        {
            toplamBedel = 0;
            if (string.IsNullOrWhiteSpace(txtToplamBedel.Text))
                return false;

            string cleanText = txtToplamBedel.Text.Split('(')[0].Trim();

            return decimal.TryParse(cleanText,
                                  System.Globalization.NumberStyles.Any,
                                  new System.Globalization.CultureInfo("tr-TR"),
                                  out toplamBedel);
        }

        private decimal ParseToplamBedelOrDefault()
        {
            if (ParseToplamBedel(out decimal total))
            {
                return total;
            }
            return 0m; // Or a default value if needed
        }

        private void UpdateTutarColumn(decimal toplamBedel)
        {
            var culture = new System.Globalization.CultureInfo("tr-TR");

            tableLayoutPanel1.SuspendLayout();
            try
            {
                // Adjusted loop start for data rows
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var lblOran = GetLabelAt(row, 2); // Adjusted column index
                    var txtTutar = GetTextBoxAt(row, 3); // Adjusted column index

                    if (lblOran != null && txtTutar != null)
                    {
                        string oranText = lblOran.Text.Replace("%", "").Trim();
                        if (decimal.TryParse(oranText,
                                           System.Globalization.NumberStyles.Any,
                                           culture,
                                           out decimal oran))
                        {
                            decimal tutar = (toplamBedel * oran) / 100m;
                            string formattedTutar = Math.Round(tutar, 2).ToString("N2", culture);
                            txtTutar.Text = formattedTutar;
                        }
                    }
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private TextBox GetTextBoxAt(int row, int col)
        {
            var ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
            return ctl as TextBox;
        }

        private Label GetLabelAt(int row, int col)
        {
            var ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
            return ctl as Label;
        }

        private RichTextBox GetRichTextBoxAt(int row, int col)
        {
            var ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
            return ctl as RichTextBox;
        }

        private DateTimePicker GetDateTimePickerAt(int row, int col)
        {
            var ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
            return ctl as DateTimePicker;
        }


        private void btnKilometreTasiEkle_Click(object sender, EventArgs e)
        {
            // Proje seçili mi kontrol et
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen önce bir proje seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> alreadySelectedMilestones = new List<string>();

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                Label lblKilometreTasiAdi = tableLayoutPanel1.GetControlFromPosition(1, row) as Label;
                if (lblKilometreTasiAdi != null && !string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text))
                {
                    alreadySelectedMilestones.Add(lblKilometreTasiAdi.Text.Trim());
                }
            }

            using (var frm = new Forms.frmYeniKilometreTasi(alreadySelectedMilestones))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(frm.KilometreTasiAdi) && !string.IsNullOrEmpty(frm.Oran))
                    {
                        AddYeniKilometreTasiSatiri(frm.KilometreTasiAdi, frm.Oran);
                    }
                }
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tableLayoutPanel1.RowCount <= 2 && !_pendingDeletions.Any())
            {
                MessageBox.Show("Kaydedilecek veya silinecek ödeme bilgisi bulunmamaktadır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var projeNo = txtProjeAra.Text.Trim();
            var proje = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            if (proje == null)
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var odemeSekilleriData = new ProjeFinans_OdemeSekilleriData();
            var kilometreTasiData = new ProjeFinans_FiyatlandirmaKilometreTaslariData();
            try
            {
                // Perform pending deletions first
                foreach (var itemToDelete in _pendingDeletions)
                {
                    odemeSekilleriData.DeleteOdemeBilgi(itemToDelete.projeNo, itemToDelete.kilometreTasiId);
                }
                _pendingDeletions.Clear(); // Clear the list after deleting from DB

                // Start from row 1 to skip header, and end before the last row which is the spacer
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    // Debug için hangi satırda olduğumuzu görelim
                    System.Diagnostics.Debug.WriteLine($"Kontrol ediliyor: Satır {row}");

                    // Adjusted column indices for all Get*At methods
                    var lblKilometreTasiAdi = GetLabelAt(row, 1);
                    var lblOran = GetLabelAt(row, 2);
                    var txtTutar = GetTextBoxAt(row, 3);
                    var dtpTahminiTarih = GetDateTimePickerAt(row, 4);
                    var dtpGerceklesenTarih = GetDateTimePickerAt(row, 5);
                    var rtbAciklama = GetRichTextBoxAt(row, 6);

                    var lblDurum = GetLabelAt(row, 11);
                    var pnlTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(7, row) as Panel;
                    var chkTeminatMektubuVar = pnlTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");
                    var lblTeminatDurum = GetLabelAt(row, 8);
                    string teminatDurumu = lblTeminatDurum?.Text; // Null-check eklendi


                    // Hangi kontrolün null veya boş olduğunu görmek için ek Debug.WriteLine'lar
                    // Bu satırları hata tespiti için kullanmaya devam edebilirsiniz.
                    if (lblKilometreTasiAdi == null) System.Diagnostics.Debug.WriteLine($"Satır {row}, lblKilometreTasiAdi null");
                    else if (string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text)) System.Diagnostics.Debug.WriteLine($"Satır {row}, lblKilometreTasiAdi boş");

                    if (lblOran == null) System.Diagnostics.Debug.WriteLine($"Satır {row}, lblOran null");
                    else if (string.IsNullOrWhiteSpace(lblOran.Text)) System.Diagnostics.Debug.WriteLine($"Satır {row}, lblOran boş");

                    if (txtTutar == null) System.Diagnostics.Debug.WriteLine($"Satır {row}, txtTutar null");
                    else if (string.IsNullOrWhiteSpace(txtTutar.Text)) System.Diagnostics.Debug.WriteLine($"Satır {row}, txtTutar boş");

                    if (rtbAciklama == null) System.Diagnostics.Debug.WriteLine($"Satır {row}, rtbAciklama null");
                    else if (string.IsNullOrWhiteSpace(rtbAciklama.Text)) System.Diagnostics.Debug.WriteLine($"Satır {row}, rtbAciklama boş");

                    if (lblDurum == null) System.Diagnostics.Debug.WriteLine($"Satır {row}, lblDurum null");
                    else if (string.IsNullOrWhiteSpace(lblDurum.Text)) System.Diagnostics.Debug.WriteLine($"Satır {row}, lblDurum boş");


                    if (lblKilometreTasiAdi == null || string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text) ||
                        lblOran == null || string.IsNullOrWhiteSpace(lblOran.Text) ||
                        txtTutar == null || string.IsNullOrWhiteSpace(txtTutar.Text) ||
                        rtbAciklama == null || string.IsNullOrWhiteSpace(rtbAciklama.Text) || // <- Burası Açıklama alanını kontrol ediyor
                        lblDurum == null || string.IsNullOrWhiteSpace(lblDurum.Text))
                    {
                        MessageBox.Show($"Satır {row}: Tüm alanlar doldurulmalıdır. Tahmini Tarih ve Gerçekleşen Tarih boş bırakılabilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Eğer bir satırda hata varsa, işlemi durdur.
                    }

                    string kilometreTasiAdi = lblKilometreTasiAdi.Text;
                    int kilometreTasiId = kilometreTasiData.GetKilometreTasiId(kilometreTasiAdi);
                    if (kilometreTasiId == 0)
                    {
                        MessageBox.Show($"Kilometre taşı '{kilometreTasiAdi}' için ID bulunamadı. Lütfen önce kilometre taşını tanımlayın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string oranText = lblOran.Text.Replace("%", "").Trim();
                    if (!decimal.TryParse(oranText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal oran))
                    {
                        MessageBox.Show($"Geçersiz oran formatı: {oranText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string tutarText = txtTutar.Text.Trim();
                    tutarText = tutarText.Replace(".", "").Replace(",", "."); // Türkçe formatı düzelt
                    if (!decimal.TryParse(tutarText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal tutar))
                    {
                        MessageBox.Show($"Geçersiz tutar formatı: {tutarText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string tahminiTarih = dtpTahminiTarih.Checked ? dtpTahminiTarih.Value.ToString("yyyy-MM-dd") : "";
                    string gerceklesenTarih = dtpGerceklesenTarih.Checked ? dtpGerceklesenTarih.Value.ToString("yyyy-MM-dd") : "";
                    string aciklama = rtbAciklama.Text;
                    bool teminatMektubu = chkTeminatMektubuVar != null && chkTeminatMektubuVar.Checked;
                    string durum = lblDurum.Text;
                    int siralama = row; // row'u sıralama olarak kullanıyoruz

                    odemeSekilleriData.SaveOrUpdateOdemeBilgi(
                        projeNo,
                        kilometreTasiId.ToString(),
                        siralama,
                        oran.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                        tutar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                        tahminiTarih,
                        gerceklesenTarih,
                        aciklama,
                        teminatMektubu,
                        teminatDurumu,
                        durum
                    );
                }
                // Başarı mesajı tüm satırlar işlendikten sonra verilmeli
                MessageBox.Show("Ödeme bilgileri başarıyla kaydedildi/güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // After successful save/delete, reload the UI to reflect the actual database state
                LoadOdemeBilgileri(projeNo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödeme bilgileri kaydedilirken/güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnOdendi_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int rowIndex = tableLayoutPanel1.GetRow(button);

                string projeNo = txtProjeAra.Text.Trim();
                if (string.IsNullOrEmpty(projeNo))
                {
                    MessageBox.Show("Proje numarası boş olduğu için ödeme durumu güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Adjusted column indices
                var lblKilometreTasiAdi = GetLabelAt(rowIndex, 1);
                var lblOran = GetLabelAt(rowIndex, 2);
                var txtTutar = GetTextBoxAt(rowIndex, 3);
                var dtpTahminiTarih = GetDateTimePickerAt(rowIndex, 4);
                var dtpGerceklesenTarih = GetDateTimePickerAt(rowIndex, 5);
                var rtbAciklama = GetRichTextBoxAt(rowIndex, 6);
                var pnlTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(7, rowIndex) as Panel;
                var chkTeminatMektubuVar = pnlTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");
                var lblDurum = GetLabelAt(rowIndex, 11);
                var lblTeminatDurum = GetLabelAt(rowIndex, 8);
                string teminatDurumu = lblTeminatDurum.Text;

                if (lblKilometreTasiAdi == null || string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text))
                {
                    MessageBox.Show("Kilometre taşı adı bulunamadı. Ödeme durumu güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var kilometreTasiData = new ProjeFinans_FiyatlandirmaKilometreTaslariData();
                int kilometreTasiId = kilometreTasiData.GetKilometreTasiId(lblKilometreTasiAdi.Text);

                if (kilometreTasiId == 0)
                {
                    MessageBox.Show($"Kilometre taşı '{lblKilometreTasiAdi.Text}' için ID bulunamadı. Ödeme durumu güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string currentStatus = lblDurum.Text;
                string newStatus = "";
                string buttonText = "";
                Color statusColor;
                string confirmationMessage = "";

                if (currentStatus == "Ödendi")
                {
                    newStatus = "Bekliyor";
                    buttonText = "Ödendi Yap";
                    statusColor = Color.Red;
                    confirmationMessage = "Bu ödemeyi 'Bekliyor' olarak işaretlemek istediğinizden emin misiniz?";
                }
                else
                {
                    newStatus = "Ödendi";
                    buttonText = "Bekliyor Yap";
                    statusColor = Color.Green;
                    confirmationMessage = "Bu ödemeyi 'Ödendi' olarak işaretlemek istediğinizden emin misiniz?";
                }

                DialogResult result = MessageBox.Show(
                    confirmationMessage,
                    "Onay",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string oranText = lblOran.Text.Replace("%", "").Trim();
                        decimal oran = decimal.Parse(oranText, System.Globalization.CultureInfo.InvariantCulture);

                        string tutarText = txtTutar.Text.Trim().Replace(".", "").Replace(",", ".");
                        decimal tutar = decimal.Parse(tutarText, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture);

                        string tahminiTarih = dtpTahminiTarih.Checked ? dtpTahminiTarih.Value.ToString("yyyy-MM-dd") : "";
                        string gerceklesenTarih = dtpGerceklesenTarih.Checked ? dtpGerceklesenTarih.Value.ToString("yyyy-MM-dd") : "";
                        string aciklama = rtbAciklama.Text;
                        bool teminatMektubu = chkTeminatMektubuVar != null && chkTeminatMektubuVar.Checked;
                        int siralama = rowIndex;

                        var odemeSekilleriData = new ProjeFinans_OdemeSekilleriData();
                        odemeSekilleriData.SaveOrUpdateOdemeBilgi(
                            projeNo,
                            kilometreTasiId.ToString(),
                            siralama,
                            oran.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                            tutar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                            tahminiTarih,
                            gerceklesenTarih,
                            aciklama,
                            teminatMektubu,
                            teminatDurumu,
                            newStatus
                        );

                        if (lblDurum != null)
                        {
                            lblDurum.Text = newStatus;
                            lblDurum.ForeColor = statusColor;
                        }
                        button.Text = buttonText;

                        MessageBox.Show("Ödeme durumu veritabanında güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ödeme durumu güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (lblDurum != null)
                        {
                            lblDurum.Text = currentStatus;
                            lblDurum.ForeColor = currentStatus == "Ödendi" ? Color.Green : Color.Red;
                        }
                        button.Text = currentStatus == "Ödendi" ? "Bekliyor Yap" : "Ödendi Yap";
                    }
                }
            }
        }
    }
}