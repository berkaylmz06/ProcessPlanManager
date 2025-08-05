using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Helper;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CEKA_APP.Forms;

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
            dataGridOdemeSartlari.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridOdemeSartlari_CellFormatting);
        }

        public void LoadOdemeSartlariToDataGridView()
        {
            ProjeFinans_OdemeSartlariData odemeSekilleriData = new ProjeFinans_OdemeSartlariData();
            try
            {
                List<OdemeSartlari> odemeSekilleri = odemeSekilleriData.GetOdemeBilgileri();
                dataGridOdemeSartlari.DataSource = odemeSekilleri;

                if (dataGridOdemeSartlari.Columns.Contains("odemeId"))
                    dataGridOdemeSartlari.Columns["odemeId"].Visible = false;
                if (dataGridOdemeSartlari.Columns.Contains("kilometreTasiId"))
                    dataGridOdemeSartlari.Columns["kilometreTasiId"].Visible = false;
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
                    dataGridOdemeSartlari.Columns["tahminiTarih"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
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
                    dataGridOdemeSartlari.Columns["teminatMektubu"].HeaderText = "Teminat Mektubu Var Mı?";
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
                if (dataGridOdemeSartlari.Columns.Contains("kalanTutar"))
                {
                    dataGridOdemeSartlari.Columns["kalanTutar"].HeaderText = "Kalan Tutar";
                    dataGridOdemeSartlari.Columns["kalanTutar"].DisplayIndex = 11;
                    dataGridOdemeSartlari.Columns["kalanTutar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("odemeTarihi"))
                {
                    dataGridOdemeSartlari.Columns["odemeTarihi"].HeaderText = "Ödeme Tarihi";
                    dataGridOdemeSartlari.Columns["odemeTarihi"].DisplayIndex = 12;
                    dataGridOdemeSartlari.Columns["odemeTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dataGridOdemeSartlari.Columns["odemeTarihi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                if (dataGridOdemeSartlari.Columns.Contains("faturaNo"))
                {
                    dataGridOdemeSartlari.Columns["faturaNo"].HeaderText = "Fatura No";
                    dataGridOdemeSartlari.Columns["faturaNo"].DisplayIndex = 13;
                    dataGridOdemeSartlari.Columns["faturaNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }

                if (!dataGridOdemeSartlari.Columns.Contains("OdemeSapmasi"))
                {
                    DataGridViewTextBoxColumn sapmaColumn = new DataGridViewTextBoxColumn
                    {
                        Name = "OdemeSapmasi",
                        HeaderText = "Ödeme Sapması (Gün)",
                        DisplayIndex = 14,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                    };
                    dataGridOdemeSartlari.Columns.Add(sapmaColumn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödeme şartları bilgileri yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridOdemeSartlari_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridOdemeSartlari.RowCount)
            {
                return;
            }

            int odemeId;
            try
            {
                odemeId = Convert.ToInt32(dataGridOdemeSartlari.Rows[e.RowIndex].Cells["odemeId"].Value);
            }
            catch
            {
                MessageBox.Show("Seçilen satırda geçerli bir 'odemeId' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var odemeHareketleriData = new ProjeFinans_OdemeHareketleriData();
            List<OdemeHareketleri> hareketler = odemeHareketleriData.GetOdemeHareketleriByOdemeId(odemeId);

            if (hareketler != null && hareketler.Count > 0)
            {
                frmOdemeHareketleri odemeHareketleriForm = new frmOdemeHareketleri(hareketler);
                odemeHareketleriForm.ShowDialog();
            }
        }

        private void dataGridOdemeSartlari_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridOdemeSartlari.Columns[e.ColumnIndex].Name == "OdemeSapmasi")
            {
                var row = dataGridOdemeSartlari.Rows[e.RowIndex];
                var gerceklesenTarih = row.Cells["gerceklesenTarih"].Value;
                var odemeTarihi = row.Cells["odemeTarihi"].Value;

                if (gerceklesenTarih != null && odemeTarihi != null && gerceklesenTarih != DBNull.Value && odemeTarihi != DBNull.Value)
                {
                    try
                    {
                        DateTime gerceklesen = Convert.ToDateTime(gerceklesenTarih);
                        DateTime odeme = Convert.ToDateTime(odemeTarihi);
                        int sapma = (odeme - gerceklesen).Days;

                        if (sapma > 0)
                        {
                            e.Value = $"-{sapma}";
                            e.CellStyle.ForeColor = System.Drawing.Color.Red;
                        }
                        else if (sapma < 0)
                        {
                            e.Value = $"+{Math.Abs(sapma)}"; 
                            e.CellStyle.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            e.Value = "0"; 
                            e.CellStyle.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    catch
                    {
                        e.Value = "-";
                        e.CellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
                else
                {
                    e.Value = "-";
                    e.CellStyle.ForeColor = System.Drawing.Color.Black;
                }
            }
        }
    }
}