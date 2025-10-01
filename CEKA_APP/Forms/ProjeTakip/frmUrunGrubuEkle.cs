using CEKA_APP.Entitys.ProjeTakip;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.ProjeTakip;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CEKA_APP.Forms.ProjeTakip
{
    public partial class frmUrunGrubuEkle : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUrunGruplariService _urunGruplariService;
        public List<UrunGruplari> SelectedUrunGruplari { get; private set; }

        public frmUrunGrubuEkle(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _urunGruplariService = _serviceProvider.GetRequiredService<IUrunGruplariService>();
            this.Icon = Properties.Resources.cekalogokirmizi;
            SelectedUrunGruplari = new List<UrunGruplari>();


            DataGridViewHelper.StilUygulaUrunGrubuSecim(dataGridUrunGruplari);
        }

        private void frmUrunGrubuEkle_Load(object sender, EventArgs e)
        {
            ConfigureDataGridViewColumns();
            LoadUrunGruplariToDataGridView();
        }

        private void LoadUrunGruplariToDataGridView()
        {
            try
            {
                List<UrunGruplari> urunGruplari = _urunGruplariService.GetUrunGrubuBilgileri();
                dataGridUrunGruplari.DataSource = null;
                dataGridUrunGruplari.DataSource = urunGruplari;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürün grupları yüklenirken bir sorun oluştu: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            dataGridUrunGruplari.Columns.Clear();

            dataGridUrunGruplari.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridUrunGruplari.MultiSelect = true;
            dataGridUrunGruplari.AllowUserToAddRows = false;
            dataGridUrunGruplari.AutoGenerateColumns = false;

            dataGridUrunGruplari.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "+",
                Name = "Secim",   
                Width = 40,
                ReadOnly = false  
            });

            dataGridUrunGruplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "urunGrubu",
                HeaderText = "Ürün Grubu",
                Name = "UrunGrubu",
                ReadOnly = false
            });

            dataGridUrunGruplari.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "urunGrubuAdi",
                HeaderText = "Ürün Grubu Adı",
                Name = "UrunGrubuAdi"
            });
        }

        private void dataGridUrunGruplari_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == dataGridUrunGruplari.Columns["Secim"].Index)
            {
                try
                {
                    var urunGruplari = (List<UrunGruplari>)dataGridUrunGruplari.DataSource ?? new List<UrunGruplari>();

                    urunGruplari.Add(new UrunGruplari
                    {
                        urunGrubuId = 0,
                        urunGrubu = "",
                        urunGrubuAdi = ""
                    });

                    dataGridUrunGruplari.DataSource = null;
                    dataGridUrunGruplari.DataSource = urunGruplari;

                    dataGridUrunGruplari.CurrentCell = dataGridUrunGruplari.Rows[dataGridUrunGruplari.Rows.Count - 1].Cells["UrunGrubu"];
                    dataGridUrunGruplari.BeginEdit(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Yeni satır eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSecilenleriEkle_Click(object sender, EventArgs e)
        {
            SelectedUrunGruplari.Clear();

            foreach (DataGridViewRow row in dataGridUrunGruplari.Rows)
            {
                bool secildi = Convert.ToBoolean(row.Cells["Secim"].Value ?? false);
                if (secildi && row.DataBoundItem is UrunGruplari urunGrubu)
                {
                    SelectedUrunGruplari.Add(urunGrubu);
                }
            }

            if (SelectedUrunGruplari.Count == 0)
            {
                MessageBox.Show("Lütfen en az bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var urunGruplari = (List<UrunGruplari>)dataGridUrunGruplari.DataSource;
                if (urunGruplari == null || urunGruplari.Count == 0)
                {
                    MessageBox.Show("Kaydedilecek veri bulunamadı!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (var urunGrubu in urunGruplari)
                {
                    if (string.IsNullOrWhiteSpace(urunGrubu.urunGrubu) ||
                        string.IsNullOrWhiteSpace(urunGrubu.urunGrubuAdi))
                    {
                        MessageBox.Show("Ürün Grubu ve Ürün Grubu Adı boş olamaz!",
                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (urunGrubu.urunGrubuId > 0)
                    {
                        bool degisiklikVar;
                        _urunGruplariService.UrunGrubuGuncelle(
                            urunGrubu.urunGrubuId,
                            urunGrubu.urunGrubu,
                            urunGrubu.urunGrubuAdi,
                            out degisiklikVar
                        );
                    }
                    else
                    {
                        _urunGruplariService.UrunGrubuEkle(
                            urunGrubu.urunGrubu,
                            urunGrubu.urunGrubuAdi
                        );
                    }
                }

                MessageBox.Show("Veriler başarıyla kaydedildi.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUrunGruplariToDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler kaydedilirken hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Arama işlevselliği henüz uygulanmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            var seciliSatirlar = new List<UrunGruplari>();

            foreach (DataGridViewRow row in dataGridUrunGruplari.Rows)
            {
                bool secildi = Convert.ToBoolean(row.Cells["Secim"].Value ?? false);
                if (secildi && row.DataBoundItem is UrunGruplari urunGrubu)
                {
                    seciliSatirlar.Add(urunGrubu);
                }
            }

            if (seciliSatirlar.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için en az bir satırı işaretleyin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Seçilen ürün gruplarını silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    foreach (var urunGrubu in seciliSatirlar)
                    {
                        if (urunGrubu.urunGrubuId > 0)
                        {
                            _urunGruplariService.UrunGrubuSil(urunGrubu.urunGrubuId);
                        }
                    }

                    MessageBox.Show("Seçilen ürün grupları başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUrunGruplariToDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ürün grupları silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridUrunGruplari.SelectedRows.Count != 1)
            {
                MessageBox.Show("Lütfen güncellemek için yalnızca bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridUrunGruplari.SelectedRows[0];
            if (selectedRow.DataBoundItem is UrunGruplari urunGrubu)
            {
                using (var dialog = new Form())
                {
                    dialog.Text = "Ürün Grubu Güncelle";
                    dialog.Size = new System.Drawing.Size(300, 200);
                    dialog.StartPosition = FormStartPosition.CenterParent;

                    Label lblUrunGrubu = new Label { Text = "Ürün Grubu:", Location = new System.Drawing.Point(20, 20), AutoSize = true };
                    TextBox txtUrunGrubu = new TextBox { Location = new System.Drawing.Point(20, 40), Width = 200, Text = urunGrubu.urunGrubu };
                    Label lblUrunGrubuAdi = new Label { Text = "Ürün Grubu Adı:", Location = new System.Drawing.Point(20, 70), AutoSize = true };
                    TextBox txtUrunGrubuAdi = new TextBox { Location = new System.Drawing.Point(20, 90), Width = 200, Text = urunGrubu.urunGrubuAdi };
                    Button btnKaydet = new Button { Text = "Kaydet", Location = new System.Drawing.Point(20, 120), Width = 80 };
                    Button btnIptal = new Button { Text = "İptal", Location = new System.Drawing.Point(110, 120), Width = 80 };

                    btnKaydet.Click += (s, args) =>
                    {
                        if (string.IsNullOrWhiteSpace(txtUrunGrubu.Text) || string.IsNullOrWhiteSpace(txtUrunGrubuAdi.Text))
                        {
                            MessageBox.Show("Ürün Grubu ve Ürün Grubu Adı boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        try
                        {
                            bool degisiklikVar;
                            _urunGruplariService.UrunGrubuGuncelle(urunGrubu.urunGrubuId, txtUrunGrubu.Text, txtUrunGrubuAdi.Text, out degisiklikVar);
                            if (degisiklikVar)
                            {
                                MessageBox.Show("Ürün grubu başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            LoadUrunGruplariToDataGridView();
                            dialog.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ürün grubu güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    btnIptal.Click += (s, args) => dialog.Close();

                    dialog.Controls.AddRange(new Control[] { lblUrunGrubu, txtUrunGrubu, lblUrunGrubuAdi, txtUrunGrubuAdi, btnKaydet, btnIptal });
                    dialog.ShowDialog();
                }
            }
        }
    }
}