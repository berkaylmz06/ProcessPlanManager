using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.ProjeFinans;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CEKA_APP.Forms
{
    public partial class frmAraBilgi : Form
    {
        public string SecilenMusteriNo { get; private set; }

        private readonly IServiceProvider _serviceProvider;
        private IMusterilerService _musterilerService => _serviceProvider.GetRequiredService<IMusterilerService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();
        public frmAraBilgi(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));


            Helper.DataGridViewHelper.StilUygulaProjeFinans(dataGridBilgi);
            this.Icon = Properties.Resources.cekalogokirmizi;
        }
        private void frmAraBilgi_Load(object sender, EventArgs e)
        {
            LoadMusterilerToDataGridView();
        }
        private void LoadMusterilerToDataGridView()
        {
            try
            {
                List<Musteriler> musteriler = _musterilerService.GetMusterilerAraFormu();
                dataGridBilgi.DataSource = null;
                dataGridBilgi.DataSource = musteriler;

                ConfigureDataGridViewColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müşteriler yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ConfigureDataGridViewColumns()
        {
            foreach (DataGridViewColumn column in dataGridBilgi.Columns)
            {
                if (column.Name != "musteriNo" && column.Name != "musteriAdi")
                {
                    column.Visible = false; 
                }
            }

            dataGridBilgi.Columns["musteriNo"].HeaderText = "Müşteri No";
            dataGridBilgi.Columns["musteriAdi"].HeaderText = "Müşteri Adı";
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string baseSql = _musterilerService.GetMusterilerAraFormuQuery();

            var frm = new frmAra(
                dataGridBilgi,
                _tabloFiltreleService,
                (dt) => { dataGridBilgi.DataSource = dt; },
                baseSql,
                _serviceProvider,
                detayEkle: true
            );

            frm.ShowDialog();
        }

        private void dataGridBilgi_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridBilgi.CurrentRow != null && dataGridBilgi.CurrentRow.Index >= 0)
            {
                SecilenMusteriNo = dataGridBilgi.CurrentRow.Cells["musteriNo"].Value?.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            if (dataGridBilgi.CurrentRow != null && dataGridBilgi.CurrentRow.Index >= 0)
            {
                SecilenMusteriNo = dataGridBilgi.CurrentRow.Cells["musteriNo"].Value?.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
