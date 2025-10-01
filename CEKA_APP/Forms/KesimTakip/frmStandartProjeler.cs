using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.ERP;
using System;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmStandartProjeler : Form
    {
        private readonly IAutoCadAktarimService _autoCadAktarimService;
        public frmStandartProjeler(IAutoCadAktarimService autoCadAktarimService)
        {
            InitializeComponent();

            _autoCadAktarimService = autoCadAktarimService ?? throw new ArgumentNullException(nameof(autoCadAktarimService));

            ListBoxHelper.StilUygula(listGruplar);
            this.Icon = Properties.Resources.cekalogokirmizi;
        }
        private void btnProjeOlustur_Click(object sender, EventArgs e)
        {
            string projeNo = txtProjeNo.Text.Trim();
            string aciklama = txtAciklama.Text.Trim();
            DateTime olusturmaTarihi = dtOlusturmaTarihi.Value;

            if (string.IsNullOrWhiteSpace(projeNo))
            {
                MessageBox.Show("Lütfen proje numarasını girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _autoCadAktarimService.ProjeEkle(projeNo, aciklama, olusturmaTarihi);

                MessageBox.Show(
                    $"Proje '{projeNo}' başarıyla oluşturuldu.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtProjeNo.Clear();
                txtAciklama.Clear();
                dtOlusturmaTarihi.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Proje oluşturulurken bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            string grupNo = txtGrupNo.Text.Trim();

            if (string.IsNullOrWhiteSpace(grupNo))
            {
                MessageBox.Show("Lütfen bir grup numarası girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _autoCadAktarimService.StandartGrupEkle(grupNo);
                MessageBox.Show("Grup başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGrupNo.Clear();

                ListeleGruplar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt sırasında bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmStandartProjeler_Load(object sender, EventArgs e)
        {
            ListeleGruplar();
        }

        private void ListeleGruplar()
        {
            listGruplar.Items.Clear();
            var gruplar = _autoCadAktarimService.GetirStandartGruplarListe();
            listGruplar.Items.AddRange(gruplar.ToArray());
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (listGruplar.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silmek için bir grup seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secilenGrupNo = listGruplar.SelectedItem.ToString();

            var confirmResult = MessageBox.Show(
                $"'{secilenGrupNo}' grubunu silmek istediğinize emin misiniz?",
                "Onay",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    _autoCadAktarimService.StandartGrupSil(secilenGrupNo);
                    MessageBox.Show("Grup başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ListeleGruplar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme işlemi sırasında hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnProjeBagla_Click(object sender, EventArgs e)
        {
            if (listGruplar.SelectedItem == null || string.IsNullOrWhiteSpace(txtProjeBagla.Text))
            {
                MessageBox.Show("Lütfen bir grup ve proje numarası girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string secilenGrup = listGruplar.SelectedItem.ToString();
                string projeAdi = txtProjeBagla.Text.Trim();

                _autoCadAktarimService.BaglaProjeVeGrup(projeAdi, secilenGrup);

                MessageBox.Show($"Grup '{secilenGrup}' başarıyla '{projeAdi}' projesine bağlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtProjeBagla.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
