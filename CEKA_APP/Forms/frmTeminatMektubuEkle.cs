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

        public frmTeminatMektubuEkle(TeminatMektuplari teminatMektup)
        {
            InitializeComponent();

            this.Icon = Properties.Resources.cekalogokirmizi;

            if (teminatMektup != null)
            {
                _currentMektup = teminatMektup;
                _isUpdateMode = true;
                _originalMektupNoForUpdate = teminatMektup.mektupNo; // Changed from musteriNo to mektupNo
                this.Text = "Teminat Mektubu Bilgilerini Güncelle"; // Updated form title
                btnKaydet.Text = "Güncelle";
                LoadMektupDataToForm();
                txtMektupNo.Enabled = false;
            }
            else
            {
                _currentMektup = new TeminatMektuplari();
                _isUpdateMode = false;
                this.Text = "Yeni Teminat Mektubu Ekle"; // Updated form title
                btnKaydet.Text = "Kaydet";
                ClearFormFields(); // Added to clear fields for new entry
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
            txtMektupTuru.Clear();
            dtVadeTarihi.Value = DateTime.Now;
            dtIadeTarihi.Value = DateTime.Now;
            txtKomisyonTutari.Clear();
            txtKomisyonOrani.Clear();
            txtKomisyonVadesi.Clear();
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
                txtMektupTuru.Text = _currentMektup.mektupTuru;
                dtVadeTarihi.Value = _currentMektup.vadeTarihi;
                dtIadeTarihi.Value = _currentMektup.iadeTarihi;
                txtKomisyonTutari.Text = _currentMektup.komisyonTutari.ToString("F2");
                txtKomisyonOrani.Text = _currentMektup.komisyonOrani.ToString("F2");
                txtKomisyonVadesi.Text = _currentMektup.komisyonVadesi.ToString();
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
            {
                paraBirimi = "TL";
            }
            else if (chkDolar.Checked)
            {
                paraBirimi = "USD";
            }
            else if (chkEuro.Checked)
            {
                paraBirimi = "EUR";
            }
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
            string mektupTuru = txtMektupTuru.Text.Trim();
            DateTime vadeTarihi = dtVadeTarihi.Value;
            DateTime iadeTarihi = dtIadeTarihi.Value;

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
                    // Güncelleme Modu
                    _currentMektup.mektupNo = mektupNo;
                    _currentMektup.musteriNo = musteriNo;
                    _currentMektup.musteriAdi = musteriAdi;
                    _currentMektup.paraBirimi = paraBirimi;
                    _currentMektup.tutar = tutar;
                    _currentMektup.banka = banka;
                    _currentMektup.mektupTuru = mektupTuru;
                    _currentMektup.vadeTarihi = vadeTarihi;
                    _currentMektup.iadeTarihi = iadeTarihi;
                    _currentMektup.komisyonTutari = komisyonTutari;
                    _currentMektup.komisyonOrani = komisyonOrani;
                    _currentMektup.komisyonVadesi = komisyonVadesi;

                    // Assuming there's an Update method similar to MusteriGuncelle
                    mektupData.MektupGuncelle(_originalMektupNoForUpdate, _currentMektup);
                    MessageBox.Show("Teminat Mektubu başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // Ekleme Modu
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
                        _currentMektup.iadeTarihi = iadeTarihi;
                        _currentMektup.komisyonTutari = komisyonTutari;
                        _currentMektup.komisyonOrani = komisyonOrani;
                        _currentMektup.komisyonVadesi = komisyonVadesi;

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
        }

        private void ApplyCharacterLimits()
        {
            if (txtMektupNo != null)
            {
                txtMektupNo.MaxLength = 50;
            }
            if (txtMusteriNo != null)
            {
                txtMusteriNo.MaxLength = 50;
            }
            if (txtMusteriAdi != null)
            {
                txtMusteriAdi.MaxLength = 250;
            }
            if (txtBanka != null)
            {
                txtBanka.MaxLength = 250;
            }
            if (txtMektupTuru != null)
            {
                txtMektupTuru.MaxLength = 250;
            }
            // Tutar, Komisyon Tutarı, Komisyon Oranı ve Komisyon Vadesi sayısal alanlar olduğu için MaxLength ayarına gerek yoktur.
            // Bu alanlar için TextBox yerine NumericUpDown kontrolü kullanmak daha uygun olabilir.
        }
    }
}