using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
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

            if (_altProjeler == null || !_altProjeler.Any())
            {
                tpTeminatMektubu.TabPages.Remove(tpMektupDagit);
            }
            else
            {
                dgvMektupDagitim.AutoGenerateColumns = false;
                dgvMektupDagitim.Columns.Clear();

                DataGridViewTextBoxColumn colProjeNo = new DataGridViewTextBoxColumn();
                colProjeNo.Name = "ProjeNo";
                colProjeNo.HeaderText = "Proje No";
                colProjeNo.ReadOnly = true;
                colProjeNo.DataPropertyName = "ProjeNo";
                dgvMektupDagitim.Columns.Add(colProjeNo);

                DataGridViewTextBoxColumn colTutar = new DataGridViewTextBoxColumn();
                colTutar.Name = "Tutar";
                colTutar.HeaderText = "Dağıtılan Tutar";
                colTutar.DataPropertyName = "Tutar";
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

                LoadMektupDataToForm();
                txtMektupNo.Enabled = false;
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

        private void frmTeminatMektubuEkle_Load(object sender, EventArgs e)
        {
            ApplyCharacterLimits();

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
                LoadAltProjelerToDataGridView();
            }

            UpdateIadeTarihi();
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
                LoadAltProjelerToDataGridView();
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
            UpdateIadeTarihi();
        }

        private void dtVadeTarihi_ValueChanged(object sender, EventArgs e)
        {
            UpdateIadeTarihi();
        }

        private void cmbKomisyonVadesi_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateIadeTarihi();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            string mektupNo = txtMektupNo.Text.Trim();
            string musteriNo = txtMusteriNo.Text.Trim();
            string musteriAdi = txtMusteriAdi.Text.Trim();
            string paraBirimi = "";

            if (string.IsNullOrWhiteSpace(mektupNo) || string.IsNullOrWhiteSpace(musteriNo) || string.IsNullOrWhiteSpace(musteriAdi))
            {
                MessageBox.Show("Mektup No, Müşteri No ve Müşteri Adı boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            if (!decimal.TryParse(txtKomisyonTutari.Text, NumberStyles.Any, trCulture, out decimal komisyonTutari))
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

            if (_altProjeler != null && _altProjeler.Any() && !ValidateAltProjeTutarlari())
            {
                return;
            }

            try
            {
                if (_isUpdateMode)
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
                    _currentMektup.komisyonTutari = komisyonTutari;
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

                            int? altProjeId = _finansProjelerService.GetProjeIdByNo(altProjeNo);
                            if (!altProjeId.HasValue)
                            {
                                MessageBox.Show($"Alt proje '{altProjeNo}' için proje ID bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            string altProjeSuffix = altProjeNo.Contains(".") ? altProjeNo.Split('.').Last() : altProjeNo.Substring(altProjeNo.Length - 2);
                            string altProjeMektupNo = $"{mektupNo}-{altProjeSuffix}";

                            if (_teminatMektuplariService.MektupNoVarMi(altProjeMektupNo))
                            {
                                MessageBox.Show($"'{altProjeMektupNo}' mektup numarası zaten mevcut. Lütfen farklı bir numara girin.", "Mevcut Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

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

                            var altProjeMektup = new TeminatMektuplari
                            {
                                mektupNo = altProjeMektupNo,
                                musteriNo = musteriNo,
                                musteriAdi = musteriAdi,
                                paraBirimi = paraBirimi,
                                tutar = altProjeTutar,
                                banka = banka,
                                mektupTuru = mektupTuru,
                                vadeTarihi = vadeTarihi,
                                iadeTarihi = dtIadeTarihi.Value,
                                komisyonTutari = komisyonTutari,
                                komisyonOrani = komisyonOrani,
                                komisyonVadesi = komisyonVadesi,
                                projeId = altProjeId.Value,
                                projeNo = altProjeNo,
                                kilometreTasiId = _kilometreTasiId
                            };

                            _teminatMektuplariService.TeminatMektubuKaydet(altProjeMektup);
                        }

                        MessageBox.Show("Tüm alt projeler için teminat mektupları başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFormFields();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
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
                        _currentMektup.komisyonTutari = komisyonTutari;
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
        private void LoadAltProjelerToDataGridView()
        {
            dgvMektupDagitim.Rows.Clear();
            CultureInfo trCulture = new CultureInfo("tr-TR");

            decimal anaTutar = 0;
            decimal.TryParse(txtTutar.Text, NumberStyles.Any, trCulture, out anaTutar);

            int altProjeSayisi = _altProjeler != null ? _altProjeler.Count : 0;
            decimal paylasilanTutar = altProjeSayisi > 0 ? anaTutar / altProjeSayisi : 0;

            if (_altProjeler != null && _altProjeler.Any())
            {
                foreach (var altProje in _altProjeler)
                {
                    dgvMektupDagitim.Rows.Add(altProje, paylasilanTutar.ToString("N2", trCulture));
                }
            }
            RecalculateSummary();
        }
        private void DgvMektupDagitim_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMektupDagitim.Columns[e.ColumnIndex].Name == "Tutar")
            {
                RecalculateSummary();
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
                        dgvMektupDagitim.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = tutar.ToString("N2", trCulture);
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

        private bool ValidateAltProjeTutarlari()
        {
            if (_altProjeler == null || !_altProjeler.Any()) return true;

            CultureInfo trCulture = new CultureInfo("tr-TR");
            decimal toplam = 0;

            foreach (DataGridViewRow row in dgvMektupDagitim.Rows)
            {
                if (row.Cells["Tutar"].Value != null)
                {
                    string tutarText = row.Cells["Tutar"].Value.ToString();
                    if (!decimal.TryParse(tutarText, NumberStyles.Any, trCulture, out decimal tutar))
                    {
                        MessageBox.Show($"Alt proje '{row.Cells["ProjeNo"].Value}' için geçerli tutar giriniz. Lütfen Dağılım sekmesini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tpTeminatMektubu.SelectedTab = tpMektupDagit;
                        return false;
                    }
                    toplam += tutar;
                }
            }

            if (decimal.TryParse(txtTutar.Text, NumberStyles.Any, trCulture, out decimal anaTutar))
            {
                if (Math.Abs(toplam - anaTutar) > 0.01m)
                {
                    MessageBox.Show("Alt projeler için girilen tutarların toplamı ana tutar ile uyuşmuyor. Lütfen Dağılım sekmesini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tpTeminatMektubu.SelectedTab = tpMektupDagit;
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir ana tutar giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
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

        private void LoadMektupDataToForm()
        {
            if (_currentMektup == null) return;

            txtMektupNo.Text = _currentMektup.mektupNo;
            txtMusteriNo.Text = _currentMektup.musteriNo;
            txtMusteriAdi.Text = _currentMektup.musteriAdi;

            chkTL.Checked = _currentMektup.paraBirimi == "TL" || _currentMektup.paraBirimi == "TRY";
            chkDolar.Checked = _currentMektup.paraBirimi == "USD";
            chkEuro.Checked = _currentMektup.paraBirimi == "EUR";

            CultureInfo trCulture = new CultureInfo("tr-TR");
            txtTutar.Text = _currentMektup.tutar.ToString("N2", trCulture);

            if (!string.IsNullOrEmpty(_currentMektup.banka))
            {
                int bankaIndex = cmbBankalar.FindStringExact(_currentMektup.banka.Trim());
                if (bankaIndex >= 0) cmbBankalar.SelectedIndex = bankaIndex;
                else cmbBankalar.Text = _currentMektup.banka;
            }
            if (!string.IsNullOrEmpty(_currentMektup.mektupTuru))
            {
                int turIndex = cmbMektupTuru.FindStringExact(_currentMektup.mektupTuru.Trim());
                if (turIndex >= 0) cmbMektupTuru.SelectedIndex = turIndex;
                else cmbMektupTuru.Text = _currentMektup.mektupTuru;
            }

            dtVadeTarihi.Value = _currentMektup.vadeTarihi ?? DateTime.Now;

            txtKomisyonTutari.Text = _currentMektup.komisyonTutari.ToString("N2", trCulture);
            txtKomisyonOrani.Text = _currentMektup.komisyonOrani.ToString("N2", trCulture);

            if (_currentMektup.komisyonVadesi > 0)
            {
                string vadeText = _currentMektup.komisyonVadesi.ToString() + " Ay";
                int vadeIndex = cmbKomisyonVadesi.FindStringExact(vadeText);
                if (vadeIndex >= 0) cmbKomisyonVadesi.SelectedIndex = vadeIndex;
                else cmbKomisyonVadesi.Text = vadeText;
            }
            if (_altProjeler != null && _altProjeler.Any())
            {
                LoadAltProjelerToDataGridView();
            }

            HesaplaKomisyonTutari();
            UpdateIadeTarihi();
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

        private void LoadBankalar()
        {
            string filePath = ConfigurationManager.AppSettings["ConfigYolu"];
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                cmbBankalar.Items.AddRange(new object[] { "Garanti BBVA", "İş Bankası", "Akbank", "Yapı Kredi", "Halkbank", "Vakıfbank" });
                return;
            }

            try
            {
                string[] satirlar = File.ReadAllLines(filePath);
                string bankalarSatiri = satirlar.FirstOrDefault(s => s.Trim().StartsWith("Bankalar", StringComparison.OrdinalIgnoreCase));

                if (bankalarSatiri != null)
                {
                    string bankalarString = bankalarSatiri.Substring(bankalarSatiri.IndexOf('=') + 1).Trim();
                    string[] bankalar = bankalarString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    cmbBankalar.Items.Clear();
                    foreach (string banka in bankalar)
                    {
                        string trimmedBanka = banka.Trim();
                        cmbBankalar.Items.Add(trimmedBanka);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Banka listesi yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateIadeTarihi()
        {
            DateTime vadeTarihi = dtVadeTarihi.Value;
            int komisyonVadesiAy = 0;
            string komisyonVadesiText = cmbKomisyonVadesi.Text.Trim();

            if (!string.IsNullOrWhiteSpace(komisyonVadesiText))
            {
                string numericPart = System.Text.RegularExpressions.Regex.Match(komisyonVadesiText, @"\d+").Value;

                if (int.TryParse(numericPart, out komisyonVadesiAy))
                {
                    dtIadeTarihi.Value = vadeTarihi.AddMonths(komisyonVadesiAy);
                    return;
                }
            }

            dtIadeTarihi.Value = vadeTarihi;
        }
    }
}