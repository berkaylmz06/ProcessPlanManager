using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CEKA_APP.Abstracts;

namespace CEKA_APP.Concretes
{
    public class FormArayuzu : IFormArayuzu
    {
        private readonly frmAnaSayfa _form;
        public FormArayuzu(frmAnaSayfa form)
        {
            _form = form;
        }
        public string RichTextBox1MetniAl()
        {
            return _form.richTextBox1.Text;
        }
        public string RichTextBox2MetniAl()
        {
            return _form.richTextBox2.Text;
        }
        public string RichTextBox3MetniAl()
        {
            return _form.richTextBox3.Text;
        }
        public string RichTextBox4MetniAl()
        {
            return _form.richTextBox4.Text;
        }
        public void RichTextBox1Temizle()
        {
            _form.richTextBox1.Clear();
        }
        public void RichTextBox2Temizle()
        {
            _form.richTextBox2.Clear();
        }
        public void RichTextBox3Temizle()
        {
            _form.richTextBox3.Clear();
        }
        public void RichTextBox4Temizle()
        {
            _form.richTextBox4.Clear();
        }
        public void RichTextBox1Yaz(string metin)
        {
            _form.richTextBox1.Text = metin;
        }
        public void RichTextBox2Yaz(string metin)
        {
            _form.richTextBox2.Text = metin;
        }
        public void RichTextBox3Yaz(string metin)
        {
            _form.richTextBox3.Text = metin;
        }
        public void RichTextBox4Yaz(string metin)
        {
            _form.richTextBox4.Text = metin;
        }

        public void RichTextBox2MetinEkle(string metin)
        {
            _form.richTextBox2.AppendText(metin);
        }

        public void RichTextBox3MetinEkle(string metin)
        {
            _form.richTextBox3.AppendText(metin);
        }

        public bool RichTextBox1BosMu()
        {
            return string.IsNullOrWhiteSpace(_form.richTextBox1.Text);
        }

        public string[] RichTextBox1SatirlariGetir()
        {
            return _form.richTextBox1.Text
        .Replace("\r\n", "\n")
        .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] RichTextBox4SatirlariAl()
        {
            return _form.richTextBox4.Lines;
        }

        public string[] RichTextBox1SatirlariAl()
        {
            return _form.richTextBox1.Lines;
        }

        public string lblSistemKullaniciMetinAl()
        {
            return _form.lblSistemKullanici.Text;
        }
    }
}
