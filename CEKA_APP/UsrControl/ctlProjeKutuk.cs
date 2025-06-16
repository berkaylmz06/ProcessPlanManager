using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlProjeKutuk : UserControl
    {
        public ctlProjeKutuk()
        {
            InitializeComponent();

            txtAltProjeVarMi.Visible=false;

            chkAltProjeVar.CheckedChanged += (s, e) =>
            {
                if (chkAltProjeVar.Checked)
                {
                    chkAltProjeYok.Checked = false;
                }
                else
                {
                    chkAltProjeVar.Visible = false;
                }
            };

            chkAltProjeYok.CheckedChanged += (s, e) =>
            {
                if (chkAltProjeYok.Checked)
                {
                    chkAltProjeVar.Checked = false;
                    txtAltProjeVarMi.Visible = true;
                }
                else
                {
                    chkAltProjeYok.Visible = false;
                }
            };
        }

        private void ctlProjeKutuk_Load(object sender, EventArgs e)
        {

        }
    }
}
