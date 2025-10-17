using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic; 
namespace CEKA_APP.Forms.KesimTakip
{
    public partial class frmKesimPlaniSec : Form
    {
        IServiceProvider _serviceProvider;
        private IKesimListesiPaketService _kesimListesiPaketService => _serviceProvider.GetRequiredService<IKesimListesiPaketService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();

        public string SelectedKesimIds { get; private set; }
        public string SelectedLotNo { get; private set; }
        public string SelectedEn { get; private set; }
        public string SelectedBoy { get; private set; }
        public List<string> ExcludedKesimIds { get; private set; }
        public frmKesimPlaniSec(IServiceProvider serviceProvider, List<string> excludedKesimIds)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            ExcludedKesimIds = excludedKesimIds ?? new List<string>();

            InitializeComponent();

            this.Icon = Properties.Resources.cekalogokirmizi;

            LoadKesimListesiToDataGridView();

            DataGridViewHelper.StilUygulaUrunGrubuSecim(dataGridKesimPaket);

        }

        private void dataGridKesimPaket_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridKesimPaket.Columns[e.ColumnIndex].Name == "LotNo")
            {
                dataGridKesimPaket.BeginEdit(true);
            }
        }

        public void LoadKesimListesiToDataGridView()
        {
            try
            {
                DataTable kesimListesi = _kesimListesiPaketService.GetKesimListesiPaketSure();

                if (kesimListesi == null || kesimListesi.Rows.Count == 0)
                {
                    MessageBox.Show("Veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dataGridKesimPaket.DataSource = null;

                string kesimIdColumnName = "KesimId"; 

                DataTable dataSource = kesimListesi;

                if (kesimListesi.Columns.Contains(kesimIdColumnName) && ExcludedKesimIds.Any()) 
                {
                    try
                    {
                        var filteredRows = kesimListesi.AsEnumerable()
                                                       .Where(row => !ExcludedKesimIds.Contains(row.Field<string>(kesimIdColumnName)))
                                                       .ToList();

                        if (filteredRows.Any())
                        {
                            dataSource = filteredRows.CopyToDataTable();
                        }
                        else
                        {
                            MessageBox.Show("Seçilebilecek başka kesim planı bulunmamaktadır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Filtreleme sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataSource = kesimListesi;
                    }
                }

                dataGridKesimPaket.DataSource = dataSource;

                if (!((DataTable)dataGridKesimPaket.DataSource).Columns.Contains("LotNo"))
                {
                    ((DataTable)dataGridKesimPaket.DataSource).Columns.Add("LotNo", typeof(string));
                    foreach (DataRow row in ((DataTable)dataGridKesimPaket.DataSource).Rows)
                    {
                        row["LotNo"] = string.Empty;
                    }
                }

                dataGridKesimPaket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridKesimPaket.ScrollBars = ScrollBars.Both;

                ConfigureDataGridViewColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kesim listesi paket yüklenirken hata oluştu: {ex.Message}\nDetay: {ex.InnerException?.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (!dataGridKesimPaket.Columns.Contains("LotNo"))
            {
                DataGridViewTextBoxColumn lotNoColumn = new DataGridViewTextBoxColumn
                {
                    Name = "LotNo",
                    HeaderText = "Lot Numarası",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    DisplayIndex = 1, 
                    ReadOnly = false 
                };
                dataGridKesimPaket.Columns.Add(lotNoColumn);
            }

            foreach (DataGridViewColumn column in dataGridKesimPaket.Columns)
            {
                if (column.Name == "Secim")
                    continue;

                if (column.Name == "LotNo")
                {
                    column.HeaderText = "Lot Numarası";
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    column.DisplayIndex = 1;
                    continue; 
                }

                switch (column.Name)
                {
                    case "kesimId":
                        column.HeaderText = "Kesim Planı Numarası";
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        column.DisplayIndex = 2;
                        break;
                    case "en":
                        column.HeaderText = "Malzeme En";
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        column.DisplayIndex = 3;
                        break;
                    case "boy":
                        column.HeaderText = "Malzeme Boy";
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        column.DisplayIndex = 4;
                        break;
                    case "eklemeTarihi":
                        column.HeaderText = "Ekleme Tarihi";
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        column.DisplayIndex = 5;
                        break;
                    default:
                        column.Visible = false;
                        break;
                }
            }
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            dataGridKesimPaket.EndEdit();

            var selectedRows = dataGridKesimPaket.Rows.Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells["Secim"].Value))
                .ToList();

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen en az bir kesim planı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> kesimIds = new List<string>();
            List<string> lotNos = new List<string>();
            List<string> ens = new List<string>();
            List<string> boys = new List<string>();

            foreach (var row in selectedRows)
            {
                string lotNo = row.Cells["LotNo"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(lotNo))
                {
                    MessageBox.Show($"Kesim Planı No: {row.Cells["kesimId"].Value} için Lot Numarası alanı boş bırakılamaz.", "Lot No Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string kesimId = row.Cells["kesimId"].Value?.ToString();
                string en = row.Cells["en"].Value?.ToString() ?? "0";
                string boy = row.Cells["boy"].Value?.ToString() ?? "0";

                if (!string.IsNullOrEmpty(kesimId))
                {
                    kesimIds.Add(kesimId);
                    lotNos.Add(lotNo.Trim());
                    ens.Add(en);
                    boys.Add(boy);
                }
            }

            SelectedKesimIds = string.Join("; ", kesimIds);
            SelectedLotNo = string.Join("; ", lotNos);
            SelectedEn = string.Join("; ", ens);
            SelectedBoy = string.Join("; ", boys);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string baseSql = _kesimListesiPaketService.GetKesimListesiPaketSureQuery();

            var frm = new frmAra(
                dataGridKesimPaket,
                _tabloFiltreleService,
                (dt) => {
                    dataGridKesimPaket.DataSource = dt;
                    if (!((DataTable)dataGridKesimPaket.DataSource).Columns.Contains("LotNo"))
                    {
                        ((DataTable)dataGridKesimPaket.DataSource).Columns.Add("LotNo", typeof(string));
                        foreach (DataRow row in ((DataTable)dataGridKesimPaket.DataSource).Rows)
                        {
                            row["LotNo"] = string.Empty;
                        }
                    }
                    ConfigureDataGridViewColumns();
                },
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