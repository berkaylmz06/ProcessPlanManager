using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CEKA_APP.DataBase;
using CEKA_APP.Helper;

namespace CEKA_APP.UsrControl
{
    public partial class ctlKarsilastirmaTablosu : UserControl
    {
        private DataTable kaliteTablo;
        private DataTable malzemeTablo;
        private DataTable kesimTablo;
        private BindingSource kaliteBindingSource;
        private BindingSource malzemeBindingSource;
        private BindingSource kesimBindingSource;

        public ctlKarsilastirmaTablosu()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            kaliteTablo = new DataTable();
            malzemeTablo = new DataTable();
            kesimTablo = new DataTable();
            kaliteBindingSource = new BindingSource();
            malzemeBindingSource = new BindingSource();
            kesimBindingSource = new BindingSource();

            DataGridViewHelper.StilUygula(dataGridKalite);
            DataGridViewHelper.StilUygula(dataGridMalzeme);
            DataGridViewHelper.StilUygula(dataGridKesim);

            ClearSelections();
            TextBoxlariTemizle();
            btnEkle.Enabled = false;
            btnSil.Enabled = false;
        }

        private void ClearSelections()
        {
            dataGridKalite.ClearSelection();
            dataGridKalite.CurrentCell = null;
            dataGridMalzeme.ClearSelection();
            dataGridMalzeme.CurrentCell = null;
            dataGridKesim.ClearSelection();
            dataGridKesim.CurrentCell = null;
        }

        private void TextBoxlariTemizle()
        {
            txtCode.Text = "";
            txtIfsCode.Text = "";
            txtAciklama.Text = "";
        }

        private void LoadKaliteData()
        {
            kaliteTablo.Clear();
            kaliteTablo = KarsilastirmaTablosuData.GetAllKaliteKarsilastirmalari();
            kaliteBindingSource.DataSource = kaliteTablo;
            dataGridKalite.DataSource = kaliteBindingSource;
            ConfigureDataGrid(dataGridKalite, new[] { "CekaCode", "IfsCode", "Aciklama" },
                new[] { "Çeka Kodu", "IFS Kodu", "Açıklama" });
        }

        private void LoadMalzemeData()
        {
            malzemeTablo.Clear();
            malzemeTablo = KarsilastirmaTablosuData.GetAllMalzemeKarsilastirmalari();
            malzemeBindingSource.DataSource = malzemeTablo;
            dataGridMalzeme.DataSource = malzemeBindingSource;
            ConfigureDataGrid(dataGridMalzeme, new[] { "AutoCadCode", "IfsCode", "Aciklama" },
                new[] { "AutoCAD Kodu", "IFS Kodu", "Açıklama" });
        }

        private void LoadKesimData()
        {
            kesimTablo.Clear();
            kesimTablo = KarsilastirmaTablosuData.GetAllKesimKarsilastirmalari();
            kesimBindingSource.DataSource = kesimTablo;
            dataGridKesim.DataSource = kesimBindingSource;
            ConfigureDataGrid(dataGridKesim, new[] { "KesimCode", "IfsCode", "Aciklama" },
                new[] { "Kesim Kodu", "IFS Kodu", "Açıklama" });
        }

        private void ConfigureDataGrid(DataGridView grid, string[] columnNames, string[] headerTexts)
        {
            if (grid.Columns["id"] != null)
            {
                grid.Columns["id"].Visible = false;
            }
            for (int i = 0; i < columnNames.Length; i++)
            {
                if (grid.Columns[columnNames[i]] != null)
                {
                    grid.Columns[columnNames[i]].HeaderText = headerTexts[i];
                    grid.Columns[columnNames[i]].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    grid.Columns[columnNames[i]].DisplayIndex = i;
                }
            }
        }

        private void ctlKarsilastirmaTablosu_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Karşılaştırma Tablosu";
            tabControl1.SelectedIndex = -1; // Tüm sekmeler kapalı
            labelCode.Text = "Kodu:"; // Varsayılan label metni
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxlariTemizle();
            btnEkle.Enabled = tabControl1.SelectedIndex >= 0;
            btnSil.Enabled = false;

            if (tabControl1.SelectedIndex == 0)
            {
                labelCode.Text = "Çeka Kodu:";
                LoadKaliteData();
                dataGridMalzeme.DataSource = null;
                dataGridKesim.DataSource = null;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                labelCode.Text = "AutoCAD Kodu:";
                LoadMalzemeData();
                dataGridKalite.DataSource = null;
                dataGridKesim.DataSource = null;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                labelCode.Text = "Kesim Kodu:";
                LoadKesimData();
                dataGridKalite.DataSource = null;
                dataGridMalzeme.DataSource = null;
            }
            else
            {
                labelCode.Text = "Kodu:";
                dataGridKalite.DataSource = null;
                dataGridMalzeme.DataSource = null;
                dataGridKesim.DataSource = null;
            }

            ClearSelections();
        }

        private void dataGridKalite_SelectionChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0 && dataGridKalite.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridKalite.SelectedRows[0];
                txtCode.Text = row.Cells["CekaCode"].Value?.ToString() ?? "";
                txtIfsCode.Text = row.Cells["IfsCode"].Value?.ToString() ?? "";
                txtAciklama.Text = row.Cells["Aciklama"].Value?.ToString() ?? "";
                btnSil.Enabled = true;
            }
            else
            {
                TextBoxlariTemizle();
                btnSil.Enabled = false;
            }
        }

        private void dataGridMalzeme_SelectionChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1 && dataGridMalzeme.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridMalzeme.SelectedRows[0];
                txtCode.Text = row.Cells["AutoCadCode"].Value?.ToString() ?? "";
                txtIfsCode.Text = row.Cells["IfsCode"].Value?.ToString() ?? "";
                txtAciklama.Text = row.Cells["Aciklama"].Value?.ToString() ?? "";
                btnSil.Enabled = true;
            }
            else
            {
                TextBoxlariTemizle();
                btnSil.Enabled = false;
            }
        }

        private void dataGridKesim_SelectionChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2 && dataGridKesim.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridKesim.SelectedRows[0];
                txtCode.Text = row.Cells["KesimCode"].Value?.ToString() ?? "";
                txtIfsCode.Text = row.Cells["IfsCode"].Value?.ToString() ?? "";
                txtAciklama.Text = row.Cells["Aciklama"].Value?.ToString() ?? "";
                btnSil.Enabled = true;
            }
            else
            {
                TextBoxlariTemizle();
                btnSil.Enabled = false;
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                string code = txtCode.Text.Trim();
                string ifsCode = txtIfsCode.Text.Trim();
                string aciklama = txtAciklama.Text.Trim();

                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(ifsCode))
                {
                    MessageBox.Show("Gerekli bilgileri doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string message;
                if (tabControl1.SelectedIndex == 0)
                {
                    message = "Seçili kalite kaydını eklemek istediğinizden emin misiniz?";
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    message = "Seçili malzeme kaydını eklemek istediğinizden emin misiniz?";
                }
                else if (tabControl1.SelectedIndex == 2)
                {
                    message = "Seçili kesim kaydını eklemek istediğinizden emin misiniz?";
                }
                else
                {
                    MessageBox.Show("Geçerli bir sekme seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show(message, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                if (tabControl1.SelectedIndex == 0)
                {
                    KarsilastirmaTablosuData.SaveKarsilastirmaKalite(code, ifsCode, aciklama);
                    LoadKaliteData();
                    dataGridKalite.ClearSelection();
                    dataGridKalite.CurrentCell = null;
                    MessageBox.Show("Kalite verisi başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    KarsilastirmaTablosuData.SaveKarsilastirmaMalzeme(code, ifsCode, aciklama);
                    LoadMalzemeData();
                    dataGridMalzeme.ClearSelection();
                    dataGridMalzeme.CurrentCell = null;
                    MessageBox.Show("Malzeme verisi başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (tabControl1.SelectedIndex == 2)
                {
                    KarsilastirmaTablosuData.SaveKarsilastirmaKesim(code, ifsCode, aciklama);
                    LoadKesimData();
                    dataGridKesim.ClearSelection();
                    dataGridKesim.CurrentCell = null;
                    MessageBox.Show("Kesim verisi başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                TextBoxlariTemizle();
                btnSil.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0 && dataGridKalite.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir kalite satırı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tabControl1.SelectedIndex == 1 && dataGridMalzeme.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir malzeme satırı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tabControl1.SelectedIndex == 2 && dataGridKesim.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir kesim satırı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string message;
            if (tabControl1.SelectedIndex == 0)
            {
                message = "Seçili kalite kaydını silmek istediğinizden emin misiniz?";
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                message = "Seçili malzeme kaydını silmek istediğinizden emin misiniz?";
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                message = "Seçili kesim kaydını silmek istediğinizden emin misiniz?";
            }
            else
            {
                message = "Seçili kaydı silmek istediğinizden emin misiniz?";
            }

            if (MessageBox.Show(message, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                int id;
                string tableName;

                if (tabControl1.SelectedIndex == 0)
                {
                    id = Convert.ToInt32(dataGridKalite.SelectedRows[0].Cells["id"].Value);
                    tableName = "KarsilastirmaTablosuKalite";
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    id = Convert.ToInt32(dataGridMalzeme.SelectedRows[0].Cells["id"].Value);
                    tableName = "KarsilastirmaTablosuMalzeme";
                }
                else if (tabControl1.SelectedIndex == 2)
                {
                    id = Convert.ToInt32(dataGridKesim.SelectedRows[0].Cells["id"].Value);
                    tableName = "KarsilastirmaTablosuKesim";
                }
                else
                {
                    throw new InvalidOperationException("Geçersiz sekme indeksi");
                }

                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var command = new SqlCommand($"DELETE FROM {tableName} WHERE id = @Id", conn))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }

                if (tabControl1.SelectedIndex == 0)
                {
                    LoadKaliteData();
                    dataGridKalite.ClearSelection();
                    dataGridKalite.CurrentCell = null;
                    MessageBox.Show("Kalite başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    LoadMalzemeData();
                    dataGridMalzeme.ClearSelection();
                    dataGridMalzeme.CurrentCell = null;
                    MessageBox.Show("Malzeme başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (tabControl1.SelectedIndex == 2)
                {
                    LoadKesimData();
                    dataGridKesim.ClearSelection();
                    dataGridKesim.CurrentCell = null;
                    MessageBox.Show("Kesim başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                TextBoxlariTemizle();
                btnSil.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}