using CEKA_APP.Entitys.Genel;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Forms;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.ProjeFinans;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlOdemeSartlari : UserControl
    {
        private List<(int projeId, int kilometreTasiId)> _pendingDeletions = new List<(int projeId, int kilometreTasiId)>();
        private List<OdemeSartlari> _newlyAddedMilestones = new List<OdemeSartlari>();
        private List<OdemeHareketleri> _odemeHareketleri = new List<OdemeHareketleri>();
        private int? projeId;
        private int sayfaIdOdemeSartlari = (int)SayfaTipi.OdemeSartlari;
        private readonly IServiceProvider _serviceProvider;

        private IOdemeSartlariService _odemeSartlariService => _serviceProvider.GetRequiredService<IOdemeSartlariService>();
        private IOdemeHareketleriService _odemeHareketleriService => _serviceProvider.GetRequiredService<IOdemeHareketleriService>();
        private IKilometreTaslariService _kilometreTaslariService => _serviceProvider.GetRequiredService<IKilometreTaslariService>();
        private IFiyatlandirmaService _fiyatlandirmaService => _serviceProvider.GetRequiredService<IFiyatlandirmaService>();
        private IMusterilerService _musterilerService => _serviceProvider.GetRequiredService<IMusterilerService>();
        private IFinansProjelerService _finansProjelerService => _serviceProvider.GetRequiredService<IFinansProjelerService>();
        private IProjeIliskiService _projeIliskiService => _serviceProvider.GetRequiredService<IProjeIliskiService>();
        private IProjeKutukService _projeKutukService => _serviceProvider.GetRequiredService<IProjeKutukService>();
        private ITeminatMektuplariService _teminatMektuplariService => _serviceProvider.GetRequiredService<ITeminatMektuplariService>();
        private ISayfaStatusService _sayfaStatusService => _serviceProvider.GetRequiredService<ISayfaStatusService>();

        public ctlOdemeSartlari(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            this.DoubleBuffered = true;
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.AutoSize = false;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.MinimumSize = new Size(tableLayoutPanel1.Width, 200);

            tableLayoutPanel1.ColumnCount = 13;
            tableLayoutPanel1.ColumnStyles.Clear();

            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8f));

            btnKilometreTasiEkle.Enabled = false;
            tableLayoutPanel1.AllowDrop = false;

            chkTutarTamaminiKullan.CheckedChanged += ChkTutarTamaminiKullan_CheckedChanged;
            txtEksilenTutar.TextChanged += TxtEksilenTutar_TextChanged;
        }
        private void TxtCikarilacakTutar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) && ((sender as TextBox).Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) > -1))
            {
                e.Handled = true;
            }
        }
        private void LoadOdemeBilgileri(string projeNo)
        {
            projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            tableLayoutPanel1.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.RowCount = 0;
                AddHeaderRow();

                bool isUpperProject = !_projeIliskiService.CheckAltProje(projeId.Value);

                lblTeminatBilgi.Text = string.Empty;
                if (!isUpperProject)
                {
                    var ustProjeId = _projeIliskiService.GetUstProjeId(projeId.Value);
                    if (ustProjeId.HasValue)
                    {
                        var ustProjeNo = _finansProjelerService.GetProjeNoById(ustProjeId.Value);
                        if (!string.IsNullOrEmpty(ustProjeNo))
                        {
                            var ustProjeTeminatlari = _teminatMektuplariService
                                .GetTeminatMektuplari()
                                .Where(tm => tm.projeId == ustProjeId.Value)
                                .ToList();

                            if (ustProjeTeminatlari.Any())
                            {
                                var teminatKilometreTaslari = ustProjeTeminatlari
                                    .Select(tm => tm.kilometreTasiId)
                                    .Distinct()
                                    .ToList();

                                if (teminatKilometreTaslari.Any())
                                {
                                    var kilometreTasiAdlari = _kilometreTaslariService.GetKilometreTasiAdlariByIds(teminatKilometreTaslari);
                                    if (kilometreTasiAdlari.Any())
                                    {
                                        lblTeminatBilgi.Text = $"Üst proje ({ustProjeNo}) için şu kilometre taşlarına teminat mektubu verilmiştir: {string.Join(", ", kilometreTasiAdlari)}";
                                        lblTeminatBilgi.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        lblTeminatBilgi.Text = string.Empty;
                                    }
                                }
                            }
                        }
                    }
                }
                var odemeBilgileriFromDb = _odemeSartlariService.GetOdemeBilgileriByProjeId(projeId.Value);
                var teminatMektuplari = _teminatMektuplariService.GetTeminatMektuplari().Where(m => m.projeId == projeId.Value).ToList();

                List<OdemeSartlari> combinedOdemeBilgileri = new List<OdemeSartlari>();
                combinedOdemeBilgileri.AddRange(odemeBilgileriFromDb);
                combinedOdemeBilgileri.AddRange(_newlyAddedMilestones.Where(m => m.projeId == projeId && m.kilometreTasiId == 0));

                var filteredOdemeBilgileri = combinedOdemeBilgileri
                    .Where(odemeBilgi => !_pendingDeletions.Any(pd => pd.projeId == projeId && pd.kilometreTasiId == odemeBilgi.kilometreTasiId))
                    .OrderBy(o => o.tahminiTarih.HasValue ? 0 : 1)
                    .ThenBy(o => o.tahminiTarih.HasValue ? o.tahminiTarih.Value : DateTime.MaxValue)
                    .ToList();

                foreach (var odemeBilgi in filteredOdemeBilgileri)
                {
                    int newRowIndex = tableLayoutPanel1.RowCount;
                    tableLayoutPanel1.RowCount++;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                    CheckBox chkSelect = new CheckBox
                    {
                        AutoSize = true,
                        Tag = odemeBilgi,
                        Margin = new Padding(3, 10, 3, 3)
                    };
                    chkSelect.CheckedChanged += new System.EventHandler(this.chkSelect_CheckedChanged);
                    tableLayoutPanel1.Controls.Add(chkSelect, 0, newRowIndex);

                    var pbDelete = new PictureBox
                    {
                        Image = Properties.Resources.copKutusu,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        Size = new Size(26, 26),
                        Cursor = Cursors.Hand
                    };
                    pbDelete.Click += PbDelete_Click;
                    pbDelete.Tag = odemeBilgi;
                    tableLayoutPanel1.Controls.Add(pbDelete, 1, newRowIndex);

                    var lblKilometreTasiAdi = new Label
                    {
                        Text = odemeBilgi.kilometreTasiAdi,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 10)
                    };
                    var lblOran = new Label
                    {
                        Text = $"%{odemeBilgi.oran.ToString("F4", CultureInfo.CurrentCulture)}",
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 10)
                    };
                    var txtTutar = new TextBox
                    {
                        Text = odemeBilgi.tutar.ToString("N2", CultureInfo.CurrentCulture),
                        Dock = DockStyle.Fill,
                        TextAlign = HorizontalAlignment.Center,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 10),
                        BorderStyle = BorderStyle.FixedSingle,
                        Enabled = false,
                        BackColor = Color.LightGray
                    };
                    var txtKalanTutar = new TextBox
                    {
                        Text = odemeBilgi.kalanTutar.ToString("N2", CultureInfo.CurrentCulture),
                        Dock = DockStyle.Fill,
                        TextAlign = HorizontalAlignment.Center,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 10),
                        BorderStyle = BorderStyle.FixedSingle,
                        Enabled = false,
                        BackColor = Color.LightGray
                    };
                    var dtpTahminiTarih = new DateTimePicker
                    {
                        Dock = DockStyle.Fill,
                        Format = DateTimePickerFormat.Short,
                        ShowCheckBox = true,
                        Margin = new Padding(2),
                        Font = new Font("Segoe UI", 10),
                        Height = 30,
                        MaximumSize = new Size(0, 30)
                    };
                    var dtpGerceklesenTarih = new DateTimePicker
                    {
                        Dock = DockStyle.Fill,
                        Format = DateTimePickerFormat.Short,
                        ShowCheckBox = true,
                        Margin = new Padding(2),
                        Font = new Font("Segoe UI", 10),
                        Height = 30,
                        MaximumSize = new Size(0, 30)
                    };
                    var rtbAciklama = new RichTextBox
                    {
                        Text = odemeBilgi.odemeAciklama,
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 10),
                        BorderStyle = BorderStyle.FixedSingle,
                        ScrollBars = RichTextBoxScrollBars.Vertical
                    };

                    if (odemeBilgi.tahminiTarih.HasValue)
                    {
                        dtpTahminiTarih.Value = odemeBilgi.tahminiTarih.Value;
                        dtpTahminiTarih.Checked = true;
                    }
                    else
                    {
                        dtpTahminiTarih.Checked = false;
                    }
                    dtpTahminiTarih.Enabled = (odemeBilgi.kilometreTasiId == 0);

                    if (odemeBilgi.gerceklesenTarih.HasValue)
                    {
                        dtpGerceklesenTarih.Value = odemeBilgi.gerceklesenTarih.Value;
                        dtpGerceklesenTarih.Checked = true;
                    }
                    else
                    {
                        dtpGerceklesenTarih.Checked = false;
                    }

                    var pnlTeminatMektubu = new Panel
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        Height = 30
                    };
                    var chkTeminatMektubuVar = new CheckBox
                    {
                        Text = "Var",
                        Dock = DockStyle.Left,
                        Margin = new Padding(2),
                        Width = 50,
                        Font = new Font("Segoe UI", 10),
                        Enabled = odemeBilgi.odemeId > 0
                    };
                    var chkTeminatMektubuYok = new CheckBox
                    {
                        Text = "Yok",
                        Dock = DockStyle.Right,
                        Margin = new Padding(2),
                        Width = 50,
                        Font = new Font("Segoe UI", 10),
                        Enabled = odemeBilgi.odemeId > 0
                    };
                    pnlTeminatMektubu.Controls.Add(chkTeminatMektubuVar);
                    pnlTeminatMektubu.Controls.Add(chkTeminatMektubuYok);

                    chkTeminatMektubuVar.Tag = txtTutar;

                    bool teminatVar = teminatMektuplari.Any(m => m.kilometreTasiId == odemeBilgi.kilometreTasiId && m.projeId == projeId);

                    odemeBilgi.teminatMektubu = teminatVar;
                    string teminatDurumu = odemeBilgi.teminatDurumu ?? "Pasif";

                    chkTeminatMektubuVar.Checked = odemeBilgi.teminatMektubu;
                    chkTeminatMektubuYok.Checked = !odemeBilgi.teminatMektubu;

                    if (odemeBilgi.teminatMektubu)
                    {
                        chkTeminatMektubuYok.Enabled = false;
                        chkTeminatMektubuVar.Enabled = false;
                    }
                    if (chkTeminatMektubuYok.Checked)
                    {
                        chkTeminatMektubuYok.Enabled = false;
                    }
                    var newLblTeminatDurum = new Label
                    {
                        Text = teminatDurumu,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(2),
                        ForeColor = teminatDurumu == "Aktif" ? Color.Green : Color.Red,
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 12, FontStyle.Bold)
                    };

                    var btnMektupGeriAl = new Button
                    {
                        Text = "Geri Al",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Width = 75,
                        Height = 30,
                        Font = new Font("Segoe UI", 10),
                        Enabled = odemeBilgi.teminatMektubu && teminatDurumu == "Aktif"
                    };
                    btnMektupGeriAl.Click += (s, e) =>
                    {
                        var button = s as Button;
                        int rowIdx = tableLayoutPanel1.GetRow(button);
                        var lblTempTeminatDurum = GetLabelAt(rowIdx, 9);
                        var pnlTempTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(8, rowIdx) as Panel;
                        var chkTempTeminatMektubuVar = pnlTempTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");
                        var chkTempTeminatMektubuYok = pnlTempTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Yok");

                        if (lblTempTeminatDurum != null)
                        {
                            lblTempTeminatDurum.Text = "Pasif";
                            lblTempTeminatDurum.ForeColor = Color.Red;
                            button.Enabled = false;

                            odemeBilgi.teminatMektubu = true;
                            odemeBilgi.teminatDurumu = "Pasif";

                            if (chkTempTeminatMektubuVar != null)
                            {
                                chkTempTeminatMektubuVar.Checked = true;
                                chkTempTeminatMektubuVar.Enabled = false;
                            }
                            if (chkTempTeminatMektubuYok != null)
                            {
                                chkTempTeminatMektubuYok.Checked = false;
                                chkTempTeminatMektubuYok.Enabled = false;
                            }

                            _odemeSartlariService.SaveOrUpdateOdemeBilgi(odemeBilgi);
                        }
                    };

                    var lblDurum = new Label
                    {
                        Text = odemeBilgi.durum,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(2),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        Font = new Font("Segoe UI", 10)
                    };
                    if (odemeBilgi.durum == "Ödendi")
                    {
                        lblDurum.ForeColor = Color.Green;
                    }
                    else if (odemeBilgi.durum == "Bekliyor")
                    {
                        lblDurum.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblDurum.ForeColor = Color.Black;
                    }

                    tableLayoutPanel1.Controls.Add(lblKilometreTasiAdi, 2, newRowIndex);
                    tableLayoutPanel1.Controls.Add(lblOran, 3, newRowIndex);
                    tableLayoutPanel1.Controls.Add(txtTutar, 4, newRowIndex);
                    tableLayoutPanel1.Controls.Add(dtpTahminiTarih, 5, newRowIndex);
                    tableLayoutPanel1.Controls.Add(dtpGerceklesenTarih, 6, newRowIndex);
                    tableLayoutPanel1.Controls.Add(rtbAciklama, 7, newRowIndex);
                    tableLayoutPanel1.Controls.Add(pnlTeminatMektubu, 8, newRowIndex);
                    tableLayoutPanel1.Controls.Add(newLblTeminatDurum, 9, newRowIndex);
                    tableLayoutPanel1.Controls.Add(btnMektupGeriAl, 10, newRowIndex);
                    tableLayoutPanel1.Controls.Add(lblDurum, 11, newRowIndex);
                    tableLayoutPanel1.Controls.Add(txtKalanTutar, 12, newRowIndex);

                    chkTeminatMektubuVar.CheckedChanged += (s, e) =>
                    {
                        if (chkTeminatMektubuVar.Checked)
                        {
                            chkTeminatMektubuYok.Checked = false;
                            chkTeminatMektubuYok.Enabled = false;
                            var lblTempTeminatDurum = GetLabelAt(newRowIndex, 9);

                            var tagValue = chkSelect.Tag as OdemeSartlari;
                            if (tagValue != null)
                            {
                                var frm = new frmTeminatMektubuEkle(_musterilerService, null, _finansProjelerService, _projeKutukService, _teminatMektuplariService, projeId, tagValue.kilometreTasiId, txtTutar.Text);
                                if (frm.ShowDialog() == DialogResult.OK)
                                {
                                    if (lblTempTeminatDurum != null)
                                    {
                                        lblTempTeminatDurum.Text = "Aktif";
                                        lblTempTeminatDurum.ForeColor = Color.Green;
                                        btnMektupGeriAl.Enabled = true;
                                        odemeBilgi.teminatMektubu = true;
                                        odemeBilgi.teminatDurumu = "Aktif";
                                        chkTeminatMektubuVar.Enabled = false;
                                        _odemeSartlariService.SaveOrUpdateOdemeBilgi(odemeBilgi);
                                    }
                                }
                                else
                                {
                                    chkTeminatMektubuVar.Checked = false;
                                    chkTeminatMektubuYok.Checked = true;
                                    if (lblTempTeminatDurum != null)
                                    {
                                        lblTempTeminatDurum.Text = "Pasif";
                                        lblTempTeminatDurum.ForeColor = Color.Red;
                                        btnMektupGeriAl.Enabled = false;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Kilometre taşı ID'si alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                chkTeminatMektubuVar.Checked = false;
                                chkTeminatMektubuYok.Checked = true;
                                if (lblTempTeminatDurum != null)
                                {
                                    lblTempTeminatDurum.Text = "Pasif";
                                    lblTempTeminatDurum.ForeColor = Color.Red;
                                    btnMektupGeriAl.Enabled = false;
                                }
                            }
                        }
                    };

                }

                AddBottomSpacer();
                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
                UpdateBulkInvoiceButtonText();
            }
        }
        private void PbDelete_Click(object sender, EventArgs e)
        {
            var pictureBox = sender as PictureBox;
            if (pictureBox != null)
            {
                int rowIndex = tableLayoutPanel1.GetRow(pictureBox);

                if (rowIndex <= 0 || rowIndex >= tableLayoutPanel1.RowCount - 1)
                {
                    return;
                }

                var odemeBilgi = pictureBox.Tag as OdemeSartlari;

                tableLayoutPanel1.SuspendLayout();
                try
                {
                    if (odemeBilgi != null && odemeBilgi.kilometreTasiId != 0)
                    {
                        _pendingDeletions.Add((odemeBilgi.projeId, odemeBilgi.kilometreTasiId));
                        MessageBox.Show("Satır başarıyla silinmek üzere işaretlendi. Kaydet butonuna tıklayarak veritabanından silmeyi onaylayın.", "Silme Onayı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (odemeBilgi != null && odemeBilgi.kilometreTasiId == 0)
                    {
                        _newlyAddedMilestones.RemoveAll(m => m.projeId == odemeBilgi.projeId && m.kilometreTasiAdi == odemeBilgi.kilometreTasiAdi);
                        MessageBox.Show("Yeni eklenen satır başarıyla kaldırıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Silme işlemi başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    LoadOdemeBilgileri(txtProjeAra.Text.Trim());

                    btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
                }
                finally
                {
                    tableLayoutPanel1.ResumeLayout(true);
                }
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text)) { MessageBox.Show("Lütfen aranacak Proje No girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var projeNo = txtProjeAra.Text.Trim();
            projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            if (!projeId.HasValue)
            {
                MessageBox.Show("Proje bulunamadı. Lütfen geçerli bir Proje No girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var sayfaStatusEntity = _sayfaStatusService.Get(sayfaIdOdemeSartlari, projeId.Value);
            txtStatu.Text = sayfaStatusEntity?.status ?? "Başlatıldı";

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(projeId.Value);

            var projeKutuk = _projeKutukService.ProjeKutukAra(projeId.Value);

            bool isAltProje = _projeIliskiService.CheckAltProje(projeId.Value);

            if (isAltProje)
            {
                var ustProjeId = _projeIliskiService.GetUstProjeId(projeId.Value);
                if (ustProjeId.HasValue)
                {
                    var ustProjeKutuk = _projeKutukService.ProjeKutukAra(ustProjeId.Value);
                    bool ustTekil = _projeKutukService.IsFaturalamaSekliTekil(ustProjeId.Value);

                    if (ustProjeKutuk != null && ustTekil)
                    {
                        MessageBox.Show(
                            "Üst projenin Kütük Faturalama Şekli Tekil olduğundan bu projeye ödeme şekli giremezsiniz.",
                            "Hata",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                }
            }

            if (projeBilgi != null)
            {
                List<int> altProjeler = null;

                if (isAltProje)
                {
                    altProjeler = _projeIliskiService.GetAltProjeler(projeId.Value);
                }
                else if (projeKutuk != null && projeKutuk.altProjeVarMi)
                {
                    altProjeler = projeKutuk.altProjeBilgileri?.Select(s => s).ToList() ?? new List<int>();
                }

                var (toplamBedel, eksikFiyatlandirmaProjeler) = _fiyatlandirmaService.GetToplamBedel(projeId.Value, altProjeler);

                lblToplamBedelBilgi.Text = "";

                UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler);

                _pendingDeletions.Clear();
                _newlyAddedMilestones.Clear();
                LoadOdemeBilgileri(projeNo);
                UpdateTutarColumn(toplamBedel);

                decimal oranToplami = HesaplaOranToplami();
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var txtKalanTutar = GetTextBoxAt(row, 12);
                    if (txtKalanTutar != null && decimal.TryParse(txtKalanTutar.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal kalanTutar))
                    {
                        if (kalanTutar != 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                UpdateBulkInvoiceButtonText();

                btnKilometreTasiEkle.Enabled = HesaplaOranToplami() < 100;
            }
            else
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void UpdateToplamBedelUI(string projeNo, decimal toplamBedel, List<int> eksikFiyatlandirmaProjeler)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateToplamBedelUI(projeNo, toplamBedel, eksikFiyatlandirmaProjeler)));
                return;
            }

            txtToplamBedel.Text = toplamBedel.ToString("F2", CultureInfo.CurrentCulture);

            var projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            if (!projeId.HasValue)
            {
                txtToplamBedel.ForeColor = Color.Black;
                lblToplamBedelBilgi.Text = string.Empty;
                lblToplamBedelBilgi.ForeColor = Color.Black;
                txtToplamBedel.Refresh();
                lblToplamBedelBilgi.Refresh();
                return;
            }

            List<int> altProjeler = _projeIliskiService.GetAltProjeler(projeId.Value)?.ToList();

            if (altProjeler != null && altProjeler.Any())
            {
                if (eksikFiyatlandirmaProjeler != null && eksikFiyatlandirmaProjeler.Any())
                {
                    var eksikProjeNumaralari = eksikFiyatlandirmaProjeler
                        .Where(id => altProjeler.Contains(id))
                        .Select(id => _finansProjelerService.GetProjeNoById(id))
                        .Where(no => !string.IsNullOrEmpty(no))
                        .ToList();

                    if (eksikProjeNumaralari.Any())
                    {
                        txtToplamBedel.ForeColor = Color.Red;
                        lblToplamBedelBilgi.Text = $"Alt projeler: {string.Join(", ", eksikProjeNumaralari)} için fiyatlandırma bulunmamaktadır.";
                        lblToplamBedelBilgi.ForeColor = Color.Red;
                    }
                    else
                    {
                        txtToplamBedel.ForeColor = Color.Black;
                        lblToplamBedelBilgi.Text = string.Empty;
                        lblToplamBedelBilgi.ForeColor = Color.Black;
                    }
                }
                else
                {
                    txtToplamBedel.ForeColor = Color.Black;
                    lblToplamBedelBilgi.Text = string.Empty;
                    lblToplamBedelBilgi.ForeColor = Color.Black;
                }
            }
            else
            {
                if (eksikFiyatlandirmaProjeler != null && eksikFiyatlandirmaProjeler.Contains(projeId.Value))
                {
                    string projeNoById = _finansProjelerService.GetProjeNoById(projeId.Value);
                    if (!string.IsNullOrEmpty(projeNoById))
                    {
                        txtToplamBedel.ForeColor = Color.Red;
                        lblToplamBedelBilgi.Text = $"Proje: {projeNoById} için fiyatlandırma bulunmamaktadır.";
                        lblToplamBedelBilgi.ForeColor = Color.Red;
                    }
                    else
                    {
                        txtToplamBedel.ForeColor = Color.Black;
                        lblToplamBedelBilgi.Text = string.Empty;
                        lblToplamBedelBilgi.ForeColor = Color.Black;
                    }
                }
                else
                {
                    txtToplamBedel.ForeColor = Color.Black;
                    lblToplamBedelBilgi.Text = string.Empty;
                    lblToplamBedelBilgi.ForeColor = Color.Black;
                }
            }

            txtToplamBedel.Refresh();
            lblToplamBedelBilgi.Refresh();
        }
        public void UpdateToplamBedelUI(decimal toplamBedel)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateToplamBedelUI(toplamBedel);
                }));
            }
            else
            {
                txtToplamBedel.Text = toplamBedel.ToString("F2");
                txtToplamBedel.ForeColor = Color.Black;
            }
        }

        private void ctlOdemeSartlari_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Ödeme Şartları";
            AddHeaderRow();
            UpdateBulkInvoiceButtonText();
        }

        private void AddHeaderRow()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.RowCount = 0;

                tableLayoutPanel1.RowCount = 1;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                int headerRowIndex = 0;

                string[] headers = {
                    "",
                    "Sil",
                    "Proje Kilometre Taşları",
                    "Oran (%)",
                    "Tutar",
                    "Tahmini Tarih",
                    "Gerçekleşen Tarih",
                    "Açıklama",
                    "Teminat Mektubu",
                    "Teminat Durumu",
                    "Mektup Geri Al",
                    "Durum",
                    "Kalan Tutar"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    var lbl = new Label
                    {
                        Text = headers[i],
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        AutoSize = false,
                        Height = 30,
                        MaximumSize = new Size(0, 30),
                        BackColor = Color.LightGray,
                    };
                    tableLayoutPanel1.Controls.Add(lbl, i, headerRowIndex);
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }
        private void AddBottomSpacer()
        {
            tableLayoutPanel1.SuspendLayout();
            try
            {
                if (tableLayoutPanel1.RowCount > 1)
                {
                    Control lastControlInFirstCol = tableLayoutPanel1.GetControlFromPosition(0, tableLayoutPanel1.RowCount - 1);
                    if (lastControlInFirstCol is Label lbl && string.IsNullOrEmpty(lbl.Text) && lbl.Height == 10)
                    {
                        return;
                    }
                }

                tableLayoutPanel1.RowCount++;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));
                int newSpacerRowIndex = tableLayoutPanel1.RowCount - 1;

                for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                {
                    var emptyLabel = new Label
                    {
                        Text = "",
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        AutoSize = false,
                        Height = 10,
                        Font = new Font("Segoe UI", 8)
                    };
                    tableLayoutPanel1.Controls.Add(emptyLabel, col, newSpacerRowIndex);
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private decimal HesaplaOranToplami()
        {
            decimal oranToplami = 0;
            var culture = CultureInfo.CurrentCulture;

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                var lblOran = GetLabelAt(row, 3);
                if (lblOran != null && decimal.TryParse(lblOran.Text.Replace("%", "").Trim(),
                    NumberStyles.Any, culture, out decimal oran))
                {
                    oranToplami += oran;
                }
            }
            return oranToplami;
        }

        private bool ParseToplamBedel(out decimal toplamBedel)
        {
            toplamBedel = 0;
            if (string.IsNullOrWhiteSpace(txtToplamBedel.Text))
                return false;

            string cleanText = txtToplamBedel.Text.Split('(')[0].Trim();

            return decimal.TryParse(cleanText, NumberStyles.Any, new CultureInfo("tr-TR"), out toplamBedel);
        }

        private void UpdateTutarColumn(decimal toplamBedel)
        {
            var culture = new CultureInfo("tr-TR");

            tableLayoutPanel1.SuspendLayout();
            try
            {
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var lblOran = GetLabelAt(row, 3);
                    var txtTutar = GetTextBoxAt(row, 4);
                    var txtKalanTutar = GetTextBoxAt(row, 13);

                    if (lblOran != null && txtTutar != null && txtKalanTutar != null)
                    {
                        string oranText = lblOran.Text.Replace("%", "").Trim();
                        if (decimal.TryParse(oranText,
                                           NumberStyles.Any,
                                           culture,
                                           out decimal oran))
                        {
                            decimal tutar = (toplamBedel * oran) / 100m;
                            string formattedTutar = Math.Round(tutar, 2).ToString("N2", culture);
                            txtTutar.Text = formattedTutar;
                        }
                    }
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }
        }

        private TextBox GetTextBoxAt(int row, int col)
        {
            var ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
            return ctl as TextBox;
        }

        private Label GetLabelAt(int row, int col)
        {
            var ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
            return ctl as Label;
        }

        private RichTextBox GetRichTextBoxAt(int row, int col)
        {
            var ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
            return ctl as RichTextBox;
        }

        private DateTimePicker GetDateTimePickerAt(int row, int col)
        {
            var ctl = tableLayoutPanel1.GetControlFromPosition(col, row);
            return ctl as DateTimePicker;
        }
        private void btnKilometreTasiEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen önce bir proje seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projeNo = txtProjeAra.Text.Trim();
            var projeKutuk = _projeKutukService.ProjeKutukAra(projeId.Value);
            bool isAltProje = _projeIliskiService.CheckAltProje(projeId.Value);

            if (!ParseToplamBedel(out decimal toplamBedel) || toplamBedel == 0)
            {
                MessageBox.Show("Projeye ait fiyatlandırma bulunmadığından ödeme bilgisi girilemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isAltProje && projeKutuk != null && projeKutuk.altProjeVarMi)
            {
                var altProjeler = projeKutuk.altProjeBilgileri?.ToList() ?? new List<int>();
                var (_, eksikFiyatlandirmaProjeler) = _fiyatlandirmaService.GetToplamBedel(projeId.Value, altProjeler);

                var eksikAltProjeler = eksikFiyatlandirmaProjeler
                    .Where(id => !id.Equals(projeId.Value))
                    .ToList();

                if (eksikAltProjeler.Any())
                {
                    var eksikProjeNumaralari = eksikAltProjeler
                        .Select(id => _finansProjelerService.GetProjeNoById(id))
                        .Where(no => !string.IsNullOrEmpty(no))
                        .ToList();
                    MessageBox.Show($"Üst proje için kilometre taşı eklenemez. Şu alt projeler için fiyatlandırma eksik: {string.Join(", ", eksikProjeNumaralari)}",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            List<string> alreadySelectedMilestones = new List<string>();

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                Label lblKilometreTasiAdi = tableLayoutPanel1.GetControlFromPosition(2, row) as Label;
                if (lblKilometreTasiAdi != null && !string.IsNullOrWhiteSpace(lblKilometreTasiAdi.Text))
                {
                    alreadySelectedMilestones.Add(lblKilometreTasiAdi.Text.Trim());
                }
            }

            using (var frm = new frmYeniKilometreTasi(_kilometreTaslariService, alreadySelectedMilestones))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(frm.KilometreTasiAdi) && !string.IsNullOrEmpty(frm.GirilenDeger))
                    {
                        decimal mevcutOranToplami = HesaplaOranToplami();
                        decimal yeniOran;
                        decimal tutar;

                        if (frm.TutarIleGirildi)
                        {
                            if (!decimal.TryParse(frm.GirilenDeger, NumberStyles.Any, CultureInfo.CurrentCulture, out tutar))
                            {
                                MessageBox.Show("Geçersiz tutar formatı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            if (tutar <= 0 || tutar > toplamBedel)
                            {
                                MessageBox.Show("Girilen tutar 0'dan büyük ve toplam bedelden küçük/eşit olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            yeniOran = (tutar / toplamBedel) * 100m;
                        }
                        else
                        {
                            string cleanOran = frm.GirilenDeger.Replace("%", "").Trim();
                            if (!decimal.TryParse(cleanOran, NumberStyles.Any, CultureInfo.CurrentCulture, out yeniOran))
                            {
                                MessageBox.Show("Geçersiz oran formatı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            tutar = (toplamBedel * yeniOran) / 100m;
                        }

                        if (mevcutOranToplami + yeniOran > 100)
                        {
                            MessageBox.Show($"Oran toplamı %100'ü aşıyor (Mevcut: {mevcutOranToplami}, Yeni: {yeniOran}). Yeni kilometre taşı eklenemez.",
                                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        OdemeSartlari newMilestone = new OdemeSartlari
                        {
                            projeId = projeId.Value,
                            kilometreTasiId = 0,
                            kilometreTasiAdi = frm.KilometreTasiAdi,
                            oran = yeniOran,
                            tutar = tutar,
                            kalanTutar = tutar,
                            tahminiTarih = null,
                            gerceklesenTarih = null,
                            odemeAciklama = "",
                            teminatMektubu = false,
                            teminatDurumu = "Pasif",
                            durum = "Bekliyor"
                        };

                        _newlyAddedMilestones.Add(newMilestone);
                        LoadOdemeBilgileri(txtProjeAra.Text.Trim());
                        UpdateBulkInvoiceButtonText();
                    }
                }
            }
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var projeNo = txtProjeAra.Text.Trim();
            var proje = _finansProjelerService.GetProjeBilgileri(projeId.Value);
            if (proje == null)
            {
                MessageBox.Show("Proje bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                foreach (var itemToDelete in _pendingDeletions.ToList())
                {
                    try
                    {
                        _odemeSartlariService.DeleteOdemeBilgi(itemToDelete.projeId, itemToDelete.kilometreTasiId);
                        _pendingDeletions.Remove(itemToDelete);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Silme işlemi başarısız (ProjeId: {itemToDelete.projeId}, KilometreTasiId: {itemToDelete.kilometreTasiId}): İlişkili ödeme hareketleri nedeniyle silme işlemi yapılamadı. Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                bool tumSatirlarTamam = true;
                decimal oranToplami = HesaplaOranToplami();
                decimal toplamKalanTutar = 0m;
                bool tumSatirlarDolu = true;
                List<string> nedenTamamlanmadiList = new List<string>();

                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var lblKilometreTasiAdi = GetLabelAt(row, 2);
                    var lblOran = GetLabelAt(row, 3);
                    var txtTutar = GetTextBoxAt(row, 4);
                    var txtKalanTutar = GetTextBoxAt(row, 12);
                    var dtpTahminiTarih = GetDateTimePickerAt(row, 5);
                    var dtpGerceklesenTarih = GetDateTimePickerAt(row, 6);
                    var rtbAciklama = GetRichTextBoxAt(row, 7);
                    var lblDurum = GetLabelAt(row, 11);
                    var pnlTeminatMektubu = tableLayoutPanel1.GetControlFromPosition(8, row) as Panel;
                    var chkTeminatMektubuVar = pnlTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Var");
                    var chkTeminatMektubuYok = pnlTeminatMektubu?.Controls.OfType<CheckBox>().FirstOrDefault(c => c.Text == "Yok");
                    var lblTeminatDurum = GetLabelAt(row, 9);

                    if (string.IsNullOrWhiteSpace(txtTutar?.Text) ||
                        string.IsNullOrWhiteSpace(txtKalanTutar?.Text) ||
                        !dtpTahminiTarih.Checked)
                    {
                        MessageBox.Show($"Projenin bazı zorunlu alanları boş bırakılamaz.\n\nEksik alanlar: {(string.IsNullOrWhiteSpace(txtTutar?.Text) ? "Tutar, " : "")}{(string.IsNullOrWhiteSpace(txtKalanTutar?.Text) ? "Kalan Tutar, " : "")}{(!dtpTahminiTarih.Checked ? "Tahmini Tarih" : "")}".TrimEnd(' ', ','), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    bool satirDolu = dtpGerceklesenTarih != null && dtpGerceklesenTarih.Checked &&
                                     rtbAciklama != null && !string.IsNullOrWhiteSpace(rtbAciklama.Text);

                    string kilometreTasiAdi = lblKilometreTasiAdi?.Text;

                    if (!satirDolu)
                    {
                        var eksikler = new List<string>();

                        if (!dtpGerceklesenTarih.Checked)
                            eksikler.Add("Gerçekleşen Tarih");
                        if (string.IsNullOrWhiteSpace(rtbAciklama?.Text))
                            eksikler.Add("Açıklama");

                        if (eksikler.Any())
                        {
                            nedenTamamlanmadiList.Add(
                                $"Projenin {row}. satırına ({kilometreTasiAdi}) bazı bilgiler girilmedi: '{string.Join(" ve ", eksikler)}' eksik"
                            );
                            tumSatirlarDolu = false;
                        }
                    }

                    int kilometreTasiId = _kilometreTaslariService.GetKilometreTasiId(kilometreTasiAdi);
                    if (kilometreTasiId == 0)
                    {
                        MessageBox.Show($"Kilometre taşı '{kilometreTasiAdi}' için ID bulunamadı. Lütfen önce kilometre taşını tanımlayın veya geçerli bir kilometre taşı seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string oranText = lblOran?.Text.Replace("%", "").Trim();
                    if (!decimal.TryParse(oranText, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal oran))
                    {
                        MessageBox.Show($"Geçersiz oran formatı: {oranText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string tutarText = txtTutar?.Text.Trim();
                    if (!decimal.TryParse(tutarText, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out decimal tutar))
                    {
                        MessageBox.Show($"Geçersiz tutar formatı: {tutarText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string kalanTutarText = txtKalanTutar?.Text.Trim();
                    decimal? kalanTutar = null;
                    if (!string.IsNullOrEmpty(kalanTutarText) && decimal.TryParse(kalanTutarText, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out decimal parsedKalanTutar))
                    {
                        kalanTutar = parsedKalanTutar;
                        toplamKalanTutar += parsedKalanTutar;
                        if (parsedKalanTutar != 0)
                        {
                            nedenTamamlanmadiList.Add($"Projenin {row}. satırına ({kilometreTasiAdi}) ödeme yapılmamış");
                        }
                    }

                    string tahminiTarih = dtpTahminiTarih?.Checked ?? false ? dtpTahminiTarih.Value.ToString("yyyy-MM-dd") : null;
                    string gerceklesenTarih = dtpGerceklesenTarih?.Checked ?? false ? dtpGerceklesenTarih.Value.ToString("yyyy-MM-dd") : null;
                    string aciklama = rtbAciklama?.Text;
                    string durum = lblDurum?.Text;
                    int siralama = row;

                    bool teminatMektubu = chkTeminatMektubuVar != null && chkTeminatMektubuVar.Checked;
                    string teminatDurumu = lblTeminatDurum?.Text ?? "Pasif";
                    string faturaNo = _odemeSartlariService.GetFaturaNo(projeId.Value, kilometreTasiId) ?? "";

                    string odemeTarihi = null;
                    var existingOdemeBilgi = _odemeSartlariService.GetOdemeBilgi(projeNo, kilometreTasiId);
                    if (kalanTutar == 0 && durum == "Ödendi")
                    {
                        var relatedOdemeHareketi = _odemeHareketleri
                            .Where(oh => oh.odemeId == existingOdemeBilgi?.odemeId)
                            .OrderByDescending(oh => oh.odemeTarihi)
                            .FirstOrDefault();

                        if (relatedOdemeHareketi != null)
                        {
                            odemeTarihi = relatedOdemeHareketi.odemeTarihi.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            var dbOdemeHareketleri = _odemeHareketleriService.GetOdemeHareketleriByOdemeId(existingOdemeBilgi?.odemeId ?? 0);
                            var dbRelatedOdemeHareketi = dbOdemeHareketleri
                                .OrderByDescending(oh => oh.odemeTarihi)
                                .FirstOrDefault();

                            if (dbRelatedOdemeHareketi != null)
                            {
                                odemeTarihi = dbRelatedOdemeHareketi.odemeTarihi.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                MessageBox.Show($"Satır {row}: Ödeme durumu 'Ödendi' ancak ödeme hareketi bulunamadı. Lütfen ödeme hareketini ekleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    else if (existingOdemeBilgi != null && existingOdemeBilgi.odemeTarihi != null)
                    {
                        odemeTarihi = existingOdemeBilgi.odemeTarihi.Value.ToString("yyyy-MM-dd");
                    }

                    if (durum == "Ödendi" && string.IsNullOrEmpty(odemeTarihi))
                    {
                        MessageBox.Show($"Satır {row}: Ödeme durumu 'Ödendi' seçiliyken ödeme tarihi belirtilmelidir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    bool satirTamamMi = (satirDolu && (kalanTutar == 0));
                    string satirStatus = satirTamamMi ? "Kapatıldı" : "Başlatıldı";

                    if (!satirTamamMi)
                    {
                        tumSatirlarTamam = false;
                    }

                    var odemeSartlari = new OdemeSartlari
                    {
                        projeId = projeId.Value,
                        kilometreTasiId = kilometreTasiId,
                        siralama = siralama,
                        oran = oran,
                        tutar = tutar,
                        kalanTutar = kalanTutar ?? 0,
                        tahminiTarih = dtpTahminiTarih?.Checked ?? false ? dtpTahminiTarih.Value : (DateTime?)null,
                        gerceklesenTarih = dtpGerceklesenTarih?.Checked ?? false ? dtpGerceklesenTarih.Value : (DateTime?)null,
                        odemeAciklama = aciklama,
                        teminatMektubu = teminatMektubu,
                        teminatDurumu = teminatDurumu,
                        durum = durum,
                        faturaNo = faturaNo,
                        odemeTarihi = !string.IsNullOrEmpty(odemeTarihi) ? DateTime.ParseExact(odemeTarihi, "yyyy-MM-dd", CultureInfo.InvariantCulture) : (DateTime?)null,
                        status = satirStatus
                    };

                    try
                    {
                        _odemeSartlariService.SaveOrUpdateOdemeBilgi(odemeSartlari);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ödeme bilgisi kaydedilirken hata (ProjeId: {odemeSartlari.projeId}, KilometreTasiId: {odemeSartlari.kilometreTasiId}): {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (oranToplami != 100)
                {
                    nedenTamamlanmadiList.Add("Proje oranları tamamlanmamış");
                }

                bool tumSatirlarKalanTutarSifir = true;
                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    var txtKalanTutar = GetTextBoxAt(row, 12);
                    string kalanTutarText = txtKalanTutar?.Text.Trim();

                    if (!string.IsNullOrEmpty(kalanTutarText) &&
                        decimal.TryParse(kalanTutarText, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out decimal parsedKalanTutar))
                    {
                        if (parsedKalanTutar != 0)
                            tumSatirlarKalanTutarSifir = false;
                    }
                    else
                    {
                        tumSatirlarKalanTutarSifir = false;
                    }
                }

                bool bilgilerTamamMi = tumSatirlarDolu && tumSatirlarKalanTutarSifir;
                string sayfaStatus = bilgilerTamamMi ? "Kapatıldı" : "Başlatıldı";

                txtStatu.Text = sayfaStatus;

                string nedenTamamlanmadi = nedenTamamlanmadiList.Any() ? string.Join("; ", nedenTamamlanmadiList) : string.Empty;

                var mevcutSayfaStatus = _sayfaStatusService.Get(sayfaId: sayfaIdOdemeSartlari, projeId.Value);
                if (mevcutSayfaStatus != null)
                {
                    mevcutSayfaStatus.status = sayfaStatus;
                    mevcutSayfaStatus.bilgilerTamamMi = bilgilerTamamMi;
                    mevcutSayfaStatus.nedenTamamlanmadi = nedenTamamlanmadi;
                    try
                    {
                        _sayfaStatusService.Update(mevcutSayfaStatus);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"SayfaStatus güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    var yeniStatus = new SayfaStatus
                    {
                        sayfaId = sayfaIdOdemeSartlari,
                        projeId = projeId.Value,
                        status = sayfaStatus,
                        bilgilerTamamMi = bilgilerTamamMi,
                        nedenTamamlanmadi = nedenTamamlanmadi
                    };
                    try
                    {
                        _sayfaStatusService.Insert(yeniStatus);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"SayfaStatus eklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (_odemeHareketleri.Any())
                {
                    foreach (var odemeHareketi in _odemeHareketleri.ToList())
                    {
                        try
                        {
                            _odemeHareketleriService.SaveOdemeHareketi(odemeHareketi);
                            _odemeHareketleri.Remove(odemeHareketi);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ödeme hareketi kaydedilirken hata (OdemeId: {odemeHareketi.odemeId}): {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    _odemeHareketleri.Clear();
                }
                _projeKutukService.UpdateProjeKutukDurum(projeId.Value, null);

                MessageBox.Show("Ödeme bilgileri başarıyla kaydedildi/güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _newlyAddedMilestones.Clear();
                LoadOdemeBilgileri(projeNo);
                UpdateBulkInvoiceButtonText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ödeme bilgileri kaydedilirken/güncellenirken genel hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBulkInvoiceButtonText();
        }

        private void UpdateBulkInvoiceButtonText()
        {
            int checkedCount = 0;
            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                CheckBox chk = tableLayoutPanel1.GetControlFromPosition(0, row) as CheckBox;
                if (chk != null && chk.Checked)
                {
                    checkedCount++;
                }
            }

            if (checkedCount >= 1)
            {
                btnTopluFaturaOlustur.Text = "Seçili Satırları Faturala";
            }
            else
            {
                btnTopluFaturaOlustur.Text = "Toplu Fatura Oluştur";
            }
        }

        private void btnTopluFaturaOlustur_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<int> selectedRowIndexes = new List<int>();
            decimal totalTutar = 0m;
            string proje = txtProjeAra.Text.Trim();
            var culture = CultureInfo.CurrentCulture;
            List<int> kilometreTasiIds = new List<int>();
            List<int?> odemeIds = new List<int?>();

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                CheckBox chk = tableLayoutPanel1.GetControlFromPosition(0, row) as CheckBox;
                if (chk != null && chk.Checked)
                {
                    selectedRowIndexes.Add(row);
                }
            }

            if (selectedRowIndexes.Count == 1)
            {
                int rowIndex = selectedRowIndexes[0];

                var lblKmTasiAdi = GetLabelAt(rowIndex, 2);
                var txtTutarFatura = GetTextBoxAt(rowIndex, 4);
                var rtbAciklamaFatura = tableLayoutPanel1.GetControlFromPosition(7, rowIndex) as RichTextBox;
                var dtpTahminiTarihFatura = tableLayoutPanel1.GetControlFromPosition(5, rowIndex) as DateTimePicker;
                var dtpGerceklesenTarihFatura = tableLayoutPanel1.GetControlFromPosition(6, rowIndex) as DateTimePicker;
                var checkBox = tableLayoutPanel1.GetControlFromPosition(0, rowIndex) as CheckBox;

                if (lblKmTasiAdi == null || txtTutarFatura == null || rtbAciklamaFatura == null ||
                    string.IsNullOrWhiteSpace(lblKmTasiAdi.Text) ||
                    string.IsNullOrWhiteSpace(txtTutarFatura.Text) ||
                    string.IsNullOrWhiteSpace(rtbAciklamaFatura.Text))
                {
                    MessageBox.Show("Seçili satırda gerekli bilgiler eksik veya boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedTarih = "Belirtilmemiş";
                if (dtpTahminiTarihFatura != null && dtpGerceklesenTarihFatura != null)
                {
                    if (dtpTahminiTarihFatura.Checked && dtpGerceklesenTarihFatura.Checked)
                    {
                        selectedTarih = dtpTahminiTarihFatura.Value.Date == dtpGerceklesenTarihFatura.Value.Date
                            ? dtpTahminiTarihFatura.Value.ToString("yyyy-MM-dd")
                            : dtpGerceklesenTarihFatura.Value.ToString("yyyy-MM-dd");
                    }
                    else if (dtpTahminiTarihFatura.Checked)
                    {
                        selectedTarih = dtpTahminiTarihFatura.Value.ToString("yyyy-MM-dd");
                    }
                    else if (dtpGerceklesenTarihFatura.Checked)
                    {
                        selectedTarih = dtpGerceklesenTarihFatura.Value.ToString("yyyy-MM-dd");
                    }
                }

                int? odemeId = null;
                int kilometreTasiId = _kilometreTaslariService.GetKilometreTasiId(lblKmTasiAdi.Text);

                if (checkBox != null && checkBox.Tag is OdemeSartlari odemeBilgi)
                {
                    odemeId = odemeBilgi.odemeId;
                }
                else
                {
                    var odemeInfo = _odemeSartlariService.GetOdemeBilgileriByProjeId(projeId.Value)
                        .FirstOrDefault(o => o.kilometreTasiId == kilometreTasiId);
                    odemeId = odemeInfo?.odemeId;
                }

                if (!odemeId.HasValue || odemeId <= 0)
                {
                    MessageBox.Show($"Satır {rowIndex} için ödeme ID'si bulunamadı. Lütfen önce 'Kaydet' butonuna basarak ödeme bilgilerini kaydedin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var frm = new frmFaturaOlustur(
                    _odemeSartlariService,
                    _musterilerService,
                    _finansProjelerService,
                    _projeIliskiService,
                    _projeKutukService,
                    tutar: txtTutarFatura.Text,
                    aciklama: rtbAciklamaFatura.Text,
                    tarih: selectedTarih,
                    proje: proje,
                    kilometreTasiId: kilometreTasiId,
                    odemeId: odemeId
                );
                frm.ShowDialog();
            }
            else
            {
                string combinedDescriptions = "";
                string combinedAmounts = "";

                int count = Math.Max(tableLayoutPanel1.RowCount - 2, 0);
                List<int> rowsToProcess = selectedRowIndexes.Count > 0
                    ? selectedRowIndexes
                    : Enumerable.Range(1, count).ToList();


                foreach (int rowIndex in rowsToProcess)
                {
                    Label lblKmTasiAdi = GetLabelAt(rowIndex, 2);
                    TextBox txtTutar = GetTextBoxAt(rowIndex, 4);
                    var checkBox = tableLayoutPanel1.GetControlFromPosition(0, rowIndex) as CheckBox;
                    int kilometreTasiId = _kilometreTaslariService.GetKilometreTasiId(lblKmTasiAdi.Text);

                    int? odemeId = null;
                    if (checkBox != null && checkBox.Tag is OdemeSartlari odemeBilgi)
                    {
                        odemeId = odemeBilgi.odemeId;
                    }
                    else
                    {
                        var odemeInfo = _odemeSartlariService.GetOdemeBilgileriByProjeId(projeId.Value)
                            .FirstOrDefault(o => o.kilometreTasiId == kilometreTasiId);
                        odemeId = odemeInfo?.odemeId;
                    }

                    if (odemeId.HasValue && odemeId > 0)
                    {
                        kilometreTasiIds.Add(kilometreTasiId);
                        odemeIds.Add(odemeId);
                    }

                    if (txtTutar != null && decimal.TryParse(txtTutar.Text, NumberStyles.Currency | NumberStyles.Number, culture, out decimal tutar))
                    {
                        totalTutar += tutar;
                        combinedAmounts += tutar.ToString("N2", culture) + "\\n";
                    }

                    if (lblKmTasiAdi != null && !string.IsNullOrWhiteSpace(lblKmTasiAdi.Text))
                    {
                        combinedDescriptions += lblKmTasiAdi.Text.Trim() + "\\n";
                    }
                }

                if (combinedDescriptions.EndsWith("\\n"))
                    combinedDescriptions = combinedDescriptions.Substring(0, combinedDescriptions.Length - 2);

                if (combinedAmounts.EndsWith("\\n"))
                    combinedAmounts = combinedAmounts.Substring(0, combinedAmounts.Length - 2);

                if (odemeIds.Count == 0)
                {
                    MessageBox.Show("Seçili satırlara ait ödeme ID'leri bulunamadı. Lütfen önce 'Kaydet' butonuna basarak ödeme bilgilerini kaydedin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var frm = new frmFaturaOlustur(
                    _odemeSartlariService,
                    _musterilerService,
                    _finansProjelerService,
                    _projeIliskiService,
                    _projeKutukService,
                    tutar: combinedAmounts,
                    aciklama: combinedDescriptions,
                    tarih: "",
                    proje: proje,
                    kilometreTasiIds: kilometreTasiIds,
                    odemeIds: odemeIds
                );
                frm.ShowDialog();
            }

            UpdateBulkInvoiceButtonText();
        }
        private void btnHesapla_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen önce bir proje seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tableLayoutPanel1.RowCount <= 2)
            {
                MessageBox.Show("Hiç kilometre taşı eklenmemiş. Lütfen önce kilometre taşı ekleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEksilenTutar.Text))
            {
                MessageBox.Show("Lütfen çıkarılacak tutarı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var culture = CultureInfo.CurrentCulture;
            if (!decimal.TryParse(txtEksilenTutar.Text, NumberStyles.Any, culture, out decimal herSatirdanDusecekTutar))
            {
                MessageBox.Show("Geçersiz tutar formatı. Lütfen sayısal bir değer girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(rtxtAciklama.Text))
            {
                MessageBox.Show("Lütfen ödeme için bir açıklama girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!dtOdemeTarihi.Checked)
            {
                MessageBox.Show("Lütfen bir ödeme tarihi seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string aciklama = rtxtAciklama.Text.Trim();
            if (string.IsNullOrEmpty(aciklama))
            {
                aciklama = "Otomatik ödeme düşümü";
            }

            List<int> selectedRowIndexes = new List<int>();
            decimal toplamKalanTutar = 0m;

            for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
            {
                CheckBox chk = tableLayoutPanel1.GetControlFromPosition(0, row) as CheckBox;
                TextBox txtKalanTutar = GetTextBoxAt(row, 12);
                if (txtKalanTutar != null && decimal.TryParse(txtKalanTutar.Text, NumberStyles.Any, culture, out decimal kalanTutar))
                {
                    if (chk != null && chk.Checked)
                    {
                        selectedRowIndexes.Add(row);
                        toplamKalanTutar += kalanTutar;
                    }
                    else if (selectedRowIndexes.Count == 0)
                    {
                        toplamKalanTutar += kalanTutar;
                    }
                }
            }

            if (selectedRowIndexes.Count > 0)
            {
                if (herSatirdanDusecekTutar > toplamKalanTutar)
                {
                    MessageBox.Show($"Girilen tutar ({herSatirdanDusecekTutar:N2}) seçili satırların toplam kalan tutarından ({toplamKalanTutar:N2}) büyük. İşlem iptal edildi.",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                if (herSatirdanDusecekTutar > toplamKalanTutar)
                {
                    MessageBox.Show($"Girilen tutar ({herSatirdanDusecekTutar:N2}) tüm satırların toplam kalan tutarından ({toplamKalanTutar:N2}) büyük. İşlem iptal edildi.",
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            _odemeHareketleri.Clear();

            if (selectedRowIndexes.Count == 0)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Hiçbir satır seçilmedi. Girilen tutar tüm satırlardaki 'Kalan Tutar' alanlarından çıkarılacaktır. Devam etmek istiyor musunuz?",
                    "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    selectedRowIndexes.Add(row);
                }
            }

            tableLayoutPanel1.SuspendLayout();
            try
            {
                decimal cikarilacakDeger = herSatirdanDusecekTutar;

                foreach (int rowIndex in selectedRowIndexes)
                {
                    TextBox txtKalanTutar = GetTextBoxAt(rowIndex, 12);
                    Label lblDurum = GetLabelAt(rowIndex, 11);
                    var odemeBilgi = GetCheckBoxAt(rowIndex, 0).Tag as OdemeSartlari;
                    if (odemeBilgi == null || odemeBilgi.odemeId <= 0)
                    {
                        MessageBox.Show($"Seçili satırın bir ödeme ID'si bulunmuyor. Lütfen önce Kaydet butonuna basın. Satır: {rowIndex}", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (txtKalanTutar != null)
                    {
                        decimal currentKalanTutar = 0m;
                        if (decimal.TryParse(txtKalanTutar.Text, NumberStyles.Any, culture, out currentKalanTutar))
                        {
                            decimal odemeMiktari = Math.Min(currentKalanTutar, herSatirdanDusecekTutar);
                            decimal newKalanTutar = currentKalanTutar - odemeMiktari;

                            if (newKalanTutar < 0)
                            {
                                MessageBox.Show($"Satır {rowIndex}'deki 'Kalan Tutar' değeri çıkarılan tutardan daha küçük olamaz. İşlem iptal edildi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            txtKalanTutar.Text = newKalanTutar.ToString("N2", culture);

                            _odemeHareketleri.Add(new OdemeHareketleri
                            {
                                odemeId = odemeBilgi.odemeId,
                                odemeMiktari = odemeMiktari,
                                odemeTarihi = dtOdemeTarihi.Value,
                                odemeAciklama = aciklama
                            });

                            if (newKalanTutar == 0 && lblDurum != null)
                            {
                                lblDurum.Text = "Ödendi";
                                lblDurum.ForeColor = Color.Green;

                                odemeBilgi.durum = "Ödendi";
                                odemeBilgi.kalanTutar = newKalanTutar;
                                odemeBilgi.odemeTarihi = dtOdemeTarihi.Value;
                            }
                        }
                        else
                        {
                            TextBox txtTutar = GetTextBoxAt(rowIndex, 4);
                            if (txtTutar != null && decimal.TryParse(txtTutar.Text, NumberStyles.Any, culture, out decimal currentTutar))
                            {
                                decimal odemeMiktari = Math.Min(currentTutar, herSatirdanDusecekTutar);
                                decimal newKalanTutar = currentTutar - odemeMiktari;

                                if (newKalanTutar < 0)
                                {
                                    MessageBox.Show($"Satır {rowIndex}'deki 'Tutar' değeri çıkarılan tutardan daha küçük olamaz. İşlem iptal edildi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                txtKalanTutar.Text = newKalanTutar.ToString("N2", culture);

                                _odemeHareketleri.Add(new OdemeHareketleri
                                {
                                    odemeId = odemeBilgi.odemeId,
                                    odemeMiktari = odemeMiktari,
                                    odemeTarihi = dtOdemeTarihi.Value,
                                    odemeAciklama = aciklama
                                });

                                if (newKalanTutar == 0 && lblDurum != null)
                                {
                                    lblDurum.Text = "Ödendi";
                                    lblDurum.ForeColor = Color.Green;

                                    odemeBilgi.durum = "Ödendi";
                                    odemeBilgi.kalanTutar = newKalanTutar;
                                    odemeBilgi.odemeTarihi = dtOdemeTarihi.Value;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
            }

            MessageBox.Show("Tutar çıkarma işlemi tamamlandı. Kaydetmek için 'Kaydet' butonuna basın.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private CheckBox GetCheckBoxAt(int row, int column)
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (tableLayoutPanel1.GetRow(control) == row && tableLayoutPanel1.GetColumn(control) == column && control is CheckBox)
                {
                    return (CheckBox)control;
                }
            }
            return null;
        }

        private void ChkTutarTamaminiKullan_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTutarTamaminiKullan.Checked)
            {
                decimal totalKalanTutar = 0m;
                var culture = CultureInfo.CurrentCulture;
                List<int> selectedRowIndexes = new List<int>();

                for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                {
                    CheckBox chk = tableLayoutPanel1.GetControlFromPosition(0, row) as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        selectedRowIndexes.Add(row);
                    }
                }

                if (selectedRowIndexes.Count > 0)
                {
                    foreach (int rowIndex in selectedRowIndexes)
                    {
                        TextBox txtKalanTutar = GetTextBoxAt(rowIndex, 12);
                        if (txtKalanTutar != null && decimal.TryParse(txtKalanTutar.Text, NumberStyles.Any, culture, out decimal kalanTutar))
                        {
                            totalKalanTutar += kalanTutar;
                        }
                    }
                }
                else
                {
                    for (int row = 1; row < tableLayoutPanel1.RowCount - 1; row++)
                    {
                        TextBox txtKalanTutar = GetTextBoxAt(row, 12);
                        if (txtKalanTutar != null && decimal.TryParse(txtKalanTutar.Text, NumberStyles.Any, culture, out decimal kalanTutar))
                        {
                            totalKalanTutar += kalanTutar;
                        }
                    }
                }
                txtEksilenTutar.Text = totalKalanTutar.ToString("N2", culture);
            }
        }

        private void TxtEksilenTutar_TextChanged(object sender, EventArgs e)
        {
            if (chkTutarTamaminiKullan.Checked)
            {
                chkTutarTamaminiKullan.CheckedChanged -= ChkTutarTamaminiKullan_CheckedChanged;
                chkTutarTamaminiKullan.Checked = false;
                chkTutarTamaminiKullan.CheckedChanged += ChkTutarTamaminiKullan_CheckedChanged;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string projeNo = txtProjeAra.Text.Trim();
            if (string.IsNullOrEmpty(projeNo))
            {
                MessageBox.Show("Lütfen bir proje numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{projeNo}' için projeId bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var projeBilgi = _finansProjelerService.GetProjeBilgileri(projeId.Value);
            if (projeBilgi == null)
            {
                MessageBox.Show($"Proje '{projeNo}' bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show($"Proje '{projeNo}' için tüm ödeme şartları kayıtları silinecek. Onaylıyor musunuz?", "Ödeme Şartları Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            if (_odemeSartlariService.OdemeSartlariSil(projeId.Value))
            {
                var mevcutSayfaStatus = _sayfaStatusService.Get(sayfaId: sayfaIdOdemeSartlari, projeId.Value);
                if (mevcutSayfaStatus != null)
                {
                    mevcutSayfaStatus.status = "Başlatıldı";
                    mevcutSayfaStatus.bilgilerTamamMi = false;
                    mevcutSayfaStatus.nedenTamamlanmadi = "Ödeme Şartları bilgileri girilmemiş";
                    _sayfaStatusService.Update(mevcutSayfaStatus);
                }
                else
                {
                    var yeniStatus = new SayfaStatus
                    {
                        sayfaId = sayfaIdOdemeSartlari,
                        projeId = projeId.Value,
                        status = "Başlatıldı",
                        bilgilerTamamMi = false,
                        nedenTamamlanmadi = "Ödeme Şartları bilgileri girilmemiş"
                    };
                    _sayfaStatusService.Insert(yeniStatus);
                }

                txtStatu.Text = "Başlatıldı";
                MessageBox.Show("Ödeme şartları kayıtları başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _pendingDeletions.Clear();
                _newlyAddedMilestones.Clear();
                AddHeaderRow();
                AddBottomSpacer();
                UpdateBulkInvoiceButtonText();
            }
            else
            {
                MessageBox.Show("Ödeme şartları silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtProjeAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.btnAra.PerformClick();
            }
        }
        private void btnKopyala_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeAra.Text))
            {
                MessageBox.Show("Lütfen öncelikle ödeme şartlarını kopyalamak istediğiniz projeyi aratın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string targetProjeNo = txtProjeAra.Text.Trim();
            projeId = _finansProjelerService.GetProjeIdByNo(targetProjeNo);
            if (!projeId.HasValue)
            {
                MessageBox.Show($"Proje '{targetProjeNo}' bulunamadı. Lütfen geçerli bir proje numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtToplamBedel.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal toplamBedel))
            {
                MessageBox.Show("Hedef projenin toplam bedeli geçerli bir sayısal değer değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ParseToplamBedel(out toplamBedel) || toplamBedel == 0)
            {
                MessageBox.Show("Projeye ait fiyatlandırma bulunmadığından ödeme bilgisine kopyalama yapılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sourceProjeNo = Microsoft.VisualBasic.Interaction.InputBox("Kopyalamak istediğiniz proje numarasını girin:", "Proje Kopyalama", "");
            var sourceProjeId = _finansProjelerService.GetProjeIdByNo(sourceProjeNo);

            if (string.IsNullOrWhiteSpace(sourceProjeNo))
            {
                MessageBox.Show("Kaynak proje numarası girilmedi.", "İşlem İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (sourceProjeNo.Equals(targetProjeNo, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Kaynak ve hedef proje numaraları aynı olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sourceMilestones = _odemeSartlariService.GetOdemeBilgileriByProjeId(sourceProjeId.Value);

            if (sourceMilestones == null || !sourceMilestones.Any())
            {
                MessageBox.Show($"'{sourceProjeNo}' numaralı projeye ait ödeme şartı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var existingOdemeBilgileri = _odemeSartlariService.GetOdemeBilgileriByProjeId(projeId.Value);
            foreach (var existing in existingOdemeBilgileri)
            {
                _pendingDeletions.Add((existing.projeId, existing.kilometreTasiId));
            }

            _newlyAddedMilestones.Clear();

            foreach (var milestone in sourceMilestones)
            {
                decimal newTutar = toplamBedel * (milestone.oran / 100);
                decimal newKalanTutar = newTutar;

                var newMilestone = new OdemeSartlari
                {
                    projeId = projeId.Value,
                    kilometreTasiId = 0,
                    siralama = milestone.siralama,
                    kilometreTasiAdi = milestone.kilometreTasiAdi,
                    oran = milestone.oran,
                    tutar = newTutar,
                    kalanTutar = newKalanTutar,
                    tahminiTarih = null,
                    gerceklesenTarih = null,
                    odemeAciklama = "",
                    durum = "Bekliyor",
                    teminatMektubu = false,
                    teminatDurumu = "Pasif"
                };
                _newlyAddedMilestones.Add(newMilestone);
            }

            MessageBox.Show($"'{sourceProjeNo}' projesine ait ödeme şartları '{targetProjeNo}' projesine başarıyla kopyalandı. Kaydet butonuna basarak değişiklikleri kaydedebilirsiniz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadOdemeBilgileri(targetProjeNo);
        }
    }
}