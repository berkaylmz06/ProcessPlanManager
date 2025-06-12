using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts
{
    public interface IFormArayuzu
    {
        string RichTextBox1MetniAl();
        string RichTextBox2MetniAl();
        string RichTextBox3MetniAl();
        string RichTextBox4MetniAl();
        void RichTextBox1Yaz(string metin);
        void RichTextBox2Yaz(string metin);
        void RichTextBox3Yaz(string metin);

        void RichTextBox4Yaz(string metin);
        void RichTextBox1Temizle();
        void RichTextBox2Temizle();
        void RichTextBox3Temizle();
        void RichTextBox4Temizle();

        void RichTextBox2MetinEkle(string metin);
        void RichTextBox3MetinEkle(string metin);
        bool RichTextBox1BosMu();

        string[] RichTextBox1SatirlariGetir();

        string[] RichTextBox4SatirlariAl();
        string[] RichTextBox1SatirlariAl();
        string lblSistemKullaniciMetinAl();
    }
}
