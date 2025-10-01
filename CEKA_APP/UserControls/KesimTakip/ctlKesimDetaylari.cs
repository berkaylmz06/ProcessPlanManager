using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.KesimTakip;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CEKA_APP.UsrControl
{
    public partial class ctlKesimDetaylari : UserControl
    {
        private List<KesimDetaylari> tumPozlar;
        private string placeholderText = "Ara";
        private readonly int unlemGenislik = 20;

        private readonly IServiceProvider _serviceProvider;

        private IKesimDetaylariService _kesimDetaylariService => _serviceProvider.GetRequiredService<IKesimDetaylariService>();
        private IAutoCadAktarimService _autoCadAktarimService => _serviceProvider.GetRequiredService<IAutoCadAktarimService>();
        public ctlKesimDetaylari(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            lstPozlar.DrawMode = DrawMode.OwnerDrawFixed;
            lstPozlar.ItemHeight = 25;
            lstPozlar.MouseDown += LstPozlar_MouseDown;

            YukleVeListele();

            txtArama.TextChanged += TxtAra_TextChanged;
            lstPozlar.SelectedIndexChanged += LstPozlar_SelectedIndexChanged;
            ListBoxHelper.StilUygula(lstPozlar); 

            panelKart1.BackColor = Color.FromArgb(0, 95, 107);
            panelKart2.BackColor = Color.FromArgb(191, 128, 255);
            panelKart3.BackColor = Color.FromArgb(255, 83, 97);
            panelKart4.BackColor = Color.FromArgb(15, 48, 56);

            OrtalaLabel(lblKesilecekPoz, panelKart1);
            OrtalaLabel(lblKesilmisPoz, panelKart2);
            OrtalaLabel(lblToplamPoz, panelKart3);
            OrtalaLabel(lblToplamPozIfsKarsiligi, panelKart4);

            PanelRoundHelper.RoundCorners(panelKart1, 20);
            PanelRoundHelper.RoundCorners(panelKart2, 20);
            PanelRoundHelper.RoundCorners(panelKart3, 20);
            PanelRoundHelper.RoundCorners(panelKart4, 20);
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
            var dt = _kesimDetaylariService.GetKesimDetaylariBilgi();
            if (dt == null || dt.Rows.Count == 0)
                return;

            tumPozlar = dt.AsEnumerable()
                .GroupBy(row => new
                {
                    kalite = row["kalite"].ToString(),
                    malzeme = row["malzeme"].ToString(),
                    malzemeKod = row["malzemeKod"].ToString(),
                    proje = row["proje"].ToString()
                })
                .Select(gr => new KesimDetaylari
                {
                    kalite = gr.Key.kalite,
                    malzeme = gr.Key.malzeme,
                    malzemeKod = gr.Key.malzemeKod,
                    proje = gr.Key.proje,
                    toplamAdet = gr.Sum(r => Convert.ToDecimal(r["toplamAdet"])),
                    kesilecekAdet = gr.Sum(r => Convert.ToDecimal(r["kesilecekAdet"])),
                    kesilmisAdet = gr.Sum(r => Convert.ToDecimal(r["kesilmisAdet"])),
                    ekBilgi = gr.Any(r => r["ekBilgi"] != DBNull.Value && Convert.ToBoolean(r["ekBilgi"])),
                    ekBilgiMesaji = gr.Any(r => r["ekBilgi"] != DBNull.Value && Convert.ToBoolean(r["ekBilgi"]))
                        ? "Bu poz, Ek'lere ayrılmıştır."  : ""
                })
                .OrderBy(x => x.kalite)
                .ThenBy(x => x.malzeme)
                .ThenBy(x => x.malzemeKod)
                .ThenBy(x => x.proje)
                .ToList();

            PozlariListele(tumPozlar);
        }

        private void PozlariListele(List<KesimDetaylari> kesimDetaylari)
        {
            lstPozlar.DataSource = null;
            lstPozlar.DataSource = kesimDetaylari;
        }

        private void LstPozlar_MouseDown(object sender, MouseEventArgs e)
        {
            int index = lstPozlar.IndexFromPoint(e.Location);
            if (index < 0 || index >= lstPozlar.Items.Count) return;

            KesimDetaylari veri = (KesimDetaylari)lstPozlar.Items[index];
            if (!veri.ekBilgi) return;

            Rectangle itemRect = lstPozlar.GetItemRectangle(index);
            bool hasScrollBar = lstPozlar.Items.Count * lstPozlar.ItemHeight > lstPozlar.ClientSize.Height;
            int scrollBarWidth = hasScrollBar ? SystemInformation.VerticalScrollBarWidth : 0;
            Rectangle unlemRect = new Rectangle(
                lstPozlar.ClientSize.Width - unlemGenislik - scrollBarWidth,
                itemRect.Top,
                unlemGenislik,
                itemRect.Height
            );

            if (unlemRect.Contains(e.Location))
            {
                MessageBox.Show(veri.ekBilgiMesaji, "Ek Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void TxtAra_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);

            if (txtArama.Text == placeholderText || string.IsNullOrWhiteSpace(txtArama.Text))
            {
                PozlariListele(tumPozlar ?? new List<KesimDetaylari>());
                return;
            }

            var filtre = txtArama.Text.Trim().ToLower();

            if (tumPozlar == null || !tumPozlar.Any())
            {
                PozlariListele(new List<KesimDetaylari>());
                return;
            }

            var filtrelenmis = tumPozlar
                .Where(p =>
                    (p.kalite?.ToLower().Contains(filtre) ?? false) ||
                    (p.malzeme?.ToLower().Contains(filtre) ?? false) ||
                    (p.malzemeKod?.ToLower().Contains(filtre) ?? false) ||
                    (p.proje?.ToLower().Contains(filtre) ?? false) ||
                    (p.poz?.ToLower().Contains(filtre) ?? false)
                )
                .OrderBy(p => p.kalite)
                .ThenBy(p => p.malzeme)
                .ThenBy(p => p.malzemeKod)
                .ThenBy(p => p.proje)
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

                string[] pozParcalari = secilen.poz.Split('-');
                if (pozParcalari.Length == 6)
                {
                    string kalite = pozParcalari[0]; 
                    string malzeme = pozParcalari[1];
                    string malzemeKodIlkKisim = pozParcalari[2]; 
                    string malzemeKodIkinciKisim = pozParcalari[3]; 
                    string malzemeKodUcuncuKisim = pozParcalari[4]; 
                    string proje = pozParcalari[5]; 

                    decimal toplamAdet, kesilmisAdet, kesilecekAdet;
                    List<string> eslesenPozlar = new List<string>();

                    if (secilen.ekBilgi)
                    {
                        var (toplam, kesilmis, kesilecek, pozlar) = _kesimDetaylariService.GetAdetlerVeEslesenPozlar(
                            kalite,
                            malzeme,
                            proje,
                            malzemeKodIlkKisim,
                            malzemeKodUcuncuKisim
                        );
                        toplamAdet = toplam;
                        kesilmisAdet = kesilmis;
                        kesilecekAdet = kesilecek;
                        eslesenPozlar = pozlar;

                        if (eslesenPozlar.Count > 1)
                        {
                            eslesenPozlar.Remove(secilen.poz); 
                            if (eslesenPozlar.Any())
                            {
                                lblBilgi.Text = $"Bu poz, {string.Join(", ", eslesenPozlar)} pozlarıyla örtüştüğünden, kesilecek poz, kesilen poz ve toplam poz bilgileri ortak değerlendirilmektedir.";
                            }
                            else
                            {
                                lblBilgi.Text = " ";
                            }
                        }
                        else
                        {
                            lblBilgi.Text = " ";
                        }
                    }
                    else
                    {
                        toplamAdet = secilen.toplamAdet;
                        kesilmisAdet = secilen.kesilmisAdet;
                        kesilecekAdet = secilen.kesilecekAdet;
                        lblBilgi.Text = " ";
                    }

                    lblToplamPoz.Text = DecimalToTrimmedString(toplamAdet);
                    lblKesilmisPoz.Text = DecimalToTrimmedString(kesilmisAdet);
                    lblKesilecekPoz.Text = DecimalToTrimmedString(kesilecekAdet);

                    int index1 = seri.Points.AddXY("Kesilecek Adet", Convert.ToDouble(kesilecekAdet));
                    seri.Points[index1].Color = Color.FromArgb(0, 95, 107);

                    int index2 = seri.Points.AddXY("Kesilmiş Adet", Convert.ToDouble(kesilmisAdet));
                    seri.Points[index2].Color = Color.FromArgb(191, 128, 255);

                    int index3 = seri.Points.AddXY("Toplam Adet", Convert.ToDouble(toplamAdet));
                    seri.Points[index3].Color = Color.FromArgb(255, 83, 97);


                    chartKesim.Series.Add(seri);

                    string orijinalMalzemeKod = $"{pozParcalari[2]}-{pozParcalari[3]}-{pozParcalari[4]}";
                    string malzemeKod = NormalizeMalzemeKod(orijinalMalzemeKod);
                    var (uygunMu, toplamAdetIFS) = _autoCadAktarimService.AdetGetir(
                        kalite,
                        malzeme,
                        malzemeKod,
                        proje,
                        kesilmisAdet
                    );
                    lblToplamPozIfsKarsiligi.Text = toplamAdetIFS.ToString();
                    int index4 = seri.Points.AddXY("IFS Toplam Poz", toplamAdetIFS);
                    seri.Points[index4].Color = Color.FromArgb(15, 48, 56);
                    panelKart4.BackColor = uygunMu ? Color.FromArgb(15, 48, 56) : Color.Red;
                }
                else
                {
                    lblToplamPoz.Text = "Hatalı poz";
                    lblKesilmisPoz.Text = "Hatalı poz";
                    lblKesilecekPoz.Text = "Hatalı poz";
                    lblToplamPozIfsKarsiligi.Text = "Hatalı poz";
                    lblBilgi.Text = "Hatalı poz formatı.";
                    panelKart4.BackColor = Color.Red;
                }

                panelKart1.BackColor = Color.FromArgb(0, 95, 107);
                panelKart2.BackColor = Color.FromArgb(191, 128, 255);
                panelKart3.BackColor = Color.FromArgb(255, 83, 97);
            }
        }
        private string DecimalToTrimmedString(decimal value)
        {
            return value.ToString("G29");
        }
        private string NormalizeMalzemeKod(string malzemeKod)
        {
            if (string.IsNullOrWhiteSpace(malzemeKod)) return malzemeKod;

            var parts = malzemeKod.Split('-');
            if (parts.Length != 3) return malzemeKod;

            string kalipKodu = $"{parts[0]}-{parts[1]}";

            if (_autoCadAktarimService.GetirStandartGruplar(kalipKodu))
            {
                return malzemeKod; 
            }

            return $"{parts[0]}-00-{parts[2]}";
        }
        private void OrtalaLabel(Label lbl, Panel pnl)
        {
            int x = (pnl.Width - lbl.Width) / 2;
            int y = (pnl.Height - lbl.Height) / 2;
            lbl.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
        }

        private void panel3_Resize(object sender, EventArgs e)
        {
            int margin = 30;

            int y1 = Math.Max((panelKartContainer.Height - panelKart1.Height) / 2, 0);
            int y2 = Math.Max((panelKartContainer.Height - panelKart2.Height) / 2, 0);
            int y3 = Math.Max((panelKartContainer.Height - panelKart3.Height) / 2, 0);
            int y4 = Math.Max((panelKartContainer.Height - panelKart4.Height) / 2, 0);

            int totalCardWidth = panelKart1.Width + panelKart2.Width + panelKart3.Width + panelKart4.Width;

            int availableSpacing = panelKartContainer.Width - totalCardWidth - (2 * margin);

            int spacing = Math.Max(availableSpacing / 3, 0);

            panelKart1.Location = new Point(margin, y1);
            panelKart2.Location = new Point(panelKart1.Location.X + panelKart1.Width + spacing, y2);
            panelKart3.Location = new Point(panelKart2.Location.X + panelKart2.Width + spacing, y3);
            panelKart4.Location = new Point(panelKart3.Location.X + panelKart3.Width + spacing, y4);
        }
        private void ctlKesimDetaylari_Load(object sender, EventArgs e)
        {
            txtArama.Text = placeholderText;
            txtArama.ForeColor = Color.Gray;
            txtArama.GotFocus += TextBox1_GotFocus;
            txtArama.LostFocus += TextBox1_LostFocus;

            ChartiTiklanmazYap();

            ctlBaslik1.Baslik = "Kesim Detayları";
        }

        private void ChartiTiklanmazYap()
        {
            ChartiTiklanmazYap engelleyici = new ChartiTiklanmazYap();
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