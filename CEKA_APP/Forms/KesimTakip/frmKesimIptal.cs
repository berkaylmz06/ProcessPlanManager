using CEKA_APP.Interfaces.KesimTakip;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CEKA_APP.Forms.KesimTakip;

namespace CEKA_APP.Forms.KesimTakip
{
    public partial class frmKesimIptal : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _kesimEmriNo;
        private readonly string _operatorAd;
        private readonly IKesimListesiService _kesimListesiService;
        private Dictionary<string, decimal> _kesimAdetleri;
        public Dictionary<string, List<YanUrunDetay>> YanUrunVerileri { get; private set; }


        public string IptalNedeni
        {
            get
            {
                return comboBoxIptalNedeniSec.SelectedItem?.ToString() ?? string.Empty;
            }
        }
        public Dictionary<string, decimal> KesimAdetleri => _kesimAdetleri;

        public frmKesimIptal(IServiceProvider serviceProvider, string kesimEmriNo, string operatorAd)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _kesimEmriNo = kesimEmriNo;
            _operatorAd = operatorAd;
            _kesimListesiService = _serviceProvider.GetRequiredService<IKesimListesiService>();
            _kesimAdetleri = new Dictionary<string, decimal>();
            YanUrunVerileri = new Dictionary<string, List<YanUrunDetay>>();

            LoadKesimListesi();
            LoadIptalNedenleri();
            this.Icon = Properties.Resources.cekalogokirmizi;

            Helper.DataGridViewHelper.StilUygula(dgvKesimAdetleri);

            dgvKesimAdetleri.CellValidating += DgvKesimAdetleri_CellValidating;

            dgvKesimAdetleri.CellFormatting += (s, ev) =>
            {
                if (ev.ColumnIndex == dgvKesimAdetleri.Columns["Poz Adet"].Index && ev.Value != null)
                {
                    if (decimal.TryParse(ev.Value.ToString(), out decimal val))
                    {
                        ev.Value = val.ToString("G29", System.Globalization.CultureInfo.CurrentCulture);
                        ev.FormattingApplied = true;
                    }
                }
            };
        }

        private void LoadIptalNedenleri()
        {
            comboBoxIptalNedeniSec.Items.Clear();

            comboBoxIptalNedeniSec.Items.Add("Malzeme Hasarlı/Uygunsuz");
            comboBoxIptalNedeniSec.Items.Add("Ölçü Hatası (Genişlik/Uzunluk)");
            comboBoxIptalNedeniSec.Items.Add("Makine Arızası");
            comboBoxIptalNedeniSec.Items.Add("Yanlış Malzeme Seçimi (Kalite)");
            comboBoxIptalNedeniSec.Items.Add("Yanlış Malzeme Seçimi (Kalınlık)");
            comboBoxIptalNedeniSec.Items.Add("Operatör Hatası");
            comboBoxIptalNedeniSec.Items.Add("Enerji Kesintisi");
            comboBoxIptalNedeniSec.Items.Add("ERP/Planlama Hatası");
            comboBoxIptalNedeniSec.Items.Add("Revizyon");
        }


        private void LoadKesimListesi()
        {
            var kesimIds = _kesimEmriNo.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            DataTable combinedTable = new DataTable();
            combinedTable.Columns.Add("KesimId", typeof(string));
            combinedTable.Columns.Add("Kalite", typeof(string));
            combinedTable.Columns.Add("Malzeme", typeof(string));
            combinedTable.Columns.Add("KalipNo", typeof(string));
            combinedTable.Columns.Add("Poz", typeof(string));
            combinedTable.Columns.Add("Proje", typeof(string));
            combinedTable.Columns.Add("Poz Adet", typeof(decimal));
            combinedTable.Columns.Add("KesilenAdet", typeof(decimal));

            foreach (var kesimId in kesimIds)
            {
                var dt = _kesimListesiService.GetirKesimListesi(kesimId);
                foreach (DataRow row in dt.Rows)
                {
                    var newRow = combinedTable.NewRow();
                    newRow["KesimId"] = kesimId;
                    newRow["Kalite"] = row["kalite"];
                    newRow["Malzeme"] = row["malzeme"];
                    newRow["KalipNo"] = row["kalipNo"];
                    newRow["Poz"] = row["kesilecekPozlar"];
                    newRow["Proje"] = row["projeNo"];
                    newRow["Poz Adet"] = row["kpAdetSayilari"];
                    newRow["KesilenAdet"] = 0;
                    combinedTable.Rows.Add(newRow);
                }
            }

            dgvKesimAdetleri.DataSource = combinedTable;

            dgvKesimAdetleri.Columns["KesimId"].HeaderText = "Kesim ID";
            dgvKesimAdetleri.Columns["Kalite"].HeaderText = "Malzeme Kalite";
            dgvKesimAdetleri.Columns["Malzeme"].HeaderText = "Malzeme Tanımı";
            dgvKesimAdetleri.Columns["KalipNo"].HeaderText = "Kalıp No";
            dgvKesimAdetleri.Columns["Poz"].HeaderText = "Kesilecek Poz";
            dgvKesimAdetleri.Columns["Proje"].HeaderText = "Proje No";
            dgvKesimAdetleri.Columns["Poz Adet"].HeaderText = "Poz Adedi";
            dgvKesimAdetleri.Columns["KesilenAdet"].HeaderText = "İptal Edilecek Adet";

            foreach (DataGridViewColumn col in dgvKesimAdetleri.Columns)
            {
                if (col.Name != "KesilenAdet")
                {
                    col.ReadOnly = true;
                }
            }
        }

        private void DgvKesimAdetleri_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvKesimAdetleri.Columns[e.ColumnIndex].Name == "KesilenAdet")
            {
                if (!decimal.TryParse(dgvKesimAdetleri.Rows[e.RowIndex].Cells["Poz Adet"].Value?.ToString(), out decimal pozAdet))
                {
                    MessageBox.Show("Poz Adet değeri okunamadı. Lütfen sistem yöneticinizle iletişime geçin.", "Veri Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                if (decimal.TryParse(e.FormattedValue.ToString(), out decimal kesilenAdet))
                {
                    if (kesilenAdet > pozAdet)
                    {
                        e.Cancel = true;
                        dgvKesimAdetleri.Rows[e.RowIndex].ErrorText = $"İptal Edilecek Adet ({kesilenAdet}) Poz Adedi ({pozAdet}) geçemez.";
                        MessageBox.Show($"İptal Edilecek Adet ({kesilenAdet}) Poz Adedi ({pozAdet}) geçemez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (kesilenAdet < 0)
                    {
                        e.Cancel = true;
                        dgvKesimAdetleri.Rows[e.RowIndex].ErrorText = "İptal Edilecek Adet negatif olamaz.";
                        MessageBox.Show("İptal Edilecek Adet negatif olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        dgvKesimAdetleri.Rows[e.RowIndex].ErrorText = String.Empty;
                    }
                }
                else
                {
                    e.Cancel = true;
                    dgvKesimAdetleri.Rows[e.RowIndex].ErrorText = "Lütfen geçerli bir sayısal değer girin.";
                    MessageBox.Show("Lütfen geçerli bir sayısal değer girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.IptalNedeni))
            {
                MessageBox.Show("Lütfen bir iptal nedeni seçin veya girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in dgvKesimAdetleri.Rows)
            {
                string kalite = row.Cells["Kalite"].Value?.ToString();
                string malzeme = row.Cells["Malzeme"].Value?.ToString();
                string kalipNo = row.Cells["KalipNo"].Value?.ToString();
                string poz = row.Cells["Poz"].Value?.ToString();
                string proje = row.Cells["Proje"].Value?.ToString();
                string pozKey = $"{kalite}-{malzeme}-{kalipNo}-{poz}-{proje}";

                if (!decimal.TryParse(row.Cells["KesilenAdet"].Value?.ToString(), out decimal kesilenAdet) || kesilenAdet < 0)
                {
                    MessageBox.Show($"Geçersiz iptal adedi değeri: {pozKey} - Değer negatif veya sayısal değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (decimal.TryParse(row.Cells["Poz Adet"].Value?.ToString(), out decimal pozAdet) && kesilenAdet > pozAdet)
                {
                    MessageBox.Show($"'Tamam' butonuna basmadan önce İptal Edilecek Adet ({kesilenAdet}) Poz Adedi ({pozAdet}) geçemez. Lütfen hatalı hücreleri düzeltin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _kesimAdetleri[pozKey] = kesilenAdet;
            }

            DialogResult result = MessageBox.Show(
                "İptal edilen kesimler için yan ürün bilgisi girilecek mi?",
                "Yan Ürün Girişi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                var kesimIds = _kesimEmriNo.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();

                using (var frmYanUrun = new frmYanUrunGiris(_serviceProvider, kesimIds))
                {
                    if (frmYanUrun.ShowDialog() == DialogResult.OK)
                    {
                        this.YanUrunVerileri = frmYanUrun.YanUrunVerileriByKesimId;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Yan Ürün Girişi iptal edildi. Kesim İptal işlemi de iptal ediliyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}