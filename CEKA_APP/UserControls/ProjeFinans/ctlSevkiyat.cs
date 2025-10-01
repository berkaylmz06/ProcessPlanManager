using CEKA_APP.Entitys.Genel;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Interfaces;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl.ProjeFinans
{
    public partial class ctlSevkiyat : UserControl
    {
        private List<(int projeId, int sevkiyatId, int aracSira)> _pendingDeletions = new List<(int, int, int)>();
        private int? projeId = null;
        public event Action<string> OnSevkiyatKaydedildi;

        private ComboBox cmbProjeNo;
        private int sevkiyatAracSayisi = 0;
        private int sayfaIdSevkiyat = (int)SayfaTipi.Sevkiyat;
        private readonly IServiceProvider _serviceProvider;

        private ISevkiyatService _sevkiyatService => _serviceProvider.GetRequiredService<ISevkiyatService>();
        private IFiyatlandirmaService _fiyatlandirmaService => _serviceProvider.GetRequiredService<IFiyatlandirmaService>();
        private ISevkiyatPaketleriService _sevkiyatPaketleriService => _serviceProvider.GetRequiredService<ISevkiyatPaketleriService>();
        private IFinansProjelerService _finansProjelerService => _serviceProvider.GetRequiredService<IFinansProjelerService>();
        private IProjeIliskiService _projeIliskiService => _serviceProvider.GetRequiredService<IProjeIliskiService>();
        private IProjeKutukService _projeKutukService => _serviceProvider.GetRequiredService<IProjeKutukService>();
        private ISayfaStatusService _sayfaStatusService => _serviceProvider.GetRequiredService<ISayfaStatusService>();
        public ctlSevkiyat(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            this.DoubleBuffered = true;

            cmbProjeNo = new ComboBox
            {
                Size = txtProjeAra.Size,
                Location = txtProjeAra.Location,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Visible = false,
                Font = new Font("Segoe UI", 10)
            };
            cmbProjeNo.SelectedIndexChanged += (s, e) =>
            {
                if (cmbProjeNo.SelectedItem != null)
                {
                    projeId = _finansProjelerService.GetProjeIdByNo(cmbProjeNo.SelectedItem.ToString());
                    LoadSevkiyatlar(cmbProjeNo.SelectedItem.ToString());
                }
            };
            panelUst.Controls.Add(cmbProjeNo);
            cmbProjeNo.BringToFront();

            btnPaketEkle.Enabled = false;
            btnSevkiyatEkle.Enabled = false;
        }
        private void ctlSevkiyat_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Sevkiyat";
            panelUst.AutoScroll = true;

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
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 10));
            }

            InitializeTableStructure();
        }

        private void InitializeTableStructure()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.RowCount = 0;
                sevkiyatAracSayisi = 0;

                AddHeaderRow();
                AddSpacerRow();
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private void LoadSevkiyatlar(string projeNo)
        {
            projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' için projeId bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tableLayoutPanel1.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnCount = 11;
                tableLayoutPanel1.RowCount = 1;
                tableLayoutPanel1.RowStyles.Clear();

                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
                for (int i = 1; i < 11; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 10));
                }

                AddHeaderRow();
                AddSpacerRow();

                var sevkiyatVerileri = _sevkiyatService.GetSevkiyatByProje(projeId.Value);
                sevkiyatAracSayisi = sevkiyatVerileri.Any() ? sevkiyatVerileri.Max(s => s.aracSira) : 0;

                if (sevkiyatVerileri.Any())
                {
                    var filteredSevkiyatVerileri = sevkiyatVerileri
                        .Where(s => !_pendingDeletions.Any(pd => pd.projeId == projeId.Value && pd.sevkiyatId == s.sevkiyatId && pd.aracSira == s.aracSira))
                        .OrderBy(s => s.aracSira)
                        .ToList();

                    foreach (var veri in filteredSevkiyatVerileri)
                    {
                        string sevkTarihi = veri.aracSevkTarihi.HasValue
         ? veri.aracSevkTarihi.Value.ToString("dd.MM.yyyy HH:mm")
         : string.Empty;

                        AddSevkiyatSatiri(veri.aracSira, sevkTarihi, veri.sevkId, veri.paketAdi);
                        int row = tableLayoutPanel1.RowCount - 1;

                        var txtTasimaBilgileri = GetTextBoxAt(row, 5);
                        var txtSatisSipNo = GetTextBoxAt(row, 6);
                        var txtIrsaliyeNo = GetTextBoxAt(row, 7);
                        var txtAgirlik = GetTextBoxAt(row, 8);
                        var txtFaturaToplami = GetTextBoxAt(row, 9);
                        var txtFaturaNo = GetTextBoxAt(row, 10);

                        if (txtTasimaBilgileri != null) txtTasimaBilgileri.Text = veri.tasimaBilgileri;
                        if (txtSatisSipNo != null) txtSatisSipNo.Text = veri.satisSiparisNo;
                        if (txtIrsaliyeNo != null) txtIrsaliyeNo.Text = veri.irsaliyeNo;
                        if (txtAgirlik != null) txtAgirlik.Text = veri.agirlik.ToString("N2");
                        if (txtFaturaToplami != null)
                        {
                            txtFaturaToplami.Text = veri.faturaToplami.HasValue ? veri.faturaToplami.Value.ToString("N2") : string.Empty;
                        }
                        if (txtFaturaNo != null) txtFaturaNo.Text = veri.faturaNo;
                    }
                    AddSpacerRow();
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }
        private void AddHeaderRow()
        {
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));

            string[] headers = {
        "",
        "Araç No",
        "Araç Sevk Tarihi",
        "Sevk Id",
        "Paket",
        "Taşıma Bilgileri",
        "Satış Sipariş No",
        "İrsaliye No",
        "Ağırlık",
        "Fatura Toplamı",
        "Fatura No"
    };

            tableLayoutPanel1.ColumnCount = headers.Length;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
            for (int i = 1; i < headers.Length; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (100f / (headers.Length - 1))));
            }

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

        private void AddSevkiyatSatiri(int aracNo, string aracSevkTarihiValue = "", string sevkIdValue = "", string paketAdiValue = "")
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
                pbDelete.Tag = (sevkIdValue, aracNo);
                tableLayoutPanel1.Controls.Add(pbDelete, 0, newRowIndex);

                var lblAracNo = new Label
                {
                    Text = (aracNo > 0) ? $"{aracNo}. Araç" : "",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(2),
                    AutoSize = false,
                    Height = 30,
                    Font = new Font("Segoe UI", 10),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.LightCyan
                };
                tableLayoutPanel1.Controls.Add(lblAracNo, 1, newRowIndex);
                lblAracNo.Tag = aracNo;

                var txtAracSevkTarihi = new TextBox
                {
                    Text = string.IsNullOrEmpty(aracSevkTarihiValue) ? DateTime.Now.ToString("dd.MM.yyyy HH:mm") : aracSevkTarihiValue,
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(2),
                    AutoSize = false,
                    Height = 30,
                    Font = new Font("Segoe UI", 10),
                    BorderStyle = BorderStyle.FixedSingle
                };
                tableLayoutPanel1.Controls.Add(txtAracSevkTarihi, 2, newRowIndex);

                var txtSevkId = new TextBox
                {
                    Text = sevkIdValue,
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(2),
                    AutoSize = false,
                    Height = 30,
                    Font = new Font("Segoe UI", 10),
                    BorderStyle = BorderStyle.FixedSingle
                };
                tableLayoutPanel1.Controls.Add(txtSevkId, 3, newRowIndex);

                var txtPaketAdi = new TextBox
                {
                    Text = paketAdiValue,
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Center,
                    Margin = new Padding(2),
                    AutoSize = false,
                    Height = 30,
                    Font = new Font("Segoe UI", 10),
                    BorderStyle = BorderStyle.FixedSingle
                };
                txtPaketAdi.Leave += txtPaketAdi_Leave;
                tableLayoutPanel1.Controls.Add(txtPaketAdi, 4, newRowIndex);

                for (int i = 5; i < 11; i++)
                {
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

                    if (i == 8 || i == 9)
                    {
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
                    if (i == 9)
                    {
                        txt.TextChanged += (s, args) =>
                        {
                            if (decimal.TryParse(txt.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal fatura))
                            {
                                decimal toplamFatura = 0;
                                for (int r = 1; r < tableLayoutPanel1.RowCount; r++)
                                {
                                    var otherTxt = GetTextBoxAt(r, 9);
                                    if (otherTxt != null && decimal.TryParse(otherTxt.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal f))
                                    {
                                        toplamFatura += f;
                                    }
                                }
                                if (ParseToplamBedel(out decimal toplamBedel))
                                {
                                    decimal kalan = toplamBedel - toplamFatura;
                                    if (kalan < 0)
                                    {
                                        MessageBox.Show("Fatura toplamı toplam bedeli aşıyor. Lütfen geçerli bir değer giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        txt.Text = ""; 
                                        return;
                                    }
                                    txtKalanTutar.Text = kalan.ToString("F2", CultureInfo.CurrentCulture);
                                }
                            }
                        };
                    }
                    tableLayoutPanel1.Controls.Add(txt, i, newRowIndex);
                }
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

                var tag = pictureBox.Tag as (string sevkId, int aracSira)?;
                if (tag == null)
                {
                    MessageBox.Show("Silme işlemi başarısız: Satır bilgisi alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeAra.Text.Trim();
                if (!projeId.HasValue)
                {
                    MessageBox.Show("Proje ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var mevcutSevkiyat = _sevkiyatService.GetSevkiyatByProje(projeId.Value)
                    .FirstOrDefault(s => s.sevkId == tag.Value.sevkId && s.aracSira == tag.Value.aracSira);

                tableLayoutPanel1.SuspendLayout();
                try
                {
                    if (mevcutSevkiyat != null && mevcutSevkiyat.sevkiyatId > 0) 
                    {
                        _pendingDeletions.Add((projeId.Value, mevcutSevkiyat.sevkiyatId, tag.Value.aracSira));
                        MessageBox.Show("Satır başarıyla silinmek üzere işaretlendi. Kaydet butonuna tıklayarak veritabanından silmeyi onaylayın.", "Silme Onayı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Yeni eklenen satır başarıyla kaldırıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadSevkiyatlar(projeNo);
                }
                finally
                {
                    tableLayoutPanel1.ResumeLayout(true);
                }
            }
        }
        private void txtPaketAdi_Leave(object sender, EventArgs e)
        {
            TextBox txtPaketAdi = sender as TextBox;
            if (txtPaketAdi != null && !string.IsNullOrWhiteSpace(txtPaketAdi.Text))
            {
                string enteredPaketAdi = txtPaketAdi.Text.Trim();

                int paketId = _sevkiyatPaketleriService.GetPaketIdByAdi(enteredPaketAdi);
                if (paketId == 0)
                {
                    MessageBox.Show($"'{enteredPaketAdi}' adında bir paket bulunamadı. Lütfen önce paketi tanımlayın veya doğru bir paket adı girin.", "Paket Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPaketAdi.Focus();
                    return;
                }

                int currentRow = tableLayoutPanel1.GetRow(txtPaketAdi);
                Label lblAracNo = GetLabelAt(currentRow, 1); 
                TextBox txtSevkId = GetTextBoxAt(currentRow, 3); 

                if (lblAracNo != null && lblAracNo.Tag is int currentAracSira)
                {
                    for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                    {
                        if (row == currentRow) continue;

                        Label otherLblAracNo = GetLabelAt(row, 1);
                        TextBox otherTxtPaketAdi = GetTextBoxAt(row, 4);
                        TextBox otherTxtSevkId = GetTextBoxAt(row, 3);

                        if (otherLblAracNo != null && otherLblAracNo.Tag is int otherAracSira &&
                            otherTxtPaketAdi != null && !string.IsNullOrWhiteSpace(otherTxtPaketAdi.Text) &&
                            otherTxtSevkId != null && !string.IsNullOrWhiteSpace(otherTxtSevkId.Text))
                        {
                            if (currentAracSira == otherAracSira &&
                                enteredPaketAdi.Equals(otherTxtPaketAdi.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                txtSevkId.Text.Trim().Equals(otherTxtSevkId.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Bu araca aynı paket ve sevk ID daha önce yüklenmiştir.", "Tekrar Eden Paket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtPaketAdi.Text = "";
                                txtPaketAdi.Focus();
                                return;
                            }
                        }
                    }
                }
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
            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeAra.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' için projeId bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(projeId.Value);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Aranan proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProjeAra.Text = null;
                return;
            }

            var projeKutuk = _projeKutukService.ProjeKutukAra(projeId.Value);
            if (projeKutuk != null && projeKutuk.altProjeVarMi)
            {
                MessageBox.Show($"Proje '{projeNo}' alt projelere sahip. Ana proje için sevkiyat yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ParseToplamBedel(out decimal toplamBedel))
            {
                MessageBox.Show("Toplam bedel geçerli bir sayı değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal toplamFatura = 0;
            bool tumSatirlarDolu = true;
            List<string> nedenTamamlanmadiList = new List<string>();

            for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
            {
                var lblAracNo = GetLabelAt(row, 1);
                if (lblAracNo == null || string.IsNullOrWhiteSpace(lblAracNo.Text))
                {
                    continue;
                }

                var txtFaturaToplami = GetTextBoxAt(row, 9);
                if (txtFaturaToplami != null && !string.IsNullOrWhiteSpace(txtFaturaToplami.Text))
                {
                    if (!decimal.TryParse(txtFaturaToplami.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal fatura))
                    {
                        MessageBox.Show($"Satır {row}: Fatura Toplamı geçerli bir sayı değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    toplamFatura += fatura;
                }
                else
                {
                    tumSatirlarDolu = false;
                }
            }

            if (toplamFatura > toplamBedel)
            {
                MessageBox.Show($"Fatura toplamları ({toplamFatura:F2}) toplam bedeli ({toplamBedel:F2}) aşıyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                foreach (var itemToDelete in _pendingDeletions)
                {
                    if (!_sevkiyatService.SevkiyatSilBySevkiyatId(projeId.Value, itemToDelete.sevkiyatId, itemToDelete.aracSira))
                    {
                        throw new Exception($"Silme işlemi başarısız: {itemToDelete.sevkiyatId}");
                    }
                }

                var mevcutSevkiyatlar = _sevkiyatService.GetSevkiyatByProje(projeId.Value);

                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    var lblAracNo = GetLabelAt(row, 1);
                    if (lblAracNo == null || string.IsNullOrWhiteSpace(lblAracNo.Text))
                    {
                        continue;
                    }

                    var txtSevkId = GetTextBoxAt(row, 3);
                    var txtPaketAdi = GetTextBoxAt(row, 4);
                    var txtAracSevkTarihi = GetTextBoxAt(row, 2);
                    var txtTasimaBilgileri = GetTextBoxAt(row, 5);
                    var txtSatisSipNo = GetTextBoxAt(row, 6);
                    var txtIrsaliyeNo = GetTextBoxAt(row, 7);
                    var txtAgirlik = GetTextBoxAt(row, 8);
                    var txtFaturaToplami = GetTextBoxAt(row, 9);
                    var txtFaturaNo = GetTextBoxAt(row, 10);

                    bool tumAlanlarDolu = !string.IsNullOrWhiteSpace(txtSevkId?.Text) &&
                                          !string.IsNullOrWhiteSpace(txtPaketAdi?.Text) &&
                                          !string.IsNullOrWhiteSpace(txtAracSevkTarihi?.Text) &&
                                          !string.IsNullOrWhiteSpace(txtTasimaBilgileri?.Text) &&
                                          !string.IsNullOrWhiteSpace(txtSatisSipNo?.Text) &&
                                          !string.IsNullOrWhiteSpace(txtIrsaliyeNo?.Text) &&
                                          !string.IsNullOrWhiteSpace(txtAgirlik?.Text) &&
                                          !string.IsNullOrWhiteSpace(txtFaturaToplami?.Text) &&
                                          !string.IsNullOrWhiteSpace(txtFaturaNo?.Text);

                    decimal kalanTutar = toplamBedel - toplamFatura;
                    string statu = (tumAlanlarDolu && kalanTutar == 0) ? "Kapatıldı" : "Başlatıldı";

                    if (!tumAlanlarDolu)
                    {
                        nedenTamamlanmadiList.Add($"Projenin {row}. satırına bazı bilgiler girilmedi");
                        tumSatirlarDolu = false;
                    }

                    int aracSira = 0;
                    if (lblAracNo != null && lblAracNo.Tag is int)
                    {
                        aracSira = (int)lblAracNo.Tag;
                    }
                    else if (lblAracNo != null && lblAracNo.Text.Contains("."))
                    {
                        int.TryParse(lblAracNo.Text.Split('.')[0], out aracSira);
                    }

                    string sevkId = txtSevkId?.Text?.Trim() ?? "";
                    string paketAdi = txtPaketAdi?.Text?.Trim() ?? "";
                    string tasimaBilgileri = txtTasimaBilgileri?.Text?.Trim() ?? "";
                    string satisSiparisNo = txtSatisSipNo?.Text?.Trim() ?? "";
                    string irsaliyeNo = txtIrsaliyeNo?.Text?.Trim() ?? "";
                    DateTime? aracSevkTarihi = null;
                    if (!string.IsNullOrWhiteSpace(txtAracSevkTarihi?.Text))
                    {
                        if (DateTime.TryParseExact(txtAracSevkTarihi.Text.Trim(),
                                                   "dd.MM.yyyy HH:mm",
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None,
                                                   out DateTime parsedDate))
                        {
                            aracSevkTarihi = parsedDate;
                        }
                        else
                        {
                            MessageBox.Show($"Satır {row}: Araç Sevk Tarihi geçerli formatta değil (dd.MM.yyyy HH:mm).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    decimal agirlik = decimal.TryParse(txtAgirlik?.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal a) ? a : 0;

                    decimal? faturaToplami = null;
                    if (!string.IsNullOrWhiteSpace(txtFaturaToplami?.Text))
                    {
                        if (decimal.TryParse(txtFaturaToplami.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal ft))
                        {
                            faturaToplami = ft;
                        }
                    }

                    string faturaNo = txtFaturaNo?.Text?.Trim() ?? "";
                    int paketId = _sevkiyatPaketleriService.GetPaketIdByAdi(paketAdi);
                    if (paketId == 0 && !string.IsNullOrWhiteSpace(paketAdi))
                    {
                        MessageBox.Show($"Satır {row}: Paket '{paketAdi}' için ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var sevkiyat = new Sevkiyat
                    {
                        projeId = projeId.Value,
                        sevkId = sevkId,
                        paketId = paketId,
                        tasimaBilgileri = tasimaBilgileri,
                        satisSiparisNo = satisSiparisNo,
                        irsaliyeNo = irsaliyeNo,
                        aracSevkTarihi = aracSevkTarihi,
                        agirlik = agirlik,
                        faturaToplami = faturaToplami,
                        faturaNo = faturaNo,
                        aracSira = aracSira,
                        status = statu,
                        paketAdi = paketAdi
                    };

                    var mevcutSevkiyat = mevcutSevkiyatlar.FirstOrDefault(s =>
                        s.projeId == projeId.Value &&
                        s.aracSira == aracSira &&
                        s.paketId == paketId &&
                        s.sevkId == sevkId);

                    if (mevcutSevkiyat != null && mevcutSevkiyat.sevkiyatId > 0) 
                    {
                        sevkiyat.sevkiyatId = mevcutSevkiyat.sevkiyatId; 
                        _sevkiyatService.SevkiyatGuncelle(sevkiyat);
                    }
                    else
                    {
                        _sevkiyatService.SevkiyatKaydet(sevkiyat);
                    }
                }

                decimal kalanTutarFinal = toplamBedel - toplamFatura;
                string sayfaStatus = (tumSatirlarDolu && (toplamBedel - toplamFatura) == 0) ? "Kapatıldı" : "Başlatıldı";
                if (kalanTutarFinal > 0)
                {
                    nedenTamamlanmadiList.Add($"Projenin toplam bedeli kadar fatura kesilmemiş");
                }
                txtStatus.Text = sayfaStatus;

                var mevcutSayfaStatus = _sayfaStatusService.Get(sayfaId: sayfaIdSevkiyat, projeId.Value);
                if (mevcutSayfaStatus != null)
                {
                    mevcutSayfaStatus.status = sayfaStatus;
                    mevcutSayfaStatus.bilgilerTamamMi = tumSatirlarDolu;
                    mevcutSayfaStatus.nedenTamamlanmadi = string.Join("; ", nedenTamamlanmadiList);
                    _sayfaStatusService.Update(mevcutSayfaStatus);
                }
                else
                {
                    var yeniStatus = new SayfaStatus
                    {
                        sayfaId = sayfaIdSevkiyat,
                        projeId = projeId.Value,
                        status = sayfaStatus,
                        bilgilerTamamMi = tumSatirlarDolu,
                        nedenTamamlanmadi = string.Join("; ", nedenTamamlanmadiList)
                    };
                    _sayfaStatusService.Insert(yeniStatus);
                }
                _projeKutukService.UpdateProjeKutukDurum(projeId.Value, null);

                _pendingDeletions.Clear();
                LoadSevkiyatlar(projeNo);
                txtKalanTutar.Text = kalanTutarFinal.ToString("F2", CultureInfo.CurrentCulture);
                OnSevkiyatKaydedildi?.Invoke(projeNo);
                MessageBox.Show("Sevkiyat bilgileri kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sevkiyat bilgileri kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }
        private void btnProjeAra_Click(object sender, EventArgs e)
        {
            string arananProjeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeAra.Text.Trim();
            if (string.IsNullOrEmpty(arananProjeNo))
            {
                MessageBox.Show("Lütfen bir proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateButtonsState();
                return;
            }

            projeId = _finansProjelerService.GetProjeIdByNo(arananProjeNo);
            if (!projeId.HasValue)
            {
                MessageBox.Show($"Aranan proje '{arananProjeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProjeAra.Text = null;
                UpdateButtonsState();
                return;
            }
            var sayfaStatusEntity = _sayfaStatusService.Get(sayfaIdSevkiyat, projeId.Value);
            txtStatus.Text = sayfaStatusEntity?.status ?? "Başlatıldı";

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(projeId.Value);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Aranan proje '{arananProjeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProjeAra.Text = null;
                UpdateButtonsState();
                return;
            }

            var projeKutuk = _projeKutukService.ProjeKutukAra(projeId.Value);
            var arananProjeId = _finansProjelerService.GetProjeIdByNo(arananProjeNo);
            if (projeKutuk != null && projeKutuk.altProjeVarMi)
            {
                MessageBox.Show($"Proje '{arananProjeNo}' alt projelere sahip. Ana proje için sevkiyat yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<int> altProjeler = null;
            var (toplamBedel, eksikFiyatlandirmaProjeler) = _fiyatlandirmaService.GetToplamBedel(arananProjeId.Value, altProjeler);

            var sevkiyatlar = _sevkiyatService.GetSevkiyatByProje(projeId.Value);

            decimal toplamFatura = sevkiyatlar.Sum(s => s.faturaToplami.GetValueOrDefault());
            decimal kalanTutar = toplamBedel - toplamFatura;

            UpdateToplamBedelUI(arananProjeNo, toplamBedel, eksikFiyatlandirmaProjeler, kalanTutar);
            _pendingDeletions.Clear();
            LoadSevkiyatlar(arananProjeNo);
            UpdateButtonsState();
        }
        private void UpdateToplamBedelUI(string projeNo, decimal toplamBedel, List<int> eksikFiyatlandirmaProjeler, decimal kalanTutar)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler, kalanTutar)));
                return;
            }

            txtToplamBedel.Text = toplamBedel.ToString("F2", CultureInfo.CurrentCulture);
            txtKalanTutar.Text = kalanTutar.ToString("F2", CultureInfo.CurrentCulture);

            var projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            var projeKutuk = _projeKutukService.ProjeKutukAra(projeId.Value);

            bool isAltProje = _projeIliskiService.CheckAltProje(projeId.Value);

            if (eksikFiyatlandirmaProjeler.Any())
            {
                var eksikProjeNumaralari = eksikFiyatlandirmaProjeler
                    .Select(id => _finansProjelerService.GetProjeNoById(id))
                    .Where(no => !string.IsNullOrEmpty(no))
                    .ToList();

                txtToplamBedel.ForeColor = Color.Red;
                lblToplamBedelBilgi.Text = isAltProje
                    ? $"Alt projeler: {string.Join(", ", eksikProjeNumaralari)} için fiyatlandırma bulunmamaktadır."
                    : $"Proje: {string.Join(", ", eksikProjeNumaralari)} için fiyatlandırma bulunmamaktadır.";
                lblToplamBedelBilgi.ForeColor = Color.Red;
            }
            else
            {
                txtToplamBedel.ForeColor = Color.Black;
                lblToplamBedelBilgi.Text = string.Empty;
                lblToplamBedelBilgi.ForeColor = Color.Black;
            }

            txtKalanTutar.Refresh();
            txtToplamBedel.Refresh();
            lblToplamBedelBilgi.Refresh();
        }
        private void btnPaketEkle_Click(object sender, EventArgs e)
        {
            var frm = new frmYeniPaketEkle(_sevkiyatPaketleriService);
            frm.ShowDialog();
        }

        private void btnSevkiyatEkle_Click(object sender, EventArgs e)
        {
            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeAra.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Lütfen önce bir proje seçin veya girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (decimal.TryParse(txtKalanTutar.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal kalanTutar) && kalanTutar == 0)
            {
                MessageBox.Show("Kalan tutar bilgisi 0, sevkiyat eklenemez", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' için projeId bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ParseToplamBedel(out decimal toplamBedel) || toplamBedel == 0)
            {
                MessageBox.Show("Projeye ait fiyatlandırma bulunmadığından sevkiyat bilgisi girilemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(projeId.Value);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Geçerli proje '{projeNo}' bulunamadı. Sevkiyat eklenemiyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var projeKutuk = _projeKutukService.ProjeKutukAra(projeId.Value);
            if (projeKutuk != null && projeKutuk.altProjeVarMi)
            {
                MessageBox.Show($"Proje '{projeNo}' alt projelere sahip. Ana proje için sevkiyat eklenemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sevkiyatAracSayisi++;
            string currentDateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            DialogResult result = MessageBox.Show(
                "Araca birden fazla paket eklenecek mi?",
                "Paket Ekleme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                AddSevkiyatSatiri(sevkiyatAracSayisi, currentDateTime);
                AddSevkiyatSatiri(sevkiyatAracSayisi, currentDateTime);
            }
            else
            {
                AddSevkiyatSatiri(sevkiyatAracSayisi, currentDateTime);
            }

            AddSpacerRow();
        }
        private void UpdateButtonsState()
        {
            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeAra.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                btnPaketEkle.Enabled = false;
                btnSevkiyatEkle.Enabled = false;
                return;
            }

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(projeId.HasValue ? projeId.Value : _finansProjelerService.GetProjeIdByNo(projeNo).GetValueOrDefault());
            bool isProjeValid = projeBilgi != null;

            btnPaketEkle.Enabled = isProjeValid;
            btnSevkiyatEkle.Enabled = isProjeValid;
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            string projeNo = cmbProjeNo.Visible ? cmbProjeNo.SelectedItem?.ToString() : txtProjeAra.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Lütfen bir proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' için projeId bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(projeId.Value);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show($"Proje '{projeNo}' için tüm sevkiyat kayıtları silinecek. Onaylıyor musunuz?", "Sevkiyat Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            if (_sevkiyatService.SevkiyatSil(projeId.Value))
            {
                var mevcutSayfaStatus = _sayfaStatusService.Get(sayfaId: sayfaIdSevkiyat, projeId.Value);
                if (mevcutSayfaStatus != null)
                {
                    mevcutSayfaStatus.status = "Başlatıldı";
                    mevcutSayfaStatus.bilgilerTamamMi = false;
                    mevcutSayfaStatus.nedenTamamlanmadi = "Sevkiyat bilgileri girilmemiş.";
                    _sayfaStatusService.Update(mevcutSayfaStatus);
                }
                else
                {
                    var yeniStatus = new SayfaStatus
                    {
                        sayfaId = sayfaIdSevkiyat,
                        projeId = projeId.Value,
                        status = "Başlatıldı",
                        bilgilerTamamMi = false,
                        nedenTamamlanmadi = "Sevkiyat bilgileri girilmemiş."
                    };
                    _sayfaStatusService.Insert(yeniStatus);
                }

                txtStatus.Text = "Başlatıldı";
                MessageBox.Show("Sevkiyat kayıtları başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                InitializeTableStructure();
                UpdateButtonsState();
            }
            else
            {
                MessageBox.Show("Sevkiyat silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnProjeAra_Click(sender, e);
        }
        private void txtProjeAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.btnAra.PerformClick();
            }
        }
        private bool ParseToplamBedel(out decimal toplamBedel)
        {
            toplamBedel = 0;
            if (string.IsNullOrWhiteSpace(txtToplamBedel.Text))
                return false;

            string cleanText = txtToplamBedel.Text.Split('(')[0].Trim();

            return decimal.TryParse(cleanText, NumberStyles.Any, new CultureInfo("tr-TR"), out toplamBedel);
        }
    }
}