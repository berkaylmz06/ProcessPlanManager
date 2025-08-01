using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public frmTeminatMektubuEkle(TeminatMektuplari teminatMektup, string projeNo = null)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.cekalogokirmizi;
            _projeNo = projeNo; // Proje numarasını ata

            if (teminatMektup != null)
            {
                _currentMektup = teminatMektup;
                _isUpdateMode = true;
                _originalMektupNoForUpdate = teminatMektup.mektupNo;
                this.Text = "Teminat Mektubu Bilgilerini Güncelle";
                btnKaydet.Text = "Güncelle";
                LoadMektupDataToForm();
                txtMektupNo.Enabled = false;
            }
            else
            {
                _currentMektup = new TeminatMektuplari();
                _isUpdateMode = false;
                this.Text = "Yeni Teminat Mektubu Ekle";
                btnKaydet.Text = "Kaydet";
                ClearFormFields();
                LoadProjeKutukData();
            }

            // ComboBox değişikliğinde vade tarihi kontrolü
            cmbMektupTuru.SelectedIndexChanged += CmbMektupTuru_SelectedIndexChanged;
        }

        private void CmbMektupTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            // "Kesin" veya "Kati" seçilirse vade tarihi devre dışı bırakılır
            if (cmbMektupTuru.SelectedItem != null &&
                (cmbMektupTuru.SelectedItem.ToString() == "Kesin" || cmbMektupTuru.SelectedItem.ToString() == "Kati"))
            {
                dtVadeTarihi.Enabled = false;
                dtVadeTarihi.Value = DateTime.Now; // Varsayılan değer, kaydedilirken null olacak
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
            txtBanka.Clear();
            cmbMektupTuru.SelectedIndex = -1; // ComboBox'ı sıfırla
            dtVadeTarihi.Value = DateTime.Now;
            dtIadeTarihi.Value = DateTime.Now;
            txtKomisyonTutari.Clear();
            txtKomisyonOrani.Clear();
            txtKomisyonVadesi.Clear();
        }

        private void LoadProjeKutukData()
        {
            if (!string.IsNullOrEmpty(_projeNo))
            {
                var projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(_projeNo);
                string effectiveProjeNo = _projeNo; // Varsayılan olarak mevcut proje numarası

                if (projeKutuk == null)
                {
                    // Alt proje kontrolü
                    if (ProjeFinans_ProjeIliskiData.CheckAltProje(_projeNo))
                    {
                        // Alt proje ise üst projenin numarasını al
                        string ustProjeNo = ProjeFinans_ProjeIliskiData.GetUstProjeNo(_projeNo);
                        if (!string.IsNullOrEmpty(ustProjeNo))
                        {
                            effectiveProjeNo = ustProjeNo; // Üst proje numarasını kullan
                            projeKutuk = ProjeFinans_ProjeKutukData.ProjeKutukAra(ustProjeNo);
                        }
                    }
                }

                // ProjeNo'yu _currentMektup'a ata (alt proje ise üst proje numarası)
                _currentMektup.projeNo = effectiveProjeNo;

                if (projeKutuk != null)
                {
                    txtMusteriNo.Text = projeKutuk.musteriNo ?? "";
                    txtMusteriAdi.Text = projeKutuk.musteriAdi ?? "";
                }
                else
                {
                    MessageBox.Show($"Proje '{_projeNo}' için kütük bilgileri bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMusteriNo.Text = "";
                    txtMusteriAdi.Text = "";
                }
            }
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

            string banka = txtBanka.Text.Trim();
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

            if (!int.TryParse(txtKomisyonVadesi.Text, out int komisyonVadesi))
            {
                MessageBox.Show("Lütfen geçerli bir komisyon vadesi sayısı giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProjeFinans_TeminatMektuplariData mektupData = new ProjeFinans_TeminatMektuplariData();

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
                    _currentMektup.projeNo = _projeNo; // Güncelleme modunda mevcut projeNo'yu koru

                    mektupData.MektupGuncelle(_originalMektupNoForUpdate, _currentMektup);
                    MessageBox.Show("Teminat Mektubu başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    if (mektupData.MektupNoVarMi(mektupNo))
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
                        // projeNo'yu LoadProjeKutukData'da zaten ayarladık, buraya gerek yok

                        mektupData.TeminatMektubuKaydet(_currentMektup);
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

        private void frmTeminatMektubuEkle_Load(object sender, EventArgs e)
        {
            ApplyCharacterLimits();
            // Form yüklenirken mevcut mektup türüne göre vade tarihi durumunu ayarla
            if (_isUpdateMode && _currentMektup != null)
            {
                if (_currentMektup.mektupTuru == "Kesin" || _currentMektup.mektupTuru == "Kati")
                {
                    dtVadeTarihi.Enabled = false;
                }
                // ProjeNo'yu güncelle modunda arka planda ata (mevcut projeNo'yu koru)
                _currentMektup.projeNo = _projeNo;
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
            if (txtBanka != null)
                txtBanka.MaxLength = 250;
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
                txtBanka.Text = _currentMektup.banka;
                cmbMektupTuru.Text = _currentMektup.mektupTuru;
                dtVadeTarihi.Value = _currentMektup.vadeTarihi.HasValue ? _currentMektup.vadeTarihi.Value : DateTime.Now;
                dtIadeTarihi.Value = _currentMektup.iadeTarihi;
                txtKomisyonTutari.Text = _currentMektup.komisyonTutari.ToString("F2");
                txtKomisyonOrani.Text = _currentMektup.komisyonOrani.ToString("F2");
                txtKomisyonVadesi.Text = _currentMektup.komisyonVadesi.ToString();
            }
        }
    }
}