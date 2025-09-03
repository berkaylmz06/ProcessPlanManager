using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Forms;
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
        private ProjeFinans_SevkiyatData sevkiyatData = new ProjeFinans_SevkiyatData();
        private ProjeFinans_SevkiyatPaketleriData sevkiyatPaketleriData = new ProjeFinans_SevkiyatPaketleriData();

        public event Action<string> OnSevkiyatKaydedildi;

        private ComboBox cmbProjeNo;
        private List<string> altProjeler;
        private int sevkiyatAracSayisi = 0; 

        public ctlSevkiyat()
        {
            InitializeComponent();

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

        public void LoadProjeSevkiyat(string projeNo, bool autoSearch = false, List<string> altProjeler = null)
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

            txtProjeAra.Text = projeNo;

            if (altProjeler != null && altProjeler.Any())
            {
                txtProjeAra.Visible = false;
                cmbProjeNo.Visible = true;
                cmbProjeNo.Items.Clear();
                cmbProjeNo.Items.AddRange(altProjeler.ToArray());
                cmbProjeNo.SelectedIndex = altProjeler.Contains(projeNo) ? altProjeler.IndexOf(projeNo) : 0;
                cmbProjeNo.BringToFront();
            }
            else
            {
                txtProjeAra.Visible = true;
                cmbProjeNo.Visible = false;
            }

            if (autoSearch)
            {
                LoadSevkiyatlar(projeNo);
            }
            UpdateButtonsState();
        }

        private void LoadSevkiyatlar(string projeNo)
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnCount = 10;
                tableLayoutPanel1.RowCount = 1;
                tableLayoutPanel1.RowStyles.Clear();

                tableLayoutPanel1.ColumnStyles.Clear();
                for (int i = 0; i < 10; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 10));
                }

                AddHeaderRow();
                AddSpacerRow();

                var sevkiyatVerileri = sevkiyatData.GetSevkiyatByProje(projeNo);

                sevkiyatAracSayisi = sevkiyatVerileri.Any() ? sevkiyatVerileri.Max(s => s.aracSira) : 0;

                if (sevkiyatVerileri.Any())
                {
                    foreach (var veri in sevkiyatVerileri)
                    {
                        AddSevkiyatSatiri(veri.aracSira, veri.aracSevkTarihi.ToString("dd.MM.yyyy HH:mm"), veri.sevkId, veri.paketAdi);
                        int row = tableLayoutPanel1.RowCount - 1;

                        var txtTasimaBilgileri = GetTextBoxAt(row, 4);
                        var txtSatisSipNo = GetTextBoxAt(row, 5);
                        var txtIrsaliyeNo = GetTextBoxAt(row, 6);
                        var txtAgirlik = GetTextBoxAt(row, 7);
                        var txtFaturaToplami = GetTextBoxAt(row, 8);
                        var txtFaturaNo = GetTextBoxAt(row, 9);

                        if (txtTasimaBilgileri != null) txtTasimaBilgileri.Text = veri.tasimaBilgileri;
                        if (txtSatisSipNo != null) txtSatisSipNo.Text = veri.satisSipNo;
                        if (txtIrsaliyeNo != null) txtIrsaliyeNo.Text = veri.irsaliyeNo;
                        if (txtAgirlik != null) txtAgirlik.Text = veri.agirlik.ToString("N2");
                        if (txtFaturaToplami != null) txtFaturaToplami.Text = veri.faturaToplami.ToString("N2");
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
                tableLayoutPanel1.Controls.Add(lblAracNo, 0, newRowIndex);
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
                tableLayoutPanel1.Controls.Add(txtAracSevkTarihi, 1, newRowIndex);

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
                tableLayoutPanel1.Controls.Add(txtSevkId, 2, newRowIndex);

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
                tableLayoutPanel1.Controls.Add(txtPaketAdi, 3, newRowIndex);

                for (int i = 4; i < 10; i++)
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

                    if (i == 7 || i == 8)
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

                    tableLayoutPanel1.Controls.Add(txt, i, newRowIndex);
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private void txtPaketAdi_Leave(object sender, EventArgs e)
        {
            TextBox txtPaketAdi = sender as TextBox;
            if (txtPaketAdi != null && !string.IsNullOrWhiteSpace(txtPaketAdi.Text))
            {
                string enteredPaketAdi = txtPaketAdi.Text.Trim();

                int paketId = sevkiyatPaketleriData.GetPaketIdByAdi(enteredPaketAdi);
                if (paketId == 0)
                {
                    MessageBox.Show($"'{enteredPaketAdi}' adında bir paket bulunamadı. Lütfen önce paketi tanımlayın veya doğru bir paket adı girin.", "Paket Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPaketAdi.Focus();
                    return;
                }

                int currentRow = tableLayoutPanel1.GetRow(txtPaketAdi);
                Label lblAracNo = GetLabelAt(currentRow, 0);

                if (lblAracNo != null && lblAracNo.Tag is int currentAracSira)
                {
                    for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                    {
                        if (row == currentRow) continue;

                        Label otherLblAracNo = GetLabelAt(row, 0);
                        TextBox otherTxtPaketAdi = GetTextBoxAt(row, 3);

                        if (otherLblAracNo != null && otherLblAracNo.Tag is int otherAracSira &&
                            otherTxtPaketAdi != null && !string.IsNullOrWhiteSpace(otherTxtPaketAdi.Text))
                        {
                            if (currentAracSira == otherAracSira && enteredPaketAdi.Equals(otherTxtPaketAdi.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Bu araca aynı paket daha önce yüklenmiştir.", "Tekrar Eden Paket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Aranan proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProjeAra.Text = null;
                return;
            }

            var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
            if (projeKutuk != null && projeKutuk.altProjeVarMi)
            {
                MessageBox.Show($"Proje '{projeNo}' alt projelere sahip. Ana proje için sevkiyat yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tableLayoutPanel1.SuspendLayout();
            try
            {
                var mevcutSevkiyatlar = sevkiyatData.GetSevkiyatByProje(projeNo)
                    .ToDictionary(s => s.sevkId, s => s);

                for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
                {
                    var lblAracNo = GetLabelAt(row, 0); 
                    var txtSevkId = GetTextBoxAt(row, 2);
                    var txtPaketAdi = GetTextBoxAt(row, 3);

                    if (txtSevkId == null || string.IsNullOrEmpty(txtSevkId.Text.Trim()) ||
                        txtPaketAdi == null || string.IsNullOrEmpty(txtPaketAdi.Text.Trim()))
                    {
                        continue;
                    }

                    int aracSira = 0;
                    if (lblAracNo != null && lblAracNo.Tag is int)
                    {
                        aracSira = (int)lblAracNo.Tag; 
                    }
                    else
                    {
                        if (lblAracNo != null && lblAracNo.Text.Contains("."))
                        {
                            int.TryParse(lblAracNo.Text.Split('.')[0], out aracSira);
                        }
                    }

                    var txtAracSevkTarihi = GetTextBoxAt(row, 1);
                    var txtTasimaBilgileri = GetTextBoxAt(row, 4);
                    var txtSatisSipNo = GetTextBoxAt(row, 5);
                    var txtIrsaliyeNo = GetTextBoxAt(row, 6);
                    var txtAgirlik = GetTextBoxAt(row, 7);
                    var txtFaturaToplami = GetTextBoxAt(row, 8);
                    var txtFaturaNo = GetTextBoxAt(row, 9);

                    string sevkId = txtSevkId.Text.Trim();
                    string paketAdi = txtPaketAdi.Text.Trim();
                    string tasimaBilgileri = txtTasimaBilgileri?.Text ?? "";
                    string satisSiparisNo = txtSatisSipNo?.Text ?? "";
                    string irsaliyeNo = txtIrsaliyeNo?.Text ?? "";
                    DateTime? aracSevkTarihi = DateTime.TryParse(txtAracSevkTarihi?.Text, out DateTime date) ? date : (DateTime?)null;
                    decimal agirlik = decimal.TryParse(txtAgirlik?.Text, out decimal a) ? a : 0;
                    decimal faturaToplami = decimal.TryParse(txtFaturaToplami?.Text, out decimal ft) ? ft : 0;
                    string faturaNo = txtFaturaNo?.Text ?? "";

                    int sevkiyatKalemId = sevkiyatPaketleriData.GetPaketIdByAdi(paketAdi);
                    if (sevkiyatKalemId == 0)
                    {
                        MessageBox.Show($"Paket '{paketAdi}' için ID bulunamadı. Bu satır kaydedilemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    if (mevcutSevkiyatlar.TryGetValue(sevkId, out var mevcutSevkiyat))
                    {
                        sevkiyatData.SevkiyatGuncelle(
                            projeNo,
                            sevkId,
                            sevkiyatKalemId,
                            tasimaBilgileri,
                            satisSiparisNo,
                            irsaliyeNo,
                            aracSevkTarihi.GetValueOrDefault(DateTime.MinValue),
                            agirlik,
                            faturaToplami,
                            faturaNo,
                            aracSira
                        );
                    }
                    else
                    {
                        sevkiyatData.SevkiyatKaydet(
                            projeNo,
                            sevkId,
                            sevkiyatKalemId,
                            tasimaBilgileri,
                            satisSiparisNo,
                            irsaliyeNo,
                            aracSevkTarihi.GetValueOrDefault(DateTime.MinValue),
                            agirlik,
                            faturaToplami,
                            faturaNo,
                            aracSira
                        );
                    }
                }

                OnSevkiyatKaydedildi?.Invoke(projeNo);
                MessageBox.Show("Sevkiyat bilgileri kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(arananProjeNo);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Aranan proje '{arananProjeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProjeAra.Text = null;
                UpdateButtonsState();
                return;
            }

            var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(arananProjeNo);
            if (projeKutuk != null && projeKutuk.altProjeVarMi)
            {
                MessageBox.Show($"Proje '{arananProjeNo}' alt projelere sahip. Ana proje için sevkiyat yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
            List<string> altProjeler = null;
            var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(arananProjeNo, altProjeler);

            UpdateToplamBedelUI(arananProjeNo, toplamBedel, eksikFiyatlandirmaProjeler);

            LoadSevkiyatlar(arananProjeNo);
            UpdateButtonsState();
        }

        private void btnPaketEkle_Click(object sender, EventArgs e)
        {
            var frm = new frmYeniPaketEkle();
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
            if (!ParseToplamBedel(out decimal toplamBedel) || toplamBedel == 0)
            {
                MessageBox.Show("Projeye ait fiyatlandırma bulunmadığından sevkiyat bilgisi girilemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Geçerli proje '{projeNo}' bulunamadı. Sevkiyat eklenemiyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
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

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
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

            var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
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

            if (sevkiyatData.SevkiyatSil(projeNo))
            {
                MessageBox.Show("Sevkiyat kayıtları başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                InitializeTableStructure(); 
                UpdateButtonsState();
            }
            else
            {
                MessageBox.Show("Sevkiyat silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProjeAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.btnAra.PerformClick();
            }
        }
        private void UpdateToplamBedelUI(string projeNo, decimal toplamBedel, List<string> eksikFiyatlandirmaProjeler)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler)));
                return;
            }

            txtToplamBedel.Text = toplamBedel.ToString("F2", CultureInfo.CurrentCulture);
            var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);

            bool isAltProje = ProjeFinans_ProjeIliskiData.CheckAltProje(projeNo);

            if (eksikFiyatlandirmaProjeler.Any())
            {
                txtToplamBedel.ForeColor = Color.Red;
                lblToplamBedelBilgi.Text = isAltProje
                   ? $"Alt projeler: {string.Join(", ", eksikFiyatlandirmaProjeler)} için fiyatlandırma bulunmamaktadır."
                   : $"Proje: {string.Join(", ", eksikFiyatlandirmaProjeler)} için fiyatlandırma bulunmamaktadır.";
                lblToplamBedelBilgi.ForeColor = Color.Red;
            }
            else
            {
                txtToplamBedel.ForeColor = Color.Black;
                lblToplamBedelBilgi.Text = string.Empty;
                lblToplamBedelBilgi.ForeColor = Color.Black;
            }

            txtToplamBedel.Refresh();
            lblToplamBedelBilgi.Refresh();
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