using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_OdemeSekilleriData
    {
        public void OdemeBilgiKaydet(string projeNo, string kilometreTasiId, int siralama, string oran, string tutar, string tarih, string aciklama, string durum)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO ProjeFinans_OdemeSekilleri 
                        (projeNo, kilometreTasiId, siralama, oran, tutar, tarih, aciklama, durum)
                        VALUES (@projeNo, @kilometreTasiId, @siralama, @oran, @tutar, @tarih, @aciklama, @durum)";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add("@projeNo", SqlDbType.NVarChar, 50).Value = projeNo?.Trim() ?? throw new ArgumentNullException(nameof(projeNo));
                        cmd.Parameters.Add("@kilometreTasiId", SqlDbType.Int).Value = int.Parse(kilometreTasiId);
                        cmd.Parameters.Add("@siralama", SqlDbType.Int).Value = siralama;
                        cmd.Parameters.Add("@oran", SqlDbType.Decimal).Value = decimal.Parse(oran, System.Globalization.CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@tutar", SqlDbType.Decimal).Value = decimal.Parse(tutar, System.Globalization.CultureInfo.InvariantCulture);
                        cmd.Parameters.Add("@tarih", SqlDbType.DateTime2).Value = string.IsNullOrWhiteSpace(tarih) ? (object)DBNull.Value : DateTime.Parse(tarih);
                        cmd.Parameters.Add("@aciklama", SqlDbType.NVarChar, 250).Value = string.IsNullOrWhiteSpace(aciklama) ? (object)DBNull.Value : aciklama;
                        cmd.Parameters.Add("@durum", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(durum) ? (object)DBNull.Value : durum;

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ödeme kaydı eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
    }
}
