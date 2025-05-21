using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using iText.Forms.Form.Element;
using KesimTakip.Abstracts;
using KesimTakip.Concretes;
using KesimTakip.DataBase;
using KesimTakip.Entitys;
using KesimTakip.Helper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static iText.Commons.Utils.PlaceHolderTextUtil;

namespace KesimTakip.UsrControl
{
    public partial class ctlKesimDetaylari : UserControl
    {
        private List<KesimDetaylari> tumPozlar;
        private string placeholderText = "Ara"; 

        public ctlKesimDetaylari()
        {
            InitializeComponent();

            YukleVeListele();

            txtArama.TextChanged += TxtAra_TextChanged;
            lstPozlar.SelectedIndexChanged += LstPozlar_SelectedIndexChanged;
            ListBoxHelper.StilUygula(lstPozlar);

            panelKart1.BackColor = Color.FromArgb(0, 95, 107);
            panelKart2.BackColor = Color.FromArgb(191, 128, 255);
            panelKart3.BackColor = Color.FromArgb(255, 83, 97);

            OrtalaLabel(lblKesilecekPoz, panelKart1);
            OrtalaLabel(lblKesilmisPoz, panelKart2);
            OrtalaLabel(lblToplamPoz, panelKart3);

            PanelRoundHelper.RoundCorners(panelKart1, 20);
            PanelRoundHelper.RoundCorners(panelKart2, 20);
            PanelRoundHelper.RoundCorners(panelKart3, 20);
            RoundedPanelHelper.ApplyRoundedBorder(panelList, 10);
            RoundedPanelHelper.ApplyRoundedBorder(panelHeader, 10);
            RoundedPanelHelper.ApplyRoundedBorder(panelSearch, 10);
            RoundedPanelHelper.ApplyRoundedBorder(panelChart, 10);
            RoundedPanelHelper.ApplyRoundedBorder(panelKartContainer, 10);

            panelHeader.BackColor = ColorTranslator.FromHtml("#2C3E50");
            panelKartContainer.BackColor = ColorTranslator.FromHtml("#2C3E50");

            panelDisContainer.BackColor = ColorTranslator.FromHtml("#ADD8E6");
            panel2.BackColor = ColorTranslator.FromHtml("#ADD8E6");

            txtArama.Multiline = true;
            txtArama.Height = 35;
            txtArama.Font = new Font("Segoe UI", 14F);
        }
        private void YukleVeListele()
        {
            var dt = KesimDetaylariData.GetKesimDetaylari();
            if (dt == null || dt.Rows.Count == 0)
                return;

            tumPozlar = dt.AsEnumerable()
                .GroupBy(row => row["poz"].ToString())
                .Select(gr => new KesimDetaylari
                {
                    poz = gr.Key,
                    toplamAdet = gr.Sum(r => Convert.ToInt32(r["toplamAdet"])),
                    kesilecekAdet = gr.Sum(r => Convert.ToInt32(r["kesilecekAdet"])),
                    kesilmisAdet = gr.Sum(r => Convert.ToInt32(r["kesilmisAdet"]))
                }).OrderBy(x => x.poz).ToList();

            PozlariListele(tumPozlar);
        }

        private void PozlariListele(List<KesimDetaylari> pozlar)
        {
            lstPozlar.DataSource = null;
            lstPozlar.DataSource = pozlar;
            lstPozlar.DisplayMember = "poz";
        }

        private void TxtAra_TextChanged(object sender, EventArgs e)
        {
            if (txtArama.Text == placeholderText || string.IsNullOrWhiteSpace(txtArama.Text))
            {
                PozlariListele(tumPozlar);
                return;
            }

            var filtre = txtArama.Text.ToLower();
            var filtrelenmis = tumPozlar
                .Where(p => p.poz.ToLower().Contains(filtre))
                .ToList();

            PozlariListele(filtrelenmis);
        }

        private void LstPozlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPozlar.SelectedItem is KesimDetaylari secilen)
            {
                chartKesim.Series.Clear();
                if (chartKesim.ChartAreas.Count == 0)
                    chartKesim.ChartAreas.Add(new ChartArea());

                var seri = new Series(secilen.poz);

                int index1 = seri.Points.AddXY("Kesilecek Adet", secilen.kesilecekAdet);
                seri.Points[index1].Color = Color.FromArgb(0, 95, 107);

                int index2 = seri.Points.AddXY("Kesilmiş Adet", secilen.kesilmisAdet);
                seri.Points[index2].Color = Color.FromArgb(191, 128, 255);

                int index3 = seri.Points.AddXY("Toplam Adet", secilen.toplamAdet);
                seri.Points[index3].Color = Color.FromArgb(255, 83, 97);

                chartKesim.Series.Add(seri);

                lblKesilecekPoz.Text = secilen.kesilecekAdet.ToString();
                panelKart1.BackColor = Color.FromArgb(0, 95, 107);
                lblKesilmisPoz.Text = secilen.kesilmisAdet.ToString();
                panelKart2.BackColor = Color.FromArgb(191, 128, 255);
                lblToplamPoz.Text = secilen.toplamAdet.ToString();
                panelKart3.BackColor = Color.FromArgb(255, 83, 97);
            }

        }
        private void OrtalaLabel(Label lbl, Panel pnl)
        {
            int x = (pnl.Width - lbl.Width) / 2;
            int y = (pnl.Height - lbl.Height) / 2;
            lbl.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
        }
        private void panel3_Resize(object sender, EventArgs e)
        {
            int spacing = 60;
            int totalWidth = panelKart1.Width + panelKart2.Width + panelKart3.Width + (2 * spacing);
            int startX = Math.Max((panelKartContainer.Width - totalWidth) / 2, 0);

            int y1 = Math.Max((panelKartContainer.Height - panelKart1.Height) / 2, 0);
            int y2 = Math.Max((panelKartContainer.Height - panelKart2.Height) / 2, 0);
            int y3 = Math.Max((panelKartContainer.Height - panelKart3.Height) / 2, 0);

            panelKart1.Location = new Point(startX, y1);
            panelKart2.Location = new Point(startX + panelKart1.Width + spacing, y2);
            panelKart3.Location = new Point(startX + panelKart1.Width + spacing + panelKart2.Width + spacing, y3);

        }

        private void ctlKesimDetaylari_Load(object sender, EventArgs e)
        {
            txtArama.Text = placeholderText;
            txtArama.ForeColor = Color.Gray; 
            txtArama.GotFocus += TextBox1_GotFocus;
            txtArama.LostFocus += TextBox1_LostFocus;


            ChartiTiklanmazYap();
        }
        private void ChartiTiklanmazYap()
        {
            ClickThroughPanel engelleyici = new ClickThroughPanel();
            engelleyici.Bounds = chartKesim.Bounds;
            engelleyici.Parent = chartKesim.Parent;
            engelleyici.BringToFront();

            chartKesim.SendToBack();
        }
        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            if (txtArama.Text == placeholderText)
            {
                txtArama.Text = "";
                txtArama.ForeColor = Color.Black;
            }
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArama.Text))
            {
                txtArama.Text = placeholderText;
                txtArama.ForeColor = Color.Gray;
            }
        }

        private void ctlKesimDetaylari_Resize(object sender, EventArgs e)
        {
            Refresh();
            ChartiTiklanmazYap();
        }
    }
}
