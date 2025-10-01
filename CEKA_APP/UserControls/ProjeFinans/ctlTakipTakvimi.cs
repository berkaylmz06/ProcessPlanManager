using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Services.ProjeFinans;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl.ProjeFinans
{
    public partial class ctlTakipTakvimi : UserControl
    {
        private DateTime _currentDate;
        private Panel _expandedPanel = null;
        private int _originalColumn;
        private int _originalRow;
        private readonly IServiceProvider _serviceProvider;

        private IOdemeSartlariService _odemeSartlariService => _serviceProvider.GetRequiredService<IOdemeSartlariService>();
        public ctlTakipTakvimi(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));


            _currentDate = DateTime.Now;
            this.Click += CtlTakipTakvimi_Click;
        }

        private void ctlTakipTakvimi_Load(object sender, EventArgs e)
        {
            TakvimiOlustur(_currentDate);
        }

        private void CtlTakipTakvimi_Click(object sender, EventArgs e)
        {
            if (_expandedPanel != null && sender is Control ctrl && !IsDescendant(ctrl, _expandedPanel))
            {
                CollapsePanel();
            }
        }

        private bool IsDescendant(Control c1, Control c2)
        {
            Control parent = c1.Parent;
            while (parent != null)
            {
                if (parent.Equals(c2))
                {
                    return true;
                }
                parent = parent.Parent;
            }
            return false;
        }

        private void TakvimiOlustur(DateTime tarih)
        {
            if (_expandedPanel != null)
            {
                CollapsePanel();
            }

            tlpTakvim.Controls.Clear();
            lblTarih.Text = tarih.ToString("MMMM yyyy", new CultureInfo("tr-TR"));

            string[] gunler = { "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt", "Paz" };
            for (int i = 0; i < 7; i++)
            {
                Panel gunAdiPanel = new Panel()
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.LightGray,
                    Margin = new Padding(0)
                };

                Label lblGunAdi = new Label()
                {
                    Text = gunler[i],
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 10F, FontStyle.Bold),
                    Margin = new Padding(0)
                };

                gunAdiPanel.Controls.Add(lblGunAdi);
                tlpTakvim.Controls.Add(gunAdiPanel, i, 0);
            }

            List<OdemeSartlari> odemeBilgileri = _odemeSartlariService.GetOdemeBilgileri();

            int gunSayisi = DateTime.DaysInMonth(tarih.Year, tarih.Month);
            DateTime ilkGun = new DateTime(tarih.Year, tarih.Month, 1);

            int bosluk;
            if (ilkGun.DayOfWeek == DayOfWeek.Sunday)
            {
                bosluk = 6;
            }
            else
            {
                bosluk = (int)ilkGun.DayOfWeek - 1;
            }

            for (int i = 1; i <= gunSayisi; i++)
            {
                var gununTarihi = new DateTime(tarih.Year, tarih.Month, i);

                Panel gunPanel = new Panel()
                {
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = i,
                    Margin = new Padding(2)
                };
                gunPanel.Click += GunPanel_Click;

                Label lblGunNumarasi = new Label()
                {
                    Text = $"{i}",
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.TopCenter,
                    Font = new Font("Arial", 9F, FontStyle.Bold),
                    Name = "lblGunNumarasi"
                };
                lblGunNumarasi.Click += (s, ev) => GunPanel_Click(gunPanel, ev);
                gunPanel.Controls.Add(lblGunNumarasi);

                var buGununOdemeleri = odemeBilgileri.Where(o =>
                    o.gerceklesenTarih.HasValue && o.gerceklesenTarih.Value.Date == gununTarihi.Date).ToList();

                FlowLayoutPanel odemeListesiPanel = new FlowLayoutPanel()
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    AutoScroll = false,
                    WrapContents = false,
                    Padding = new Padding(2, 30, 2, 2),
                    Margin = new Padding(0),
                    Name = "odemeListesiPanel",
                    Tag = buGununOdemeleri
                };

                foreach (var odeme in buGununOdemeleri.Take(3))
                {
                    string kmTasiAdi = odeme.kilometreTasiAdi;
                    string displayText = $"Km: {kmTasiAdi}";

                    if (kmTasiAdi.Length > 18)
                    {
                        displayText = $"Km: {kmTasiAdi.Substring(0, 18)}...";
                    }

                    Label odemeLabel = new Label()
                    {
                        Text = displayText,
                        AutoSize = false,
                        Width = (int)(gunPanel.Width * 0.90),
                        Margin = new Padding((int)(gunPanel.Width * 0.05), 2, (int)(gunPanel.Width * 0.05), 0),
                        Padding = new Padding(2),
                        ForeColor = Color.Black,
                        BorderStyle = BorderStyle.FixedSingle,
                        Font = new Font("Arial", 8F, FontStyle.Regular),
                        Tag = odeme
                    };

                    if (odeme.kalanTutar == 0)
                    {
                        odemeLabel.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        odemeLabel.BackColor = Color.Yellow;
                    }

                    odemeLabel.Click += (s, ev) => GunPanel_Click(gunPanel, ev);
                    odemeListesiPanel.Controls.Add(odemeLabel);
                }

                if (buGununOdemeleri.Count > 3)
                {
                    Label moreLabel = new Label()
                    {
                        Text = "...",
                        AutoSize = true,
                        Font = new Font("Arial", 8F, FontStyle.Bold),
                        ForeColor = Color.DarkGray,
                        Margin = new Padding(5, 2, 5, 0)
                    };
                    moreLabel.Click += (s, ev) => GunPanel_Click(gunPanel, ev);
                    odemeListesiPanel.Controls.Add(moreLabel);
                }

                odemeListesiPanel.Click += (s, ev) => GunPanel_Click(gunPanel, ev);
                gunPanel.Controls.Add(odemeListesiPanel);

                gunPanel.Layout += (s, ev) =>
                {
                    foreach (Control control in odemeListesiPanel.Controls)
                    {
                        if (control is Label odemeLabel && odemeLabel.Name != "moreLabel")
                        {
                            odemeLabel.Width = (int)(gunPanel.Width * 0.90);
                            odemeLabel.Margin = new Padding((int)(gunPanel.Width * 0.05), 2, (int)(gunPanel.Width * 0.05), 0);
                        }
                    }
                };

                int row = (i + bosluk - 1) / 7 + 1;
                int col = (i + bosluk - 1) % 7;

                if (row < tlpTakvim.RowCount && col < tlpTakvim.ColumnCount)
                {
                    tlpTakvim.Controls.Add(gunPanel, col, row);
                }
            }
        }
        private void btnOncekiAy_Click(object sender, EventArgs e)
        {
            if (_expandedPanel != null)
            {
                CollapsePanel();
            }
            _currentDate = _currentDate.AddMonths(-1);
            TakvimiOlustur(_currentDate);
        }

        private void btnSonrakiAy_Click(object sender, EventArgs e)
        {
            if (_expandedPanel != null)
            {
                CollapsePanel();
            }
            _currentDate = _currentDate.AddMonths(1);
            TakvimiOlustur(_currentDate);
        }

        private void GunPanel_Click(object sender, EventArgs e)
        {
            Panel clickedPanel = sender as Panel;
            if (clickedPanel == null) return;

            var odemeListesiPanel = clickedPanel.Controls.Find("odemeListesiPanel", true).FirstOrDefault() as FlowLayoutPanel;
            if (odemeListesiPanel != null)
            {
                var buGununOdemeleri = odemeListesiPanel.Tag as List<OdemeSartlari>;

                if (buGununOdemeleri == null || !buGununOdemeleri.Any())
                {
                    return;
                }
            }

            if (_expandedPanel == clickedPanel)
            {
                CollapsePanel();
                return;
            }

            if (_expandedPanel != null)
            {
                CollapsePanel();
            }

            ExpandPanel(clickedPanel);
        }

        private void ExpandPanel(Panel panel)
        {
            if (panel == null) return;

            this.SuspendLayout();
            panel.SuspendLayout();

            _expandedPanel = panel;
            _originalColumn = tlpTakvim.GetColumn(panel);
            _originalRow = tlpTakvim.GetRow(panel);

            SetDoubleBuffered(panel, true);

            tlpTakvim.Controls.Remove(panel);
            this.Controls.Add(panel);

            panel.BringToFront();
            panel.Location = new Point(0, 0);
            panel.Size = this.Size;
            panel.BorderStyle = BorderStyle.Fixed3D;

            if (btnOncekiAy != null) btnOncekiAy.Visible = false;
            if (btnSonrakiAy != null) btnSonrakiAy.Visible = false;

            var odemeListesiPanel = panel.Controls.Find("odemeListesiPanel", true).FirstOrDefault() as FlowLayoutPanel;
            if (odemeListesiPanel != null)
            {
                odemeListesiPanel.Controls.Clear();
                odemeListesiPanel.AutoScroll = true;
                odemeListesiPanel.Padding = new Padding(5, 40, 5, 5);
                SetDoubleBuffered(odemeListesiPanel, true);

                var buGununOdemeleri = odemeListesiPanel.Tag as List<OdemeSartlari>;
                if (buGununOdemeleri != null)
                {
                    foreach (var odeme in buGununOdemeleri)
                    {
                        Label odemeLabel = new Label()
                        {
                            Text = $"Proje: {odeme.projeNo}\nKm Taşı: {odeme.kilometreTasiAdi}\nTutar: {odeme.tutar} TL\nKalan: {odeme.kalanTutar} TL",
                            AutoSize = false,
                            Height = 70,
                            Width = (int)(odemeListesiPanel.Width * 0.90), // %90 genişlik
                            Margin = new Padding((int)(odemeListesiPanel.Width * 0.05), 2, (int)(odemeListesiPanel.Width * 0.05), 2), // %5 sol ve sağ boşluk
                            Padding = new Padding(5),
                            ForeColor = Color.Black,
                            BorderStyle = BorderStyle.FixedSingle,
                            Font = new Font("Arial", 9F, FontStyle.Regular),
                            BackColor = odeme.kalanTutar == 0 ? Color.LightGreen : Color.Yellow
                        };
                        odemeListesiPanel.Controls.Add(odemeLabel);
                    }

                    odemeListesiPanel.Layout -= LayoutHandler;
                    odemeListesiPanel.Layout += LayoutHandler;
                }
            }

            panel.ResumeLayout();
            this.ResumeLayout();
        }

        private void SetDoubleBuffered(Control control, bool value)
        {
            if (control == null) return;
            var prop = control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop?.SetValue(control, value);
        }

        private void LayoutHandler(object sender, LayoutEventArgs e)
        {
            if (sender is FlowLayoutPanel odemeListesiPanel)
            {
                foreach (Control control in odemeListesiPanel.Controls)
                {
                    if (control is Label odemeLabel)
                    {
                        odemeLabel.Width = (int)(odemeListesiPanel.Width * 0.90);
                        odemeLabel.Margin = new Padding((int)(odemeListesiPanel.Width * 0.05), 2, (int)(odemeListesiPanel.Width * 0.05), 2);
                    }
                }
            }
        }
        private void CollapsePanel()
        {
            if (_expandedPanel == null) return;

            this.Controls.Remove(_expandedPanel);

            if (btnOncekiAy != null) btnOncekiAy.Visible = true;
            if (btnSonrakiAy != null) btnSonrakiAy.Visible = true;

            _expandedPanel = null;

            TakvimiOlustur(_currentDate);
        }
    }
}