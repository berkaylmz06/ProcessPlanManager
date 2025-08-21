using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeFiyatlandirma : UserControl
    {
        private ProjeFinans_FiyatlandirmaKalemleriData iscilikData = new ProjeFinans_FiyatlandirmaKalemleriData();
        private ProjeFinans_FiyatlandirmaData fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
        public event Action<string> OnFiyatlandirmaKaydedildi;
        private ComboBox cmbProjeNo;
        private List<string> altProjeler;
        private List<(string projeNo, string kalemAdi)> _pendingDeletions = new List<(string projeNo, string kalemAdi)>();
        private bool _isUpdating = false;
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
                    LoadKalemler(cmbProjeNo.SelectedItem.ToString());
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

            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.AutoSize = false;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.MinimumSize = new Size(tableLayoutPanel1.Width, 200);

            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f)); 
            for (int i = 1; i < 10; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 9));
            }

            InitializeTableStructure();
            InitializeTxtDovizKuruEvents();

            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            tableLayoutPanel2.ColumnCount = 10;
            tableLayoutPanel2.ColumnStyles.Clear();
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
            for (int i = 1; i < 10; i++)
            {
                tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 9));
            }
        }
        private void InitializeTableStructure()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.RowCount = 0;

                AddHeaderRow();
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
                LoadKalemler(projeNo);
            }
        }
        private decimal GetKur(string paraBirimi, int fiyatlandirmaKalemId = 0, string projeNo = "")
        {
            if (paraBirimi == "TL")
            {
                lblEskiKur.Text = "";
                return 1m;
            }

            if (fiyatlandirmaKalemId > 0 && !string.IsNullOrEmpty(projeNo))
            {
                var fiyatlandirmaVerileri = fiyatlandirmaData.GetFiyatlandirmaByProje(projeNo);
                var mevcutFiyatlandirma = fiyatlandirmaVerileri?
                    .FirstOrDefault(f => f.fiyatlandirmaKalemId == fiyatlandirmaKalemId && f.teklifKuru.HasValue)
                    ?? fiyatlandirmaVerileri?
                    .LastOrDefault(f => f.teklifKuru.HasValue);

            }

            string kurText = txtDovizKuru.Text?.Trim().Replace(",", ".") ?? "0";
            if (decimal.TryParse(kurText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal kurFromText))
            {
                kurFromText = Math.Round(kurFromText, 4);
                if (kurFromText < 0.1m || kurFromText > 1000m)
                {
                    kurFromText = 1m;
                }
                return kurFromText;
            }

            lblEskiKur.Text = "";
            return 1m;
        }
        private void LoadKalemler(string projeNo)
        {
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnCount = 10;
                tableLayoutPanel1.RowCount = 1;
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
                for (int i = 1; i < 10; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 9));
                }

                AddHeaderRow();
                AddSpacerRow();
                UpdateTotalsRow();

                string paraBirimi = ProjeFinans_ProjeKutukData.GetProjeParaBirimi(projeNo);
                var fiyatlandirmaVerileri = fiyatlandirmaData.GetFiyatlandirmaByProje(projeNo);


                foreach (var veri in fiyatlandirmaVerileri)
                {
                    if (string.IsNullOrEmpty(veri.kalemAdi))
                    {
                        continue;
                    }

                    string kalemBirimi = veri.kalemBirimi;
                    AddYeniKalemSatiri(veri.kalemAdi, veri.kalemBirimi);
                    int row = tableLayoutPanel1.RowCount - 1;

                    var panelTeklifAdet = tableLayoutPanel1.GetControlFromPosition(2, row) as Panel;
                    var txtTeklifBirimFiyat = GetTextBoxAt(row, 3);
                    var txtTeklifToplam = GetTextBoxAt(row, 4);
                    var txtTeklifToplamTL = GetTextBoxAt(row, 5);
                    var panelGerceklesenAdet = tableLayoutPanel1.GetControlFromPosition(6, row) as Panel;
                    var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 7);
                    var txtGerceklesenMaliyet = GetTextBoxAt(row, 8);
                    var lblSonDurum = GetLabelAt(row, 9);

                    var txtTeklifAdet = panelTeklifAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtTeklifAdet");
                    var txtTeklifBirim = panelTeklifAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtTeklifBirim");
                    var txtGerceklesenAdet = panelGerceklesenAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtGerceklesenAdet");
                    var txtGerceklesenBirim = panelGerceklesenAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtGerceklesenBirim");

                    if (txtTeklifAdet != null) txtTeklifAdet.Text = veri.teklifBirimMiktar.ToString("N2", new CultureInfo("tr-TR"));
                    if (txtTeklifBirim != null) txtTeklifBirim.Text = kalemBirimi;
                    if (txtTeklifBirimFiyat != null) txtTeklifBirimFiyat.Text = veri.teklifBirimFiyat.ToString("N2", new CultureInfo("tr-TR"));
                    if (txtGerceklesenAdet != null) txtGerceklesenAdet.Text = veri.gerceklesenBirimMiktar.ToString("N2", new CultureInfo("tr-TR"));
                    if (txtGerceklesenBirim != null) txtGerceklesenBirim.Text = kalemBirimi;
                    if (txtGerceklesenBirimFiyat != null) txtGerceklesenBirimFiyat.Text = veri.gerceklesenBirimFiyat.ToString("N2", new CultureInfo("tr-TR"));

                    decimal kur = GetKur(paraBirimi, veri.fiyatlandirmaKalemId, projeNo);
                    decimal teklifToplam;
                    try
                    {
                        checked
                        {
                            teklifToplam = veri.teklifBirimMiktar * veri.teklifBirimFiyat;
                        }
                    }
                    catch (OverflowException)
                    {
                        teklifToplam = 0m;
                    }

                    decimal teklifToplamTL;
                    try
                    {
                        checked
                        {
                            teklifToplamTL = teklifToplam * kur;
                        }
                    }
                    catch (OverflowException)
                    {
                        teklifToplamTL = 0m;
                    }

                    decimal gerceklesenMaliyet;
                    try
                    {
                        checked
                        {
                            gerceklesenMaliyet = veri.gerceklesenBirimMiktar * veri.gerceklesenBirimFiyat;
                        }
                    }
                    catch (OverflowException)
                    {
                        gerceklesenMaliyet = 0m;
                    }

                    decimal fark = teklifToplamTL - gerceklesenMaliyet;

                    if (txtTeklifToplam != null) txtTeklifToplam.Text = $"{teklifToplam.ToString("N2", new CultureInfo("tr-TR"))} {paraBirimi}";
                    if (txtTeklifToplamTL != null) txtTeklifToplamTL.Text = $"{teklifToplamTL.ToString("N2", new CultureInfo("tr-TR"))} TL";
                    if (txtGerceklesenMaliyet != null) txtGerceklesenMaliyet.Text = $"{gerceklesenMaliyet.ToString("N2", new CultureInfo("tr-TR"))} TL";
                    if (lblSonDurum != null)
                    {
                        lblSonDurum.Text = $"{fark.ToString("N2", new CultureInfo("tr-TR"))} TL";
                        lblSonDurum.ForeColor = fark < 0 ? Color.Red : Color.Green;
                    }
                    AddSpacerRow();
                    UpdateTotalsRow();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        "Sil",
        "Üretim ve Montaj Kalemleri",
        "Teklif Birim",
        "Teklif Birim Fiyat",
        "Teklif Toplam",
        "Teklif Toplam (TL)",
        "Gerçekleşen Birim",
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
                tableLayoutPanel1.SetColumnSpan(lbl, 1); 
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

        private void AddYeniKalemSatiri(string kalemAdi, string kalemBirimi)
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

                string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
                string paraBirimi = ProjeFinans_ProjeKutukData.GetProjeParaBirimi(projeNo);

                var pictureBoxSil = new PictureBox
                {
                    Image = Properties.Resources.copKutusu,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(26, 26),
                    Dock = DockStyle.Fill,
                    Margin = new Padding(2),
                    Cursor = Cursors.Hand
                };
                pictureBoxSil.Click += (s, e) => SilSatir(newRowIndex);
                tableLayoutPanel1.Controls.Add(pictureBoxSil, 0, newRowIndex);

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
                tableLayoutPanel1.Controls.Add(lblKalemAdi, 1, newRowIndex);

                for (int i = 2; i < 10; i++)
                {
                    if (i == 9)
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

                    if (i == 2 || i == 6)
                    {
                        var panel = new Panel
                        {
                            Dock = DockStyle.Fill,
                            Margin = new Padding(2),
                            Padding = new Padding(0),
                            AutoSize = false,
                            Height = 26
                        };

                        var txtValue = new TextBox
                        {
                            Name = i == 2 ? "txtTeklifAdet" : "txtGerceklesenAdet",
                            Dock = DockStyle.Left,
                            TextAlign = HorizontalAlignment.Center,
                            Margin = new Padding(0),
                            AutoSize = false,
                            Width = 120,
                            Height = 26,
                            Font = new Font("Segoe UI", 10),
                            BorderStyle = BorderStyle.FixedSingle,
                            Text = "0,00",
                            MaxLength = 20 
                        };

                        var txtUnit = new TextBox
                        {
                            Name = i == 2 ? "txtTeklifBirim" : "txtGerceklesenBirim",
                            Dock = DockStyle.Fill,
                            TextAlign = HorizontalAlignment.Center,
                            Margin = new Padding(0),
                            AutoSize = false,
                            Width = 40,
                            Height = 26,
                            Font = new Font("Segoe UI", 10),
                            BorderStyle = BorderStyle.FixedSingle,
                            Text = kalemBirimi,
                            Enabled = false,
                            BackColor = Color.LightGray
                        };

                        panel.Controls.Add(txtUnit);
                        panel.Controls.Add(txtValue);

                        txtValue.TextChanged += HesaplaVeGuncelle;
                        txtValue.Enter += (s, e) =>
                        {
                            var textBox = s as TextBox;
                            textBox.Select(0, textBox.Text.Length);
                        };
                        txtValue.Leave += (s, e) =>
                        {
                            var textBox = s as TextBox;
                            if (textBox != null)
                            {
                                string text = textBox.Text.Trim();
                                if (decimal.TryParse(text, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal value))
                                {
                                    value = Math.Round(value, 2);
                                    textBox.Text = value.ToString("N2", new CultureInfo("tr-TR"));
                                }
                                else
                                {
                                    textBox.Text = "0,00";
                                }
                            }
                        };
                        txtValue.KeyPress += (s, e) =>
                        {
                            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
                            {
                                e.Handled = true;
                            }

                            var textBox = s as TextBox;
                            string text = textBox.Text;
                            if (e.KeyChar == ',' && text.Contains(","))
                            {
                                e.Handled = true;
                            }

                            string currentText = text.Replace(",", "");
                            if (currentText.Length >= 15 && !char.IsControl(e.KeyChar))
                            {
                                e.Handled = true;
                            }
                        };

                        tableLayoutPanel1.Controls.Add(panel, i, newRowIndex);
                    }
                    else
                    {
                        var txt = new TextBox
                        {
                            Dock = DockStyle.Fill,
                            TextAlign = HorizontalAlignment.Center,
                            Margin = new Padding(2),
                            AutoSize = false,
                            Height = 26,
                            Font = new Font("Segoe UI", 10),
                            BorderStyle = BorderStyle.FixedSingle,
                            Enabled = true,
                            Visible = true,
                            MinimumSize = new Size(50, 26),
                            Text = "0,00",
                            MaxLength = 20
                        };

                        if (i == 3 || i == 7)
                        {
                            txt.TextChanged += HesaplaVeGuncelle;
                            txt.Enter += (s, e) =>
                            {
                                var textBox = s as TextBox;
                                textBox.Select(0, textBox.Text.Length);
                            };
                            txt.Leave += (s, e) =>
                            {
                                var textBox = s as TextBox;
                                if (textBox != null)
                                {
                                    string text = textBox.Text.Trim();
                                    if (decimal.TryParse(text, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal value))
                                    {
                                        value = Math.Round(value, 2);
                                        textBox.Text = value.ToString("N2", new CultureInfo("tr-TR"));
                                    }
                                    else
                                    {
                                        textBox.Text = "0,00";
                                    }
                                }
                            };
                            txt.KeyPress += (s, e) =>
                            {
                                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
                                {
                                    e.Handled = true;
                                }

                                var textBox = s as TextBox;
                                string text = textBox.Text;
                                if (e.KeyChar == ',' && text.Contains(","))
                                {
                                    e.Handled = true;
                                }

                                string currentText = text.Replace(",", "");
                                if (currentText.Length >= 15 && !char.IsControl(e.KeyChar))
                                {
                                    e.Handled = true;
                                }
                            };
                        }

                        if (i == 4 || i == 5 || i == 8)
                        {
                            txt.Enabled = false;
                            txt.BackColor = Color.LightGray;
                        }

                        tableLayoutPanel1.Controls.Add(txt, i, newRowIndex);
                    }
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }
        private void SilSatir(int rowIndex)
        {
            if (rowIndex <= 0 || rowIndex >= tableLayoutPanel1.RowCount - 1)
            {
                return;
            }

            var lblKalemAdi = tableLayoutPanel1.GetControlFromPosition(1, rowIndex) as Label;
            string kalemAdi = lblKalemAdi?.Text ?? "Bilinmeyen Kalem";
            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();

            var result = MessageBox.Show(
                $"Kalem '{kalemAdi}' silinecek. Devam etmek istiyor musunuz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            tableLayoutPanel1.SuspendLayout();
            try
            {
                _pendingDeletions.Add((projeNo, kalemAdi));
                MessageBox.Show("Satır başarıyla silinmek üzere işaretlendi. Kaydet butonuna tıklayarak veritabanından silmeyi onaylayın.", "Silme Onayı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    var control = tableLayoutPanel1.GetControlFromPosition(col, rowIndex);
                    if (control != null)
                    {
                        tableLayoutPanel1.Controls.Remove(control);
                    }
                }
                tableLayoutPanel1.RowStyles.RemoveAt(rowIndex);
                tableLayoutPanel1.RowCount--;

                for (int row = rowIndex; row < tableLayoutPanel1.RowCount; row++)
                {
                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                    {
                        var control = tableLayoutPanel1.GetControlFromPosition(col, row + 1);
                        if (control != null)
                        {
                            tableLayoutPanel1.SetRow(control, row);
                        }
                    }
                }

                if (tableLayoutPanel1.RowCount == 1)
                {
                    tableLayoutPanel1.RowCount++;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));
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
                        tableLayoutPanel1.Controls.Add(emptyLabel, col, tableLayoutPanel1.RowCount - 1);
                    }
                }

                HesaplaVeGuncelle(null, null);
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }
        private int GetRowFromControl(Control control)
        {
            if (control == null) return -1;

            var pos = tableLayoutPanel1.GetPositionFromControl(control);
            if (pos.Row >= 0) return pos.Row;

            // Üst kontrolü kontrol et
            return GetRowFromControl(control.Parent);
        }
        private void HesaplaVeGuncelle(object sender, EventArgs e)
        {
            if (_isUpdating) return;

            int row = -1;

            if (sender is TextBox txtChanged)
            {
                row = GetRowFromControl(txtChanged);
            }

            if (row < 1 || tableLayoutPanel1.GetControlFromPosition(1, row) is Label lbl && lbl.Text == "")
            {
                UpdateTotalsRow();
                return;
            }

            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
            string paraBirimi = ProjeFinans_ProjeKutukData.GetProjeParaBirimi(projeNo);

            string kalemAdi = GetLabelAt(row, 1)?.Text;
            int fiyatlandirmaKalemId = 0;
            decimal kur = 0;

            if (!string.IsNullOrEmpty(kalemAdi))
            {
                var kalemDetay = iscilikData.GetFiyatlandirmaKalemByAdi(kalemAdi);
                if (kalemDetay != null)
                {
                    fiyatlandirmaKalemId = kalemDetay.FiyatlandirmaKalemId;
                    kur = GetKur(paraBirimi, fiyatlandirmaKalemId, projeNo);
                }
                else
                {
                    UpdateTotalsRow();
                    return;
                }
            }

            _isUpdating = true;
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            try
            {
                var panelTeklifAdet = tableLayoutPanel1.GetControlFromPosition(2, row) as Panel;
                var panelGerceklesenAdet = tableLayoutPanel1.GetControlFromPosition(6, row) as Panel;
                var txtTeklifBirimFiyat = GetTextBoxAt(row, 3);
                var txtTeklifToplam = GetTextBoxAt(row, 4);
                var txtTeklifToplamTL = GetTextBoxAt(row, 5);
                var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 7);
                var txtGerceklesenMaliyet = GetTextBoxAt(row, 8);
                var lblSonDurum = GetLabelAt(row, 9);

                var txtTeklifAdet = panelTeklifAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtTeklifAdet");
                var txtGerceklesenAdet = panelGerceklesenAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtGerceklesenAdet");

                if (txtTeklifAdet == null || txtGerceklesenAdet == null || txtTeklifBirimFiyat == null || txtGerceklesenBirimFiyat == null)
                {
                    return;
                }

                string teklifAdetText = txtTeklifAdet.Text?.Trim() ?? "0";
                string teklifBirimFiyatText = txtTeklifBirimFiyat.Text?.Trim() ?? "0";
                string gerceklesenAdetText = txtGerceklesenAdet.Text?.Trim() ?? "0";
                string gerceklesenBirimFiyatText = txtGerceklesenBirimFiyat.Text?.Trim() ?? "0";

                decimal teklifAdet = decimal.TryParse(teklifAdetText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal ta) ? ta : 0m;
                decimal teklifBirimFiyat = decimal.TryParse(teklifBirimFiyatText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal tbf) ? tbf : 0m;
                decimal gerceklesenAdet = decimal.TryParse(gerceklesenAdetText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal ga) ? ga : 0m;
                decimal gerceklesenBirimFiyat = decimal.TryParse(gerceklesenBirimFiyatText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal gbf) ? gbf : 0m;

                teklifAdet = Math.Round(teklifAdet, 2);
                teklifBirimFiyat = Math.Round(teklifBirimFiyat, 2);
                gerceklesenAdet = Math.Round(gerceklesenAdet, 2);
                gerceklesenBirimFiyat = Math.Round(gerceklesenBirimFiyat, 2);


                decimal teklifToplam;
                try
                {
                    checked
                    {
                        teklifToplam = teklifAdet * teklifBirimFiyat;
                    }
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Girilen değerler çok büyük, lütfen daha küçük değerler kullanın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    teklifToplam = 0m;
                    txtTeklifAdet.Text = "0,00";
                    txtTeklifBirimFiyat.Text = "0,00";
                }

                decimal teklifToplamTL;
                try
                {
                    checked
                    {
                        teklifToplamTL = teklifToplam * kur;
                    }
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Girilen değerler çok büyük, lütfen daha küçük değerler kullanın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    teklifToplamTL = 0m;
                    txtTeklifAdet.Text = "0,00";
                    txtTeklifBirimFiyat.Text = "0,00";
                }

                decimal gerceklesenMaliyet;
                try
                {
                    checked
                    {
                        gerceklesenMaliyet = gerceklesenAdet * gerceklesenBirimFiyat;
                    }
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Girilen değerler çok büyük, lütfen daha küçük değerler kullanın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gerceklesenMaliyet = 0m;
                    txtGerceklesenAdet.Text = "0,00";
                    txtGerceklesenBirimFiyat.Text = "0,00";
                }

                decimal fark = teklifToplamTL - gerceklesenMaliyet;

                if (txtTeklifToplam != null)
                    txtTeklifToplam.Text = $"{teklifToplam.ToString("N2", new CultureInfo("tr-TR"))} {paraBirimi}";
                if (txtTeklifToplamTL != null)
                    txtTeklifToplamTL.Text = $"{teklifToplamTL.ToString("N2", new CultureInfo("tr-TR"))} TL";
                if (txtGerceklesenMaliyet != null)
                    txtGerceklesenMaliyet.Text = $"{gerceklesenMaliyet.ToString("N2", new CultureInfo("tr-TR"))} TL";
                if (lblSonDurum != null)
                {
                    lblSonDurum.Text = $"{fark.ToString("N2", new CultureInfo("tr-TR"))} TL";
                    lblSonDurum.ForeColor = fark < 0 ? Color.Red : Color.Green;
                }

                UpdateTotalsRow();
            }
            finally
            {
                _isUpdating = false;
                tableLayoutPanel1.ResumeLayout(true);
                tableLayoutPanel2.ResumeLayout(true);
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

                string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
                string paraBirimi = ProjeFinans_ProjeKutukData.GetProjeParaBirimi(projeNo);
                decimal kur = GetKur(paraBirimi);

                var fiyatlandirmaVerileri = fiyatlandirmaData.GetFiyatlandirmaByProje(projeNo);
                var mevcutFiyatlandirma = fiyatlandirmaVerileri?.LastOrDefault(f => f.teklifKuru.HasValue);
                decimal? kaydedilenKur = mevcutFiyatlandirma?.teklifKuru;

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
                tableLayoutPanel2.Controls.Add(lblToplam, 1, totalRowIndex);

                decimal toplamTeklif = 0, toplamTeklifTL = 0, toplamGerceklesen = 0, toplamSonDurum = 0;

                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    if (tableLayoutPanel1.GetControlFromPosition(1, row) is Label lbl && lbl.Text == "") continue;

                    decimal teklifToplam = decimal.TryParse(GetTextBoxAt(row, 4)?.Text.Replace(paraBirimi, "").Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal tt) ? tt : 0m;
                    decimal teklifToplamTL = decimal.TryParse(GetTextBoxAt(row, 5)?.Text.Replace(" TL", "").Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal ttl) ? ttl : 0m;
                    decimal gerceklesenMaliyet = decimal.TryParse(GetTextBoxAt(row, 8)?.Text.Replace(" TL", "").Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal gm) ? gm : 0m;

                    toplamTeklif += teklifToplam;
                    toplamTeklifTL += teklifToplamTL;
                    toplamGerceklesen += gerceklesenMaliyet;
                    toplamSonDurum += teklifToplamTL - gerceklesenMaliyet;
                }

                var lblToplamTeklifHesap = new Label
                {
                    Text = $"{toplamTeklif.ToString("N2", new CultureInfo("tr-TR"))} {paraBirimi}",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    AutoSize = false,
                    Height = 30,
                    BackColor = Color.LightYellow
                };
                tableLayoutPanel2.Controls.Add(lblToplamTeklifHesap, 4, totalRowIndex);

                var lblToplamTeklifTLHesap = new Label
                {
                    Text = $"{toplamTeklifTL.ToString("N2", new CultureInfo("tr-TR"))} TL",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    AutoSize = false,
                    Height = 30,
                    BackColor = Color.LightYellow
                };
                tableLayoutPanel2.Controls.Add(lblToplamTeklifTLHesap, 5, totalRowIndex);

                var lblToplamGerceklesenHesap = new Label
                {
                    Text = $"{toplamGerceklesen.ToString("N2", new CultureInfo("tr-TR"))} TL",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    AutoSize = false,
                    Height = 30,
                    BackColor = Color.LightYellow
                };
                tableLayoutPanel2.Controls.Add(lblToplamGerceklesenHesap, 8, totalRowIndex);

                var lblToplamSonDurumHesap = new Label
                {
                    Text = $"{toplamSonDurum.ToString("N2", new CultureInfo("tr-TR"))} TL",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    AutoSize = false,
                    Height = 30,
                    ForeColor = toplamSonDurum < 0 ? Color.Red : Color.Green,
                    BackColor = Color.LightYellow
                };
                tableLayoutPanel2.Controls.Add(lblToplamSonDurumHesap, 9, totalRowIndex);

                for (int col = 0; col < tableLayoutPanel2.ColumnCount; col++)
                {
                    if (col == 1 || col == 4 || col == 5 || col == 8 || col == 9) continue;
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
                bool isAnyUpdateOrSave = false;

                foreach (var deletion in _pendingDeletions)
                {
                    var kalemDetay = iscilikData.GetFiyatlandirmaKalemByAdi(deletion.kalemAdi);
                    if (kalemDetay != null)
                    {
                        if (fiyatlandirmaData.FiyatlandirmaSilById(deletion.projeNo, kalemDetay.FiyatlandirmaKalemId))
                        {
                            isAnyUpdateOrSave = true;
                        }
                    }
                }
                _pendingDeletions.Clear();

                var mevcutFiyatlandirmalar = fiyatlandirmaData.GetFiyatlandirmaByProje(projeNo)
                    .ToDictionary(f => f.kalemAdi, f => f, StringComparer.OrdinalIgnoreCase);

                string paraBirimi = ProjeFinans_ProjeKutukData.GetProjeParaBirimi(projeNo);
                decimal kurToUse;

                string eskiKurText = lblEskiKur.Text.Replace($"Kaydedilen Kur ({paraBirimi}): ", "").Replace(" TL", "").Trim();
                if (!string.IsNullOrEmpty(eskiKurText) && decimal.TryParse(eskiKurText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal eskiKur))
                {
                    kurToUse = Math.Round(eskiKur, 4);
                }
                else
                {
                    string dovizKuruText = txtDovizKuru.Text?.Trim() ?? "0";
                    if (!decimal.TryParse(dovizKuruText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal dovizKuru) || dovizKuru <= 0)
                    {
                        MessageBox.Show("Geçerli bir döviz kuru giriniz veya kaydedilen kur mevcut değilse kayıt yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    kurToUse = Math.Round(dovizKuru, 4);
                }

                if (kurToUse < 0.1m || kurToUse > 1000m)
                {
                    MessageBox.Show($"Kur değeri ({kurToUse:N4}) geçerli bir aralıkta değil (0.1 - 1000).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    var lblKalemAdi = GetLabelAt(row, 1);
                    if (lblKalemAdi == null || string.IsNullOrEmpty(lblKalemAdi.Text))
                    {
                        continue;
                    }

                    var kalemDetay = iscilikData.GetFiyatlandirmaKalemByAdi(lblKalemAdi.Text);
                    if (kalemDetay == null)
                    {
                        MessageBox.Show($"Kalem '{lblKalemAdi.Text}' için ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    int fiyatlandirmaKalemId = kalemDetay.FiyatlandirmaKalemId;
                    var panelTeklifAdet = tableLayoutPanel1.GetControlFromPosition(2, row) as Panel;
                    var txtTeklifBirimFiyat = GetTextBoxAt(row, 3);
                    var panelGerceklesenAdet = tableLayoutPanel1.GetControlFromPosition(6, row) as Panel;
                    var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 7);

                    var txtTeklifAdet = panelTeklifAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtTeklifAdet");
                    var txtGerceklesenAdet = panelGerceklesenAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtGerceklesenAdet");

                    if (txtTeklifAdet == null || txtGerceklesenAdet == null || txtTeklifBirimFiyat == null || txtGerceklesenBirimFiyat == null)
                    {
                        continue;
                    }

                    string teklifAdetText = txtTeklifAdet.Text?.Trim() ?? "0";
                    string teklifBirimFiyatText = txtTeklifBirimFiyat.Text?.Trim() ?? "0";
                    string gerceklesenAdetText = txtGerceklesenAdet.Text?.Trim() ?? "0";
                    string gerceklesenBirimFiyatText = txtGerceklesenBirimFiyat.Text?.Trim() ?? "0";

                    decimal teklifAdet = decimal.TryParse(teklifAdetText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal ta) ? ta : 0m;
                    decimal teklifBirimFiyat = decimal.TryParse(teklifBirimFiyatText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal tbf) ? tbf : 0m;
                    decimal gerceklesenAdet = decimal.TryParse(gerceklesenAdetText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal ga) ? ga : 0m;
                    decimal gerceklesenBirimFiyat = decimal.TryParse(gerceklesenBirimFiyatText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal gbf) ? gbf : 0m;

                    teklifAdet = Math.Round(teklifAdet, 2);
                    teklifBirimFiyat = Math.Round(teklifBirimFiyat, 2);
                    gerceklesenAdet = Math.Round(gerceklesenAdet, 2);
                    gerceklesenBirimFiyat = Math.Round(gerceklesenBirimFiyat, 2);


                    if (mevcutFiyatlandirmalar.TryGetValue(lblKalemAdi.Text, out var mevcutFiyatlandirma))
                    {
                        bool hasChanges = Math.Abs(mevcutFiyatlandirma.teklifBirimMiktar - teklifAdet) > 0.01m ||
                                          Math.Abs(mevcutFiyatlandirma.teklifBirimFiyat - teklifBirimFiyat) > 0.01m ||
                                          Math.Abs(mevcutFiyatlandirma.gerceklesenBirimMiktar - gerceklesenAdet) > 0.01m ||
                                          Math.Abs(mevcutFiyatlandirma.gerceklesenBirimFiyat - gerceklesenBirimFiyat) > 0.01m ||
                                          Math.Abs(mevcutFiyatlandirma.teklifKuru.GetValueOrDefault() - kurToUse) > 0.0001m;

                        if (hasChanges)
                        {
                            fiyatlandirmaData.FiyatlandirmaGuncelle(
                                projeNo,
                                fiyatlandirmaKalemId,
                                teklifAdet,
                                teklifBirimFiyat,
                                gerceklesenAdet,
                                gerceklesenBirimFiyat,
                                paraBirimi,
                                kurToUse
                            );
                            isAnyUpdateOrSave = true;
                        }
                    }
                    else
                    {
                        fiyatlandirmaData.FiyatlandirmaKaydet(
                            projeNo,
                            fiyatlandirmaKalemId,
                            teklifAdet,
                            teklifBirimFiyat,
                            gerceklesenAdet,
                            gerceklesenBirimFiyat,
                            paraBirimi,
                            kurToUse
                        );
                        isAnyUpdateOrSave = true;
                    }

                    txtTeklifAdet.Text = teklifAdet.ToString("N2", new CultureInfo("tr-TR"));
                    txtTeklifBirimFiyat.Text = teklifBirimFiyat.ToString("N2", new CultureInfo("tr-TR"));
                    txtGerceklesenAdet.Text = gerceklesenAdet.ToString("N2", new CultureInfo("tr-TR"));
                    txtGerceklesenBirimFiyat.Text = gerceklesenBirimFiyat.ToString("N2", new CultureInfo("tr-TR"));
                }

                if (isAnyUpdateOrSave)
                {
                    var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);
                    OnFiyatlandirmaKaydedildi?.Invoke(projeNo);
                    lblEskiKur.Text = $"Kaydedilen Kur ({paraBirimi}): {kurToUse.ToString("N4", new CultureInfo("tr-TR"))}";
                    MessageBox.Show("Fiyatlandırma başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Kaydedilecek veya güncellenecek bir veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fiyatlandırma kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }
        public void btnProjeAra_Click(object sender, EventArgs e)
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
                _pendingDeletions.Clear();

                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowCount = 0;
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
                for (int i = 1; i < 10; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 9));
                }

                tableLayoutPanel1.RowCount = 1;
                AddHeaderRow();

                string paraBirimi = ProjeFinans_ProjeKutukData.GetProjeParaBirimi(arananProjeNo);
                var fiyatlandirmaVerileri = fiyatlandirmaData.GetFiyatlandirmaByProje(arananProjeNo);
                if (fiyatlandirmaVerileri.Any())
                {
                    foreach (var veri in fiyatlandirmaVerileri)
                    {
                        if (string.IsNullOrEmpty(veri.kalemAdi)) continue;
                        string kalemBirimi = veri.kalemBirimi;
                        AddYeniKalemSatiri(veri.kalemAdi, veri.kalemBirimi);
                        int row = tableLayoutPanel1.RowCount - 1;

                        var panelTeklifAdet = tableLayoutPanel1.GetControlFromPosition(2, row) as Panel;
                        var txtTeklifBirimFiyat = GetTextBoxAt(row, 3);
                        var txtTeklifToplam = GetTextBoxAt(row, 4);
                        var txtTeklifToplamTL = GetTextBoxAt(row, 5);
                        var panelGerceklesenAdet = tableLayoutPanel1.GetControlFromPosition(6, row) as Panel;
                        var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 7);
                        var txtGerceklesenMaliyet = GetTextBoxAt(row, 8);
                        var lblSonDurum = GetLabelAt(row, 9);

                        var txtTeklifAdet = panelTeklifAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtTeklifAdet");
                        var txtTeklifBirim = panelTeklifAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtTeklifBirim");
                        var txtGerceklesenAdet = panelGerceklesenAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtGerceklesenAdet");
                        var txtGerceklesenBirim = panelGerceklesenAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtGerceklesenBirim");

                        if (txtTeklifAdet != null) txtTeklifAdet.Text = $"{veri.teklifBirimMiktar:N2}";
                        if (txtTeklifBirim != null) txtTeklifBirim.Text = kalemBirimi;
                        if (txtTeklifBirimFiyat != null) txtTeklifBirimFiyat.Text = veri.teklifBirimFiyat.ToString("N2");
                        if (txtGerceklesenAdet != null) txtGerceklesenAdet.Text = $"{veri.gerceklesenBirimMiktar:N2}";
                        if (txtGerceklesenBirim != null) txtGerceklesenBirim.Text = kalemBirimi;
                        if (txtGerceklesenBirimFiyat != null) txtGerceklesenBirimFiyat.Text = veri.gerceklesenBirimFiyat.ToString("N2");

                        decimal kur = GetKur(paraBirimi, veri.fiyatlandirmaKalemId, arananProjeNo);
                        decimal teklifToplam = veri.teklifBirimMiktar * veri.teklifBirimFiyat;
                        decimal teklifToplamTL = teklifToplam * kur;
                        decimal gerceklesenMaliyet = veri.gerceklesenBirimMiktar * veri.gerceklesenBirimFiyat;
                        decimal fark = teklifToplamTL - gerceklesenMaliyet;

                        if (txtTeklifToplam != null) txtTeklifToplam.Text = $"{teklifToplam:N2} {paraBirimi}";
                        if (txtTeklifToplamTL != null) txtTeklifToplamTL.Text = $"{teklifToplamTL:N2} TL";
                        if (txtGerceklesenMaliyet != null) txtGerceklesenMaliyet.Text = $"{gerceklesenMaliyet:N2} TL";
                        if (lblSonDurum != null)
                        {
                            lblSonDurum.Text = $"{fark:N2} TL";
                            lblSonDurum.ForeColor = fark < 0 ? Color.Red : Color.Green;
                        }
                    }
                }

                AddSpacerRow();
                KurlariGetir();
                UpdateTotalsRow();
                lblKurBaslik.Visible = true;
                lblKurBilgi.Visible = true;
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
                if (string.IsNullOrWhiteSpace(frm.KalemAdi))
                {
                    MessageBox.Show("Kalem adı boş olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                AddYeniKalemSatiri(frm.KalemAdi, frm.KalemBirimi);
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

        private void btnSil_Click(object sender, EventArgs e)
        {
            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Lütfen bir proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var odemeSartlariData = new ProjeFinans_OdemeSartlariData();
            var projeIliskiData = new ProjeFinans_ProjeIliskiData();
            bool hasOdemeSartlari = false;
            List<string> ilgiliProjeler = new List<string> { projeNo };

            var altProjeler = ProjeFinans_ProjeIliskiData.GetAltProjeler(projeNo);
            if (altProjeler != null && altProjeler.Any())
            {
                ilgiliProjeler.AddRange(altProjeler);
            }

            var ustProjeNo = ProjeFinans_ProjeIliskiData.GetUstProjeNo(projeNo);
            if (!string.IsNullOrEmpty(ustProjeNo))
            {
                ilgiliProjeler.Add(ustProjeNo);
            }

            foreach (var proje in ilgiliProjeler)
            {
                var odemeBilgileri = odemeSartlariData.GetOdemeBilgileriByProjeNo(proje);
                if (odemeBilgileri.Any())
                {
                    hasOdemeSartlari = true;
                    break;
                }
            }

            if (hasOdemeSartlari)
            {
                MessageBox.Show($"Proje '{projeNo}' veya ilişkili üst/alt projeleri için Ödeme Şartlari'nda kaydı var, silinemedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Proje '{projeNo}' için tüm fiyatlandırma kayıtları silinecek. Onaylıyor musunuz?", "Fiyatlandırma Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
            if (fiyatlandirmaData.FiyatlandirmaSil(projeNo))
            {
                MessageBox.Show("Fiyatlandırma kayıtları başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                InitializeTableStructure();
                UpdateTotalsRow();
                UpdateKalemEkleButtonState();
                OnFiyatlandirmaKaydedildi?.Invoke(projeNo);
            }
            else
            {
                MessageBox.Show("Fiyatlandırma kayıtları silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProjeNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.btnProjeAra.PerformClick();
            }
        }
        private void KurlariGetir()
        {
            try
            {
                string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text?.Trim();
                if (string.IsNullOrEmpty(projeNo))
                {
                    txtDovizKuru.Text = "";
                    lblKurBilgi.Text = "Proje numarası girilmemiş.";
                    lblEskiKur.Text = "";
                    txtDovizKuru.Visible = true;
                    return;
                }

                string paraBirimi = ProjeFinans_ProjeKutukData.GetProjeParaBirimi(projeNo);
                if (string.IsNullOrEmpty(paraBirimi))
                {
                    txtDovizKuru.Text = "";
                    lblKurBilgi.Text = "Proje için para birimi bilgisi bulunamadı.";
                    lblEskiKur.Text = "";
                    txtDovizKuru.Visible = true;
                    return;
                }

                if (paraBirimi == "TL")
                {
                    txtDovizKuru.Text = "";
                    lblKurBilgi.Text = "";
                    lblEskiKur.Text = "";
                    txtDovizKuru.Visible = false;
                    return;
                }

                string tcmbUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";
                XDocument xmlDoc = XDocument.Load(tcmbUrl);

                string kurText = "";
                if (paraBirimi == "USD")
                {
                    var usdKur = xmlDoc.Descendants("Currency")
                                       .FirstOrDefault(c => c.Attribute("Kod")?.Value == "USD");
                    kurText = usdKur?.Element("BanknoteSelling")?.Value;
                    lblKurBilgi.Text = "ABD Doları (USD) Satış Kuru";
                }
                else if (paraBirimi == "EUR")
                {
                    var eurKur = xmlDoc.Descendants("Currency")
                                       .FirstOrDefault(c => c.Attribute("Kod")?.Value == "EUR");
                    kurText = eurKur?.Element("BanknoteSelling")?.Value;
                    lblKurBilgi.Text = "Euro (EUR) Satış Kuru";
                }

                if (!string.IsNullOrEmpty(kurText) && decimal.TryParse(kurText.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal kur))
                {
                    kur = Math.Round(kur, 4);
                    txtDovizKuru.Text = kur.ToString("N4", new CultureInfo("tr-TR")); 
                }
                else
                {
                    txtDovizKuru.Text = "";
                    lblKurBilgi.Text = "Kur bilgisi alınamadı.";
                }
                txtDovizKuru.Visible = true;

                var fiyatlandirmaVerileri = fiyatlandirmaData.GetFiyatlandirmaByProje(projeNo);
                decimal? kaydedilenKur = fiyatlandirmaVerileri?
                    .Where(f => f.teklifKuru.HasValue)
                    .OrderByDescending(f => f.fiyatlandirmaKalemId)
                    .Select(f => f.teklifKuru)
                    .FirstOrDefault();

                if (kaydedilenKur.HasValue)
                {
                    lblEskiKur.Text = $"Kaydedilen Kur ({paraBirimi}): {kaydedilenKur.Value.ToString("N4", new CultureInfo("tr-TR"))}";
                }
                else
                {
                    lblEskiKur.Text = "Kayıtlı kur bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                txtDovizKuru.Text = "";
                lblKurBilgi.Text = $"Hata oluştu: {ex.Message}";
                lblEskiKur.Text = "";
                txtDovizKuru.Visible = true;
            }
        }
        private void btnKurGuncelle_Click(object sender, EventArgs e)
        {
            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string paraBirimi = ProjeFinans_ProjeKutukData.GetProjeParaBirimi(projeNo);
            if (string.IsNullOrEmpty(paraBirimi))
            {
                MessageBox.Show("Proje için para birimi bilgisi bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string dovizKuruText = txtDovizKuru.Text?.Trim() ?? "0";
            if (!decimal.TryParse(dovizKuruText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal kur) || kur <= 0)
            {
                MessageBox.Show("Geçerli bir döviz kuru giriniz (ör. 47,7028).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            kur = Math.Round(kur, 4); 

            var fiyatlandirmaVerileri = fiyatlandirmaData.GetFiyatlandirmaByProje(projeNo);
            var mevcutFiyatlandirma = fiyatlandirmaVerileri?.LastOrDefault(f => f.teklifKuru.HasValue);
            decimal? mevcutKur = mevcutFiyatlandirma?.teklifKuru;

            if (mevcutKur.HasValue && Math.Abs(mevcutKur.Value - kur) > 0.0001m)
            {
                var result = MessageBox.Show(
                    $"Fiyatlandırma kuru {mevcutKur.Value.ToString("N4", new CultureInfo("tr-TR"))} {paraBirimi}'den {kur.ToString("N4", new CultureInfo("tr-TR"))} {paraBirimi}'ye değiştirilecektir. Onaylıyor musunuz?",
                    "Kur Değiştirme Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (result != DialogResult.Yes)
                {
                    return;
                }

                foreach (var veri in fiyatlandirmaVerileri)
                {
                    if (veri.teklifKuru.HasValue)
                    {
                        fiyatlandirmaData.FiyatlandirmaGuncelle(
                            projeNo,
                            veri.fiyatlandirmaKalemId,
                            veri.teklifBirimMiktar,
                            veri.teklifBirimFiyat,
                            veri.gerceklesenBirimMiktar,
                            veri.gerceklesenBirimFiyat,
                            paraBirimi,
                            kur
                        );
                    }
                }
                lblEskiKur.Text = $"Kaydedilen Kur ({paraBirimi}): {kur.ToString("N4", new CultureInfo("tr-TR"))}";
            }
            else
            {
                lblEskiKur.Text = $"Kaydedilen Kur ({paraBirimi}): {kur.ToString("N4", new CultureInfo("tr-TR"))}";
            }

            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            try
            {
                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    var lblKalemAdi = GetLabelAt(row, 1);
                    if (lblKalemAdi == null || string.IsNullOrEmpty(lblKalemAdi.Text)) continue;

                    var panelTeklifAdet = tableLayoutPanel1.GetControlFromPosition(2, row) as Panel;
                    var panelGerceklesenAdet = tableLayoutPanel1.GetControlFromPosition(6, row) as Panel;
                    var txtTeklifBirimFiyat = GetTextBoxAt(row, 3);
                    var txtTeklifToplam = GetTextBoxAt(row, 4);
                    var txtTeklifToplamTL = GetTextBoxAt(row, 5);
                    var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 7);
                    var txtGerceklesenMaliyet = GetTextBoxAt(row, 8);
                    var lblSonDurum = GetLabelAt(row, 9);

                    var txtTeklifAdet = panelTeklifAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtTeklifAdet");
                    var txtGerceklesenAdet = panelGerceklesenAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtGerceklesenAdet");

                    decimal teklifAdet = decimal.TryParse(txtTeklifAdet?.Text?.Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal ta) ? ta : 0m;
                    decimal teklifBirimFiyat = decimal.TryParse(txtTeklifBirimFiyat?.Text?.Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal tbf) ? tbf : 0m;
                    decimal gerceklesenAdet = decimal.TryParse(txtGerceklesenAdet?.Text?.Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal ga) ? ga : 0m;
                    decimal gerceklesenBirimFiyat = decimal.TryParse(txtGerceklesenBirimFiyat?.Text?.Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal gbf) ? gbf : 0m;

                    teklifAdet = Math.Round(teklifAdet, 2);
                    teklifBirimFiyat = Math.Round(teklifBirimFiyat, 2);
                    gerceklesenAdet = Math.Round(gerceklesenAdet, 2);
                    gerceklesenBirimFiyat = Math.Round(gerceklesenBirimFiyat, 2);

                    decimal teklifToplam;
                    try
                    {
                        checked
                        {
                            teklifToplam = teklifAdet * teklifBirimFiyat;
                        }
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Girilen değerler çok büyük, lütfen daha küçük değerler kullanın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        teklifToplam = 0m;
                        txtTeklifAdet.Text = "0,00";
                        txtTeklifBirimFiyat.Text = "0,00";
                    }

                    decimal teklifToplamTL;
                    try
                    {
                        checked
                        {
                            teklifToplamTL = teklifToplam * kur;
                        }
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Girilen değerler çok büyük, lütfen daha küçük değerler kullanın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        teklifToplamTL = 0m;
                        txtTeklifAdet.Text = "0,00";
                        txtTeklifBirimFiyat.Text = "0,00";
                    }

                    decimal gerceklesenMaliyet;
                    try
                    {
                        checked
                        {
                            gerceklesenMaliyet = gerceklesenAdet * gerceklesenBirimFiyat;
                        }
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Girilen değerler çok büyük, lütfen daha küçük değerler kullanın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gerceklesenMaliyet = 0m;
                        txtGerceklesenAdet.Text = "0,00";
                        txtGerceklesenBirimFiyat.Text = "0,00";
                    }

                    decimal fark = teklifToplamTL - gerceklesenMaliyet;

                    if (txtTeklifToplam != null)
                        txtTeklifToplam.Text = $"{teklifToplam.ToString("N2", new CultureInfo("tr-TR"))} {paraBirimi}";
                    if (txtTeklifToplamTL != null)
                        txtTeklifToplamTL.Text = $"{teklifToplamTL.ToString("N2", new CultureInfo("tr-TR"))} TL";
                    if (txtGerceklesenMaliyet != null)
                        txtGerceklesenMaliyet.Text = $"{gerceklesenMaliyet.ToString("N2", new CultureInfo("tr-TR"))} TL";
                    if (lblSonDurum != null)
                    {
                        lblSonDurum.Text = $"{fark.ToString("N2", new CultureInfo("tr-TR"))} TL";
                        lblSonDurum.ForeColor = fark < 0 ? Color.Red : Color.Green;
                    }
                }

                UpdateTotalsRow();
                lblKurBaslik.Visible = false;
                lblKurBilgi.Visible = false;
                MessageBox.Show("Kur başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
                tableLayoutPanel2.ResumeLayout(true);
            }
        }
        private void InitializeTxtDovizKuruEvents()
        {
            txtDovizKuru.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
                {
                    e.Handled = true;
                }

                var textBox = s as TextBox;
                string text = textBox.Text;
                if (e.KeyChar == ',' && text.Contains(","))
                {
                    e.Handled = true;
                }

                string currentText = text.Replace(",", "");
                if (currentText.Length >= 15 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            txtDovizKuru.Leave += (s, e) =>
            {
                var textBox = s as TextBox;
                if (textBox != null)
                {
                    string text = textBox.Text.Trim();
                    if (decimal.TryParse(text, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal value))
                    {
                        value = Math.Round(value, 4);
                        textBox.Text = value.ToString("N4", new CultureInfo("tr-TR"));
                    }
                    else
                    {
                        textBox.Text = "0,0000";
                    }
                }
            };
        }
    }
}