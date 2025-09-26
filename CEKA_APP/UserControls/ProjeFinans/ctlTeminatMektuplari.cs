using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Properties;
using CEKA_APP.Services.ProjeFinans;
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
                    else if (item.Name == "tsmiSutunSiralama")
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
            try
            {
                dataGridTeminatMektuplari.DataSource = null;
                var mektuplar = _teminatMektuplariService.GetTeminatMektuplari();
                dataGridTeminatMektuplari.DataSource = mektuplar;
                ConfigureDataGridViewColumns();
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
                (dt) => { dataGridTeminatMektuplari.DataSource = dt; },
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
                "komisyonOrani", "komisyonVadesi", "kilometreTasiId"
            };
        }
    }
}