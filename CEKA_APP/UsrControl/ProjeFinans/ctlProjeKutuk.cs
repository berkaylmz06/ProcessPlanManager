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

        public ctlProjeKutuk()
        {
            InitializeComponent();
            projeBilgileriKaydedildi = false;

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
                            UpdateToplamBedelUI(projeAdi, toplamBedel, eksikFiyatlandirmaProjeler);
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
                            parentFormFiyat.projeFiyatlandirma.LoadProjeFiyatlandirma(form.Tag.ToString(), autoSearch: true, altProjeler: altProjeler);
                            parentFormFiyat.NavigateToUserControl(parentFormFiyat.projeFiyatlandirma);
                        }
                    }
                }
                else
                {
                    parentFormFiyat.projeFiyatlandirma.LoadProjeFiyatlandirma(projeNo, autoSearch: true, altProjeler: altProjeler);
                    parentFormFiyat.NavigateToUserControl(parentFormFiyat.projeFiyatlandirma);
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
            chkEuro.CheckedChanged += (s, e) =>
            {
                if (chkEuro.Checked)
                {
                    chkDolar.Checked = false;
                    chkTL.Checked = false;
                }
            };
            chkDolar.CheckedChanged += (s, e) =>
            {
                if (chkDolar.Checked)
                {
                    chkEuro.Checked = false;
                    chkTL.Checked = false;
                }
            };
            chkTL.CheckedChanged += (s, e) =>
            {
                if (chkTL.Checked)
                {
                    chkEuro.Checked = false;
                    chkDolar.Checked = false;
                }
            };

            txtProjeNo.TextChanged += (s, e) => UpdateContextMenu();
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
                var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(projeNo);
                if (projeKutuk != null && projeKutuk.altProjeVarMi)
                {
                    if (eksikFiyatlandirmaProjeler.Any())
                    {
                        txtToplamBedel.Text = toplamBedel.ToString("F2");
                        lblAltProjeHata.Text = $"Alt projeler: {string.Join(", ", eksikFiyatlandirmaProjeler)} için fiyatlandırma yok";
                        lblAltProjeHata.ForeColor = Color.Red;
                    }
                    else
                    {
                        txtToplamBedel.Text = toplamBedel.ToString("F2");
                        lblAltProjeHata.Text = "";
                        lblAltProjeHata.ForeColor = Color.Black;
                    }
                }
                else
                {
                    if (eksikFiyatlandirmaProjeler.Any() && eksikFiyatlandirmaProjeler.Contains(projeNo))
                    {
                        txtToplamBedel.Text = toplamBedel.ToString("F2");
                        lblAltProjeHata.Text = $"{projeNo} için fiyatlandırma bilgisi yok.";
                        lblAltProjeHata.ForeColor = Color.Red;
                    }
                    else
                    {
                        txtToplamBedel.Text = toplamBedel.ToString("F2");
                        lblAltProjeHata.Text = "";
                        lblAltProjeHata.ForeColor = Color.Black;
                    }
                }
            }
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

                    AddEkleButton(flp, prefix);
                    var btnEkle = flp.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "Ekle");
                    if (btnEkle != null)
                    {
                        flp.Controls.SetChildIndex(btnEkle, flp.Controls.Count - 1);
                    }

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
                chkEuro.Checked = proje.paraBirimi == "EUR";
                chkDolar.Checked = proje.paraBirimi == "USD";
                chkTL.Checked = proje.paraBirimi == "TL";

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
                else
                {
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
                            FlatStyle = FlatStyle.Flat,
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
                else
                {
                    AddEkleButton(flpIliskiTextBoxes, "Iliski");
                }
                flpIliskiTextBoxes.Visible = proje.digerProjeIliskisiVarMi != "0";

                UpdateContextMenu();
                UpdatePanelLayout();

                var fiyatlandirmaData = new ProjeFinans_FiyatlandirmaData();
                var altProjeler = chkAltProjeVar.Checked
                    ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                        .Select(txt => txt.Text.Trim())
                        .ToList()
                    : null;
                var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(projeNo, altProjeler);
                UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler);
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

            decimal bedel = decimal.TryParse(txtToplamBedel.Text.Trim(), out var parsedBedel) ? parsedBedel : 0;

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

            if (!chkTekil.Checked && !chkCogul.Checked)
            {
                hataMesajlari.Add("Ödeme şekli (Tekil veya Çoğul) seçilmelidir.");
            }

            if (!chkEuro.Checked && !chkDolar.Checked && !chkTL.Checked)
            {
                hataMesajlari.Add("Para birimi (EUR, USD veya TL) seçilmelidir.");
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
                    : new List<string>(),
                faturalamaSekli = chkTekil.Checked ? "Tekil" : "Cogul",
                paraBirimi = chkEuro.Checked ? "EUR" : chkDolar.Checked ? "USD" : "TL"
            };

            string proje = txtProjeNo.Text.Trim();

            if (ProjeFinans_ProjeKutukData.ProjeKutukAra(proje) != null)
            {
                MessageBox.Show($"Proje No '{proje}' zaten kayıtlı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ProjeFinans_ProjeKutukData.ProjeKutukEkle(kutuk))
            {
                if (chkAltProjeVar.Checked && kutuk.altProjeBilgileri.Any())
                {
                    foreach (var altProje in kutuk.altProjeBilgileri)
                    {
                        if (!ProjeFinans_ProjeKutukData.AltProjeEkle(proje, altProje))
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
                var altProjeler = chkAltProjeVar.Checked
                    ? flpAltProjeTextBoxes.Controls.OfType<Panel>()
                        .SelectMany(p => p.Controls.OfType<TextBox>())
                        .Where(txt => txt.Text != $"Proje #{txt.Name.Split('_').Last()}")
                        .Select(txt => txt.Text.Trim())
                        .ToList()
                    : null;
                var (toplamBedel, eksikFiyatlandirmaProjeler) = fiyatlandirmaData.GetToplamBedel(proje, altProjeler);
                UpdateToplamBedelUI(proje, toplamBedel, eksikFiyatlandirmaProjeler);
            }
            else
            {
                MessageBox.Show("Kayıt eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}