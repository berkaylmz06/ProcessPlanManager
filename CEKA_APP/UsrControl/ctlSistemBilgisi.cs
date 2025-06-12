using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CEKA_APP.Abstracts;

namespace CEKA_APP.UsrControl
{
    public partial class ctlSistemBilgisi : UserControl
    {

        public ctlSistemBilgisi()
        {
            InitializeComponent();
        }
        private void ctlSistemBilgisi_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Sistem Bilgisi";
        }
        public void FormArayuzuAyarla(IFormArayuzu formArayuzu)
        {
            TextBoxaVeriGetir();
        }
        public void TextBoxaVeriGetir()
        {
          
        }

    }
}
