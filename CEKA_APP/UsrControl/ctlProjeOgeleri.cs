using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CEKA_APP.DataBase;
using CEKA_APP.Helper;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeOgeleri : UserControl
    {
        private DataTable tempTablo;
        private bool isDirty = false;
        private BindingSource bindingSource;
        private int silinenSatirSayisi = 0;
        private ContextMenuStrip contextMenuStrip;

        public class SilinenSatirBilgisi
        {
            public string GrupAdi { get; set; }
            public string MalzemeKod { get; set; }
        }

        private List<SilinenSatirBilgisi> silinenSatirlar = new List<SilinenSatirBilgisi>();

        public ctlProjeOgeleri()
        {
            InitializeComponent();
            DataGridOlaylariniAyarla();
            ContextMenuAyarla();
            DataGridDogrulamaAyarla();

            DataGridViewHelper.StilUygulaProjeOge(dataGridOgeDetay);

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
        }

        private void DataGridOlaylariniAyarla()
        {
            bindingSource = new BindingSource();
            dataGridOgeDetay.DataSource = bindingSource;

            dataGridOgeDetay.CellValueChanged += (s, e) =>
            {
                if (bindingSource != null && tempTablo != null)
                {
                    isDirty = true;
                    UpdateSaveButtonState();
                }
            };
        }

        private void ContextMenuAyarla()
        {
            contextMenuStrip = new ContextMenuStrip();
            var menuItemSil = new ToolStripMenuItem("Satır Sil");
            contextMenuStrip.Items.Add(menuItemSil);
            dataGridOgeDetay.ContextMenuStrip = contextMenuStrip;

            contextMenuStrip.Opening += (s, e) =>
            {
                if (dataGridOgeDetay == null || bindingSource == null || bindingSource.Count == 0)
                {
                    e.Cancel = true;
                    return;
                }

                Point mousePosition = dataGridOgeDetay.PointToClient(Control.MousePosition);
                DataGridView.HitTestInfo hit = dataGridOgeDetay.HitTest(mousePosition.X, mousePosition.Y);

                if (hit.Type == DataGridViewHitTestType.Cell && hit.RowIndex >= 0)
                {
                    dataGridOgeDetay.ClearSelection();
                    dataGridOgeDetay.Rows[hit.RowIndex].Selected = true;
                }
                else
                {
                    e.Cancel = true;
                }
            };

            menuItemSil.Click += (s, e) =>
            {
                if (dataGridOgeDetay.SelectedRows.Count > 0 && bindingSource != null)
                {
                    foreach (DataGridViewRow row in dataGridOgeDetay.SelectedRows)
                    {
                        if (!row.IsNewRow && row.DataBoundItem != null)
                        {
                            var dataRow = ((DataRowView)row.DataBoundItem).Row;
                            string grupAdi = dataRow["grupAdi"]?.ToString()?.Trim();
                            string malzemeKod = dataRow.Table.Columns.Contains("malzemeKod") ? dataRow["malzemeKod"]?.ToString()?.Trim() : null;

                            silinenSatirlar.Add(new SilinenSatirBilgisi
                            {
                                GrupAdi = grupAdi,
                                MalzemeKod = malzemeKod
                            });

                            if (dataRow.RowState == DataRowState.Added)
                            {
                                if (tempTablo != null)
                                {
                                    tempTablo.Rows.Remove(dataRow);
                                }
                            }
                            else
                            {
                                dataRow.Delete();
                            }

                            isDirty = true;
                            silinenSatirSayisi++;
                        }
                    }
                    if (bindingSource != null)
                    {
                        bindingSource.ResetBindings(false);
                    }
                    UpdateSaveButtonState();
                }
                else
                {
                    MessageBox.Show("Lütfen silmek için bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
        }

        private void DataGridDogrulamaAyarla()
        {
            dataGridOgeDetay.CellValidating += (s, e) =>
            {
                string columnName = dataGridOgeDetay.Columns[e.ColumnIndex].DataPropertyName;
                string input = e.FormattedValue?.ToString()?.Trim();
                int currentRowIndex = e.RowIndex;

                if (columnName == "adet")
                {
                    if (!string.IsNullOrEmpty(input) && (!int.TryParse(input, out int adet) || adet <= 0))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Adet alanı yalnızca pozitif bir tam sayı olabilir.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (columnName == "grupAdi" || columnName == "malzemeKod")
                {
                    if (string.IsNullOrEmpty(input))
                    {
                        e.Cancel = true;
                        MessageBox.Show($"{(columnName == "grupAdi" ? "Grup adı" : "Malzeme kodu")} boş olamaz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (treeView1.SelectedNode != null)
                    {
                        string projeNo = treeView1.SelectedNode.Parent == null ? treeView1.SelectedNode.Text :
                                        treeView1.SelectedNode.Parent.Parent == null ? treeView1.SelectedNode.Parent.Text :
                                        treeView1.SelectedNode.Parent.Parent.Text;
                        string grupAdi = columnName == "malzemeKod" ? (treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent != null
                                        ? treeView1.SelectedNode.Parent.Text : treeView1.SelectedNode.Text) : null;

                        if (columnName == "grupAdi")
                        {
                            var gruplar = AutoCadAktarimData.GruplariGetir(projeNo);
                            if (gruplar.AsEnumerable().Any(r => r.Field<string>("grupAdi")?.Trim() == input &&
                                (currentRowIndex < 0 || tempTablo.Rows[currentRowIndex]["grupAdi"]?.ToString()?.Trim() != input)))
                            {
                                e.Cancel = true;
                                MessageBox.Show("Bu grup adı zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else if (columnName == "malzemeKod")
                        {
                            var malzemeler = AutoCadAktarimData.MalzemeleriGetir(projeNo, grupAdi);
                            if (malzemeler.AsEnumerable().Any(r => r.Field<string>("malzemeKod")?.Trim() == input &&
                                (currentRowIndex < 0 || tempTablo.Rows[currentRowIndex]["malzemeKod"]?.ToString()?.Trim() != input)))
                            {
                                e.Cancel = true;
                                MessageBox.Show("Bu malzeme kodu zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            };
        }
        private void ctlProjeOgeleri_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Proje Öğeleri";
            tempTablo = new DataTable();
            tempTablo.RowChanged += (s, args) => { isDirty = true; UpdateSaveButtonState(); };
            tempTablo.RowDeleted += (s, args) => { isDirty = true; UpdateSaveButtonState(); };
            UpdateSaveButtonState();
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

            var sutunlar = dataGridOgeDetay.Columns.Cast<DataGridViewColumn>().Select(c => c.DataPropertyName).ToList();
        }
        //private void InitializeTempTablo(string level)
        //{
        //    tempTablo.Clear();
        //    tempTablo.Columns.Clear();
        //    dataGridOgeDetay.Columns.Clear();
        //    bindingSource.DataSource = null;

        //    if (level == "Proje")
        //    {
        //        tempTablo.Columns.Add("projeAdi", typeof(string));
        //        tempTablo.Columns.Add("grupAdi", typeof(string));

        //        dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "projeAdi",
        //            HeaderText = "Proje",
        //            ReadOnly = true,
        //            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        //        });
        //        dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "grupAdi",
        //            HeaderText = "Grup",
        //            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        //        });
        //    }
        //    else if (level == "Grup" || level == "Malzeme")
        //    {
        //        tempTablo.Columns.Add("projeAdi", typeof(string));
        //        tempTablo.Columns.Add("grupAdi", typeof(string));
        //        tempTablo.Columns.Add("malzemeKod", typeof(string));
        //        tempTablo.Columns.Add("adet", typeof(int));
        //        tempTablo.Columns.Add("malzemeAd", typeof(string));
        //        tempTablo.Columns.Add("kalite", typeof(string));

        //        dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "projeAdi",
        //            HeaderText = "Proje",
        //            ReadOnly = true,
        //            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        //        });
        //        dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "grupAdi",
        //            HeaderText = "Grup",
        //            ReadOnly = true,
        //            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        //        });
        //        dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "malzemeKod",
        //            HeaderText = "Malzeme Kodu",
        //            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        //            ReadOnly = level == "Malzeme"
        //        });
        //        dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "adet",
        //            HeaderText = "Adet",
        //            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        //        });
        //        dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "malzemeAd",
        //            HeaderText = "Malzeme Adı",
        //            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        //        });
        //        dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "kalite",
        //            HeaderText = "Kalite",
        //            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        //        });
        //    }

        //    bindingSource.DataSource = tempTablo;
        //}

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

            if (treeView1.Nodes.Count > 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
            }
        }

        private bool ValidateTableData(TreeNode selectedNode)
        {
            if (tempTablo == null || tempTablo.Rows.Count == 0) return true;

            bool isProjectLevel = selectedNode?.Parent == null;
            bool isMaterialLevel = selectedNode?.Parent != null && selectedNode.Parent.Parent != null;

            foreach (DataRow row in tempTablo.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted))
            {
                if (isProjectLevel)
                {
                    string grupAdi = row["grupAdi"]?.ToString()?.Trim();
                    if (string.IsNullOrEmpty(grupAdi))
                    {
                        MessageBox.Show("Grup adı boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    if (tempTablo.Columns.Contains("malzemeKod"))
                    {
                        string malzemeKod = row["malzemeKod"]?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(malzemeKod))
                        {
                            MessageBox.Show("Malzeme kodu boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        if (tempTablo.Columns.Contains("adet"))
                        {
                            if (row["adet"] == DBNull.Value || !int.TryParse(row["adet"]?.ToString(), out int adet) || adet <= 0)
                            {
                                MessageBox.Show("Adet sıfırdan büyük bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool HandleUnsavedChanges()
        {
            if (!isDirty || tempTablo == null || tempTablo.GetChanges() == null)
                return true;

            if (treeView1.SelectedNode != null && !ValidateTableData(treeView1.SelectedNode))
                return false;

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
            else if (result == DialogResult.Cancel)
            {
                return false;
            }
            else
            {
                tempTablo.RejectChanges();
                isDirty = false;
                silinenSatirlar.Clear();
                silinenSatirSayisi = 0;
                bindingSource.ResetBindings(false);
                UpdateSaveButtonState();
                return true;
            }
        }

        private void UpdateSaveButtonState()
        {
            btnKaydet.Enabled = isDirty && tempTablo != null && tempTablo.GetChanges() != null;
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string proje = txtProjeNo.Text.Trim();
            if (string.IsNullOrEmpty(proje))
            {
                MessageBox.Show("Lütfen bir proje numarası girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!HandleUnsavedChanges())
                return;

            try
            {
                var kayitlar = AutoCadAktarimData.GetAutoCadKayitlari(proje);
                if (kayitlar.Count == 0)
                {
                    MessageBox.Show(
                        $"'{proje}' adlı proje bulunamadı.\nİşlem iptal edildi.",
                        "Bilgi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    treeView1.Nodes.Clear();
                    return;
                }

                TreeViewYukle(proje);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama hatası: {ex.Message}\nDetay: {ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TreeNode _pendingNode = null;

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            dataGridOgeDetay.ClearSelection();

            if (isDirty && tempTablo != null && tempTablo.GetChanges() != null)
            {
                if (treeView1.SelectedNode != null && !ValidateTableData(treeView1.SelectedNode))
                {
                    e.Cancel = true;
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Değişiklikler kaydedilmedi. Kaydetmek istiyor musunuz?",
                    "Uyarı",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    btnKaydet_Click(this, new EventArgs());
                    if (!isDirty)
                    {
                        _pendingNode = e.Node;
                    }
                    e.Cancel = true;
                }
                else if (result == DialogResult.No)
                {
                    if (tempTablo != null)
                    {
                        tempTablo.RejectChanges();
                        isDirty = false;
                        silinenSatirlar.Clear();
                        silinenSatirSayisi = 0;
                        bindingSource.ResetBindings(false);
                        UpdateSaveButtonState();
                    }
                    _pendingNode = e.Node;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                _pendingNode = e.Node;
            }
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_pendingNode == null || _pendingNode != e.Node)
                return;

            try
            {
                dataGridOgeDetay.ClearSelection();

                if (bindingSource != null)
                {
                    bindingSource.DataSource = null;
                }

                tempTablo = new DataTable();
                tempTablo.RowChanged += (s, args) => { isDirty = true; UpdateSaveButtonState(); };
                tempTablo.RowDeleted += (s, args) => { isDirty = true; UpdateSaveButtonState(); };

                if (e.Node == null) return;

                if (e.Node.Parent == null)
                {
                    string projeNo = e.Node.Text;
                    InitializeTempTablo("Proje");
                    var gruplar = AutoCadAktarimData.GruplariGetir(projeNo);
                    if (gruplar != null)
                    {
                        foreach (DataRow row in gruplar.Rows)
                        {
                            tempTablo.Rows.Add(row["projeAdi"], row["grupAdi"]);
                        }
                    }
                }
                else if (e.Node.Parent != null && e.Node.Parent.Parent == null)
                {
                    string projeNo = e.Node.Parent.Text;
                    string grup = e.Node.Text;
                    InitializeTempTablo("Grup");
                    var malzemeler = AutoCadAktarimData.MalzemeleriGetir(projeNo, grup);
                    if (malzemeler != null)
                    {
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
                }
                else if (e.Node.Parent != null && e.Node.Parent.Parent != null)
                {
                    string projeNo = e.Node.Parent.Parent.Text;
                    string grup = e.Node.Parent.Text;
                    string malzemeKod = e.Node.Text;
                    InitializeTempTablo("Malzeme");
                    var detaylar = AutoCadAktarimData.MalzemeDetaylariniGetir(projeNo, grup, malzemeKod);
                    if (detaylar != null)
                    {
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
                }

                if (tempTablo != null)
                {
                    tempTablo.AcceptChanges();
                    silinenSatirlar.Clear();
                    isDirty = false;
                    silinenSatirSayisi = 0;
                    dataGridOgeDetay.AllowUserToDeleteRows = true;
                    dataGridOgeDetay.ClearSelection();
                    bindingSource.DataSource = tempTablo;
                    dataGridOgeDetay.DataSource = bindingSource;
                    dataGridOgeDetay.AutoGenerateColumns = false;
                    Application.DoEvents();
                    var sutunlar = dataGridOgeDetay.Columns.Cast<DataGridViewColumn>().Select(c => c.DataPropertyName).ToList();
                    if (sutunlar.Contains("projeAdi") && sutunlar.Contains("grupAdi") &&
                        sutunlar.Contains("malzemeKod") && sutunlar.Contains("adet") &&
                        sutunlar.Contains("malzemeAd") && sutunlar.Contains("kalite"))
                    {
                        ColorRows();
                    }
                    UpdateSaveButtonState();
                }
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

            if (!ValidateTableData(treeView1.SelectedNode))
                return;

            bool saveSuccess = true;
            try
            {
                TreeNode seciliDugum = treeView1.SelectedNode;
                string projeNo = seciliDugum.Parent == null ? seciliDugum.Text :
                                seciliDugum.Parent.Parent == null ? seciliDugum.Parent.Text :
                                seciliDugum.Parent.Parent.Text;
                string seciliYol = seciliDugum.FullPath;

                // Silinen satırları işle
                foreach (var silinen in silinenSatirlar)
                {
                    if (seciliDugum.Parent == null && !string.IsNullOrEmpty(silinen.GrupAdi))
                    {
                        AutoCadAktarimData.GrupSil(projeNo, silinen.GrupAdi);
                    }
                    else if (!string.IsNullOrEmpty(silinen.MalzemeKod))
                    {
                        string grup = seciliDugum.Parent != null && seciliDugum.Parent.Parent != null ? seciliDugum.Parent.Text : seciliDugum.Text;
                        AutoCadAktarimData.MalzemeSil(projeNo, grup, silinen.MalzemeKod);
                    }
                }

                // Değiştirilen ve eklenen satırları işle
                if (seciliDugum.Parent == null)
                {
                    foreach (DataRow row in tempTablo.Rows.Cast<DataRow>()
                        .Where(r => r.RowState == DataRowState.Added || r.RowState == DataRowState.Modified).ToList())
                    {
                        string grupAdi = row["grupAdi"]?.ToString()?.Trim();
                        string eskiGrupAdi = row.RowState == DataRowState.Modified ? row["grupAdi", DataRowVersion.Original]?.ToString()?.Trim() : null;

                        if (!string.IsNullOrEmpty(grupAdi))
                        {
                            AutoCadAktarimData.GrupEkleGuncelle(projeNo, grupAdi, eskiGrupAdi);
                            var gruplar = AutoCadAktarimData.GruplariGetir(projeNo);
                            if (!gruplar.AsEnumerable().Any(r => r.Field<string>("grupAdi")?.Trim() == grupAdi))
                            {
                                saveSuccess = false;
                            }
                        }
                        else
                        {
                            saveSuccess = false;
                        }
                    }
                }
                else
                {
                    string grup = seciliDugum.Parent != null && seciliDugum.Parent.Parent != null ? seciliDugum.Parent.Text : seciliDugum.Text;
                    foreach (DataRow row in tempTablo.Rows.Cast<DataRow>()
                        .Where(r => r.RowState == DataRowState.Added || r.RowState == DataRowState.Modified).ToList())
                    {
                        if (tempTablo.Columns.Contains("malzemeKod"))
                        {
                            string malzemeKod = row["malzemeKod"]?.ToString()?.Trim();
                            string eskiMalzemeKod = row.RowState == DataRowState.Modified ? row["malzemeKod", DataRowVersion.Original]?.ToString()?.Trim() : null;

                            if (!string.IsNullOrEmpty(malzemeKod) && row["adet"] != DBNull.Value && int.TryParse(row["adet"]?.ToString(), out int adet) && adet > 0)
                            {
                                string malzemeAd = row["malzemeAd"]?.ToString()?.Trim();
                                string kalite = row["kalite"]?.ToString()?.Trim();

                                AutoCadAktarimData.MalzemeEkleGuncelle(projeNo, grup, malzemeKod, adet, malzemeAd, kalite, eskiMalzemeKod);
                                var malzemeler = AutoCadAktarimData.MalzemeleriGetir(projeNo, grup);
                                if (!malzemeler.AsEnumerable().Any(r => r.Field<string>("malzemeKod")?.Trim() == malzemeKod))
                                {
                                    saveSuccess = false;
                                }
                            }
                            else
                            {
                                saveSuccess = false;
                            }
                        }
                    }
                }

                // Tabloyu güncelle
                string seviye = seciliDugum.Parent == null ? "Proje" :
                                seciliDugum.Parent.Parent == null ? "Grup" : "Malzeme";
                InitializeTempTablo(seviye);
                if (seviye == "Proje")
                {
                    var gruplar = AutoCadAktarimData.GruplariGetir(projeNo);
                    foreach (DataRow row in gruplar.Rows)
                    {
                        tempTablo.Rows.Add(row["projeAdi"], row["grupAdi"]);
                    }
                }
                else if (seviye == "Grup")
                {
                    var malzemeler = AutoCadAktarimData.MalzemeleriGetir(projeNo, seciliDugum.Text);
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
                else if (seviye == "Malzeme")
                {
                    var detaylar = AutoCadAktarimData.MalzemeDetaylariniGetir(projeNo, seciliDugum.Parent.Text, seciliDugum.Text);
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

                tempTablo.AcceptChanges();
                silinenSatirlar.Clear();
                silinenSatirSayisi = 0;
                TreeViewYukle(projeNo);

                TreeNode[] dugumler = treeView1.Nodes.FindByFullPath(seciliYol);
                if (dugumler.Length > 0)
                {
                    treeView1.SelectedNode = dugumler[0];
                    treeView1_AfterSelect(this, new TreeViewEventArgs(dugumler[0]));
                }
                else
                {
                    treeView1.SelectedNode = treeView1.Nodes[0];
                    treeView1_AfterSelect(this, new TreeViewEventArgs(treeView1.Nodes[0]));
                }

                if (saveSuccess)
                {
                    MessageBox.Show("Veriler başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isDirty = false;
                    ColorRows();
                }
                else
                {
                    MessageBox.Show("Veriler kaydedilmedi. Lütfen zorunlu alanları kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isDirty = true;
                }
                UpdateSaveButtonState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştuğu: {ex.Message}\nDetay: {ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isDirty = true;
                UpdateSaveButtonState();
            }
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Lütfen bir düğüm seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!HandleUnsavedChanges())
                return;

            if (treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent != null)
            {
                MessageBox.Show("Malzeme seviyesinde yeni kayıt eklenemez, sadece güncelleme yapılabilir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataRow yeniSatir = tempTablo.NewRow();
                if (treeView1.SelectedNode.Parent == null)
                {
                    yeniSatir["projeAdi"] = treeView1.SelectedNode.Text;
                    yeniSatir["grupAdi"] = string.Empty;
                }
                else if (treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent == null)
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
                UpdateSaveButtonState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yeni satır ekleme hatası: {ex.Message}\nDetay: {ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStandartProje_Click(object sender, EventArgs e)
        {
            frmStandartProjeler standartProjeler = new frmStandartProjeler();
            standartProjeler.ShowDialog();
        }

        public bool OnUserControlExit()
        {
            if (isDirty && tempTablo != null && tempTablo.GetChanges() != null)
            {
                if (!ValidateTableData(treeView1.SelectedNode))
                    return false;

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
                    UpdateSaveButtonState();
                    return true;
                }
                return false;
            }
            return true;
        }



        private void ColorRows()
        {
            if (treeView1.SelectedNode == null)
            {
                return;
            }

            string seviye = treeView1.SelectedNode.Parent == null ? "Proje" :
                            treeView1.SelectedNode.Parent.Parent == null ? "Grup" : "Malzeme";

            if (seviye != "Grup" && seviye != "Malzeme")
            {
                return;
            }

            var kesimDetaylari = KesimDetaylariData.GetKesimDetaylariBilgileri();
            var mevcutSutunlar = dataGridOgeDetay.Columns.Cast<DataGridViewColumn>().Select(c => c.DataPropertyName).ToList();
            var sutunAdlari = dataGridOgeDetay.Columns.Cast<DataGridViewColumn>()
                .ToDictionary(c => c.DataPropertyName.ToLowerInvariant(), c => c.Index, StringComparer.OrdinalIgnoreCase);
            var eksikSutunlar = new List<string>();

            string[] gerekliSutunlar = { "projeadi", "grupadi", "malzemekod", "adet", "malzemead", "kalite" };
            foreach (var sutun in gerekliSutunlar)
            {
                if (!sutunAdlari.ContainsKey(sutun))
                {
                    eksikSutunlar.Add(sutun);
                }
            }
            if (eksikSutunlar.Any())
            {
                MessageBox.Show($"DataGridView sütunları eksik: {string.Join(", ", eksikSutunlar)}. Lütfen destek ekibine başvurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int projeAdiIndex = sutunAdlari["projeadi"];
            int grupAdiIndex = sutunAdlari["grupadi"];
            int malzemeKodIndex = sutunAdlari["malzemekod"];
            int adetIndex = sutunAdlari["adet"];
            int malzemeAdIndex = sutunAdlari["malzemead"];
            int kaliteIndex = sutunAdlari["kalite"];

            foreach (DataGridViewRow row in dataGridOgeDetay.Rows)
            {
                if (!row.IsNewRow)
                {
                    try
                    {
                        string projeAdi = row.Cells[projeAdiIndex].Value?.ToString()?.Trim();
                        string grupAdi = row.Cells[grupAdiIndex].Value?.ToString()?.Trim();
                        string malzemeKod = row.Cells[malzemeKodIndex].Value?.ToString()?.Trim();
                        string adet = row.Cells[adetIndex].Value?.ToString()?.Trim();
                        string malzemeAd = row.Cells[malzemeAdIndex].Value?.ToString()?.Trim();
                        string kalite = row.Cells[kaliteIndex].Value?.ToString()?.Trim();

                        if (string.IsNullOrEmpty(projeAdi) || string.IsNullOrEmpty(grupAdi) ||
                            string.IsNullOrEmpty(malzemeKod) || string.IsNullOrEmpty(adet) ||
                            string.IsNullOrEmpty(malzemeAd) || string.IsNullOrEmpty(kalite))
                        {
                            row.DefaultCellStyle.BackColor = Color.Empty;
                            continue;
                        }

                        string key = $"{kalite}_{malzemeAd}_{malzemeKod}_{projeAdi}";
                        var kesimDetayi = kesimDetaylari.FirstOrDefault(k => k.Key == key);

                        if (kesimDetayi != null)
                        {
                            if (kesimDetayi.kesilmisAdet == kesimDetayi.toplamAdet && kesimDetayi.toplamAdet > 0)
                            {
                                row.DefaultCellStyle.BackColor = Color.Red;
                            }
                            else if (kesimDetayi.kesilmisAdet > 0)
                            {
                                row.DefaultCellStyle.BackColor = Color.Orange;
                            }
                            else
                            {
                                row.DefaultCellStyle.BackColor = Color.Green;
                            }
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ColorRows: Satır {row.Index} için hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
