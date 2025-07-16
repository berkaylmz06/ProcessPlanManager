using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeFiyatlandirma : UserControl
    {
        private ProjeFinans_FiyatlandirmaKalemleriData iscilikData = new ProjeFinans_FiyatlandirmaKalemleriData();
        private ProjeFinans_FiyatlandirmaData fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
        public event Action<string> OnFiyatlandirmaKaydedildi;
        private ComboBox cmbProjeNo;
        private List<string> altProjeler;

        public ctlProjeFiyatlandirma()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            cmbProjeNo = new ComboBox
            {
                Size = txtProjeNo.Size,
                Location = txtProjeNo.Location,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Visible = false,
                Font = new Font("Segoe UI", 10)
            };
            cmbProjeNo.SelectedIndexChanged += (s, e) =>
            {
                if (cmbProjeNo.SelectedItem != null)
                {
                    LoadIscilikler(cmbProjeNo.SelectedItem.ToString());
                }
            };
            panelUst.Controls.Add(cmbProjeNo);
            cmbProjeNo.BringToFront();

            btnYeniKalemEkle.Enabled = false;
            btnKaydet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        private void ctlProjeFiyatlandirma_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Proje Fiyatlandırma";
            panelUst.AutoScroll = true;

            // Tablo ayarları
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.AutoSize = false;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.MinimumSize = new Size(tableLayoutPanel1.Width, 200);

            // Sütun ayarları
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < 8; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            }

            // Başlangıçta başlık ve boş satır ekle
            InitializeTableStructure();

            // Alt panel ayarları
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            tableLayoutPanel2.ColumnCount = 8;
            tableLayoutPanel2.ColumnStyles.Clear();
            for (int i = 0; i < 8; i++)
            {
                tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            }
        }

        private void InitializeTableStructure()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                // Tüm kontrolleri temizle
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.RowCount = 0;

                // Başlık satırını ekle
                AddHeaderRow();

                // Boş spacer satırını ekle
                AddSpacerRow();
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        public void LoadProjeFiyatlandirma(string projeNo, bool autoSearch = false, List<string> altProjeler = null)
        {
            this.altProjeler = altProjeler;

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Proje '{projeNo}' ProjeFinans_Projeler tablosunda bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
            if (projeKutuk != null && projeKutuk.altProjeVarMi && altProjeler == null)
            {
                MessageBox.Show($"Proje '{projeNo}' alt projelere sahip. Lütfen bir alt proje seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtProjeNo.Text = projeNo;

            if (altProjeler != null && altProjeler.Any())
            {
                txtProjeNo.Visible = false;
                cmbProjeNo.Visible = true;
                cmbProjeNo.Items.Clear();
                cmbProjeNo.Items.AddRange(altProjeler.ToArray());
                cmbProjeNo.SelectedIndex = altProjeler.Contains(projeNo) ? altProjeler.IndexOf(projeNo) : 0;
                cmbProjeNo.BringToFront();
            }
            else
            {
                txtProjeNo.Visible = true;
                cmbProjeNo.Visible = false;
            }

            if (autoSearch)
            {
                LoadIscilikler(projeNo);
            }
        }

        private void LoadIscilikler(string projeNo)
        {
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnCount = 8;
                tableLayoutPanel1.RowCount = 1;
                tableLayoutPanel1.RowStyles.Clear();

                tableLayoutPanel1.ColumnStyles.Clear();
                for (int i = 0; i < 8; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
                }

                AddHeaderRow();
                AddSpacerRow();
                UpdateTotalsRow();

                var fiyatlandirmaVerileri = fiyatlandirmaData.GetFiyatlandirmaByProje(projeNo);
                if (fiyatlandirmaVerileri.Any())
                {
                    foreach (var veri in fiyatlandirmaVerileri)
                    {
                        if (string.IsNullOrEmpty(veri.kalemAdi)) continue;
                        AddYeniKalemSatiri(veri.kalemAdi);
                        int row = tableLayoutPanel1.RowCount - 1;

                        var txtTeklifAdet = GetTextBoxAt(row, 1);
                        var txtTeklifBirimFiyat = GetTextBoxAt(row, 2);
                        var txtTeklifToplam = GetTextBoxAt(row, 3);
                        var txtGerceklesenAdet = GetTextBoxAt(row, 4);
                        var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 5);
                        var txtGerceklesenMaliyet = GetTextBoxAt(row, 6);
                        var lblSonDurum = GetLabelAt(row, 7);

                        if (txtTeklifAdet != null) txtTeklifAdet.Text = veri.teklifBirimMiktar.ToString("N2");
                        if (txtTeklifBirimFiyat != null) txtTeklifBirimFiyat.Text = veri.teklifBirimFiyat.ToString("N2");
                        if (txtGerceklesenAdet != null) txtGerceklesenAdet.Text = veri.gerceklesenBirimMiktar.ToString("N2");
                        if (txtGerceklesenBirimFiyat != null) txtGerceklesenBirimFiyat.Text = veri.gerceklesenBirimFiyat.ToString("N2");

                        decimal teklifToplam = veri.teklifBirimMiktar * veri.teklifBirimFiyat;
                        decimal gerceklesenMaliyet = veri.gerceklesenBirimMiktar * veri.gerceklesenBirimFiyat;

                        if (txtTeklifToplam != null) txtTeklifToplam.Text = teklifToplam.ToString("N2");
                        if (txtGerceklesenMaliyet != null) txtGerceklesenMaliyet.Text = gerceklesenMaliyet.ToString("N2");
                        if (lblSonDurum != null)
                        {
                            lblSonDurum.Text = (teklifToplam - gerceklesenMaliyet).ToString("N2");
                            lblSonDurum.ForeColor = (teklifToplam - gerceklesenMaliyet) < 0 ? Color.Red : Color.Green;
                        }
                    }
                    AddSpacerRow();
                    UpdateTotalsRow();
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
                tableLayoutPanel2.ResumeLayout(true);
            }
        }

        private void AddHeaderRow()
        {
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));

            string[] headers = {
        "Üretim ve Montaj Kalemleri",
        "Teklif Adet/Ağırlık",
        "Teklif Birim Fiyat",
        "Teklif Toplam",
        "Gerçekleşen Adet/Ağırlık",
        "Gerçekleşen Birim Fiyat",
        "Gerçekleşen Maliyet",
        "Son Durum"
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
                    Padding = new Padding(2),
                    Height = 23,
                    AutoSize = false,
                    BackColor = Color.LightGray
                };
                tableLayoutPanel1.Controls.Add(lbl, i, 0);
            }
        }

        private void AddSpacerRow()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                int spacerRowIndex = -1;
                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    var ctl = tableLayoutPanel1.GetControlFromPosition(0, row);
                    if (ctl is Label lbl && lbl.Text == "")
                    {
                        spacerRowIndex = row;
                        break;
                    }
                }

                if (spacerRowIndex >= 0)
                {
                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                    {
                        var ctl = tableLayoutPanel1.GetControlFromPosition(col, spacerRowIndex);
                        if (ctl != null)
                        {
                            tableLayoutPanel1.Controls.Remove(ctl);
                        }
                    }
                    tableLayoutPanel1.RowStyles.RemoveAt(spacerRowIndex);
                    tableLayoutPanel1.RowCount--;
                }

                tableLayoutPanel1.RowCount++;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    var emptyLabel = new Label
                    {
                        Text = "",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        AutoSize = false,
                        Height = 30,
                        Font = new Font("Segoe UI", 10)
                    };
                    tableLayoutPanel1.Controls.Add(emptyLabel, col, tableLayoutPanel1.RowCount - 1);
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private void AddYeniKalemSatiri(string kalemAdi)
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                int spacerRowIndex = -1;
                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    var ctl = tableLayoutPanel1.GetControlFromPosition(0, row);
                    if (ctl is Label lbl && lbl.Text == "")
                    {
                        spacerRowIndex = row;
                        break;
                    }
                }

                if (spacerRowIndex >= 0)
                {
                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                    {
                        var ctl = tableLayoutPanel1.GetControlFromPosition(col, spacerRowIndex);
                        if (ctl != null)
                        {
                            tableLayoutPanel1.Controls.Remove(ctl);
                        }
                    }
                    tableLayoutPanel1.RowStyles.RemoveAt(spacerRowIndex);
                    tableLayoutPanel1.RowCount--;
                }

                tableLayoutPanel1.RowCount++;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                int newRowIndex = tableLayoutPanel1.RowCount - 1;

                var lblKalemAdi = new Label
                {
                    Text = kalemAdi,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(2),
                    AutoSize = false,
                    Height = 30,
                    Font = new Font("Segoe UI", 10)
                };
                tableLayoutPanel1.Controls.Add(lblKalemAdi, 0, newRowIndex);

                for (int i = 1; i < 8; i++)
                {
                    if (i == 7)
                    {
                        var lblSonDurum = new Label
                        {
                            Dock = DockStyle.Fill,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Margin = new Padding(2),
                            ForeColor = Color.Black,
                            AutoSize = false,
                            Height = 30,
                            Font = new Font("Segoe UI", 10)
                        };
                        tableLayoutPanel1.Controls.Add(lblSonDurum, i, newRowIndex);
                        continue;
                    }

                    var txt = new TextBox
                    {
                        Dock = DockStyle.Fill,
                        TextAlign = HorizontalAlignment.Center,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        Font = new Font("Segoe UI", 10),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    if (i == 1 || i == 2 || i == 4 || i == 5)
                    {
                        txt.TextChanged += HesaplaVeGuncelle;
                        txt.KeyPress += (s, e) =>
                        {
                            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                            {
                                e.Handled = true;
                            }
                            if (e.KeyChar == '.' && (s as TextBox).Text.Contains("."))
                            {
                                e.Handled = true;
                            }
                        };
                    }

                    if (i == 3 || i == 6)
                    {
                        txt.Enabled = false;
                        txt.BackColor = Color.LightGray;
                    }

                    tableLayoutPanel1.Controls.Add(txt, i, newRowIndex);
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private void HesaplaVeGuncelle(object sender, EventArgs e)
        {
            if (sender is TextBox txtChanged)
            {
                var pos = tableLayoutPanel1.GetPositionFromControl(txtChanged);
                int row = pos.Row;

                if (row < 1 || tableLayoutPanel1.GetControlFromPosition(0, row) is Label lbl && lbl.Text == "") return;

                tableLayoutPanel1.SuspendLayout();
                tableLayoutPanel2.SuspendLayout();
                try
                {
                    var txtTeklifAdet = GetTextBoxAt(row, 1);
                    var txtTeklifBirimFiyat = GetTextBoxAt(row, 2);
                    var txtTeklifToplam = GetTextBoxAt(row, 3);
                    var txtGerceklesenAdet = GetTextBoxAt(row, 4);
                    var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 5);
                    var txtGerceklesenMaliyet = GetTextBoxAt(row, 6);
                    var lblSonDurum = GetLabelAt(row, 7);

                    decimal teklifAdet = decimal.TryParse(txtTeklifAdet?.Text, out decimal ta) ? ta : 0;
                    decimal teklifBirimFiyat = decimal.TryParse(txtTeklifBirimFiyat?.Text, out decimal tbf) ? tbf : 0;
                    decimal gerceklesenAdet = decimal.TryParse(txtGerceklesenAdet?.Text, out decimal ga) ? ga : 0;
                    decimal gerceklesenBirimFiyat = decimal.TryParse(txtGerceklesenBirimFiyat?.Text, out decimal gbf) ? gbf : 0;

                    decimal teklifToplam = teklifAdet * teklifBirimFiyat;
                    decimal gerceklesenMaliyet = gerceklesenAdet * gerceklesenBirimFiyat;
                    decimal fark = teklifToplam - gerceklesenMaliyet;

                    if (txtTeklifToplam != null)
                        txtTeklifToplam.Text = teklifToplam.ToString("N2");

                    if (txtGerceklesenMaliyet != null)
                        txtGerceklesenMaliyet.Text = gerceklesenMaliyet.ToString("N2");

                    if (lblSonDurum != null)
                    {
                        lblSonDurum.Text = fark.ToString("N2");
                        lblSonDurum.ForeColor = fark < 0 ? Color.Red : Color.Green;
                    }

                    UpdateTotalsRow();
                }
                finally
                {
                    tableLayoutPanel1.ResumeLayout(true);
                    tableLayoutPanel2.ResumeLayout(true);
                }
            }
        }

        private void UpdateTotalsRow()
        {
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            try
            {
                tableLayoutPanel2.Controls.Clear();
                tableLayoutPanel2.RowStyles.Clear();
                tableLayoutPanel2.RowCount = 1;

                int totalRowIndex = 0;
                tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                var lblToplam = new Label
                {
                    Text = "Toplam",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    AutoSize = false,
                    Height = 30,
                    BackColor = Color.LightYellow
                };
                tableLayoutPanel2.Controls.Add(lblToplam, 0, totalRowIndex);

                decimal toplamTeklif = 0, toplamGerceklesen = 0, toplamSonDurum = 0;

                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    if (tableLayoutPanel1.GetControlFromPosition(0, row) is Label lbl && lbl.Text == "") continue;
                    toplamTeklif += decimal.TryParse(GetTextBoxAt(row, 3)?.Text, out decimal tt) ? tt : 0;
                    toplamGerceklesen += decimal.TryParse(GetTextBoxAt(row, 6)?.Text, out decimal gm) ? gm : 0;
                    toplamSonDurum += decimal.TryParse(GetLabelAt(row, 7)?.Text, out decimal sd) ? sd : 0;
                }

                var lblToplamTeklifHesap = new Label
                {
                    Text = toplamTeklif.ToString("N2"),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    AutoSize = false,
                    Height = 30,
                    BackColor = Color.LightYellow
                };
                tableLayoutPanel2.Controls.Add(lblToplamTeklifHesap, 3, totalRowIndex);

                var lblToplamGerceklesenHesap = new Label
                {
                    Text = toplamGerceklesen.ToString("N2"),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    AutoSize = false,
                    Height = 30,
                    BackColor = Color.LightYellow
                };
                tableLayoutPanel2.Controls.Add(lblToplamGerceklesenHesap, 6, totalRowIndex);

                var lblToplamSonDurumHesap = new Label
                {
                    Text = toplamSonDurum.ToString("N2"),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    AutoSize = false,
                    Height = 30,
                    ForeColor = toplamSonDurum < 0 ? Color.Red : Color.Green,
                    BackColor = Color.LightYellow
                };
                tableLayoutPanel2.Controls.Add(lblToplamSonDurumHesap, 7, totalRowIndex);

                for (int col = 1; col < tableLayoutPanel2.ColumnCount; col++)
                {
                    if (col == 3 || col == 6 || col == 7) continue;
                    var emptyLabel = new Label
                    {
                        Text = "",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        AutoSize = false,
                        Height = 30,
                        Font = new Font("Segoe UI", 10),
                        BackColor = Color.LightYellow
                    };
                    tableLayoutPanel2.Controls.Add(emptyLabel, col, totalRowIndex);
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
                tableLayoutPanel2.ResumeLayout(true);
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

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Aranan proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProjeNo.Text = null;
                return;
            }

            var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
            if (projeKutuk != null && projeKutuk.altProjeVarMi)
            {
                MessageBox.Show($"Proje '{projeNo}' alt projelere sahip. Ana proje için fiyatlandırma yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tableLayoutPanel1.SuspendLayout();
            try
            {
                var mevcutFiyatlandirmalar = fiyatlandirmaData.GetFiyatlandirmaByProje(projeNo)
                    .ToDictionary(f => f.fiyatlandirmaKalemId, f => f);

                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    var lblKalemAdi = GetLabelAt(row, 0);
                    if (lblKalemAdi == null || string.IsNullOrEmpty(lblKalemAdi.Text)) continue;

                    var txtTeklifAdet = GetTextBoxAt(row, 1);
                    var txtTeklifBirimFiyat = GetTextBoxAt(row, 2);
                    var txtGerceklesenAdet = GetTextBoxAt(row, 4);
                    var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 5);

                    decimal teklifAdet = decimal.TryParse(txtTeklifAdet?.Text, out decimal ta) ? ta : 0;
                    decimal teklifBirimFiyat = decimal.TryParse(txtTeklifBirimFiyat?.Text, out decimal tbf) ? tbf : 0;
                    decimal gerceklesenAdet = decimal.TryParse(txtGerceklesenAdet?.Text, out decimal ga) ? ga : 0;
                    decimal gerceklesenBirimFiyat = decimal.TryParse(txtGerceklesenBirimFiyat?.Text, out decimal gbf) ? gbf : 0;

                    int fiyatlandirmaKalemId = iscilikData.GetFiyatlandirmaKalemIdByAdi(lblKalemAdi.Text);
                    if (fiyatlandirmaKalemId == 0)
                    {
                        MessageBox.Show($"Kalem '{lblKalemAdi.Text}' için ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    if (mevcutFiyatlandirmalar.TryGetValue(fiyatlandirmaKalemId, out var mevcutFiyatlandirma))
                    {
                        fiyatlandirmaData.FiyatlandirmaGuncelle(
                            projeNo,
                            fiyatlandirmaKalemId,
                            teklifAdet,
                            teklifBirimFiyat,
                            gerceklesenAdet,
                            gerceklesenBirimFiyat
                        );
                    }
                    else
                    {
                        fiyatlandirmaData.FiyatlandirmaKaydet(
                            projeNo,
                            fiyatlandirmaKalemId,
                            teklifAdet,
                            teklifBirimFiyat,
                            gerceklesenAdet,
                            gerceklesenBirimFiyat
                        );
                    }
                }

                var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);
                OnFiyatlandirmaKaydedildi?.Invoke(projeNo);
                MessageBox.Show("Fiyatlandırma kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private void btnProjeAra_Click(object sender, EventArgs e)
        {
            string arananProjeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(arananProjeNo))
            {
                MessageBox.Show("Lütfen bir proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnYeniKalemEkle.Enabled = false;
                return;
            }

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(arananProjeNo);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Aranan proje '{arananProjeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProjeNo.Text = null;
                btnYeniKalemEkle.Enabled = false;
                return;
            }

            var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(arananProjeNo);
            if (projeKutuk != null && projeKutuk.altProjeVarMi)
            {
                MessageBox.Show($"Proje '{arananProjeNo}' alt projelere sahip. Ana proje için fiyatlandırma yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnYeniKalemEkle.Enabled = false;
                return;
            }

            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowCount = 0;
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
                for (int i = 0; i < 8; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
                }

                tableLayoutPanel1.RowCount = 1;
                AddHeaderRow();

                var fiyatlandirmaVerileri = fiyatlandirmaData.GetFiyatlandirmaByProje(arananProjeNo);
                if (fiyatlandirmaVerileri.Any())
                {
                    foreach (var veri in fiyatlandirmaVerileri)
                    {
                        if (string.IsNullOrEmpty(veri.kalemAdi)) continue;
                        AddYeniKalemSatiri(veri.kalemAdi);
                        int row = tableLayoutPanel1.RowCount - 1;

                        var txtTeklifAdet = GetTextBoxAt(row, 1);
                        var txtTeklifBirimFiyat = GetTextBoxAt(row, 2);
                        var txtTeklifToplam = GetTextBoxAt(row, 3);
                        var txtGerceklesenAdet = GetTextBoxAt(row, 4);
                        var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 5);
                        var txtGerceklesenMaliyet = GetTextBoxAt(row, 6);
                        var lblSonDurum = GetLabelAt(row, 7);

                        if (txtTeklifAdet != null) txtTeklifAdet.Text = veri.teklifBirimMiktar.ToString("N2");
                        if (txtTeklifBirimFiyat != null) txtTeklifBirimFiyat.Text = veri.teklifBirimFiyat.ToString("N2");
                        if (txtGerceklesenAdet != null) txtGerceklesenAdet.Text = veri.gerceklesenBirimMiktar.ToString("N2");
                        if (txtGerceklesenBirimFiyat != null) txtGerceklesenBirimFiyat.Text = veri.gerceklesenBirimFiyat.ToString("N2");

                        decimal teklifToplam = veri.teklifBirimMiktar * veri.teklifBirimFiyat;
                        decimal gerceklesenMaliyet = veri.gerceklesenBirimMiktar * veri.gerceklesenBirimFiyat;

                        if (txtTeklifToplam != null) txtTeklifToplam.Text = teklifToplam.ToString("N2");
                        if (txtGerceklesenMaliyet != null) txtGerceklesenMaliyet.Text = gerceklesenMaliyet.ToString("N2");
                        if (lblSonDurum != null)
                        {
                            lblSonDurum.Text = (teklifToplam - gerceklesenMaliyet).ToString("N2");
                            lblSonDurum.ForeColor = (teklifToplam - gerceklesenMaliyet) < 0 ? Color.Red : Color.Green;
                        }
                    }
                }

                AddSpacerRow();
                UpdateTotalsRow();
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
                tableLayoutPanel2.ResumeLayout(true);
            }

            UpdateKalemEkleButtonState();
        }

        private void btnYeniKalemEkle_Click(object sender, EventArgs e)
        {
            var frm = new frmYeniFiyatlandirmaKalemiEkle();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                AddYeniKalemSatiri(frm.KalemAdi);
                AddSpacerRow();
                UpdateTotalsRow();
            }
        }

        private void UpdateKalemEkleButtonState()
        {
            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                btnYeniKalemEkle.Enabled = false;
                return;
            }

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            btnYeniKalemEkle.Enabled = projeBilgi != null;
        }
    }
}