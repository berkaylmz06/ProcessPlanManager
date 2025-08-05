using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Helper;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CEKA_APP.Forms
{
    public partial class frmOdemeHareketleri : Form
    {
        public frmOdemeHareketleri(List<OdemeHareketleri> hareketler)
        {
            InitializeComponent();
            LoadOdemeHareketleri(hareketler);

            this.Icon = Properties.Resources.cekalogokirmizi;
        }

        private void LoadOdemeHareketleri(List<OdemeHareketleri> hareketler)
        {
            try
            {
                dataGridOdemeHareketleri.DataSource = hareketler;
                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödeme hareketleri yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void FormatDataGridView()
        {
            DataGridViewHelper.StilUygulaProjeFinans(dataGridOdemeHareketleri);

            if (dataGridOdemeHareketleri.Columns.Contains("odemeId"))
                dataGridOdemeHareketleri.Columns["odemeId"].Visible = false;
            if (dataGridOdemeHareketleri.Columns.Contains("odemeHareketId"))
                dataGridOdemeHareketleri.Columns["odemeHareketId"].HeaderText = "Hareket ID";
            if (dataGridOdemeHareketleri.Columns.Contains("odemeMiktari"))
                dataGridOdemeHareketleri.Columns["odemeMiktari"].HeaderText = "Ödeme Miktarı";
            if (dataGridOdemeHareketleri.Columns.Contains("odemeTarihi"))
            {
                dataGridOdemeHareketleri.Columns["odemeTarihi"].HeaderText = "Ödeme Tarihi";
                dataGridOdemeHareketleri.Columns["odemeTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
            }
            if (dataGridOdemeHareketleri.Columns.Contains("odemeAciklama"))
                dataGridOdemeHareketleri.Columns["odemeAciklama"].HeaderText = "Açıklama";
        }
    }
}