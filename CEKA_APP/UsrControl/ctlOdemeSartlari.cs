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
            tableLayoutPanel1.ColumnCount = 6;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            for (int i = 0; i < 6; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666f));
            }

            AddHeaderRow();
            AddSpacerRow();
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
        }

        private void AddHeaderRow()
        {
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));

            string[] headers = {
                "Proje Kilometre Taşları",
                "Oran (%)",
                "Tutar",
                "Tarih",
                "Açıklama",
                "Durum"
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
                    AutoSize = false,
                    Height = 23
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
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                int newSpacerRowIndex = tableLayoutPanel1.RowCount - 1;

                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    var emptyLabel = new Label
                    {
                        Text = "",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        AutoSize = false,
                        Height = 23,
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

                // Başlık satırından sonra yeni satır ekle
                int newRowIndex = 1; // Başlık satırı 0. indeks

                // Tüm satırları bir aşağı kaydır
                tableLayoutPanel1.RowCount++;
                for (int row = tableLayoutPanel1.RowCount - 2; row >= newRowIndex; row--)
                {
                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                    {
                        var ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
                        if (ctl != null)
                        {
                            tableLayoutPanel1.SetRow(ctl, row + 1);
                        }
                    }
                }

                // Yeni satır için row style ekle
                tableLayoutPanel1.RowStyles.Insert(newRowIndex, new RowStyle(SizeType.Absolute, 23));

                var lblKilometreTasiAdi = new Label
                {
                    Text = kilometreTasiAdi,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(2),
                    AutoSize = false,
                    Height = 23,
                    Font = new Font("Segoe UI", 8)
                };
                tableLayoutPanel1.Controls.Add(lblKilometreTasiAdi, 0, newRowIndex);

                var lblOran = new Label
                {
                    Text = $"%{cleanOran}",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(2),
                    AutoSize = false,
                    Height = 23,
                    Font = new Font("Segoe UI", 8)
                };
                tableLayoutPanel1.Controls.Add(lblOran, 1, newRowIndex);

                var txtTutar = new TextBox
                {
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(2),
                    AutoSize = false,
                    Height = 23,
                    Font = new Font("Segoe UI", 8),
                    BorderStyle = BorderStyle.FixedSingle,
                    Enabled = false,
                    BackColor = Color.LightGray
                };
                txtTutar.KeyPress += (s, e) =>
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
                tableLayoutPanel1.Controls.Add(txtTutar, 2, newRowIndex);

                var dtpTarih = new DateTimePicker
                {
                    Dock = DockStyle.Fill,
                    Format = DateTimePickerFormat.Short,
                    ShowCheckBox = true,
                    Checked = false,
                    Margin = new Padding(2),
                    Font = new Font("Segoe UI", 8),
                    Height = 23
                };
                tableLayoutPanel1.Controls.Add(dtpTarih, 3, newRowIndex);

                var txtAciklama = new TextBox
                {
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(2),
                    AutoSize = false,
                    Height = 23,
                    Font = new Font("Segoe UI", 8),
                    BorderStyle = BorderStyle.FixedSingle,
                    Multiline = true
                };
                tableLayoutPanel1.Controls.Add(txtAciklama, 4, newRowIndex);

                var lblDurum = new Label
                {
                    Text = "Bekliyor",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(2),
                    ForeColor = Color.Black,
                    AutoSize = false,
                    Height = 23,
                    Font = new Font("Segoe UI", 8)
                };
                tableLayoutPanel1.Controls.Add(lblDurum, 5, newRowIndex);

                if (ParseToplamBedel(out decimal toplamBedel))
                {
                    UpdateTutarColumn(toplamBedel);
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
            for (int row = 0; row < tableLayoutPanel1.RowCount - 2; row++) // Spacer ve başlık satırlarını hariç tut
            {
                var lblOran = GetLabelAt(row, 1);
                if (lblOran != null && decimal.TryParse(lblOran.Text.Replace("%", "").Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal oran))
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

            string cleanText = txtToplamBedel.Text.Split(' ')[0];
            return decimal.TryParse(cleanText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out toplamBedel);
        }

        private void UpdateTutarColumn(decimal toplamBedel)
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                for (int row = 0; row < tableLayoutPanel1.RowCount - 2; row++) // Spacer ve başlık satırlarını hariç tut
                {
                    var lblOran = GetLabelAt(row, 1);
                    var txtTutar = GetTextBoxAt(row, 2);
                    if (lblOran != null && txtTutar != null && decimal.TryParse(lblOran.Text.Replace("%", "").Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal oran))
                    {
                        decimal tutar = toplamBedel * (oran / 100);
                        txtTutar.Text = tutar.ToString("N2");
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
                for (int row = 0; row < tableLayoutPanel1.RowCount - 2; row++) // Spacer ve başlık satırlarını hariç tut
                {
                    var lblKilometreTasiAdi = GetLabelAt(row, 0);
                    var lblOran = GetLabelAt(row, 1);
                    var txtTutar = GetTextBoxAt(row, 2);
                    var dtpTarih = tableLayoutPanel1.GetControlFromPosition(3, row) as DateTimePicker;
                    var txtAciklama = GetTextBoxAt(row, 4);
                    var lblDurum = GetLabelAt(row, 5);

                    if (lblKilometreTasiAdi == null || string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text) ||
                        lblOran == null || string.IsNullOrWhiteSpace(lblOran.Text) ||
                        txtTutar == null || string.IsNullOrWhiteSpace(txtTutar.Text) ||
                        dtpTarih == null || !dtpTarih.Checked ||
                        txtAciklama == null || string.IsNullOrWhiteSpace(txtAciklama.Text) ||
                        lblDurum == null || string.IsNullOrWhiteSpace(lblDurum.Text))
                    {
                        MessageBox.Show($"Satır {row + 1}: Tüm alanlar doldurulmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    if (!decimal.TryParse(oranText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal oran))
                    {
                        MessageBox.Show($"Geçersiz oran formatı: {oranText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string tutarText = txtTutar.Text;
                    if (!decimal.TryParse(tutarText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal tutar))
                    {
                        MessageBox.Show($"Geçersiz tutar formatı: {tutarText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string tarih = dtpTarih.Checked ? dtpTarih.Value.ToString("d", System.Globalization.CultureInfo.InvariantCulture) : "";
                    string aciklama = txtAciklama.Text;
                    string durum = lblDurum.Text;
                    int siralama = row + 1;

                    odemeSekilleriData.OdemeBilgiKaydet(
                        projeNo,
                        kilometreTasiId.ToString(),
                        siralama,
                        oran.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                        Math.Floor(tutar).ToString("F0", System.Globalization.CultureInfo.InvariantCulture),
                        tarih,
                        aciklama,
                        durum
                    );

                    if (proje.altProjeVarMi && proje.altProjeBilgileri != null)
                    {
                        foreach (var altProjeNo in proje.altProjeBilgileri)
                        {
                            var altProje = ProjeKutukData.ProjeKutukAra(altProjeNo);
                            decimal altToplamBedel = 0;
                            if (altProje != null)
                            {
                                var fiyatlandirmaData = new ProjeFiyatlandirmaData();
                                var (bedel, _) = fiyatlandirmaData.GetToplamBedel(altProjeNo, null);
                                altToplamBedel = bedel;
                            }

                            string altTutar = altToplamBedel > 0
                                ? Math.Floor(altToplamBedel * (oran / 100)).ToString("F0", System.Globalization.CultureInfo.InvariantCulture)
                                : tutarText;

                            odemeSekilleriData.OdemeBilgiKaydet(
                                altProjeNo,
                                kilometreTasiId.ToString(),
                                siralama,
                                oran.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                                altTutar,
                                tarih,
                                aciklama,
                                durum
                            );
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