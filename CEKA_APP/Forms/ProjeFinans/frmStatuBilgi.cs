using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Genel;
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
    public partial class frmStatuBilgi : Form
    {
        int projeId;
        private readonly ISayfaStatusService _sayfaStatusService;
        public frmStatuBilgi(int projeId, ISayfaStatusService sayfaStatusService)
        {
            InitializeComponent();

            _sayfaStatusService = sayfaStatusService ?? throw new ArgumentNullException(nameof(sayfaStatusService));


            ListBoxHelper.ProjeFinansStillUygula(listStatuBilgi);
            this.projeId = projeId;
            this.Icon = Properties.Resources.cekalogokirmizi;
        }

        private void frmStatuBilgi_Load(object sender, EventArgs e)
        {
            var statuBilgi = _sayfaStatusService.GetNedenTamamlanmadiByProjeId(projeId);

            listStatuBilgi.Items.Clear();
            foreach (var item in statuBilgi)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    listStatuBilgi.Items.Add(item);
                }
            }

        }
    }
}
