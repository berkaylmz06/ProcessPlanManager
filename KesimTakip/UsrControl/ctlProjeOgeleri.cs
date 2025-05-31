using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KesimTakip.DataBase;
using KesimTakip.Helper;
using static KesimTakip.DataBase.AutoCadAktarimData;

namespace KesimTakip.UsrControl
{
    public partial class ctlProjeOgeleri : UserControl
    {
        private DataTable tempTablo;
        private bool isDirty = false;
        private BindingSource bindingSource;
        private int silinenSatirSayisi = 0; // Silme sayacı
        private ContextMenuStrip contextMenuStrip;

        public ctlProjeOgeleri()
        {
            InitializeComponent();
            RoundedPanelHelper.ApplyRoundedBorder(panelList, 10);
            RoundedPanelHelper.ApplyRoundedBorder(panelSearch, 10);
            RoundedPanelHelper.ApplyRoundedBorder(panelHeader, 10);

            panelDisContainer.BackColor = ColorTranslator.FromHtml("#ADD8E6");
            panelDetayContainer.BackColor = ColorTranslator.FromHtml("#ADD8E6");

            panelSearch.BackColor = ColorTranslator.FromHtml("#2C3E50");
            panelHeader.BackColor = ColorTranslator.FromHtml("#2C3E50");

            ButonGenelHelper.KullaniciEkleButonAyari(btnKaydet);
            ButonGenelHelper.KullaniciEkleButonAyari(btnYeni);
            ButonGenelHelper.KullaniciEkleButonAyari(btnStandartProje);

            dataGridOgeDetay.AutoGenerateColumns = false;
            dataGridOgeDetay.AllowUserToAddRows = false;
            dataGridOgeDetay.AllowUserToDeleteRows = true;
            dataGridOgeDetay.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridOgeDetay.CellValueChanged += (s, e) => isDirty = true;
            dataGridOgeDetay.RowsRemoved += (s, e) =>
            {
                isDirty = true;
                silinenSatirSayisi += e.RowCount; // Silinen satır sayısını artır
                lblSilinenSatirSayisi.Text = $"Silinen Satır: {silinenSatirSayisi}";
            };

            bindingSource = new BindingSource();
            dataGridOgeDetay.DataSource = bindingSource;

            // Sağ tık menüsünü ekle
            contextMenuStrip = new ContextMenuStrip();
            var menuItemSil = new ToolStripMenuItem("Satır Sil");
            menuItemSil.Click += (s, e) =>
            {
                if (dataGridOgeDetay.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dataGridOgeDetay.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            dataGridOgeDetay.Rows.Remove(row);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen silmek için bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            contextMenuStrip.Items.Add(menuItemSil);
            dataGridOgeDetay.ContextMenuStrip = contextMenuStrip;

            DataGridViewHelper.StilUygulaProjeOge(dataGridOgeDetay);

            // Silinen satır sayısını gösterecek Label'ı ekle
            lblSilinenSatirSayisi = new Label
            {
                Name = "lblSilinenSatirSayisi",
                Text = "Silinen Satır: 0",
                Location = new Point(20, 50), // Uygun konumu ayarlayın
                AutoSize = true,
                ForeColor = ColorTranslator.FromHtml("#2C3E50"),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panelHeader.Controls.Add(lblSilinenSatirSayisi); // Veya uygun panele ekleyin
        }
        private void ctlProjeOgeleri_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Proje Öğeleri";
            tempTablo = new DataTable();
            tempTablo.RowChanged += (s, args) => isDirty = true;
            tempTablo.RowDeleted += (s, args) => isDirty = true;
        }

        private void InitializeTempTablo(string level)
        {
            tempTablo.Clear();
            tempTablo.Columns.Clear();
            dataGridOgeDetay.Columns.Clear();
            bindingSource.DataSource = null;

            if (level == "Proje")
            {
                tempTablo.Columns.Add("projeAdi", typeof(string));
                tempTablo.Columns.Add("grupAdi", typeof(string));

                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "projeAdi",
                    HeaderText = "Proje",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "grupAdi",
                    HeaderText = "Grup",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
            }
            else if (level == "Grup" || level == "Malzeme")
            {
                tempTablo.Columns.Add("projeAdi", typeof(string));
                tempTablo.Columns.Add("grupAdi", typeof(string));
                tempTablo.Columns.Add("malzemeKod", typeof(string));
                tempTablo.Columns.Add("adet", typeof(int));
                tempTablo.Columns.Add("malzemeAd", typeof(string));
                tempTablo.Columns.Add("kalite", typeof(string));

                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "projeAdi",
                    HeaderText = "Proje",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "grupAdi",
                    HeaderText = "Grup",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "malzemeKod",
                    HeaderText = "Malzeme Kodu",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    ReadOnly = level == "Malzeme"
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "adet",
                    HeaderText = "Adet",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "malzemeAd",
                    HeaderText = "Malzeme Adı",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "kalite",
                    HeaderText = "Kalite",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
            }

            bindingSource.DataSource = tempTablo;
        }

        public void TreeViewYukle(string projeNo)
        {
            var kayitlar = AutoCadAktarimData.GetAutoCadKayitlari(projeNo);
            treeView1.Nodes.Clear();
            var projeDugumleri = new Dictionary<string, TreeNode>();
            var grupDugumleri = new Dictionary<string, TreeNode>();

            foreach (var kayit in kayitlar)
            {
                if (!projeDugumleri.ContainsKey(kayit.Proje))
                {
                    var projeDugumu = new TreeNode(kayit.Proje);
                    treeView1.Nodes.Add(projeDugumu);
                    projeDugumleri[kayit.Proje] = projeDugumu;
                }

                string grupAnahtari = $"{kayit.Proje}_{kayit.Grup}";
                if (!string.IsNullOrEmpty(kayit.Grup) && !grupDugumleri.ContainsKey(grupAnahtari))
                {
                    var grupDugumu = new TreeNode(kayit.Grup);
                    projeDugumleri[kayit.Proje].Nodes.Add(grupDugumu);
                    grupDugumleri[grupAnahtari] = grupDugumu;
                }

                if (!string.IsNullOrEmpty(kayit.MalzemeKod))
                {
                    var malzemeDugumu = new TreeNode(kayit.MalzemeKod);
                    grupDugumleri[grupAnahtari].Nodes.Add(malzemeDugumu);
                }
            }

            treeView1.ExpandAll();
            foreach (TreeNode projeDugumu in treeView1.Nodes)
            {
                foreach (TreeNode grupDugumu in projeDugumu.Nodes)
                {
                    grupDugumu.Collapse();
                }
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string proje = txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(proje))
            {
                MessageBox.Show("Lütfen bir proje numarası girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (isDirty && tempTablo != null && tempTablo.GetChanges() != null)
            {
                DialogResult result = MessageBox.Show(
                    "Değişiklikler kaydedilmedi. Kaydetmek istiyor musunuz?",
                    "Uyarı",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    btnKaydet_Click(this, new EventArgs());
                    if (isDirty) return;
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            try
            {
                var kayitlar = AutoCadAktarimData.GetAutoCadKayitlari(proje);
                if (kayitlar.Count == 0)
                {
                    DialogResult result = MessageBox.Show(
                        $"Proje '{proje}' bulunamadı. Yeni proje oluşturulsun mu?",
                        "Proje Bulunamadı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        AutoCadAktarimData.ProjeEkle(proje);
                    }
                    else
                    {
                        treeView1.Nodes.Clear();
                        dataGridOgeDetay.DataSource = null;
                        bindingSource.DataSource = null;
                        tempTablo.Clear();
                        return;
                    }
                }
                TreeViewYukle(proje);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama hatası: {ex.Message}\nDetay: {ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (isDirty && tempTablo != null && tempTablo.GetChanges() != null)
            {
                DialogResult result = MessageBox.Show(
                    "Değişiklikler kaydedilmedi. Kaydetmek istiyor musunuz?",
                    "Uyarı",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    btnKaydet_Click(this, new EventArgs());
                    if (isDirty) return;
                }
                else if (result == DialogResult.Cancel)
                {
                    treeView1.SelectedNode = treeView1.SelectedNode;
                    return;
                }
            }

            try
            {
                tempTablo = new DataTable();
                tempTablo.RowChanged += (s, args) => isDirty = true;
                tempTablo.RowDeleted += (s, args) => isDirty = true;

                if (e.Node.Parent == null) // Proje seçildi
                {
                    string projeNo = e.Node.Text;
                    InitializeTempTablo("Proje");
                    var gruplar = AutoCadAktarimData.GruplariGetir(projeNo);
                    foreach (DataRow row in gruplar.Rows)
                    {
                        tempTablo.Rows.Add(row["projeAdi"], row["grupAdi"]);
                    }
                }
                else if (e.Node.Parent != null && e.Node.Parent.Parent == null) // Grup seçildi
                {
                    string projeNo = e.Node.Parent.Text;
                    string grup = e.Node.Text;
                    InitializeTempTablo("Grup");
                    var malzemeler = AutoCadAktarimData.MalzemeleriGetir(projeNo, grup);
                    foreach (DataRow row in malzemeler.Rows)
                    {
                        tempTablo.Rows.Add(
                            row["projeAdi"],
                            row["grupAdi"],
                            row["malzemeKod"],
                            row["adet"],
                            row["malzemeAd"],
                            row["kalite"]);
                    }
                }
                else if (e.Node.Parent != null && e.Node.Parent.Parent != null) // Malzeme seçildi
                {
                    string projeNo = e.Node.Parent.Parent.Text;
                    string grup = e.Node.Parent.Text;
                    string malzemeKod = e.Node.Text;
                    InitializeTempTablo("Malzeme");
                    var detaylar = AutoCadAktarimData.MalzemeDetaylariniGetir(projeNo, grup, malzemeKod);
                    foreach (DataRow row in detaylar.Rows)
                    {
                        tempTablo.Rows.Add(
                            row["projeAdi"],
                            row["grupAdi"],
                            row["malzemeKod"],
                            row["adet"],
                            row["malzemeAd"],
                            row["kalite"]);
                    }
                }

                DataGridViewHelper.StilUygulaProjeOge(dataGridOgeDetay);
                isDirty = false;
                silinenSatirSayisi = 0; // Yeni bir düğüm seçildiğinde sayacı sıfırla
                lblSilinenSatirSayisi.Text = "Silinen Satır: 0";

                // Proje seviyesinde satır silmeyi kontrol et
                dataGridOgeDetay.AllowUserToDeleteRows = true; // Her zaman silmeye izin ver
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}\nDetay: {ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
     
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Lütfen bir düğüm seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TreeNode seciliDugum = treeView1.SelectedNode;
                string projeNo = seciliDugum.Parent == null ? seciliDugum.Text :
                                seciliDugum.Parent.Parent == null ? seciliDugum.Parent.Text :
                                seciliDugum.Parent.Parent.Text;

                Console.WriteLine($"btnKaydet_Click başladı: projeNo={projeNo}, seçiliDugum={seciliDugum.Text}");

                // Zorunlu alan doğrulama
                foreach (DataRow row in tempTablo.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    if (seciliDugum.Parent == null) // Proje seviyesi
                    {
                        string grupAdi = row["grupAdi"]?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(grupAdi))
                        {
                            MessageBox.Show("Grup adı boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else // Grup veya Malzeme seviyesi
                    {
                        string malzemeKod = row["malzemeKod"]?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(malzemeKod))
                        {
                            MessageBox.Show("Malzeme kodu boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (row["adet"] == DBNull.Value || !int.TryParse(row["adet"]?.ToString(), out int adet) || adet <= 0)
                        {
                            MessageBox.Show("Adet sıfırdan büyük bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                if (seciliDugum.Parent == null) // Proje seçili
                {
                    var processedGrupAdi = new HashSet<string>();
                    foreach (DataRow row in tempTablo.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;
                        string grupAdi = row["grupAdi"]?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(grupAdi) || processedGrupAdi.Contains(grupAdi)) continue;
                        processedGrupAdi.Add(grupAdi);

                        string eskiGrupAdi = row.RowState == DataRowState.Modified ? row["grupAdi", DataRowVersion.Original]?.ToString()?.Trim() : null;
                        Console.WriteLine($"GrupEkleGuncelle çağrılıyor: grupAdi={grupAdi}, eskiGrupAdi={eskiGrupAdi}");
                        AutoCadAktarimData.GrupEkleGuncelle(projeNo, grupAdi, eskiGrupAdi);
                    }

                    // Silinen grupları işle
                    foreach (DataRow row in tempTablo.AsEnumerable().Where(r => r.RowState == DataRowState.Deleted).ToList())
                    {
                        string grupAdi = row["grupAdi", DataRowVersion.Original]?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(grupAdi))
                        {
                            continue; // Geçersiz grupAdi varsa atla
                        }
                        Console.WriteLine($"GrupSil çağrılıyor: grupAdi={grupAdi}");
                        AutoCadAktarimData.GrupSil(projeNo, grupAdi);
                    }
                }
                else // Grup veya malzeme seçili
                {
                    DataTable uniqueTempTablo = tempTablo.Clone();
                    var groupedRows = tempTablo.AsEnumerable()
                        .GroupBy(row => new
                        {
                            GrupAdi = row["grupAdi"]?.ToString()?.Trim(),
                            MalzemeKod = row.Field<string>("malzemeKod")?.Trim()
                        })
                        .Select(g =>
                        {
                            var row = uniqueTempTablo.NewRow();
                            row.ItemArray = g.First().ItemArray;
                            if (g.Key.MalzemeKod != null)
                            {
                                int totalAdet = g.Sum(r => r["adet"] != DBNull.Value && int.TryParse(r["adet"]?.ToString(), out int adet) ? adet : 0);
                                row["adet"] = totalAdet;
                            }
                            return row;
                        });

                    foreach (DataRow row in groupedRows)
                    {
                        uniqueTempTablo.Rows.Add(row);
                    }

                    string grup = seciliDugum.Parent != null && seciliDugum.Parent.Parent != null ? seciliDugum.Parent.Text : seciliDugum.Text;
                    var processedMalzemeKod = new HashSet<string>();
                    foreach (DataRow row in uniqueTempTablo.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;
                        string malzemeKod = row["malzemeKod"]?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(malzemeKod) || processedMalzemeKod.Contains(malzemeKod)) continue;
                        processedMalzemeKod.Add(malzemeKod);

                        int adet = row["adet"] != DBNull.Value && int.TryParse(row["adet"]?.ToString(), out int parsedAdet) ? parsedAdet : 0;
                        string malzemeAd = row["malzemeAd"]?.ToString()?.Trim();
                        string kalite = row["kalite"]?.ToString()?.Trim();
                        Console.WriteLine($"MalzemeEkleGuncelle çağrılıyor: malzemeKod={malzemeKod}, adet={adet}");
                        AutoCadAktarimData.MalzemeEkleGuncelle(projeNo, grup, malzemeKod, adet, malzemeAd, kalite);
                    }

                    // Silinen malzemeleri işle
                    foreach (DataRow row in tempTablo.AsEnumerable().Where(r => r.RowState == DataRowState.Deleted).ToList())
                    {
                        string malzemeKod = row["malzemeKod", DataRowVersion.Original]?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(malzemeKod))
                        {
                            continue; // Geçersiz malzemeKod varsa atla
                        }
                        Console.WriteLine($"MalzemeSil çağrılıyor: malzemeKod={malzemeKod}");
                        AutoCadAktarimData.MalzemeSil(projeNo, grup, malzemeKod);
                    }
                }

                tempTablo.AcceptChanges();
                TreeViewYukle(projeNo);
                MessageBox.Show("Veriler başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isDirty = false;
                silinenSatirSayisi = 0; // Kaydetme başarılıysa sayacı sıfırla
                lblSilinenSatirSayisi.Text = "Silinen Satır: 0";
                Console.WriteLine("btnKaydet_Click tamamlandı");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"btnKaydet_Click hatası: {ex.Message}");
                MessageBox.Show($"Hata oluştu: {ex.Message}\nDetay: {ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isDirty = true;
            }
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Lütfen bir düğüm seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent != null)
            {
                MessageBox.Show("Malzeme seviyesinde yeni kayıt eklenemez, sadece güncelleme yapılabilir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataRow yeniSatir = tempTablo.NewRow();
                if (treeView1.SelectedNode.Parent == null) // Proje seçili
                {
                    yeniSatir["projeAdi"] = treeView1.SelectedNode.Text;
                    yeniSatir["grupAdi"] = string.Empty;
                }
                else if (treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent == null) // Grup seçili
                {
                    yeniSatir["projeAdi"] = treeView1.SelectedNode.Parent.Text;
                    yeniSatir["grupAdi"] = treeView1.SelectedNode.Text;
                    yeniSatir["malzemeKod"] = string.Empty;
                    yeniSatir["adet"] = DBNull.Value;
                    yeniSatir["malzemeAd"] = string.Empty;
                    yeniSatir["kalite"] = string.Empty;
                }
                tempTablo.Rows.Add(yeniSatir);
                isDirty = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yeni satır ekleme hatası: {ex.Message}\nDetay: {ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStandartProje_Click(object sender, EventArgs e)
        {
            string proje = txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(proje))
            {
                MessageBox.Show("Lütfen bir proje numarası girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (isDirty && tempTablo != null && tempTablo.GetChanges() != null)
            {
                DialogResult result = MessageBox.Show(
                    "Değişiklikler kaydedilmedi. Kaydetmek istiyor musunuz?",
                    "Uyarı",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    btnKaydet_Click(this, new EventArgs());
                    if (isDirty) return;
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            var kayitlar = AutoCadAktarimData.GetAutoCadKayitlari(proje);
            if (kayitlar.Count == 0)
            {
                AutoCadAktarimData.ProjeEkle(proje);
            }
            TreeViewYukle(proje);
        }

        public bool OnUserControlExit()
        {
            if (isDirty && tempTablo != null && tempTablo.GetChanges() != null)
            {
                DialogResult result = MessageBox.Show(
                    "Değişiklikler kaydedilmedi. Kaydetmek istiyor musunuz?",
                    "Uyarı",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    btnKaydet_Click(this, new EventArgs());
                    return !isDirty;
                }
                else if (result == DialogResult.No)
                {
                    isDirty = false;
                    tempTablo.AcceptChanges();
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}