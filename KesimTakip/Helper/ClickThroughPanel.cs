using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KesimTakip.Helper
{
    public class ClickThroughPanel : Panel
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        public ClickThroughPanel()
        {
            this.BackColor = Color.FromArgb(1, 1, 1); // Neredeyse şeffaf, ama çizim yok
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Arka plan çizilmesin = altındaki Chart görünür kalsın
        }
    }
}
