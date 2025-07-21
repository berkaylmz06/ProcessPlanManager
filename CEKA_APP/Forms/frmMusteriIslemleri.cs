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
    public partial class frmMusteriIslemleri : Form
    {
        private Musteriler _currentMusteri;
        private bool _isUpdateMode = false;
        private string _originalMusteriNoForUpdate; 

        public frmMusteriIslemleri(Musteriler musteri)
        {
            InitializeComponent();

            this.Icon = Properties.Resources.cekalogokirmizi;

            if (musteri != null)
            {
                _currentMusteri = musteri;
                _isUpdateMode = true;
                _originalMusteriNoForUpdate = musteri.musteriNo; 
                this.Text = "Müşteri Bilgilerini Güncelle";
                btnKaydet.Text = "Güncelle";
                LoadMusteriDataToForm();
                txtMusteriNo.Enabled = false; 
            }
            else
            {
                _currentMusteri = new Musteriler();
                _isUpdateMode = false;
                this.Text = "Yeni Müşteri Ekle";
                btnKaydet.Text = "Kaydet";
            }
        }

        private void ClearFormFields()
        {
            txtMusteriNo.Clear();
            txtMusteriAdi.Clear();
            txtMusteriMensei.Clear();
            txtVergiNo.Clear();
            txtVergiDairesi.Clear();
            txtAdres.Clear();
        }

        private void LoadMusteriDataToForm()
        {
            if (_currentMusteri != null)
            {
                txtMusteriNo.Text = _currentMusteri.musteriNo;
                txtMusteriAdi.Text = _currentMusteri.musteriAdi;
                txtMusteriMensei.Text = _currentMusteri.musteriMensei;
                txtVergiNo.Text = _currentMusteri.vergiNo;
                txtVergiDairesi.Text = _currentMusteri.vergiDairesi;
                txtAdres.Text = _currentMusteri.adres;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            string musteriNo = txtMusteriNo.Text.Trim();
            string musteriAdi = txtMusteriAdi.Text.Trim();
            string musteriMensei = txtMusteriMensei.Text.Trim();
            string vergiNo = txtVergiNo.Text.Trim();
            string vergiDairesi = txtVergiDairesi.Text.Trim();
            string adres = txtAdres.Text.Trim();

            if (string.IsNullOrWhiteSpace(musteriNo) || string.IsNullOrWhiteSpace(musteriAdi))
            {
                MessageBox.Show("Müşteri No ve Müşteri Adı boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProjeFinans_MusterilerData musteriData = new ProjeFinans_MusterilerData();

            try
            {
                if (_isUpdateMode)
                {
                    // Güncelleme Modu
                    // _currentMusteri nesnesini formdaki yeni değerlerle doldur
                    _currentMusteri.musteriNo = musteriNo; // musteriNo değişmese de güncel değeri atayalım
                    _currentMusteri.musteriAdi = musteriAdi;
                    _currentMusteri.musteriMensei = musteriMensei;
                    _currentMusteri.vergiNo = vergiNo;
                    _currentMusteri.vergiDairesi = vergiDairesi;
                    _currentMusteri.adres = adres;

                    // Eğer musteriNo değiştirilmeye çalışıldıysa (ki Enabled=false olduğu için teorik olarak imkansız ama önlem)
                    if (_originalMusteriNoForUpdate != musteriNo)
                    {
                        // Bu durum sadece txtMusteriNo.Enabled = true olsaydı gerçekleşirdi.
                        // PK değişimi ekstra kontrol gerektirir. Şu an için engelli olduğu için bu kontrole gerek yok.
                        // Ancak ileride PK değiştirilebilir olursa, musteriNoVarMi kontrolü yapılmalı.
                    }

                    musteriData.MusteriGuncelle(_originalMusteriNoForUpdate, _currentMusteri); // Eski PK ile güncelle
                    MessageBox.Show("Müşteri başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; // Ana forma başarılı dönüş sinyali
                    this.Close(); // Formu kapat
                }
                else
                {
                    // Ekleme Modu
                    if (musteriData.MusteriNoVarMi(musteriNo))
                    {
                        MessageBox.Show($"'{musteriNo}' müşteri numarası zaten mevcut. Lütfen farklı bir numara girin.", "Mevcut Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _currentMusteri.musteriNo = musteriNo;
                        _currentMusteri.musteriAdi = musteriAdi;
                        _currentMusteri.musteriMensei = musteriMensei;
                        _currentMusteri.vergiNo = vergiNo;
                        _currentMusteri.vergiDairesi = vergiDairesi;
                        _currentMusteri.adres = adres;

                        musteriData.MusteriKaydet(_currentMusteri);
                        MessageBox.Show("Müşteri başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void frmMusteriIslemleri_Load(object sender, EventArgs e)
        {
            ApplyCharacterLimits();
        }
        private void ApplyCharacterLimits()
        {
            if (txtMusteriNo != null)
            {
                txtMusteriNo.MaxLength = 50;
            }

            if (txtMusteriAdi != null)
            {
                txtMusteriAdi.MaxLength = 250;
            }

            if (txtMusteriMensei != null)
            {
                txtMusteriMensei.MaxLength = 250;
            }

            if (txtVergiNo != null)
            {
                txtVergiNo.MaxLength = 50;
            }

            if (txtVergiDairesi != null)
            {
                txtVergiDairesi.MaxLength = 250;
            }

            if (txtAdres != null)
            {
                txtAdres.MaxLength = 500;
            }

        }
    }
}
