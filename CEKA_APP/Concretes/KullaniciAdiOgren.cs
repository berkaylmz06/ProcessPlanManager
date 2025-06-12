using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CEKA_APP.Abstracts;

namespace CEKA_APP.Concretes
{
    public class KullaniciAdiOgren : IKullaniciAdiOgren
    {
        private readonly frmAnaSayfa _form;
        public KullaniciAdiOgren(frmAnaSayfa form)
        {
            _form = form;
        }
        public string lblSistemKullaniciMetinAl()
        {
            return _form.lblSistemKullanici.Text;
        }
    }
}
