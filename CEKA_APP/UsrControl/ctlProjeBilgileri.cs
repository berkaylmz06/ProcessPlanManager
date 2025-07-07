using CEKA_APP.DataBase;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeBilgileri : UserControl
    {
        public event Action OnKaydet;
        private const string PlaceholderText = "Açıklama girin";
        private const string ProjeAdiPlaceholder = "Proje adı girin";
        private bool hasNewData;
        private Dictionary<string, (TextBox txtProjeAdi, DateTimePicker dtpOlusturmaTarihi, TextBox txtAciklama)> projeInputs;

        public ctlProjeBilgileri()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            hasNewData = false;
            projeInputs = new Dictionary<string, (TextBox, DateTimePicker, TextBox)>();
        }

        public void LoadProjects(List<string> projects, string anaProjeNo = null)
        {
            panelProjeler.Controls.Clear();
            hasNewData = false;
            projeInputs.Clear();
            panelProjeler.AutoScroll = true; // AutoScroll özelliğini aç

            // Tümünü Kaydet butonu
            var btnKaydetHepsi = new Button
            {
                Text = "Tümünü Kaydet",
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnKaydetHepsi.MouseEnter += (s, e) => btnKaydetHepsi.BackColor = Color.FromArgb(39, 174, 96);
            btnKaydetHepsi.MouseLeave += (s, e) => btnKaydetHepsi.BackColor = Color.FromArgb(46, 204, 113);
            btnKaydetHepsi.Click += BtnKaydetHepsi_Click;

            if (!string.IsNullOrEmpty(anaProjeNo) && projects.Any())
            {
                var anaProjeBilgi = ProjeKutukData.GetProjeBilgileri(anaProjeNo);
                var anaKart = CreateProjectCard(anaProjeNo, anaProjeBilgi, true);
                anaKart.Location = new Point(20, 20);
                panelProjeler.Controls.Add(anaKart);

                var altProjelerPanel = new Panel
                {
                    AutoSize = false,
                    Size = new Size(panelProjeler.Width - 40, 300),
                    Location = new Point(20, 320),
                    Visible = false,
                    BackColor = Color.FromArgb(240, 240, 240)
                };

                var lblOk = new Label
                {
                    Text = "▶",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(280, 20),
                    ForeColor = Color.FromArgb(44, 62, 80),
                    BackColor = Color.Transparent
                };
                anaKart.Controls.Add(lblOk);

                anaKart.Click += (s, e) =>
                {
                    altProjelerPanel.Visible = !altProjelerPanel.Visible;
                    lblOk.Text = altProjelerPanel.Visible ? "▼" : "▶";
                    CopyToSubProjects(anaProjeNo, altProjelerPanel);
                    var timer = new Timer { Interval = 10 };
                    int targetHeight = altProjelerPanel.Visible ? 300 : 0;
                    int startHeight = altProjelerPanel.Height;
                    int step = (targetHeight - startHeight) / 20;
                    timer.Tick += (t, te) =>
                    {
                        startHeight += step;
                        altProjelerPanel.Height = Math.Max(0, Math.Min(targetHeight, startHeight));
                        if (altProjelerPanel.Height == targetHeight || altProjelerPanel.Height == 0)
                            timer.Stop();
                    };
                    timer.Start();
                };

                int xOffset = 20;
                foreach (var proje in projects)
                {
                    var projeBilgi = ProjeKutukData.GetProjeBilgileri(proje);
                    var card = CreateProjectCard(proje, projeBilgi, false);
                    card.Location = new Point(xOffset, 20);
                    altProjelerPanel.Controls.Add(card);
                    xOffset += card.Width + 20;
                }

                panelProjeler.Controls.Add(altProjelerPanel);
                CopyToSubProjects(anaProjeNo, altProjelerPanel);
            }
            else
            {
                int xOffset = 20;
                foreach (var proje in projects)
                {
                    var projeBilgi = ProjeKutukData.GetProjeBilgileri(proje);
                    var card = CreateProjectCard(proje, projeBilgi, false);
                    card.Location = new Point(xOffset, 20);
                    panelProjeler.Controls.Add(card);
                    xOffset += card.Width + 20;
                }
            }

            // Butonu panelin sonuna ekle ve görünür alanda olmasını sağla
            int yPosition = panelProjeler.Controls.OfType<Control>().Any()
                ? panelProjeler.Controls.OfType<Control>().Max(c => c.Bottom) + 20
                : 20;
            btnKaydetHepsi.Location = new Point(20, yPosition);
            panelProjeler.Controls.Add(btnKaydetHepsi);
            panelProjeler.ScrollControlIntoView(btnKaydetHepsi); // Butonu görünür alana kaydır
            panelProjeler.Refresh(); // Paneli yeniden çiz
        }

        private CustomPanel CreateProjectCard(string projeNo, ProjeBilgi projeBilgi, bool isAnaProje)
        {
            var card = new CustomPanel(Color.FromArgb(240, 240, 240), Color.FromArgb(245, 245, 245))
            {
                Size = new Size(320, isAnaProje ? 280 : 240),
                Margin = new Padding(15),
                Tag = projeNo
            };
            card.MouseEnter += (s, e) => card.StartHoverTransition(true);
            card.MouseLeave += (s, e) => card.StartHoverTransition(false);

            var contextMenu = new ContextMenuStrip();
            var menuFiyatlandirma = new ToolStripMenuItem("Fiyatlandırma Ekle");
            menuFiyatlandirma.Click += (s, e) => AcFiyatlandirmaFormu(projeNo);
            contextMenu.Items.Add(menuFiyatlandirma);
            card.ContextMenuStrip = contextMenu;

            var lblProje = new Label
            {
                Text = isAnaProje ? $"Ana Proje: {projeNo}" : projeNo,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20),
                ForeColor = Color.FromArgb(44, 62, 80),
                BackColor = Color.Transparent // Şeffaf arka plan
            };

            var lblProjeAdi = new Label
            {
                Text = "Proje Adı:",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(20, 50),
                ForeColor = Color.FromArgb(44, 62, 80),
                BackColor = Color.Transparent // Şeffaf arka plan
            };

            var txtProjeAdi = new TextBox
            {
                Text = projeBilgi?.ProjeAdi ?? ProjeAdiPlaceholder,
                ForeColor = projeBilgi?.ProjeAdi != null ? Color.Black : Color.Gray,
                Size = new Size(260, 21),
                Location = new Point(20, 70),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250),
                Font = new Font("Segoe UI", 9)
            };
            txtProjeAdi.TextChanged += (s, e) =>
            {
                hasNewData = true;
                if (projeInputs.ContainsKey(projeNo))
                    CopyToSubProjects(projeNo, panelProjeler.Controls.OfType<Panel>().FirstOrDefault(p => !p.Visible));
            };
            txtProjeAdi.Enter += (s, e) =>
            {
                if (txtProjeAdi.Text == ProjeAdiPlaceholder)
                {
                    txtProjeAdi.Text = "";
                    txtProjeAdi.ForeColor = Color.Black;
                }
            };
            txtProjeAdi.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtProjeAdi.Text))
                {
                    txtProjeAdi.Text = ProjeAdiPlaceholder;
                    txtProjeAdi.ForeColor = Color.Gray;
                }
            };

            var lblOlusturmaTarihi = new Label
            {
                Text = "Oluşturma Tarihi:",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(20, 110),
                ForeColor = Color.FromArgb(44, 62, 80),
                BackColor = Color.Transparent // Şeffaf arka plan
            };

            var dtpOlusturmaTarihi = new DateTimePicker
            {
                Size = new Size(260, 20),
                Location = new Point(20, 130),
                Format = DateTimePickerFormat.Long,
                Value = projeBilgi?.OlusturmaTarihi ?? DateTime.Now,
                BackColor = Color.FromArgb(250, 250, 250),
                Font = new Font("Segoe UI", 9)
            };
            dtpOlusturmaTarihi.ValueChanged += (s, e) =>
            {
                hasNewData = true;
                if (projeInputs.ContainsKey(projeNo))
                    CopyToSubProjects(projeNo, panelProjeler.Controls.OfType<Panel>().FirstOrDefault(p => !p.Visible));
            };

            var lblAciklama = new Label
            {
                Text = "Açıklama:",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(20, 170),
                ForeColor = Color.FromArgb(44, 62, 80),
                BackColor = Color.Transparent // Şeffaf arka plan
            };

            var txtAciklama = new TextBox
            {
                Text = projeBilgi?.Aciklama ?? PlaceholderText,
                ForeColor = projeBilgi?.Aciklama != null ? Color.Black : Color.Gray,
                Size = new Size(260, 21),
                Location = new Point(20, 190),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250),
                Font = new Font("Segoe UI", 9)
            };
            txtAciklama.TextChanged += (s, e) =>
            {
                hasNewData = true;
                if (projeInputs.ContainsKey(projeNo))
                    CopyToSubProjects(projeNo, panelProjeler.Controls.OfType<Panel>().FirstOrDefault(p => !p.Visible));
            };
            txtAciklama.Enter += (s, e) =>
            {
                if (txtAciklama.Text == PlaceholderText)
                {
                    txtAciklama.Text = "";
                    txtAciklama.ForeColor = Color.Black;
                }
            };
            txtAciklama.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtAciklama.Text))
                {
                    txtAciklama.Text = PlaceholderText;
                    txtAciklama.ForeColor = Color.Gray;
                }
            };

            card.Controls.Add(lblProje);
            card.Controls.Add(lblProjeAdi);
            card.Controls.Add(txtProjeAdi);
            card.Controls.Add(lblOlusturmaTarihi);
            card.Controls.Add(dtpOlusturmaTarihi);
            card.Controls.Add(lblAciklama);
            card.Controls.Add(txtAciklama);

            projeInputs[projeNo] = (txtProjeAdi, dtpOlusturmaTarihi, txtAciklama);

            return card;
        }

        private void CopyToSubProjects(string anaProjeNo, Panel altProjelerPanel)
        {
            if (projeInputs.TryGetValue(anaProjeNo, out var anaInputs) && altProjelerPanel != null)
            {
                string projeAdi = anaInputs.txtProjeAdi.Text == ProjeAdiPlaceholder ? "" : anaInputs.txtProjeAdi.Text;
                string aciklama = anaInputs.txtAciklama.Text == PlaceholderText ? "" : anaInputs.txtAciklama.Text;
                DateTime tarih = anaInputs.dtpOlusturmaTarihi.Value;

                foreach (Control ctrl in altProjelerPanel.Controls)
                {
                    if (ctrl is CustomPanel card && projeInputs.TryGetValue(card.Tag.ToString(), out var subInputs))
                    {
                        if (subInputs.txtProjeAdi.Text == ProjeAdiPlaceholder || subInputs.txtProjeAdi.Text != projeAdi)
                            subInputs.txtProjeAdi.Text = projeAdi;
                        if (subInputs.txtAciklama.Text == PlaceholderText || subInputs.txtAciklama.Text != aciklama)
                            subInputs.txtAciklama.Text = aciklama;
                        subInputs.dtpOlusturmaTarihi.Value = tarih;
                        hasNewData = true;
                    }
                }
            }
        }

        private void BtnKaydetHepsi_Click(object sender, EventArgs e)
        {
            if (!hasNewData)
            {
                MessageBox.Show("Herhangi bir değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool anySuccess = false;
            foreach (var kvp in projeInputs)
            {
                string projeNo = kvp.Key;
                var (txtProjeAdi, dtpOlusturmaTarihi, txtAciklama) = kvp.Value;

                string projeAdi = txtProjeAdi.Text == ProjeAdiPlaceholder ? "" : txtProjeAdi.Text;
                string aciklama = txtAciklama.Text == PlaceholderText ? "" : txtAciklama.Text;
                var projeBilgi = ProjeKutukData.GetProjeBilgileri(projeNo);
                bool success;

                if (projeBilgi != null)
                {
                    success = ProjeKutukData.UpdateProjeFinans(projeNo, aciklama, projeAdi, dtpOlusturmaTarihi.Value);
                }
                else
                {
                    success = ProjeKutukData.ProjeEkleProjeFinans(projeNo, aciklama, projeAdi, dtpOlusturmaTarihi.Value);
                }

                if (success)
                {
                    anySuccess = true;
                }
            }

            if (anySuccess)
            {
                hasNewData = false;
                OnKaydet?.Invoke();
                MessageBox.Show("Değişiklikler kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Kayıt sırasında hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AcFiyatlandirmaFormu(string projeNo)
        {
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

            parentForm.projeFiyatlandirma.LoadProjeFiyatlandirma(projeNo);
            parentForm.NavigateToUserControl(parentForm.projeFiyatlandirma);
        }
    }
}