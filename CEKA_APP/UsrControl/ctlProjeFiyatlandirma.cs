using CEKA_APP.DataBase.ProjeFinans;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeFiyatlandirma : UserControl
    {
        private IscilikData iscilikData = new IscilikData();
        private ProjeFiyatlandirmaData fiyatlandirmaData = new ProjeFiyatlandirmaData();

        public ctlProjeFiyatlandirma()
        {
            InitializeComponent();
        }

        private void ctlProjeFiyatlandirma_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Proje Fiyatlandırma";
        }

        private void btnProjeAra_Click(object sender, EventArgs e)
        {
            LoadIscilikler();
        }

        private void LoadIscilikler()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Clear();

            tableLayoutPanel1.Controls.Add(new Label { Text = "İşçilik", TextAlign = ContentAlignment.MiddleCenter }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label { Text = "Teklif Toplam", TextAlign = ContentAlignment.MiddleCenter }, 1, 0);
            tableLayoutPanel1.Controls.Add(new Label { Text = "Gerçekleşen Maliyet", TextAlign = ContentAlignment.MiddleCenter }, 2, 0);

            string projeAdi = txtProjeNo.Text.Trim();
            if (!string.IsNullOrEmpty(projeAdi))
            {
                var fiyatlandirmalar = fiyatlandirmaData.GetFiyatlandirmaByProje(projeAdi);
                int row = 1;
                foreach (var (IscilikAdi, Teklif, Maliyet) in fiyatlandirmalar)
                {
                    AddIscilikRow(IscilikAdi, row++);
                    var txtTeklif = (TextBox)tableLayoutPanel1.GetControlFromPosition(1, row - 1);
                    var txtMaliyet = (TextBox)tableLayoutPanel1.GetControlFromPosition(2, row - 1);
                    if (txtTeklif != null) txtTeklif.Text = Teklif.ToString();
                    if (txtMaliyet != null) txtMaliyet.Text = Maliyet.ToString();
                }
                EnsureEkleButtonAtBottom();
            }
            else
            {
                EnsureEkleButtonAtBottom();
            }
        }

        private void AddIscilikRow(string iscilikAdi, int row)
        {
            tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel1.Controls.Add(new Label { Text = iscilikAdi, TextAlign = ContentAlignment.MiddleCenter }, 0, row);
            tableLayoutPanel1.Controls.Add(new TextBox { Name = $"txtTeklif_{row}", TextAlign = HorizontalAlignment.Center }, 1, row);
            tableLayoutPanel1.Controls.Add(new TextBox { Name = $"txtMaliyet_{row}", TextAlign = HorizontalAlignment.Center }, 2, row);
        }

        private void EnsureEkleButtonAtBottom()
        {
            var existingButton = tableLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "İşçilik Ekle");
            if (existingButton != null)
            {
                tableLayoutPanel1.Controls.Remove(existingButton);
            }

            tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var btnEkleSatir = new Button { Text = "İşçilik Ekle", TextAlign = ContentAlignment.MiddleCenter };
            btnEkleSatir.Click += (s, e) => AddNewIscilikRow(tableLayoutPanel1.RowCount - 1);
            tableLayoutPanel1.Controls.Add(btnEkleSatir, 0, tableLayoutPanel1.RowCount - 1);
        }

        private void AddNewIscilikRow(int row)
        {
            using (var form = new frmYeniIscilikEkle())
            {
                if (form.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(form.IscilikAdi))
                {
                    bool isAlreadyAdded = false;
                    for (int i = 1; i < tableLayoutPanel1.RowCount - 1; i++)
                    {
                        var lbl = tableLayoutPanel1.GetControlFromPosition(0, i) as Label;
                        if (lbl != null && lbl.Text == form.IscilikAdi)
                        {
                            isAlreadyAdded = true;
                            break;
                        }
                    }

                    if (!isAlreadyAdded)
                    {
                        AddIscilikRow(form.IscilikAdi, tableLayoutPanel1.RowCount);
                        EnsureEkleButtonAtBottom();
                    }
                    else
                    {
                        MessageBox.Show("Bu işçilik zaten eklenmiş!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            string projeAdi = txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(projeAdi))
            {
                MessageBox.Show("Proje adı giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool hasData = false;
            var existingFiyatlandirmalar = fiyatlandirmaData.GetFiyatlandirmaByProje(projeAdi);
            var iscilikler = iscilikData.GetIscilikler();
            bool hasNewData = false;
            List<(string IscilikAdi, int IscilikId, decimal Teklif, decimal Maliyet, bool IsUpdate)> entries = new List<(string, int, decimal, decimal, bool)>();

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                var lbl = tableLayoutPanel1.GetControlFromPosition(0, row) as Label;
                var txtTeklif = tableLayoutPanel1.GetControlFromPosition(1, row) as TextBox;
                var txtMaliyet = tableLayoutPanel1.GetControlFromPosition(2, row) as TextBox;
                if (lbl != null && txtTeklif != null && txtMaliyet != null)
                {
                    string iscilikAdi = lbl.Text;
                    int iscilikId = iscilikler.First(x => x.Adi == iscilikAdi).Id;
                    decimal teklif = decimal.TryParse(txtTeklif.Text, out decimal t) ? t : 0;
                    decimal maliyet = decimal.TryParse(txtMaliyet.Text, out decimal m) ? m : 0;

                    var existing = existingFiyatlandirmalar.FirstOrDefault(x => x.Item1 == iscilikAdi);
                    bool isUpdate = false;
                    if (existing.Item1 != null)
                    {
                        if (teklif != existing.Item2 || maliyet != existing.Item3)
                        {
                            hasNewData = true;
                            isUpdate = true;
                        }
                    }
                    else
                    {
                        hasNewData = true;
                    }

                    if (teklif > 0 || maliyet > 0) hasData = true;
                    entries.Add((iscilikAdi, iscilikId, teklif, maliyet, isUpdate));
                }
            }

            if (!hasData)
            {
                MessageBox.Show("Lütfen en az bir işçilik için teklif veya maliyet giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!hasNewData)
            {
                MessageBox.Show("Bu proje için bu işçilikler daha önce kaydedilmiş. Yeni veri girilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var entry in entries)
            {
                if (entry.IsUpdate)
                {
                    fiyatlandirmaData.FiyatlandirmaGuncelle(projeAdi, entry.IscilikId, entry.Teklif, entry.Maliyet);
                }
                else if (entry.Item1 != null)
                {
                    fiyatlandirmaData.FiyatlandirmaKaydet(projeAdi, entry.IscilikId, entry.Teklif, entry.Maliyet);
                }
            }

            MessageBox.Show("Veriler kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadIscilikler();
        }
        public void LoadProjeFiyatlandirma(string projeNo, bool autoSearch = false)
        {
            txtProjeNo.Text = projeNo;
            if (autoSearch)
            {
                LoadIscilikler();
            }
        }
    }
}