using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
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
        private const string PlaceholderText = "Açıklama girin...";
        private const string ProjeAdiPlaceholder = "Proje adı girin...";
        private bool hasNewData;
        private Dictionary<string, (TextBox txtProjeAdi, DateTimePicker dtpOlusturmaTarihi, TextBox txtAciklama)> projeInputs;

        // "Tümünü Kaydet" butonu için sınıf düzeyinde bir alan tanımlandı
        private Button btnKaydetHepsi;

        // Daha Zengin Renk Paleti
        private static class AppColors
        {
            public static readonly Color PrimaryColor = Color.FromArgb(39, 55, 70); // Koyu Lacivert Gri
            public static readonly Color SecondaryColor = Color.FromArgb(25, 118, 210); // Koyu Mavi
            public static readonly Color SuccessColor = Color.FromArgb(67, 160, 71); // Koyu Yeşil
            public static readonly Color WarningColor = Color.FromArgb(255, 152, 0); // Turuncu
            public static readonly Color BackgroundColor = Color.FromArgb(245, 248, 250); // Çok Açık Gri (Kirli Beyaz)
            public static readonly Color CardColor = Color.FromArgb(227, 242, 253); // Açık Gökyüzü Mavisi
            public static readonly Color CardHoverColor = Color.FromArgb(207, 231, 246); // Açık Gökyüzü Mavisinin Bir Ton Koyu Hali
        }

        private static class AppFonts
        {
            public static readonly Font TitleFont = new Font("Arial", 14, FontStyle.Bold);
            public static readonly Font SubtitleFont = new Font("Arial", 10, FontStyle.Regular);
            public static readonly Font ButtonFont = new Font("Arial", 10, FontStyle.Bold);
            public static readonly Font LabelFont = new Font("Arial", 9, FontStyle.Regular);
            public static readonly Font InputFont = new Font("Arial", 9, FontStyle.Regular);
        }

        public ctlProjeBilgileri()
        {
            InitializeComponent();
            DoubleBuffered = true;
            hasNewData = false;
            projeInputs = new Dictionary<string, (TextBox, DateTimePicker, TextBox)>();
            this.BackColor = AppColors.BackgroundColor;

            // Butonun oluşturulması ve ana kontrolün altına sabitlenmesi
            btnKaydetHepsi = CreateStyledButton("Tümünü Kaydet", AppColors.SuccessColor, AppColors.SuccessColor);
            btnKaydetHepsi.Click += BtnKaydetHepsi_Click;
            btnKaydetHepsi.Dock = DockStyle.Bottom; // Butonu en alta sabitle
            this.Controls.Add(btnKaydetHepsi);

            // panelProjeler'in butonun üzerinde kalan alanı doldurması sağlandı
            panelProjeler.Dock = DockStyle.Fill;
            this.Controls.SetChildIndex(panelProjeler, 0); // panelProjeler'i en arkaya taşı
        }

        public void LoadProjects(List<string> projects, string anaProjeNo = null)
        {
            panelProjeler.Controls.Clear();
            hasNewData = false;
            projeInputs.Clear();
            panelProjeler.AutoScroll = true;

            int yPosition = 20;

            if (!string.IsNullOrEmpty(anaProjeNo) && projects.Any())
            {
                var anaProjeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(anaProjeNo);
                var anaKart = CreateProjectCard(anaProjeNo, anaProjeBilgi, true);
                anaKart.Location = new Point(20, yPosition);
                panelProjeler.Controls.Add(anaKart);
                yPosition = anaKart.Bottom + 20;

                var altProjelerPanel = new Panel
                {
                    AutoSize = false,
                    Size = new Size(panelProjeler.Width - 40, 0),
                    Location = new Point(20, yPosition),
                    Visible = true,
                    BackColor = AppColors.BackgroundColor,
                    AutoScroll = true,
                    Tag = "altProjelerPanel"
                };

                var lblOk = new Label
                {
                    Text = "►",
                    Font = AppFonts.TitleFont,
                    AutoSize = true,
                    Location = new Point(anaKart.Width - 40, 20),
                    ForeColor = AppColors.PrimaryColor,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand
                };
                anaKart.Controls.Add(lblOk);

                lblOk.Click += (s, e) => {
                    bool isPanelVisible = altProjelerPanel.Height > 0;
                    lblOk.Text = isPanelVisible ? "►" : "▼";
                    if (!isPanelVisible)
                    {
                        CopyToSubProjects(anaProjeNo, altProjelerPanel);
                    }
                    AnimatePanelHeight(altProjelerPanel, isPanelVisible ? 0 : 300);
                };

                anaKart.Click += (s, e) =>
                {
                    bool isPanelVisible = altProjelerPanel.Height > 0;
                    lblOk.Text = isPanelVisible ? "►" : "▼";

                    if (!isPanelVisible)
                    {
                        CopyToSubProjects(anaProjeNo, altProjelerPanel);
                    }

                    AnimatePanelHeight(altProjelerPanel, isPanelVisible ? 0 : 300);
                };

                int xOffset = 20;
                foreach (var proje in projects)
                {
                    var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(proje);
                    var card = CreateProjectCard(proje, projeBilgi, false);
                    card.Location = new Point(xOffset, 20);
                    altProjelerPanel.Controls.Add(card);
                    xOffset += card.Width + 20;
                }

                panelProjeler.Controls.Add(altProjelerPanel);
            }
            else
            {
                int xOffset = 20;
                foreach (var proje in projects)
                {
                    var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(proje);
                    var card = CreateProjectCard(proje, projeBilgi, false);
                    card.Location = new Point(xOffset, yPosition);
                    panelProjeler.Controls.Add(card);
                    xOffset += card.Width + 20;
                }
            }
        }

        private Button CreateStyledButton(string text, Color backColor, Color hoverColor)
        {
            var btn = new Button
            {
                Text = text,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseDownBackColor = backColor, MouseOverBackColor = hoverColor },
                Size = new Size(160, 45),
                Font = AppFonts.ButtonFont,
                Cursor = Cursors.Hand,
                Padding = new Padding(10),
                Margin = new Padding(10)
            };
            btn.FlatAppearance.BorderColor = backColor;
            return btn;
        }

        private CustomPanel CreateProjectCard(string projeNo, ProjeBilgi projeBilgi, bool isAnaProje)
        {
            var card = new CustomPanel(AppColors.CardColor, AppColors.CardHoverColor)
            {
                Size = new Size(320, isAnaProje ? 280 : 240),
                Margin = new Padding(15),
                Tag = projeNo,
                Cursor = Cursors.Hand
            };

            var contextMenu = new ContextMenuStrip();
            var menuFiyatlandirma = new ToolStripMenuItem("Fiyatlandırma Ekle");
            menuFiyatlandirma.Click += (s, e) => AcFiyatlandirmaFormu(projeNo);
            contextMenu.Items.Add(menuFiyatlandirma);
            card.ContextMenuStrip = contextMenu;

            var lblProje = new Label
            {
                Text = isAnaProje ? $"Ana Proje: {projeNo}" : projeNo,
                Font = AppFonts.TitleFont,
                AutoSize = true,
                Location = new Point(20, 20),
                ForeColor = AppColors.PrimaryColor,
                BackColor = Color.Transparent
            };

            var txtProjeAdi = CreateTextBox(projeBilgi?.ProjeAdi, ProjeAdiPlaceholder, new Point(20, 70));
            var dtpOlusturmaTarihi = CreateDateTimePicker(projeBilgi?.OlusturmaTarihi, new Point(20, 130));
            var txtAciklama = CreateTextBox(projeBilgi?.Aciklama, PlaceholderText, new Point(20, 190));

            Action onInputChange = () => hasNewData = true;

            txtProjeAdi.TextChanged += (s, e) => onInputChange();
            dtpOlusturmaTarihi.ValueChanged += (s, e) => onInputChange();
            txtAciklama.TextChanged += (s, e) => onInputChange();

            card.Controls.Add(lblProje);
            card.Controls.Add(CreateLabel("Proje Adı:", new Point(20, 50)));
            card.Controls.Add(txtProjeAdi);
            card.Controls.Add(CreateLabel("Oluşturma Tarihi:", new Point(20, 110)));
            card.Controls.Add(dtpOlusturmaTarihi);
            card.Controls.Add(CreateLabel("Açıklama:", new Point(20, 170)));
            card.Controls.Add(txtAciklama);

            projeInputs[projeNo] = (txtProjeAdi, dtpOlusturmaTarihi, txtAciklama);
            return card;
        }

        private Label CreateLabel(string text, Point location)
        {
            return new Label
            {
                Text = text,
                Font = AppFonts.LabelFont,
                AutoSize = true,
                Location = location,
                ForeColor = AppColors.PrimaryColor,
                BackColor = Color.Transparent
            };
        }

        private TextBox CreateTextBox(string text, string placeholder, Point location)
        {
            var txtBox = new TextBox
            {
                Text = string.IsNullOrEmpty(text) ? placeholder : text,
                ForeColor = string.IsNullOrEmpty(text) ? Color.Gray : AppColors.PrimaryColor,
                Size = new Size(280, 25),
                Location = location,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = AppColors.CardHoverColor,
                Font = AppFonts.InputFont,
                Padding = new Padding(5)
            };
            txtBox.Enter += (s, e) =>
            {
                if (txtBox.Text == placeholder)
                {
                    txtBox.Text = "";
                    txtBox.ForeColor = AppColors.PrimaryColor;
                }
            };
            txtBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtBox.Text))
                {
                    txtBox.Text = placeholder;
                    txtBox.ForeColor = Color.Gray;
                }
            };
            return txtBox;
        }

        private DateTimePicker CreateDateTimePicker(DateTime? date, Point location)
        {
            return new DateTimePicker
            {
                Size = new Size(280, 25),
                Location = location,
                Format = DateTimePickerFormat.Short,
                Value = date ?? DateTime.Now,
                CalendarForeColor = AppColors.PrimaryColor,
                CalendarTitleBackColor = AppColors.SecondaryColor,
                CalendarTitleForeColor = Color.White,
                Font = AppFonts.InputFont,
            };
        }

        private void AnimatePanelHeight(Panel panel, int targetHeight)
        {
            var timer = new Timer { Interval = 16 };
            int startHeight = panel.Height;
            int step = (targetHeight - startHeight) / 20;

            timer.Tick += (t, te) =>
            {
                startHeight += step;
                panel.Height = Math.Max(0, Math.Min(targetHeight, startHeight));
                panel.Invalidate();

                if (panel.Height == targetHeight || panel.Height == 0)
                {
                    timer.Stop();
                }
            };
            timer.Start();
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
                        subInputs.txtProjeAdi.Text = projeAdi;
                        subInputs.txtProjeAdi.ForeColor = string.IsNullOrEmpty(projeAdi) ? Color.Gray : AppColors.PrimaryColor;

                        subInputs.txtAciklama.Text = aciklama;
                        subInputs.txtAciklama.ForeColor = string.IsNullOrEmpty(aciklama) ? Color.Gray : AppColors.PrimaryColor;

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
            bool anyChange = false;

            foreach (var kvp in projeInputs)
            {
                string projeNo = kvp.Key;
                var (txtProjeAdi, dtpOlusturmaTarihi, txtAciklama) = kvp.Value;

                string projeAdi = txtProjeAdi.Text == ProjeAdiPlaceholder ? "" : txtProjeAdi.Text;
                string aciklama = txtAciklama.Text == PlaceholderText ? "" : txtAciklama.Text;
                var projeBilgi = ProjeFinans_Projeler.GetProjeBilgileri(projeNo);
                bool success;

                if (projeBilgi != null)
                {
                    success = ProjeFinans_Projeler.UpdateProjeFinans(projeNo, aciklama, projeAdi, dtpOlusturmaTarihi.Value, out bool changed);
                    if (changed) anyChange = true;
                }
                else
                {
                    success = ProjeFinans_Projeler.ProjeEkleProjeFinans(projeNo, aciklama, projeAdi, dtpOlusturmaTarihi.Value);
                    if (success) anyChange = true;
                }

                if (success)
                {
                    anySuccess = true;
                }
            }

            if (anySuccess && anyChange)
            {
                hasNewData = false;
                OnKaydet?.Invoke();
                MessageBox.Show("Değişiklikler başarıyla kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!anyChange)
            {
                MessageBox.Show("Herhangi bir değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Kayıt sırasında bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
