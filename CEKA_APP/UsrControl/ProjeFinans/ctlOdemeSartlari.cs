﻿using CEKA_APP.DataBase;
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
using CEKA_APP.Entitys.ProjeFinans;
using System.Globalization; // Add this for CultureInfo

namespace CEKA_APP.UsrControl
{
    public partial class ctlOdemeSartlari : UserControl
    {
        private List<(string projeNo, int kilometreTasiId)> _pendingDeletions = new List<(string projeNo, int kilometreTasiId)>();
        private List<OdemeSartlari> _newlyAddedMilestones = new List<OdemeSartlari>();

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

            tableLayoutPanel1.ColumnCount = 14;
            tableLayoutPanel1.ColumnStyles.Clear();

            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));

            btnKilometreTasiEkle.Enabled = false;
            tableLayoutPanel1.AllowDrop = false;

            chkTutarTamaminiKullan.CheckedChanged += ChkTutarTamaminiKullan_CheckedChanged;
            txtEksilenTutar.TextChanged += TxtEksilenTutar_TextChanged;
        }

        private void TxtCikarilacakTutar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) && ((sender as TextBox).Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) > -1))
            {
                e.Handled = true;
            }
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

                // Üst proje kontrolü
                bool isUpperProject = !ProjeFinans_ProjeIliskiData.CheckAltProje(projeNo);
                if (!isUpperProject)
                {
                    tableLayoutPanel1.ColumnStyles[8].Width = 0; // Teminat Mektubu
                    tableLayoutPanel1.ColumnStyles[9].Width = 0; // Teminat Durumu
                    tableLayoutPanel1.ColumnStyles[10].Width = 0; // Mektup Geri Al
                }
                else
                {
                    tableLayoutPanel1.ColumnStyles[8].Width = 10f; // Teminat Mektubu
                    tableLayoutPanel1.ColumnStyles[9].Width = 5f;  // Teminat Durumu
                    tableLayoutPanel1.ColumnStyles[10].Width = 5f; // Mektup Geri Al
                }

                var odemeSekilleriData = new ProjeFinans_OdemeSartlariData();
                var odemeBilgileriFromDb = odemeSekilleriData.GetOdemeBilgileriByProjeNo(projeNo);

                List<OdemeSartlari> combinedOdemeBilgileri = new List<OdemeSartlari>();
                combinedOdemeBilgileri.AddRange(odemeBilgileriFromDb);
                combinedOdemeBilgileri.AddRange(_newlyAddedMilestones.Where(m => m.projeNo == projeNo && m.kilometreTasiId == 0));

                var filteredOdemeBilgileri = combinedOdemeBilgileri
                    .Where(odemeBilgi => !_pendingDeletions.Any(pd => pd.projeNo == projeNo && pd.kilometreTasiId == odemeBilgi.kilometreTasiId))
                    .OrderBy(o => o.tahminiTarih.HasValue ? 0 : 1)
                    .ThenBy(o => o.tahminiTarih.HasValue ? o.tahminiTarih.Value : DateTime.MaxValue)
                    .ToList();

                foreach (var odemeBilgi in filteredOdemeBilgileri)
                {
                    int newRowIndex = tableLayoutPanel1.RowCount;
                    tableLayoutPanel1.RowCount++;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                    CheckBox chkSelect = new CheckBox();
                    chkSelect.AutoSize = true;
                    chkSelect.Tag = odemeBilgi.kilometreTasiId;
                    chkSelect.CheckedChanged += new System.EventHandler(this.chkSelect_CheckedChanged);
                    chkSelect.Margin = new Padding(3, 10, 3, 3);
                    tableLayoutPanel1.Controls.Add(chkSelect, 0, newRowIndex);

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
                    tableLayoutPanel1.Controls.Add(pbDelete, 1, newRowIndex);

                    var lblKilometreTasiAdi = new Label { Text = odemeBilgi.kilometreTasiAdi, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };
                    var lblOran = new Label { Text = $"%{odemeBilgi.oran:F0}", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };
                    var txtTutar = new TextBox
                    {
                        Text = odemeBilgi.tutar.ToString("N2", CultureInfo.CurrentCulture),
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

                    var txtKalanTutar = new TextBox
                    {
                        Text = odemeBilgi.kalanTutar.ToString("N2", CultureInfo.CurrentCulture),
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
                    dtpTahminiTarih.Enabled = (odemeBilgi.kilometreTasiId == 0);

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
                        var lblTempTeminatDurum = GetLabelAt(rowIdx, 9);

                        var pnlTempTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(8, rowIdx) as Panel;
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

                    tableLayoutPanel1.Controls.Add(lblKilometreTasiAdi, 2, newRowIndex);
                    tableLayoutPanel1.Controls.Add(lblOran, 3, newRowIndex);
                    tableLayoutPanel1.Controls.Add(txtTutar, 4, newRowIndex);
                    tableLayoutPanel1.Controls.Add(dtpTahminiTarih, 5, newRowIndex);
                    tableLayoutPanel1.Controls.Add(dtpGerceklesenTarih, 6, newRowIndex);
                    tableLayoutPanel1.Controls.Add(rtbAciklama, 7, newRowIndex);
                    if (isUpperProject)
                    {
                        tableLayoutPanel1.Controls.Add(pnlTeminatMektubu, 8, newRowIndex);
                        tableLayoutPanel1.Controls.Add(newLblTeminatDurum, 9, newRowIndex);
                        tableLayoutPanel1.Controls.Add(btnMektupGeriAl, 10, newRowIndex);
                    }
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

                    tableLayoutPanel1.Controls.Add(txtKalanTutar, 13, newRowIndex);

                    if (isUpperProject)
                    {
                        chkTeminatMektubuVar.CheckedChanged += (s, e) =>
                        {
                            if (chkTeminatMektubuVar.Checked)
                            {
                                chkTeminatMektubuYok.Checked = false;
                                var lblTempTeminatDurum = GetLabelAt(newRowIndex, 9);
                                var frm = new frmTeminatMektubuEkle(null, txtProjeAra.Text.Trim());
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
                                var lblTempTeminatDurum = GetLabelAt(newRowIndex, 9);
                                if (lblTempTeminatDurum != null)
                                {
                                    lblTempTeminatDurum.Text = "Pasif";
                                    lblTempTeminatDurum.ForeColor = Color.Red;
                                    btnMektupGeriAl.Enabled = false;
                                }
                            }
                        };
                    }
                }

                AddBottomSpacer();
                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
                UpdateBulkInvoiceButtonText();
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

                var odemeBilgi = pictureBox.Tag as OdemeSartlari;

                tableLayoutPanel1.SuspendLayout();
                try
                {
                    if (odemeBilgi != null && odemeBilgi.kilometreTasiId != 0)
                    {
                        _pendingDeletions.Add((txtProjeAra.Text.Trim(), odemeBilgi.kilometreTasiId));
                        MessageBox.Show("Satır başarıyla silinmek üzere işaretlendi. Kaydet butonuna tıklayarak veritabanından silmeyi onaylayın.", "Silme Onayı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            Console.WriteLine($"projeBilgi: {(projeBilgi != null ? "Bulundu" : "Bulunamadı")}");

            var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
            Console.WriteLine($"projeKutuk: {(projeKutuk != null ? "Bulundu" : "Bulunamadı")}");

            bool isAltProje = ProjeFinans_ProjeIliskiData.CheckAltProje(projeNo);
            Console.WriteLine($"isAltProje: {isAltProje}");

            if (isAltProje)
            {
                var ustProjeNo = ProjeFinans_ProjeIliskiData.GetUstProjeNo(projeNo);
                if (!string.IsNullOrEmpty(ustProjeNo))
                {
                    var ustProjeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(ustProjeNo);
                    bool ustTekil = ProjeFinans_ProjeKutukData.IsFaturalamaSekliTekil(ustProjeNo);

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

                lblToplamBedelBilgi.Text = "";
                
                UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler);

                _pendingDeletions.Clear();
                _newlyAddedMilestones.Clear();
                LoadOdemeBilgileri(projeNo);
                UpdateTutarColumn(toplamBedel);

                UpdateBulkInvoiceButtonText();

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
                        txtToplamBedel.Text = $"{toplamBedel:F2}";
                        txtToplamBedel.ForeColor = Color.Red;
                        lblToplamBedelBilgi.Text = $"Alt projeler: {string.Join(", ", eksikFiyatlandirmaProjeler)} için fiyatlandırma yok";
                        lblToplamBedelBilgi.ForeColor = Color.Red;
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
            UpdateBulkInvoiceButtonText();
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
                    "Sil",
                    "Proje Kilometre Taşları",
                    "Oran (%)",
                    "Tutar",
                    "Tahmini Tarih",
                    "Gerçekleşen Tarih",
                    "Açıklama",
                    "Teminat Mektubu",
                    "Teminat Durumu",
                    "Mektup Geri Al",
                    "Durum",
                    "Durum Değiştir",
                    "Kalan Tutar"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    var lbl = new Label
                    {
                        Text = headers[i],
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
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

        private decimal HesaplaOranToplami()
        {
            decimal oranToplami = 0;
            var culture = new CultureInfo("tr-TR");

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                var lblOran = GetLabelAt(row, 3);
                if (lblOran != null && decimal.TryParse(lblOran.Text.Replace("%", "").Trim(),
                    NumberStyles.Any, culture, out decimal oran))
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

            return decimal.TryParse(cleanText, NumberStyles.Any, new CultureInfo("tr-TR"), out toplamBedel);
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
            var culture = new CultureInfo("tr-TR");

            tableLayoutPanel1.SuspendLayout();
            try
            {
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var lblOran = GetLabelAt(row, 3);
                    var txtTutar = GetTextBoxAt(row, 4);
                    var txtKalanTutar = GetTextBoxAt(row, 13);

                    if (lblOran != null && txtTutar != null && txtKalanTutar != null)
                    {
                        string oranText = lblOran.Text.Replace("%", "").Trim();
                        if (decimal.TryParse(oranText,
                                           NumberStyles.Any,
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
                Label lblKilometreTasiAdi = tableLayoutPanel1.GetControlFromPosition(2, row) as Label;
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
                        if (!decimal.TryParse(cleanOran, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal yeniOran))
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

                        decimal tutar = (ParseToplamBedelOrDefault() * yeniOran) / 100m;

                        OdemeSartlari newMilestone = new OdemeSartlari
                        {
                            projeNo = txtProjeAra.Text.Trim(),
                            kilometreTasiId = 0,
                            kilometreTasiAdi = frm.KilometreTasiAdi,
                            oran = yeniOran,
                            tutar = tutar,
                            kalanTutar = tutar,
                            tahminiTarih = null,
                            gerceklesenTarih = null,
                            aciklama = "",
                            teminatMektubu = false,
                            teminatDurumu = "Pasif",
                            durum = "Bekliyor"
                        };

                        _newlyAddedMilestones.Add(newMilestone);
                        LoadOdemeBilgileri(txtProjeAra.Text.Trim());
                        UpdateBulkInvoiceButtonText();
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

            var odemeSekilleriData = new ProjeFinans_OdemeSartlariData();
            var kilometreTasiData = new ProjeFinans_FiyatlandirmaKilometreTaslariData();
            try
            {
                // Silme işlemlerini gerçekleştir
                foreach (var itemToDelete in _pendingDeletions)
                {
                    odemeSekilleriData.DeleteOdemeBilgi(itemToDelete.projeNo, itemToDelete.kilometreTasiId);
                }
                _pendingDeletions.Clear();

                // Her satırı kaydet veya güncelle
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var lblKilometreTasiAdi = GetLabelAt(row, 2);
                    var lblOran = GetLabelAt(row, 3);
                    var txtTutar = GetTextBoxAt(row, 4);
                    var txtKalanTutar = GetTextBoxAt(row, 13);
                    var dtpTahminiTarih = GetDateTimePickerAt(row, 5);
                    var dtpGerceklesenTarih = GetDateTimePickerAt(row, 6);
                    var rtbAciklama = GetRichTextBoxAt(row, 7);
                    var lblDurum = GetLabelAt(row, 11);
                    var pnlTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(8, row) as Panel;
                    var chkTeminatMektubuVar = pnlTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");
                    var lblTeminatDurum = GetLabelAt(row, 9);
                    string teminatDurumu = lblTeminatDurum?.Text;

                    if (lblKilometreTasiAdi == null || string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text) ||
                        lblOran == null || string.IsNullOrWhiteSpace(lblOran.Text) ||
                        txtTutar == null || string.IsNullOrWhiteSpace(txtTutar.Text) ||
                        txtKalanTutar == null || string.IsNullOrWhiteSpace(txtKalanTutar.Text) ||
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
                    if (!decimal.TryParse(oranText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal oran))
                    {
                        MessageBox.Show($"Geçersiz oran formatı: {oranText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string tutarText = txtTutar.Text.Trim();
                    if (!decimal.TryParse(tutarText, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out decimal tutar))
                    {
                        MessageBox.Show($"Geçersiz tutar formatı: {tutarText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string kalanTutarText = txtKalanTutar.Text.Trim();
                    decimal? kalanTutar = null;
                    if (!string.IsNullOrEmpty(kalanTutarText) && decimal.TryParse(kalanTutarText, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out decimal parsedKalanTutar))
                    {
                        kalanTutar = parsedKalanTutar;
                    }

                    string tahminiTarih = dtpTahminiTarih.Checked ? dtpTahminiTarih.Value.ToString("yyyy-MM-dd") : "";
                    string gerceklesenTarih = dtpGerceklesenTarih.Checked ? dtpGerceklesenTarih.Value.ToString("yyyy-MM-dd") : "";
                    string aciklama = rtbAciklama.Text;
                    bool teminatMektubu = chkTeminatMektubuVar != null && chkTeminatMektubuVar.Checked;
                    string durum = lblDurum.Text;
                    int siralama = row;

                    // Mevcut faturaNo değerini veritabanından al
                    string faturaNo = odemeSekilleriData.GetFaturaNo(projeNo, kilometreTasiId) ?? "";

                    odemeSekilleriData.SaveOrUpdateOdemeBilgi(
                        projeNo,
                        kilometreTasiId,
                        siralama,
                        oran.ToString("F2", CultureInfo.InvariantCulture),
                        tutar.ToString("F2", CultureInfo.InvariantCulture),
                        tahminiTarih,
                        gerceklesenTarih,
                        aciklama,
                        teminatMektubu,
                        teminatDurumu,
                        durum,
                        faturaNo,
                        kalanTutar?.ToString("F2", CultureInfo.InvariantCulture) ?? string.Empty
                    );
                }
                MessageBox.Show("Ödeme bilgileri başarıyla kaydedildi/güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _newlyAddedMilestones.Clear();
                LoadOdemeBilgileri(projeNo);
                UpdateBulkInvoiceButtonText();
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

                var lblKilometreTasiAdi = GetLabelAt(rowIndex, 2);
                var lblOran = GetLabelAt(rowIndex, 3);
                var txtTutar = GetTextBoxAt(rowIndex, 4);
                var txtKalanTutar = GetTextBoxAt(rowIndex, 13);
                var dtpTahminiTarih = GetDateTimePickerAt(rowIndex, 5);
                var dtpGerceklesenTarih = GetDateTimePickerAt(rowIndex, 6);
                var rtbAciklama = GetRichTextBoxAt(rowIndex, 7);
                var txtFaturaNo = GetTextBoxAt(rowIndex, 13);
                var pnlTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(8, rowIndex) as Panel;
                var chkTeminatMektubuVar = pnlTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");
                var lblDurum = GetLabelAt(rowIndex, 11);
                var lblTeminatDurum = GetLabelAt(rowIndex, 9);
                string teminatDurumu = lblTeminatDurum?.Text;

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
                        decimal oran = decimal.Parse(oranText, CultureInfo.InvariantCulture);

                        string tutarText = txtTutar.Text.Trim();
                        decimal tutar = decimal.Parse(tutarText, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture);

                        string kalanTutarText = txtKalanTutar.Text.Trim();
                        decimal kalanTutar = 0m;
                        if (!string.IsNullOrEmpty(kalanTutarText) && decimal.TryParse(kalanTutarText, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out decimal parsedKalanTutar))
                        {
                            kalanTutar = parsedKalanTutar;
                        }

                        string tahminiTarih = dtpTahminiTarih.Checked ? dtpTahminiTarih.Value.ToString("yyyy-MM-dd") : "";
                        string gerceklesenTarih = dtpGerceklesenTarih.Checked ? dtpGerceklesenTarih.Value.ToString("yyyy-MM-dd") : "";
                        string aciklama = rtbAciklama.Text;
                        bool teminatMektubu = chkTeminatMektubuVar != null && chkTeminatMektubuVar.Checked;
                        int siralama = rowIndex;
                        string faturaNo = "";

                        var odemeSekilleriData = new ProjeFinans_OdemeSartlariData();
                        odemeSekilleriData.SaveOrUpdateOdemeBilgi(
                            projeNo,
                            kilometreTasiId,
                            siralama,
                            oran.ToString("F2", CultureInfo.InvariantCulture),
                            tutar.ToString("F2", CultureInfo.InvariantCulture),
                            tahminiTarih,
                            gerceklesenTarih,
                            aciklama,
                            teminatMektubu,
                            teminatDurumu,
                            newStatus,
                            faturaNo,
                            kalanTutar.ToString("F2", CultureInfo.InvariantCulture)
                        );

                        LoadOdemeBilgileri(projeNo);
                        UpdateBulkInvoiceButtonText();

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

        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBulkInvoiceButtonText();
        }

        private void UpdateBulkInvoiceButtonText()
        {
            int checkedCount = 0;
            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                CheckBox chk = tableLayoutPanel1.GetControlFromPosition(0, row) as CheckBox;
                if (chk != null && chk.Checked)
                {
                    checkedCount++;
                }
            }

            if (checkedCount >= 1)
            {
                btnTopluFaturaOlustur.Text = "Seçili Satırları Faturala";
            }
            else
            {
                btnTopluFaturaOlustur.Text = "Toplu Fatura Oluştur";
            }
        }

        private void btnTopluFaturaOlustur_Click(object sender, EventArgs e)
        {
            List<int> selectedRowIndexes = new List<int>();
            decimal totalTutar = 0m;
            string musteriAdi = txtProjeAra.Text.Trim();
            var culture = CultureInfo.CurrentCulture;

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                CheckBox chk = tableLayoutPanel1.GetControlFromPosition(0, row) as CheckBox;
                if (chk != null && chk.Checked)
                {
                    selectedRowIndexes.Add(row);
                }
            }

            if (selectedRowIndexes.Count == 1)
            {
                int rowIndex = selectedRowIndexes[0];

                var lblKmTasiAdi = GetLabelAt(rowIndex, 2);
                var txtTutarFatura = GetTextBoxAt(rowIndex, 4);
                var rtbAciklamaFatura = tableLayoutPanel1.GetControlFromPosition(7, rowIndex) as RichTextBox;
                var dtpTahminiTarihFatura = tableLayoutPanel1.GetControlFromPosition(5, rowIndex) as DateTimePicker;
                var dtpGerceklesenTarihFatura = tableLayoutPanel1.GetControlFromPosition(6, rowIndex) as DateTimePicker;
                var checkBox = tableLayoutPanel1.GetControlFromPosition(0, rowIndex) as CheckBox;

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

                int? odemeId = null;
                var kilometreTasiData = new ProjeFinans_FiyatlandirmaKilometreTaslariData();
                int kilometreTasiId = kilometreTasiData.GetKilometreTasiId(lblKmTasiAdi.Text);

                if (checkBox != null && checkBox.Tag != null && int.TryParse(checkBox.Tag.ToString(), out int tagId))
                {
                    var odemeData = new ProjeFinans_OdemeSartlariData();
                    var odemeInfo = odemeData.GetOdemeBilgileriByProjeNo(txtProjeAra.Text.Trim())
                        .FirstOrDefault(o => o.kilometreTasiId == tagId);
                    odemeId = odemeInfo?.odemeId;
                }

                if (!odemeId.HasValue)
                {
                    MessageBox.Show($"Satır {rowIndex} için ödeme ID'si bulunamadı. Lütfen geçerli bir kayıt seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var frm = new frmFaturaOlustur(
                    tutar: txtTutarFatura.Text,
                    aciklama: rtbAciklamaFatura.Text,
                    tarih: selectedTarih,
                    musteriAdi: musteriAdi,
                    kilometreTasiId: kilometreTasiId,
                    odemeId: odemeId
                );
                frm.ShowDialog();
            }
            else
            {
                if (selectedRowIndexes.Count == 0)
                {
                    for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                    {
                        TextBox txtTutar = GetTextBoxAt(row, 4);
                        if (txtTutar != null && decimal.TryParse(txtTutar.Text, NumberStyles.Currency | NumberStyles.Number, culture, out decimal tutar))
                        {
                            totalTutar += tutar;
                        }
                    }
                }
                else
                {
                    foreach (int rowIndex in selectedRowIndexes)
                    {
                        TextBox txtTutar = GetTextBoxAt(rowIndex, 4);
                        if (txtTutar != null && decimal.TryParse(txtTutar.Text, NumberStyles.Currency | NumberStyles.Number, culture, out decimal tutar))
                        {
                            totalTutar += tutar;
                        }
                    }
                }

                var frm = new frmFaturaOlustur(
                    totalTutar.ToString("N2", culture),
                    "",
                    "",
                    musteriAdi
                );
                frm.ShowDialog();
            }
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen önce bir proje seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEksilenTutar.Text))
            {
                MessageBox.Show("Lütfen çıkarılacak tutarı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var culture = CultureInfo.CurrentCulture;
            if (!decimal.TryParse(txtEksilenTutar.Text, NumberStyles.Any, culture, out decimal cikarilacakDeger))
            {
                MessageBox.Show("Geçersiz tutar formatı. Lütfen sayısal bir değer girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<int> selectedRowIndexes = new List<int>();
            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                CheckBox chk = tableLayoutPanel1.GetControlFromPosition(0, row) as CheckBox;
                if (chk != null && chk.Checked)
                {
                    selectedRowIndexes.Add(row);
                }
            }

            if (selectedRowIndexes.Count == 0)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Hiçbir satır seçilmedi. Girilen tutar tüm satırlardaki 'Kalan Tutar' alanlarından çıkarılacaktır. Devam etmek istiyor musunuz?",
                    "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    selectedRowIndexes.Add(row);
                }
            }

            tableLayoutPanel1.SuspendLayout();
            try
            {
                foreach (int rowIndex in selectedRowIndexes)
                {
                    TextBox txtKalanTutar = GetTextBoxAt(rowIndex, 13);
                    Label lblDurum = GetLabelAt(rowIndex, 11);

                    if (txtKalanTutar != null)
                    {
                        decimal currentKalanTutar = 0m;
                        if (decimal.TryParse(txtKalanTutar.Text, NumberStyles.Any, culture, out currentKalanTutar))
                        {
                            decimal newKalanTutar = currentKalanTutar - cikarilacakDeger;
                            if (newKalanTutar < 0)
                            {
                                MessageBox.Show($"Satır {rowIndex}'deki 'Kalan Tutar' değeri çıkarılan tutardan daha küçük olamaz. İşlem iptal edildi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            txtKalanTutar.Text = newKalanTutar.ToString("N2", culture);

                            if (newKalanTutar == 0 && lblDurum != null)
                            {
                                lblDurum.Text = "Ödendi";
                                lblDurum.ForeColor = Color.Green;
                            }
                            else if (newKalanTutar > 0 && lblDurum != null && lblDurum.Text == "Ödendi")
                            {

                            }
                        }
                        else
                        {
                            TextBox txtTutar = GetTextBoxAt(rowIndex, 4);
                            if (txtTutar != null && decimal.TryParse(txtTutar.Text, NumberStyles.Any, culture, out decimal currentTutar))
                            {
                                decimal newKalanTutar = currentTutar - cikarilacakDeger;
                                if (newKalanTutar < 0)
                                {
                                    MessageBox.Show($"Satır {rowIndex}'deki 'Tutar' değeri çıkarılan tutardan daha küçük olamaz. İşlem iptal edildi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                txtKalanTutar.Text = newKalanTutar.ToString("N2", culture);

                                if (newKalanTutar == 0 && lblDurum != null)
                                {
                                    lblDurum.Text = "Ödendi";
                                    lblDurum.ForeColor = Color.Green;
                                }
                                else if (newKalanTutar > 0 && lblDurum != null && lblDurum.Text == "Ödendi")
                                {

                                }
                            }
                        }
                    }
                }
                MessageBox.Show("Tutar çıkarma işlemi tamamlandı. Kaydetmek için 'Kaydet' butonuna basın.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private void ChkTutarTamaminiKullan_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTutarTamaminiKullan.Checked)
            {
                decimal totalKalanTutar = 0m;
                var culture = CultureInfo.CurrentCulture;
                List<int> selectedRowIndexes = new List<int>();

                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    CheckBox chk = tableLayoutPanel1.GetControlFromPosition(0, row) as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        selectedRowIndexes.Add(row);
                    }
                }

                if (selectedRowIndexes.Count > 0)
                {
                    foreach (int rowIndex in selectedRowIndexes)
                    {
                        TextBox txtKalanTutar = GetTextBoxAt(rowIndex, 13);
                        if (txtKalanTutar != null && decimal.TryParse(txtKalanTutar.Text, NumberStyles.Any, culture, out decimal kalanTutar))
                        {
                            totalKalanTutar += kalanTutar;
                        }
                    }
                }
                else
                {
                    for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                    {
                        TextBox txtKalanTutar = GetTextBoxAt(row, 13);
                        if (txtKalanTutar != null && decimal.TryParse(txtKalanTutar.Text, NumberStyles.Any, culture, out decimal kalanTutar))
                        {
                            totalKalanTutar += kalanTutar;
                        }
                    }
                }
                txtEksilenTutar.Text = totalKalanTutar.ToString("N2", culture);
            }
        }

        private void TxtEksilenTutar_TextChanged(object sender, EventArgs e)
        {
            if (chkTutarTamaminiKullan.Checked)
            {
                chkTutarTamaminiKullan.CheckedChanged -= ChkTutarTamaminiKullan_CheckedChanged;
                chkTutarTamaminiKullan.Checked = false;
                chkTutarTamaminiKullan.CheckedChanged += ChkTutarTamaminiKullan_CheckedChanged;
            }
        }
    }
}