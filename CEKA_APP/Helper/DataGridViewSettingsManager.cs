using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Specialized;
using CEKA_APP.Properties;

namespace CEKA_APP.Helper
{
    public static class DataGridViewSettingsManager
    {
        public static void SaveColumnWidths(DataGridView dataGridView, string settingsKey)
        {
            try
            {
                var widthsDict = dataGridView.Columns.Cast<DataGridViewColumn>()
                    .ToDictionary(c => c.Name, c => c.Width);

                var widthsString = string.Join(",", widthsDict.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
                Settings.Default[settingsKey] = widthsString;
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sütun genişlikleri kaydedilirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void LoadColumnWidths(DataGridView dataGridView, string settingsKey)
        {
            try
            {
                var widthsString = Settings.Default[settingsKey] as string;
                if (!string.IsNullOrEmpty(widthsString))
                {
                    var widthsDict = widthsString.Split(',')
                        .Select(part => part.Split(':'))
                        .Where(parts => parts.Length == 2 && int.TryParse(parts[1], out _))
                        .ToDictionary(parts => parts[0], parts => int.Parse(parts[1]));

                    foreach (var column in dataGridView.Columns.Cast<DataGridViewColumn>())
                    {
                        if (widthsDict.TryGetValue(column.Name, out int width))
                        {
                            column.Width = width;
                        }
                        else
                        {
                            column.Width = 100;
                        }
                    }
                }
                else
                {
                    var oldWidths = Settings.Default[settingsKey] as StringCollection;
                    if (oldWidths != null && oldWidths.Count > 0)
                    {
                        var columnNames = dataGridView.Columns.Cast<DataGridViewColumn>().Select(c => c.Name).ToList();
                        var widthsDict = new Dictionary<string, int>();
                        for (int i = 0; i < Math.Min(oldWidths.Count, columnNames.Count); i++)
                        {
                            if (int.TryParse(oldWidths[i], out int width))
                            {
                                widthsDict[columnNames[i]] = width;
                            }
                        }

                        widthsString = string.Join(",", widthsDict.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
                        Settings.Default[settingsKey] = widthsString;
                        Settings.Default.Save();

                        foreach (var column in dataGridView.Columns.Cast<DataGridViewColumn>())
                        {
                            if (widthsDict.TryGetValue(column.Name, out int width))
                            {
                                column.Width = width;
                            }
                            else
                            {
                                column.Width = 100;
                            }
                        }
                    }
                    else
                    {
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            column.Width = 100;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sütun genişlikleri yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}