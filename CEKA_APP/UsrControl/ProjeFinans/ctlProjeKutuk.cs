using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeKutuk : UserControl
    {
        private FlowLayoutPanel flpAltProjeTextBoxes;
        private FlowLayoutPanel flpIliskiTextBoxes;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem mnuProjeFiyatlandirma;
        private ToolStripMenuItem mnuProjeBilgileri;
        private bool projeBilgileriKaydedildi;
        
        private const int TEXTBOX_WIDTH = 130;
        private const int BUTTON_X_WIDTH = 20;
        private const int HORIZONTAL_MARGIN = 40; 
        private const int PANEL_INNER_MARGIN = 5;
        private const int MAX_PANEL_HEIGHT = 200;

        private int contentWidth;
        private int panelWidth;
        private int buttonWidth;

        public ctlProjeKutuk()
        {
            InitializeComponent();
            projeBilgileriKaydedildi = false;

            contentWidth = TEXTBOX_WIDTH + PANEL_INNER_MARGIN + BUTTON_X_WIDTH;
            panelWidth = contentWidth + 2 * HORIZONTAL_MARGIN;
            buttonWidth = contentWidth;

            contextMenu = new ContextMenuStrip();
            mnuProjeFiyatlandirma = new ToolStripMenuItem("Proje Fiyatlandırma") { Enabled = false };
            mnuProjeBilgileri = new ToolStripMenuItem("Proje Bilgileri") { Enabled = false };
            contextMenu.Items.Add(mnuProjeFiyatlandirma);
            contextMenu.Items.Add(mnuProjeBilgileri);
            this.ContextMenuStrip = contextMenu;

            mnuProjeBilgileri.Click += (s, e) =>
            {
                var parentForm = this.FindForm() as frmAnaSayfa;
                if (parentForm == null)
                {
                    MessageBox.Show("Ana form bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (parentForm.projeBilgileri == null)
                {
                    parentForm.projeBilgileri = new ctlProjeBilgileri();
                    parentForm.projeBilgileri.Dock = DockStyle.Fill;
                    parentForm.projeBilgileri.OnKaydet -= () => projeBilgileriKaydedildi = true;
                    parentForm.projeBilgileri.OnKaydet += () => projeBilgileriKaydedildi = true;
                }

                var altProjeler = chkAltProjeVar.Checked
                    ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                        .Select(txt => txt.Text.Trim())
                        .ToList()
                    : new List<string> { txtProjeNo.Text.Trim() };

                parentForm.projeBilgileri.LoadProjects(altProjeler, chkAltProjeVar.Checked ? txtProjeNo.Text.Trim() : null);
                parentForm.NavigateToUserControl(parentForm.projeBilgileri);
            };

            mnuProjeFiyatlandirma.Click += (s, e) =>
            {
                string projeNo = txtProjeNo.Text.Trim();
                if (string.IsNullOrEmpty(projeNo))
                {
                    MessageBox.Show("Lütfen bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<string> altProjeler = chkAltProjeVar.Checked
                    ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                        .Select(txt => txt.Text.Trim())
                        .ToList()
                    : null;

                var parentFormFiyat = this.FindForm() as frmAnaSayfa;
                if (parentFormFiyat == null)
                {
                    MessageBox.Show("Ana form bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (parentFormFiyat.projeFiyatlandirma == null)
                {
                    parentFormFiyat.projeFiyatlandirma = new ctlProjeFiyatlandirma();
                    parentFormFiyat.projeFiyatlandirma.Dock = DockStyle.Fill;

                    parentFormFiyat.projeFiyatlandirma.OnFiyatlandirmaKaydedildi += (projeAdi) =>
                    {
                        try
                        {
                            var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
                            var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeAdi, altProjeler);
                            UpdateToplamBedelUI(projeAdi, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"OnFiyatlandirmaKaydedildi sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };
                }

                if (chkAltProjeVar.Checked && altProjeler != null && altProjeler.Any())
                {
                    using (var form = new Form
                    {
                        Text = "Alt Proje Seç",
                        Size = new Size(300, 150),
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        MaximizeBox = false,
                        MinimizeBox = false
                    })
                    {
                        var comboBox = new ComboBox
                        {
                            Location = new Point(20, 20),
                            Width = 240,
                            DropDownStyle = ComboBoxStyle.DropDownList
                        };
                        comboBox.Items.AddRange(altProjeler.ToArray());
                        form.Controls.Add(comboBox);

                        var btnTamam = new Button
                        {
                            Text = "Tamam",
                            Location = new Point(20, 60),
                            Width = 100
                        };
                        btnTamam.Click += (s2, e2) =>
                        {
                            if (comboBox.SelectedItem != null)
                            {
                                form.Tag = comboBox.SelectedItem.ToString();
                                form.DialogResult = DialogResult.OK;
                                form.Close();
                            }
                            else
                            {
                                MessageBox.Show("Lütfen bir alt proje seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        };
                        form.Controls.Add(btnTamam);

                        var btnIptal = new Button
                        {
                            Text = "İptal",
                            Location = new Point(160, 60),
                            Width = 100
                        };
                        btnIptal.Click += (s2, e2) => form.Close();
                        form.Controls.Add(btnIptal);

                        if (form.ShowDialog() == DialogResult.OK && form.Tag != null)
                        {
                            parentFormFiyat.projeFiyatlandirma.LoadProjeFiyatlandirma(form.Tag.ToString(), autoSearch: false, altProjeler: altProjeler);
                            parentFormFiyat.NavigateToUserControl(parentFormFiyat.projeFiyatlandirma);
                            parentFormFiyat.projeFiyatlandirma.btnProjeAra_Click(null, EventArgs.Empty);
                        }
                    }
                }
                else
                {
                    parentFormFiyat.projeFiyatlandirma.LoadProjeFiyatlandirma(projeNo, autoSearch: false, altProjeler: altProjeler);
                    parentFormFiyat.NavigateToUserControl(parentFormFiyat.projeFiyatlandirma);
                    parentFormFiyat.projeFiyatlandirma.btnProjeAra_Click(null, EventArgs.Empty);
                }
            };

            flpAltProjeTextBoxes = new FlowLayoutPanel
            {
                AutoSize = false,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                AutoScrollMinSize = new Size(0, 0),
                Height = 150,
                Width = panelWidth,
                Visible = false,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(PANEL_INNER_MARGIN)
            };
            flpIliskiTextBoxes = new FlowLayoutPanel
            {
                AutoSize = false,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                AutoScrollMinSize = new Size(0, 0),
                Height = 150,
                Width = panelWidth,
                Visible = false,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(PANEL_INNER_MARGIN)
            };

            flpAltProjeTextBoxes.Location = new Point(txtProjeNo.Location.X + txtProjeNo.Width + 10, txtProjeNo.Location.Y + txtProjeNo.Height - 10);
            flpIliskiTextBoxes.Location = new Point(txtProjeNo.Location.X + txtProjeNo.Width + 10, txtProjeNo.Location.Y + txtProjeNo.Height - 10);

            var lblAltProjeler = new Label
            {
                Text = "Alt Projeler",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding((flpAltProjeTextBoxes.ClientSize.Width - 80) / 2, 0, 0, 5),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            flpAltProjeTextBoxes.Controls.Add(lblAltProjeler);

            var lblIliskiProjeler = new Label
            {
                Text = "İlişkili Projeler",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding((flpIliskiTextBoxes.ClientSize.Width - 100) / 2, 0, 0, 5),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            flpIliskiTextBoxes.Controls.Add(lblIliskiProjeler);

            AddEkleButton(flpAltProjeTextBoxes, "AltProje");
            AddEkleButton(flpIliskiTextBoxes, "Iliski");

            this.Controls.Add(flpAltProjeTextBoxes);
            this.Controls.Add(flpIliskiTextBoxes);

            flpAltProjeTextBoxes.ClientSizeChanged += Flp_ClientSizeChanged;
            flpIliskiTextBoxes.ClientSizeChanged += Flp_ClientSizeChanged;

            chkAltProjeVar.CheckedChanged += (s, e) =>
            {
                CheckBoxKontrol(chkAltProjeVar, chkAltProjeYok, flpAltProjeTextBoxes, "AltProje");
            };
            chkAltProjeYok.CheckedChanged += (s, e) =>
            {
                CheckBoxKontrol(chkAltProjeYok, chkAltProjeVar, flpAltProjeTextBoxes, "AltProje", false);
            };
            chkProjeIliskisiVar.CheckedChanged += (s, e) =>
            {
                CheckBoxKontrol(chkProjeIliskisiVar, chkProjeIliskisiYok, flpIliskiTextBoxes, "Iliski");
            };
            chkProjeIliskisiYok.CheckedChanged += (s, e) =>
            {
                CheckBoxKontrol(chkProjeIliskisiYok, chkProjeIliskisiVar, flpIliskiTextBoxes, "Iliski", false);
            };
            chkNakliyeVar.CheckedChanged += (s, e) =>
            {
                if (chkNakliyeVar.Checked) chkNakliyeYok.Checked = false;
            };
            chkNakliyeYok.CheckedChanged += (s, e) =>
            {
                if (chkNakliyeYok.Checked) chkNakliyeVar.Checked = false;
            };
            chkTekil.CheckedChanged += (s, e) =>
            {
                if (chkTekil.Checked) chkCogul.Checked = false;
            };
            chkCogul.CheckedChanged += (s, e) =>
            {
                if (chkCogul.Checked) chkTekil.Checked = false;
            };

            txtProjeNo.TextChanged += (s, e) => UpdateContextMenu();
        }

        private void Flp_ClientSizeChanged(object sender, EventArgs e)
        {
            var flp = sender as FlowLayoutPanel;
            if (flp != null)
            {
                UpdateAllPanelWidths(flp);
            }
        }

        private void UpdateAllPanelWidths(FlowLayoutPanel flp)
        {
            int horizontalMargin = (flp.ClientSize.Width - contentWidth) / 2;
            int panelMargin = horizontalMargin > 0 ? horizontalMargin : 0;

            foreach (var panel in flp.Controls.OfType<Panel>())
            {
                panel.Margin = new Padding(panelMargin, 5, panelMargin, 5);
                panel.Width = contentWidth;
            }

            var btnEkle = flp.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "Ekle");
            if (btnEkle != null)
            {
                btnEkle.Margin = new Padding(panelMargin, 5, panelMargin, 5);
                btnEkle.Width = contentWidth;
            }

            var lbl = flp.Controls.OfType<Label>().FirstOrDefault();
            if (lbl != null)
            {
                lbl.Margin = new Padding((flp.ClientSize.Width - lbl.Width) / 2, 0, 0, 5);
            }
        }

        public void UpdateToplamBedelUI(string projeNo, decimal toplamBedel, List<string> eksikFiyatlandirmaProjeler, List<string> altProjeler)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
                }));
            }
            else
            {
                txtToplamBedel.Text = toplamBedel.ToString("F2");

                if (altProjeler != null && altProjeler.Any())
                {
                    if (eksikFiyatlandirmaProjeler != null && eksikFiyatlandirmaProjeler.Any())
                    {
                        lblAltProjeHata.Text = $"Alt projeler: {string.Join(", ", eksikFiyatlandirmaProjeler)} için fiyatlandırma yok";
                        lblAltProjeHata.ForeColor = Color.Red;
                        lblAltProjeHata.Visible = true;
                    }
                    else
                    {
                        lblAltProjeHata.Text = "";
                        lblAltProjeHata.Visible = false;
                    }
                }
                else
                {
                    if (eksikFiyatlandirmaProjeler != null && eksikFiyatlandirmaProjeler.Contains(projeNo))
                    {
                        lblAltProjeHata.Text = $"{projeNo} için fiyatlandırma bilgisi yok.";
                        lblAltProjeHata.ForeColor = Color.Red;
                        lblAltProjeHata.Visible = true;
                    }
                    else
                    {
                        lblAltProjeHata.Text = "";
                        lblAltProjeHata.Visible = false;
                    }
                }
            }
        }

        private void CheckBoxKontrol(CheckBox aktif, CheckBox pasif, FlowLayoutPanel flp, string prefix, bool goster = true)
        {
            if (aktif.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtProjeNo.Text.Trim()))
                {
                    aktif.Checked = false;
                    MessageBox.Show("Lütfen önce proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (aktif.Name == "chkAltProjeVar")
                {
                    string projeNo = txtProjeNo.Text.Trim();
                    if (!Regex.IsMatch(projeNo, @"^\d{5}$|^\d{5}\.00$"))
                    {
                        aktif.Checked = false;
                        MessageBox.Show("Proje numarası 5 basamaklı bir sayı formatında olmalıdır (örneğin: 12345 veya 12345.00).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                pasif.Checked = false;
                flp.Controls.Clear();
                flp.Controls.Add(aktif.Name == "chkAltProjeVar"
                    ? new Label { Text = "Alt Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.ClientSize.Width - 80) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) }
                    : new Label { Text = "İlişkili Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.ClientSize.Width - 100) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) });

                int index = 1;
                AddTextBoxWithButton(flp, prefix, index, true);

                AddEkleButton(flp, prefix);
                var btnEkle = flp.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "Ekle");
                if (btnEkle != null)
                {
                    flp.Controls.SetChildIndex(btnEkle, flp.Controls.Count - 1);
                }

                flp.Visible = true;
                flp.BringToFront();
                flp.Controls.OfType<Panel>().FirstOrDefault()?.Controls.OfType<TextBox>().FirstOrDefault()?.Focus();
                flp.Controls.OfType<Panel>().FirstOrDefault()?.Controls.OfType<TextBox>().FirstOrDefault()?.Select();
            }
            else
            {
                flp.Controls.Clear();
                flp.Controls.Add(aktif.Name == "chkAltProjeVar"
                    ? new Label { Text = "Alt Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.ClientSize.Width - 80) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) }
                    : new Label { Text = "İlişkili Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.ClientSize.Width - 100) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) });
                AddEkleButton(flp, prefix);
                flp.Visible = false;
            }
            UpdatePanelLayout();
            UpdateContextMenu();
            UpdateFlowLayoutPanelHeight(flp);
        }

        private void UpdateFlowLayoutPanelHeight(FlowLayoutPanel flp)
        {
            int totalHeight = flp.Padding.Vertical;
            foreach (Control control in flp.Controls)
            {
                if (control.Visible)
                {
                    totalHeight += control.Height + control.Margin.Vertical;
                }
            }

            if (totalHeight > MAX_PANEL_HEIGHT)
            {
                flp.Height = MAX_PANEL_HEIGHT;
                flp.AutoScroll = true;
            }
            else
            {
                flp.Height = totalHeight;
                flp.AutoScroll = false;
            }
        }

        private void AddEkleButton(FlowLayoutPanel flp, string prefix)
        {
            int horizontalMargin = (flp.ClientSize.Width - contentWidth) / 2;
            int buttonMargin = horizontalMargin > 0 ? horizontalMargin : 0;

            var btnEkle = new Button
            {
                Text = "Ekle",
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Width = buttonWidth,
                Height = 25,
                Margin = new Padding(buttonMargin, 5, buttonMargin, 5),
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            btnEkle.Click += (s, e) =>
            {
                int index = flp.Controls.OfType<Panel>().Count() + 1;
                AddTextBoxWithButton(flp, prefix, index, true);
                var btn = (s as Button);
                if (btn != null)
                {
                    flp.Controls.SetChildIndex(btn, flp.Controls.Count - 1);
                }
                flp.Controls.OfType<Panel>().LastOrDefault()?.Controls.OfType<TextBox>().FirstOrDefault()?.Focus();
                flp.Controls.OfType<Panel>().LastOrDefault()?.Controls.OfType<TextBox>().FirstOrDefault()?.Select();
                UpdateFlowLayoutPanelHeight(flp);
            };
            flp.Controls.Add(btnEkle);
        }

        private void AddTextBoxWithButton(FlowLayoutPanel flp, string prefix, int index, bool withButton = false)
        {
            int horizontalMargin = (flp.ClientSize.Width - contentWidth) / 2;
            int panelMargin = horizontalMargin > 0 ? horizontalMargin : 0;

            var panel = new Panel
            {
                Width = contentWidth,
                Height = 25,
                Margin = new Padding(panelMargin, 5, panelMargin, 5)
            };

            var txt = new TextBox
            {
                Width = TEXTBOX_WIDTH,
                Height = 20,
                AutoSize = false,
                Location = new Point(0, 0),
                Name = $"txt_{prefix}_{index}",
                ForeColor = Color.Gray,
                Text = $"Proje #{index}",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            txt.Enter += (s2, e2) =>
            {
                if (txt.Text == $"Proje #{txt.Name.Split('_').Last()}")
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };
            txt.Leave += (s2, e2) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = $"Proje #{txt.Name.Split('_').Last()}";
                    txt.ForeColor = Color.Gray;
                }
            };
            txt.TextChanged += (s2, e2) => UpdateContextMenu();

            panel.Controls.Add(txt);

            if (withButton)
            {
                var btnKaldir = new Button
                {
                    Text = "X",
                    Width = BUTTON_X_WIDTH,
                    Height = 20,
                    Location = new Point(TEXTBOX_WIDTH + PANEL_INNER_MARGIN, 0),
                    BackColor = Color.FromArgb(231, 76, 60),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Segoe UI", 8, FontStyle.Bold)
                };
                btnKaldir.Click += (s2, e2) =>
                {
                    flp.Controls.Remove(panel);
                    UpdateContextMenu();
                    UpdateFlowLayoutPanelHeight(flp);
                    UpdateAllPanelWidths(flp);
                };
                panel.Controls.Add(btnKaldir);
            }

            flp.Controls.Add(panel);
        }

        private void UpdatePanelLayout()
        {
            flpAltProjeTextBoxes.Visible = chkAltProjeVar.Checked;
            flpIliskiTextBoxes.Visible = chkProjeIliskisiVar.Checked;

            if (chkAltProjeVar.Checked && chkProjeIliskisiVar.Checked)
            {
                flpAltProjeTextBoxes.Location = new Point(txtProjeNo.Location.X + txtProjeNo.Width + 10, txtProjeNo.Location.Y + txtProjeNo.Height - 10);
                flpIliskiTextBoxes.Location = new Point(flpAltProjeTextBoxes.Location.X + flpAltProjeTextBoxes.Width + 10, flpAltProjeTextBoxes.Location.Y);
            }
            else
            {
                flpAltProjeTextBoxes.Location = new Point(txtProjeNo.Location.X + txtProjeNo.Width + 10, txtProjeNo.Location.Y + txtProjeNo.Height - 10);
                flpIliskiTextBoxes.Location = new Point(txtProjeNo.Location.X + txtProjeNo.Width + 10, txtProjeNo.Location.Y + txtProjeNo.Height - 10);
            }
        }

        private void UpdateContextMenu()
        {
            bool isProjeKayitli = !string.IsNullOrWhiteSpace(txtProjeNo.Text.Trim()) && ProjeFinans_ProjeKutukData.ProjeKutukAra(txtProjeNo.Text.Trim()) != null;

            if (chkAltProjeVar.Checked)
            {
                var altProjeler = flpAltProjeTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                    .Select(txt => txt.Text.Trim()).ToList();
                mnuProjeBilgileri.Enabled = altProjeler.Any();
                mnuProjeFiyatlandirma.Enabled = isProjeKayitli && altProjeler.Any();
            }
            else if (chkAltProjeYok.Checked && !string.IsNullOrWhiteSpace(txtProjeNo.Text.Trim()))
            {
                mnuProjeBilgileri.Enabled = true;
                mnuProjeFiyatlandirma.Enabled = isProjeKayitli;
            }
            else
            {
                mnuProjeBilgileri.Enabled = false;
                mnuProjeFiyatlandirma.Enabled = false;
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
            var proje = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);

            if (proje != null)
            {
                txtMusteriNo.Text = proje.musteriNo ?? "";
                txtMusteriAdi.Text = proje.musteriAdi ?? "";
                txtTeklifNo.Text = proje.teklifNo ?? "";
                txtIsFirsatiNo.Text = proje.isFirsatiNo ?? "";
                txtProjeNo.Text = proje.projeNo ?? "";
                chkAltProjeVar.Checked = proje.altProjeVarMi;
                chkAltProjeYok.Checked = !proje.altProjeVarMi;
                chkProjeIliskisiVar.Checked = proje.digerProjeIliskisiVarMi != "0";
                chkProjeIliskisiYok.Checked = proje.digerProjeIliskisiVarMi == "0";
                dtpSiparisSozlesmeTarihi.Value = proje.siparisSozlesmeTarihi;
                chkNakliyeVar.Checked = proje.nakliyeVarMi;
                chkNakliyeYok.Checked = !proje.nakliyeVarMi;
                chkTekil.Checked = proje.faturalamaSekli == "Tekil";
                chkCogul.Checked = proje.faturalamaSekli == "Cogul";

                string paraBirimiDegeri = proje.paraBirimi;
                if (paraBirimiDegeri == "TL")
                {
                    cmbParaBirimi.SelectedItem = "Türk Lirası (₺)";
                }
                else if (paraBirimiDegeri == "USD")
                {
                    cmbParaBirimi.SelectedItem = "Dolar ($)";
                }
                else if (paraBirimiDegeri == "EUR")
                {
                    cmbParaBirimi.SelectedItem = "Euro (€)";
                }
                else
                {
                    cmbParaBirimi.SelectedIndex = -1; 
                }

                flpAltProjeTextBoxes.Controls.Clear();
                flpAltProjeTextBoxes.Controls.Add(new Label
                {
                    Text = "Alt Projeler",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding((flpAltProjeTextBoxes.ClientSize.Width - 80) / 2, 0, 0, 5),
                    ForeColor = Color.FromArgb(44, 62, 80)
                });
                if (proje.altProjeVarMi && proje.altProjeBilgileri != null)
                {
                    int index = 1;
                    foreach (var altProje in proje.altProjeBilgileri)
                    {
                        AddTextBoxWithButton(flpAltProjeTextBoxes, "AltProje", index, true, altProje);
                        index++;
                    }
                }
                AddEkleButton(flpAltProjeTextBoxes, "AltProje");
                flpAltProjeTextBoxes.Visible = proje.altProjeVarMi;

                flpIliskiTextBoxes.Controls.Clear();
                flpIliskiTextBoxes.Controls.Add(new Label
                {
                    Text = "İlişkili Projeler",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding((flpIliskiTextBoxes.ClientSize.Width - 100) / 2, 0, 0, 5),
                    ForeColor = Color.FromArgb(44, 62, 80)
                });
                if (proje.digerProjeIliskisiVarMi != "0" && !string.IsNullOrEmpty(proje.digerProjeIliskisiVarMi))
                {
                    int index = 1;
                    var iliskiProjeler = proje.digerProjeIliskisiVarMi.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var iliskiProje in iliskiProjeler)
                    {
                        AddTextBoxWithButton(flpIliskiTextBoxes, "Iliski", index, true, iliskiProje.Trim());
                        index++;
                    }
                }
                AddEkleButton(flpIliskiTextBoxes, "Iliski");
                flpIliskiTextBoxes.Visible = proje.digerProjeIliskisiVarMi != "0";

                UpdateContextMenu();
                UpdatePanelLayout();
                UpdateFlowLayoutPanelHeight(flpAltProjeTextBoxes);
                UpdateFlowLayoutPanelHeight(flpIliskiTextBoxes);
                UpdateAllPanelWidths(flpAltProjeTextBoxes);
                UpdateAllPanelWidths(flpIliskiTextBoxes);

                var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
                var altProjeler = chkAltProjeVar.Checked
                    ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                        .Select(txt => txt.Text.Trim())
                        .ToList()
                    : null;
                var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);
                UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
            }
            else
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddTextBoxWithButton(FlowLayoutPanel flp, string prefix, int index, bool withButton = false, string text = null)
        {
            int horizontalMargin = (flp.ClientSize.Width - contentWidth) / 2;
            int panelMargin = horizontalMargin > 0 ? horizontalMargin : 0;

            var panel = new Panel
            {
                Width = contentWidth,
                Height = 25,
                Margin = new Padding(panelMargin, 5, panelMargin, 5)
            };

            var txt = new TextBox
            {
                Width = TEXTBOX_WIDTH,
                Height = 20,
                AutoSize = false,
                Location = new Point(0, 0),
                Name = $"txt_{prefix}_{index}",
                ForeColor = string.IsNullOrEmpty(text) ? Color.Gray : Color.Black,
                Text = string.IsNullOrEmpty(text) ? $"Proje #{index}" : text,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            txt.Enter += (s2, e2) =>
            {
                if (txt.Text == $"Proje #{txt.Name.Split('_').Last()}")
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };
            txt.Leave += (s2, e2) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = $"Proje #{txt.Name.Split('_').Last()}";
                    txt.ForeColor = Color.Gray;
                }
            };
            txt.TextChanged += (s2, e2) => UpdateContextMenu();

            panel.Controls.Add(txt);

            if (withButton)
            {
                var btnKaldir = new Button
                {
                    Text = "X",
                    Width = BUTTON_X_WIDTH,
                    Height = 20,
                    Location = new Point(TEXTBOX_WIDTH + PANEL_INNER_MARGIN, 0),
                    BackColor = Color.FromArgb(231, 76, 60),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Segoe UI", 8, FontStyle.Bold)
                };
                btnKaldir.Click += (s2, e2) =>
                {
                    flp.Controls.Remove(panel);
                    UpdateContextMenu();
                    UpdateFlowLayoutPanelHeight(flp);
                    UpdateAllPanelWidths(flp);
                };
                panel.Controls.Add(btnKaldir);
            }

            flp.Controls.Add(panel);
        }

        private void ctlProjeKutuk_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Proje Kütük";
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            var hataMesajlari = new List<string>();

            if (string.IsNullOrWhiteSpace(txtMusteriNo.Text))
                hataMesajlari.Add("Müşteri No boş olamaz.");

            if (string.IsNullOrWhiteSpace(txtMusteriAdi.Text))
                hataMesajlari.Add("Müşteri Adı boş olamaz.");

            if (string.IsNullOrWhiteSpace(txtTeklifNo.Text))
                hataMesajlari.Add("Teklif No boş olamaz.");
            else if (!int.TryParse(txtTeklifNo.Text.Trim(), out var teklifNo) || teklifNo <= 0)
                hataMesajlari.Add("Teklif No pozitif bir tam sayı olmalıdır.");

            if (string.IsNullOrWhiteSpace(txtIsFirsatiNo.Text))
                hataMesajlari.Add("İş Fırsatı No boş olamaz.");
            else if (!int.TryParse(txtIsFirsatiNo.Text.Trim(), out var isFirsatiNo) || isFirsatiNo <= 0)
                hataMesajlari.Add("İş Fırsatı No pozitif bir tam sayı olmalıdır.");

            if (string.IsNullOrWhiteSpace(txtProjeNo.Text))
                hataMesajlari.Add("Proje No boş olamaz.");
            else if (chkAltProjeVar.Checked)
            {
                string projeNumarasi = txtProjeNo.Text.Trim();
                if (!Regex.IsMatch(projeNumarasi, @"^\d{5}$|^\d{5}\.00$"))
                    hataMesajlari.Add("Proje No 5 haneli bir sayı (ör: 12345 veya 12345.00) olmalıdır.");
            }

            decimal bedel = decimal.TryParse(txtToplamBedel.Text.Trim(), out var parsedBedel) ? parsedBedel : 0;

            var altProjeler = chkAltProjeVar.Checked
                ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                    .Select(txt => txt.Text.Trim()).ToList()
                : null;

            if (chkAltProjeVar.Checked && (altProjeler == null || !altProjeler.Any()))
            {
                lblAltProjeHata.Text = "Alt proje var işaretlediniz fakat alt proje bilgisi girmediniz.";
                lblAltProjeHata.Visible = true;
                hataMesajlari.Add("Alt proje var işaretlendiniz fakat alt proje bilgisi girmediniz.");
            }
            else
            {
                lblAltProjeHata.Visible = false;
            }

            if (chkProjeIliskisiVar.Checked)
            {
                var iliskiProjeler = flpIliskiTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                    .Select(txt => txt.Text.Trim()).ToList();
                if (!iliskiProjeler.Any())
                {
                    hataMesajlari.Add("İlişkili proje var işaretlendiniz fakat ilişkili proje bilgisi girmediniz.");
                }
            }

            if (!chkTekil.Checked && !chkCogul.Checked)
            {
                hataMesajlari.Add("Ödeme şekli (Tekil veya Çoğul) seçilmelidir.");
            }

            if (cmbParaBirimi.SelectedItem == null)
            {
                hataMesajlari.Add("Para birimi seçilmelidir.");
            }

            if (hataMesajlari.Any())
            {
                MessageBox.Show(string.Join("\n", hataMesajlari), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string projeNo = txtProjeNo.Text.Trim();

            var anaProjeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
            if (anaProjeBilgi == null)
            {
                MessageBox.Show($"Proje '{projeNo}' için proje bilgileri bulunamadı. Lütfen önce proje bilgilerini kaydedin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (chkAltProjeVar.Checked)
            {
                var eksikAltProjeler = new List<string>();
                foreach (var altProje in altProjeler)
                {
                    if (ProjeFinans_Projeler.GetProjeBilgileri(altProje) == null)
                    {
                        eksikAltProjeler.Add(altProje);
                    }
                }

                if (eksikAltProjeler.Any())
                {
                    MessageBox.Show($"Aşağıdaki alt projeler için proje bilgileri bulunamadı:\n\n{string.Join("\n", eksikAltProjeler)}\n\nLütfen önce bu projelerin bilgilerini kaydedin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            string paraBirimi = "";
            string selectedParaBirimiText = cmbParaBirimi.SelectedItem.ToString();
            if (selectedParaBirimiText.Contains("Türk Lirası"))
            {
                paraBirimi = "TL";
            }
            else if (selectedParaBirimiText.Contains("Dolar"))
            {
                paraBirimi = "USD";
            }
            else if (selectedParaBirimiText.Contains("Euro"))
            {
                paraBirimi = "EUR";
            }

            var kutuk = new ProjeKutuk
            {
                musteriNo = txtMusteriNo.Text.Trim(),
                musteriAdi = txtMusteriAdi.Text.Trim(),
                teklifNo = txtTeklifNo.Text.Trim(),
                isFirsatiNo = txtIsFirsatiNo.Text.Trim(),
                projeNo = projeNo,
                altProjeVarMi = chkAltProjeVar.Checked,
                digerProjeIliskisiVarMi = chkProjeIliskisiVar.Checked
                    ? string.Join(",", flpIliskiTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                        .Select(txt => txt.Text.Trim()))
                    : "0",
                siparisSozlesmeTarihi = dtpSiparisSozlesmeTarihi.Value,
                toplamBedel = bedel,
                nakliyeVarMi = chkNakliyeVar.Checked,
                altProjeBilgileri = altProjeler ?? new List<string>(),
                faturalamaSekli = chkTekil.Checked ? "Tekil" : "Cogul",
                paraBirimi = paraBirimi 
            };

            bool isProjeKayitli = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo) != null;

            if (isProjeKayitli)
            {
                if (ProjeFinans_ProjeKutukData.ProjeKutukGuncelle(kutuk))
                {
                    var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
                    var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);
                    UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
                }
                else
                {
                    MessageBox.Show("Proje güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (ProjeFinans_ProjeKutukData.ProjeKutukEkle(kutuk))
                {
                    if (chkAltProjeVar.Checked && kutuk.altProjeBilgileri.Any())
                    {
                        foreach (var altProje in kutuk.altProjeBilgileri)
                        {
                            if (!ProjeFinans_ProjeKutukData.AltProjeEkle(projeNo, altProje))
                            {
                                MessageBox.Show($"Alt Proje '{altProje}' için ilişki kaydı eklenirken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    MessageBox.Show("Kayıt başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    projeBilgileriKaydedildi = false;
                    UpdateContextMenu();

                    var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
                    var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);
                    UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
                }
                else
                {
                    MessageBox.Show("Kayıt eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void txtMusteriNo_Leave(object sender, EventArgs e)
        {
            string musteriNo = txtMusteriNo.Text.Trim();
            if (!string.IsNullOrEmpty(musteriNo))
            {
                ProjeFinans_MusterilerData musteriData = new ProjeFinans_MusterilerData();
                Musteriler musteri = musteriData.GetMusteriByMusteriNo(musteriNo);

                if (musteri != null)
                {
                    txtMusteriAdi.Text = musteri.musteriAdi;
                }
                else
                {
                    txtMusteriAdi.Text = "";
                    MessageBox.Show("Belirtilen müşteri numarası ile bir müşteri bulunamadı.", "Müşteri Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtMusteriAdi.Text = "";
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeNo.Text))
            {
                MessageBox.Show("Lütfen silinecek proje numarasını girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string projeNo = txtProjeNo.Text.Trim();
            var altProjeler = chkAltProjeVar.Checked
                ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                    .Select(txt => txt.Text.Trim())
                    .ToList()
                : null;

            if (ProjeFinans_ProjeKutukData.HasRelatedRecords(projeNo, altProjeler))
            {
                return;
            }

            var result = MessageBox.Show($"Proje '{projeNo}' ve varsa alt projeleri silinecek. Onaylıyor musunuz?", "Proje Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            if (ProjeFinans_ProjeKutukData.ProjeKutukSil(projeNo, altProjeler))
            {
                if (ProjeFinans_Projeler.ProjeSil(projeNo))
                {
                    MessageBox.Show("Proje ve alt projeleri başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtMusteriNo.Text = "";
                    txtMusteriAdi.Text = "";
                    txtTeklifNo.Text = "";
                    txtIsFirsatiNo.Text = "";
                    txtProjeNo.Text = "";
                    txtToplamBedel.Text = "";
                    chkAltProjeVar.Checked = false;
                    chkAltProjeYok.Checked = false;
                    chkProjeIliskisiVar.Checked = false;
                    chkProjeIliskisiYok.Checked = false;
                    chkNakliyeVar.Checked = false;
                    chkNakliyeYok.Checked = false;
                    chkTekil.Checked = false;
                    chkCogul.Checked = false;
                    cmbParaBirimi.SelectedIndex = -1; 
                    lblAltProjeHata.Text = "";

                    flpAltProjeTextBoxes.Controls.Clear();
                    flpIliskiTextBoxes.Controls.Clear();
                    flpAltProjeTextBoxes.Controls.Add(new Label
                    {
                        Text = "Alt Projeler",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        AutoSize = true,
                        Margin = new Padding((flpAltProjeTextBoxes.ClientSize.Width - 80) / 2, 0, 0, 5),
                        ForeColor = Color.FromArgb(44, 62, 80)
                    });
                    flpIliskiTextBoxes.Controls.Add(new Label
                    {
                        Text = "İlişkili Projeler",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        AutoSize = true,
                        Margin = new Padding((flpIliskiTextBoxes.ClientSize.Width - 100) / 2, 0, 0, 5),
                        ForeColor = Color.FromArgb(44, 62, 80)
                    });
                    AddEkleButton(flpAltProjeTextBoxes, "AltProje");
                    AddEkleButton(flpIliskiTextBoxes, "Iliski");
                    UpdateContextMenu();
                    UpdatePanelLayout();
                    UpdateFlowLayoutPanelHeight(flpAltProjeTextBoxes);
                    UpdateFlowLayoutPanelHeight(flpIliskiTextBoxes);
                }
                else
                {
                    MessageBox.Show("Proje silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Proje silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}