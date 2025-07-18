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
        public ctlTeminatMektuplari()
        {
            InitializeComponent();

            DataGridViewHelper.StilUygulaProjeFinans(dataGridTeminatMektuplari);

            if (this.dataGridTeminatMektuplari.ContextMenuStrip != null)
            {
                this.cmsTeminatMektubuIslemleri = (System.Windows.Forms.ContextMenuStrip)this.dataGridTeminatMektuplari.ContextMenuStrip;

                foreach (ToolStripItem item in cmsTeminatMektubuIslemleri.Items)
                {
                    if (item.Name == "tsmiMektupEkle")
                        tsmiMektupEkle = (ToolStripMenuItem)item;
                    else if (item.Name == "tsmiMektupGuncelle")
                        tsmiMektupGuncelle = (ToolStripMenuItem)item;
                    else if (item.Name == "tsmiMektupSil")
                        tsmiMektupSil= (ToolStripMenuItem)item;
                }
            }
        }

        private void ctlTeminatMektuplari_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Teminat Mektupları";

            LoadMektuplarToDataGridView();
        }
        public void LoadMektuplarToDataGridView()
        {
            ProjeFinans_TeminatMektuplariData mektupData = new ProjeFinans_TeminatMektuplariData();
            try
            {
                List<TeminatMektuplari> mektuplar = mektupData.GetTeminatMektuplari();

                dataGridTeminatMektuplari.DataSource = mektuplar;

                if (dataGridTeminatMektuplari.Columns.Contains("mektupNo"))
                {
                    dataGridTeminatMektuplari.Columns["mektupNo"].HeaderText = "Mektup No";
                    dataGridTeminatMektuplari.Columns["mektupNo"].DisplayIndex = 0;
                    dataGridTeminatMektuplari.Columns["mektupNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("musteriNo"))
                {
                    dataGridTeminatMektuplari.Columns["musteriNo"].HeaderText = "Müşteri No";
                    dataGridTeminatMektuplari.Columns["musteriNo"].DisplayIndex = 1;
                    dataGridTeminatMektuplari.Columns["musteriNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("musteriAdi"))
                {
                    dataGridTeminatMektuplari.Columns["musteriAdi"].HeaderText = "Müşteri Adı";
                    dataGridTeminatMektuplari.Columns["musteriAdi"].DisplayIndex = 2;
                    dataGridTeminatMektuplari.Columns["musteriAdi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("paraBirimi"))
                {
                    dataGridTeminatMektuplari.Columns["paraBirimi"].HeaderText = "Para Birimi";
                    dataGridTeminatMektuplari.Columns["paraBirimi"].DisplayIndex = 3;
                    dataGridTeminatMektuplari.Columns["paraBirimi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("banka"))
                {
                    dataGridTeminatMektuplari.Columns["banka"].HeaderText = "Banka";
                    dataGridTeminatMektuplari.Columns["banka"].DisplayIndex = 4;
                    dataGridTeminatMektuplari.Columns["banka"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("mektupTuru"))
                {
                    dataGridTeminatMektuplari.Columns["mektupTuru"].HeaderText = "Mektup Türü";
                    dataGridTeminatMektuplari.Columns["mektupTuru"].DisplayIndex = 5;
                    dataGridTeminatMektuplari.Columns["mektupTuru"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 
                }
                if (dataGridTeminatMektuplari.Columns.Contains("tutar"))
                {
                    dataGridTeminatMektuplari.Columns["tutar"].HeaderText = "Tutar";
                    dataGridTeminatMektuplari.Columns["tutar"].DisplayIndex = 6;
                    dataGridTeminatMektuplari.Columns["tutar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("vadeTarihi"))
                {
                    dataGridTeminatMektuplari.Columns["vadeTarihi"].HeaderText = "Vade Tarihi";
                    dataGridTeminatMektuplari.Columns["vadeTarihi"].DisplayIndex = 7;
                    dataGridTeminatMektuplari.Columns["vadeTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                    dataGridTeminatMektuplari.Columns["vadeTarihi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("iadeTarihi"))
                {
                    dataGridTeminatMektuplari.Columns["iadeTarihi"].HeaderText = "İade Tarihi";
                    dataGridTeminatMektuplari.Columns["iadeTarihi"].DisplayIndex = 8;
                    dataGridTeminatMektuplari.Columns["iadeTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                    dataGridTeminatMektuplari.Columns["iadeTarihi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("komisyonTutari"))
                {
                    dataGridTeminatMektuplari.Columns["komisyonTutari"].HeaderText = "Komisyon Tutarı";
                    dataGridTeminatMektuplari.Columns["komisyonTutari"].DisplayIndex = 9;
                    dataGridTeminatMektuplari.Columns["komisyonTutari"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("komisyonOrani"))
                {
                    dataGridTeminatMektuplari.Columns["komisyonOrani"].HeaderText = "Komisyon Oranı";
                    dataGridTeminatMektuplari.Columns["komisyonOrani"].DisplayIndex = 10;
                    dataGridTeminatMektuplari.Columns["komisyonOrani"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridTeminatMektuplari.Columns.Contains("komisyonVadesi"))
                {
                    dataGridTeminatMektuplari.Columns["komisyonVadesi"].HeaderText = "Komisyon Vadesi";
                    dataGridTeminatMektuplari.Columns["komisyonVadesi"].DisplayIndex = 11;
                    dataGridTeminatMektuplari.Columns["komisyonVadesi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mektuplar yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridTeminatMektuplari_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    dataGridTeminatMektuplari.ClearSelection();
                    dataGridTeminatMektuplari.Rows[e.RowIndex].Selected = true;
                    dataGridTeminatMektuplari.CurrentCell = dataGridTeminatMektuplari.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
            }
        }

        private void cmsTeminatMektubuIslemleri_Opening(object sender, CancelEventArgs e)
        {
            if (tsmiMektupGuncelle != null) tsmiMektupGuncelle.Enabled = false;
            if (tsmiMektupSil != null) tsmiMektupSil.Enabled = false;

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
                            MessageBox.Show($"Mektup silinirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir mektup seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridTeminatMektuplari_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridTeminatMektuplari.Rows.Count > 0)
            {
                dataGridTeminatMektuplari.ClearSelection();
                dataGridTeminatMektuplari.CurrentCell = null;
            }
        }
    }
}
