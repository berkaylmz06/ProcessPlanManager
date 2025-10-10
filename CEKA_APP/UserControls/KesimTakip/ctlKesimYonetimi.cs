using CEKA_APP.Abstracts;
using CEKA_APP.Forms.KesimTakip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UserControls.KesimTakip
{
    public partial class ctlKesimYonetimi : UserControl
    {
        private IKullaniciAdiOgren _kullaniciAdi;
        public Dictionary<TabPage, List<Panel>> PagePanels => pagePanels;

        private readonly Dictionary<TabPage, List<Panel>> pagePanels = new Dictionary<TabPage, List<Panel>>();
        private const int MAX_PAGES = 10;

        private readonly IServiceProvider _serviceProvider;

        public ctlKesimYonetimi(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitializeComponent();

            this.tabControlMain.DrawMode = TabDrawMode.OwnerDrawFixed;

            this.tabControlMain.ItemSize = new Size(0, 28);

            this.tabControlMain.Padding = new Point(25, 5);

            this.buttonAddPage.BringToFront();

            AddNewPage();

            this.Resize += (sender, e) => UpdatePlusButtonLocations();
            this.Load += CtlKesimYonetimi_Load;
            this.buttonAddPage.Click += ButtonAddPage_Click;

            this.tabControlMain.Selected += TabControlMain_Selected;
            this.tabControlMain.DrawItem += TabControlMain_DrawItem;
            this.tabControlMain.MouseDown += TabControlMain_MouseDown;
        }

        public void FormKullaniciAdiGetir(IKullaniciAdiOgren kullaniciAdi)
        {
            _kullaniciAdi = kullaniciAdi;
        }
        private void CtlKesimYonetimi_Load(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
            {
                this.ParentForm.WindowState = FormWindowState.Maximized;
            }
        }
        private void TabControlMain_Selected(object sender, TabControlEventArgs e)
        {
            UpdatePlusButtonLocations();
            tabControlMain.Invalidate();
        }

        private void ButtonAddPage_Click(object sender, EventArgs e)
        {
            AddNewPage();
        }

        private void AddNewPage()
        {
            if (tabControlMain.TabPages.Count >= MAX_PAGES)
            {
                MessageBox.Show($"Maksimum {MAX_PAGES} adet sayfa ekleyebilirsiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maxExistingPageNumber = (tabControlMain.TabPages.OfType<TabPage>()
                .Select(t => t.Text.StartsWith("Sayfa ") ? int.TryParse(t.Text.Replace("Sayfa ", ""), out int n) ? n : 0 : 0)
                .DefaultIfEmpty(0).Max()) + 1;

            TabPage newPage = new TabPage($"Sayfa {maxExistingPageNumber}");
            newPage.AutoScroll = true;

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
            {
                Name = $"tableLayoutPanelPage{maxExistingPageNumber}",
                ColumnCount = 3,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33334F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33334F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            List<Panel> currentPanels = new List<Panel>();

            for (int i = 0; i < 6; i++)
            {
                Panel newPanel = new Panel
                {
                    Name = $"panelScreen_P{maxExistingPageNumber}_{i + 1}",
                    BorderStyle = BorderStyle.FixedSingle,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(3)
                };

                currentPanels.Add(newPanel);

                int row = i / tableLayoutPanel.ColumnCount;
                int col = i % tableLayoutPanel.ColumnCount;

                tableLayoutPanel.Controls.Add(newPanel, col, row);
                CreatePlusButtonForPanel(newPanel);
            }

            pagePanels.Add(newPage, currentPanels);
            newPage.Controls.Add(tableLayoutPanel);
            tabControlMain.Controls.Add(newPage);
            tabControlMain.SelectedTab = newPage;
            UpdatePlusButtonLocations();
        }

        private void TabControlMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tabPage = tabControlMain.TabPages[e.Index];
            Rectangle tabRect = tabControlMain.GetTabRect(e.Index);

            bool isSelected = e.Index == tabControlMain.SelectedIndex;

            Color backColor = isSelected ? Color.White : Color.FromArgb(240, 240, 240);
            Color foreColor = isSelected ? Color.Black : Color.Gray;

            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.None,
                FormatFlags = StringFormatFlags.NoWrap
            };

            int textPaddingRight = 20;

            Rectangle textRect = new Rectangle(tabRect.X + 5, tabRect.Y, tabRect.Width - textPaddingRight, tabRect.Height);

            Font tabFont = new Font(e.Font.FontFamily, e.Font.Size, FontStyle.Regular);

            using (SolidBrush foreBrush = new SolidBrush(foreColor))
            {
                e.Graphics.DrawString(tabPage.Text, tabFont, foreBrush, textRect, sf);
            }

            Rectangle closeButtonRect = new Rectangle(
                tabRect.Right - 15,
                tabRect.Top + (tabRect.Height - 8) / 2,
                8,
                8);

            Color xColor = isSelected ? Color.Black : Color.DarkGray;

            using (Pen pen = new Pen(xColor, 1.5f)) 
            {
                e.Graphics.DrawLine(pen, closeButtonRect.X, closeButtonRect.Y, closeButtonRect.Right, closeButtonRect.Bottom);
                e.Graphics.DrawLine(pen, closeButtonRect.Right, closeButtonRect.Y, closeButtonRect.X, closeButtonRect.Bottom);
            }

            if (isSelected)
            {
                using (Pen bluePen = new Pen(Color.FromArgb(0, 120, 215), 3))
                {
                    e.Graphics.DrawLine(bluePen, tabRect.X, tabRect.Bottom - 1, tabRect.Right, tabRect.Bottom - 1);
                }
                e.Graphics.DrawRectangle(new Pen(Color.LightGray), tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1);
            }
            else
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1);
            }
        }

        private void TabControlMain_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControlMain.TabPages.Count; i++)
            {
                Rectangle tabRect = tabControlMain.GetTabRect(i);

                Rectangle closeButtonRect = new Rectangle(
                    tabRect.Right - 20,
                    tabRect.Top + (tabRect.Height - 15) / 2,
                    20,
                    20);

                if (closeButtonRect.Contains(e.Location))
                {
                    TabPage tabPageToClose = tabControlMain.TabPages[i];

                    if (CheckIfKesimControlActive(tabPageToClose))
                    {
                        MessageBox.Show("Bu sayfada aktif bir kesim ataması bulunmaktadır. Sayfa kapatılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    tabControlMain.TabPages.Remove(tabPageToClose);
                    pagePanels.Remove(tabPageToClose);

                    if (tabControlMain.TabPages.Count == 0)
                    {
                        AddNewPage();
                    }

                    UpdatePlusButtonLocations();
                    return;
                }
            }
        }
        private bool CheckIfKesimControlActive(TabPage tabPage)
        {
            if (pagePanels.TryGetValue(tabPage, out List<Panel> panels))
            {
                foreach (var panel in panels)
                {
                    if (panel.Controls.Count > 0)
                    {
                        if (panel.Controls[0] is Button btn && btn.Text == "+")
                        {
                            continue;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private void CreatePlusButtonForPanel(Panel panel)
        {
            Button plusButton = new Button
            {
                Text = "+",
                Tag = panel,
                Width = 60,
                Height = 60,
                Anchor = AnchorStyles.None,
                Font = new Font("Arial", 20, FontStyle.Bold)
            };

            plusButton.Click += PlusButton_Click;
            panel.Controls.Add(plusButton);
        }

        private void UpdatePlusButtonLocations()
        {
            TabPage activePage = tabControlMain.SelectedTab;
            if (activePage == null || !pagePanels.ContainsKey(activePage)) return;

            var panelsInActivePage = pagePanels[activePage];

            foreach (var panel in panelsInActivePage)
            {
                var plusButton = panel.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "+");
                if (plusButton != null)
                {
                    plusButton.Location = new Point(
                        (panel.Width / 2) - (plusButton.Width / 2),
                        (panel.Height / 2) - (plusButton.Height / 2)
                    );
                }
            }
        }

        private string GetSelectedOperator(Panel targetPanel)
        {
            using (var selectionForm = new frmKesimAtamaSecimi(_serviceProvider))
            {
                if (selectionForm.ShowDialog() == DialogResult.OK)
                {
                    return selectionForm.cmbOperator.Text;
                }
                else
                {
                    return null;
                }
            }
        }

        private void PlusButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            Panel targetPanel = clickedButton.Tag as Panel;
            if (targetPanel == null) return;

            string selectedOperator = GetSelectedOperator(targetPanel);

            if (selectedOperator != null)
            {
                AddKesimPaneli(targetPanel, selectedOperator);
                clickedButton.Visible = false;
            }
        }

        private void AddKesimPaneli(Panel targetPanel, string operatorName)
        {
            try
            {
                var kesimControl = new ctlKesimPaneli(_serviceProvider);
                kesimControl.OperatorAd = operatorName;
                kesimControl.AutoSize = false;
                kesimControl.MinimumSize = new Size(0, 0);
                kesimControl.MaximumSize = new Size(0, 0);
                kesimControl.Dock = DockStyle.Fill;
                kesimControl.Margin = new Padding(0);
                kesimControl.Padding = new Padding(0);
                kesimControl.Size = targetPanel.ClientSize;
                targetPanel.AutoScroll = true;
                targetPanel.Controls.Clear();
                kesimControl.FormKullaniciAdiGetir(_kullaniciAdi);
                kesimControl.KesimTamamlandi += (s, e) =>
                {
                    targetPanel.Controls.Clear();
                    CreatePlusButtonForPanel(targetPanel);
                    UpdatePlusButtonLocations();
                };

                targetPanel.Controls.Add(kesimControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kontrol eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                targetPanel.Controls.Clear();
                CreatePlusButtonForPanel(targetPanel);
                UpdatePlusButtonLocations();
            }
        }
    }
}