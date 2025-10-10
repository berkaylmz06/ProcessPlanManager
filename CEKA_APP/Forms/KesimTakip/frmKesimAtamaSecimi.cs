using CEKA_APP.Interfaces.Muhasebe;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace CEKA_APP.Forms.KesimTakip
{
    public partial class frmKesimAtamaSecimi : Form
    {
        public int SelectedPersonnelId { get; private set; }
        public int SelectedCuttingOrderId { get; private set; }
        private readonly IServiceProvider _serviceProvider;
        private IPersonellerService _personellerService=> _serviceProvider.GetRequiredService<IPersonellerService>();
        public frmKesimAtamaSecimi(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitializeComponent();
        }
        private void frmKesimAtamaSecimi_Load(object sender, EventArgs e)
        {
            var operatorListesi = _personellerService.GetPersonelOperator();

            cmbOperator.DataSource = operatorListesi;
            cmbOperator.DisplayMember = "adSoyad";    
        }
        private void btnTamam_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; 
            this.Close();
        }
    }
}
