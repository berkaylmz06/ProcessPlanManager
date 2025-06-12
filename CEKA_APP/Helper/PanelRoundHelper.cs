using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Helper
{
    public static class PanelRoundHelper
    {
        public static void RoundCorners(Control control, int radius)
        {
            if (control.Width < 1 || control.Height < 1) return;

            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);

            control.Resize -= Control_Resize;
            control.Resize += Control_Resize;
        }

        private static void Control_Resize(object sender, System.EventArgs e)
        {
            if (sender is Control c)
                RoundCorners(c, 20); 
        }
    }
}
