using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Helper;
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
    public partial class ctlTeminatMektuplari : UserControl
    {
        private Dictionary<string, string> sonFiltreKriterleri = new Dictionary<string, string>();
        public ctlTeminatMektuplari()
        {
            InitializeComponent();

            DataGridViewHelper.StilUygulaProjeFinans(dataGridTeminatMektuplari);
            dataGridTeminatMektuplari.AutoGenerateColumns = false;

            if (this.dataGridTeminatMektuplari.ContextMenuStrip != null)
            {
                this.cmsTeminatMektubuIslemleri = dataGridTeminatMektuplari.ContextMenuStrip;

                foreach (ToolStripItem item in cmsTeminatMektubuIslemleri.Items)
                {
                    if (item.Name == "tsmiMektupEkle")
                        tsmiMektupEkle = (ToolStripMenuItem)item;
                    else if (item.Name == "tsmiMektupGuncelle")
                        tsmiMektupGuncelle = (ToolStripMenuItem)item;
                    else if (item.Name == "tsmiMektupSil")
                        tsmiMektupSil = (ToolStripMenuItem)item;
                    else if (item.Name == "tsmiAra")
                        tsmiAra = (ToolStripMenuItem)item;
                }
            }

            ConfigureDataGridViewColumns();
        }

        private void ctlTeminatMektuplari_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Teminat Mektupları";
            LoadMektuplarToDataGridView();
        }

        private void LoadMektuplarToDataGridView()
        {
            ProjeFinans_TeminatMektuplariData mektupData = new ProjeFinans_TeminatMektuplariData();
            try
            {
                dataGridTeminatMektuplari.DataSource = null;
                var mektuplar = mektupData.GetTeminatMektuplari();
                dataGridTeminatMektuplari.DataSource = mektuplar;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mektuplar yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            dataGridTeminatMektuplari.Columns.Clear();

            dataGridTeminatMektuplari.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "mektupNo",
                Name = "mektupNo",
                HeaderText = "Mektup No",
                DisplayIndex = 0,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "musteriNo",
                Name = "musteriNo",
                HeaderText = "Müşteri No",
                DisplayIndex = 1,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "musteriAdi",
                Name = "musteriAdi",
                HeaderText = "Müşteri Adı",
                DisplayIndex = 2,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "kilometreTasiAdi",
                Name = "kilometreTasiAdi",
                HeaderText = "Kilometre Taşı Adı",
                DisplayIndex = 3,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "paraBirimi",
                Name = "paraBirimi",
                HeaderText = "Para Birimi",
                DisplayIndex = 4,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "banka",
                Name = "banka",
                HeaderText = "Banka",
                DisplayIndex = 5,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "mektupTuru",
                Name = "mektupTuru",
                HeaderText = "Mektup Türü",
                DisplayIndex = 6,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "tutar",
                Name = "tutar",
                HeaderText = "Tutar",
                DisplayIndex = 7,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "vadeTarihi",
                Name = "vadeTarihi",
                HeaderText = "Vade Tarihi",
                DisplayIndex = 8,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy HH:mm" }
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "iadeTarihi",
                Name = "iadeTarihi",
                HeaderText = "İade Tarihi",
                DisplayIndex = 9,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy HH:mm" }
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "komisyonTutari",
                Name = "komisyonTutari",
                HeaderText = "Komisyon Tutarı",
                DisplayIndex = 10,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "komisyonOrani",
                Name = "komisyonOrani",
                HeaderText = "Komisyon Oranı",
                DisplayIndex = 11,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "komisyonVadesi",
                Name = "komisyonVadesi",
                HeaderText = "Komisyon Vadesi",
                DisplayIndex = 12,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "projeNo",
                Name = "projeNo",
                HeaderText = "Proje No",
                DisplayIndex = 13,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridTeminatMektuplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "kilometreTasiId",
                Name = "kilometreTasiId",
                HeaderText = "Kilometre Taşı ID",
                DisplayIndex = 14,
                Visible = false
            });
        }

        private void dataGridTeminatMektuplari_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridTeminatMektuplari.ClearSelection();
                dataGridTeminatMektuplari.Rows[e.RowIndex].Selected = true;
                dataGridTeminatMektuplari.CurrentCell = dataGridTeminatMektuplari.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void cmsTeminatMektubuIslemleri_Opening(object sender, CancelEventArgs e)
        {
            if (tsmiMektupGuncelle != null) tsmiMektupGuncelle.Enabled = false;
            if (tsmiMektupSil != null) tsmiMektupSil.Enabled = false;
            if (tsmiAra != null) tsmiAra.Enabled = true;

            if (dataGridTeminatMektuplari.CurrentRow != null && dataGridTeminatMektuplari.CurrentRow.Index >= 0)
            {
                if (tsmiMektupGuncelle != null) tsmiMektupGuncelle.Enabled = true;
                if (tsmiMektupSil != null) tsmiMektupSil.Enabled = true;
            }
            if (tsmiMektupEkle != null) tsmiMektupEkle.Enabled = true;
        }

        private void tsmiMektupEkle_Click(object sender, EventArgs e)
        {
            using (frmTeminatMektubuEkle mektupForm = new frmTeminatMektubuEkle(null))
            {
                if (mektupForm.ShowDialog() == DialogResult.OK)
                {
                    LoadMektuplarToDataGridView();
                }
            }
        }

        private void tsmiMektupGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridTeminatMektuplari.CurrentRow != null)
            {
                TeminatMektuplari selectedMektup = dataGridTeminatMektuplari.CurrentRow.DataBoundItem as TeminatMektuplari;

                if (selectedMektup != null)
                {
                    using (frmTeminatMektubuEkle musteriForm = new frmTeminatMektubuEkle(selectedMektup))
                    {
                        if (musteriForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadMektuplarToDataGridView();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek için bir mektup seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsmiMektupSil_Click(object sender, EventArgs e)
        {
            if (dataGridTeminatMektuplari.CurrentRow != null)
            {
                TeminatMektuplari selectedMektup = dataGridTeminatMektuplari.CurrentRow.DataBoundItem as TeminatMektuplari;

                if (selectedMektup != null)
                {
                    DialogResult result = MessageBox.Show(
                        $"'{selectedMektup.mektupNo}' numaralı mektubu silmek istediğinizden emin misiniz?",
                        "Mektup Silme Onayı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        ProjeFinans_TeminatMektuplariData mektupData = new ProjeFinans_TeminatMektuplariData();
                        try
                        {
                            mektupData.MektupSil(selectedMektup.mektupNo);
                            MessageBox.Show("Mektup başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadMektuplarToDataGridView();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Mektup silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir mektup seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsmiAra_Click(object sender, EventArgs e)
        {
            try
            {
                frmAra araForm = new frmAra(
                    dataGridTeminatMektuplari.Columns,
                    TeminatMektuplariFiltrele,
                    GuncelleDataGrid,
                    false,
                    sonFiltreKriterleri
                );
                araForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama işlemi başlatılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable TeminatMektuplariFiltrele(Dictionary<string, TextBox> filtreKutulari)
        {
            try
            {
                ProjeFinans_TeminatMektuplariData mektupData = new ProjeFinans_TeminatMektuplariData();
                sonFiltreKriterleri.Clear();
                foreach (var kutu in filtreKutulari)
                {
                    if (!string.IsNullOrEmpty(kutu.Value.Text.Trim()))
                    {
                        sonFiltreKriterleri[kutu.Key] = kutu.Value.Text.Trim();
                    }
                }
                return mektupData.FiltreleTeminatMektuplari(filtreKutulari, dataGridTeminatMektuplari);
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
            try
            {
                dataGridTeminatMektuplari.DataSource = null;
                dataGridTeminatMektuplari.DataSource = sonucTablo;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama sonuçları yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}