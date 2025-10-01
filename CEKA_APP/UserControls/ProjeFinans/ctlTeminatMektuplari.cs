using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Properties;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlTeminatMektuplari : UserControl
    {

        private readonly IServiceProvider _serviceProvider;
        private IMusterilerService _musterilerService => _serviceProvider.GetRequiredService<IMusterilerService>();
        private IFinansProjelerService _finansProjelerService => _serviceProvider.GetRequiredService<IFinansProjelerService>();
        private IProjeKutukService _projeKutukService => _serviceProvider.GetRequiredService<IProjeKutukService>();
        private ITeminatMektuplariService _teminatMektuplariService => _serviceProvider.GetRequiredService<ITeminatMektuplariService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();

        public ctlTeminatMektuplari(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DataGridViewHelper.StilUygulaProjeFinans(dataGridTeminatMektuplari);
           
            dataGridTeminatMektuplari.AutoGenerateColumns = false;
        }

        private void ctlTeminatMektuplari_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Teminat Mektupları";
            LoadMektuplarToDataGridView();
            DataGridViewSettingsManager.LoadColumnWidths(dataGridTeminatMektuplari, "SutunGenislikTeminatMektuplari");
        }

        private void LoadMektuplarToDataGridView()
        {
            try
            {
                List<TeminatMektuplari> mektuplar = _teminatMektuplariService.GetTeminatMektuplari();
                dataGridTeminatMektuplari.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridTeminatMektuplari.DataSource = new SiralabilirBindingList<TeminatMektuplari>(mektuplar);

                ConfigureDataGridViewColumns();

                dataGridTeminatMektuplari.ScrollBars = ScrollBars.Both;
                DataGridViewSettingsManager.LoadColumnWidths(dataGridTeminatMektuplari, "SutunGenislikTeminatMektuplari");
                foreach (var m in mektuplar)
                {
                    Console.WriteLine(m.projeNo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mektuplar yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            var sutunSirasi = Settings.Default.SutunSirasiTeminatMektuplari != null
                ? StringCollectionToList(Settings.Default.SutunSirasiTeminatMektuplari)
                : VarsayilanSutunSirasiniAl();

            dataGridTeminatMektuplari.SuspendLayout();
            try
            {
                for (int i = 0; i < sutunSirasi.Count; i++)
                {
                    var sutunAdi = sutunSirasi[i];
                    DataGridViewColumn col;

                    if (!dataGridTeminatMektuplari.Columns.Contains(sutunAdi))
                    {
                        col = new DataGridViewTextBoxColumn
                        {
                            Name = sutunAdi,
                            DataPropertyName = sutunAdi,
                            Visible = true,
                            MinimumWidth = 50
                        };
                        dataGridTeminatMektuplari.Columns.Add(col);
                    }
                    else
                    {
                        col = dataGridTeminatMektuplari.Columns[sutunAdi];
                        col.DataPropertyName = sutunAdi;
                        col.Visible = true;
                        col.MinimumWidth = 50;
                    }

                    switch (sutunAdi)
                    {
                        case "mektupNo": col.HeaderText = "Mektup No"; break;
                        case "musteriNo": col.HeaderText = "Müşteri No"; break;
                        case "musteriAdi": col.HeaderText = "Müşteri Adı"; break;
                        case "kilometreTasiAdi": col.HeaderText = "Kilometre Taşı Adı"; break;
                        case "paraBirimi": col.HeaderText = "Para Birimi"; break;
                        case "banka": col.HeaderText = "Banka"; break;
                        case "mektupTuru": col.HeaderText = "Mektup Türü"; break;
                        case "tutar": col.HeaderText = "Tutar"; break;
                        case "vadeTarihi":
                            col.HeaderText = "Vade Tarihi";
                            col.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                            break;
                        case "iadeTarihi":
                            col.HeaderText = "İade Tarihi";
                            col.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                            break;
                        case "komisyonTutari": col.HeaderText = "Komisyon Tutarı"; break;
                        case "komisyonOrani": col.HeaderText = "Komisyon Oranı"; break;
                        case "komisyonVadesi": col.HeaderText = "Komisyon Vadesi"; break;
                        case "projeNo": col.HeaderText = "Proje No"; break;
                        case "kilometreTasiId":
                        case "projeId":
                            col.Visible = false; break;
                    }

                    col.DisplayIndex = i;
                }

                foreach (DataGridViewColumn c in dataGridTeminatMektuplari.Columns)
                {
                    if (!sutunSirasi.Contains(c.Name))
                        c.Visible = false;
                }
            }
            finally
            {
                dataGridTeminatMektuplari.ResumeLayout();
            }

            DataGridViewSettingsManager.LoadColumnWidths(dataGridTeminatMektuplari, "SutunGenislikTeminatMektuplari");
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
            using (frmTeminatMektubuEkle mektupForm = new frmTeminatMektubuEkle(_musterilerService, null, _finansProjelerService, _projeKutukService, _teminatMektuplariService))
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
                    using (frmTeminatMektubuEkle musteriForm = new frmTeminatMektubuEkle(
                        _musterilerService,
                        selectedMektup,
                        _finansProjelerService,
                        _projeKutukService,
                        _teminatMektuplariService,
                        selectedMektup.projeId))
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
                        try
                        {
                            _teminatMektuplariService.MektupSil(selectedMektup.mektupNo);
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
            string baseSql = _teminatMektuplariService.GetTeminatMektuplariQuery();

            var frm = new frmAra(
                dataGridTeminatMektuplari,
                _tabloFiltreleService,
                (dt) => 
                { 
                    dataGridTeminatMektuplari.DataSource = dt;
                    ConfigureDataGridViewColumns();
                    DataGridViewSettingsManager.LoadColumnWidths(dataGridTeminatMektuplari, "SutunGenislikTeminatMektuplari");
                },
                baseSql,
                _serviceProvider,
                detayEkle: false
            );

            frm.ShowDialog();
        }


        private void tsmiSutunSirala_Click(object sender, EventArgs e)
        {
            var mevcutSira = Settings.Default.SutunSirasiTeminatMektuplari != null
                ? StringCollectionToList(Settings.Default.SutunSirasiTeminatMektuplari)
                : VarsayilanSutunSirasiniAl();
            var gorunurSutunlar = mevcutSira.Where(sutun =>
                dataGridTeminatMektuplari.Columns.Contains(sutun) &&
                dataGridTeminatMektuplari.Columns[sutun].Visible).ToList();

            frmSutunSiralama sutunSiralamaFormu = new frmSutunSiralama(gorunurSutunlar);
            if (sutunSiralamaFormu.ShowDialog() == DialogResult.OK)
            {
                var yeniSira = sutunSiralamaFormu.SutunSirasiniAl();
                var tamSira = VarsayilanSutunSirasiniAl();
                var gorunmezSutunlar = tamSira.Except(yeniSira).ToList();
                yeniSira.AddRange(gorunmezSutunlar);

                Settings.Default.SutunSirasiTeminatMektuplari = new System.Collections.Specialized.StringCollection();
                Settings.Default.SutunSirasiTeminatMektuplari.AddRange(yeniSira.ToArray());
                Settings.Default.Save();
                LoadMektuplarToDataGridView();
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
                "mektupNo", "musteriNo", "musteriAdi", "kilometreTasiAdi", "paraBirimi", "banka",
                "mektupTuru", "tutar", "vadeTarihi", "iadeTarihi", "komisyonTutari",
                "komisyonOrani", "komisyonVadesi", "kilometreTasiId", "projeId", "projeNo"
            };
        }

        private void dataGridTeminatMektuplari_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridViewSettingsManager.SaveColumnWidths(dataGridTeminatMektuplari, "SutunGenislikTeminatMektuplari");
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