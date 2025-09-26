using CEKA_APP.Forms;
using CEKA_APP.Interfaces.Genel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmAra : Form
    {
        private readonly Action<DataTable> _aramaSonucuCallback;
        private readonly bool _detayEklenecekMi;
        private static Dictionary<string, string> _sonFiltreKriterleri = new Dictionary<string, string>();
        private Dictionary<string, Control> _filtreKutulari = new Dictionary<string, Control>();
        private readonly string _sourceSql;

        private Dictionary<string, Type> _columnTypes = new Dictionary<string, Type>();

        private readonly ITabloFiltreleService _tabloFiltreleService;
        private readonly IServiceProvider _serviceProvider;


        public frmAra(DataGridView dataGrid, ITabloFiltreleService tabloFiltreleService, Action<DataTable> callback, string sourceSql, IServiceProvider serviceProvider, bool detayEkle = false)
        {
            InitializeComponent();
            _tabloFiltreleService = tabloFiltreleService;
            _aramaSonucuCallback = callback;
            _detayEklenecekMi = detayEkle;
            _sourceSql = sourceSql;
            _serviceProvider = serviceProvider;


            AraFormDinamikLabel(dataGrid.Columns);
            this.Icon = Properties.Resources.cekalogokirmizi;
        }

        public frmAra()
        {
            InitializeComponent();
        }

        private void AraFormDinamikLabel(DataGridViewColumnCollection columns)
        {
            panelFiltreler.Controls.Clear();
            panelFiltreler.AutoScroll = true;
            _filtreKutulari.Clear();
            _columnTypes.Clear();

            int yOffset = 10;
            int xOffset = 10;
            int controlWidth = 150;
            int labelWidth = 150;

            var sortedColumns = columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible && !string.IsNullOrEmpty(c.DataPropertyName))
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            foreach (var column in sortedColumns)
            {
                string columnKey = column.DataPropertyName;
                if (string.IsNullOrEmpty(columnKey))
                    continue;

                Type colType = column.ValueType ?? typeof(string);
                _columnTypes[columnKey] = colType;

                if (column.HeaderText.Contains("Tarih"))
                {
                    var lblBaslangic = new Label
                    {
                        Text = column.HeaderText + " (Başlangıç):",
                        AutoSize = true,
                        Location = new Point(xOffset, yOffset + 5)
                    };
                    panelFiltreler.Controls.Add(lblBaslangic);

                    var dtpBaslangic = new DateTimePicker
                    {
                        Format = DateTimePickerFormat.Custom,
                        CustomFormat = "dd.MM.yyyy",
                        ShowCheckBox = true,
                        Checked = false,
                        Name = columnKey + "_Baslangic",
                        Location = new Point(xOffset + labelWidth, yOffset),
                        Width = controlWidth
                    };
                    panelFiltreler.Controls.Add(dtpBaslangic);
                    _filtreKutulari[dtpBaslangic.Name] = dtpBaslangic;

                    if (_sonFiltreKriterleri.TryGetValue(dtpBaslangic.Name, out var val1) &&
                        DateTime.TryParse(val1, out var parsed1))
                    {
                        dtpBaslangic.Value = parsed1;
                        dtpBaslangic.Checked = true;
                    }

                    yOffset += Math.Max(lblBaslangic.Height, dtpBaslangic.Height) + 10;

                    var lblBitis = new Label
                    {
                        Text = column.HeaderText + " (Bitiş):",
                        AutoSize = true,
                        Location = new Point(xOffset, yOffset + 5)
                    };
                    panelFiltreler.Controls.Add(lblBitis);

                    var dtpBitis = new DateTimePicker
                    {
                        Format = DateTimePickerFormat.Custom,
                        CustomFormat = "dd.MM.yyyy",
                        ShowCheckBox = true,
                        Checked = false,
                        Name = columnKey + "_Bitis",
                        Location = new Point(xOffset + labelWidth, yOffset),
                        Width = controlWidth
                    };
                    panelFiltreler.Controls.Add(dtpBitis);
                    _filtreKutulari[dtpBitis.Name] = dtpBitis;

                    if (_sonFiltreKriterleri.TryGetValue(dtpBitis.Name, out var val2) &&
                        DateTime.TryParse(val2, out var parsed2))
                    {
                        dtpBitis.Value = parsed2;
                        dtpBitis.Checked = true;
                    }

                    yOffset += Math.Max(lblBitis.Height, dtpBitis.Height) + 10;
                }
                else if (column.HeaderText == "Durum")
                {
                    var lbl = new Label
                    {
                        Text = column.HeaderText + ":",
                        AutoSize = true,
                        Location = new Point(xOffset, yOffset + 5)
                    };
                    panelFiltreler.Controls.Add(lbl);

                    var cmb = new ComboBox
                    {
                        Name = columnKey,
                        Tag = column.HeaderText,
                        Location = new Point(xOffset + labelWidth, yOffset),
                        Width = controlWidth,
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };
                    cmb.Items.AddRange(new object[] { "", "Ödendi", "Bekliyor" });
                    panelFiltreler.Controls.Add(cmb);
                    _filtreKutulari[columnKey] = cmb;

                    if (_sonFiltreKriterleri.TryGetValue(columnKey, out var val))
                        cmb.SelectedItem = val;

                    yOffset += Math.Max(lbl.Height, cmb.Height) + 10;
                }
                else
                {
                    var lbl = new Label
                    {
                        Text = column.HeaderText + ":",
                        AutoSize = true,
                        Location = new Point(xOffset, yOffset + 5)
                    };
                    panelFiltreler.Controls.Add(lbl);

                    var txt = new TextBox
                    {
                        Name = columnKey,
                        Tag = column.HeaderText,
                        Location = new Point(xOffset + labelWidth, yOffset),
                        Width = controlWidth
                    };
                    txt.KeyDown += btnAra_KeyDown;

                    if (_sonFiltreKriterleri.TryGetValue(columnKey, out var val))
                        txt.Text = val;

                    panelFiltreler.Controls.Add(txt);
                    _filtreKutulari[columnKey] = txt;

                    if (column.HeaderText.Contains("Müşteri No"))
                    {
                        var btnSec = new Button
                        {
                            Text = "...",
                            Width = 30,
                            Height = txt.Height,
                            Location = new Point(txt.Right + 5, yOffset),
                            Tag = txt
                        };
                        btnSec.Click += BtnMusteriSec_Click;
                        panelFiltreler.Controls.Add(btnSec);
                    }

                    yOffset += Math.Max(lbl.Height, txt.Height) + 10;
                }
            }
        }
        private bool IsNumericType(Type t)
        {
            if (t == null) return false;
            var tc = Type.GetTypeCode(t);
            return tc == TypeCode.Byte || tc == TypeCode.SByte ||
                   tc == TypeCode.Int16 || tc == TypeCode.UInt16 ||
                   tc == TypeCode.Int32 || tc == TypeCode.UInt32 ||
                   tc == TypeCode.Int64 || tc == TypeCode.UInt64 ||
                   tc == TypeCode.Decimal || tc == TypeCode.Double || tc == TypeCode.Single;
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            var filters = new Dictionary<string, object>();
            _sonFiltreKriterleri.Clear();
            var invalidFilters = new List<string>();
            var trCulture = CultureInfo.GetCultureInfo("tr-TR");
            string[] operators = new[] { ">=", "<=", "<>", ">", "<", "=" };

            foreach (var item in _filtreKutulari)
            {
                var key = item.Key;
                var control = item.Value;

                if (control is TextBox txt && !string.IsNullOrWhiteSpace(txt.Text))
                {
                    string raw = txt.Text.Trim();
                    _columnTypes.TryGetValue(key, out Type colType);
                    if (colType == null)
                        colType = typeof(string);

                    if (raw.Contains("; "))
                    {
                        var values = raw.Split(new[] { "; " }, StringSplitOptions.None).Select(v => v.Trim()).Where(v => !string.IsNullOrEmpty(v)).ToList();
                        if (values.Any())
                        {
                            filters[key] = values;
                            _sonFiltreKriterleri[key] = raw;
                        }
                        continue;
                    }

                    if (IsNumericType(colType))
                    {
                        var usedOp = operators.FirstOrDefault(op => raw.StartsWith(op));
                        var realValue = usedOp != null ? raw.Substring(usedOp.Length).Trim() : raw;

                        if (realValue.Contains("%") || realValue.Contains("_") || realValue.Contains("["))
                        {
                            invalidFilters.Add($"{txt.Tag ?? key} = '{raw}' (sayısal bekleniyor)");
                            continue;
                        }

                        if (decimal.TryParse(realValue, NumberStyles.Any, trCulture, out _))
                        {
                            filters[key] = raw;
                            _sonFiltreKriterleri[key] = raw;
                        }
                        else
                        {
                            invalidFilters.Add($"{txt.Tag ?? key} = '{raw}' (geçerli sayı değil)");
                        }
                    }
                    else if (colType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(raw, trCulture, DateTimeStyles.None, out DateTime dt))
                        {
                            filters[key] = dt;
                            _sonFiltreKriterleri[key] = raw;
                        }
                        else
                        {
                            invalidFilters.Add($"{txt.Tag ?? key} = '{raw}' (geçerli tarih değil)");
                        }
                    }
                    else
                    {
                        filters[key] = raw;
                        _sonFiltreKriterleri[key] = raw;
                    }
                }
                else if (control is DateTimePicker dtp && dtp.Checked)
                {
                    filters[key] = dtp.Value;
                    _sonFiltreKriterleri[key] = dtp.Value.ToShortDateString();
                }
                else if (control is ComboBox cmb && !string.IsNullOrEmpty(cmb.SelectedItem?.ToString()))
                {
                    string selectedValue = cmb.SelectedItem.ToString();
                    filters[key] = selectedValue;
                    _sonFiltreKriterleri[key] = selectedValue;
                }
            }

            if (invalidFilters.Any())
            {
                string msg = "Aşağıdaki filtre değerleri sütun tipine uymuyor:\n\n" +
                             string.Join("\n", invalidFilters) +
                             "\n\nLütfen düzeltip tekrar deneyin.";
                MessageBox.Show(msg, "Filtre Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable sonucTablo = _tabloFiltreleService.GetFilteredData(_sourceSql, filters);

            if (sonucTablo == null || sonucTablo.Rows.Count == 0)
            {
                MessageBox.Show("Arama sonucu bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_detayEklenecekMi && !sonucTablo.Columns.Contains("Detay"))
            {
                sonucTablo.Columns.Add("Detay", typeof(string));
                foreach (DataRow row in sonucTablo.Rows)
                    row["Detay"] = "Detay Görmek İçin Tıklayınız.";
            }

            _aramaSonucuCallback?.Invoke(sonucTablo);
            this.Close();
        }
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            foreach (var control in _filtreKutulari.Values)
            {
                switch (control)
                {
                    case TextBox txt:
                        txt.Text = string.Empty;
                        break;
                    case DateTimePicker dtp:
                        dtp.Checked = false;
                        break;
                    case ComboBox cmb:
                        cmb.SelectedIndex = 0;
                        break;
                }
            }
            _sonFiltreKriterleri.Clear();
        }

        private void btnAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAra_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
        private void BtnMusteriSec_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is TextBox targetTxt)
            {
                using (var frm = new frmAraBilgi(_serviceProvider))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        string yeniMusteriNo = frm.SecilenMusteriNo;
                        if (!string.IsNullOrEmpty(yeniMusteriNo))
                        {
                            if (!string.IsNullOrWhiteSpace(targetTxt.Text))
                            {
                                var mevcutVeriler = targetTxt.Text.Split(new[] { "; " }, StringSplitOptions.None).Select(v => v.Trim()).ToList();
                                if (!mevcutVeriler.Contains(yeniMusteriNo))
                                {
                                    targetTxt.Text += $"; {yeniMusteriNo}";
                                }
                            }
                            else
                            {
                                targetTxt.Text = yeniMusteriNo;
                            }
                        }
                    }
                }
            }
        }
    }
}
