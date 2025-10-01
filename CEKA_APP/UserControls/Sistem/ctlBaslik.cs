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
    public partial class ctlBaslik : UserControl
    {
        private Label baslikEtiketi;
        public ctlBaslik()
        {
            InitializeComponent();

            baslikEtiketi = new Label
            {
                Text = "Başlık",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = ColorTranslator.FromHtml("#2C3E50"),
                ForeColor = Color.White
            };
            this.Controls.Add(baslikEtiketi);
        }
        public string Baslik
        {
            get => baslikEtiketi.Text;
            set => baslikEtiketi.Text = value;
        }
    }
}
