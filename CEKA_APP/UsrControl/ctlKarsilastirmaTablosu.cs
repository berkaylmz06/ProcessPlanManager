using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CEKA_APP.DataBase;
using CEKA_APP.Helper;

namespace CEKA_APP.UsrControl
{
    public partial class ctlKarsilastirmaTablosu : UserControl
    {
        private DataTable kaliteTablo;
        private DataTable malzemeTablo;
        private BindingSource kaliteBindingSource;
        private BindingSource malzemeBindingSource;

        public ctlKarsilastirmaTablosu()
        {
            InitializeComponent();
            DataGridYukle();

            DataGridViewHelper.StilUygula(dataGridKalite);
            DataGridViewHelper.StilUygula(dataGridMalzeme);

            dataGridKalite.ClearSelection();
            dataGridKalite.CurrentCell = null;
            dataGridMalzeme.ClearSelection();
            dataGridMalzeme.CurrentCell = null;

            btnEkle.Enabled = false;
            btnSil.Enabled = false;

        }

        private void TextBoxlariTemizle()
        {
            txtCode.Text = "";
            txtIfsCode.Text = "";
            txtAciklama.Text = "";
        }

        public void DataGridYukle()
        {
            kaliteTablo = KarsilastirmaTablosuData.GetAllKaliteKarsilastirmalari();
            kaliteBindingSource = new BindingSource { DataSource = kaliteTablo };
            dataGridKalite.DataSource = kaliteBindingSource;
            dataGridKalite.Columns["id"].Visible = false;
            dataGridKalite.Columns["CekaCode"].HeaderText = "Çeka Kodu";
            dataGridKalite.Columns["IfsCode"].HeaderText = "IFS Kodu";
            dataGridKalite.Columns["Aciklama"].HeaderText = "Açıklama";

            malzemeTablo = KarsilastirmaTablosuData.GetAllMalzemeKarsilastirmalari();
            malzemeBindingSource = new BindingSource { DataSource = malzemeTablo };
            dataGridMalzeme.DataSource = malzemeBindingSource;
            dataGridMalzeme.Columns["id"].Visible = false;
            dataGridMalzeme.Columns["AutoCadCode"].HeaderText = "AutoCAD Kodu";
            dataGridMalzeme.Columns["IfsCode"].HeaderText = "IFS Kodu";
            dataGridMalzeme.Columns["Aciklama"].HeaderText = "Açıklama";

            dataGridKalite.ClearSelection();
            dataGridKalite.CurrentCell = null;
            dataGridMalzeme.ClearSelection();
            dataGridMalzeme.CurrentCell = null;
            TextBoxlariTemizle();
        }

        private void ctlKarsilastirmaTablosu_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Karşılaştırma Tablosu";

            DataGridYukle();

            dataGridKalite.DataSource = null;
            dataGridKalite.ClearSelection();
            dataGridKalite.CurrentCell = null;

            dataGridMalzeme.DataSource = null;
            dataGridMalzeme.ClearSelection();
            dataGridMalzeme.CurrentCell = null;

            tabControl1.SelectedIndex = -1;
            TextBoxlariTemizle();
            btnEkle.Enabled = false;
            btnSil.Enabled = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxlariTemizle();
            btnEkle.Enabled = tabControl1.SelectedIndex >= 0;
            btnSil.Enabled = false;

            if (tabControl1.SelectedIndex == 0)
            {
                dataGridKalite.DataSource = kaliteBindingSource;
                dataGridMalzeme.DataSource = null;
                dataGridKalite.Columns["id"].Visible = false;
                dataGridKalite.ClearSelection();
                dataGridKalite.CurrentCell = null;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                dataGridMalzeme.DataSource = malzemeBindingSource;
                dataGridKalite.DataSource = null;
                dataGridMalzeme.Columns["id"].Visible = false;
                dataGridMalzeme.ClearSelection();
                dataGridMalzeme.CurrentCell = null;
            }
            else
            {
                dataGridKalite.DataSource = null;
                dataGridMalzeme.DataSource = null;
                dataGridKalite.ClearSelection();
                dataGridMalzeme.ClearSelection();
                dataGridKalite.CurrentCell = null;
                dataGridMalzeme.CurrentCell = null;
            }

            labelCode.Text = tabControl1.SelectedIndex == 0 ? "Çeka Kodu:" : "AutoCAD Kodu:";
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

                string message = tabControl1.SelectedIndex == 0
                    ? "Seçili kalite kaydını eklemek istediğinizden emin misiniz?"
                    : "Seçili malzeme kaydını eklemek istediğinizden emin misiniz?";
                DialogResult result = MessageBox.Show(message, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (tabControl1.SelectedIndex == 0)
                    {
                        KarsilastirmaTablosuData.SaveKarsilastirmaKalite(code, ifsCode, aciklama);
                        kaliteTablo.Clear();
                        kaliteTablo = KarsilastirmaTablosuData.GetAllKaliteKarsilastirmalari();
                        kaliteBindingSource.DataSource = kaliteTablo;
                        dataGridKalite.ClearSelection();
                        dataGridKalite.CurrentCell = null;
                        TextBoxlariTemizle();
                        MessageBox.Show("Kalite başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (tabControl1.SelectedIndex == 1)
                    {
                        KarsilastirmaTablosuData.SaveKarsilastirmaMalzeme(code, ifsCode, aciklama);
                        malzemeTablo.Clear();
                        malzemeTablo = KarsilastirmaTablosuData.GetAllMalzemeKarsilastirmalari();
                        malzemeBindingSource.DataSource = malzemeTablo;
                        dataGridMalzeme.ClearSelection();
                        dataGridMalzeme.CurrentCell = null;
                        TextBoxlariTemizle();
                        MessageBox.Show("Malzeme başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
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

            string message = tabControl1.SelectedIndex == 0
                ? "Seçili kalite kaydını silmek istediğinizden emin misiniz?"
                : "Seçili malzeme kaydını silmek istediğinizden emin misiniz?";
            DialogResult result = MessageBox.Show(message, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int id = tabControl1.SelectedIndex == 0
                        ? Convert.ToInt32(dataGridKalite.SelectedRows[0].Cells["id"].Value)
                        : Convert.ToInt32(dataGridMalzeme.SelectedRows[0].Cells["id"].Value);

                    using (var conn = DataBaseHelper.GetConnection())
                    {
                        conn.Open();
                        string tableName = tabControl1.SelectedIndex == 0 ? "KarsilastirmaTablosuKalite" : "KarsilastirmaTablosuMalzeme";
                        var command = new SqlCommand($"DELETE FROM {tableName} WHERE id = @Id", conn);
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }

                    if (tabControl1.SelectedIndex == 0)
                    {
                        kaliteTablo.Clear();
                        kaliteTablo = KarsilastirmaTablosuData.GetAllKaliteKarsilastirmalari();
                        kaliteBindingSource.DataSource = kaliteTablo;
                        dataGridKalite.ClearSelection();
                        dataGridKalite.CurrentCell = null;
                        TextBoxlariTemizle();
                        MessageBox.Show("Kalite başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (tabControl1.SelectedIndex == 1)
                    {
                        malzemeTablo.Clear();
                        malzemeTablo = KarsilastirmaTablosuData.GetAllMalzemeKarsilastirmalari();
                        malzemeBindingSource.DataSource = malzemeTablo;
                        dataGridMalzeme.ClearSelection();
                        dataGridMalzeme.CurrentCell = null;
                        TextBoxlariTemizle();
                        MessageBox.Show("Malzeme başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}