using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Helper;
using CEKA_APP.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing; // Font ve Color için gerekli

namespace CEKA_APP.UsrControl
{
    public partial class ctlMusteriler : UserControl
    {
        public ctlMusteriler()
        {
            InitializeComponent();

            DataGridViewHelper.StilUygulaMusteriler(dataGridMusteriler);

            this.Load += new EventHandler(ctlMusteriler_Load);

            if (this.dataGridMusteriler.ContextMenuStrip != null)
            {
                this.contextMenuStrip1 = (System.Windows.Forms.ContextMenuStrip)this.dataGridMusteriler.ContextMenuStrip;
                this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(cmsMusteriIslemleri_Opening);

                foreach (ToolStripItem item in contextMenuStrip1.Items)
                {
                    if (item.Name == "tsmiMusteriEkle")
                        tsmiMusteriEkle = (ToolStripMenuItem)item;
                    else if (item.Name == "tsmiMusteriGuncelle")
                        tsmiMusteriGuncelle = (ToolStripMenuItem)item;
                    else if (item.Name == "tsmiMusteriSil")
                        tsmiMusteriSil = (ToolStripMenuItem)item;
                }
            }
        }

        private void ctlMusteriler_Load(object sender, System.EventArgs e)
        {
            ctlBaslik1.Baslik="Müşteriler";

            LoadMusterilerToDataGridView();
        }

        private void LoadMusterilerToDataGridView()
        {
            ProjeFinans_MusterilerData musteriData = new ProjeFinans_MusterilerData();
            try
            {
                List<Musteriler> musteriler = musteriData.GetMusteriler();

                dataGridMusteriler.DataSource = musteriler;

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
                    dataGridMusteriler.Columns["musteriAdi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Genelde adı dolu gösterir
                }
                if (dataGridMusteriler.Columns.Contains("musteriMensei"))
                {
                    dataGridMusteriler.Columns["musteriMensei"].HeaderText = "Müşteri Menşei";
                    dataGridMusteriler.Columns["musteriMensei"].DisplayIndex = 2;
                    dataGridMusteriler.Columns["musteriMensei"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridMusteriler.Columns.Contains("vergiNo"))
                {
                    dataGridMusteriler.Columns["vergiNo"].HeaderText = "Vergi No";
                    dataGridMusteriler.Columns["vergiNo"].DisplayIndex = 3;
                    dataGridMusteriler.Columns["vergiNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridMusteriler.Columns.Contains("vergiDairesi"))
                {
                    dataGridMusteriler.Columns["vergiDairesi"].HeaderText = "Vergi Dairesi";
                    dataGridMusteriler.Columns["vergiDairesi"].DisplayIndex = 4;
                    dataGridMusteriler.Columns["vergiDairesi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridMusteriler.Columns.Contains("adres"))
                {
                    dataGridMusteriler.Columns["adres"].HeaderText = "Adres";
                    dataGridMusteriler.Columns["adres"].DisplayIndex = 5;
                    dataGridMusteriler.Columns["adres"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Adres alanı biraz daha geniş olabilir
                }
                if (dataGridMusteriler.Columns.Contains("olusturmaTarihi"))
                {
                    dataGridMusteriler.Columns["olusturmaTarihi"].HeaderText = "Oluşturma Tarihi";
                    dataGridMusteriler.Columns["olusturmaTarihi"].DisplayIndex = 6;
                    dataGridMusteriler.Columns["olusturmaTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                    dataGridMusteriler.Columns["olusturmaTarihi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müşteriler yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridMusteriler_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    dataGridMusteriler.ClearSelection(); 
                    dataGridMusteriler.Rows[e.RowIndex].Selected = true;
                    dataGridMusteriler.CurrentCell = dataGridMusteriler.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
            }
        }

        private void cmsMusteriIslemleri_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tsmiMusteriGuncelle != null) tsmiMusteriGuncelle.Enabled = false;
            if (tsmiMusteriSil != null) tsmiMusteriSil.Enabled = false;

            if (dataGridMusteriler.CurrentRow != null && dataGridMusteriler.CurrentRow.Index >= 0)
            {
                if (tsmiMusteriGuncelle != null) tsmiMusteriGuncelle.Enabled = true;
                if (tsmiMusteriSil != null) tsmiMusteriSil.Enabled = true;
            }
            if (tsmiMusteriEkle != null) tsmiMusteriEkle.Enabled = true;
        }


        private void tsmiMusteriEkle_Click(object sender, EventArgs e)
        {
            using (frmMusteriIslemleri musteriForm = new frmMusteriIslemleri(null)) 
            {
                if (musteriForm.ShowDialog() == DialogResult.OK)
                {
                    LoadMusterilerToDataGridView(); 
                }
            }
        }

        private void tsmiMusteriGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridMusteriler.CurrentRow != null)
            {
                Musteriler selectedMusteri = dataGridMusteriler.CurrentRow.DataBoundItem as Musteriler;

                if (selectedMusteri != null)
                {
                    using (frmMusteriIslemleri musteriForm = new frmMusteriIslemleri(selectedMusteri))
                    {
                        if (musteriForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadMusterilerToDataGridView(); 
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek için bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsmiMusteriSil_Click(object sender, EventArgs e)
        {
            if (dataGridMusteriler.CurrentRow != null)
            {
                Musteriler selectedMusteri = dataGridMusteriler.CurrentRow.DataBoundItem as Musteriler;

                if (selectedMusteri != null)
                {
                    DialogResult result = MessageBox.Show(
                        $"'{selectedMusteri.musteriAdi}' ({selectedMusteri.musteriNo}) adlı müşteriyi silmek istediğinizden emin misiniz?",
                        "Müşteri Silme Onayı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        ProjeFinans_MusterilerData musteriData = new ProjeFinans_MusterilerData();
                        try
                        {
                            musteriData.MusteriSil(selectedMusteri.musteriNo);
                            MessageBox.Show("Müşteri başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadMusterilerToDataGridView(); 
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Müşteri silinirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridMusteriler_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridMusteriler.Rows.Count > 0)
            {
                dataGridMusteriler.ClearSelection();
                dataGridMusteriler.CurrentCell = null;
            }
        }
    }
}