using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using KesimTakip.DataBase;
using KesimTakip.Entitys;
using KesimTakip.Helper;

namespace KesimTakip.UsrControl
{
    public partial class ctlKesimDetaylari : UserControl
    {
        private List<KesimDetaylari> tumPozlar;

        public ctlKesimDetaylari()
        {
            InitializeComponent();

            YukleVeListele();

            txtArama.TextChanged += TxtAra_TextChanged;
            lstPozlar.SelectedIndexChanged += LstPozlar_SelectedIndexChanged;
            ListBoxHelper.StilUygula(lstPozlar);
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
                // Grafik güncelleme
                chartKesim.Series.Clear();
                if (chartKesim.ChartAreas.Count == 0)
                    chartKesim.ChartAreas.Add(new ChartArea());

                var seri = new Series(secilen.poz)
                {
                    ChartType = SeriesChartType.Pie
                };

                seri.Points.AddXY("Toplam", secilen.toplamAdet);
                seri.Points.AddXY("Kesilecek", secilen.kesilecekAdet);
                seri.Points.AddXY("Kesilmiş", secilen.kesilmisAdet);

                chartKesim.Series.Add(seri);

                // Seçilen pozun detaylarını kart olarak panel3'e ekle
                KartlariPanel3eEkle(secilen.poz);
            }
        }

        private void KartlariPanel3eEkle(string secilenPoz)
        {
            panel3.Controls.Clear();

            var dt = KesimDetaylariData.GetKesimDetaylari();
            if (dt == null || dt.Rows.Count == 0)
                return;

            var filtrelenmisSatirlar = dt.AsEnumerable()
                .Where(r => r["poz"].ToString() == secilenPoz)
                .ToList();

            int kartGenislik = 180;
            int kartYukseklik = 100;
            int kartArasiBosluk = 15;

            int toplamGenislik = filtrelenmisSatirlar.Count * kartGenislik + (filtrelenmisSatirlar.Count - 1) * kartArasiBosluk;

            // Panelin ortasında başlamak için sol boşluk:
            int baslangicX = Math.Max(10, (panel3.Width - toplamGenislik) / 2);
            int y = (panel3.Height - kartYukseklik) / 2; // dikey ortalama

            for (int i = 0; i < filtrelenmisSatirlar.Count; i++)
            {
                var row = filtrelenmisSatirlar[i];

                Panel kart = new Panel
                {
                    Width = kartGenislik,
                    Height = kartYukseklik,
                    BackColor = Color.LightSteelBlue,
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(baslangicX + i * (kartGenislik + kartArasiBosluk), y)
                };

                Label lblPoz = new Label
                {
                    Text = row["poz"].ToString(),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Dock = DockStyle.Top,
                    Height = 30,
                    TextAlign = ContentAlignment.MiddleCenter,
                };

                Label lblToplamAdet = new Label
                {
                    Text = $"Toplam Adet: {row["toplamAdet"]}",
                    Dock = DockStyle.Top,
                    Height = 25,
                    TextAlign = ContentAlignment.MiddleCenter,
                };

                Label lblKesilecekAdet = new Label
                {
                    Text = $"Kesilecek: {row["kesilecekAdet"]}",
                    Dock = DockStyle.Top,
                    Height = 25,
                    TextAlign = ContentAlignment.MiddleCenter,
                };

                kart.Controls.Add(lblKesilecekAdet);
                kart.Controls.Add(lblToplamAdet);
                kart.Controls.Add(lblPoz);

                panel3.Controls.Add(kart);
            }
        }

    }
}
