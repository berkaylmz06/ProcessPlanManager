using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.Forms
{
    public partial class frmTeminatMektubuEkle : Form
    {
        private TeminatMektuplari _currentMektup;
        private bool _isUpdateMode = false;
        private string _originalMektupNoForUpdate;
        private string _projeNo;
        private int _kilometreTasiId;
        private string _tutar;
        private int? _projeId;
        private List<string> _altProjeler;
        private bool _activateTeminatTab;
        private Dictionary<string, TeminatMektuplari> _mevcutAltProjeMektuplari = new Dictionary<string, TeminatMektuplari>();
        private ComboBox cmbMektupNoSec;
        private Dictionary<string, TeminatMektuplari> _mevcutMektuplarDict = new Dictionary<string, TeminatMektuplari>();
        private bool _isFirstMektupDagitLoad = true;

        private readonly IMusterilerService _musterilerService;
        private readonly IFinansProjelerService _finansProjelerService;
        private readonly IProjeKutukService _projeKutukService;
        private readonly ITeminatMektuplariService _teminatMektuplariService;

        public frmTeminatMektubuEkle(
           IMusterilerService musterilerService,
           TeminatMektuplari teminatMektup,
           IFinansProjelerService finansProjelerService,
           IProjeKutukService projeKutukService,
           ITeminatMektuplariService teminatMektuplariService,
           int? projeId = null,
           int kilometreTasiId = 0,
           string tutar = "0.00",
           List<string> altProjeler = null,
           bool activateTeminatTab = false
       )
        {
            InitializeComponent();

            _musterilerService = musterilerService ?? throw new ArgumentNullException(nameof(musterilerService));
            _finansProjelerService = finansProjelerService ?? throw new ArgumentNullException(nameof(finansProjelerService));
            _projeKutukService = projeKutukService ?? throw new ArgumentNullException(nameof(projeKutukService));
            _teminatMektuplariService = teminatMektuplariService ?? throw new ArgumentNullException(nameof(teminatMektuplariService));

            this.Icon = Properties.Resources.cekalogokirmizi;

            Helper.DataGridViewHelper.StilUygulaProjeOge(dgvMektupDagitim);

            _kilometreTasiId = kilometreTasiId;
            _tutar = tutar;
            _altProjeler = altProjeler ?? new List<string>();
            _activateTeminatTab = activateTeminatTab;

            cmbMektupNoSec = new ComboBox
            {
                Name = "cmbMektupNoSec",
                Font = txtMektupNo.Font,
                Visible = false,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = txtMektupNo.Anchor,
                TabIndex = txtMektupNo.TabIndex,
                Dock = DockStyle.Fill 
            };
            cmbMektupNoSec.SelectedIndexChanged += cmbMektupNoSec_SelectedIndexChanged;

            if (_altProjeler == null || !_altProjeler.Any())
            {
                tpTeminatMektubu.TabPages.Remove(tpMektupDagit);
            }
            else
            {
                dgvMektupDagitim.AutoGenerateColumns = false;
                dgvMektupDagitim.Columns.Clear();

                DataGridViewTextBoxColumn colProjeNo = new DataGridViewTextBoxColumn
                {
                    Name = "ProjeNo",
                    HeaderText = "Proje No",
                    ReadOnly = true,
                    DataPropertyName = "ProjeNo"
                };
                dgvMektupDagitim.Columns.Add(colProjeNo);

                DataGridViewTextBoxColumn colTutar = new DataGridViewTextBoxColumn
                {
                    Name = "Tutar",
                    HeaderText = "Dağıtılan Tutar",
                    DataPropertyName = "Tutar"
                };
                dgvMektupDagitim.Columns.Add(colTutar);
            }

            LoadBankalar();
            LoadKomisyonVadesi();
            LoadMektupTurleri();

            if (teminatMektup != null)
            {
                _currentMektup = teminatMektup;
                _isUpdateMode = true;
                _originalMektupNoForUpdate = teminatMektup.mektupNo;
                _projeId = teminatMektup.projeId > 0 ? teminatMektup.projeId : projeId;

                this.Text = "Teminat Mektubu Bilgilerini Güncelle";
                btnKaydet.Text = "Güncelle";
            }
            else
            {
                _currentMektup = new TeminatMektuplari();
                _isUpdateMode = false;

                if (projeId.HasValue)
                {
                    _projeId = projeId.Value;
                    _currentMektup.projeId = _projeId.Value;
                }

                this.Text = "Yeni Teminat Mektubu Ekle";
                btnKaydet.Text = "Kaydet";

                _currentMektup.kilometreTasiId = _kilometreTasiId;
                txtTutar.Text = _tutar;

                if (cmbMektupTuru.Items.Count > 0)
                {
                    int geciciIndex = cmbMektupTuru.FindStringExact("Geçici");
                    if (geciciIndex >= 0)
                    {
                        cmbMektupTuru.SelectedIndex = geciciIndex;
                    }
                    else
                    {
                        cmbMektupTuru.SelectedIndex = 0;
                    }
                }

                HesaplaKomisyonTutari();
            }

            tpTeminatMektubu.SelectedTab = tpTeminatMektubu.TabPages["tpTeminatMektubuEkle"];
        }
        private void KontrolEtMevcutAltProjeMektuplari()
        {
            if (_altProjeler == null || !_altProjeler.Any())
            {
                txtMektupNo.Visible = true;
                cmbMektupNoSec.Visible = false;
                return;
            }

            List<string> mevcutMektupAltProjeler = new List<string>();
            _mevcutAltProjeMektuplari.Clear();
            _mevcutMektuplarDict.Clear();
            bool tumMektuplarAyni = true;
            bool bazilariGuncellenmis = false;
            TeminatMektuplari referansMektup = null;

            CultureInfo trCulture = new CultureInfo("tr-TR");
            decimal toplamTutar = 0m;
            decimal toplamKomisyonTutari = 0m;

            foreach (var altProjeNo in _altProjeler)
            {
                TeminatMektuplari mevcutMektup = _teminatMektuplariService.GetTeminatMektubuByProjeNo(altProjeNo);

                if (mevcutMektup != null)
                {
                    mevcutMektupAltProjeler.Add(altProjeNo);
                    _mevcutAltProjeMektuplari.Add(altProjeNo, mevcutMektup);
                    if (!_mevcutMektuplarDict.ContainsKey(mevcutMektup.mektupNo))
                    {
                        _mevcutMektuplarDict.Add(mevcutMektup.mektupNo, mevcutMektup);
                    }
                    toplamTutar += mevcutMektup.tutar;
                    toplamKomisyonTutari += mevcutMektup.komisyonTutari;

                    if (referansMektup == null)
                    {
                        referansMektup = mevcutMektup;
                    }
                    else
                    {
                        if (mevcutMektup.musteriNo != referansMektup.musteriNo ||
                            mevcutMektup.musteriAdi != referansMektup.musteriAdi ||
                            mevcutMektup.banka != referansMektup.banka ||
                            mevcutMektup.mektupTuru != referansMektup.mektupTuru ||
                            mevcutMektup.paraBirimi != referansMektup.paraBirimi ||
                            mevcutMektup.komisyonOrani != referansMektup.komisyonOrani ||
                            mevcutMektup.komisyonVadesi != referansMektup.komisyonVadesi ||
                            mevcutMektup.vadeTarihi != referansMektup.vadeTarihi)
                        {
                            tumMektuplarAyni = false;
                        }
                    }
                }
            }

            if (mevcutMektupAltProjeler.Any())
            {
                string projeListesi = string.Join(", ", mevcutMektupAltProjeler);
                string mesaj;

                txtMektupNo.Visible = false;
                cmbMektupNoSec.Visible = true;
                cmbMektupNoSec.Items.Clear();
                foreach (var mektupNo in _mevcutMektuplarDict.Keys)
                {
                    cmbMektupNoSec.Items.Add(mektupNo);
                }
                cmbMektupNoSec.BringToFront();

                if (cmbMektupNoSec.Items.Count > 0)
                {
                    cmbMektupNoSec.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Mevcut mektuplar bulundu ancak ComboBox doldurulamadı. Servis verilerini kontrol edin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (tpTeminatMektubu.TabPages.Contains(tpMektupDagit))
                {
                    tpTeminatMektubu.TabPages.Remove(tpMektupDagit);
                }

                if (tumMektuplarAyni && !bazilariGuncellenmis)
                {
                    mesaj = $"UYARI: Aşağıdaki alt projeler için teminat mektubu zaten mevcut: \n{projeListesi}";
                    lblTeminatBilgi.Text = mesaj;
                    lblTeminatBilgi.ForeColor = Color.DarkOrange;
                    lblTeminatBilgi.Font = new Font(lblTeminatBilgi.Font, FontStyle.Bold);

                    _currentMektup = new TeminatMektuplari
                    {
                        mektupNo = referansMektup.mektupNo,
                        musteriNo = referansMektup.musteriNo,
                        musteriAdi = referansMektup.musteriAdi,
                        paraBirimi = referansMektup.paraBirimi,
                        tutar = referansMektup.tutar,
                        banka = referansMektup.banka,
                        mektupTuru = referansMektup.mektupTuru,
                        vadeTarihi = referansMektup.vadeTarihi,
                        iadeTarihi = referansMektup.iadeTarihi,
                        komisyonTutari = referansMektup.komisyonTutari,
                        komisyonOrani = referansMektup.komisyonOrani,
                        komisyonVadesi = referansMektup.komisyonVadesi,
                        projeId = _projeId.HasValue ? _projeId.Value : 0,
                        projeNo = _projeNo,
                        kilometreTasiId = _kilometreTasiId
                    };
                    _isUpdateMode = true;
                    _originalMektupNoForUpdate = referansMektup.mektupNo;
                    this.Text = "Teminat Mektubu Bilgilerini Güncelle";
                    btnKaydet.Text = "Güncelle";
                }
                else if (bazilariGuncellenmis)
                {
                    mesaj = $"UYARI: Bazı alt projeler için teminat mektupları güncellenmiş: \n{projeListesi}";
                    lblTeminatBilgi.Text = mesaj;
                    lblTeminatBilgi.ForeColor = Color.DarkOrange;
                    lblTeminatBilgi.Font = new Font(lblTeminatBilgi.Font, FontStyle.Bold);
                }
                else
                {
                    mesaj = $"UYARI: Bazı alt projeler için teminat mektubu bilgileri farklı";
                    lblTeminatBilgi.Text = mesaj;
                    lblTeminatBilgi.ForeColor = Color.DarkOrange;
                    lblTeminatBilgi.Font = new Font(lblTeminatBilgi.Font, FontStyle.Bold);
                }
            }
            else
            {
                txtMektupNo.Visible = true;
                cmbMektupNoSec.Visible = false;
                if (!tpTeminatMektubu.TabPages.Contains(tpMektupDagit) && _altProjeler.Any())
                {
                    tpTeminatMektubu.TabPages.Add(tpMektupDagit);
                }
                lblTeminatBilgi.Text = "";
                lblTeminatBilgi.ForeColor = SystemColors.ControlText;
                lblTeminatBilgi.Font = new Font(lblTeminatBilgi.Font, FontStyle.Regular);
            }
        }
        private void frmTeminatMektubuEkle_Load(object sender, EventArgs e)
        {
            ApplyCharacterLimits();

            if (!tlpTemelBilgiler.Controls.ContainsKey("cmbMektupNoSec"))
            {
                tlpTemelBilgiler.Controls.Add(cmbMektupNoSec, 1, 0);
            }

            if (_altProjeler != null && _altProjeler.Any())
            {
                KontrolEtMevcutAltProjeMektuplari();
            }

            cmbMektupNoSec.BringToFront();

            txtTutar.ReadOnly = false;
            txtTutar.Enabled = true;

            LoadMektupDataToForm();

            if (_isUpdateMode && _currentMektup != null)
            {
                if (_currentMektup.mektupTuru == "Kesin" || _currentMektup.mektupTuru == "Kati")
                {
                    dtVadeTarihi.Enabled = false;
                }
            }
            else
            {
                if (cmbMektupTuru.Text == "Kesin" || cmbMektupTuru.Text == "Kati")
                {
                    dtVadeTarihi.Enabled = false;
                }
            }

            if (_altProjeler != null && _altProjeler.Any() && _activateTeminatTab)
            {
                if (cmbMektupNoSec.Visible)
                {
                }
                else
                {
                    LoadAltProjelerToDataGridView();
                }
            }

            this.BeginInvoke((Action)(() =>
            {
                cmbBankalar.Refresh();
                this.Invalidate();
                this.Refresh();
            }));
        }
        private void LoadBankalar()
        {
            if (cmbBankalar.Items.Count > 0)
            {
                return;
            }

            cmbBankalar.Items.Clear();

            string[] hardcodedBankalar = {
                "HALKBANK",
                "YAPIKREDI",
                "DENIZBANK",
                "GARANTI",
                "ISBANK",
                "ZIRAAT",
                "TEB",
                "EXIMBANK",
                "VAKIFBANK",
                "KUVEVTTURK",
                "AKBANK",
                "QNB BANK"
            };

            try
            {
                foreach (string banka in hardcodedBankalar)
                {
                    string trimmedBanka = banka.Trim();
                    if (!string.IsNullOrEmpty(trimmedBanka) && !cmbBankalar.Items.Contains(trimmedBanka))
                    {
                        cmbBankalar.Items.Add(trimmedBanka);
                    }
                }

                if (cmbBankalar.Items.Count > 0)
                {
                    cmbBankalar.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Banka listesi yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (cmbBankalar.Items.Count == 0)
            {
                MessageBox.Show("Banka listesi yüklenemedi. Uygulamayı yeniden başlatın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMektupDataToForm()
        {
            if (_currentMektup == null)
            {
                return;
            }

            if (cmbBankalar.Items.Count == 0)
            {
                LoadBankalar();
            }

            if (txtMektupNo.Visible)
            {
                txtMektupNo.Text = _currentMektup.mektupNo;
            }

            txtMusteriNo.Text = _currentMektup.musteriNo;
            txtMusteriAdi.Text = _currentMektup.musteriAdi;

            chkTL.Checked = _currentMektup.paraBirimi == "TL" || _currentMektup.paraBirimi == "TRY";
            chkDolar.Checked = _currentMektup.paraBirimi == "USD";
            chkEuro.Checked = _currentMektup.paraBirimi == "EUR";

            CultureInfo trCulture = new CultureInfo("tr-TR");
            txtTutar.Text = _currentMektup.tutar.ToString("N2", trCulture);

            if (!string.IsNullOrEmpty(_currentMektup.banka))
            {
                string banka = _currentMektup.banka.Trim();

                string normalizedBanka = banka.ToUpper();
                int index = -1;
                for (int i = 0; i < cmbBankalar.Items.Count; i++)
                {
                    string listedBank = cmbBankalar.Items[i].ToString().ToUpper();
                    if (listedBank == normalizedBanka)
                    {
                        index = i;
                        break;
                    }
                }

                this.BeginInvoke((Action)(() =>
                {
                    if (index >= 0)
                    {
                        cmbBankalar.SelectedIndex = index;
                        cmbBankalar.SelectedItem = cmbBankalar.Items[index];
                    }
                    else
                    {
                        string bankaToAdd = banka;
                        cmbBankalar.Items.Add(bankaToAdd);
                        int newIndex = cmbBankalar.Items.Count - 1;
                        cmbBankalar.SelectedIndex = newIndex;
                        cmbBankalar.SelectedItem = bankaToAdd;
                        MessageBox.Show($"Banka '{bankaToAdd}' listeye eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    cmbBankalar.Refresh();
                }));
            }
            else
            {
                this.BeginInvoke((Action)(() =>
                {
                    cmbBankalar.SelectedIndex = 0;
                    cmbBankalar.Refresh();
                }));
            }

            if (!string.IsNullOrEmpty(_currentMektup.mektupTuru))
            {
                int index = cmbMektupTuru.FindStringExact(_currentMektup.mektupTuru.Trim());
                if (index >= 0)
                {
                    cmbMektupTuru.SelectedIndex = index;
                }
                else
                {
                    cmbMektupTuru.Text = _currentMektup.mektupTuru;
                }
            }

            dtVadeTarihi.Value = _currentMektup.vadeTarihi ?? DateTime.Now;

            txtKomisyonTutari.Text = _currentMektup.komisyonTutari.ToString("N2", trCulture);
            txtKomisyonOrani.Text = _currentMektup.komisyonOrani.ToString("N2", trCulture);

            if (_currentMektup.komisyonVadesi > 0)
            {
                string vadeText = _currentMektup.komisyonVadesi.ToString() + " Ay";
                int index = cmbKomisyonVadesi.FindStringExact(vadeText);
                if (index >= 0)
                {
                    cmbKomisyonVadesi.SelectedIndex = index;
                }
                else
                {
                    cmbKomisyonVadesi.Text = vadeText;
                }
            }

            if (_altProjeler != null && _altProjeler.Any())
            {
                LoadAltProjelerToDataGridView();
            }

            HesaplaKomisyonTutari();
        }
        private void LoadKomisyonVadesi()
        {
            cmbKomisyonVadesi.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                cmbKomisyonVadesi.Items.Add(i + " Ay");
            }
            cmbKomisyonVadesi.SelectedIndex = 0;
        }

        private void LoadMektupTurleri()
        {
            cmbMektupTuru.Items.Clear();
            cmbMektupTuru.Items.AddRange(new object[] { "Geçici", "Kesin", "Avans", "Kati" });
        }

        private void ApplyCharacterLimits()
        {
            if (txtMektupNo != null)
                txtMektupNo.MaxLength = 50;
            if (txtMusteriNo != null)
                txtMusteriNo.MaxLength = 50;
            if (txtMusteriAdi != null)
                txtMusteriAdi.MaxLength = 250;
            if (cmbBankalar != null)
                cmbBankalar.MaxLength = 250;
            if (cmbMektupTuru != null)
                cmbMektupTuru.MaxLength = 250;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            HesaplaKomisyonTutari();

            if (!string.IsNullOrEmpty(lblTeminatBilgi.Text) && lblTeminatBilgi.Text.Contains("ZATEN MEVCUT") && !cmbMektupNoSec.Visible)
            {
                MessageBox.Show("Mevcut teminat mektupları tespit edildi. Yeni teminat mektubu kaydedilemez. Lütfen mevcut mektupları güncelleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mektupNo = cmbMektupNoSec.Visible ? cmbMektupNoSec.SelectedItem?.ToString() : txtMektupNo.Text.Trim();
            if (string.IsNullOrWhiteSpace(mektupNo))
            {
                MessageBox.Show("Mektup No boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string musteriNo = txtMusteriNo.Text.Trim();
            string musteriAdi = txtMusteriAdi.Text.Trim();
            string paraBirimi = "";

            if (string.IsNullOrWhiteSpace(musteriNo) || string.IsNullOrWhiteSpace(musteriAdi))
            {
                MessageBox.Show("Müşteri No ve Müşteri Adı boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (chkTL.Checked)
                paraBirimi = "TL";
            else if (chkDolar.Checked)
                paraBirimi = "USD";
            else if (chkEuro.Checked)
                paraBirimi = "EUR";
            else
            {
                MessageBox.Show("Lütfen para birimi seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CultureInfo trCulture = new CultureInfo("tr-TR");
            if (!decimal.TryParse(txtTutar.Text, NumberStyles.Any, trCulture, out decimal anaTutar))
            {
                MessageBox.Show("Lütfen geçerli bir ana tutar giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string banka = cmbBankalar.Text.Trim();
            string mektupTuru = cmbMektupTuru.Text.Trim();
            DateTime? vadeTarihi = (mektupTuru == "Kesin" || mektupTuru == "Kati") ? null : (DateTime?)dtVadeTarihi.Value;

            if (!decimal.TryParse(txtKomisyonTutari.Text, NumberStyles.Any, trCulture, out decimal toplamKomisyonTutari))
            {
                MessageBox.Show("Lütfen geçerli bir komisyon tutarı giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtKomisyonOrani.Text, NumberStyles.Any, trCulture, out decimal komisyonOrani))
            {
                MessageBox.Show("Lütfen geçerli bir komisyon oranı giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int komisyonVadesi = 0;
            string komisyonVadesiText = cmbKomisyonVadesi.Text.Trim();
            if (!string.IsNullOrWhiteSpace(komisyonVadesiText))
            {
                string numericPart = System.Text.RegularExpressions.Regex.Match(komisyonVadesiText, @"\d+").Value;
                if (!int.TryParse(numericPart, out komisyonVadesi))
                {
                    MessageBox.Show("Lütfen geçerli bir komisyon vadesi seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Lütfen komisyon vadesi seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_isUpdateMode || cmbMektupNoSec.Visible)
                {
                    _currentMektup.mektupNo = mektupNo;
                    _currentMektup.musteriNo = musteriNo;
                    _currentMektup.musteriAdi = musteriAdi;
                    _currentMektup.paraBirimi = paraBirimi;
                    _currentMektup.tutar = anaTutar;
                    _currentMektup.banka = banka;
                    _currentMektup.mektupTuru = mektupTuru;
                    _currentMektup.vadeTarihi = vadeTarihi;
                    _currentMektup.iadeTarihi = dtIadeTarihi.Value;
                    _currentMektup.komisyonTutari = toplamKomisyonTutari;
                    _currentMektup.komisyonOrani = komisyonOrani;
                    _currentMektup.komisyonVadesi = komisyonVadesi;
                    if (_projeId.HasValue)
                    {
                        _currentMektup.projeId = _projeId.Value;
                    }
                    _currentMektup.projeNo = _projeNo;

                    _teminatMektuplariService.MektupGuncelle(_originalMektupNoForUpdate, _currentMektup);
                    MessageBox.Show("Teminat Mektubu başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    if (_altProjeler != null && _altProjeler.Any())
                    {
                        foreach (DataGridViewRow row in dgvMektupDagitim.Rows)
                        {
                            string altProjeNo = row.Cells["ProjeNo"].Value.ToString();

                            bool mevcutMektupVar = _mevcutAltProjeMektuplari.ContainsKey(altProjeNo);

                            decimal altProjeTutar = 0;
                            if (row.Cells["Tutar"].Value != null)
                            {
                                string tutarText = row.Cells["Tutar"].Value.ToString();
                                if (!decimal.TryParse(tutarText, NumberStyles.Any, trCulture, out altProjeTutar))
                                {
                                    MessageBox.Show($"Alt proje '{altProjeNo}' için geçerli bir tutar girilemedi. Dağıtılan Tutar: {tutarText}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Alt proje '{altProjeNo}' için dağıtım tutarı bulunamadı. Lütfen Dağılım sekmesini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            decimal altKomisyonTutari = altProjeTutar * komisyonOrani / 100m;

                            int? altProjeId = _finansProjelerService.GetProjeIdByNo(altProjeNo);
                            if (!altProjeId.HasValue)
                            {
                                MessageBox.Show($"Alt proje '{altProjeNo}' için proje ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            string altProjeMektupNo;
                            TeminatMektuplari mektupToSave;

                            if (mevcutMektupVar)
                            {
                                mektupToSave = _mevcutAltProjeMektuplari[altProjeNo];
                                altProjeMektupNo = mektupToSave.mektupNo;
                            }
                            else
                            {
                                string altProjeSuffix = altProjeNo.Contains(".") ? altProjeNo.Split('.').Last() : altProjeNo.Substring(altProjeNo.Length - 2);
                                altProjeMektupNo = $"{mektupNo}-{altProjeSuffix}";

                                if (_teminatMektuplariService.MektupNoVarMi(altProjeMektupNo))
                                {
                                    MessageBox.Show($"'{altProjeMektupNo}' mektup numarası zaten mevcut. Lütfen farklı bir numara girin.", "Mevcut Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                                mektupToSave = new TeminatMektuplari();
                            }

                            mektupToSave.mektupNo = altProjeMektupNo;
                            mektupToSave.musteriNo = musteriNo;
                            mektupToSave.musteriAdi = musteriAdi;
                            mektupToSave.paraBirimi = paraBirimi;
                            mektupToSave.tutar = altProjeTutar;
                            mektupToSave.banka = banka;
                            mektupToSave.mektupTuru = mektupTuru;
                            mektupToSave.vadeTarihi = vadeTarihi;
                            mektupToSave.iadeTarihi = dtIadeTarihi.Value;
                            mektupToSave.komisyonTutari = altKomisyonTutari;
                            mektupToSave.komisyonOrani = komisyonOrani;
                            mektupToSave.komisyonVadesi = komisyonVadesi;
                            mektupToSave.projeId = altProjeId.Value;
                            mektupToSave.projeNo = altProjeNo;
                            mektupToSave.kilometreTasiId = _kilometreTasiId;

                            if (mevcutMektupVar)
                            {
                                _teminatMektuplariService.MektupGuncelle(altProjeMektupNo, mektupToSave);
                            }
                            else
                            {
                                _teminatMektuplariService.TeminatMektubuKaydet(mektupToSave);
                            }
                        }

                        MessageBox.Show("Tüm alt projeler için teminat mektupları başarıyla kaydedildi/güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFormFields();
                    }
                    else
                    {
                        if (_teminatMektuplariService.MektupNoVarMi(mektupNo))
                        {
                            MessageBox.Show($"'{mektupNo}' mektup numarası zaten mevcut. Lütfen farklı bir numara girin.", "Mevcut Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        _currentMektup.mektupNo = mektupNo;
                        _currentMektup.musteriNo = musteriNo;
                        _currentMektup.musteriAdi = musteriAdi;
                        _currentMektup.paraBirimi = paraBirimi;
                        _currentMektup.tutar = anaTutar;
                        _currentMektup.banka = banka;
                        _currentMektup.mektupTuru = mektupTuru;
                        _currentMektup.vadeTarihi = vadeTarihi;
                        _currentMektup.iadeTarihi = dtIadeTarihi.Value;
                        _currentMektup.komisyonTutari = toplamKomisyonTutari;
                        _currentMektup.komisyonOrani = komisyonOrani;
                        _currentMektup.komisyonVadesi = komisyonVadesi;
                        _currentMektup.kilometreTasiId = _kilometreTasiId;
                        _currentMektup.projeNo = _projeNo;
                        if (_projeId.HasValue)
                        {
                            _currentMektup.projeId = _projeId.Value;
                        }

                        _teminatMektuplariService.TeminatMektubuKaydet(_currentMektup);
                        MessageBox.Show("Teminat Mektubu başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFormFields();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cmbMektupNoSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            string secilenMektupNo = cmbMektupNoSec.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(secilenMektupNo) && _mevcutMektuplarDict.ContainsKey(secilenMektupNo))
            {
                _currentMektup = _mevcutMektuplarDict[secilenMektupNo];
                _originalMektupNoForUpdate = secilenMektupNo;

                _projeNo = _currentMektup.projeNo;
                _projeId = _currentMektup.projeId;

                LoadMektupDataToForm();
                HesaplaKomisyonTutari();
            }
        }
        private void LoadAltProjelerToDataGridView()
        {
            dgvMektupDagitim.Rows.Clear();
            CultureInfo trCulture = new CultureInfo("tr-TR");

            if (_altProjeler != null && _altProjeler.Any())
            {
                foreach (var altProje in _altProjeler)
                {
                    TeminatMektuplari altMektup = _teminatMektuplariService.GetTeminatMektubuByProjeNo(altProje);
                    string tutarText = "0,00";

                    if (altMektup != null)
                    {
                        tutarText = altMektup.tutar.ToString("N2", trCulture);
                    }
                    else if (!_isUpdateMode && !cmbMektupNoSec.Visible)
                    {
                        decimal anaTutar = 0m;
                        decimal.TryParse(txtTutar.Text, NumberStyles.Any, trCulture, out anaTutar);
                        int altProjeSayisi = _altProjeler.Count;
                        decimal paylasilanTutar = altProjeSayisi > 0 ? anaTutar / altProjeSayisi : 0m;
                        tutarText = paylasilanTutar.ToString("N2", trCulture);
                    }

                    dgvMektupDagitim.Rows.Add(altProje, tutarText);
                }
            }

            RecalculateSummary();
        }
        private void RecalculateSummary()
        {
            CultureInfo trCulture = new CultureInfo("tr-TR");

            decimal anaTutar = 0;
            decimal.TryParse(txtTutar.Text, NumberStyles.Any, trCulture, out anaTutar);

            decimal toplamDagitilan = 0;

            foreach (DataGridViewRow row in dgvMektupDagitim.Rows)
            {
                if (row.Cells["Tutar"].Value != null)
                {
                    string tutarText = row.Cells["Tutar"].Value.ToString();
                    if (decimal.TryParse(tutarText, NumberStyles.Any, trCulture, out decimal dagitilanTutar))
                    {
                        toplamDagitilan += dagitilanTutar;
                    }
                }
            }

            decimal fark = anaTutar - toplamDagitilan;

            txtAnaTutar.Text = anaTutar.ToString("N2", trCulture);
            txtToplamDagitilan.Text = toplamDagitilan.ToString("N2", trCulture);
            txtFark.Text = fark.ToString("N2", trCulture);

            if (Math.Abs(fark) < 0.01m)
            {
                txtFark.BackColor = Color.PaleGreen;
            }
            else
            {
                txtFark.BackColor = Color.LightCoral;
            }
        }

        private void HesaplaKomisyonTutari()
        {
            try
            {
                CultureInfo trCulture = new CultureInfo("tr-TR");

                string tutarText = txtTutar.Text.Trim();
                string oranText = txtKomisyonOrani.Text.Trim();

                bool tutarOk = decimal.TryParse(tutarText, NumberStyles.Any, trCulture, out decimal tutar);
                bool oranOk = decimal.TryParse(oranText, NumberStyles.Any, trCulture, out decimal oran);

                if (tutarOk && oranOk)
                {
                    decimal komisyonTutari = tutar * oran / 100m;
                    txtKomisyonTutari.Text = komisyonTutari.ToString("N2", trCulture);
                }
                else
                {
                    txtKomisyonTutari.Text = "0,00";
                }
            }
            catch
            {
                txtKomisyonTutari.Text = "0,00";
            }
        }

        private void ClearFormFields()
        {
            txtMektupNo.Clear();
            txtMusteriNo.Clear();
            txtMusteriAdi.Clear();
            chkTL.Checked = false;
            chkDolar.Checked = false;
            chkEuro.Checked = false;
            txtTutar.Clear();
            cmbBankalar.SelectedIndex = -1;
            cmbMektupTuru.SelectedIndex = -1;
            dtVadeTarihi.Value = DateTime.Now;
            dtIadeTarihi.Value = DateTime.Now;
            txtKomisyonTutari.Clear();
            txtKomisyonOrani.Clear();
            cmbKomisyonVadesi.SelectedIndex = -1;

            if (tpTeminatMektubu.TabPages.ContainsKey("tpMektupDagit"))
            {
                dgvMektupDagitim.Rows.Clear();
                txtAnaTutar.Text = "0,00";
                txtToplamDagitilan.Text = "0,00";
                txtFark.Text = "0,00";
                txtFark.BackColor = System.Drawing.Color.LightCoral;
            }
        }

        private void TxtTutar_TextChanged(object sender, EventArgs e)
        {
            HesaplaKomisyonTutari();

            if (_altProjeler != null && _altProjeler.Any())
            {
                RecalculateSummary();
            }
        }

        private void TxtKomisyonOrani_TextChanged(object sender, EventArgs e)
        {
            HesaplaKomisyonTutari();
        }

        private void txtMusteriNo_Leave(object sender, EventArgs e)
        {
            string musteriNo = txtMusteriNo.Text.Trim();
            if (!string.IsNullOrEmpty(musteriNo))
            {
                Musteriler musteri = _musterilerService.GetMusteriByMusteriNo(musteriNo);

                if (musteri != null)
                {
                    txtMusteriAdi.Text = musteri.musteriAdi;
                }
                else
                {
                    txtMusteriAdi.Text = "";
                    MessageBox.Show("Belirtilen müşteri numarası ile bir müşteri bulunamadı.", "Müşteri Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtMusteriAdi.Text = "";
            }
        }

        private void chkEuro_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEuro.Checked)
            {
                chkTL.Checked = false;
                chkDolar.Checked = false;
            }
        }

        private void chkDolar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDolar.Checked)
            {
                chkTL.Checked = false;
                chkEuro.Checked = false;
            }
        }

        private void chkTL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTL.Checked)
            {
                chkDolar.Checked = false;
                chkEuro.Checked = false;
            }
        }

        private void TpTeminatMektubu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tpTeminatMektubu.SelectedTab == tpTeminatMektubu.TabPages["tpMektupDagit"] && _altProjeler != null && _altProjeler.Any())
            {
                if (_isFirstMektupDagitLoad)
                {
                    LoadAltProjelerToDataGridView(); 
                    _isFirstMektupDagitLoad = false; 
                }
            }
        }

        private void CmbMektupTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMektupTuru.Text == "Kesin" || cmbMektupTuru.Text == "Kati")
            {
                dtVadeTarihi.Enabled = false;
                dtVadeTarihi.Value = DateTime.Now;
            }
            else
            {
                dtVadeTarihi.Enabled = true;
            }
        }

        private void dtVadeTarihi_ValueChanged(object sender, EventArgs e)
        {
            dtIadeTarihi.Value = dtVadeTarihi.Value;
        }

        private void DgvMektupDagitim_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMektupDagitim.Columns[e.ColumnIndex].Name == "Tutar")
            {
                RecalculateSummary();
                HesaplaKomisyonTutari(); 
            }
        }

        private void DgvMektupDagitim_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvMektupDagitim.Columns[e.ColumnIndex].Name == "Tutar")
            {
                CultureInfo trCulture = new CultureInfo("tr-TR");

                if (e.FormattedValue != null && !string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                {
                    if (decimal.TryParse(e.FormattedValue.ToString(), NumberStyles.Any, trCulture, out decimal tutar))
                    {
                        dgvMektupDagitim.Rows[e.RowIndex].ErrorText = string.Empty;
                    }
                    else
                    {
                        e.Cancel = true;
                        dgvMektupDagitim.Rows[e.RowIndex].ErrorText = "Geçerli bir parasal tutar giriniz (ör: 100,50)";
                    }
                }
                else
                {
                    dgvMektupDagitim.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0.00m.ToString("N2", trCulture);
                    dgvMektupDagitim.Rows[e.RowIndex].ErrorText = string.Empty;
                }
            }
        }
    }
}