using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Helper
{
    public class ChartiTiklanmazYap : Panel
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x20; 
                return cp;
            }
        }

        public ChartiTiklanmazYap()
        {
            this.BackColor = Color.FromArgb(1, 1, 1);
        }
    }
}
