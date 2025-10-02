using CEKA_APP.Forms.ProjeTakip;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace CEKA_APP.UserControls.ProjeTakip
{
    public partial class ctlProjeKarti : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        public ctlProjeKarti(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            ctlBaslik1.Baslik = "Proje Kartı";


            chkRefProjeVar.CheckedChanged += (s, e) =>
            {
                if (chkRefProjeVar.Checked) chkRefProjeYok.Checked = false;
                if (chkRefProjeVar.Checked)
                {
                    txtRefProje.Visible = true;
                }
                else
                {
                    txtRefProje.Visible = false;
                }
            };
            chkRefProjeYok.CheckedChanged += (s, e) =>
            {
                if (chkRefProjeYok.Checked) chkRefProjeVar.Checked = false;
            };

            Helper.DataGridViewHelper.StilUygulaUrunGrubuSecim(dataGridUrunGruplari);
            Helper.DataGridViewHelper.StilUygulaUrunGrubuSecim(dataGridUstGruplar);
        }
        private void tbUrunGruplari_Resize(object sender, System.EventArgs e)
        {
            this.tableLayoutPanel1.Left = (this.tbUrunGruplari.ClientSize.Width - this.tableLayoutPanel1.Width) / 2;
            this.tableLayoutPanel1.Top = (this.tbUrunGruplari.ClientSize.Height - this.tableLayoutPanel1.Height) / 2;
        }
        private void btnUrunGrubuEkle_Click(object sender, EventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var frm = ActivatorUtilities.CreateInstance<frmUrunGrubuEkle>(scope.ServiceProvider);
                var result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    var secilenUrunGruplari = frm.SelectedUrunGruplari;

                    if (secilenUrunGruplari != null && secilenUrunGruplari.Count > 0)
                    {
                        dataGridUstGruplar.DataSource = null;

                        dataGridUstGruplar.DataSource = secilenUrunGruplari;

                        dataGridUstGruplar.Columns["urunGrubuId"].Visible = false; 
                        dataGridUstGruplar.Columns["urunGrubu"].HeaderText = "Ürün Grubu";
                        dataGridUstGruplar.Columns["urunGrubuAdi"].HeaderText = "Ürün Grubu Adı";
                    }
                }
            }
        }

    }
}
