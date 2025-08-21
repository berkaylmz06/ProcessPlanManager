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
using CEKA_APP.DataBase; // Veritabanı sınıfınızın bulunduğu namespace

namespace CEKA_APP.UsrControl.ProjeFinans
{
    public partial class ctlTakipTakvimi : UserControl
    {
        private DateTime _currentDate;

        public ctlTakipTakvimi()
        {
            InitializeComponent();
            _currentDate = DateTime.Now;
        }

        private void ctlTakipTakvimi_Load(object sender, EventArgs e)
        {
            TakvimiOlustur(_currentDate);
        }

        private void TakvimiOlustur(DateTime tarih)
        {
            // tlpTakvim'i temizle ve başlık panelini koru
            tlpTakvim.Controls.Clear();
            lblTarih.Text = tarih.ToString("MMMM yyyy", new CultureInfo("tr-TR"));

            // Haftanın günlerini başlık olarak ekle
            string[] gunler = { "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt", "Paz" };
            for (int i = 0; i < 7; i++)
            {
                Label lblGunAdi = new Label()
                {
                    Text = gunler[i],
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.LightGray,
                    Font = new Font("Arial", 10F, FontStyle.Bold)
                };
                tlpTakvim.Controls.Add(lblGunAdi, i, 0);
            }

            // Veritabanından o aya ait verileri çek
            DataTable takvimVerileri = TakvimData.GetTakvimVerileri(tarih.Year, tarih.Month);

            int gunSayisi = DateTime.DaysInMonth(tarih.Year, tarih.Month);
            DateTime ilkGun = new DateTime(tarih.Year, tarih.Month, 1);

            // DayOfWeek.Monday = 1, DayOfWeek.Tuesday = 2... DayOfWeek.Sunday = 0
            // Takvimde haftayı Pazartesi'den başlatmak için gerekli boşluk hesaplaması
            int bosluk;
            if (ilkGun.DayOfWeek == DayOfWeek.Sunday)
            {
                bosluk = 6; // Pazar ise 6 boşluk
            }
            else
            {
                bosluk = (int)ilkGun.DayOfWeek - 1; // Pazartesi = 1 ise 0 boşluk, Salı = 2 ise 1 boşluk
            }

            // Günleri ekle
            for (int i = 1; i <= gunSayisi; i++)
            {
                DataRow[] olaylar = takvimVerileri.Select($"Gun = {i}");
                string olayMetni = "";

                if (olaylar.Length > 0)
                {
                    olayMetni = $"\n({olaylar[0]["Olay"].ToString()})";
                }

                Label lblGun = new Label()
                {
                    Text = $"{i}{olayMetni}",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Padding = new Padding(3),
                    Font = new Font("Arial", 9F, FontStyle.Regular),
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = i
                };

                lblGun.Click += Gun_Click;

                int row = (i + bosluk - 1) / 7 + 1; // Satır numarasını hesapla
                int col = (i + bosluk - 1) % 7;     // Sütun numarasını hesapla

                if (row < tlpTakvim.RowCount && col < tlpTakvim.ColumnCount)
                {
                    tlpTakvim.Controls.Add(lblGun, col, row);
                }
            }
        }
        private void btnOncekiAy_Click(object sender, EventArgs e)
        {
            _currentDate = _currentDate.AddMonths(-1);
            TakvimiOlustur(_currentDate);
        }

        private void btnSonrakiAy_Click(object sender, EventArgs e)
        {
            _currentDate = _currentDate.AddMonths(1);
            TakvimiOlustur(_currentDate);
        }

        private void Gun_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            if (clickedLabel != null)
            {
                MessageBox.Show($"Tıkladığınız gün: {clickedLabel.Tag}");
                // Burada o güne ait detayları göstermek için farklı bir işlem yapabilirsiniz.
            }
        }
    }

    // Bu sınıf, veritabanından takvim verilerini çekmek için bir örnektir.
    // Kendi projenizdeki veritabanı erişim katmanına göre düzenlemelisiniz.
    public static class TakvimData
    {
        public static DataTable GetTakvimVerileri(int yil, int ay)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Gun", typeof(int));
            dt.Columns.Add("Olay", typeof(string));

            // Örnek veriler (veritabanından geldiğini varsayalım)
            if (yil == DateTime.Now.Year && ay == DateTime.Now.Month)
            {
                dt.Rows.Add(15, "Toplantı");
                dt.Rows.Add(20, "Proje Teslimi");
                dt.Rows.Add(25, "Sunum");
            }
            // Diğer aylar için de örnek veriler eklenebilir.

            return dt;
        }
    }
}