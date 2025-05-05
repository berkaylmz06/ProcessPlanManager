using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using System.Xml.Linq;
using iText.Kernel.Pdf.Canvas.Parser;

namespace KesimTakip
{
    public partial class PdfTOXml : Form
    {
        public PdfTOXml()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Dosyaları (*.pdf)|*.pdf";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string pdfText = "";

                    using (PdfReader reader = new PdfReader(ofd.FileName))
                    using (PdfDocument pdfDoc = new PdfDocument(reader))
                    {
                        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                        {
                            var page = pdfDoc.GetPage(i);
                            pdfText += PdfTextExtractor.GetTextFromPage(page) + "\n";
                        }
                    }

                    XElement xml = new XElement("PdfBelgesi", new XElement("Icerik", pdfText));
                    string xmlPath = System.IO.Path.ChangeExtension(ofd.FileName, ".xml");
                    xml.Save(xmlPath);

                    MessageBox.Show("XML dosyası oluşturuldu:\n" + xmlPath);
                }
            }
        }
    }
}
