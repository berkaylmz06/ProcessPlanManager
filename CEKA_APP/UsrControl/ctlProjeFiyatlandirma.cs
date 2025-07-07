using CEKA_APP.DataBase.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeFiyatlandirma : UserControl
    {
        private FiyatlandirmaKalemleriData iscilikData = new FiyatlandirmaKalemleriData();
        private ProjeFiyatlandirmaData fiyatlandirmaData = new ProjeFiyatlandirmaData();
        public event Action<string> OnFiyatlandirmaKaydedildi;
        private ComboBox cmbProjeNo;
        private List<string> altProjeler;

        private Button btnYeniKalemEkle;
        private Label lblToplamTeklif;
        private Label lblToplamGerceklesen;

        public ctlProjeFiyatlandirma()
        {
            InitializeComponent();

            cmbProjeNo = new ComboBox
            {
                Size = txtProjeNo.Size,
                Location = txtProjeNo.Location,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Visible = false
            };
            cmbProjeNo.SelectedIndexChanged += (s, e) =>
            {
                if (cmbProjeNo.SelectedItem != null)
                {
                    LoadIscilikler(cmbProjeNo.SelectedItem.ToString());
                }
            };
            panelUst.Controls.Add(cmbProjeNo);
            cmbProjeNo.BringToFront();

            btnKaydet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnKaydet.Location = new Point(panelUst.Width - btnKaydet.Width - 10, 10);

            btnProjeAra.Click += btnProjeAra_Click;

            btnYeniKalemEkle = new Button
            {
                Text = "+ Kalem Ekle",
                BackColor = Color.Gainsboro,
                Font = new Font("Segoe UI", 8),
                AutoSize = true,
                Padding = new Padding(5),
                Anchor = AnchorStyles.Right
            };
            btnYeniKalemEkle.Click += BtnYeniKalemEkle_Click;

            lblToplamTeklif = new Label
            {
                Text = "Toplam Teklif: 0.00",
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Anchor = AnchorStyles.Left
            };

            lblToplamGerceklesen = new Label
            {
                Text = "Toplam Gerçekleşen: 0.00",
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Anchor = AnchorStyles.Left
            };

            var altPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true
            };
            altPanel.Controls.Add(lblToplamTeklif);
            altPanel.Controls.Add(lblToplamGerceklesen);
            altPanel.Controls.Add(btnYeniKalemEkle);

            Controls.Add(altPanel);
        }

        private void ctlProjeFiyatlandirma_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Proje Fiyatlandırma";
            panelUst.AutoScroll = true;

            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            tableLayoutPanel1.AutoScroll = true;
        }

        public void LoadProjeFiyatlandirma(string projeNo, bool autoSearch = false, List<string> altProjeler = null)
        {
            this.altProjeler = altProjeler;
            txtProjeNo.Text = projeNo;

            if (altProjeler != null && altProjeler.Any())
            {
                txtProjeNo.Visible = false;
                cmbProjeNo.Visible = true;
                cmbProjeNo.Items.Clear();
                cmbProjeNo.Items.Add(projeNo);
                cmbProjeNo.Items.AddRange(altProjeler.ToArray());
                cmbProjeNo.SelectedItem = projeNo;
                cmbProjeNo.BringToFront();
            }
            else
            {
                txtProjeNo.Visible = true;
                cmbProjeNo.Visible = false;
            }

            if (autoSearch)
            {
                LoadIscilikler(projeNo);
            }
        }

        private void LoadIscilikler(string projeNo)
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Clear();

            AddHeaderRow();

            if (!string.IsNullOrEmpty(projeNo))
            {
                var fiyatlandirmalar = fiyatlandirmaData.GetFiyatlandirmaByProje(projeNo);
                // Eğer hazır veri varsa burada eklersiniz
            }
        }

        private void AddHeaderRow()
        {
            string[] headers = {
                "Üretim ve Montaj Kalemleri",
                "Teklif Adet/Ağırlık",
                "Teklif Birim Fiyat",
                "Teklif Toplam",
                "Gerçekleşen Adet/Ağırlık",
                "Gerçekleşen Birim Fiyat",
                "Gerçekleşen Maliyet",
                "Son Durum"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var lbl = new Label
                {
                    Text = headers[i],
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0)
                };
                tableLayoutPanel1.Controls.Add(lbl, i, 0);
            }
        }

        private void BtnYeniKalemEkle_Click(object sender, EventArgs e)
        {
            var frm = new frmYeniFiyatlandirmaKalemiEkle();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                AddYeniKalemSatiri(frm.KalemAdi);
            }
        }

        private void AddYeniKalemSatiri(string kalemAdi)
        {
            int newRowIndex = tableLayoutPanel1.RowCount;
            tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Kalem Adı TextBox olarak eklendi
            var txtKalemAdi = new TextBox
            {
                Text = kalemAdi,
                Dock = DockStyle.Fill,
                TextAlign = HorizontalAlignment.Left,
                Margin = new Padding(3)
            };
            tableLayoutPanel1.Controls.Add(txtKalemAdi, 0, newRowIndex);

            for (int i = 1; i < 8; i++)
            {
                if (i == 7) continue;

                var txt = new TextBox
                {
                    Dock = DockStyle.Fill,
                    TextAlign = HorizontalAlignment.Right,
                    Margin = new Padding(3)
                };

                if (i == 3 || i == 6)
                {
                    txt.Enabled = false;
                    txt.BackColor = Color.LightGray;
                }

                if (i == 1 || i == 2 || i == 4 || i == 5)
                {
                    txt.TextChanged += HesaplaVeGuncelle;
                }

                tableLayoutPanel1.Controls.Add(txt, i, newRowIndex);
            }

            var lblSonDurum = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Margin = new Padding(3),
                ForeColor = Color.Black
            };
            tableLayoutPanel1.Controls.Add(lblSonDurum, 7, newRowIndex);
        }

        private void HesaplaVeGuncelle(object sender, EventArgs e)
        {
            if (sender is TextBox txtChanged)
            {
                var pos = tableLayoutPanel1.GetPositionFromControl(txtChanged);
                int row = pos.Row;

                if (row < 1 || row >= tableLayoutPanel1.RowCount) return;

                var txtTeklifAdet = GetTextBoxAt(row, 1);
                var txtTeklifBirimFiyat = GetTextBoxAt(row, 2);
                var txtTeklifToplam = GetTextBoxAt(row, 3);
                var txtGerceklesenAdet = GetTextBoxAt(row, 4);
                var txtGerceklesenBirimFiyat = GetTextBoxAt(row, 5);
                var txtGerceklesenMaliyet = GetTextBoxAt(row, 6);
                var lblSonDurum = GetLabelAt(row, 7);

                decimal teklifAdet = decimal.TryParse(txtTeklifAdet?.Text, out decimal ta) ? ta : 0;
                decimal teklifBirimFiyat = decimal.TryParse(txtTeklifBirimFiyat?.Text, out decimal tbf) ? tbf : 0;
                decimal gerceklesenAdet = decimal.TryParse(txtGerceklesenAdet?.Text, out decimal ga) ? ga : 0;
                decimal gerceklesenBirimFiyat = decimal.TryParse(txtGerceklesenBirimFiyat?.Text, out decimal gbf) ? gbf : 0;

                decimal teklifToplam = teklifAdet * teklifBirimFiyat;
                decimal gerceklesenMaliyet = gerceklesenAdet * gerceklesenBirimFiyat;
                decimal fark = teklifToplam - gerceklesenMaliyet;

                if (txtTeklifToplam != null)
                    txtTeklifToplam.Text = teklifToplam.ToString("N2");

                if (txtGerceklesenMaliyet != null)
                    txtGerceklesenMaliyet.Text = gerceklesenMaliyet.ToString("N2");

                if (lblSonDurum != null)
                {
                    lblSonDurum.Text = fark.ToString("N2");
                    lblSonDurum.ForeColor = fark < 0 ? Color.Red : Color.Green;
                }

                decimal toplamTeklif = 0, toplamGerceklesen = 0;

                for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                {
                    toplamTeklif += decimal.TryParse(GetTextBoxAt(i, 3)?.Text, out decimal tt) ? tt : 0;
                    toplamGerceklesen += decimal.TryParse(GetTextBoxAt(i, 6)?.Text, out decimal gm) ? gm : 0;
                }

                lblToplamTeklif.Text = $"Toplam Teklif: {toplamTeklif:N2}";
                lblToplamGerceklesen.Text = $"Toplam Gerçekleşen: {toplamGerceklesen:N2}";
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
            OnFiyatlandirmaKaydedildi?.Invoke(txtProjeNo.Text);
        }

        private void btnProjeAra_Click(object sender, EventArgs e)
        {
            string arananProjeNo = txtProjeNo.Text.Trim();
            if (!string.IsNullOrEmpty(arananProjeNo))
            {
                LoadIscilikler(arananProjeNo);
            }
            else
            {
                MessageBox.Show("Lütfen bir proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
