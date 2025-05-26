using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KesimTakip.Abstracts;

namespace KesimTakip.UsrControl
{
    public partial class ctlSistemBilgisi : UserControl
    {

        public ctlSistemBilgisi()
        {
            InitializeComponent();
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
