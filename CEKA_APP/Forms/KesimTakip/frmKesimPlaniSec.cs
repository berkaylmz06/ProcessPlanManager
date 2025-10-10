using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.Forms.KesimTakip
{
    public partial class frmKesimPlaniSec : Form
    {
        IServiceProvider _serviceProvider;
        private IKesimListesiPaketService _kesimListesiPaketService=> _serviceProvider.GetRequiredService<IKesimListesiPaketService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();

        public string SelectedKesimIds { get; private set; }

        public frmKesimPlaniSec(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitializeComponent();

            this.Icon = Properties.Resources.cekalogokirmizi;

            LoadKesimListesiToDataGridView();

            DataGridViewHelper.StilUygulaUrunGrubuSecim(dataGridKesimPaket);
        }

        public void LoadKesimListesiToDataGridView()
        {
            try
            {
                DataTable odemeSekilleri = _kesimListesiPaketService.GetKesimListesiPaketSure();

                if (odemeSekilleri == null || odemeSekilleri.Rows.Count == 0)
                {
                    MessageBox.Show("Veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dataGridKesimPaket.DataSource = null;
                dataGridKesimPaket.DataSource = odemeSekilleri;

                dataGridKesimPaket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridKesimPaket.ScrollBars = ScrollBars.Both;

                ConfigureDataGridViewColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kesim listesi paket yüklenirken hata oluştu: {ex.Message}\nDetay: {ex.InnerException?.Message}",
                    "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            if (!dataGridKesimPaket.Columns.Contains("Secim"))
            {
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "Secim",
                    HeaderText = "",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    DisplayIndex = 0
                };
                dataGridKesimPaket.Columns.Add(checkBoxColumn);
            }

            foreach (DataGridViewColumn column in dataGridKesimPaket.Columns)
            {
                if (column.Name == "Secim")
                    continue;

                switch (column.Name)
                {
                    case "kesimId":
                        column.HeaderText = "Kesim Planı Numarası";
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        column.DisplayIndex = 1;
                        break;
                    case "eklemeTarihi":
                        column.HeaderText = "Ekleme Tarihi";
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        column.DisplayIndex = 2;
                        break;
                    default:
                        column.Visible = false;
                        break;
                }
            }
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            var selectedIds = dataGridKesimPaket.Rows.Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells["Secim"].Value))
                .Select(row => row.Cells["kesimId"].Value?.ToString())
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            if (selectedIds.Count == 0)
            {
                MessageBox.Show("Lütfen en az bir kesim planı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedKesimIds = string.Join("; ", selectedIds);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string baseSql = _kesimListesiPaketService.GetKesimListesiPaketSureQuery();

            var frm = new frmAra(
                dataGridKesimPaket,
                _tabloFiltreleService,
                (dt) => { dataGridKesimPaket.DataSource = dt; },
                baseSql,
                _serviceProvider
            );

            frm.ShowDialog();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}