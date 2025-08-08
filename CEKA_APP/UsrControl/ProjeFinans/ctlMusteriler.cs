using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Helper;
using CEKA_APP.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ExcelDataReader;
using System.IO;
using System.Linq;

namespace CEKA_APP.UsrControl
{
    public partial class ctlMusteriler : UserControl
    {
        private ToolStripMenuItem tsmiExcelYukle;
        private ToolStripMenuItem tsmiAra;
        private Dictionary<string, string> sonFiltreKriterleri = new Dictionary<string, string>();

        public ctlMusteriler()
        {
            InitializeComponent();

            DataGridViewHelper.StilUygulaProjeFinans(dataGridMusteriler);

            this.Load += new EventHandler(ctlMusteriler_Load);

            this.cmsMusteriIslemleri = new ContextMenuStrip();
            tsmiExcelYukle = new ToolStripMenuItem
            {
                Name = "tsmiExcelYukle",
                Text = "Excel'den Müşteri Yükle"
            };
            tsmiExcelYukle.Click += new EventHandler(tsmiExcelYukle_Click);

            tsmiAra = new ToolStripMenuItem
            {
                Name = "tsmiAra",
                Text = "Ara"
            };
            tsmiAra.Click += new EventHandler(tsmiAra_Click);

            this.cmsMusteriIslemleri.Items.Add(tsmiExcelYukle);
            this.cmsMusteriIslemleri.Items.Add(tsmiAra);
            this.dataGridMusteriler.ContextMenuStrip = this.cmsMusteriIslemleri;

            dataGridMusteriler.AutoGenerateColumns = true;
        }

        private void ctlMusteriler_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Müşteriler";
            LoadMusterilerToDataGridView();
        }

        private void LoadMusterilerToDataGridView()
        {
            ProjeFinans_MusterilerData musteriData = new ProjeFinans_MusterilerData();
            try
            {
                List<Musteriler> musteriler = musteriData.GetMusteriler();
                dataGridMusteriler.DataSource = null;
                dataGridMusteriler.DataSource = musteriler;

                ConfigureDataGridViewColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müşteriler yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            if (dataGridMusteriler.Columns.Contains("musteriNo"))
            {
                dataGridMusteriler.Columns["musteriNo"].HeaderText = "Müşteri Numarası";
                dataGridMusteriler.Columns["musteriNo"].DisplayIndex = 0;
                dataGridMusteriler.Columns["musteriNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            if (dataGridMusteriler.Columns.Contains("musteriAdi"))
            {
                dataGridMusteriler.Columns["musteriAdi"].HeaderText = "Müşteri Adı";
                dataGridMusteriler.Columns["musteriAdi"].DisplayIndex = 1;
                dataGridMusteriler.Columns["musteriAdi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dataGridMusteriler.Columns.Contains("vergiDairesi"))
            {
                dataGridMusteriler.Columns["vergiDairesi"].HeaderText = "Vergi Dairesi";
                dataGridMusteriler.Columns["vergiDairesi"].DisplayIndex = 2;
                dataGridMusteriler.Columns["vergiDairesi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            if (dataGridMusteriler.Columns.Contains("vergiNo"))
            {
                dataGridMusteriler.Columns["vergiNo"].HeaderText = "Vergi No";
                dataGridMusteriler.Columns["vergiNo"].DisplayIndex = 3;
                dataGridMusteriler.Columns["vergiNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            if (dataGridMusteriler.Columns.Contains("adres"))
            {
                dataGridMusteriler.Columns["adres"].HeaderText = "Adres";
                dataGridMusteriler.Columns["adres"].DisplayIndex = 4;
                dataGridMusteriler.Columns["adres"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dataGridMusteriler.Columns.Contains("musteriMensei"))
            {
                dataGridMusteriler.Columns["musteriMensei"].HeaderText = "Müşteri Menşei";
                dataGridMusteriler.Columns["musteriMensei"].DisplayIndex = 5;
                dataGridMusteriler.Columns["musteriMensei"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            if (dataGridMusteriler.Columns.Contains("doviz"))
            {
                dataGridMusteriler.Columns["doviz"].HeaderText = "Döviz";
                dataGridMusteriler.Columns["doviz"].DisplayIndex = 6;
                dataGridMusteriler.Columns["doviz"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
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
                                ProjeFinans_MusterilerData musteriData = new ProjeFinans_MusterilerData();

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

                                musteriData.TumMusterileriSil();
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

                                    musteriData.MusteriKaydet(musteri);
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
            frmAra araForm = new frmAra(
                dataGridMusteriler.Columns,
                FiltreleMusteriler,
                GuncelleDataGrid,
                false,
                sonFiltreKriterleri
            );
            araForm.ShowDialog();
        }

        private DataTable FiltreleMusteriler(Dictionary<string, TextBox> filtreKutulari)
        {
            try
            {
                ProjeFinans_MusterilerData musteriData = new ProjeFinans_MusterilerData();
                sonFiltreKriterleri.Clear();
                foreach (var kutu in filtreKutulari)
                {
                    if (!string.IsNullOrEmpty(kutu.Value.Text.Trim()))
                    {
                        sonFiltreKriterleri[kutu.Key] = kutu.Value.Text.Trim();
                    }
                }
                return musteriData.FiltreleMusteriBilgileri(filtreKutulari, dataGridMusteriler);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Hata detayı: {ex.ToString()}");
                return null;
            }
        }

        private void GuncelleDataGrid(DataTable sonucTablo)
        {
            dataGridMusteriler.DataSource = null;
            dataGridMusteriler.DataSource = sonucTablo;
            ConfigureDataGridViewColumns();
        }
    }
}