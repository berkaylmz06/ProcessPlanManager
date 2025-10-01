using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeFiyatlandirma : UserControl
    {
        public event Action<string> OnFiyatlandirmaKaydedildi;
        private ComboBox cmbProjeNo;
        private List<int> altProjeler;
        private List<(string projeNo, string kalemAdi)> _pendingDeletions = new List<(string projeNo, string kalemAdi)>();
        private bool _isUpdating = false;
        private int? projeId = null;

        private readonly IServiceProvider _serviceProvider;
        private IOdemeSartlariService _odemeSartlariService => _serviceProvider.GetRequiredService<IOdemeSartlariService>();
        private IFiyatlandirmaService _fiyatlandirmaService => _serviceProvider.GetRequiredService<IFiyatlandirmaService>();
        private IFiyatlandirmaKalemleriService _fiyatlandirmaKalemleriService => _serviceProvider.GetRequiredService<IFiyatlandirmaKalemleriService>();
        private IFinansProjelerService _finansProjelerService => _serviceProvider.GetRequiredService<IFinansProjelerService>();
        private IProjeIliskiService _projeIliskiService => _serviceProvider.GetRequiredService<IProjeIliskiService>();
        private IProjeKutukService _projeKutukService => _serviceProvider.GetRequiredService<IProjeKutukService>();

        public ctlProjeFiyatlandirma(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

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
                    projeId = _finansProjelerService.GetProjeIdByNo(cmbProjeNo.SelectedItem.ToString());
                    if (projeId.HasValue)
                    {
                        LoadKalemler(projeId.Value);
                    }
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

        public void LoadProjeFiyatlandirma(int arananProjeId, bool autoSearch = false, List<int> altProjeler = null)
        {
            this.altProjeler = altProjeler;

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(arananProjeId);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Proje ID '{arananProjeId}' için bilgiler alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string projeNo = _finansProjelerService.GetProjeNoById(arananProjeId);
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show($"Proje ID '{arananProjeId}' için proje numarası bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var projeKutuk = _projeKutukService.ProjeKutukAra(arananProjeId);
            if (projeKutuk != null && projeKutuk.altProjeVarMi && altProjeler == null)
            {
                MessageBox.Show($"Proje '{projeNo}' alt projelere sahip. Lütfen bir alt proje seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            projeId = arananProjeId;
            txtProjeNo.Text = projeNo;

            if (altProjeler != null && altProjeler.Any())
            {
                txtProjeNo.Visible = false;
                cmbProjeNo.Visible = true;
                cmbProjeNo.Items.Clear();
                cmbProjeNo.Items.AddRange(altProjeler.Select(id => _finansProjelerService.GetProjeNoById(id) ?? id.ToString()).ToArray());
                cmbProjeNo.SelectedItem = projeNo;
                cmbProjeNo.BringToFront();
            }
            else
            {
                txtProjeNo.Visible = true;
                cmbProjeNo.Visible = false;
            }

            if (autoSearch)
            {
                LoadKalemler(arananProjeId);
            }
        }
        private decimal GetKur(string paraBirimi, int fiyatlandirmaKalemId = 0, string projeNo = "")
        {
            if (paraBirimi == "TL")
            {
                lblEskiKur.Text = "";
                return 1m;
            }

            if (projeId.HasValue && fiyatlandirmaKalemId > 0 && !string.IsNullOrEmpty(projeNo))
            {
                var fiyatlandirmaVerileri = _fiyatlandirmaService.GetFiyatlandirmaByProje(projeId.Value);
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
        private void LoadKalemler(int arananProjeId)
        {
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            try
            {
                projeId = arananProjeId;
                var projeBilgi = _finansProjelerService.GetProjeBilgileri(arananProjeId);
                if (projeBilgi == null)
                {
                    MessageBox.Show($"Proje ID '{arananProjeId}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    
                string projeNo = _finansProjelerService.GetProjeNoById(arananProjeId);
                txtProjeNo.Text = projeNo;

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

                string paraBirimi = _projeKutukService.GetProjeParaBirimi(arananProjeId);
                var fiyatlandirmaVerileri = _fiyatlandirmaService.GetFiyatlandirmaByProje(arananProjeId);

                foreach (var veri in fiyatlandirmaVerileri)
                {
                    if (string.IsNullOrEmpty(veri.kalem.kalemAdi))
                    {
                        continue;
                    }

                    string kalemBirimi = veri.kalem.kalemBirimi;
                    AddYeniKalemSatiri(veri.kalem.kalemAdi, veri.kalem.kalemBirimi);
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

                    if (txtTeklifAdet != null) txtTeklifAdet.Text = veri.teklifBirimMiktar.ToString("N4", new CultureInfo("tr-TR"));
                    if (txtTeklifBirim != null) txtTeklifBirim.Text = kalemBirimi;
                    if (txtTeklifBirimFiyat != null) txtTeklifBirimFiyat.Text = veri.teklifBirimFiyat.ToString("N4", new CultureInfo("tr-TR"));
                    if (txtGerceklesenAdet != null) txtGerceklesenAdet.Text = veri.gerceklesenBirimMiktar.ToString("N4", new CultureInfo("tr-TR"));
                    if (txtGerceklesenBirim != null) txtGerceklesenBirim.Text = kalemBirimi;
                    if (txtGerceklesenBirimFiyat != null) txtGerceklesenBirimFiyat.Text = veri.gerceklesenBirimFiyat.ToString("N4", new CultureInfo("tr-TR"));

                    decimal kur = veri.teklifKuru.HasValue ? veri.teklifKuru.Value : GetKur(paraBirimi, veri.fiyatlandirmaKalemId, projeNo);
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

                    if (txtTeklifToplam != null) txtTeklifToplam.Text = $"{teklifToplam.ToString("N4", new CultureInfo("tr-TR"))} {paraBirimi}";
                    if (txtTeklifToplamTL != null) txtTeklifToplamTL.Text = $"{teklifToplamTL.ToString("N4", new CultureInfo("tr-TR"))} TL";
                    if (txtGerceklesenMaliyet != null) txtGerceklesenMaliyet.Text = $"{gerceklesenMaliyet.ToString("N4", new CultureInfo("tr-TR"))} TL";
                    if (lblSonDurum != null)
                    {
                        lblSonDurum.Text = $"{fark.ToString("N4", new CultureInfo("tr-TR"))} TL";
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
                "",
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

                // DÜZELTME: Bu metodun içinde de projeId kontrolü ekleniyor.
                string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
                var currentProjeId = _finansProjelerService.GetProjeIdByNo(projeNo);
                if (!currentProjeId.HasValue)
                {
                    MessageBox.Show($"Proje '{projeNo}' bulunamadığı için para birimi alınamadı ve kalem eklenemiyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string paraBirimi = _projeKutukService.GetProjeParaBirimi(currentProjeId.Value);

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
                                    value = Math.Round(value, 4);
                                    textBox.Text = value.ToString("N4", new CultureInfo("tr-TR"));
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
                                        value = Math.Round(value, 4);
                                        textBox.Text = value.ToString("N4", new CultureInfo("tr-TR"));
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

            return GetRowFromControl(control.Parent);
        }
        private void HesaplaVeGuncelle(object sender, EventArgs e)
        {
            if (_isUpdating) return;

            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                UpdateTotalsRow();
                return;
            }

            var currentProjeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            if (!currentProjeId.HasValue)
            {
                UpdateTotalsRow();
                return;
            }

            int row = -1;
            if (sender is Control control)
            {
                row = GetRowFromControl(control);
            }

            if (row < 1 || (tableLayoutPanel1.GetControlFromPosition(1, row) is Label lbl && string.IsNullOrEmpty(lbl.Text)))
            {
                UpdateTotalsRow();
                return;
            }

            string paraBirimi = _projeKutukService.GetProjeParaBirimi(currentProjeId.Value);
            string kalemAdi = GetLabelAt(row, 1)?.Text;
            int fiyatlandirmaKalemId = 0;
            decimal kur = 0;

            if (!string.IsNullOrEmpty(kalemAdi))
            {
                var kalemDetay = _fiyatlandirmaKalemleriService.GetFiyatlandirmaKalemByAdi(kalemAdi);
                if (kalemDetay != null)
                {
                    fiyatlandirmaKalemId = kalemDetay.fiyatlandirmaKalemId;
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

                teklifAdet = Math.Round(teklifAdet, 4);
                teklifBirimFiyat = Math.Round(teklifBirimFiyat, 4);
                gerceklesenAdet = Math.Round(gerceklesenAdet, 4);
                gerceklesenBirimFiyat = Math.Round(gerceklesenBirimFiyat, 4);

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
                    txtTeklifToplam.Text = $"{teklifToplam.ToString("N4", new CultureInfo("tr-TR"))} {paraBirimi}";
                if (txtTeklifToplamTL != null)
                    txtTeklifToplamTL.Text = $"{teklifToplamTL.ToString("N4", new CultureInfo("tr-TR"))} TL";
                if (txtGerceklesenMaliyet != null)
                    txtGerceklesenMaliyet.Text = $"{gerceklesenMaliyet.ToString("N4", new CultureInfo("tr-TR"))} TL";
                if (lblSonDurum != null)
                {
                    lblSonDurum.Text = $"{fark.ToString("N4", new CultureInfo("tr-TR"))} TL";
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

                var currentProjeId = _finansProjelerService.GetProjeIdByNo(projeNo);
                if (!currentProjeId.HasValue)
                {
                    return;
                }
                this.projeId = currentProjeId;

                string paraBirimi = _projeKutukService.GetProjeParaBirimi(projeId.Value);
                decimal kur = GetKur(paraBirimi);

                var fiyatlandirmaVerileri = _fiyatlandirmaService.GetFiyatlandirmaByProje(projeId.Value);
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
                    if (tableLayoutPanel1.GetControlFromPosition(1, row) is Label lbl && string.IsNullOrEmpty(lbl.Text)) continue;

                    decimal teklifToplamTL = decimal.TryParse(GetTextBoxAt(row, 5)?.Text.Replace(" TL", "").Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal ttl) ? ttl : 0m;
                    decimal gerceklesenMaliyet = decimal.TryParse(GetTextBoxAt(row, 8)?.Text.Replace(" TL", "").Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal gm) ? gm : 0m;

                    toplamTeklifTL += teklifToplamTL;
                    toplamGerceklesen += gerceklesenMaliyet;
                    toplamSonDurum += teklifToplamTL - gerceklesenMaliyet;
                }

                var lblToplamTeklifHesap = new Label
                {
                    Text = $"{toplamTeklif.ToString("N4", new CultureInfo("tr-TR"))} {paraBirimi}",
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
                    Text = $"{toplamTeklifTL.ToString("N4", new CultureInfo("tr-TR"))} TL",
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
                    Text = $"{toplamGerceklesen.ToString("N4", new CultureInfo("tr-TR"))} TL",
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
                    Text = $"{toplamSonDurum.ToString("N4", new CultureInfo("tr-TR"))} TL",
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
            return tableLayoutPanel1.GetControlFromPosition(col, row) as TextBox;
        }

        private Label GetLabelAt(int row, int col)
        {
            return tableLayoutPanel1.GetControlFromPosition(col, row) as Label;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!projeId.HasValue)
            {
                MessageBox.Show("Lütfen geçerli bir proje seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(projeId.Value);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Proje ID '{projeId.Value}' için bilgiler alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string projeNo = _finansProjelerService.GetProjeNoById(projeId.Value);
            var projeKutuk = _projeKutukService.ProjeKutukAra(projeId.Value);
            if (projeKutuk != null && projeKutuk.altProjeVarMi && altProjeler == null)
            {
                MessageBox.Show($"Proje ID '{projeId.Value}' alt projelere sahip. Ana proje için fiyatlandırma yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tableLayoutPanel1.SuspendLayout();
            try
            {
                bool isAnyUpdateOrSave = false;
                foreach (var deletion in _pendingDeletions)
                {
                    var kalemDetay = _fiyatlandirmaKalemleriService.GetFiyatlandirmaKalemByAdi(deletion.kalemAdi);
                    if (kalemDetay != null)
                    {
                        var deletionProjeId = _finansProjelerService.GetProjeIdByNo(deletion.projeNo);
                        if (deletionProjeId.HasValue && _fiyatlandirmaService.FiyatlandirmaSilById(deletionProjeId.Value, kalemDetay.fiyatlandirmaKalemId))
                        {
                            isAnyUpdateOrSave = true;
                        }
                    }
                }
                _pendingDeletions.Clear();

                var mevcutFiyatlandirmalar = _fiyatlandirmaService.GetFiyatlandirmaByProje(projeId.Value)
                    .ToDictionary(f => f.kalem.kalemAdi, f => f, StringComparer.OrdinalIgnoreCase);

                string paraBirimi = _projeKutukService.GetProjeParaBirimi(projeId.Value);
                decimal kurToUse;
                if (paraBirimi == "TL")
                {
                    kurToUse = 1m;
                }
                else
                {
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
                            MessageBox.Show($"Geçerli bir döviz kuru giriniz (ör. 47,7028). {paraBirimi} için kur gereklidir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        kurToUse = Math.Round(dovizKuru, 4);
                    }

                    if (kurToUse < 0.1m || kurToUse > 1000m)
                    {
                        MessageBox.Show($"Kur değeri ({kurToUse:N4}) geçerli bir aralıkta değil (0.1 - 1000).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    var lblKalemAdi = GetLabelAt(row, 1);
                    if (lblKalemAdi == null || string.IsNullOrEmpty(lblKalemAdi.Text))
                    {
                        continue;
                    }

                    var kalemDetay = _fiyatlandirmaKalemleriService.GetFiyatlandirmaKalemByAdi(lblKalemAdi.Text);
                    if (kalemDetay == null)
                    {
                        MessageBox.Show($"Kalem '{lblKalemAdi.Text}' için ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    int fiyatlandirmaKalemId = kalemDetay.fiyatlandirmaKalemId;
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

                    teklifAdet = Math.Round(teklifAdet, 4);
                    teklifBirimFiyat = Math.Round(teklifBirimFiyat, 4);
                    gerceklesenAdet = Math.Round(gerceklesenAdet, 4);
                    gerceklesenBirimFiyat = Math.Round(gerceklesenBirimFiyat, 4);

                    var fiyatlandirma = new Fiyatlandirma
                    {
                        projeId = projeId.Value,
                        fiyatlandirmaKalemId = fiyatlandirmaKalemId,
                        teklifBirimMiktar = teklifAdet,
                        teklifBirimFiyat = teklifBirimFiyat,
                        gerceklesenBirimMiktar = gerceklesenAdet,
                        gerceklesenBirimFiyat = gerceklesenBirimFiyat,
                        teklifDovizKodu = paraBirimi,
                        teklifKuru = kurToUse
                    };

                    if (mevcutFiyatlandirmalar.TryGetValue(lblKalemAdi.Text, out var mevcutFiyatlandirma))
                    {
                        bool hasChanges = Math.Abs(mevcutFiyatlandirma.teklifBirimMiktar - teklifAdet) > 0.0001m ||
                                          Math.Abs(mevcutFiyatlandirma.teklifBirimFiyat - teklifBirimFiyat) > 0.0001m ||
                                          Math.Abs(mevcutFiyatlandirma.gerceklesenBirimMiktar - gerceklesenAdet) > 0.0001m ||
                                          Math.Abs(mevcutFiyatlandirma.gerceklesenBirimFiyat - gerceklesenBirimFiyat) > 0.0001m ||
                                          Math.Abs(mevcutFiyatlandirma.teklifKuru.GetValueOrDefault() - kurToUse) > 0.0001m;

                        if (hasChanges)
                        {
                            _fiyatlandirmaService.FiyatlandirmaGuncelle(fiyatlandirma);
                            isAnyUpdateOrSave = true;
                        }
                    }
                    else
                    {
                        _fiyatlandirmaService.FiyatlandirmaKaydet(fiyatlandirma);
                        isAnyUpdateOrSave = true;
                    }

                    txtTeklifAdet.Text = teklifAdet.ToString("N4", new CultureInfo("tr-TR"));
                    txtTeklifBirimFiyat.Text = teklifBirimFiyat.ToString("N4", new CultureInfo("tr-TR"));
                    txtGerceklesenAdet.Text = gerceklesenAdet.ToString("N4", new CultureInfo("tr-TR"));
                    txtGerceklesenBirimFiyat.Text = gerceklesenBirimFiyat.ToString("N4", new CultureInfo("tr-TR"));
                }

                if (isAnyUpdateOrSave)
                {
                    var (toplamBedel, eksikFiyatlandirmaProjeler) = _fiyatlandirmaService.GetToplamBedel(projeId.Value, altProjeler);
                    OnFiyatlandirmaKaydedildi?.Invoke(projeNo);
                    if (paraBirimi != "TL")
                    {
                        lblEskiKur.Text = $"Kaydedilen Kur ({paraBirimi}): {kurToUse.ToString("N4", new CultureInfo("tr-TR"))}";
                    }
                    else
                    {
                        lblEskiKur.Text = "";
                    }
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
        private decimal ParseDecimal(string text)
        {
            return decimal.TryParse(text?.Trim(), NumberStyles.Any, new CultureInfo("tr-TR"), out decimal value) ? Math.Round(value, 4) : 0m;
        }

        private TextBox GetTextBoxAtRowAndColumn(int row, int column, string name)
        {
            var panel = tableLayoutPanel1.GetControlFromPosition(column, row) as Panel;
            return panel?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == name);
        }
        public void btnProjeAra_Click(object sender, EventArgs e)
        {
            string projeNo = txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Lütfen geçerli bir proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnYeniKalemEkle.Enabled = false;
                return;
            }

            var arananProjeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            if (!arananProjeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProjeNo.Text = null;
                btnYeniKalemEkle.Enabled = false;
                return;
            }

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(arananProjeId.Value);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Proje '{projeNo}' için bilgiler alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProjeNo.Text = null;
                btnYeniKalemEkle.Enabled = false;
                return;
            }

            var projeKutuk = _projeKutukService.ProjeKutukAra(arananProjeId.Value);
            if (projeKutuk != null && projeKutuk.altProjeVarMi)
            {
                MessageBox.Show($"Proje '{projeNo}' alt projelere sahip. Ana proje için fiyatlandırma yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnYeniKalemEkle.Enabled = false;
                return;
            }

            projeId = arananProjeId.Value;
            txtProjeNo.Text = projeNo;

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

                string paraBirimi = _projeKutukService.GetProjeParaBirimi(arananProjeId.Value);
                var fiyatlandirmaVerileri = _fiyatlandirmaService.GetFiyatlandirmaByProje(arananProjeId.Value);
                if (fiyatlandirmaVerileri.Any())
                {
                    foreach (var veri in fiyatlandirmaVerileri)
                    {
                        if (string.IsNullOrEmpty(veri.kalem.kalemAdi)) continue;
                        string kalemBirimi = veri.kalem.kalemBirimi;
                        AddYeniKalemSatiri(veri.kalem.kalemAdi, veri.kalem.kalemBirimi);
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

                        if (txtTeklifAdet != null) txtTeklifAdet.Text = veri.teklifBirimMiktar.ToString("N4", new CultureInfo("tr-TR"));
                        if (txtTeklifBirim != null) txtTeklifBirim.Text = kalemBirimi;
                        if (txtTeklifBirimFiyat != null) txtTeklifBirimFiyat.Text = veri.teklifBirimFiyat.ToString("N4", new CultureInfo("tr-TR"));
                        if (txtGerceklesenAdet != null) txtGerceklesenAdet.Text = veri.gerceklesenBirimMiktar.ToString("N4", new CultureInfo("tr-TR"));
                        if (txtGerceklesenBirim != null) txtGerceklesenBirim.Text = kalemBirimi;
                        if (txtGerceklesenBirimFiyat != null) txtGerceklesenBirimFiyat.Text = veri.gerceklesenBirimFiyat.ToString("N4", new CultureInfo("tr-TR"));

                        decimal kur = veri.teklifKuru.HasValue ? veri.teklifKuru.Value : GetKur(paraBirimi, veri.fiyatlandirmaKalemId, projeNo);
                        if (veri.teklifKuru.HasValue)
                        {
                            lblEskiKur.Text = $"Kaydedilen Kur ({paraBirimi}): {kur.ToString("N4", new CultureInfo("tr-TR"))}";
                        }
                        else
                        {
                            lblEskiKur.Text = "";
                        }

                        decimal teklifToplam = veri.teklifBirimMiktar * veri.teklifBirimFiyat;
                        decimal teklifToplamTL = teklifToplam * kur;
                        decimal gerceklesenMaliyet = veri.gerceklesenBirimMiktar * veri.gerceklesenBirimFiyat;
                        decimal fark = teklifToplamTL - gerceklesenMaliyet;

                        if (txtTeklifToplam != null) txtTeklifToplam.Text = $"{teklifToplam.ToString("N4", new CultureInfo("tr-TR"))} {paraBirimi}";
                        if (txtTeklifToplamTL != null) txtTeklifToplamTL.Text = $"{teklifToplamTL.ToString("N4", new CultureInfo("tr-TR"))} TL";
                        if (txtGerceklesenMaliyet != null) txtGerceklesenMaliyet.Text = $"{gerceklesenMaliyet.ToString("N4", new CultureInfo("tr-TR"))} TL";
                        if (lblSonDurum != null)
                        {
                            lblSonDurum.Text = $"{fark.ToString("N4", new CultureInfo("tr-TR"))} TL";
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
            catch (Exception ex)
            {
                MessageBox.Show($"Proje verileri yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var frm = new frmYeniFiyatlandirmaKalemiEkle(_fiyatlandirmaKalemleriService);
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

            projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            var projeBilgi = projeId.HasValue ? _finansProjelerService.GetProjeBilgileri(projeId.Value) : null;
            btnYeniKalemEkle.Enabled = projeBilgi != null;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string projeNo = cmbProjeNo.Visible
                ? cmbProjeNo.SelectedItem?.ToString()
                : txtProjeNo.Text.Trim();

            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Lütfen bir proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projeIdToDelete = _finansProjelerService.GetProjeIdByNo(projeNo);
            if (!projeIdToDelete.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(projeIdToDelete.Value);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Proje '{projeNo}' için bilgiler alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ilgiliProjeIds = new List<int> { projeIdToDelete.Value };

            var altProjeler = _projeIliskiService.GetAltProjeler(projeIdToDelete.Value);
            if (altProjeler != null && altProjeler.Any())
            {
                ilgiliProjeIds.AddRange(altProjeler);
            }

            var ustProjeId = _projeIliskiService.GetUstProjeId(projeIdToDelete.Value);
            if (ustProjeId.HasValue)
            {
                ilgiliProjeIds.Add(ustProjeId.Value);
            }

            bool hasOdemeSartlari = ilgiliProjeIds.Any(pid =>
            {
                var odemeBilgileri = _odemeSartlariService.GetOdemeBilgileriByProjeId(pid);
                return odemeBilgileri.Any();
            });

            if (hasOdemeSartlari)
            {
                MessageBox.Show(
                    $"Proje '{projeNo}' veya ilişkili üst/alt projeleri için Ödeme Şartları'nda kaydı var, silinemedi.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var result = MessageBox.Show(
                $"Proje '{projeNo}' için tüm fiyatlandırma kayıtları silinecek. Onaylıyor musunuz?",
                "Fiyatlandırma Silme",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            if (_fiyatlandirmaService.FiyatlandirmaSil(projeIdToDelete.Value))
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

                var currentProjeId = _finansProjelerService.GetProjeIdByNo(projeNo);
                if (!currentProjeId.HasValue)
                {
                    txtDovizKuru.Text = "";
                    lblKurBilgi.Text = $"Proje '{projeNo}' bulunamadı.";
                    lblEskiKur.Text = "";
                    panelLeft.Visible = true;
                    return;
                }

                string paraBirimi = _projeKutukService.GetProjeParaBirimi(currentProjeId.Value);
                if (string.IsNullOrEmpty(paraBirimi))
                {
                    txtDovizKuru.Text = "";
                    lblKurBilgi.Text = "Proje için para birimi bilgisi bulunamadı.";
                    lblEskiKur.Text = "";
                    panelLeft.Visible = true;
                    return;
                }

                if (paraBirimi == "TL")
                {
                    txtDovizKuru.Text = "";
                    lblKurBilgi.Text = "";
                    lblEskiKur.Text = "";
                    panelLeft.Visible = false;
                    return;
                }

                DateTime previousDay = DateTime.Today.AddDays(-1);
                if (previousDay.DayOfWeek == DayOfWeek.Saturday)
                    previousDay = previousDay.AddDays(-1);
                else if (previousDay.DayOfWeek == DayOfWeek.Sunday)
                    previousDay = previousDay.AddDays(-2);

                string yearMonth = previousDay.ToString("yyyyMM");
                string dayMonthYear = previousDay.ToString("ddMMyyyy");
                string tcmbUrl = $"https://www.tcmb.gov.tr/kurlar/{yearMonth}/{dayMonthYear}.xml";

                XDocument xmlDoc;
                try
                {
                    xmlDoc = XDocument.Load(tcmbUrl);
                }
                catch (System.Net.WebException)
                {
                    txtDovizKuru.Text = "";
                    lblKurBilgi.Text = "Önceki günün kur bilgisi alınamadı.";
                    lblEskiKur.Text = "";
                    panelLeft.Visible = true;
                    return;
                }

                string kurText = "";
                if (paraBirimi == "USD")
                {
                    var usdKur = xmlDoc.Descendants("Currency")
                                       .FirstOrDefault(c => c.Attribute("Kod")?.Value == "USD");
                    kurText = usdKur?.Element("ForexBuying")?.Value;
                    lblKurBilgi.Text = "ABD Doları (USD) Alış Kuru";
                    panelLeft.Visible = true;
                }
                else if (paraBirimi == "EUR")
                {
                    var eurKur = xmlDoc.Descendants("Currency")
                                       .FirstOrDefault(c => c.Attribute("Kod")?.Value == "EUR");
                    kurText = eurKur?.Element("ForexBuying")?.Value;
                    lblKurBilgi.Text = "Euro (EUR) Alış Kuru";
                    panelLeft.Visible = true;
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

                var fiyatlandirmaVerileri = _fiyatlandirmaService.GetFiyatlandirmaByProje(currentProjeId.Value);
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
                panelLeft.Visible = true;
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

            var currentProjeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            if (!currentProjeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string paraBirimi = _projeKutukService.GetProjeParaBirimi(currentProjeId.Value);
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

            var fiyatlandirmaVerileri = _fiyatlandirmaService.GetFiyatlandirmaByProje(currentProjeId.Value);
            var mevcutFiyatlandirmaRecord = fiyatlandirmaVerileri?.LastOrDefault(f => f.teklifKuru.HasValue);
            decimal? mevcutKur = mevcutFiyatlandirmaRecord?.teklifKuru;

            if (mevcutKur.HasValue && Math.Abs(mevcutKur.Value - kur) > 0.0001m)
            {
                var result = MessageBox.Show(
                    $"Fiyatlandırma kuru {mevcutKur.Value.ToString("N4", new CultureInfo("tr-TR"))} yerine {kur.ToString("N4", new CultureInfo("tr-TR"))} olarak güncellenecek. Devam etmek istiyor musunuz?",
                    "Kur Güncelleme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            tableLayoutPanel1.SuspendLayout();
            try
            {
                bool isAnyUpdate = false;
                var mevcutFiyatlandirmalar = _fiyatlandirmaService.GetFiyatlandirmaByProje(currentProjeId.Value)
                    .ToDictionary(f => f.kalem.kalemAdi?.Trim(), f => f, StringComparer.OrdinalIgnoreCase);

                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    var lblKalemAdi = GetLabelAt(row, 1);
                    if (lblKalemAdi == null || string.IsNullOrEmpty(lblKalemAdi.Text?.Trim()))
                    {
                        continue;
                    }

                    string kalemAdi = lblKalemAdi.Text.Trim();
                    var kalemDetay = _fiyatlandirmaKalemleriService.GetFiyatlandirmaKalemByAdi(kalemAdi);
                    if (kalemDetay == null)
                    {
                        MessageBox.Show($"Kalem '{kalemAdi}' için detay bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    int fiyatlandirmaKalemId = kalemDetay.fiyatlandirmaKalemId;
                    var panelTeklifAdet = tableLayoutPanel1.GetControlFromPosition(2, row) as Panel;
                    var txtTeklifBirimFiyat = GetTextBoxAt(row, 3);
                    var panelGerceklesenAdet = tableLayoutPanel1.GetControlFromPosition(6, row) as Panel;
                    var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 7);

                    var txtTeklifAdet = panelTeklifAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtTeklifAdet");
                    var txtGerceklesenAdet = panelGerceklesenAdet?.Controls.OfType<TextBox>().FirstOrDefault(t => t.Name == "txtGerceklesenAdet");

                    if (txtTeklifAdet == null || txtGerceklesenAdet == null || txtTeklifBirimFiyat == null || txtGerceklesenBirimFiyat == null)
                    {
                        MessageBox.Show($"Satır {row} için gerekli alanlar eksik.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    teklifAdet = Math.Round(teklifAdet, 4);
                    teklifBirimFiyat = Math.Round(teklifBirimFiyat, 4);
                    gerceklesenAdet = Math.Round(gerceklesenAdet, 4);
                    gerceklesenBirimFiyat = Math.Round(gerceklesenBirimFiyat, 4);

                    var fiyatlandirma = new Fiyatlandirma
                    {
                        projeId = currentProjeId.Value,
                        fiyatlandirmaKalemId = fiyatlandirmaKalemId,
                        teklifBirimMiktar = teklifAdet,
                        teklifBirimFiyat = teklifBirimFiyat,
                        gerceklesenBirimMiktar = gerceklesenAdet,
                        gerceklesenBirimFiyat = gerceklesenBirimFiyat,
                        teklifDovizKodu = paraBirimi,
                        teklifKuru = kur
                    };

                    if (mevcutFiyatlandirmalar.TryGetValue(kalemAdi, out var fiyatlandirmaKaydi))
                    {
                        _fiyatlandirmaService.FiyatlandirmaGuncelle(fiyatlandirma);
                    }
                    else
                    {
                        _fiyatlandirmaService.FiyatlandirmaKaydet(fiyatlandirma);
                    }
                    isAnyUpdate = true;

                    txtTeklifAdet.Text = teklifAdet.ToString("N4", new CultureInfo("tr-TR"));
                    txtTeklifBirimFiyat.Text = teklifBirimFiyat.ToString("N4", new CultureInfo("tr-TR"));
                    txtGerceklesenAdet.Text = gerceklesenAdet.ToString("N4", new CultureInfo("tr-TR"));
                    txtGerceklesenBirimFiyat.Text = gerceklesenBirimFiyat.ToString("N4", new CultureInfo("tr-TR"));
                }

                if (isAnyUpdate)
                {
                    lblEskiKur.Text = $"Kaydedilen Kur ({paraBirimi}): {kur.ToString("N4", new CultureInfo("tr-TR"))}";
                    MessageBox.Show("Kur ve fiyatlandırma bilgileri başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKalemler(currentProjeId.Value);
                }
                else
                {
                    MessageBox.Show("Güncellenecek veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kur güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
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

        private void btnKopyala_Click(object sender, EventArgs e)
        {
            string targetProjeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeNo.Text.Trim();
            if (string.IsNullOrWhiteSpace(targetProjeNo))
            {
                MessageBox.Show("Lütfen önce fiyatlandırmayı kopyalamak istediğiniz projeyi aratın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            projeId = _finansProjelerService.GetProjeIdByNo(targetProjeNo);
            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{targetProjeNo}' bulunamadı. Lütfen geçerli bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sourceProjeNo = Microsoft.VisualBasic.Interaction.InputBox("Kopyalamak istediğiniz proje numarasını girin:", "Proje Kopyalama", "");
            if (string.IsNullOrWhiteSpace(sourceProjeNo))
            {
                MessageBox.Show("Kaynak proje numarası girilmedi.", "İşlem İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (sourceProjeNo.Equals(targetProjeNo, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Kaynak ve hedef proje numaraları aynı olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sourceProjeId = _finansProjelerService.GetProjeIdByNo(sourceProjeNo);
            if (!sourceProjeId.HasValue)
            {
                MessageBox.Show($"Kaynak proje '{sourceProjeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sourceFiyatlandirmalar = _fiyatlandirmaService.GetFiyatlandirmaByProje(sourceProjeId.Value);
            if (sourceFiyatlandirmalar == null || !sourceFiyatlandirmalar.Any())
            {
                MessageBox.Show($"'{sourceProjeNo}' numaralı projeye ait fiyatlandırma bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _pendingDeletions.Clear();
            var existingFiyatlandirmalar = _fiyatlandirmaService.GetFiyatlandirmaByProje(projeId.Value);
            foreach (var existing in existingFiyatlandirmalar)
            {
                _pendingDeletions.Add((targetProjeNo, existing.kalem.kalemAdi));
            }

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
            for (int i = 1; i < 10; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 9));
            }
            AddHeaderRow();

            string targetParaBirimi = _projeKutukService.GetProjeParaBirimi(projeId.Value) ?? "TL";
            if (string.IsNullOrEmpty(targetParaBirimi))
            {
                MessageBox.Show($"Hedef proje '{targetProjeNo}' için para birimi bilgisi bulunamadı, varsayılan olarak 'TL' kullanıldı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            foreach (var fiyatlandirma in sourceFiyatlandirmalar)
            {
                AddYeniKalemSatiri(fiyatlandirma.kalem.kalemAdi, fiyatlandirma.kalem.kalemBirimi);
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

                if (txtTeklifAdet != null) txtTeklifAdet.Text = fiyatlandirma.teklifBirimMiktar.ToString("N4", new CultureInfo("tr-TR"));
                if (txtTeklifBirim != null) txtTeklifBirim.Text = fiyatlandirma.kalem.kalemBirimi;
                if (txtTeklifBirimFiyat != null) txtTeklifBirimFiyat.Text = fiyatlandirma.teklifBirimFiyat.ToString("N4", new CultureInfo("tr-TR"));
                if (txtTeklifToplam != null) txtTeklifToplam.Text = (fiyatlandirma.teklifBirimMiktar * fiyatlandirma.teklifBirimFiyat).ToString("N4", new CultureInfo("tr-TR")) + $" {targetParaBirimi}";
                if (txtGerceklesenAdet != null) txtGerceklesenAdet.Text = "0,0000";
                if (txtGerceklesenBirim != null) txtGerceklesenBirim.Text = fiyatlandirma.kalem.kalemBirimi;
                if (txtGerceklesenBirimFiyat != null) txtGerceklesenBirimFiyat.Text = "0,0000";
                if (txtGerceklesenMaliyet != null) txtGerceklesenMaliyet.Text = "0,0000 TL";

                decimal kur = decimal.TryParse(txtDovizKuru.Text, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal parsedKur) ? parsedKur : 1m;
                decimal teklifToplam = 0m;
                if (txtTeklifToplam != null && !string.IsNullOrEmpty(txtTeklifToplam.Text))
                {
                    string teklifToplamText = txtTeklifToplam.Text.Replace(targetParaBirimi, "").Trim();
                    teklifToplam = decimal.TryParse(teklifToplamText, NumberStyles.Any, new CultureInfo("tr-TR"), out decimal tt) ? tt : 0m;
                }
                decimal teklifToplamTL = teklifToplam * kur;
                decimal gerceklesenMaliyet = 0m;
                decimal fark = teklifToplamTL - gerceklesenMaliyet;

                if (txtTeklifToplamTL != null) txtTeklifToplamTL.Text = $"{teklifToplamTL.ToString("N4", new CultureInfo("tr-TR"))} TL";
                if (lblSonDurum != null)
                {
                    lblSonDurum.Text = $"{fark.ToString("N4", new CultureInfo("tr-TR"))} TL";
                    lblSonDurum.ForeColor = fark < 0 ? Color.Red : Color.Green;
                }
            }

            AddSpacerRow();
            UpdateTotalsRow();
            MessageBox.Show($"'{sourceProjeNo}' projesine ait fiyatlandırma '{targetProjeNo}' projesine başarıyla kopyalandı. Kaydet butonuna basarak değişiklikleri kaydedebilirsiniz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}