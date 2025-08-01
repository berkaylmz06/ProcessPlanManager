using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
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

namespace CEKA_APP.UsrControl.ProjeFinans
{
    public partial class ctlOdemeSartlariListe : UserControl
    {
        public ctlOdemeSartlariListe()
        {
            InitializeComponent();

            DataGridViewHelper.StilUygulaProjeFinans(dataGridOdemeSartlari);
        }

        private void ctlOdemeSartlariListe_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Ödeme Şartları Liste";

            LoadOdemeSartlariToDataGridView();
        }
        public void LoadOdemeSartlariToDataGridView()
        {
            ProjeFinans_OdemeSartlariData odemeSekilleriData = new ProjeFinans_OdemeSartlariData();
            try
            {
                List<OdemeSartlari> odemeSekilleri = odemeSekilleriData.GetOdemeBilgileri();

                dataGridOdemeSartlari.DataSource = odemeSekilleri;
                if (dataGridOdemeSartlari.Columns.Contains("odemeId"))
                {
                    dataGridOdemeSartlari.Columns["odemeId"].Visible = false;
                }
                if (dataGridOdemeSartlari.Columns.Contains("kilometreTasiId"))
                {
                    dataGridOdemeSartlari.Columns["kilometreTasiId"].Visible = false;
                }
                if (dataGridOdemeSartlari.Columns.Contains("projeNo"))
                {
                    dataGridOdemeSartlari.Columns["projeNo"].HeaderText = "Proje No";
                    dataGridOdemeSartlari.Columns["projeNo"].DisplayIndex = 0;
                    dataGridOdemeSartlari.Columns["projeNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("kilometreTasiAdi"))
                {
                    dataGridOdemeSartlari.Columns["kilometreTasiAdi"].HeaderText = "Kilometre Taşı";
                    dataGridOdemeSartlari.Columns["kilometreTasiAdi"].DisplayIndex = 1;
                    dataGridOdemeSartlari.Columns["kilometreTasiAdi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (dataGridOdemeSartlari.Columns.Contains("siralama"))
                {
                    dataGridOdemeSartlari.Columns["siralama"].HeaderText = "Sıra";
                    dataGridOdemeSartlari.Columns["siralama"].DisplayIndex = 2;
                    dataGridOdemeSartlari.Columns["siralama"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("oran"))
                {
                    dataGridOdemeSartlari.Columns["oran"].HeaderText = "Oran(%)";
                    dataGridOdemeSartlari.Columns["oran"].DisplayIndex = 3;
                    dataGridOdemeSartlari.Columns["oran"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("tutar"))
                {
                    dataGridOdemeSartlari.Columns["tutar"].HeaderText = "Tutar";
                    dataGridOdemeSartlari.Columns["tutar"].DisplayIndex = 4;
                    dataGridOdemeSartlari.Columns["tutar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("tahminiTarih"))
                {
                    dataGridOdemeSartlari.Columns["tahminiTarih"].HeaderText = "Tahmini Tarih";
                    dataGridOdemeSartlari.Columns["tahminiTarih"].DisplayIndex = 5;
                    dataGridOdemeSartlari.Columns["tahminiTarih"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dataGridOdemeSartlari.Columns["tahminiTarih"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (dataGridOdemeSartlari.Columns.Contains("gerceklesenTarih"))
                {
                    dataGridOdemeSartlari.Columns["gerceklesenTarih"].HeaderText = "Gerçekleşen Tarih";
                    dataGridOdemeSartlari.Columns["gerceklesenTarih"].DisplayIndex = 6;
                    dataGridOdemeSartlari.Columns["gerceklesenTarih"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dataGridOdemeSartlari.Columns["gerceklesenTarih"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("aciklama"))
                {
                    dataGridOdemeSartlari.Columns["aciklama"].HeaderText = "Açıklama";
                    dataGridOdemeSartlari.Columns["aciklama"].DisplayIndex = 7;
                    dataGridOdemeSartlari.Columns["aciklama"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("teminatMektubu"))
                {
                    dataGridOdemeSartlari.Columns["teminatMektubu"].HeaderText = "Teminat Mektubu Var Mi?";
                    dataGridOdemeSartlari.Columns["teminatMektubu"].DisplayIndex = 8;
                    dataGridOdemeSartlari.Columns["teminatMektubu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("teminatDurumu"))
                {
                    dataGridOdemeSartlari.Columns["teminatDurumu"].HeaderText = "Teminat Durumu";
                    dataGridOdemeSartlari.Columns["teminatDurumu"].DisplayIndex = 9;
                    dataGridOdemeSartlari.Columns["teminatDurumu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("durum"))
                {
                    dataGridOdemeSartlari.Columns["durum"].HeaderText = "Durum";
                    dataGridOdemeSartlari.Columns["durum"].DisplayIndex = 10;
                    dataGridOdemeSartlari.Columns["durum"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Odeme şartlari bilgileri yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
