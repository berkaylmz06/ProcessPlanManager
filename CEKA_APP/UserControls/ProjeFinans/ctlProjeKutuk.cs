using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Services.ProjeFinans;
using CEKA_APP.UsrControl.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
        private bool _fiyatlandirmaEventAttached = false;
        private bool projeBilgileriEventAttached = false;

        private const int TEXTBOX_WIDTH = 130;
        private const int BUTTON_X_WIDTH = 20;
        private const int HORIZONTAL_MARGIN = 40;
        private const int PANEL_INNER_MARGIN = 5;
        private const int MAX_PANEL_HEIGHT = 200;

        private int contentWidth;
        private int panelWidth;
        private int buttonWidth;

        private readonly IServiceProvider _serviceProvider;
        private IFiyatlandirmaService _fiyatlandirmaService => _serviceProvider.GetRequiredService<IFiyatlandirmaService>();
        private IMusterilerService _musterilerService => _serviceProvider.GetRequiredService<IMusterilerService>();
        private IFinansProjelerService _finansProjelerService => _serviceProvider.GetRequiredService<IFinansProjelerService>();
        private IProjeKutukService _projeKutukService => _serviceProvider.GetRequiredService<IProjeKutukService>();
        private ISayfaStatusService _sayfaStatusService => _serviceProvider.GetRequiredService<ISayfaStatusService>();
        private IUserControlFactory _userControlFactory => _serviceProvider.GetRequiredService<IUserControlFactory>();
        private IProjeIliskiService _projeIliskiService=> _serviceProvider.GetRequiredService<IProjeIliskiService>();
        private ITeminatMektuplariService _teminatMektuplariService => _serviceProvider.GetRequiredService<ITeminatMektuplariService>();

        public ctlProjeKutuk(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));


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
                    parentForm.projeBilgileri = new ctlProjeBilgileri(_serviceProvider);
                    parentForm.projeBilgileri.Dock = DockStyle.Fill;

                    if (!projeBilgileriEventAttached)
                    {
                        parentForm.projeBilgileri.OnKaydet += () =>
                        {
                            projeBilgileriKaydedildi = true;
                            SaveProjeKutukIfChanged();
                        };
                        projeBilgileriEventAttached = true;
                    }
                }

                var altProjelerNo = chkAltProjeVar.Checked
                    ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                        .Select(txt => txt.Text.Trim())
                        .ToList()
                    : new List<string> { txtProjeNo.Text.Trim() };

                if (altProjelerNo.Any(no => string.IsNullOrEmpty(no)))
                {
                    MessageBox.Show("Geçerli bir proje numarası bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string anaProjeNo = chkAltProjeVar.Checked ? txtProjeNo.Text.Trim() : null;

                parentForm.projeBilgileri.LoadProjects(altProjelerNo, anaProjeNo);
                parentForm.NavigateToUserControl(parentForm.projeBilgileri);
            }; mnuProjeFiyatlandirma.Click += (s, e) =>
            {
                string projeNo = txtProjeNo.Text.Trim();
                if (string.IsNullOrEmpty(projeNo))
                {
                    MessageBox.Show("Lütfen bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int? projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
                if (!projeId.HasValue)
                {
                    MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<int> altProjeler = chkAltProjeVar.Checked
                    ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Select(txt => txt.Text.Trim())
                        .Where(text => !string.IsNullOrEmpty(text))
                        .Select(text => _finansProjelerService.GetProjeIdByNo(text))
                        .Where(id => id.HasValue)
                        .Select(id => id.Value)
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
                    parentFormFiyat.projeFiyatlandirma = _userControlFactory.CreateProjeFiyatlandirmaControl();
                    parentFormFiyat.projeFiyatlandirma.Dock = DockStyle.Fill;
                }
                else
                {
                    parentFormFiyat.projeFiyatlandirma.Dock = DockStyle.Fill;
                }

                if (!_fiyatlandirmaEventAttached)
                {
                    parentFormFiyat.projeFiyatlandirma.OnFiyatlandirmaKaydedildi += (projeAdi) =>
                    {
                        try
                        {
                            int? pid = _finansProjelerService.GetProjeIdByNo(projeAdi);
                            if (!pid.HasValue) return;

                            var (toplamBedel, eksikFiyatlandirmaProjeler) = _fiyatlandirmaService.GetToplamBedel(pid.Value, altProjeler);
                            UpdateToplamBedelUI(pid.Value, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"OnFiyatlandirmaKaydedildi sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };
                    _fiyatlandirmaEventAttached = true; 
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
                        if (altProjeler != null && altProjeler.Count > 0)
                        {
                            comboBox.Items.AddRange(altProjeler.Select(id => _finansProjelerService.GetProjeNoById(id) ?? id.ToString()).ToArray());
                        }

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
                                int? selectedProjeId = _finansProjelerService.GetProjeIdByNo(comboBox.SelectedItem.ToString());
                                if (selectedProjeId.HasValue)
                                {
                                    form.Tag = selectedProjeId.Value;
                                    form.DialogResult = DialogResult.OK;
                                    form.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Geçerli bir proje ID'si alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
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
                            parentFormFiyat.projeFiyatlandirma.LoadProjeFiyatlandirma((int)form.Tag, autoSearch: false, altProjeler: altProjeler);
                            parentFormFiyat.NavigateToUserControl(parentFormFiyat.projeFiyatlandirma);
                            parentFormFiyat.projeFiyatlandirma.btnProjeAra_Click(null, EventArgs.Empty);
                        }
                    }
                }
                else
                {
                    parentFormFiyat.projeFiyatlandirma.LoadProjeFiyatlandirma(projeId.Value, autoSearch: false, altProjeler: altProjeler);
                    parentFormFiyat.NavigateToUserControl(parentFormFiyat.projeFiyatlandirma);
                    parentFormFiyat.projeFiyatlandirma.btnProjeAra_Click(null, EventArgs.Empty);
                }
            }; flpAltProjeTextBoxes = new FlowLayoutPanel
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
                if (chkTekil.Checked) chkCoklu.Checked = false;
            };
            chkCoklu.CheckedChanged += (s, e) =>
            {
                if (chkCoklu.Checked) chkTekil.Checked = false;
            };
            chkMontajTamamlandi.CheckedChanged += (s, e) =>
            {
                if (chkMontajTamamlandi.Checked) chkMontajTamamlanmadi.Checked = false;
            };
            chkMontajTamamlanmadi.CheckedChanged += (s, e) =>
            {
                if (chkMontajTamamlanmadi.Checked) chkMontajTamamlandi.Checked = false;
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

        public void UpdateToplamBedelUI(int projeId, decimal toplamBedel, List<int> eksikFiyatlandirmaProjeler, List<int> altProjeler)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateToplamBedelUI(projeId, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
                }));
            }
            else
            {
                txtToplamBedel.Text = toplamBedel.ToString("F2");

                if (altProjeler != null && altProjeler.Any())
                {
                    if (eksikFiyatlandirmaProjeler != null && eksikFiyatlandirmaProjeler.Any())
                    {
                        var eksikProjeNumaralari = eksikFiyatlandirmaProjeler
                            .Where(id => altProjeler.Contains(id))
                            .Select(id => _finansProjelerService.GetProjeNoById(id))
                            .Where(no => !string.IsNullOrEmpty(no))
                            .ToList();

                        if (eksikProjeNumaralari.Any())
                        {
                            lblAltProjeHata.Text = $"Eksik fiyatlandırma: {string.Join(", ", eksikProjeNumaralari)}";
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
                        lblAltProjeHata.Text = "";
                        lblAltProjeHata.Visible = false;
                    }
                }
                else
                {
                    if (eksikFiyatlandirmaProjeler != null && eksikFiyatlandirmaProjeler.Contains(projeId))
                    {
                        string projeNo = _finansProjelerService.GetProjeNoById(projeId);
                        if (!string.IsNullOrEmpty(projeNo))
                        {
                            lblAltProjeHata.Text = $"Eksik fiyatlandırma: {projeNo}";
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

                string projeNo = txtProjeNo.Text.Trim();
                if (aktif.Name == "chkAltProjeVar")
                {
                    if (!Regex.IsMatch(projeNo, @"^\d{5}$|^\d{5}\.00$"))
                    {
                        aktif.Checked = false;
                        MessageBox.Show("Proje numarası 5 haneli bir sayı (örneğin: 12345 veya 12345.00) olmalıdır.",
                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtProjeNo.BackColor = Color.FromArgb(255, 204, 203);
                        txtProjeNo.Focus();
                        return;
                    }
                }

                pasif.Checked = false;
                flp.Controls.Clear();
                flp.Controls.Add(aktif.Name == "chkAltProjeVar"
                    ? new Label { Text = "Alt Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.ClientSize.Width - 80) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) }
                    : new Label { Text = "İlişkili Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.ClientSize.Width - 100) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) });

                int index = 1;
                AddTextBoxWithButton(flp, prefix, index, true, "");
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
                var lastTextBox = flp.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .LastOrDefault(t => !string.IsNullOrWhiteSpace(t.Text) && t.Text != $"Proje #{t.Name.Split('_').Last()}");

                if (lastTextBox != null)
                {
                    string mevcutProjeNo = lastTextBox.Text.Trim();
                    var altProjeNumaralari = flp.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(t => t != lastTextBox && !string.IsNullOrWhiteSpace(t.Text) && t.Text != $"Proje #{t.Name.Split('_').Last()}")
                        .Select(t => t.Text.Trim())
                        .ToList();

                    if (altProjeNumaralari.Contains(mevcutProjeNo))
                    {
                        MessageBox.Show($"Proje numarası '{mevcutProjeNo}' zaten mevcut. Lütfen farklı bir proje numarası girin.",
                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lastTextBox.BackColor = Color.FromArgb(255, 204, 203); 
                        lastTextBox.Focus();
                        return; 
                    }

                    if (!Regex.IsMatch(mevcutProjeNo, @"^\d{5}\.\d{2,}$"))
                    {
                        MessageBox.Show("Alt proje numarası doğru formatta olmalıdır.",
                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lastTextBox.BackColor = Color.FromArgb(255, 204, 203);
                        lastTextBox.Focus();
                     
                        return; 
                    }
                }

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
            string projeNo = txtProjeNo.Text.Trim(); 
            int? projeId = _finansProjelerService.GetProjeIdByNo(projeNo); 
            bool isProjeKayitli = projeId.HasValue;  

            if (chkAltProjeVar.Checked)
            {
                var altProjeler = flpAltProjeTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}") 
                    .Select(txt => txt.Text.Trim())
                    .Where(no => !string.IsNullOrEmpty(no)) 
                    .ToList();

                bool altProjelerGecerli = altProjeler.Any();  

                mnuProjeBilgileri.Enabled = true;  
                mnuProjeFiyatlandirma.Enabled = isProjeKayitli && altProjelerGecerli; 
            }
            else if (chkAltProjeYok.Checked)
            {
                mnuProjeBilgileri.Enabled = true; 
                mnuProjeFiyatlandirma.Enabled = isProjeKayitli; 
            }
            else
            {
                mnuProjeBilgileri.Enabled = true; 
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
            var projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            if (!projeId.HasValue)
            {
                MessageBox.Show("Lütfen önce geçerli bir proje numarası ile proje araması yapın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ProjeKutuk kutuk = _projeKutukService.GetProjeKutukStatus(projeId.Value);
            string status = kutuk != null ? kutuk.status : "Başlatıldı";

            txtStatus.Text = status;

            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjeNo.Enabled = true;
                return;
            }
            var proje = _projeKutukService.ProjeKutukAra(projeId.Value);

            if (proje != null)
            {
                txtMusteriNo.Text = proje.musteriNo ?? "";
                txtMusteriAdi.Text = proje.musteriAdi ?? "";
                txtIsFirsatiNo.Text = proje.isFirsatiNo ?? "";
                txtProjeNo.Text = proje.projeNo ?? "";
                txtProjeNo.Enabled = false;
                chkAltProjeVar.Checked = proje.altProjeVarMi;
                chkAltProjeYok.Checked = !proje.altProjeVarMi;
                chkProjeIliskisiVar.Checked = proje.digerProjeIliskisiVarMi != "0";
                chkProjeIliskisiYok.Checked = proje.digerProjeIliskisiVarMi == "0";
                dtpSiparisSozlesmeTarihi.Value = proje.siparisSozlesmeTarihi;
                chkNakliyeVar.Checked = proje.nakliyeVarMi;
                chkNakliyeYok.Checked = !proje.nakliyeVarMi;
                chkTekil.Checked = proje.faturalamaSekli == "Tekil";
                chkCoklu.Checked = proje.faturalamaSekli == "Cogul";
                chkMontajTamamlandi.Checked = proje.montajTamamlandiMi;
                chkMontajTamamlanmadi.Checked = !proje.montajTamamlandiMi;

                switch (proje.paraBirimi)
                {
                    case "TL":
                        cmbParaBirimi.SelectedItem = "Türk Lirası (₺)";
                        break;
                    case "USD":
                        cmbParaBirimi.SelectedItem = "Dolar ($)";
                        break;
                    case "EUR":
                        cmbParaBirimi.SelectedItem = "Euro (€)";
                        break;
                    default:
                        cmbParaBirimi.SelectedIndex = -1;
                        break;
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
                    foreach (var altProjeId in proje.altProjeBilgileri)
                    {
                        string altProjeNo = _finansProjelerService.GetProjeNoById(altProjeId);
                        if (!string.IsNullOrEmpty(altProjeNo))
                        {
                            AddTextBoxWithButton(flpAltProjeTextBoxes, "AltProje", index, true, altProjeNo);
                            index++;
                        }
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

                if (!string.IsNullOrEmpty(proje.digerProjeIliskisiVarMi) && proje.digerProjeIliskisiVarMi != "0")
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

                List<int> altProjeler = chkAltProjeVar.Checked
                    ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Select(txt => txt.Text.Trim())
                        .Where(text => !string.IsNullOrEmpty(text))
                        .Select(text => _finansProjelerService.GetProjeIdByNo(text))
                        .Where(id => id.HasValue)
                        .Select(id => id.Value)
                        .ToList()
                    : new List<int>();

                int? pid = _finansProjelerService.GetProjeIdByNo(proje.projeNo);
                if (pid.HasValue)
                {
                    var projeIdsToCheck = new List<int> { pid.Value };
                    projeIdsToCheck.AddRange(altProjeler);
                    var (toplamBedel, eksikFiyatlandirmaProjeler) = _fiyatlandirmaService.GetToplamBedel(pid.Value, altProjeler);
                    UpdateToplamBedelUI(pid.Value, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
                }
            }
            else
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProjeNo.Enabled = true;
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
                ForeColor = Color.Black,
                Text = text ?? "",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            txt.Leave += (s2, e2) =>
            {
                var args = e2 as CancelEventArgs;

                if (!string.IsNullOrWhiteSpace(txt.Text) && txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                {
                    string mevcutProjeNo = txt.Text.Trim();
                    var altProjeNumaralari = flp.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(t => t != txt && !string.IsNullOrWhiteSpace(t.Text) && t.Text != $"Proje #{t.Name.Split('_').Last()}")
                        .Select(t => t.Text.Trim())
                        .ToList();


                    if (altProjeNumaralari.Contains(mevcutProjeNo))
                    {
                        MessageBox.Show($"Proje numarası '{mevcutProjeNo}' zaten mevcut. Lütfen farklı bir proje numarası girin.",
                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt.BackColor = Color.FromArgb(255, 204, 203);
                        txt.Text = $"Proje #{txt.Name.Split('_').Last()}";
                        txt.ForeColor = Color.Gray;
                        txt.Focus();
                        if (args != null) args.Cancel = true;
                    }
                    else
                    {
                        if (!Regex.IsMatch(mevcutProjeNo, @"^\d{5}\.\d{2,}$"))
                        {
                            MessageBox.Show("Alt proje numarası 5 haneli bir sayı, nokta ve en az 2 haneli bir ek formatında olmalıdır (örneğin: 12345.12).",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txt.BackColor = Color.FromArgb(255, 204, 203);
                            txt.Text = $"Proje #{txt.Name.Split('_').Last()}";
                            txt.ForeColor = Color.Gray;
                            txt.Focus();
                            if (args != null) args.Cancel = true; 
                        }
                        else
                        {
                            txt.BackColor = Color.White;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = $"Proje #{txt.Name.Split('_').Last()}";
                    txt.ForeColor = Color.Gray;
                    txt.BackColor = Color.White;
                }

                UpdateContextMenu();
            };

            txt.Enter += (s2, e2) =>
            {
                if (txt.Text == $"Proje #{txt.Name.Split('_').Last()}")
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                    txt.BackColor = Color.White;
                }
            };

            txt.TextChanged += (s2, e2) =>
            {
                if (!string.IsNullOrWhiteSpace(txt.Text) && txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                {
                    string mevcutProjeNo = txt.Text.Trim();
                    var altProjeNumaralari = flp.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(t => t != txt && !string.IsNullOrWhiteSpace(t.Text) && t.Text != $"Proje #{t.Name.Split('_').Last()}")
                        .Select(t => t.Text.Trim())
                        .ToList();

                    if (altProjeNumaralari.Contains(mevcutProjeNo))
                    {
                        txt.BackColor = Color.FromArgb(255, 204, 203);
                    }
                    else
                    {
                        txt.BackColor = Color.White; 
                    }
                }
                else
                {
                    txt.BackColor = Color.White;
                }
                UpdateContextMenu();
            };

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
        private void SaveProjeKutukIfChanged()
        {
            var hataMesajlari = new List<string>();
            var eksikAltProjeBilgileri = new List<string>();

            if (string.IsNullOrWhiteSpace(txtMusteriNo.Text))
                hataMesajlari.Add("Müşteri No boş olamaz.");

            if (string.IsNullOrWhiteSpace(txtMusteriAdi.Text))
                hataMesajlari.Add("Müşteri Adı boş olamaz.");

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

            var altProjeler = new List<int>();
            if (chkAltProjeVar.Checked)
            {
                var altProjeTextBoxes = flpAltProjeTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .ToList();

                if (!altProjeTextBoxes.Any())
                {
                    hataMesajlari.Add("Alt proje var işaretlediniz fakat alt proje bilgisi girilmedi.");
                }
                else
                {
                    foreach (var txt in altProjeTextBoxes)
                    {
                        string altProjeNo = txt.Text.Trim();
                        if (!string.IsNullOrWhiteSpace(altProjeNo) && altProjeNo != $"Proje #{txt.Name.Split('_').Last()}")
                        {
                            int? altProjeId = _finansProjelerService.GetProjeIdByNo(altProjeNo);
                            if (altProjeId.HasValue)
                                altProjeler.Add(altProjeId.Value);
                            else
                                eksikAltProjeBilgileri.Add(altProjeNo);
                        }
                    }
                }


                if (!altProjeler.Any() && chkAltProjeVar.Checked)
                {
                    lblAltProjeHata.Text = "Alt proje var işaretlediniz fakat geçerli alt proje bilgisi girmediniz.";
                    lblAltProjeHata.ForeColor = Color.Red;
                    lblAltProjeHata.Visible = true;
                    hataMesajlari.Add("Alt proje var işaretlendiniz fakat geçerli alt proje bilgisi girmediniz.");
                }

                if (eksikAltProjeBilgileri.Any())
                {
                    MessageBox.Show($"Aşağıdaki alt projeler için proje bilgileri bulunamadı:\n\n{string.Join("\n", eksikAltProjeBilgileri)}\n\nLütfen önce bu projelerin bilgilerini kaydedin.",
                        "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (chkProjeIliskisiVar.Checked)
            {
                var iliskiProjeler = flpIliskiTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Select(txt => txt.Text.Trim())
                    .Where(text => !string.IsNullOrEmpty(text))
                    .ToList();

                if (!iliskiProjeler.Any())
                    hataMesajlari.Add("İlişkili proje var işaretlendiniz fakat ilişkili proje bilgisi girmediniz.");
            }

            if (!chkTekil.Checked && !chkCoklu.Checked)
                hataMesajlari.Add("Ödeme şekli (Tekil veya Çoğul) seçilmelidir.");

            if (cmbParaBirimi.SelectedItem == null)
                hataMesajlari.Add("Para birimi seçilmelidir.");

            if (hataMesajlari.Any())
            {
                MessageBox.Show(string.Join("\n", hataMesajlari), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string projeNo = txtProjeNo.Text.Trim();
            int? projeId = _finansProjelerService.GetProjeIdByNo(projeNo);

            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' için proje ID bulunamadı. Lütfen önce proje bilgilerini kaydedin.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var anaProjeBilgi = _finansProjelerService.GetProjeBilgileri(projeId.Value);

            if (anaProjeBilgi == null)
            {
                MessageBox.Show($"Proje '{projeNo}' için proje bilgileri bulunamadı. Lütfen önce proje bilgilerini kaydedin.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string paraBirimi = cmbParaBirimi.SelectedItem.ToString().Contains("Türk Lirası") ? "TL" :
                                cmbParaBirimi.SelectedItem.ToString().Contains("Dolar") ? "USD" :
                                cmbParaBirimi.SelectedItem.ToString().Contains("Euro") ? "EUR" : "";

            var kutuk = new ProjeKutuk
            {
                projeId = projeId.Value,
                musteriNo = txtMusteriNo.Text.Trim(),
                musteriAdi = txtMusteriAdi.Text.Trim(),
                isFirsatiNo = txtIsFirsatiNo.Text.Trim(),
                projeNo = projeNo,
                altProjeVarMi = chkAltProjeVar.Checked,
                digerProjeIliskisiVarMi = chkProjeIliskisiVar.Checked
                    ? string.Join(",", flpIliskiTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Select(txt => txt.Text.Trim())
                        .Where(text => !string.IsNullOrEmpty(text)))
                    : "0",
                siparisSozlesmeTarihi = dtpSiparisSozlesmeTarihi.Value.Date,
                toplamBedel = bedel,
                nakliyeVarMi = chkNakliyeVar.Checked,
                altProjeBilgileri = altProjeler,
                faturalamaSekli = chkTekil.Checked ? "Tekil" : "Cogul",
                paraBirimi = paraBirimi,
                montajTamamlandiMi = chkMontajTamamlandi.Checked
            };

            bool isProjeKayitli = _projeKutukService.ProjeKutukAra(projeId.Value) != null;

            var (toplamBedel, eksikFiyatlandirmaProjeler) = _fiyatlandirmaService.GetToplamBedel(projeId.Value, altProjeler);

            var bildirimMesajlari = new List<string>();
            bool degisiklikVar = false;

            try
            {
                if (isProjeKayitli)
                {
                    var mevcutKutuk = _projeKutukService.ProjeKutukAra(projeId.Value);
                    if (mevcutKutuk == null)
                    {
                        MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    degisiklikVar = mevcutKutuk.musteriNo?.Trim() != kutuk.musteriNo ||
                                    mevcutKutuk.musteriAdi?.Trim() != kutuk.musteriAdi ||
                                    mevcutKutuk.isFirsatiNo?.Trim() != kutuk.isFirsatiNo ||
                                    mevcutKutuk.altProjeVarMi != kutuk.altProjeVarMi ||
                                    mevcutKutuk.digerProjeIliskisiVarMi?.Trim() != kutuk.digerProjeIliskisiVarMi ||
                                    mevcutKutuk.siparisSozlesmeTarihi.Date != kutuk.siparisSozlesmeTarihi.Date ||
                                    mevcutKutuk.paraBirimi?.Trim() != kutuk.paraBirimi ||
                                    mevcutKutuk.toplamBedel != kutuk.toplamBedel ||
                                    mevcutKutuk.faturalamaSekli?.Trim() != kutuk.faturalamaSekli ||
                                    mevcutKutuk.nakliyeVarMi != kutuk.nakliyeVarMi ||
                                    mevcutKutuk.projeId != kutuk.projeId ||
                                    mevcutKutuk.montajTamamlandiMi != kutuk.montajTamamlandiMi ||
                                    !(mevcutKutuk.altProjeBilgileri?.OrderBy(x => x).SequenceEqual(kutuk.altProjeBilgileri.OrderBy(x => x)) ?? false);

                    var mevcutAltProjeler = mevcutKutuk.altProjeBilgileri ?? new List<int>();
                    var eklenenAltProjeler = altProjeler.Except(mevcutAltProjeler).ToList();
                    var kaldirilanAltProjeler = mevcutAltProjeler.Except(altProjeler).ToList();


                        if (!_projeKutukService.ProjeKutukGuncelle(kutuk))
                        {
                            MessageBox.Show("Proje güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        foreach (var altProjeId in eklenenAltProjeler)
                        {
                            if (!_projeIliskiService.AltProjeEkle(projeId.Value, altProjeId))
                            {
                                MessageBox.Show($"Alt Proje '{altProjeId}' için ilişki kaydı eklenirken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        foreach (var altProjeId in kaldirilanAltProjeler)
                        {
                            if (!_projeIliskiService.AltProjeSil(projeId.Value, altProjeId))
                            {
                                MessageBox.Show($"Alt Proje '{altProjeId}' için ilişki kaydı silinirken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        UpdateToplamBedelUI(projeId.Value, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
                        bildirimMesajlari.Add($"Proje '{projeNo}' başarıyla güncellendi.");
                        degisiklikVar = true;
                  
                }
                else
                {
                    if (_projeKutukService.ProjeKutukEkle(kutuk))
                    {
                        if (chkAltProjeVar.Checked && kutuk.altProjeBilgileri.Any())
                        {
                            foreach (var altProjeId in kutuk.altProjeBilgileri)
                            {
                                if (!_projeIliskiService.AltProjeEkle(projeId.Value, altProjeId))
                                {
                                    MessageBox.Show($"Alt Proje '{altProjeId}' için ilişki kaydı eklenirken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }

                        UpdateToplamBedelUI(projeId.Value, toplamBedel, eksikFiyatlandirmaProjeler, altProjeler);
                        bildirimMesajlari.Add("Proje kütük kaydı başarıyla oluşturuldu.");
                        degisiklikVar = true;
                        txtProjeNo.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Kayıt eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (degisiklikVar)
                {
                    _projeKutukService.UpdateProjeKutukDurum(projeId.Value, chkMontajTamamlandi.Checked);
                    ProjeKutuk yeniStatu = _projeKutukService.GetProjeKutukStatus(projeId.Value);
                    string status = yeniStatu != null ? yeniStatu.status : "Başlatıldı";
                    txtStatus.Text = status;
                    projeBilgileriKaydedildi = false;
                    UpdateContextMenu();

                    if (bildirimMesajlari.Any())
                    {
                        MessageBox.Show(string.Join("\n", bildirimMesajlari), "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    _projeKutukService.UpdateProjeKutukDurum(projeId.Value, chkMontajTamamlandi.Checked);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İşlem sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SaveProjeKutukIfChanged();
        }


        private void txtMusteriNo_Leave(object sender, EventArgs e)
        {
            string musteriNo = txtMusteriNo.Text.Trim();
            if (!string.IsNullOrEmpty(musteriNo))
            {
                Musteriler musteri = _musterilerService.GetMusteriByMusteriNo(musteriNo);

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
            int? projeId = _finansProjelerService.GetProjeIdByNo(projeNo);

            if (projeId == null)
            {
                MessageBox.Show($"Proje '{projeNo}' için proje ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<int> altProjeIdler = new List<int>();
            if (chkAltProjeVar.Checked)
            {
                var altProjeler = flpAltProjeTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                    .Select(txt => txt.Text.Trim())
                    .ToList();

                foreach (var altProje in altProjeler)
                {
                    int? altProjeId = _finansProjelerService.GetProjeIdByNo(altProje);
                    if (altProjeId == null)
                    {
                        MessageBox.Show($"Alt proje '{altProje}' için proje ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    altProjeIdler.Add(altProjeId.Value);
                }
            }

            var (hasRelated, details) = _projeKutukService.HasRelatedRecords(projeId.Value, altProjeIdler);

            if (hasRelated)
            {
                string detailText = string.Join("\n", details);
                MessageBox.Show(
                    $"Bu proje için ilgili kayıtlar mevcut olduğu için silme işlemi gerçekleştirilemez.\n\nBağlı veriler:\n{detailText}\n\nLütfen önce bu verileri kaldırın veya güncelleyin.",
                    "Silme İşlemi Engellendi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            var result = MessageBox.Show($"Proje '{projeNo}' ve varsa alt projeleri silinecek. Onaylıyor musunuz?", "Proje Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            if (_projeKutukService.ProjeKutukSil(projeId.Value, altProjeIdler))
            {
                bool silmeBasarili = true;

                if (!_finansProjelerService.ProjeSil(projeId.Value))
                {
                    silmeBasarili = false;
                }

                foreach (var altProjeId in altProjeIdler)
                {
                    if (!_finansProjelerService.ProjeSil(altProjeId))
                    {
                        silmeBasarili = false;
                    }
                }

                if (silmeBasarili)
                {
                    MessageBox.Show("Proje ve alt projeleri başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Temizle();
                }
                else
                {
                    MessageBox.Show("Proje veya alt projeler silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }
        public void Temizle() 
        {
            txtStatus.Text = "";
            txtMusteriNo.Text = "";
            txtMusteriAdi.Text = "";
            txtIsFirsatiNo.Text = "";
            txtProjeNo.Text = "";
            txtToplamBedel.Text = "";
            txtProjeAra.Text = "";
            txtProjeNo.Enabled = true;
            chkAltProjeVar.Checked = false;
            chkAltProjeYok.Checked = false;
            chkProjeIliskisiVar.Checked = false;
            chkProjeIliskisiYok.Checked = false;
            chkNakliyeVar.Checked = false;
            chkNakliyeYok.Checked = false;
            chkTekil.Checked = false;
            chkCoklu.Checked = false;
            cmbParaBirimi.SelectedIndex = -1;
            lblAltProjeHata.Text = "";
            lblAltProjeHata.Visible = false;
            chkMontajTamamlandi.Checked = false;
            chkMontajTamamlanmadi.Checked = false;
            flpAltProjeTextBoxes.Controls.Clear();
            flpAltProjeTextBoxes.Controls.Add(new Label
            {
                Text = "Alt Projeler",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding((flpAltProjeTextBoxes.ClientSize.Width - 80) / 2, 0, 0, 5),
                ForeColor = Color.FromArgb(44, 62, 80)
            });
            AddEkleButton(flpAltProjeTextBoxes, "AltProje");
            flpAltProjeTextBoxes.Visible = false;

            flpIliskiTextBoxes.Controls.Clear();
            flpIliskiTextBoxes.Controls.Add(new Label
            {
                Text = "İlişkili Projeler",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding((flpIliskiTextBoxes.ClientSize.Width - 100) / 2, 0, 0, 5),
                ForeColor = Color.FromArgb(44, 62, 80)
            });
            AddEkleButton(flpIliskiTextBoxes, "Iliski");
            flpIliskiTextBoxes.Visible = false;

            UpdateContextMenu();

            UpdatePanelLayout();
            UpdateFlowLayoutPanelHeight(flpAltProjeTextBoxes);
            UpdateFlowLayoutPanelHeight(flpIliskiTextBoxes);
            UpdateAllPanelWidths(flpAltProjeTextBoxes);
            UpdateAllPanelWidths(flpIliskiTextBoxes);
        }
        private void btnStatuBilgisi_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text == "Kapatıldı")
            {
                MessageBox.Show("Proje statüsü kapatıldığı için statü bilgisi mevcut değildir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string projeNo = txtProjeNo.Text.Trim();
                var projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
                if (!projeId.HasValue)
                {
                    MessageBox.Show("Lütfen önce geçerli bir proje numarası ile proje araması yapın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                frmStatuBilgi statuForm = new frmStatuBilgi(projeId.Value, _sayfaStatusService);
                statuForm.ShowDialog();
            }
        }

        private void txtProjeNo_Leave(object sender, EventArgs e)
        {
            var args = e as CancelEventArgs;

            if (!string.IsNullOrWhiteSpace(txtProjeNo.Text))
            {
                string projeNo = txtProjeNo.Text.Trim();

                if (chkAltProjeVar.Checked && !Regex.IsMatch(projeNo, @"^\d{5}$|^\d{5}\.00$"))
                {
                    MessageBox.Show("Alt proje işaretliyken proje numarası 5 haneli bir sayı (örneğin: 12345 veya 12345.00) olmalıdır.",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProjeNo.BackColor = Color.FromArgb(255, 204, 203);
                    txtProjeNo.Focus(); 
                    if (args != null) args.Cancel = true;
                    return;
                }

                string kokNo = projeNo.Length >= 5 ? projeNo.Substring(0, 5) : projeNo;

                try
                {
                    if (!_projeKutukService.ProjeNoKontrol(kokNo))
                    {
                        MessageBox.Show($"Proje numarası '{kokNo}' ile başlayan bir kütük zaten mevcut. Lütfen farklı bir kök numara girin.",
                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtProjeNo.BackColor = Color.FromArgb(255, 204, 203);
                        txtProjeNo.Text = "";
                        txtProjeNo.Focus();
                        if (args != null) args.Cancel = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Proje numarası kontrol edilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProjeNo.Focus();
                    if (args != null) args.Cancel = true;
                    return;
                }

                txtProjeNo.BackColor = Color.White;
            }
        }

        private void btnMektupEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeNo.Text))
            {
                MessageBox.Show("Lütfen önce bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string projeNo = txtProjeNo.Text.Trim();
            int? projeId = _finansProjelerService.GetProjeIdByNo(projeNo);

            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' bulunamadı. Lütfen geçerli bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> altProjeler = chkAltProjeVar.Checked
                ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}" && !string.IsNullOrWhiteSpace(txt.Text))
                    .Select(txt => txt.Text.Trim())
                    .ToList()
                : new List<string>();

            try
            {
                var teminatMektubuForm = new frmTeminatMektubuEkle(
                    musterilerService: _musterilerService,
                    teminatMektup: null, 
                    finansProjelerService: _finansProjelerService,
                    projeKutukService: _projeKutukService,
                    teminatMektuplariService: _teminatMektuplariService,
                    projeId: projeId.Value,
                    kilometreTasiId: 0,
                    tutar: "0.00",
                    altProjeler: altProjeler, 
                    activateTeminatTab: true
                );

                if (teminatMektubuForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Teminat mektubu başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Teminat mektubu eklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}