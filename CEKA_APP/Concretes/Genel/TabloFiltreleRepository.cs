using CEKA_APP.Abstracts.Genel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CEKA_APP.Concretes.Genel
{
    public class TabloFiltreleRepository : ITabloFiltreleRepository
    {
        private string NormalizeColumnName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return columnName;

            return columnName.Replace("ı", "i").Replace("İ", "I").Replace("ş", "s").Replace("Ş", "S")
                            .Replace("ğ", "g").Replace("Ğ", "G").Replace("ü", "u").Replace("Ü", "U")
                            .Replace("ç", "c").Replace("Ç", "C").Replace("ö", "o").Replace("Ö", "O")
                            .ToLower();
        }


        public DataTable GetFilteredData(SqlConnection connection, string baseSql, Dictionary<string, object> filters)
        {
            var trCulture = CultureInfo.GetCultureInfo("tr-TR");
            var sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM (");
            sql.AppendLine(baseSql);
            sql.AppendLine(") AS t WHERE 1=1");

            using (var cmd = new SqlCommand())
            {
                cmd.Connection = connection;

                if (filters?.Any() == true)
                {
                    foreach (var filter in filters)
                    {
                        string rawKey = filter.Key;
                        string columnName = rawKey.Contains(" ")
                            ? $"[{rawKey.Replace("_Baslangic", "").Replace("_Bitis", "")}]"
                            : rawKey.Replace("_Baslangic", "").Replace("_Bitis", "");

                        string paramName = "@" + rawKey.Replace(" ", "_");

                        // Liste kontrolü
                        if (filter.Value is List<string> values)
                        {
                            if (values.Any())
                            {
                                var paramNames = values.Select((v, i) => $"{paramName}_{i}").ToList();
                                sql.Append($" AND {columnName} IN ({string.Join(",", paramNames)})");
                                for (int i = 0; i < values.Count; i++)
                                {
                                    if (int.TryParse(values[i], NumberStyles.Any, trCulture, out int intVal))
                                    {
                                        cmd.Parameters.Add(paramNames[i], SqlDbType.Int).Value = intVal;
                                    }
                                    else if (decimal.TryParse(values[i], NumberStyles.Any, trCulture, out decimal decVal))
                                    {
                                        cmd.Parameters.Add(paramNames[i], SqlDbType.Decimal).Value = decVal;
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add(paramNames[i], SqlDbType.NVarChar).Value = values[i];
                                    }
                                }
                            }
                            continue;
                        }

                        // Mevcut kodun devamı (tek değer için)
                        if (filters.ContainsKey(columnName + "_Baslangic") && filters.ContainsKey(columnName + "_Bitis"))
                        {
                            var basVal = filters[columnName + "_Baslangic"]?.ToString();
                            var bitVal = filters[columnName + "_Bitis"]?.ToString();

                            if (DateTime.TryParse(basVal, trCulture, DateTimeStyles.None, out DateTime basDate) &&
                                DateTime.TryParse(bitVal, trCulture, DateTimeStyles.None, out DateTime bitDate))
                            {
                                if (basDate.Date == bitDate.Date)
                                {
                                    sql.Append($" AND CAST({columnName} AS DATE) = {paramName} ");
                                    cmd.Parameters.Add(paramName, SqlDbType.Date).Value = basDate.Date;
                                }
                                else
                                {
                                    sql.Append($" AND {columnName} >= {paramName}_1 AND {columnName} <= {paramName}_2 ");
                                    cmd.Parameters.Add(paramName + "_1", SqlDbType.DateTime).Value = basDate;
                                    cmd.Parameters.Add(paramName + "_2", SqlDbType.DateTime).Value = bitDate;
                                }
                                continue;
                            }
                        }

                        if (filter.Value is DateTime dtValue)
                        {
                            if (rawKey.EndsWith("_Baslangic"))
                                sql.Append($" AND {columnName} >= {paramName} ");
                            else if (rawKey.EndsWith("_Bitis"))
                                sql.Append($" AND {columnName} <= {paramName} ");

                            cmd.Parameters.Add(paramName, SqlDbType.DateTime).Value = dtValue;
                            continue;
                        }

                        string valueStr = filter.Value?.ToString() ?? string.Empty;

                        if (DateTime.TryParse(valueStr, trCulture, DateTimeStyles.None, out DateTime parsedDate))
                        {
                            if (rawKey.EndsWith("_Baslangic"))
                                sql.Append($" AND {columnName} >= {paramName} ");
                            else if (rawKey.EndsWith("_Bitis"))
                                sql.Append($" AND {columnName} <= {paramName} ");

                            cmd.Parameters.Add(paramName, SqlDbType.DateTime).Value = parsedDate;
                            continue;
                        }

                        string[] operators = new[] { ">=", "<=", "<>", ">", "<", "=" };
                        string usedOperator = operators.FirstOrDefault(op => valueStr.StartsWith(op));

                        if (usedOperator != null)
                        {
                            string realValue = valueStr.Substring(usedOperator.Length).Trim();

                            if (int.TryParse(realValue, NumberStyles.Any, trCulture, out int intVal))
                            {
                                sql.Append($" AND TRY_CAST({columnName} AS INT) {usedOperator} {paramName} ");
                                cmd.Parameters.Add(paramName, SqlDbType.Int).Value = intVal;
                            }
                            else if (decimal.TryParse(realValue, NumberStyles.Any, trCulture, out decimal decVal))
                            {
                                sql.Append($" AND TRY_CAST({columnName} AS DECIMAL(18,2)) {usedOperator} {paramName} ");
                                cmd.Parameters.Add(paramName, SqlDbType.Decimal).Value = decVal;
                            }
                            else
                            {
                                sql.Append($" AND CAST({columnName} AS NVARCHAR) {usedOperator} {paramName} ");
                                cmd.Parameters.Add(paramName, SqlDbType.NVarChar).Value = realValue;
                            }
                        }
                        else if (valueStr.Contains("%") || valueStr.Contains("_") || valueStr.Contains("["))
                        {
                            sql.Append($" AND CAST({columnName} AS NVARCHAR) LIKE {paramName} ");
                            cmd.Parameters.Add(paramName, SqlDbType.NVarChar).Value = valueStr;
                        }
                        else
                        {
                            if (int.TryParse(valueStr, NumberStyles.Any, trCulture, out int intVal))
                            {
                                sql.Append($" AND TRY_CAST({columnName} AS INT) = {paramName} ");
                                cmd.Parameters.Add(paramName, SqlDbType.Int).Value = intVal;
                            }
                            else if (decimal.TryParse(valueStr, NumberStyles.Any, trCulture, out decimal decVal))
                            {
                                sql.Append($" AND TRY_CAST({columnName} AS DECIMAL(18,2)) = {paramName} ");
                                cmd.Parameters.Add(paramName, SqlDbType.Decimal).Value = decVal;
                            }
                            else
                            {
                                sql.Append($" AND CAST({columnName} AS NVARCHAR) = {paramName} ");
                                cmd.Parameters.Add(paramName, SqlDbType.NVarChar).Value = valueStr;
                            }
                        }
                    }
                }

                sql.AppendLine(" ORDER BY 1");
                cmd.CommandText = sql.ToString();

                var dt = new DataTable();
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
                return dt;
            }
        }
    }
}
