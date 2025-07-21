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
        private List<OdemeSekilleri> _newlyAddedMilestones = new List<OdemeSekilleri>(); // Yeni eklenen ancak henüz kaydedilmemiş kilometre taşları için liste

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
                var odemeBilgileriFromDb = odemeSekilleriData.GetOdemeBilgileriByProjeNo(projeNo);

                // Veritabanından gelen verilerle yeni eklenen ancak kaydedilmemiş verileri birleştir
                List<OdemeSekilleri> combinedOdemeBilgileri = new List<OdemeSekilleri>();
                combinedOdemeBilgileri.AddRange(odemeBilgileriFromDb);
                // Sadece mevcut projeye ait yeni eklenmiş kilometre taşlarını ekle
                combinedOdemeBilgileri.AddRange(_newlyAddedMilestones.Where(m => m.projeNo == projeNo && m.kilometreTasiId == 0)); // Sadece yeni eklenenleri ekle

                // Silinmek üzere işaretlenmiş öğeleri filtrele
                var filteredOdemeBilgileri = combinedOdemeBilgileri.Where(odemeBilgi =>
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

                    // Silme Simgesi PictureBox
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
                    pbDelete.Tag = odemeBilgi;
                    tableLayoutPanel1.Controls.Add(pbDelete, 0, newRowIndex);

                    var lblKilometreTasiAdi = new Label { Text = odemeBilgi.kilometreTasiAdi, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };

                    var lblOran = new Label { Text = $"%{odemeBilgi.oran:F0}", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };

                    var txtTutar = new TextBox
                    {
                        Text = odemeBilgi.tutar.ToString("N2", System.Globalization.CultureInfo.CurrentCulture),
                        Dock = DockStyle.Fill,
                        TextAlign = HorizontalAlignment.Center,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 10),
                        BorderStyle = BorderStyle.FixedSingle,
                        Enabled = false,
                        BackColor = Color.LightGray
                    };

                    var dtpTahminiTarih = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Margin = new Padding(2), Font = new Font("Segoe UI", 10), Height = 30, MaximumSize = new Size(0, 30) };
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

                    // *** EKLEME BAŞLANGICI: Tahmini Tarih DateTimePicker'ın Enabled durumu ***
                    // kilometreTasiId 0 ise (yani yeni, kaydedilmemiş bir kayıt ise) aktif, değilse pasif.
                    dtpTahminiTarih.Enabled = (odemeBilgi.kilometreTasiId == 0);
                    // *** EKLEME SONU ***

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
                        Enabled = odemeBilgi.teminatMektubu
                    };
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
                            var lblTempTeminatDurum = GetLabelAt(newRowIndex, 8);
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
                            var lblTempTeminatDurum = GetLabelAt(newRowIndex, 8);
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

                        var lblKmTasiAdi = GetLabelAt(rowIndex, 1);
                        var txtTutarFatura = GetTextBoxAt(rowIndex, 3);
                        var rtbAciklamaFatura = tableLayoutPanel1.GetControlFromPosition(6, rowIndex) as RichTextBox;
                        var dtpTahminiTarihFatura = tableLayoutPanel1.GetControlFromPosition(4, rowIndex) as DateTimePicker;
                        var dtpGerceklesenTarihFatura = tableLayoutPanel1.GetControlFromPosition(5, rowIndex) as DateTimePicker;

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

                if (rowIndex <= 0 || rowIndex >= tableLayoutPanel1.RowCount - 1)
                {
                    return;
                }

                var odemeBilgi = pictureBox.Tag as OdemeSekilleri;

                tableLayoutPanel1.SuspendLayout();
                try
                {
                    // Case 1: Mevcut bir satırı silme (veritabanından)
                    if (odemeBilgi != null && odemeBilgi.kilometreTasiId != 0)
                    {
                        _pendingDeletions.Add((txtProjeAra.Text.Trim(), odemeBilgi.kilometreTasiId));
                        MessageBox.Show("Satır başarıyla silinmek üzere işaretlendi. Kaydet butonuna tıklayarak veritabanından silmeyi onaylayın.", "Silme Onayı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // Case 2: Yeni eklenmiş, henüz kaydedilmemiş bir satırı silme
                    else if (odemeBilgi != null && odemeBilgi.kilometreTasiId == 0)
                    {
                        _newlyAddedMilestones.RemoveAll(m => m.projeNo == odemeBilgi.projeNo && m.kilometreTasiAdi == odemeBilgi.kilometreTasiAdi);
                        MessageBox.Show("Yeni eklenen satır başarıyla kaldırıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Silme işlemi başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    LoadOdemeBilgileri(txtProjeAra.Text.Trim());

                    btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
                }
                finally
                {
                    tableLayoutPanel1.ResumeLayout(true);
                }
            }
        }



        private void btnAra_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen aranacak Proje No girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projeNo = txtProjeAra.Text.Trim();
            Console.WriteLine($"Girilen projeNo: '{projeNo}'");

            // ProjeBilgi al
            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            Console.WriteLine($"projeBilgi: {(projeBilgi != null ? "Bulundu" : "Bulunamadı")}");

            // ProjeKutuk al
            var projeKutuk = CEKA_APP.DataBase.ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
            Console.WriteLine($"projeKutuk: {(projeKutuk != null ? "Bulundu" : "Bulunamadı")}");

            // Alt proje kontrolü
            bool isAltProje = ProjeFinans_ProjeIliskiData.CheckAltProje(projeNo);
            Console.WriteLine($"isAltProje: {isAltProje}");

            if (isAltProje)
            {
                var ustProjeNo = ProjeFinans_ProjeIliskiData.GetUstProjeNo(projeNo);
                if (!string.IsNullOrEmpty(ustProjeNo))
                {
                    var ustProjeKutuk = CEKA_APP.DataBase.ProjeFinans_ProjeKutukData.ProjeKutukAra(ustProjeNo);
                    bool ustTekil = CEKA_APP.DataBase.ProjeFinans_ProjeKutukData.IsFaturalamaSekliTekil(ustProjeNo);

                    if (ustProjeKutuk != null && ustTekil)
                    {
                        MessageBox.Show("Proje Kütük Faturalama Şekli Tekil olduğundan bu projeye ödeme şekli giremezsiniz.",
                                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            if (projeBilgi != null)
            {
                var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
                List<string> altProjeler = null;

                if (isAltProje)
                {
                    altProjeler = ProjeFinans_ProjeIliskiData.GetAltProjeler(projeNo);
                }
                else if (projeKutuk != null && projeKutuk.altProjeVarMi)
                {
                    altProjeler = projeKutuk.altProjeBilgileri?.ToList() ?? new List<string>();
                }

                var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);

                UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler);
                UpdateTutarColumn(toplamBedel);

                _pendingDeletions.Clear();
                _newlyAddedMilestones.Clear();
                LoadOdemeBilgileri(projeNo);

                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            else
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            AddHeaderRow();
        }

        private void AddHeaderRow()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.RowCount = 0;

                tableLayoutPanel1.RowCount = 1;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                int headerRowIndex = 0;

                string[] headers = {
            "",
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
                if (tableLayoutPanel1.RowCount > 1)
                {
                    Control lastControlInFirstCol = tableLayoutPanel1.GetControlFromPosition(0, tableLayoutPanel1.RowCount - 1);
                    if (lastControlInFirstCol is Label lbl && string.IsNullOrEmpty(lbl.Text) && lbl.Height == 10)
                    {
                        return;
                    }
                }

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

        private void RemoveBottomSpacer()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                if (tableLayoutPanel1.RowCount > 1)
                {
                    int bottomSpacerIndex = tableLayoutPanel1.RowCount - 1;
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

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                var lblOran = GetLabelAt(row, 2);
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
            return 0m;
        }

        private void UpdateTutarColumn(decimal toplamBedel)
        {
            var culture = new System.Globalization.CultureInfo("tr-TR");

            tableLayoutPanel1.SuspendLayout();
            try
            {
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var lblOran = GetLabelAt(row, 2);
                    var txtTutar = GetTextBoxAt(row, 3);

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
                        string cleanOran = frm.Oran.Replace("%", "").Trim();
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

                        OdemeSekilleri newMilestone = new OdemeSekilleri
                        {
                            projeNo = txtProjeAra.Text.Trim(),
                            kilometreTasiId = 0, // 0 yeni, kaydedilmemiş bir giriş olduğunu gösterir
                            kilometreTasiAdi = frm.KilometreTasiAdi,
                            oran = yeniOran,
                            tutar = (ParseToplamBedelOrDefault() * yeniOran) / 100m,
                            tahminiTarih = null,
                            gerceklesenTarih = null,
                            aciklama = "",
                            teminatMektubu = false,
                            teminatDurumu = "Pasif",
                            durum = "Bekliyor"
                        };

                        _newlyAddedMilestones.Add(newMilestone);
                        LoadOdemeBilgileri(txtProjeAra.Text.Trim()); // Tabloyu yenile
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

            if (tableLayoutPanel1.RowCount <= 2 && !_pendingDeletions.Any() && !_newlyAddedMilestones.Any())
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
                // Bekleyen silmeleri gerçekleştir
                foreach (var itemToDelete in _pendingDeletions)
                {
                    odemeSekilleriData.DeleteOdemeBilgi(itemToDelete.projeNo, itemToDelete.kilometreTasiId);
                }
                _pendingDeletions.Clear(); // Veritabanından sildikten sonra listeyi temizle

                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    System.Diagnostics.Debug.WriteLine($"Kontrol ediliyor: Satır {row}");

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
                    string teminatDurumu = lblTeminatDurum?.Text;


                    if (lblKilometreTasiAdi == null || string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text) ||
                        lblOran == null || string.IsNullOrWhiteSpace(lblOran.Text) ||
                        txtTutar == null || string.IsNullOrWhiteSpace(txtTutar.Text) ||
                        rtbAciklama == null || string.IsNullOrWhiteSpace(rtbAciklama.Text) ||
                        lblDurum == null || string.IsNullOrWhiteSpace(lblDurum.Text))
                    {
                        MessageBox.Show($"Satır {row}: Tüm alanlar doldurulmalıdır. Tahmini Tarih ve Gerçekleşen Tarih boş bırakılabilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string kilometreTasiAdi = lblKilometreTasiAdi.Text;
                    int kilometreTasiId = kilometreTasiData.GetKilometreTasiId(kilometreTasiAdi);

                    if (kilometreTasiId == 0)
                    {
                        MessageBox.Show($"Kilometre taşı '{kilometreTasiAdi}' için ID bulunamadı. Lütfen önce kilometre taşını tanımlayın veya geçerli bir kilometre taşı seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string oranText = lblOran.Text.Replace("%", "").Trim();
                    if (!decimal.TryParse(oranText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal oran))
                    {
                        MessageBox.Show($"Geçersiz oran formatı: {oranText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string tutarText = txtTutar.Text.Trim();
                    tutarText = tutarText.Replace(".", "").Replace(",", ".");
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
                    int siralama = row;

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
                MessageBox.Show("Ödeme bilgileri başarıyla kaydedildi/güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _newlyAddedMilestones.Clear(); // Kaydedildikten sonra yeni eklenenleri temizle

                // *** EKLEME BAŞLANGICI: Kaydetme sonrası UI yenileme ***
                // Bu, yeni kaydedilen satırların dtpTahminiTarih'inin pasif olmasını sağlar.
                LoadOdemeBilgileri(projeNo);
                // *** EKLEME SONU ***
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

                        LoadOdemeBilgileri(projeNo);

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