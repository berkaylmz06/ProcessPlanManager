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

namespace CEKA_APP.UsrControl
{
    public partial class ctlOdemeSartlari : UserControl
    {
        private int _draggedRowIndex = -1;
        private Point _mouseDownLocation;

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
            tableLayoutPanel1.ColumnCount = 11; 
            tableLayoutPanel1.ColumnStyles.Clear();

            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f)); 
            for (int i = 1; i < 11; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            }

            btnKilometreTasiEkle.Enabled = false;

            tableLayoutPanel1.AllowDrop = true;
            tableLayoutPanel1.DragEnter += TableLayoutPanel1_DragEnter;
            tableLayoutPanel1.DragDrop += TableLayoutPanel1_DragDrop;
        }
        private void LoadOdemeBilgileri(string projeNo)
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                for (int i = tableLayoutPanel1.RowCount - 1; i >= 1; i--)
                {
                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                    {
                        Control ctl = tableLayoutPanel1.GetControlFromPosition(col, i);
                        if (ctl != null)
                        {
                            tableLayoutPanel1.Controls.Remove(ctl);
                            ctl.Dispose();
                        }
                    }
                    if (tableLayoutPanel1.RowStyles.Count > i)
                    {
                        tableLayoutPanel1.RowStyles.RemoveAt(i);
                    }
                    tableLayoutPanel1.RowCount--;
                }
                tableLayoutPanel1.RowCount = 1;


                var odemeSekilleriData = new ProjeFinans_OdemeSekilleriData();
                var odemeBilgileri = odemeSekilleriData.GetOdemeBilgileriByProjeNo(projeNo);

                foreach (var odemeBilgi in odemeBilgileri.OrderBy(o => o.siralama))
                {
                    int newRowIndex = tableLayoutPanel1.RowCount; 
                    tableLayoutPanel1.RowCount++;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                    var lblKilometreTasiAdi = new Label { Text = odemeBilgi.kilometreTasiAdi, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };
                    lblKilometreTasiAdi.MouseDown += LblKilometreTasiAdi_MouseDown;
                    lblKilometreTasiAdi.MouseMove += LblKilometreTasiAdi_MouseMove;

                    var lblOran = new Label { Text = $"%{odemeBilgi.oran:F0}", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };

                    var txtTutar = new TextBox { Text = odemeBilgi.tutar.ToString("N2", System.Globalization.CultureInfo.CurrentCulture), Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle, Enabled = false, BackColor = Color.LightGray };

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


                    tableLayoutPanel1.Controls.Add(lblKilometreTasiAdi, 0, newRowIndex);
                    tableLayoutPanel1.Controls.Add(lblOran, 1, newRowIndex);
                    tableLayoutPanel1.Controls.Add(txtTutar, 2, newRowIndex);
                    tableLayoutPanel1.Controls.Add(dtpTahminiTarih, 3, newRowIndex);
                    tableLayoutPanel1.Controls.Add(dtpGerceklesenTarih, 4, newRowIndex);
                    tableLayoutPanel1.Controls.Add(rtbAciklama, 5, newRowIndex);

                    tableLayoutPanel1.Controls.Add(pnlTeminatMektubu, 6, newRowIndex);   
                    tableLayoutPanel1.Controls.Add(newLblTeminatDurum, 7, newRowIndex);
                    tableLayoutPanel1.Controls.Add(btnFaturalama, 8, newRowIndex);      
                    tableLayoutPanel1.Controls.Add(lblDurum, 9, newRowIndex);           

                    chkTeminatMektubuVar.CheckedChanged += (s, e) =>
                    {
                        if (chkTeminatMektubuVar.Checked)
                        {
                            chkTeminatMektubuYok.Checked = false;
                            var lblTempTeminatDurum = GetLabelAt(newRowIndex, 7); 
                            if (lblTempTeminatDurum != null)
                            {
                                lblTempTeminatDurum.Text = "Aktif";
                                lblTempTeminatDurum.ForeColor = Color.Green;
                            }
                        }
                    };

                    chkTeminatMektubuYok.CheckedChanged += (s, e) =>
                    {
                        if (chkTeminatMektubuYok.Checked)
                        {
                            chkTeminatMektubuVar.Checked = false;
                            var lblTempTeminatDurum = GetLabelAt(newRowIndex, 7);
                            if (lblTempTeminatDurum != null)
                            {
                                lblTempTeminatDurum.Text = "Pasif";
                                lblTempTeminatDurum.ForeColor = Color.Red;
                            }
                        }
                    };


                    btnFaturalama.Click += (s, e) =>
                    {
                        var button = s as Button;
                        int rowIndex = tableLayoutPanel1.GetRow(button);

                        var lblKmTasiAdi = GetLabelAt(rowIndex, 0);
                        var txtTutarFatura = GetTextBoxAt(rowIndex, 2);
                        var rtbAciklamaFatura = tableLayoutPanel1.GetControlFromPosition(5, rowIndex) as RichTextBox;
                        var dtpTahminiTarihFatura = tableLayoutPanel1.GetControlFromPosition(3, rowIndex) as DateTimePicker;
                        var dtpGerceklesenTarihFatura = tableLayoutPanel1.GetControlFromPosition(4, rowIndex) as DateTimePicker;

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

                    var btnOdendi = new Button
                    {
                        Text = "Ödendi",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        AutoSize = true,
                        Height = 30,
                        Font = new Font("Segoe UI", 10)
                    };
                    btnOdendi.Click += BtnOdendi_Click; 
                    tableLayoutPanel1.Controls.Add(btnOdendi, 10, newRowIndex); 
                }

                AddBottomSpacer();

                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }
        private void btnAra_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen aranacak Proje No girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnKilometreTasiEkle.Enabled = false;
                return;
            }

            var projeNo = txtProjeAra.Text.Trim();
            var proje = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);

            if (proje != null)
            {
                if (IsAltProje(projeNo, proje))
                {
                    var result = MessageBox.Show(
                        "Alt proje için ödeme verisi giriyorsunuz, devam etmek istiyor musunuz?",
                        "Uyarı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        btnKilometreTasiEkle.Enabled = false;
                        return;
                    }
                }

                var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
                var altProjeler = proje.altProjeVarMi
                    ? proje.altProjeBilgileri?.ToList() ?? new List<string>()
                    : null;
                var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);
                UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler);
                UpdateTutarColumn(toplamBedel);

                LoadOdemeBilgileri(projeNo); 

                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            else
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtToplamBedel.Text = "";
                txtToplamBedel.ForeColor = Color.Black;
                btnKilometreTasiEkle.Enabled = false;

                LoadOdemeBilgileri(string.Empty);
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

        private void ctlOdemeSartlari_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Ödeme Şartları";
            AddHeaderRow();
            AddBottomSpacer();
        }

        private void AddHeaderRow()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                int headerRowIndex = 0;
                if (tableLayoutPanel1.RowCount > 0)
                {
                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                    {
                        var ctl = tableLayoutPanel1.GetControlFromPosition(col, headerRowIndex);
                        if (ctl != null)
                        {
                            tableLayoutPanel1.Controls.Remove(ctl);
                        }
                    }
                    if (tableLayoutPanel1.RowStyles.Count > headerRowIndex)
                    {
                        tableLayoutPanel1.RowStyles.RemoveAt(headerRowIndex);
                    }
                    tableLayoutPanel1.RowCount = 1;
                }

                tableLayoutPanel1.RowCount = 1;
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                headerRowIndex = 0;

                string[] headers = {
                    "Proje Kilometre Taşları",
                    "Oran (%)",
                    "Tutar",
                    "Tahmini Tarih",
                    "Gerçekleşen Tarih",
                    "Açıklama",
                    "Teminat Mektubu",    
                    "Teminat Durumu",     
                    "Faturalama",       
                    "Durum",   
                    "Ödendi"     
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
                        BackColor = Color.LightGray
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
                int bottomSpacerIndex = tableLayoutPanel1.RowCount - 1;
                if (bottomSpacerIndex >= 0) 
                {
                    var existingSpacerControl = tableLayoutPanel1.GetControlFromPosition(0, bottomSpacerIndex);
                    if (existingSpacerControl is Label lbl && string.IsNullOrEmpty(lbl.Text))
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

                var lblKilometreTasiAdi = new Label { Text = kilometreTasiAdi, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };
                lblKilometreTasiAdi.MouseDown += LblKilometreTasiAdi_MouseDown;
                lblKilometreTasiAdi.MouseMove += LblKilometreTasiAdi_MouseMove;

                tableLayoutPanel1.Controls.Add(lblKilometreTasiAdi, 0, newRowIndex);
                tableLayoutPanel1.Controls.Add(new Label { Text = $"%{cleanOran}", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) }, 1, newRowIndex);
                tableLayoutPanel1.Controls.Add(new TextBox { Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle, Enabled = false, BackColor = Color.LightGray }, 2, newRowIndex);
                tableLayoutPanel1.Controls.Add(new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Margin = new Padding(2), Font = new Font("Segoe UI", 10), Height = 30, MaximumSize = new Size(0, 30) }, 3, newRowIndex);
                tableLayoutPanel1.Controls.Add(new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Margin = new Padding(2), Font = new Font("Segoe UI", 10), Height = 30, MaximumSize = new Size(0, 30) }, 4, newRowIndex);
                tableLayoutPanel1.Controls.Add(new RichTextBox { Dock = DockStyle.Fill, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle, ScrollBars = RichTextBoxScrollBars.Vertical }, 5, newRowIndex);


                var pnlTeminatMektubu = new Panel { Dock = DockStyle.Fill, Margin = new Padding(2), Height = 30 };
                var chkTeminatMektubuVar = new CheckBox
                {
                    Text = "Var",
                    Checked = false,
                    Dock = DockStyle.Left,
                    Margin = new Padding(2),
                    Width = 50,
                    Font = new Font("Segoe UI", 10)
                };
                var chkTeminatMektubuYok = new CheckBox
                {
                    Text = "Yok",
                    Checked = true,
                    Dock = DockStyle.Right,
                    Margin = new Padding(2),
                    Width = 50,
                    Font = new Font("Segoe UI", 10)
                };
                pnlTeminatMektubu.Controls.Add(chkTeminatMektubuVar);
                pnlTeminatMektubu.Controls.Add(chkTeminatMektubuYok);
                tableLayoutPanel1.Controls.Add(pnlTeminatMektubu, 6, newRowIndex); 

                var newLblTeminatDurum = new Label
                {
                    Text = "Pasif",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(2),
                    ForeColor = Color.Red,
                    AutoSize = false,
                    Height = 30,
                    MaximumSize = new Size(0, 30),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };
                tableLayoutPanel1.Controls.Add(newLblTeminatDurum, 7, newRowIndex); 

                var btnFaturalama = new Button
                {
                    Text = "Fatura Oluştur",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(2),
                    AutoSize = true,
                    Height = 30,
                    Font = new Font("Segoe UI", 10)
                };
                tableLayoutPanel1.Controls.Add(btnFaturalama, 8, newRowIndex); 

                var lblDurum = new Label { Text = "Bekliyor", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), ForeColor = Color.Red, AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) };
                tableLayoutPanel1.Controls.Add(lblDurum, 9, newRowIndex); 


                var btnOdendi = new Button
                {
                    Text = "Ödendi",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(2),
                    AutoSize = true,
                    Height = 30,
                    Font = new Font("Segoe UI", 10)
                };
                btnOdendi.Click += BtnOdendi_Click; 
                tableLayoutPanel1.Controls.Add(btnOdendi, 10, newRowIndex); 


                chkTeminatMektubuVar.CheckedChanged += (s, e) =>
                {
                    if (chkTeminatMektubuVar.Checked)
                    {
                        chkTeminatMektubuYok.Checked = false;
                        var lblTempTeminatDurum = GetLabelAt(newRowIndex, 7); 
                        if (lblTempTeminatDurum != null)
                        {
                            lblTempTeminatDurum.Text = "Aktif";
                            lblTempTeminatDurum.ForeColor = Color.Green;
                            var frm = new frmTeminatMektubuEkle();
                            frm.ShowDialog();
                        }
                    }
                };

                chkTeminatMektubuYok.CheckedChanged += (s, e) =>
                {
                    if (chkTeminatMektubuYok.Checked)
                    {
                        chkTeminatMektubuVar.Checked = false;
                        var lblTempTeminatDurum = GetLabelAt(newRowIndex, 7); 
                        if (lblTempTeminatDurum != null)
                        {
                            lblTempTeminatDurum.Text = "Pasif";
                            lblTempTeminatDurum.ForeColor = Color.Red;
                        }
                    }
                };

                btnFaturalama.Click += (s, e) =>
                {
                    var button = s as Button;
                    int rowIndex = tableLayoutPanel1.GetRow(button);

                    var lblKilometreTasiAdiClick = GetLabelAt(rowIndex, 0);
                    var txtTutar = GetTextBoxAt(rowIndex, 2);
                    var rtbAciklama = GetRichTextBoxAt(rowIndex, 5);
                    var dtpTahminiTarih = GetDateTimePickerAt(rowIndex, 3);
                    var dtpGerceklesenTarih = GetDateTimePickerAt(rowIndex, 4);

                    if (lblKilometreTasiAdiClick == null || txtTutar == null || rtbAciklama == null ||
                        string.IsNullOrWhiteSpace(lblKilometreTasiAdiClick.Text) ||
                        string.IsNullOrWhiteSpace(txtTutar.Text) ||
                        string.IsNullOrWhiteSpace(rtbAciklama.Text))
                    {
                        MessageBox.Show("Seçili satırda gerekli bilgiler eksik veya boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string selectedTarih = "Belirtilmemiş";
                    if (dtpTahminiTarih != null && dtpGerceklesenTarih != null)
                    {
                        if (dtpTahminiTarih.Checked && dtpGerceklesenTarih.Checked)
                        {
                            if (dtpTahminiTarih.Value.Date == dtpGerceklesenTarih.Value.Date)
                            {
                                selectedTarih = dtpTahminiTarih.Value.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                selectedTarih = dtpGerceklesenTarih.Value.ToString("yyyy-MM-dd");
                            }
                        }
                        else if (dtpTahminiTarih.Checked)
                        {
                            selectedTarih = dtpTahminiTarih.Value.ToString("yyyy-MM-dd");
                        }
                        else if (dtpGerceklesenTarih.Checked)
                        {
                            selectedTarih = dtpGerceklesenTarih.Value.ToString("yyyy-MM-dd");
                        }
                    }

                    var frm = new frmFaturaOlustur(
                        txtTutar.Text,
                        rtbAciklama.Text,
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
                int bottomSpacerIndex = tableLayoutPanel1.RowCount - 1;
                if (bottomSpacerIndex >= 0) 
                {
                    var existingSpacerControl = tableLayoutPanel1.GetControlFromPosition(0, bottomSpacerIndex);
                    if (existingSpacerControl is Label lbl && string.IsNullOrEmpty(lbl.Text))
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
                var lblOran = GetLabelAt(row, 1);
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

        private void UpdateTutarColumn(decimal toplamBedel)
        {
            var culture = new System.Globalization.CultureInfo("tr-TR");

            tableLayoutPanel1.SuspendLayout();
            try
            {
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var lblOran = GetLabelAt(row, 1);
                    var txtTutar = GetTextBoxAt(row, 2);

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
            var frm = new frmYeniKilometreTasi();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                AddYeniKilometreTasiSatiri(frm.KilometreTasiAdi, frm.Oran);
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projeNo = txtProjeAra.Text.Trim();
            var proje = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
            if (proje == null)
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var odemeSekilleriData = new ProjeFinans_OdemeSekilleriData();
            var kilometreTasiData = new ProjeFinans_FiyatlandirmaKilometreTaslariData();
            try
            {
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++) 
                {
                    var lblKilometreTasiAdi = GetLabelAt(row, 0);
                    var lblOran = GetLabelAt(row, 1);
                    var txtTutar = GetTextBoxAt(row, 2);
                    var dtpTahminiTarih = GetDateTimePickerAt(row, 3);
                    var dtpGerceklesenTarih = GetDateTimePickerAt(row, 4);
                    var rtbAciklama = GetRichTextBoxAt(row, 5); 

                    var lblDurum = GetLabelAt(row, 9); 
                    var pnlTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(6, row) as Panel; 
                    var chkTeminatMektubuVar = pnlTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");
                    var lblTeminatDurum = GetLabelAt(row, 7);

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
                        MessageBox.Show($"Kilometre taşı '{kilometreTasiAdi}' için ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    string durum = lblDurum.Text;
                    bool teminatMektubu = chkTeminatMektubuVar != null && chkTeminatMektubuVar.Checked;
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
                        durum
                    );

                    if (proje.altProjeVarMi && proje.altProjeBilgileri != null && !IsAltProje(projeNo, proje))
                    {
                        foreach (var altProjeNo in proje.altProjeBilgileri)
                        {
                            var altProje = ProjeFinans_ProjeKutukData.ProjeKutukAra(altProjeNo);
                            if (altProje != null)
                            {
                                decimal altToplamBedel = 0;
                                var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
                                var (bedel, _) = fiyatlandirmaData.GetToplamBedel(altProjeNo, null);
                                altToplamBedel = bedel;

                                string altTutar = altToplamBedel > 0
                                    ? (altToplamBedel * (oran / 100)).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                                    : tutar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

                                odemeSekilleriData.SaveOrUpdateOdemeBilgi( 
                                    altProjeNo,
                                    kilometreTasiId.ToString(),
                                    siralama,
                                    oran.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                                    altTutar,
                                    tahminiTarih,
                                    gerceklesenTarih,
                                    aciklama,
                                    teminatMektubu,
                                    durum
                                );
                            }
                        }
                    }
                }

                MessageBox.Show("Ödeme bilgileri kaydedildi/güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödeme bilgileri kaydedilirken/güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LblKilometreTasiAdi_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseDownLocation = e.Location;
                Label draggedLabel = sender as Label;
                if (draggedLabel != null)
                {
                    _draggedRowIndex = tableLayoutPanel1.GetRow(draggedLabel);
                }
            }
        }

        private void LblKilometreTasiAdi_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _draggedRowIndex != -1)
            {
                if (Math.Abs(e.X - _mouseDownLocation.X) > SystemInformation.DragSize.Width ||
                    Math.Abs(e.Y - _mouseDownLocation.Y) > SystemInformation.DragSize.Height)
                {
                    tableLayoutPanel1.DoDragDrop(_draggedRowIndex, DragDropEffects.Move);
                    _draggedRowIndex = -1; 
                }
            }
        }

        private void TableLayoutPanel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(int)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TableLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(int)))
            {
                int fromRow = (int)e.Data.GetData(typeof(int));

                Point clientPoint = tableLayoutPanel1.PointToClient(new Point(e.X, e.Y));
                int toRow = GetRowIndexFromPointAccurate(clientPoint);

                if (toRow >= 1 && toRow <= tableLayoutPanel1.RowCount - 2 && fromRow != toRow)
                {
                    ReorderTableRowsUI(fromRow, toRow);

                    UpdateDatabaseOrder();
                }
            }
        }
        private int GetRowIndexFromPointAccurate(Point point)
        {
            int row = -1;
            int currentY = 0;
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                int rowHeight = tableLayoutPanel1.GetRowHeights()[i]; 

                if (point.Y >= currentY && point.Y < currentY + rowHeight)
                {
                    row = i;
                    break;
                }
                currentY += rowHeight;
            }
            return row;
        }

        private void ReorderTableRowsUI(int fromRow, int toRow)
        {
            tableLayoutPanel1.SuspendLayout(); 
            try
            {
                if (fromRow < 0 || toRow < 0 || fromRow >= tableLayoutPanel1.RowCount || toRow >= tableLayoutPanel1.RowCount || fromRow == toRow)
                    return; 

                Control[] draggedControls = new Control[tableLayoutPanel1.ColumnCount];
                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    draggedControls[col] = tableLayoutPanel1.GetControlFromPosition(col, fromRow);
                    if (draggedControls[col] != null)
                    {
                        tableLayoutPanel1.Controls.Remove(draggedControls[col]);
                    }
                }

                if (fromRow < toRow)
                {
                    for (int i = fromRow + 1; i <= toRow; i++)
                    {
                        for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                        {
                            Control ctl = tableLayoutPanel1.GetControlFromPosition(col, i);
                            if (ctl != null)
                            {
                                tableLayoutPanel1.SetRow(ctl, i - 1);
                            }
                        }
                    }
                }
                else 
                {
                    for (int i = fromRow - 1; i >= toRow; i--)
                    {
                        for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                        {
                            Control ctl = tableLayoutPanel1.GetControlFromPosition(col, i);
                            if (ctl != null)
                            {
                                tableLayoutPanel1.SetRow(ctl, i + 1);
                            }
                        }
                    }
                }

                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    if (draggedControls[col] != null)
                    {
                        tableLayoutPanel1.Controls.Add(draggedControls[col], col, toRow);
                    }
                }

                tableLayoutPanel1.PerformLayout();
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }
        private void UpdateDatabaseOrder()
        {
            var reorderedData = new List<Tuple<string, int, int>>(); 
            var odemeSekilleriData = new ProjeFinans_OdemeSekilleriData();
            var kilometreTasiData = new ProjeFinans_FiyatlandirmaKilometreTaslariData(); 

            string projeNo = txtProjeAra.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Proje numarası boş olduğu için sıralama güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++) 
            {
                string kilometreTasiAdi = GetLabelAt(row, 0)?.Text;
                if (!string.IsNullOrEmpty(kilometreTasiAdi))
                {
                    int kilometreTasiId = kilometreTasiData.GetKilometreTasiId(kilometreTasiAdi);
                    if (kilometreTasiId != 0)
                    {
                        reorderedData.Add(Tuple.Create(projeNo, kilometreTasiId, row)); 
                    }
                }
            }

            if (reorderedData.Any())
            {
                try
                {
                    odemeSekilleriData.UpdateOrder(reorderedData);
                    MessageBox.Show("Sıralama veritabanında güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOdemeBilgileri(projeNo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Veritabanı sıralaması güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnOdendi_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int rowIndex = tableLayoutPanel1.GetRow(button);

                DialogResult result = MessageBox.Show(
                    "Bu ödemeyi 'Ödendi' olarak işaretlemek istediğinizden emin misiniz?",
                    "Onay",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    string projeNo = txtProjeAra.Text.Trim();
                    if (string.IsNullOrEmpty(projeNo))
                    {
                        MessageBox.Show("Proje numarası boş olduğu için ödeme durumu güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var lblKilometreTasiAdi = GetLabelAt(rowIndex, 0);
                    var lblOran = GetLabelAt(rowIndex, 1);
                    var txtTutar = GetTextBoxAt(rowIndex, 2);
                    var dtpTahminiTarih = GetDateTimePickerAt(rowIndex, 3);
                    var dtpGerceklesenTarih = GetDateTimePickerAt(rowIndex, 4);
                    var rtbAciklama = GetRichTextBoxAt(rowIndex, 5);
                    var pnlTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(6, rowIndex) as Panel; 
                    var chkTeminatMektubuVar = pnlTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");


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

                    var lblDurum = GetLabelAt(rowIndex, 9);
                    if (lblDurum != null)
                    {
                        lblDurum.Text = "Ödendi";
                        lblDurum.ForeColor = Color.Green;
                    }

                    var odemeSekilleriData = new ProjeFinans_OdemeSekilleriData();
                    try
                    {
                        string oranText = lblOran.Text.Replace("%", "").Trim();
                        decimal oran = decimal.Parse(oranText, System.Globalization.CultureInfo.InvariantCulture);

                        string tutarText = txtTutar.Text.Trim().Replace(".", "").Replace(",", "."); 
                        decimal tutar = decimal.Parse(tutarText, System.Globalization.CultureInfo.InvariantCulture);

                        string tahminiTarih = dtpTahminiTarih.Checked ? dtpTahminiTarih.Value.ToString("yyyy-MM-dd") : "";
                        string gerceklesenTarih = dtpGerceklesenTarih.Checked ? dtpGerceklesenTarih.Value.ToString("yyyy-MM-dd") : "";
                        string aciklama = rtbAciklama.Text;
                        bool teminatMektubu = chkTeminatMektubuVar != null && chkTeminatMektubuVar.Checked;
                        int siralama = rowIndex;

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
                            "Ödendi"
                        );
                        MessageBox.Show("Ödeme durumu veritabanında güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ödeme durumu güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (lblDurum != null)
                        {
                            lblDurum.Text = "Bekliyor";
                            lblDurum.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }
    }
}