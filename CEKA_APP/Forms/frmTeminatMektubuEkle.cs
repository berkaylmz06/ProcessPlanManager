using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Services.ProjeFinans;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
     string tutar = "0.00")
        {
            InitializeComponent();

            _musterilerService = musterilerService ?? throw new ArgumentNullException(nameof(musterilerService));
            _finansProjelerService = finansProjelerService ?? throw new ArgumentNullException(nameof(finansProjelerService));
            _projeKutukService = projeKutukService ?? throw new ArgumentNullException(nameof(projeKutukService));
            _teminatMektuplariService = teminatMektuplariService ?? throw new ArgumentNullException(nameof(teminatMektuplariService));

            this.Icon = Properties.Resources.cekalogokirmizi;

            _kilometreTasiId = kilometreTasiId;
            _tutar = tutar;

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

                LoadProjeKutukData();

                _currentMektup.kilometreTasiId = _kilometreTasiId;
                txtTutar.Text = _tutar;
            }

            cmbMektupTuru.SelectedIndexChanged += CmbMektupTuru_SelectedIndexChanged;
        }

        private void frmTeminatMektubuEkle_Load(object sender, EventArgs e)
        {
            LoadBankalar();
            ApplyCharacterLimits();
            if (_isUpdateMode && _currentMektup != null)
            {
                if (_currentMektup.mektupTuru == "Kesin" || _currentMektup.mektupTuru == "Kati")
                {
                    dtVadeTarihi.Enabled = false;
                }
            }
        }
        private void CmbMektupTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMektupTuru.SelectedItem != null &&
                (cmbMektupTuru.SelectedItem.ToString() == "Kesin" || cmbMektupTuru.SelectedItem.ToString() == "Kati"))
            {
                dtVadeTarihi.Enabled = false;
                dtVadeTarihi.Value = DateTime.Now;
            }
            else
            {
                dtVadeTarihi.Enabled = true;
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
            cmbBankalar.Items.Clear();
            cmbMektupTuru.SelectedIndex = -1;
            dtVadeTarihi.Value = DateTime.Now;
            dtIadeTarihi.Value = DateTime.Now;
            txtKomisyonTutari.Clear();
            txtKomisyonOrani.Clear();
            cmbKomisyonVadesi.SelectedIndex = -1;
        }

        private void LoadProjeKutukData()
        {
            if (!string.IsNullOrEmpty(txtMusteriNo.Text.Trim()))
            {
                var musteri = _musterilerService.GetMusteriByMusteriNo(txtMusteriNo.Text.Trim());
                if (musteri != null)
                {
                    txtMusteriAdi.Text = musteri.musteriAdi ?? "";
                    string paraBirimi = musteri.doviz ?? "TRY";
                    if (string.IsNullOrEmpty(paraBirimi))
                    {
                        MessageBox.Show($"Müşteri No '{txtMusteriNo.Text.Trim()}' için döviz bilgisi null veya boş, varsayılan olarak TRY atandı. Veritabanını kontrol edin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        paraBirimi = "TRY";
                    }
                    _currentMektup.paraBirimi = paraBirimi;
                    UpdateCheckBoxes(paraBirimi);
                }
                else
                {
                    MessageBox.Show($"Müşteri No '{txtMusteriNo.Text.Trim()}' için bilgi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMusteriAdi.Text = "";
                    _currentMektup.paraBirimi = "TRY";
                    UpdateCheckBoxes("TRY");
                }
            }
            else if (!string.IsNullOrEmpty(_projeNo))
            {
                var projeId = _finansProjelerService.GetProjeIdByNo(_projeNo);
                var projeKutuk = _projeKutukService.ProjeKutukAra(projeId.Value);
                if (projeKutuk != null)
                {
                    string musteriNo = projeKutuk.musteriNo ?? "";
                    txtMusteriNo.Text = musteriNo;
                    if (!string.IsNullOrEmpty(musteriNo))
                    {
                        var musteri = _musterilerService.GetMusteriByMusteriNo(musteriNo);
                        if (musteri != null)
                        {
                            txtMusteriAdi.Text = musteri.musteriAdi ?? "";
                            string paraBirimi = musteri.doviz ?? "TRY";
                            if (string.IsNullOrEmpty(paraBirimi))
                            {
                                MessageBox.Show($"Müşteri No '{musteriNo}' için döviz bilgisi null veya boş, varsayılan olarak TRY atandı. Veritabanını kontrol edin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                paraBirimi = "TRY";
                            }
                            _currentMektup.paraBirimi = paraBirimi;
                            UpdateCheckBoxes(paraBirimi);
                        }
                        else
                        {
                            MessageBox.Show($"Müşteri No '{musteriNo}' için bilgi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtMusteriAdi.Text = "";
                            _currentMektup.paraBirimi = "TRY";
                            UpdateCheckBoxes("TRY");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Proje '{_projeNo}' için müşteri numarası bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMusteriAdi.Text = "";
                        _currentMektup.paraBirimi = "TRY";
                        UpdateCheckBoxes("TRY");
                    }
                    if (_projeId.HasValue)
                    {
                        _currentMektup.projeId = _projeId.Value;
                    }
                }
                else
                {
                    MessageBox.Show($"Proje '{_projeNo}' için kütük bilgileri bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMusteriNo.Text = "";
                    txtMusteriAdi.Text = "";
                    _currentMektup.paraBirimi = "TRY";
                    if (_projeId.HasValue)
                    {
                        _currentMektup.projeId = _projeId.Value;
                    }
                    UpdateCheckBoxes("TRY");
                }
            }
            else
            {
                txtMusteriAdi.Text = "";
                _currentMektup.paraBirimi = "TRY";
                if (_projeId.HasValue)
                {
                    _currentMektup.projeId = _projeId.Value;
                }
                UpdateCheckBoxes("TRY");
            }
        }
        private void UpdateCheckBoxes(string paraBirimi)
        {
            chkTL.Checked = paraBirimi == "TRY" || paraBirimi == "TL";
            chkDolar.Checked = paraBirimi == "USD" || paraBirimi == "DOLAR";
            chkEuro.Checked = paraBirimi == "EUR";
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

            if (!decimal.TryParse(txtTutar.Text, out decimal tutar))
            {
                MessageBox.Show("Lütfen geçerli bir tutar giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string banka = cmbBankalar.Text.Trim();
            string mektupTuru = cmbMektupTuru.Text.Trim();
            DateTime? vadeTarihi = (mektupTuru == "Kesin" || mektupTuru == "Kati") ? null : (DateTime?)dtVadeTarihi.Value;

            if (!decimal.TryParse(txtKomisyonTutari.Text, out decimal komisyonTutari))
            {
                MessageBox.Show("Lütfen geçerli bir komisyon tutarı giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtKomisyonOrani.Text, out decimal komisyonOrani))
            {
                MessageBox.Show("Lütfen geçerli bir komisyon oranı giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int komisyonVadesi = 0;
            string komisyonVadesiText = cmbKomisyonVadesi.Text.Trim();
            if (!string.IsNullOrWhiteSpace(komisyonVadesiText))
            {
                string numericPart = System.Text.RegularExpressions.Regex.Match(komisyonVadesiText, @"\d+").Value;
                if (int.TryParse(numericPart, out int parsedKomisyonVadesi))
                {
                    komisyonVadesi = parsedKomisyonVadesi;
                }
                else
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
                if (_isUpdateMode)
                {
                    _currentMektup.mektupNo = mektupNo;
                    _currentMektup.musteriNo = musteriNo;
                    _currentMektup.musteriAdi = musteriAdi;
                    _currentMektup.paraBirimi = paraBirimi;
                    _currentMektup.tutar = tutar;
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
                    _currentMektup.kilometreTasiId = _kilometreTasiId;

                    _teminatMektuplariService.MektupGuncelle(_originalMektupNoForUpdate, _currentMektup);
                    MessageBox.Show("Teminat Mektubu başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    if (_projeId.HasValue)
                    {
                        _currentMektup.projeId = _projeId.Value;
                    }

                    if (_teminatMektuplariService.MektupNoVarMi(mektupNo))
                    {
                        MessageBox.Show($"'{mektupNo}' mektup numarası zaten mevcut. Lütfen farklı bir numara girin.", "Mevcut Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _currentMektup.mektupNo = mektupNo;
                        _currentMektup.musteriNo = musteriNo;
                        _currentMektup.musteriAdi = musteriAdi;
                        _currentMektup.paraBirimi = paraBirimi;
                        _currentMektup.tutar = tutar;
                        _currentMektup.banka = banka;
                        _currentMektup.mektupTuru = mektupTuru;
                        _currentMektup.vadeTarihi = vadeTarihi;
                        _currentMektup.iadeTarihi = dtIadeTarihi.Value;
                        _currentMektup.komisyonTutari = komisyonTutari;
                        _currentMektup.komisyonOrani = komisyonOrani;
                        _currentMektup.komisyonVadesi = komisyonVadesi;
                        _currentMektup.kilometreTasiId = _kilometreTasiId;

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
        private void HesaplaKomisyonTutari()
        {
            try
            {
                CultureInfo culture = new CultureInfo("tr-TR");

                bool tutarOk = decimal.TryParse(txtTutar.Text, NumberStyles.Any, culture, out decimal tutar);
                bool oranOk = decimal.TryParse(txtKomisyonOrani.Text, NumberStyles.Any, culture, out decimal oran);

                if (tutarOk && oranOk)
                {
                    decimal komisyonTutari = tutar * oran / 100m;
                    txtKomisyonTutari.Text = komisyonTutari.ToString("N2", culture);
                }
                else
                {
                    txtKomisyonTutari.Text = "";
                }
            }
            catch
            {
                txtKomisyonTutari.Text = "";
            }
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

        private void LoadMektupDataToForm()
        {
            if (_currentMektup != null)
            {
                txtMektupNo.Text = _currentMektup.mektupNo;
                txtMusteriNo.Text = _currentMektup.musteriNo;
                txtMusteriAdi.Text = _currentMektup.musteriAdi;
                chkTL.Checked = _currentMektup.paraBirimi == "TL";
                chkDolar.Checked = _currentMektup.paraBirimi == "USD";
                chkEuro.Checked = _currentMektup.paraBirimi == "EUR";
                txtTutar.Text = _currentMektup.tutar.ToString("F2");
                cmbBankalar.Text = _currentMektup.banka;
                cmbMektupTuru.Text = _currentMektup.mektupTuru;
                dtVadeTarihi.Value = _currentMektup.vadeTarihi.HasValue ? _currentMektup.vadeTarihi.Value : DateTime.Now;
                dtIadeTarihi.Value = _currentMektup.iadeTarihi;
                txtKomisyonTutari.Text = _currentMektup.komisyonTutari.ToString("F2");
                txtKomisyonOrani.Text = _currentMektup.komisyonOrani.ToString("F2");
                cmbKomisyonVadesi.Text = _currentMektup.komisyonVadesi.ToString();
            }
        }

        private void LoadBankalar()
        {
            if (cmbBankalar == null)
            {
                MessageBox.Show("cmbBankalar kontrolü bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string filePath = ConfigurationManager.AppSettings["ConfigYolu"];

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("App.config dosyasında 'ConfigYolu' ayarı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(filePath))
            {
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
                            cmbBankalar.Items.Add(banka.Trim());
                        }
                    }
                    else
                    {
                        MessageBox.Show("config.txt dosyasında 'Bankalar' satırı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Banka listesi yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Banka listesi dosyası bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void txtKomisyonOrani_TextChanged(object sender, EventArgs e)
        {
            HesaplaKomisyonTutari();
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
        private void dtVadeTarihi_ValueChanged(object sender, EventArgs e)
        {
            UpdateIadeTarihi();
        }

        private void UpdateIadeTarihi()
        {
            DateTime secilenTarih = dtVadeTarihi.Value;

            dtIadeTarihi.Value = secilenTarih;
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
    }
}