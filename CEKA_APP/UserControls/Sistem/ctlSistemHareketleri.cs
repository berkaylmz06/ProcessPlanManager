using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Sistem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlSistemHareketleri : UserControl
    {
        private readonly IServiceProvider _serviceProvider;

        private IKullaniciHareketLogService _kullaniciHareketLogService => _serviceProvider.GetRequiredService<IKullaniciHareketLogService>();
        public ctlSistemHareketleri(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DataTable dt = _kullaniciHareketLogService.GetKullaniciLog();

            dataGridKullaniciLog.DataSource = dt;
            DataGridViewHelper.StilUygulaKullaniciLog(dataGridKullaniciLog);
            tabloDuzenle();

            dataGridKullaniciLog.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }
        private void ctlSistemHareketleri_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Sistem Hareketleri";
            dataGridKullaniciLog.ClearSelection();
        }
        private void dataGridKullaniciLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridKullaniciLog.Columns[e.ColumnIndex].Name == "islemTuru")
            {
                if (e.Value != null)
                {
                    if (Enum.TryParse<IslemTuru>(e.Value.ToString(), out var islem))
                    {
                        e.CellStyle.ForeColor = LogHelper.GetIslemRenk(islem);
                        e.Value = LogHelper.GetIslemEtiketi(islem);
                        e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    }
                }
            }
        }
        public void tabloDuzenle()
        {
            if (dataGridKullaniciLog.Columns.Contains("kullaniciAdi"))
                dataGridKullaniciLog.Columns["kullaniciAdi"].HeaderText = "Kullanıcı Adı";

            if (dataGridKullaniciLog.Columns.Contains("islemTuru"))
                dataGridKullaniciLog.Columns["islemTuru"].HeaderText = "İşlem Türü";

            if (dataGridKullaniciLog.Columns.Contains("sayfaAdi"))
                dataGridKullaniciLog.Columns["sayfaAdi"].HeaderText = "Sayfa Adı";

            if (dataGridKullaniciLog.Columns.Contains("tarihSaat"))
                dataGridKullaniciLog.Columns["tarihSaat"].HeaderText = "Tarih|Saat";

            if (dataGridKullaniciLog.Columns.Contains("ekBilgi"))
                dataGridKullaniciLog.Columns["ekBilgi"].HeaderText = "Ek Bilgi";
        }

        private void dataGridKullaniciLog_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 4)
            {
                var cell = dataGridKullaniciLog[e.ColumnIndex, e.RowIndex];
                string cellText = cell.Value?.ToString();

                dataGridKullaniciLog.ShowCellToolTips = true;
                cell.ToolTipText = !string.IsNullOrEmpty(cellText) ? cellText : "";
            }
        }
    }
}
