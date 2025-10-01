using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.UsrControl.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private Button btnKaydetHepsi;
        private ctlBaslik ctlBaslik1;

        private readonly IServiceProvider _serviceProvider;
        private IFinansProjelerService _finansProjelerService => _serviceProvider.GetRequiredService<IFinansProjelerService>();
        private IProjeKutukService _projeKutukService => _serviceProvider.GetRequiredService<IProjeKutukService>();
        private IUserControlFactory _userControlFactory => _serviceProvider.GetRequiredService<IUserControlFactory>();


        public ctlProjeBilgileri(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DoubleBuffered = true;
            hasNewData = false;
            projeInputs = new Dictionary<string, (TextBox, DateTimePicker, TextBox)>();
            this.BackColor = AppColors.BackgroundColor;

            ctlBaslik1 = new ctlBaslik
            {
                Baslik = "Proje Bilgileri",
                Dock = DockStyle.Top,
                Height = 41,
                Visible = true
            };
            this.Controls.Add(ctlBaslik1);

            btnKaydetHepsi = CreateStyledButton("Tümünü Kaydet", AppColors.SuccessColor, AppColors.SuccessColor);
            btnKaydetHepsi.Click += BtnKaydetHepsi_Click;
            btnKaydetHepsi.Dock = DockStyle.Bottom;
            if (!this.Controls.Contains(btnKaydetHepsi))
            {
                this.Controls.Add(btnKaydetHepsi);
            }

            panelProjeler.Dock = DockStyle.Fill;
            if (!this.Controls.Contains(panelProjeler))
            {
                this.Controls.Add(panelProjeler);
            }

            if (this.Controls.Contains(ctlBaslik1))
                this.Controls.SetChildIndex(ctlBaslik1, 0);
            if (this.Controls.Contains(panelProjeler))
                this.Controls.SetChildIndex(panelProjeler, 1);
            if (this.Controls.Contains(btnKaydetHepsi))
                this.Controls.SetChildIndex(btnKaydetHepsi, 2);
        }
        private static class AppColors
        {
            public static readonly Color PrimaryColor = Color.FromArgb(39, 55, 70);
            public static readonly Color SecondaryColor = Color.FromArgb(25, 118, 210);
            public static readonly Color SuccessColor = Color.FromArgb(67, 160, 71);
            public static readonly Color WarningColor = Color.FromArgb(255, 152, 0);
            public static readonly Color BackgroundColor = Color.FromArgb(245, 248, 250);
            public static readonly Color CardColor = Color.FromArgb(227, 242, 253);
            public static readonly Color CardHoverColor = Color.FromArgb(207, 231, 246);
        }

        private static class AppFonts
        {
            public static readonly Font TitleFont = new Font("Arial", 14, FontStyle.Bold);
            public static readonly Font SubtitleFont = new Font("Arial", 10, FontStyle.Regular);
            public static readonly Font ButtonFont = new Font("Arial", 10, FontStyle.Bold);
            public static readonly Font LabelFont = new Font("Arial", 9, FontStyle.Regular);
            public static readonly Font InputFont = new Font("Arial", 9, FontStyle.Regular);
        }
        public void LoadProjects(List<string> projectsNo, string anaProjeNo = null)
        {
            panelProjeler.Controls.Clear();
            hasNewData = false;
            projeInputs.Clear();
            panelProjeler.AutoScroll = true;

            int yPosition = ctlBaslik1.Height + 20;

            if (!string.IsNullOrEmpty(anaProjeNo) && projectsNo.Any())
            {
                var anaProjeBilgi = _finansProjelerService.GetProjeBilgileriByNo(anaProjeNo); 
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

                int xOffset = 20;
                foreach (var projeNo in projectsNo)
                {
                    var projeBilgi = _finansProjelerService.GetProjeBilgileriByNo(projeNo);
                    var card = CreateProjectCard(projeNo, projeBilgi, false);
                    card.Location = new Point(xOffset, 20);
                    altProjelerPanel.Controls.Add(card);
                    xOffset += card.Width + 20;
                }

                panelProjeler.Controls.Add(altProjelerPanel);
            }
            else
            {
                int xOffset = 20;
                foreach (var projeNo in projectsNo)
                {
                    var projeBilgi = _finansProjelerService.GetProjeBilgileriByNo(projeNo);
                    var card = CreateProjectCard(projeNo, projeBilgi, false);
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
            var menuFiyatlandirma = new ToolStripMenuItem("Fiyatlandırma Ekle") { Enabled = !isAnaProje };
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

            Action onInputChange = () =>
            {
                hasNewData = true;
                if (isAnaProje)
                {
                    var altProjelerPanel = panelProjeler.Controls.OfType<Panel>().FirstOrDefault(p => p.Tag?.ToString() == "altProjelerPanel");
                    if (altProjelerPanel != null)
                    {
                        CopyToSubProjects(projeNo, altProjelerPanel);
                    }
                }
            };

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

            if (isAnaProje)
            {
                var lblOk = new Label
                {
                    Text = "►",
                    Font = AppFonts.TitleFont,
                    AutoSize = true,
                    Location = new Point(card.Width - 40, 20),
                    ForeColor = AppColors.PrimaryColor,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand
                };
                card.Controls.Add(lblOk);

                lblOk.Click += (s, e) =>
                {
                    var altProjelerPanel = panelProjeler.Controls.OfType<Panel>().FirstOrDefault(p => p.Tag?.ToString() == "altProjelerPanel");
                    if (altProjelerPanel != null)
                    {
                        bool isPanelVisible = altProjelerPanel.Height > 0;
                        lblOk.Text = isPanelVisible ? "►" : "▼";
                        AnimatePanelHeight(altProjelerPanel, isPanelVisible ? 0 : 300);
                        if (!isPanelVisible)
                        {
                            CopyToSubProjects(projeNo, altProjelerPanel);
                        }
                    }
                };

                card.Click += (s, e) => lblOk_Click(lblOk, EventArgs.Empty);
            }

            menuFiyatlandirma.Click += (s, e) =>
            {
                var parentForm = this.FindForm() as frmAnaSayfa;
                if (parentForm == null)
                {
                    MessageBox.Show("Ana form bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int? projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
                if (!projeId.HasValue)
                {
                    MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (parentForm.projeFiyatlandirma == null)
                {
                    parentForm.projeFiyatlandirma = _userControlFactory.CreateProjeFiyatlandirmaControl();
                    parentForm.projeFiyatlandirma.Dock = DockStyle.Fill;
                }

                List<int> altProjeler = isAnaProje
                    ? panelProjeler.Controls.OfType<Panel>()
                        .Where(p => p.Tag?.ToString() == "altProjelerPanel")
                        .SelectMany(p => p.Controls.OfType<CustomPanel>())
                        .Select(c => _finansProjelerService.GetProjeIdByNo(c.Tag.ToString()))
                        .Where(id => id.HasValue)
                        .Select(id => id.Value)
                        .ToList()
                    : null;

                parentForm.projeFiyatlandirma.LoadProjeFiyatlandirma(projeId.Value, autoSearch: false, altProjeler: altProjeler);
                parentForm.NavigateToUserControl(parentForm.projeFiyatlandirma);
                parentForm.projeFiyatlandirma.btnProjeAra_Click(null, EventArgs.Empty);
            };

            projeInputs[projeNo] = (txtProjeAdi, dtpOlusturmaTarihi, txtAciklama);
            return card;
        }
        private void lblOk_Click(object sender, EventArgs e)
        {
            if (sender is Label lblOk)
            {
                var altProjelerPanel = panelProjeler.Controls.OfType<Panel>().FirstOrDefault(p => p.Tag?.ToString() == "altProjelerPanel");
                if (altProjelerPanel != null)
                {
                    bool isPanelVisible = altProjelerPanel.Height > 0;
                    lblOk.Text = isPanelVisible ? "►" : "▼";
                    AnimatePanelHeight(altProjelerPanel, isPanelVisible ? 0 : 300);
                    if (!isPanelVisible)
                    {
                        string anaProjeNo = lblOk.Parent.Tag.ToString();
                        CopyToSubProjects(anaProjeNo, altProjelerPanel);
                    }
                }
            }
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
                Enabled = true,
                MinDate = DateTime.MinValue,
                MaxDate = DateTime.MaxValue
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
                        string altProjeNo = card.Tag.ToString();
                        var altProjeBilgi = _finansProjelerService.GetProjeBilgileriByNo(altProjeNo); 

                        if (altProjeBilgi == null)
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
        }
        private void BtnKaydetHepsi_Click(object sender, EventArgs e)
        {
            bool anyChange = false;
            List<string> validationErrors = new List<string>();

            foreach (var kvp in projeInputs)
            {
                string projeNo = kvp.Key;
                var (txtProjeAdi, dtpOlusturmaTarihi, txtAciklama) = kvp.Value;

                string projeAdi = txtProjeAdi.Text == ProjeAdiPlaceholder ? "" : txtProjeAdi.Text.Trim();
                string aciklama = txtAciklama.Text == PlaceholderText ? "" : txtAciklama.Text.Trim();
                DateTime olusturmaTarihi = dtpOlusturmaTarihi.Value;

                if (string.IsNullOrWhiteSpace(projeAdi))
                {
                    validationErrors.Add($"Proje No: {projeNo} için proje adı zorunludur.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(aciklama))
                {
                    validationErrors.Add($"Proje No: {projeNo} için açıklama zorunludur.");
                    continue;
                }
                if (olusturmaTarihi == DateTime.MinValue || olusturmaTarihi == DateTime.MaxValue)
                {
                    validationErrors.Add($"Proje No: {projeNo} için geçerli bir oluşturma tarihi zorunludur.");
                    continue;
                }

                try
                {
                    var projeId = _finansProjelerService.GetProjeIdByNo(projeNo);

                    var projeBilgi = projeId.HasValue ? _finansProjelerService.GetProjeBilgileri(projeId.Value) : null;

                    bool hasChanges = false;
                    if (projeBilgi != null)
                    {
                        hasChanges = projeBilgi.ProjeAdi?.Trim() != projeAdi ||
                                     projeBilgi.Aciklama?.Trim() != aciklama ||
                                     projeBilgi.OlusturmaTarihi.Date != olusturmaTarihi.Date;
                    }
                    else
                    {
                        hasChanges = true;
                    }

                    if (hasChanges)
                    {
                        bool success;
                        if (projeBilgi != null)
                        {
                            success = _finansProjelerService.UpdateProjeFinans(projeId.Value, projeNo, aciklama, projeAdi, olusturmaTarihi, out bool changed);
                            if (changed) anyChange = true;
                        }
                        else
                        {
                            success = _projeKutukService.ProjeEkleProjeFinans(projeNo, aciklama, projeAdi, olusturmaTarihi);
                            if (success) anyChange = true;
                        }

                        if (!success)
                        {
                            validationErrors.Add($"Proje No: {projeNo} - Kayıt işlemi başarısız.");
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    validationErrors.Add($"Proje No: {projeNo} - {ex.Message}");
                }
                catch (Exception ex)
                {
                    validationErrors.Add($"Proje No: {projeNo} - Beklenmedik hata: {ex.Message}");
                }
            }

            if (validationErrors.Any())
            {
                MessageBox.Show(string.Join("\n", validationErrors), "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (anyChange)
            {
                hasNewData = false;
                OnKaydet?.Invoke();
                MessageBox.Show("Değişiklikler başarıyla kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Herhangi bir değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}