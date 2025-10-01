using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Properties;
using ExcelDataReader;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlMusteriler : UserControl
    {
        private readonly IServiceProvider _serviceProvider;

        private IMusterilerService _musterilerService => _serviceProvider.GetRequiredService<IMusterilerService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();

        public ctlMusteriler(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DataGridViewHelper.StilUygulaProjeFinans(dataGridMusteriler);

            this.Load += new EventHandler(ctlMusteriler_Load);

            dataGridMusteriler.AutoGenerateColumns = true;
        }

        private void ctlMusteriler_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Müşteriler";
            LoadMusterilerToDataGridView();
        }

        private void LoadMusterilerToDataGridView()
        {
            try
            {
                List<Musteriler> musteriler = _musterilerService.GetMusteriler();
                dataGridMusteriler.DataSource = null;
                dataGridMusteriler.DataSource = new SiralabilirBindingList<Musteriler>(musteriler);


                ConfigureDataGridViewColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müşteriler yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            var mevcutSira = Settings.Default.SutunSirasiMusteriler != null
                ? StringCollectionToList(Settings.Default.SutunSirasiMusteriler)
                : VarsayilanSutunSirasiniAl();

            foreach (var sutunAdi in mevcutSira)
            {
                if (dataGridMusteriler.Columns.Contains(sutunAdi))
                {
                    dataGridMusteriler.Columns[sutunAdi].Visible = true;
                    dataGridMusteriler.Columns[sutunAdi].DisplayIndex = mevcutSira.IndexOf(sutunAdi);

                    switch (sutunAdi)
                    {
                        case "musteriNo":
                            dataGridMusteriler.Columns[sutunAdi].HeaderText = "Müşteri No";
                            dataGridMusteriler.Columns[sutunAdi].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            break;
                        case "musteriAdi":
                            dataGridMusteriler.Columns[sutunAdi].HeaderText = "Müşteri Adı";
                            dataGridMusteriler.Columns[sutunAdi].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            break;
                        case "vergiDairesi":
                            dataGridMusteriler.Columns[sutunAdi].HeaderText = "Vergi Dairesi";
                            dataGridMusteriler.Columns[sutunAdi].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            break;
                        case "vergiNo":
                            dataGridMusteriler.Columns[sutunAdi].HeaderText = "Vergi No";
                            dataGridMusteriler.Columns[sutunAdi].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            break;
                        case "adres":
                            dataGridMusteriler.Columns[sutunAdi].HeaderText = "Adres";
                            dataGridMusteriler.Columns[sutunAdi].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            break;
                        case "musteriMensei":
                            dataGridMusteriler.Columns[sutunAdi].HeaderText = "Müşteri Menşei";
                            dataGridMusteriler.Columns[sutunAdi].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            break;
                        case "doviz":
                            dataGridMusteriler.Columns[sutunAdi].HeaderText = "Döviz";
                            dataGridMusteriler.Columns[sutunAdi].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            break;
                    }
                }
            }
        }
        private void dataGridMusteriler_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridMusteriler.ClearSelection();
                dataGridMusteriler.Rows[e.RowIndex].Selected = true;
                dataGridMusteriler.CurrentCell = dataGridMusteriler.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void tsmiExcelYukle_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                openFileDialog.Title = "Excel Dosyası Seçin";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                    {
                                        UseHeaderRow = true
                                    }
                                });

                                DataTable dt = result.Tables[0];

                                var columnMap = new Dictionary<string, string>
                                {
                                    { "musteriNo", "MUSTERINO" },
                                    { "musteriAdi", "MUSTERIADI" },
                                    { "vergiDairesi", "VERGIDAIRESI" },
                                    { "vergiNo", "VERGINO" },
                                    { "adres", "ADRES" },
                                    { "musteriMensei", "ULKE" },
                                    { "doviz", "DOVIZ" }
                                };

                                foreach (var column in columnMap.Values)
                                {
                                    if (!dt.Columns.Cast<DataColumn>().Any(c => NormalizeColumnName(c.ColumnName) == NormalizeColumnName(column)))
                                    {
                                        MessageBox.Show($"Lütfen excel formatını kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }

                                _musterilerService.TumMusterileriSil();
                                int newCount = 0;

                                foreach (DataRow row in dt.Rows)
                                {
                                    Musteriler musteri = new Musteriler
                                    {
                                        musteriNo = GetExcelColumnValue(row, columnMap["musteriNo"]),
                                        musteriAdi = GetExcelColumnValue(row, columnMap["musteriAdi"]),
                                        vergiDairesi = GetExcelColumnValue(row, columnMap["vergiDairesi"]),
                                        vergiNo = GetExcelColumnValue(row, columnMap["vergiNo"]),
                                        adres = GetExcelColumnValue(row, columnMap["adres"]),
                                        musteriMensei = GetExcelColumnValue(row, columnMap["musteriMensei"]),
                                        doviz = GetExcelColumnValue(row, columnMap["doviz"]) ?? "Belirtilmemiş"
                                    };

                                    if (string.IsNullOrEmpty(musteri.musteriNo))
                                        continue;

                                    _musterilerService.MusteriKaydet(musteri);
                                    newCount++;

                                    Console.WriteLine($"MusteriNo: {musteri.musteriNo}, Doviz: {musteri.doviz}, İşlem: Yeni Eklendi");
                                }

                                MessageBox.Show($"Müşteri bilgileri yüklendi. Toplam eklenen: {newCount}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadMusterilerToDataGridView();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Excel dosyası okunurken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string GetExcelColumnValue(DataRow row, string columnName)
        {
            var column = row.Table.Columns.Cast<DataColumn>()
                .FirstOrDefault(c => NormalizeColumnName(c.ColumnName) == NormalizeColumnName(columnName));
            return column != null ? row[column]?.ToString() : null;
        }

        private string NormalizeColumnName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return columnName;
            return columnName.Replace("ı", "i").Replace("İ", "I").Replace("ş", "s").Replace("Ş", "S")
                            .Replace("ğ", "g").Replace("Ğ", "G").Replace("ü", "u").Replace("Ü", "U")
                            .Replace("ç", "c").Replace("Ç", "C").ToLower();
        }

        private void dataGridMusteriler_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridMusteriler.Rows.Count > 0)
            {
                dataGridMusteriler.ClearSelection();
                dataGridMusteriler.CurrentCell = null;
            }
        }

        private void tsmiAra_Click(object sender, EventArgs e)
        {
            string baseSql = _musterilerService.GetMusterilerQuery();
            var frm = new frmAra(
                dataGridMusteriler,
                _tabloFiltreleService,
                (dt) => 
                { 
                    dataGridMusteriler.DataSource = dt; 
                    ConfigureDataGridViewColumns(); 
                },
                baseSql,
                _serviceProvider,
                detayEkle: false
            );

            frm.ShowDialog();
        }

        private void tsmiSutunSiralama_Click(object sender, EventArgs e)
        {
            var mevcutSira = Settings.Default.SutunSirasiMusteriler != null ? StringCollectionToList(Settings.Default.SutunSirasiMusteriler) : VarsayilanSutunSirasiniAl();
            var gorunurSutunlar = mevcutSira.Where(sutun =>
                dataGridMusteriler.Columns.Contains(sutun) &&
                dataGridMusteriler.Columns[sutun].Visible).ToList();

            frmSutunSiralama sutunSiralamaFormu = new frmSutunSiralama(gorunurSutunlar);
            if (sutunSiralamaFormu.ShowDialog() == DialogResult.OK)
            {
                var yeniSira = sutunSiralamaFormu.SutunSirasiniAl();
                var tamSira = VarsayilanSutunSirasiniAl();
                var gorunmezSutunlar = tamSira.Except(yeniSira).ToList();
                yeniSira.AddRange(gorunmezSutunlar);

                Settings.Default.SutunSirasiMusteriler = new System.Collections.Specialized.StringCollection();
                Settings.Default.SutunSirasiMusteriler.AddRange(yeniSira.ToArray());
                Settings.Default.Save();
                LoadMusterilerToDataGridView();
            }
        }
        private List<string> StringCollectionToList(System.Collections.Specialized.StringCollection collection)
        {
            var list = new List<string>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    list.Add(item);
                }
            }
            return list;
        }
        private List<string> VarsayilanSutunSirasiniAl()
        {
            return new List<string>
            {
                "musteriNo", "musteriAdi", "vergiDairesi", "vergiNo", "adres", "musteriMensei",
                "doviz"
            };
        }
    }
}