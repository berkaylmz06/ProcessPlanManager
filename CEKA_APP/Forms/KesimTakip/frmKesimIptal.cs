using CEKA_APP.Interfaces.KesimTakip;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.Forms.KesimTakip
{
    public partial class frmKesimIptal : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _kesimEmriNo;
        private readonly string _operatorAd;
        private readonly IKesimListesiService _kesimListesiService;
        private Dictionary<string, decimal> _kesimAdetleri;

        public string IptalNedeni => txtIptalNedeni.Text.Trim();
        public Dictionary<string, decimal> KesimAdetleri => _kesimAdetleri;

        public frmKesimIptal(IServiceProvider serviceProvider, string kesimEmriNo, string operatorAd)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _kesimEmriNo = kesimEmriNo;
            _operatorAd = operatorAd;
            _kesimListesiService = _serviceProvider.GetRequiredService<IKesimListesiService>();
            _kesimAdetleri = new Dictionary<string, decimal>();

            LoadKesimListesi();
            this.Icon = Properties.Resources.cekalogokirmizi;

            Helper.DataGridViewHelper.StilUygula(dgvKesimAdetleri);
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
                    newRow["KesilenAdet"] = 0;
                    combinedTable.Rows.Add(newRow);
                }
            }

            dgvKesimAdetleri.DataSource = combinedTable;
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIptalNedeni.Text.Trim()))
            {
                MessageBox.Show("Lütfen iptal nedeni girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show($"Geçersiz kesilen adet değeri: {pozKey}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _kesimAdetleri[pozKey] = kesilenAdet;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}