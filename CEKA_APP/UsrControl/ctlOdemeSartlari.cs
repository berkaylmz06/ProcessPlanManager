using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys;
using CEKA_APP.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlOdemeSartlari : UserControl
    {
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
            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < 10; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
            }

            btnKilometreTasiEkle.Enabled = false;
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
            var proje = ProjeKutukData.ProjeKutukAra(projeNo);

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

                var fiyatlandirmaData = new ProjeFiyatlandirmaData();
                var altProjeler = proje.altProjeVarMi
                    ? proje.altProjeBilgileri?.ToList() ?? new List<string>()
                    : null;
                var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);
                UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler);
                UpdateTutarColumn(toplamBedel);
                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            else
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtToplamBedel.Text = "";
                txtToplamBedel.ForeColor = Color.Black;
                btnKilometreTasiEkle.Enabled = false;
            }
        }

        private bool IsAltProje(string projeNo, ProjeKutuk proje)
        {
            if (proje.altProjeVarMi)
                return false;
            return ProjeKutukData.CheckAltProje(projeNo);
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
                var projeKutuk = ProjeKutukData.ProjeKutukAra(projeNo);
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
                    "Durum",
                    "Teminat Mektubu",
                    "Teminat Durumu",
                    "Faturalama"
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
                if (bottomSpacerIndex > 0 && tableLayoutPanel1.GetControlFromPosition(0, bottomSpacerIndex) is Label lbl && lbl.Text == "")
                {
                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                    {
                        var ctl = tableLayoutPanel1.GetControlFromPosition(col, bottomSpacerIndex);
                        if (ctl != null)
                        {
                            tableLayoutPanel1.Controls.Remove(ctl);
                        }
                    }
                    tableLayoutPanel1.RowStyles.RemoveAt(bottomSpacerIndex);
                    tableLayoutPanel1.RowCount--;
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

                int newRowIndex = tableLayoutPanel1.RowCount - 1;
                tableLayoutPanel1.RowCount++;
                tableLayoutPanel1.RowStyles.Insert(newRowIndex, new RowStyle(SizeType.Absolute, 30));

                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    var spacerControl = tableLayoutPanel1.GetControlFromPosition(col, tableLayoutPanel1.RowCount - 2);
                    if (spacerControl != null)
                    {
                        tableLayoutPanel1.SetRow(spacerControl, tableLayoutPanel1.RowCount - 1);
                    }
                }

                var controls = new Control[]
                {
            new Label { Text = kilometreTasiAdi, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) },
            new Label { Text = $"%{cleanOran}", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) },
            new TextBox { Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle, Enabled = false, BackColor = Color.LightGray },
            new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Margin = new Padding(2), Font = new Font("Segoe UI", 10), Height = 30, MaximumSize = new Size(0, 30) },
            new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Margin = new Padding(2), Font = new Font("Segoe UI", 10), Height = 30, MaximumSize = new Size(0, 30) },
            new TextBox { Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = new Padding(2), AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10), BorderStyle = BorderStyle.FixedSingle, Multiline = true },
            new Label { Text = "Bekliyor", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(2), ForeColor = Color.Black, AutoSize = false, Height = 30, MaximumSize = new Size(0, 30), Font = new Font("Segoe UI", 10) }
                };

                for (int col = 0; col < 7; col++)
                {
                    tableLayoutPanel1.Controls.Add(controls[col], col, newRowIndex);
                }

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
                tableLayoutPanel1.Controls.Add(pnlTeminatMektubu, 7, newRowIndex);

                chkTeminatMektubuVar.CheckedChanged += (s, e) =>
                {
                    if (chkTeminatMektubuVar.Checked)
                    {
                        chkTeminatMektubuYok.Checked = false;
                        var lblTeminatDurum = GetLabelAt(newRowIndex, 8);
                        if (lblTeminatDurum != null)
                        {
                            lblTeminatDurum.Text = "Aktif";
                            lblTeminatDurum.ForeColor = Color.Green;
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
                        var lblTeminatDurum = GetLabelAt(newRowIndex, 8);
                        if (lblTeminatDurum != null)
                        {
                            lblTeminatDurum.Text = "Pasif";
                            lblTeminatDurum.ForeColor = Color.Red;
                        }
                    }
                };

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
                tableLayoutPanel1.Controls.Add(newLblTeminatDurum, 8, newRowIndex);

                var btnFaturalama = new Button
                {
                    Text = "Fatura Oluştur",
                    Dock = DockStyle.Fill,
                    Margin = new Padding(2),
                    AutoSize = true,
                    Height = 30,
                    Font = new Font("Segoe UI", 10)
                };
                btnFaturalama.Click += (s, e) =>
                {
                    var button = s as Button;
                    int rowIndex = tableLayoutPanel1.GetRow(button);

                    var lblKilometreTasiAdi = GetLabelAt(rowIndex, 0);
                    var txtTutar = GetTextBoxAt(rowIndex, 2);
                    var txtAciklama = GetTextBoxAt(rowIndex, 5);
                    var dtpTahminiTarih = tableLayoutPanel1.GetControlFromPosition(3, rowIndex) as DateTimePicker;
                    var dtpGerceklesenTarih = tableLayoutPanel1.GetControlFromPosition(4, rowIndex) as DateTimePicker;

                    if (lblKilometreTasiAdi == null || txtTutar == null || txtAciklama == null ||
                        string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text) ||
                        string.IsNullOrWhiteSpace(txtTutar.Text) ||
                        string.IsNullOrWhiteSpace(txtAciklama.Text))
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
                        txtAciklama.Text,
                        selectedTarih,
                        txtProjeAra.Text.Trim()
                    );
                    frm.ShowDialog();
                };
                tableLayoutPanel1.Controls.Add(btnFaturalama, 9, newRowIndex);

                if (ParseToplamBedel(out decimal toplamBedel))
                {
                    UpdateTutarColumn(toplamBedel);
                }
                else
                {
                    UpdateTutarColumn(1000m);
                }

                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
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
            var proje = ProjeKutukData.ProjeKutukAra(projeNo);
            if (proje == null)
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var odemeSekilleriData = new ProjeFinans_OdemeSekilleriData();
            var kilometreTasiData = new FiyatlandirmaKilometreTaslariData();
            try
            {
                for (int row = 2; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var lblKilometreTasiAdi = GetLabelAt(row, 0);
                    var lblOran = GetLabelAt(row, 1);
                    var txtTutar = GetTextBoxAt(row, 2);
                    var dtpTahminiTarih = tableLayoutPanel1.GetControlFromPosition(3, row) as DateTimePicker;
                    var dtpGerceklesenTarih = tableLayoutPanel1.GetControlFromPosition(4, row) as DateTimePicker;
                    var txtAciklama = GetTextBoxAt(row, 5);
                    var lblDurum = GetLabelAt(row, 6);
                    var pnlTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(7, row) as Panel;
                    var chkTeminatMektubuVar = pnlTeminatMektubu?.Controls[0] as CheckBox;
                    var lblTeminatDurum = GetLabelAt(row, 8);

                    if (lblKilometreTasiAdi == null || string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text) ||
                        lblOran == null || string.IsNullOrWhiteSpace(lblOran.Text) ||
                        txtTutar == null || string.IsNullOrWhiteSpace(txtTutar.Text) ||
                        dtpTahminiTarih == null || !dtpTahminiTarih.Checked ||
                        txtAciklama == null || string.IsNullOrWhiteSpace(txtAciklama.Text) ||
                        lblDurum == null || string.IsNullOrWhiteSpace(lblDurum.Text))
                    {
                        MessageBox.Show($"Satır {row}: Tüm alanlar doldurulmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    string aciklama = txtAciklama.Text;
                    string durum = lblDurum.Text;
                    bool teminatMektubu = chkTeminatMektubuVar != null && chkTeminatMektubuVar.Checked; 
                    int siralama = row;

                    odemeSekilleriData.OdemeBilgiKaydet(
                        projeNo,
                        kilometreTasiId.ToString(),
                        siralama,
                        oran.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                        tutar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                        tahminiTarih,
                        aciklama,
                        teminatMektubu,
                        durum
                    );

                    if (proje.altProjeVarMi && proje.altProjeBilgileri != null && !IsAltProje(projeNo, proje))
                    {
                        foreach (var altProjeNo in proje.altProjeBilgileri)
                        {
                            var altProje = ProjeKutukData.ProjeKutukAra(altProjeNo);
                            if (altProje != null)
                            {
                                decimal altToplamBedel = 0;
                                var fiyatlandirmaData = new ProjeFiyatlandirmaData();
                                var (bedel, _) = fiyatlandirmaData.GetToplamBedel(altProjeNo, null);
                                altToplamBedel = bedel;

                                string altTutar = altToplamBedel > 0
                                    ? (altToplamBedel * (oran / 100)).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                                    : tutar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

                                odemeSekilleriData.OdemeBilgiKaydet(
                                    altProjeNo,
                                    kilometreTasiId.ToString(),
                                    siralama,
                                    oran.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                                    altTutar,
                                    tahminiTarih,
                                    aciklama,
                                    teminatMektubu,
                                    durum
                                );
                            }
                        }
                    }
                }

                MessageBox.Show("Ödeme bilgileri kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödeme bilgileri kaydedilirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}