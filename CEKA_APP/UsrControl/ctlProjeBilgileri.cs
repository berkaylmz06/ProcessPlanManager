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
    public partial class ctlProjeBilgileri : UserControl
    {
        public event Action OnKaydet; // Kaydetme olayını bildirmek için

        public ctlProjeBilgileri()
        {
            InitializeComponent();
        }

        public void LoadProjects(List<string> projects)
        {
            flowLayoutPanelProjeler.Controls.Clear();

            foreach (var proje in projects)
            {
                var card = new Panel
                {
                    Size = new Size(200, 100),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White,
                    Margin = new Padding(10)
                };

                var lblProje = new Label
                {
                    Text = proje,
                    Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(10, 10),
                    ForeColor = Color.FromArgb(44, 62, 80)
                };

                var btnKaydet = new Button
                {
                    Text = "Kaydet",
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Size = new Size(80, 25),
                    Location = new Point(10, 60)
                };
                btnKaydet.Click += (s, e) =>
                {
                    MessageBox.Show($"Proje '{proje}' için bilgiler kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnKaydet?.Invoke(); // Kaydetme olayını tetikle
                };

                card.Controls.Add(lblProje);
                card.Controls.Add(btnKaydet);
                flowLayoutPanelProjeler.Controls.Add(card);
            }
        }
    }
}