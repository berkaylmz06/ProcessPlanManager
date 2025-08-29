// ctlProjeOgeleri.cs
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeOgeleri : UserControl
    {
        private DataTable tempTablo;
        private bool isDirty = false;
        private BindingSource bindingSource;
        private int silinenSatirSayisi = 0;
        private ContextMenuStrip contextMenuStrip;
        private List<AutoCadAktarim> silinenSatirlar = new List<AutoCadAktarim>();
        private TreeNode previousHighlightedNode = null;
        public ctlProjeOgeleri()
        {
            InitializeComponent();
            DataGridOlaylariniAyarla();
            ContextMenuAyarla();
            DataGridDogrulamaAyarla();

            DataGridViewHelper.StilUygulaProjeOge(dataGridOgeDetay);

            RoundedPanelHelper.ApplyRoundedBorder(panelList, 10);
            RoundedPanelHelper.ApplyRoundedBorder(panelSearch, 10);

            panelSearch.BackColor = ColorTranslator.FromHtml("#2C3E50");

            ButonGenelHelper.KullaniciEkleButonAyari(btnKaydet);
            ButonGenelHelper.KullaniciEkleButonAyari(btnYeni);
            ButonGenelHelper.KullaniciEkleButonAyari(btnStandartProje);
        }
        private void HighlightTreeViewNode(string projeNo, string grupAdi, Guid? yuklemeId)
        {
            if (string.IsNullOrEmpty(projeNo) || string.IsNullOrEmpty(grupAdi) || !yuklemeId.HasValue)
            {
                return;
            }


            if (previousHighlightedNode != null)
            {
                previousHighlightedNode.BackColor = Color.Empty;
                previousHighlightedNode.ForeColor = Color.Empty;
                previousHighlightedNode = null;
            }

            TreeNode targetNode = null;
            foreach (TreeNode projeNode in treeView1.Nodes)
            {
                if (projeNode.Text != projeNo)
                    continue;

                foreach (TreeNode ustGrupNode in projeNode.Nodes)
                {
                    var tagInfo = ustGrupNode.Tag as Tuple<string, Guid?>;
                    if (tagInfo == null)
                        continue;

                    if (tagInfo.Item1 == grupAdi && tagInfo.Item2 == yuklemeId)
                    {
                        targetNode = ustGrupNode;
                        break;
                    }
                }

                if (targetNode != null)
                    break;
            }

            if (targetNode != null)
            {
                targetNode.BackColor = Color.Yellow;
                targetNode.ForeColor = Color.Black;
                targetNode.EnsureVisible();
                previousHighlightedNode = targetNode;
            }
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

            dataGridOgeDetay.SelectionChanged += (s, e) =>
            {
                try
                {
                    if (dataGridOgeDetay.SelectedRows.Count == 0 || treeView1.Nodes.Count == 0)
                    {
                        if (previousHighlightedNode != null)
                        {
                            previousHighlightedNode.BackColor = Color.Empty;
                            previousHighlightedNode.ForeColor = Color.Empty;
                            previousHighlightedNode = null;
                        }
                        return;
                    }

                    var selectedRow = dataGridOgeDetay.SelectedRows[0];
                    if (selectedRow.DataBoundItem == null)
                        return;

                    var dataRow = ((DataRowView)selectedRow.DataBoundItem).Row;
                    string projeNo = dataRow["projeAdi"]?.ToString()?.Trim();
                    string grupAdi = dataRow.Table.Columns.Contains("grupAdi") ? dataRow["grupAdi"]?.ToString()?.Trim() : null;
                    Guid? yuklemeId = dataRow.Table.Columns.Contains("yuklemeId") && dataRow["yuklemeId"] != DBNull.Value ? (Guid?)dataRow["yuklemeId"] : null;

                    bool isProjectLevel = treeView1.SelectedNode?.Parent == null;
                    if (isProjectLevel && !string.IsNullOrEmpty(grupAdi) && yuklemeId.HasValue)
                    {
                        HighlightTreeViewNode(projeNo, grupAdi, yuklemeId);
                    }
                    else
                    {
                        if (previousHighlightedNode != null)
                        {
                            previousHighlightedNode.BackColor = Color.Empty;
                            previousHighlightedNode.ForeColor = Color.Empty;
                            previousHighlightedNode = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                   MessageBox.Show($"TreeView düğüm işaretleme hatası: {ex.Message}, StackTrace: {ex.StackTrace}");
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

                    if (dataGridOgeDetay.Rows[hit.RowIndex].DefaultCellStyle.BackColor != Color.Empty)
                    {
                        menuItemSil.Enabled = false;
                    }
                    else
                    {
                        menuItemSil.Enabled = true;
                    }
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
                            if (row.DefaultCellStyle.BackColor != Color.Empty)
                            {
                                MessageBox.Show("Renkli satırlar silinemez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                continue;
                            }

                            var dataRow = ((DataRowView)row.DataBoundItem).Row;
                            string grupAdi = dataRow["grupAdi"]?.ToString()?.Trim();
                            string malzemeKod = dataRow.Table.Columns.Contains("malzemeKod") ? dataRow["malzemeKod"]?.ToString()?.Trim() : null;
                            Guid? yuklemeId = dataRow.Table.Columns.Contains("yuklemeId") && dataRow["yuklemeId"] != DBNull.Value ? (Guid?)dataRow["yuklemeId"] : null;

                            silinenSatirlar.Add(new AutoCadAktarim
                            {
                                GrupAdi = grupAdi,
                                MalzemeKod = malzemeKod,
                                YuklemeId = yuklemeId
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
                    ColorRows();
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

                bool isProjectLevel = treeView1.SelectedNode?.Parent == null;
                bool isUstGrupLevel = treeView1.SelectedNode?.Parent != null && treeView1.SelectedNode.Parent.Parent == null;
                bool isGrupLevel = treeView1.SelectedNode?.Parent != null && treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Parent == null;
                bool isMalzemeLevel = treeView1.SelectedNode?.Parent != null && treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Parent != null;

                if (columnName == "adet" || columnName == "takimCarpani")
                {
                    if (!string.IsNullOrEmpty(input) && (!int.TryParse(input, out int value) || value <= 0))
                    {
                        e.Cancel = true;
                        MessageBox.Show($"{(columnName == "adet" ? "Adet" : "Montaj Başına Miktar")} yalnızca pozitif bir tam sayı olabilir.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (columnName == "adet" && dataGridOgeDetay.Rows[currentRowIndex].DefaultCellStyle.BackColor != Color.Empty)
                    {
                        var dataRow = ((DataRowView)dataGridOgeDetay.Rows[currentRowIndex].DataBoundItem).Row;
                        int orjinalAdet = dataRow["orjinalAdet"] != DBNull.Value ? Convert.ToInt32(dataRow["orjinalAdet"]) : 0;
                        if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int yeniAdet) && yeniAdet < orjinalAdet)
                        {
                            e.Cancel = true;
                            MessageBox.Show($"Bu poz iş hazırlamada kullanılmaktadır adet ({orjinalAdet}) değerinden küçük olamaz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else if (columnName == "malzemeKod" && isMalzemeLevel)
                {
                    if (string.IsNullOrEmpty(input))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Malzeme kodu boş olamaz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string projeNo = treeView1.SelectedNode.Parent.Parent.Parent.Text;
                    string currentGrupAdi = treeView1.SelectedNode.Parent.Text;

                    var malzemeler = AutoCadAktarimData.MalzemeleriGetir(projeNo, currentGrupAdi);
                    if (malzemeler.AsEnumerable().Any(r => r.Field<string>("malzemeKod")?.Trim() == input &&
                        (currentRowIndex < 0 || tempTablo.Rows[currentRowIndex]["malzemeKod"]?.ToString()?.Trim() != input)))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Bu malzeme kodu zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (columnName == "grupAdi" && (isProjectLevel || isUstGrupLevel || isGrupLevel))
                {
                    if (string.IsNullOrEmpty(input))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Grup adı boş olamaz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string projeNo = isProjectLevel ? treeView1.SelectedNode.Text :
                                    isUstGrupLevel ? treeView1.SelectedNode.Parent.Text :
                                    treeView1.SelectedNode.Parent.Parent.Text;

                    var gruplar = AutoCadAktarimData.GruplariGetir(projeNo);
                    if (gruplar.AsEnumerable().Any(r => r.Field<string>("grupAdi")?.Trim() == input &&
                        (currentRowIndex < 0 || tempTablo.Rows[currentRowIndex]["grupAdi"]?.ToString()?.Trim() != input)))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Bu grup adı zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            };
        }

        private void ctlProjeOgeleri_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik ="Proje Öğeleri";
            tempTablo = new DataTable();
            tempTablo.RowChanged += (s, args) => { isDirty = true; UpdateSaveButtonState(); };
            tempTablo.RowDeleted += (s, args) => { isDirty = true; UpdateSaveButtonState(); };
            UpdateSaveButtonState();
            BilgiPaleti();
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
                tempTablo.Columns.Add("takimCarpani", typeof(int));
                tempTablo.Columns.Add("yuklemeId", typeof(Guid));

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
                    HeaderText = "Üst Grup",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "takimCarpani",
                    HeaderText = "Montaj Başına Miktar",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "yuklemeId",
                    HeaderText = "Yükleme ID",
                    ReadOnly = true,
                    Visible = false
                });
            }
            else if (level == "UstGrup")
            {
                tempTablo.Columns.Add("projeAdi", typeof(string));
                tempTablo.Columns.Add("ustGrupAdi", typeof(string));
                tempTablo.Columns.Add("grupAdi", typeof(string));
                tempTablo.Columns.Add("takimCarpani", typeof(int));
                tempTablo.Columns.Add("yuklemeId", typeof(Guid));

                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "projeAdi",
                    HeaderText = "Proje",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ustGrupAdi",
                    HeaderText = "Üst Grup",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "grupAdi",
                    HeaderText = "Grup Adı",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "takimCarpani",
                    HeaderText = "Montaj Başına Miktar",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "yuklemeId",
                    HeaderText = "Yükleme ID",
                    ReadOnly = true,
                    Visible = false
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
                tempTablo.Columns.Add("yuklemeId", typeof(Guid));
                tempTablo.Columns.Add("orjinalAdet", typeof(int));

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
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "yuklemeId",
                    HeaderText = "Yükleme ID",
                    ReadOnly = true,
                    Visible = false
                });
                dataGridOgeDetay.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "orjinalAdet",
                    HeaderText = "Orijinal Adet",
                    Visible = false
                });
            }

            bindingSource.DataSource = tempTablo;
        }

        private void TreeViewYukle(string projeNo)
        {
            var kayitlar = AutoCadAktarimData.GetAutoCadKayitlari(projeNo);

            treeView1.Nodes.Clear();
            var projeDugumleri = new Dictionary<string, TreeNode>();
            var ustGrupDugumleri = new Dictionary<string, TreeNode>();
            var grupDugumleri = new Dictionary<string, TreeNode>();

            foreach (var kayit in kayitlar)
            {
                if (!projeDugumleri.ContainsKey(kayit.Proje))
                {
                    var projeDugumu = new TreeNode(kayit.Proje);
                    treeView1.Nodes.Add(projeDugumu);
                    projeDugumleri[kayit.Proje] = projeDugumu;
                }

                if (!string.IsNullOrEmpty(kayit.Grup))
                {
                    string ustGrupKey = kayit.Grup.Contains("-") ? kayit.Grup.Split('-')[0] : kayit.Grup;
                    string yuklemeIdStr = kayit.YuklemeId?.ToString() ?? Guid.NewGuid().ToString();
                    string ustGrupAnahtari = $"{kayit.Proje}_{ustGrupKey}_{yuklemeIdStr}";

                    if (!ustGrupDugumleri.ContainsKey(ustGrupAnahtari))
                    {
                        var ustGrupDugumu = new TreeNode(ustGrupKey);
                        ustGrupDugumu.Tag = new Tuple<string, Guid?>(ustGrupKey, kayit.YuklemeId);
                        projeDugumleri[kayit.Proje].Nodes.Add(ustGrupDugumu);
                        ustGrupDugumleri[ustGrupAnahtari] = ustGrupDugumu;
                    }

                    if (kayit.Grup.Contains("-"))
                    {
                        string grupAnahtari = $"{kayit.Proje}_{kayit.Grup}_{yuklemeIdStr}";
                        if (!grupDugumleri.ContainsKey(grupAnahtari))
                        {
                            var grupDugumu = new TreeNode(kayit.Grup);
                            grupDugumu.Tag = kayit.YuklemeId;
                            ustGrupDugumleri[ustGrupAnahtari].Nodes.Add(grupDugumu);
                            grupDugumleri[grupAnahtari] = grupDugumu;
                        }

                        if (!string.IsNullOrEmpty(kayit.MalzemeKod))
                        {
                            var malzemeDugumu = new TreeNode(kayit.MalzemeKod);
                            grupDugumleri[grupAnahtari].Nodes.Add(malzemeDugumu);
                        }
                    }
                }
            }

            foreach (TreeNode projeDugumu in treeView1.Nodes)
            {
                projeDugumu.Expand();
                foreach (TreeNode ustGrupDugumu in projeDugumu.Nodes)
                {
                    ustGrupDugumu.Collapse();
                    foreach (TreeNode grupDugumu in ustGrupDugumu.Nodes)
                    {
                        grupDugumu.Collapse();
                    }
                }
            }

            if (treeView1.Nodes.Count > 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
            }

            treeView1.Refresh();
        }

        private bool ValidateTableData(TreeNode selectedNode)
        {
            if (tempTablo == null || tempTablo.Rows.Count == 0) return true;

            bool isProjectLevel = selectedNode?.Parent == null;
            bool isUstGrupLevel = selectedNode?.Parent != null && selectedNode.Parent.Parent == null;
            bool isGrupLevel = selectedNode?.Parent != null && selectedNode.Parent.Parent != null && selectedNode.Parent.Parent.Parent == null;
            bool isMalzemeLevel = selectedNode?.Parent != null && selectedNode.Parent.Parent != null && selectedNode.Parent.Parent.Parent != null;


            foreach (DataRow row in tempTablo.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted))
            {
                if (isProjectLevel || isUstGrupLevel)
                {
                    string grupAdi = row["grupAdi"]?.ToString()?.Trim();
                    if (string.IsNullOrEmpty(grupAdi))
                    {
                        MessageBox.Show("Grup adı boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (row["takimCarpani"] == DBNull.Value || !int.TryParse(row["takimCarpani"]?.ToString(), out int carpani) || carpani <= 0)
                    {
                        MessageBox.Show("Montaj Başına Miktar sıfırdan büyük bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (isGrupLevel || isMalzemeLevel)
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

                        if (tempTablo.Columns.Contains("kalite"))
                        {
                            string kalite = row["kalite"]?.ToString()?.Trim();
                            if (string.IsNullOrEmpty(kalite))
                            {
                                MessageBox.Show("Kalite boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            btnKaydet.Enabled = isDirty;
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

                bool isProjectLevel = e.Node.Parent == null;
                bool isUstGrupLevel = e.Node.Parent != null && e.Node.Parent.Parent == null;
                bool isGrupLevel = e.Node.Parent != null && e.Node.Parent.Parent != null && e.Node.Parent.Parent.Parent == null;
                bool isMalzemeLevel = e.Node.Parent != null && e.Node.Parent.Parent != null && e.Node.Parent.Parent.Parent != null;

                if (isProjectLevel)
                {
                    string projeNo = e.Node.Text;
                    InitializeTempTablo("Proje");
                    var ustGruplar = AutoCadAktarimData.UstGruplariGetir(projeNo);

                    foreach (DataRow row in ustGruplar.Rows)
                    {
                        tempTablo.Rows.Add(
                            row["projeAdi"],
                            row["ustGrupAdi"],
                            row["takimCarpani"],
                            row["yuklemeId"]);
                    }
                }
                else if (isUstGrupLevel)
                {
                    string projeNo = e.Node.Parent.Text;
                    Tuple<string, Guid?> tagInfo = e.Node.Tag as Tuple<string, Guid?>;
                    string ustGrupAdi = tagInfo.Item1;
                    Guid? yuklemeId = tagInfo.Item2;

                    InitializeTempTablo("UstGrup");
                    var gruplar = AutoCadAktarimData.GruplariGetir(projeNo);
                    var altGruplar = gruplar.AsEnumerable()
                        .Where(r => r.Field<string>("grupAdi").StartsWith(ustGrupAdi + "-") && r.Field<Guid?>("yuklemeId") == yuklemeId)
                        .ToList();

                    foreach (var row in altGruplar)
                    {
                        int takimCarpani = row.Table.Columns.Contains("takimCarpani") ? (row.Field<int?>("takimCarpani") ?? 1) : 1;
                        tempTablo.Rows.Add(projeNo, ustGrupAdi, row.Field<string>("grupAdi"), takimCarpani, row.Field<Guid?>("yuklemeId"));
                    }
                }
                else if (isGrupLevel)
                {
                    string projeNo = e.Node.Parent.Parent.Text;
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
                                row["kalite"],
                                row["yuklemeId"],
                                row["orjinalAdet"]);
                        }
                    }
                }
                else if (isMalzemeLevel)
                {
                    string projeNo = e.Node.Parent.Parent.Parent.Text;
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
                                row["kalite"],
                                row["yuklemeId"],
                                row["orjinalAdet"]);
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

                    ColorRows();
                    UpdateSaveButtonState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}\nDetay: {ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _pendingNode = null;
            }
        }
        private void ColorRows()
        {
            if (treeView1.SelectedNode == null)
            {
                return;
            }

            bool isProjectLevel = treeView1.SelectedNode.Parent == null;
            bool isUstGrupLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent == null;

            if (isProjectLevel || isUstGrupLevel)
            {
                foreach (DataGridViewRow row in dataGridOgeDetay.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.Empty;
                }
                return;
            }

            var kesimDetaylari = KesimDetaylariData.GetKesimDetaylariBilgileri();
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
                        string projeAdi = row.Cells[projeAdiIndex].Value?.ToString()?.Trim().ToLowerInvariant() ?? "";
                        string grupAdi = row.Cells[grupAdiIndex].Value?.ToString()?.Trim().ToLowerInvariant() ?? "";
                        string malzemeKod = row.Cells[malzemeKodIndex].Value?.ToString()?.Trim().ToLowerInvariant() ?? "";
                        string adet = row.Cells[adetIndex].Value?.ToString()?.Trim();
                        string malzemeAd = row.Cells[malzemeAdIndex].Value?.ToString()?.Trim().ToLowerInvariant() ?? "";
                        string kalite = row.Cells[kaliteIndex].Value?.ToString()?.Trim().ToLowerInvariant() ?? "";


                        if (string.IsNullOrEmpty(projeAdi) || string.IsNullOrEmpty(grupAdi) ||
                            string.IsNullOrEmpty(malzemeKod) || string.IsNullOrEmpty(adet) ||
                            string.IsNullOrEmpty(malzemeAd) || string.IsNullOrEmpty(kalite))
                        {
                            row.DefaultCellStyle.BackColor = Color.Empty;
                            continue;
                        }

                        string[] parts = malzemeKod.Split('-');
                        if (parts.Length >= 3)
                        {
                            string kalipKodu = $"{parts[0]}-{parts[1]}";
                            if (!AutoCadAktarimData.GetirStandartGruplar(kalipKodu))
                            {
                                parts[1] = "00";
                                malzemeKod = string.Join("-", parts);
                            }
                        }
                        malzemeKod = malzemeKod.ToLowerInvariant();

                        string key = $"{kalite}_{malzemeAd}_{malzemeKod}_{projeAdi}_{grupAdi}";

                        var kesimDetayi = kesimDetaylari.FirstOrDefault(k => k.Key == key);

                        if (kesimDetayi != null)
                        {
                            if (kesimDetayi.toplamAdet == 0)
                            {
                                row.DefaultCellStyle.BackColor = Color.Empty;
                                continue;
                            }

                            if (kesimDetayi.ekBilgi)
                            {
                                row.DefaultCellStyle.BackColor = Color.Blue;
                            }
                            else if (kesimDetayi.kesilmisAdet == kesimDetayi.toplamAdet && kesimDetayi.toplamAdet > 0)
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
        public void BilgiPaleti()
        {
            Label lblTitle = new Label();
            lblTitle.Text = "Durum Açıklaması:";
            lblTitle.Font = new Font(lblTitle.Font, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(10, 10);
            panelBilgi.Controls.Add(lblTitle);

            int yOffset = 35;

            AddColorLegend(panelBilgi, Color.Red, "Tamamlandı (Tümü Kesildi)", yOffset);
            yOffset += 25;

            AddColorLegend(panelBilgi, Color.Orange, "Kısmen Tamamlandı (Yerleşim Planı oluşturuldu kesim başladı)", yOffset);
            yOffset += 25;

            AddColorLegend(panelBilgi, Color.Green, "Başlanmadı (Yerleşim Planı oluşturuldu kesim bekliyor)", yOffset);
            yOffset += 25;

            AddColorLegend(panelBilgi, Color.Blue, "Ek Bilgi / Özel Durum", yOffset);
        }
        private void AddColorLegend(Panel parentPanel, Color color, string text, int yPos)
        {
            Label colorBox = new Label();
            colorBox.BackColor = color;
            colorBox.Size = new Size(20, 15); // Küçük bir renk kutusu
            colorBox.Location = new Point(10, yPos);
            colorBox.BorderStyle = BorderStyle.FixedSingle;
            parentPanel.Controls.Add(colorBox);

            Label descriptionLabel = new Label();
            descriptionLabel.Text = text;
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(colorBox.Right + 5, yPos); // Renk kutusunun sağında
            parentPanel.Controls.Add(descriptionLabel);
        }


        //private void ColorRows()
        //{
        //    if (treeView1.SelectedNode == null)
        //    {
        //        return;
        //    }

        //    bool isProjectLevel = treeView1.SelectedNode.Parent == null;
        //    bool isUstGrupLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent == null;
        //    bool isGrupLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Parent == null;
        //    bool isMalzemeLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Parent != null;

        //    if (isProjectLevel || isUstGrupLevel)
        //    {
        //        foreach (DataGridViewRow row in dataGridOgeDetay.Rows)
        //        {
        //            row.DefaultCellStyle.BackColor = Color.Empty;
        //        }
        //        return;
        //    }

        //    var kesimDetaylari = KesimDetaylariData.GetKesimDetaylariBilgileri();
        //    var mevcutSutunlar = dataGridOgeDetay.Columns.Cast<DataGridViewColumn>().Select(c => c.DataPropertyName).ToList();
        //    var sutunAdlari = dataGridOgeDetay.Columns.Cast<DataGridViewColumn>()
        //        .ToDictionary(c => c.DataPropertyName.ToLowerInvariant(), c => c.Index, StringComparer.OrdinalIgnoreCase);
        //    var eksikSutunlar = new List<string>();

        //    string[] gerekliSutunlar = { "projeadi", "grupadi", "malzemekod", "adet", "malzemead", "kalite" };
        //    foreach (var sutun in gerekliSutunlar)
        //    {
        //        if (!sutunAdlari.ContainsKey(sutun))
        //        {
        //            eksikSutunlar.Add(sutun);
        //        }
        //    }
        //    if (eksikSutunlar.Any())
        //    {
        //        MessageBox.Show($"DataGridView sütunları eksik: {string.Join(", ", eksikSutunlar)}. Lütfen destek ekibine başvurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    int projeAdiIndex = sutunAdlari["projeadi"];
        //    int grupAdiIndex = sutunAdlari["grupadi"];
        //    int malzemeKodIndex = sutunAdlari["malzemekod"];
        //    int adetIndex = sutunAdlari["adet"];
        //    int malzemeAdIndex = sutunAdlari["malzemead"];
        //    int kaliteIndex = sutunAdlari["kalite"];

        //    foreach (DataGridViewRow row in dataGridOgeDetay.Rows)
        //    {
        //        if (!row.IsNewRow)
        //        {
        //            try
        //            {
        //                string projeAdi = row.Cells[projeAdiIndex].Value?.ToString()?.Trim().ToLowerInvariant() ?? "";
        //                string grupAdi = row.Cells[grupAdiIndex].Value?.ToString()?.Trim().ToLowerInvariant() ?? "";
        //                string malzemeKod = row.Cells[malzemeKodIndex].Value?.ToString()?.Trim() ?? "";
        //                string adet = row.Cells[adetIndex].Value?.ToString()?.Trim();
        //                string malzemeAd = row.Cells[malzemeAdIndex].Value?.ToString()?.Trim().ToLowerInvariant() ?? "";
        //                string kalite = row.Cells[kaliteIndex].Value?.ToString()?.Trim().ToLowerInvariant() ?? "";

        //                if (string.IsNullOrEmpty(projeAdi) || string.IsNullOrEmpty(grupAdi) ||
        //                    string.IsNullOrEmpty(malzemeKod) || string.IsNullOrEmpty(adet) ||
        //                    string.IsNullOrEmpty(malzemeAd) || string.IsNullOrEmpty(kalite))
        //                {
        //                    row.DefaultCellStyle.BackColor = Color.Empty;
        //                    continue;
        //                }

        //                string[] parts = malzemeKod.Split('-');
        //                if (parts.Length >= 3)
        //                {
        //                    string kalipKodu = $"{parts[0]}-{parts[1]}";
        //                    if (!AutoCadAktarimData.GetirStandartGruplar(kalipKodu))
        //                    {
        //                        parts[1] = "00";
        //                        malzemeKod = string.Join("-", parts);
        //                    }
        //                }
        //                malzemeKod = malzemeKod.ToLowerInvariant();

        //                string key = $"{kalite}_{malzemeAd}_{malzemeKod}_{projeAdi}";

        //                var kesimDetayi = kesimDetaylari.FirstOrDefault(k => k.Key == key);

        //                if (kesimDetayi != null)
        //                {
        //                    if (kesimDetayi.kesilmisAdet == kesimDetayi.toplamAdet && kesimDetayi.toplamAdet > 0)
        //                    {
        //                        row.DefaultCellStyle.BackColor = Color.Red;
        //                    }
        //                    else if (kesimDetayi.kesilmisAdet > 0)
        //                    {
        //                        row.DefaultCellStyle.BackColor = Color.Orange;
        //                    }
        //                    else
        //                    {
        //                        row.DefaultCellStyle.BackColor = Color.Green;
        //                    }
        //                }
        //                else
        //                {
        //                    row.DefaultCellStyle.BackColor = Color.Empty;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"ColorRows: Satır {row.Index} için hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //    }
        //}

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || tempTablo == null || !isDirty)
            {
                MessageBox.Show("Kaydedilecek değişiklik yok veya bir node seçili değil.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateTableData(treeView1.SelectedNode))
                return;

            TreeNode currentSelectedNode = treeView1.SelectedNode;
            string projeNoAtSave = null;
            string grupAdiAtSave = null;
            string malzemeKodAtSave = null;
            Guid? yuklemeIdAtSave = null;

            bool isProjectLevelAtSave = currentSelectedNode.Parent == null;
            bool isUstGrupLevelAtSave = currentSelectedNode.Parent != null && currentSelectedNode.Parent.Parent == null;
            bool isGrupLevelAtSave = currentSelectedNode.Parent != null && currentSelectedNode.Parent.Parent != null && currentSelectedNode.Parent.Parent.Parent == null;
            bool isMalzemeLevelAtSave = currentSelectedNode.Parent != null && currentSelectedNode.Parent.Parent != null && currentSelectedNode.Parent.Parent.Parent != null;

            if (isProjectLevelAtSave)
            {
                projeNoAtSave = currentSelectedNode.Text;
            }
            else if (isUstGrupLevelAtSave)
            {
                projeNoAtSave = currentSelectedNode.Parent.Text;
                Tuple<string, Guid?> tagInfo = currentSelectedNode.Tag as Tuple<string, Guid?>;
                grupAdiAtSave = tagInfo?.Item1; 
                yuklemeIdAtSave = tagInfo?.Item2;
            }
            else if (isGrupLevelAtSave)
            {
                projeNoAtSave = currentSelectedNode.Parent.Parent.Text;
                grupAdiAtSave = currentSelectedNode.Text; 
                yuklemeIdAtSave = currentSelectedNode.Tag as Guid?;
            }
            else if (isMalzemeLevelAtSave)
            {
                projeNoAtSave = currentSelectedNode.Parent.Parent.Parent.Text;
                grupAdiAtSave = currentSelectedNode.Parent.Text;
                malzemeKodAtSave = currentSelectedNode.Text;
                var ustGrupTagInfo = currentSelectedNode.Parent.Parent.Tag as Tuple<string, Guid?>;
                yuklemeIdAtSave = ustGrupTagInfo?.Item2;
            }


            try
            {
                bool isProjectLevel = treeView1.SelectedNode.Parent == null;
                bool isUstGrupLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent == null;
                bool isGrupLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Parent == null;
                bool isMalzemeLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Parent != null;

                string projeNo = isProjectLevel ? treeView1.SelectedNode.Text :
                                 isUstGrupLevel ? treeView1.SelectedNode.Parent.Text :
                                 isGrupLevel ? treeView1.SelectedNode.Parent.Parent.Text :
                                 treeView1.SelectedNode.Parent.Parent.Parent.Text;

                string newlyAddedUstGrup = null;
                Guid? newlyAddedYuklemeId = null;

                if (isProjectLevel)
                {
                    foreach (DataRow row in tempTablo.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted))
                    {
                        string grupAdi = row["grupAdi"]?.ToString()?.Trim();
                        Guid? yuklemeId = row["yuklemeId"] == DBNull.Value ? null : (Guid?)row["yuklemeId"];
                        int takimCarpani = row["takimCarpani"] != DBNull.Value ? Convert.ToInt32(row["takimCarpani"]) : 1;

                        if (row.RowState == DataRowState.Added)
                        {
                            AutoCadAktarimData.UstGrupEkleGuncelle(projeNo, grupAdi, yuklemeId, takimCarpani, null);
                            newlyAddedUstGrup = grupAdi; 
                            newlyAddedYuklemeId = yuklemeId; 
                        }
                        else if (row.RowState == DataRowState.Modified)
                        {
                            string eskiGrupAdi = row["grupAdi", DataRowVersion.Original]?.ToString()?.Trim();
                            AutoCadAktarimData.UstGrupEkleGuncelle(projeNo, grupAdi, yuklemeId, takimCarpani, eskiGrupAdi);
                            AutoCadAktarimData.UpdateTakimCarpaniVeAltGruplar(projeNo, grupAdi, yuklemeId, takimCarpani);
                        }
                    }

                    foreach (var silinen in silinenSatirlar)
                    {
                        if (!string.IsNullOrEmpty(silinen.GrupAdi) && silinen.YuklemeId.HasValue)
                        {
                            AutoCadAktarimData.UstGrupSil(projeNo, silinen.GrupAdi, silinen.YuklemeId.Value);
                        }
                    }
                }
                else if (isUstGrupLevel)
                {
                    string ustGrup = (treeView1.SelectedNode.Tag as Tuple<string, Guid?>)?.Item1; 
                    Guid? yuklemeId = (treeView1.SelectedNode.Tag as Tuple<string, Guid?>)?.Item2; 

                    foreach (DataRow row in tempTablo.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted))
                    {
                        string grupAdi = row["grupAdi"]?.ToString()?.Trim();
                        Guid? rowYuklemeId = row["yuklemeId"] == DBNull.Value ? null : (Guid?)row["yuklemeId"];
                        int takimCarpani = row["takimCarpani"] != DBNull.Value ? Convert.ToInt32(row["takimCarpani"]) : 1;

                        if (row.RowState == DataRowState.Added)
                        {
                            AutoCadAktarimData.GrupEkleGuncelle(projeNo, grupAdi, null, rowYuklemeId);
                            AutoCadAktarimData.UpdateTakimCarpani(projeNo, grupAdi, takimCarpani);
                        }
                        else if (row.RowState == DataRowState.Modified)
                        {
                            string eskiGrupAdi = row["grupAdi", DataRowVersion.Original]?.ToString()?.Trim();
                            AutoCadAktarimData.GrupEkleGuncelle(projeNo, grupAdi, eskiGrupAdi, rowYuklemeId);
                            AutoCadAktarimData.UpdateTakimCarpani(projeNo, grupAdi, takimCarpani);
                        }
                    }

                    foreach (var silinen in silinenSatirlar)
                    {
                        if (!string.IsNullOrEmpty(silinen.GrupAdi))
                        {
                            AutoCadAktarimData.GrupSil(projeNo, silinen.GrupAdi);
                        }
                    }
                }
                else if (isGrupLevel || isMalzemeLevel)
                {
                    string grupAdi = isGrupLevel ? treeView1.SelectedNode.Text : treeView1.SelectedNode.Parent.Text;

                    foreach (DataRow row in tempTablo.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted))
                    {
                        string malzemeKod = row["malzemeKod"]?.ToString()?.Trim();
                        int adet = Convert.ToInt32(row["adet"]);
                        string malzemeAd = row["malzemeAd"]?.ToString()?.Trim();
                        string kalite = row["kalite"]?.ToString()?.Trim();
                        Guid? yuklemeId = row["yuklemeId"] == DBNull.Value ? null : (Guid?)row["yuklemeId"];
                        int orjinalAdet = row["orjinalAdet"] != DBNull.Value ? Convert.ToInt32(row["orjinalAdet"]) : 0;

                        if (row.RowState == DataRowState.Added)
                        {
                            AutoCadAktarimData.MalzemeEkleGuncelle(projeNo, grupAdi, malzemeKod, adet, malzemeAd, kalite, yuklemeId);
                        }
                        else if (row.RowState == DataRowState.Modified)
                        {
                            string eskiMalzemeKod = row["malzemeKod", DataRowVersion.Original]?.ToString()?.Trim();
                            AutoCadAktarimData.MalzemeEkleGuncelle(projeNo, grupAdi, malzemeKod, adet, malzemeAd, kalite, yuklemeId, eskiMalzemeKod);
                        }
                    }

                    foreach (var silinen in silinenSatirlar)
                    {
                        if (!string.IsNullOrEmpty(silinen.MalzemeKod))
                        {
                            AutoCadAktarimData.MalzemeSil(projeNo, grupAdi, silinen.MalzemeKod);
                        }
                    }
                }

                tempTablo.AcceptChanges();
                silinenSatirlar.Clear();
                isDirty = false;
                silinenSatirSayisi = 0;
                bindingSource.ResetBindings(false);
                UpdateSaveButtonState();

                MessageBox.Show("Değişiklikler başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                TreeViewYukle(projeNoAtSave);

                TreeNode nodeToSelect = null;
                if (isProjectLevelAtSave)
                {
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        if (node.Text == projeNoAtSave)
                        {
                            nodeToSelect = node;
                            break;
                        }
                    }
                }
                else if (isUstGrupLevelAtSave && grupAdiAtSave != null && yuklemeIdAtSave.HasValue)
                {
                    foreach (TreeNode projeNode in treeView1.Nodes)
                    {
                        if (projeNode.Text == projeNoAtSave)
                        {
                            foreach (TreeNode ustGrupNode in projeNode.Nodes)
                            {
                                var tagInfo = ustGrupNode.Tag as Tuple<string, Guid?>;
                                if (tagInfo != null && tagInfo.Item1 == grupAdiAtSave && tagInfo.Item2 == yuklemeIdAtSave)
                                {
                                    nodeToSelect = ustGrupNode;
                                    break;
                                }
                            }
                            if (nodeToSelect != null) break;
                        }
                    }
                }
                else if (isGrupLevelAtSave && grupAdiAtSave != null && yuklemeIdAtSave.HasValue)
                {
                    foreach (TreeNode projeNode in treeView1.Nodes)
                    {
                        if (projeNode.Text == projeNoAtSave)
                        {
                            foreach (TreeNode ustGrupNode in projeNode.Nodes)
                            {
                                var ustGrupTagInfo = ustGrupNode.Tag as Tuple<string, Guid?>;
                                if (ustGrupTagInfo != null)
                                {
                                    foreach (TreeNode grupNode in ustGrupNode.Nodes)
                                    {
                                        Guid? grupYuklemeId = grupNode.Tag as Guid?;
                                        if (grupNode.Text == grupAdiAtSave && grupYuklemeId == yuklemeIdAtSave)
                                        {
                                            nodeToSelect = grupNode;
                                            break;
                                        }
                                    }
                                }
                                if (nodeToSelect != null) break;
                            }
                            if (nodeToSelect != null) break;
                        }
                    }
                }
                else if (isMalzemeLevelAtSave && grupAdiAtSave != null && malzemeKodAtSave != null && yuklemeIdAtSave.HasValue)
                {
                    foreach (TreeNode projeNode in treeView1.Nodes)
                    {
                        if (projeNode.Text == projeNoAtSave)
                        {
                            foreach (TreeNode ustGrupNode in projeNode.Nodes)
                            {
                                var ustGrupTagInfo = ustGrupNode.Tag as Tuple<string, Guid?>;
                                if (ustGrupTagInfo != null && ustGrupTagInfo.Item2 == yuklemeIdAtSave)
                                {
                                    foreach (TreeNode grupNode in ustGrupNode.Nodes)
                                    {
                                        if (grupNode.Text == grupAdiAtSave)
                                        {
                                            foreach (TreeNode malzemeNode in grupNode.Nodes)
                                            {
                                                if (malzemeNode.Text == malzemeKodAtSave)
                                                {
                                                    nodeToSelect = malzemeNode;
                                                    break;
                                                }
                                            }
                                        }
                                        if (nodeToSelect != null) break;
                                    }
                                }
                                if (nodeToSelect != null) break;
                            }
                            if (nodeToSelect != null) break;
                        }
                    }
                }

                if (nodeToSelect != null)
                {
                    _pendingNode = nodeToSelect; 
                    treeView1.SelectedNode = nodeToSelect;
                    nodeToSelect.EnsureVisible();
                }
                else if (treeView1.Nodes.Count > 0)
                {
                    _pendingNode = treeView1.Nodes[0];
                    treeView1.SelectedNode = treeView1.Nodes[0];
                    treeView1.Nodes[0].EnsureVisible();
                }

                treeView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaydetme hatası: {ex.Message}\nDetay: {ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            if (!HandleUnsavedChanges())
                return;

            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Lütfen bir proje, üst grup veya grup seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isProjectLevel = treeView1.SelectedNode.Parent == null;
            bool isUstGrupLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent == null;
            bool isGrupLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Parent == null;
            bool isMalzemeLevel = treeView1.SelectedNode.Parent != null && treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Parent != null;

            if (isProjectLevel || isUstGrupLevel || isGrupLevel || isMalzemeLevel)
            {
                DataRow newRow = tempTablo.NewRow();
                string projeNo = isProjectLevel ? treeView1.SelectedNode.Text :
                                 isUstGrupLevel ? treeView1.SelectedNode.Parent.Text :
                                 isGrupLevel ? treeView1.SelectedNode.Parent.Parent.Text :
                                 treeView1.SelectedNode.Parent.Parent.Parent.Text;

                newRow["projeAdi"] = projeNo;

                if (isProjectLevel)
                {
                    newRow["yuklemeId"] = Guid.NewGuid();
                }
                else if (isUstGrupLevel)
                {
                    Tuple<string, Guid?> tagInfo = treeView1.SelectedNode.Tag as Tuple<string, Guid?>;
                    newRow["ustGrupAdi"] = tagInfo.Item1;
                    newRow["yuklemeId"] = tagInfo.Item2;
                }
                else if (isGrupLevel)
                {
                    newRow["grupAdi"] = treeView1.SelectedNode.Text;
                    Tuple<string, Guid?> tagInfo = treeView1.SelectedNode.Parent.Tag as Tuple<string, Guid?>;
                    newRow["yuklemeId"] = tagInfo.Item2;
                }
                else if (isMalzemeLevel)
                {
                    newRow["grupAdi"] = treeView1.SelectedNode.Parent.Text;
                    newRow["malzemeKod"] = treeView1.SelectedNode.Text;
                    Tuple<string, Guid?> tagInfo = treeView1.SelectedNode.Parent.Parent.Tag as Tuple<string, Guid?>;
                    if (tagInfo?.Item2 != null)
                    {
                        newRow["yuklemeId"] = tagInfo.Item2;
                    }
                    else
                    {
                        newRow["yuklemeId"] = Guid.NewGuid();
                    }
                }

                if (tempTablo.Columns.Contains("takimCarpani"))
                {
                    newRow["takimCarpani"] = 1;
                }

                if (tempTablo.Columns.Contains("adet"))
                {
                    newRow["adet"] = 1;
                }
                if (tempTablo.Columns.Contains("orjinalAdet"))
                {
                    newRow["orjinalAdet"] = 1;
                }

                tempTablo.Rows.Add(newRow);
                bindingSource.ResetBindings(false);
                isDirty = true;
                UpdateSaveButtonState();
            }
            else
            {
                MessageBox.Show("Yeni satır yalnızca proje, üst grup, grup veya malzeme seviyesinde eklenebilir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnStandartProje_Click(object sender, EventArgs e)
        {
            frmStandartProjeler standartProjeler = new frmStandartProjeler();
            standartProjeler.ShowDialog();
        }

        private void txtProjeNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.btnAra.PerformClick();
            }
        }

        private void ctlProjeOgeleri_Resize(object sender, EventArgs e)
        {
            panelList.Invalidate();
            treeView1.Invalidate();
            panelSearch.Invalidate();
        }
    }
}