using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.Forms.KesimTakip
{
    public partial class frmYanUrunGiris : Form
    {
        public Dictionary<string, List<YanUrunDetay>> YanUrunVerileriByKesimId { get; private set; }

        private List<string> _kesimIds;
        private int _currentIndex;
        private string CurrentKesimId => _kesimIds[_currentIndex];

        private DataTable _currentYanUrunTable;

        public frmYanUrunGiris(IServiceProvider serviceProvider, List<string> kesimIds)
        {
            InitializeComponent();

            _kesimIds = kesimIds ?? new List<string>();

            YanUrunVerileriByKesimId = new Dictionary<string, List<YanUrunDetay>>();
            _currentIndex = 0;

            if (_kesimIds.Count == 0)
            {
                MessageBox.Show("Yan ürün girilecek kesim planı bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            SetupDataGridView();
            LoadKesimId();
        }

        private void SetupDataGridView()
        {
            _currentYanUrunTable = new DataTable();
            _currentYanUrunTable.Columns.Add("En", typeof(int));
            _currentYanUrunTable.Columns.Add("Boy", typeof(int));
            _currentYanUrunTable.Columns.Add("Adet", typeof(int));

            dataGridYanUrunler.DataSource = _currentYanUrunTable;

            dataGridYanUrunler.Columns["En"].HeaderText = "Yan Ürün EN";
            dataGridYanUrunler.Columns["Boy"].HeaderText = "Yan Ürün BOY";
            dataGridYanUrunler.Columns["Adet"].HeaderText = "Adet";

            dataGridYanUrunler.ReadOnly = true;
            dataGridYanUrunler.AllowUserToResizeColumns = false; 
            dataGridYanUrunler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
        }

        private void LoadKesimId()
        {
            lblKesimId.Text = $"Kesim Planı No: {CurrentKesimId} ({_currentIndex + 1} / {_kesimIds.Count})";

            _currentYanUrunTable.Clear();

            if (YanUrunVerileriByKesimId.ContainsKey(CurrentKesimId))
            {
                var groupedDetails = YanUrunVerileriByKesimId[CurrentKesimId]
                                     .GroupBy(d => new { d.En, d.Boy })
                                     .Select(g => new { g.Key.En, g.Key.Boy, Adet = g.Sum(d => d.Adet) });


                foreach (var detay in groupedDetails)
                {
                    _currentYanUrunTable.Rows.Add(detay.En, detay.Boy, detay.Adet);
                }
            }

            txtEn.Text = "0";
            txtBoy.Text = "0";
            txtAdet.Text = "0";
            txtEn.Focus();

            if (_currentIndex == _kesimIds.Count - 1)
            {
                btnSiradaki.Text = "Kaydet ve Kapat";
            }
            else
            {
                btnSiradaki.Text = "Sıradaki >";
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (!ValidateYeniGiris())
            {
                return;
            }

            int en = int.Parse(txtEn.Text);
            int boy = int.Parse(txtBoy.Text);
            int adet = int.Parse(txtAdet.Text);

            if (!YanUrunVerileriByKesimId.ContainsKey(CurrentKesimId))
            {
                YanUrunVerileriByKesimId[CurrentKesimId] = new List<YanUrunDetay>();
            }

            YanUrunVerileriByKesimId[CurrentKesimId].Add(new YanUrunDetay { En = en, Boy = boy, Adet = adet });


            LoadKesimId();

            txtEn.Text = "0";
            txtBoy.Text = "0";
            txtAdet.Text = "0";
            txtEn.Focus();
        }

        private bool ValidateYeniGiris()
        {
            if (!int.TryParse(txtEn.Text, out int en) || en < 0)
            {
                MessageBox.Show($"Kesim Planı No {CurrentKesimId} için geçerli bir EN (pozitif tam sayı) giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEn.Focus();
                return false;
            }

            if (!int.TryParse(txtBoy.Text, out int boy) || boy < 0)
            {
                MessageBox.Show($"Kesim Planı No {CurrentKesimId} için geçerli bir BOY (pozitif tam sayı) giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBoy.Focus();
                return false;
            }

            if (!int.TryParse(txtAdet.Text, out int adet) || adet <= 0) 
            {
                MessageBox.Show($"Kesim Planı No {CurrentKesimId} için geçerli bir ADET (pozitif tam sayı) giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAdet.Focus();
                return false;
            }

            return true;
        }

        private void btnSiradaki_Click(object sender, EventArgs e)
        {
            if (_currentIndex == _kesimIds.Count - 1)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                _currentIndex++;
                LoadKesimId();
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
public class YanUrunDetay
{
    public int En { get; set; }
    public int Boy { get; set; }
    public int Adet { get; set; }
}