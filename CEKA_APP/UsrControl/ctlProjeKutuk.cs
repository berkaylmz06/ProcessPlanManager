using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public ctlProjeKutuk()
        {
            InitializeComponent();
            projeBilgileriKaydedildi = false;

            // ContextMenu ayarları
            contextMenu = new ContextMenuStrip();
            mnuProjeFiyatlandirma = new ToolStripMenuItem("Proje Fiyatlandırma") { Enabled = false };
            mnuProjeBilgileri = new ToolStripMenuItem("Proje Bilgileri") { Enabled = false };
            contextMenu.Items.Add(mnuProjeFiyatlandirma);
            contextMenu.Items.Add(mnuProjeBilgileri);
            this.ContextMenuStrip = contextMenu;

            // ContextMenu olayları
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

                var parentForm = this.FindForm() as frmAnaSayfa;
                if (parentForm == null)
                {
                    MessageBox.Show("Ana form bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (parentForm.projeFiyatlandirma == null)
                {
                    parentForm.projeFiyatlandirma = new ctlProjeFiyatlandirma();
                    parentForm.projeFiyatlandirma.Dock = DockStyle.Fill;
                }

                parentForm.projeFiyatlandirma.LoadProjeFiyatlandirma(projeNo, autoSearch: true);
                parentForm.NavigateToUserControl(parentForm.projeFiyatlandirma);
            };

            // FlowLayoutPanel ayarları
            flpAltProjeTextBoxes = new FlowLayoutPanel
            {
                AutoSize = false,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                AutoScrollMinSize = new Size(0, 0),
                Height = 150,
                Width = 200,
                Visible = false,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };
            flpIliskiTextBoxes = new FlowLayoutPanel
            {
                AutoSize = false,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                AutoScrollMinSize = new Size(0, 0),
                Height = 150,
                Width = 200,
                Visible = false,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };

            flpAltProjeTextBoxes.Location = new Point(txtProjeNo.Location.X + txtProjeNo.Width + 10, txtProjeNo.Location.Y + txtProjeNo.Height - 10);
            flpIliskiTextBoxes.Location = new Point(txtProjeNo.Location.X + txtProjeNo.Width + 10, txtProjeNo.Location.Y + txtProjeNo.Height - 10);

            var lblAltProjeler = new Label
            {
                Text = "Alt Projeler",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding((flpAltProjeTextBoxes.Width - 80) / 2, 0, 0, 5),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            flpAltProjeTextBoxes.Controls.Add(lblAltProjeler);

            var lblIliskiProjeler = new Label
            {
                Text = "İlişkili Projeler",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding((flpIliskiTextBoxes.Width - 100) / 2, 0, 0, 5),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            flpIliskiTextBoxes.Controls.Add(lblIliskiProjeler);

            AddEkleButton(flpAltProjeTextBoxes, "AltProje");
            AddEkleButton(flpIliskiTextBoxes, "Iliski");

            this.Controls.Add(flpAltProjeTextBoxes);
            this.Controls.Add(flpIliskiTextBoxes);

            // Checkbox olayları
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

            txtProjeNo.TextChanged += (s, e) => UpdateContextMenu();
        }

        private void CheckBoxKontrol(CheckBox aktif, CheckBox pasif, Control ilgiliKontrol, string prefix, bool goster = true)
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
                if (ilgiliKontrol is FlowLayoutPanel flp)
                {
                    flp.Controls.Clear();
                    flp.Controls.Add(aktif.Name == "chkAltProjeVar"
                        ? new Label { Text = "Alt Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.Width - 80) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) }
                        : new Label { Text = "İlişkili Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.Width - 100) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) });
                    AddEkleButton(flp, prefix);
                    // İlk TextBox'ı otomatik ekle (çarpı butonu olmadan)
                    int index = flp.Controls.OfType<TextBox>().Count() + 1;
                    var txt = new TextBox
                    {
                        Width = flp.Width - 80,
                        Height = 20,
                        AutoSize = false,
                        Margin = new Padding(5, 5, 5, 5),
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
                    var panel = new Panel
                    {
                        Width = flp.Width - 10,
                        Height = 25,
                        Margin = new Padding(5, 5, 5, 5)
                    };
                    panel.Controls.Add(txt);
                    txt.Location = new Point(0, 0);
                    flp.Controls.Add(panel);
                    flp.Visible = true;
                    flp.BringToFront();
                    txt.Focus();
                    txt.Select();
                }
            }
            else
            {
                if (ilgiliKontrol is FlowLayoutPanel flp)
                {
                    flp.Controls.Clear();
                    flp.Controls.Add(aktif.Name == "chkAltProjeVar"
                        ? new Label { Text = "Alt Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.Width - 80) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) }
                        : new Label { Text = "İlişkili Projeler", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding((flp.Width - 100) / 2, 0, 0, 5), ForeColor = Color.FromArgb(44, 62, 80) });
                    AddEkleButton(flp, prefix);
                    flp.Visible = false;
                }
            }
            UpdatePanelLayout();
            UpdateContextMenu();
        }

        private void AddEkleButton(FlowLayoutPanel flp, string prefix)
        {
            var btnEkle = new Button
            {
                Text = "Ekle",
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Width = flp.Width - 40,
                Height = 25,
                Margin = new Padding((flp.Width - (flp.Width - 40)) / 2, 5, 5, 5),
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            btnEkle.Click += (s, e) =>
            {
                int index = flp.Controls.OfType<Panel>().SelectMany(p => p.Controls.OfType<TextBox>()).Count() + 1;
                var txt = new TextBox
                {
                    Width = flp.Width - 80,
                    Height = 20,
                    AutoSize = false,
                    Margin = new Padding(0, 0, 0, 0),
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
                var btnKaldir = new Button
                {
                    Text = "X",
                    Width = 20,
                    Height = 20,
                    Margin = new Padding(0, 0, 0, 0),
                    BackColor = Color.FromArgb(231, 76, 60),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Font = new Font("Segoe UI", 8, FontStyle.Bold)
                };
                btnKaldir.Click += (s2, e2) =>
                {
                    flp.Controls.Remove(txt.Parent);
                    UpdateContextMenu();
                };
                var panel = new Panel
                {
                    Width = flp.Width - 10,
                    Height = 25,
                    Margin = new Padding(5, 5, 5, 5)
                };
                panel.Controls.Add(txt);
                panel.Controls.Add(btnKaldir);
                txt.Location = new Point(0, 0);
                btnKaldir.Location = new Point(txt.Width + 5, 0);
                flp.Controls.Add(panel);
                flp.Controls.SetChildIndex(btnEkle, flp.Controls.Count - 1);
                txt.Focus();
                txt.Select();
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
            bool isProjeKayitli = !string.IsNullOrWhiteSpace(txtProjeNo.Text.Trim()) && ProjeKutukData.ProjeKutukAra(txtProjeNo.Text.Trim()) != null;

            if (chkAltProjeVar.Checked)
            {
                var altProjeler = flpAltProjeTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                    .Select(txt => txt.Text.Trim()).ToList();
                mnuProjeBilgileri.Enabled = altProjeler.Any();
                mnuProjeFiyatlandirma.Enabled = isProjeKayitli;
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
            var proje = ProjeKutukData.ProjeKutukAra(projeNo);

            if (proje != null)
            {
                txtMusteriNo.Text = proje.musteriNo ?? "";
                txtMusteriAdi.Text = proje.musteriAdi ?? "";
                txtTeklifNo.Text = proje.teklifNo ?? "";
                txtIsFirsatiNo.Text = proje.isFirsatiNo ?? "";
                txtProjeNo.Text = proje.projeNo ?? "";
                chkAltProjeVar.Checked = proje.altProjeVarMi;
                chkProjeIliskisiVar.Checked = proje.digerProjeIliskisiVarMi != "0";
                dtpSiparisSozlesmeTarihi.Value = proje.siparisSozlesmeTarihi;
                txtToplamBedel.Text = proje.toplamBedel.ToString();
                chkNakliyeVar.Checked = proje.nakliyeVarMi;

                flpAltProjeTextBoxes.Controls.Clear();
                flpAltProjeTextBoxes.Controls.Add(new Label
                {
                    Text = "Alt Projeler",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding((flpAltProjeTextBoxes.Width - 80) / 2, 0, 0, 5),
                    ForeColor = Color.FromArgb(44, 62, 80)
                });
                if (proje.altProjeVarMi && proje.altProjeBilgileri != null)
                {
                    int index = 1;
                    foreach (var altProje in proje.altProjeBilgileri)
                    {
                        var txt = new TextBox
                        {
                            Width = flpAltProjeTextBoxes.Width - 80,
                            Height = 20,
                            Margin = new Padding(0, 0, 0, 0),
                            Name = $"txt_AltProje_{index}",
                            Text = altProje,
                            ForeColor = Color.Black,
                            BorderStyle = BorderStyle.FixedSingle,
                            BackColor = Color.White
                        };
                        txt.TextChanged += (s2, e2) => UpdateContextMenu();
                        var btnKaldir = new Button
                        {
                            Text = "X",
                            Width = 20,
                            Height = 20,
                            Margin = new Padding(0, 0, 0, 0),
                            BackColor = Color.FromArgb(231, 76, 60),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat,
                            FlatAppearance = { BorderSize = 0 },
                            Font = new Font("Segoe UI", 8, FontStyle.Bold)
                        };
                        btnKaldir.Click += (s2, e2) =>
                        {
                            flpAltProjeTextBoxes.Controls.Remove(txt.Parent);
                            UpdateContextMenu();
                        };
                        var panel = new Panel
                        {
                            Width = flpAltProjeTextBoxes.Width - 10,
                            Height = 25,
                            Margin = new Padding(5, 5, 5, 5)
                        };
                        panel.Controls.Add(txt);
                        panel.Controls.Add(btnKaldir);
                        txt.Location = new Point(0, 0);
                        btnKaldir.Location = new Point(txt.Width + 5, 0);
                        flpAltProjeTextBoxes.Controls.Add(panel);
                        index++;
                    }
                    AddEkleButton(flpAltProjeTextBoxes, "AltProje");
                }
                flpAltProjeTextBoxes.Visible = proje.altProjeVarMi;

                flpIliskiTextBoxes.Controls.Clear();
                flpIliskiTextBoxes.Controls.Add(new Label
                {
                    Text = "İlişkili Projeler",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding((flpIliskiTextBoxes.Width - 100) / 2, 0, 0, 5),
                    ForeColor = Color.FromArgb(44, 62, 80)
                });
                if (proje.digerProjeIliskisiVarMi != "0" && !string.IsNullOrEmpty(proje.digerProjeIliskisiVarMi))
                {
                    int index = 1;
                    var iliskiProjeler = proje.digerProjeIliskisiVarMi.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var iliskiProje in iliskiProjeler)
                    {
                        var txt = new TextBox
                        {
                            Width = flpIliskiTextBoxes.Width - 80,
                            Height = 20,
                            Margin = new Padding(0, 0, 0, 0),
                            Name = $"txt_Iliski_{index}",
                            Text = iliskiProje.Trim(),
                            ForeColor = Color.Black,
                            BorderStyle = BorderStyle.FixedSingle,
                            BackColor = Color.White
                        };
                        txt.TextChanged += (s2, e2) => UpdateContextMenu();
                        var btnKaldir = new Button
                        {
                            Text = "X",
                            Width = 20,
                            Height = 20,
                            Margin = new Padding(0, 0, 0, 0),
                            BackColor = Color.FromArgb(231, 76, 60),
                            ForeColor = Color.White,
                            FlatAppearance = { BorderSize = 0 },
                            Font = new Font("Segoe UI", 8, FontStyle.Bold)
                        };
                        btnKaldir.Click += (s2, e2) =>
                        {
                            flpIliskiTextBoxes.Controls.Remove(txt.Parent);
                            UpdateContextMenu();
                        };
                        var panel = new Panel
                        {
                            Width = flpIliskiTextBoxes.Width - 10,
                            Height = 25,
                            Margin = new Padding(5, 5, 5, 5)
                        };
                        panel.Controls.Add(txt);
                        panel.Controls.Add(btnKaldir);
                        txt.Location = new Point(0, 0);
                        btnKaldir.Location = new Point(txt.Width + 5, 0);
                        flpIliskiTextBoxes.Controls.Add(panel);
                        index++;
                    }
                    AddEkleButton(flpIliskiTextBoxes, "Iliski");
                }
                flpIliskiTextBoxes.Visible = proje.digerProjeIliskisiVarMi != "0";

                UpdateContextMenu();
                UpdatePanelLayout();
                MessageBox.Show("Proje bulundu ve bilgiler yüklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                string projeNo = txtProjeNo.Text.Trim();
                if (!Regex.IsMatch(projeNo, @"^\d{5}$|^\d{5}\.00$"))
                    hataMesajlari.Add("Proje No 5 haneli bir sayı (ör: 12345 veya 12345.00) olmalıdır.");
            }

            if (!decimal.TryParse(txtToplamBedel.Text.Trim(), out var bedel) || bedel < 0)
                hataMesajlari.Add("Toplam Bedel geçerli bir sayı olmalı ve negatif olamaz.");

            if (chkAltProjeVar.Checked)
            {
                var altProjeler = flpAltProjeTextBoxes.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TextBox>())
                    .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                    .Select(txt => txt.Text.Trim()).ToList();
                if (!altProjeler.Any())
                {
                    lblAltProjeHata.Text = "Alt proje var işaretlediniz fakat alt proje bilgisi girmediniz.";
                    lblAltProjeHata.Visible = true;
                    hataMesajlari.Add("Alt proje var işaretlendiniz fakat alt proje bilgisi girmediniz.");
                }
                else if (!projeBilgileriKaydedildi)
                {
                    hataMesajlari.Add("Alt projeler için proje bilgileri kaydedilmedi. Lütfen önce Proje Bilgileri'ni kaydedin.");
                }
                else
                {
                    lblAltProjeHata.Visible = false;
                }
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

            if (hataMesajlari.Any())
            {
                MessageBox.Show(string.Join("\n", hataMesajlari), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var kutuk = new ProjeKutuk
            {
                musteriNo = txtMusteriNo.Text.Trim(),
                musteriAdi = txtMusteriAdi.Text.Trim(),
                teklifNo = txtTeklifNo.Text.Trim(),
                isFirsatiNo = txtIsFirsatiNo.Text.Trim(),
                projeNo = txtProjeNo.Text.Trim(),
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
                altProjeBilgileri = chkAltProjeVar.Checked
                    ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                        .Select(txt => txt.Text.Trim()).ToList()
                    : new List<string>()
            };

            string proje = txtProjeNo.Text.Trim();

            if (ProjeKutukData.ProjeKutukAra(proje) != null)
            {
                MessageBox.Show($"Proje No '{proje}' zaten kayıtlı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ProjeKutukData.ProjeKutukEkle(kutuk))
            {
                if (chkAltProjeVar.Checked && kutuk.altProjeBilgileri.Any())
                {
                    foreach (var altProje in kutuk.altProjeBilgileri)
                    {
                        if (!ProjeKutukData.AltProjeEkle(proje, altProje))
                        {
                            MessageBox.Show($"Alt Proje '{altProje}' için ilişki kaydı eklenirken hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show("Kayıt başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                projeBilgileriKaydedildi = false;
                UpdateContextMenu();
            }
            else
            {
                MessageBox.Show("Kayıt eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}