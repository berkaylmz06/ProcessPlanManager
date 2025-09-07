using System;
using System.Data;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Windows.Forms;

namespace CEKA_APP.DataBase
{
    class DataBaseHelper
    {
        private static readonly string productionConnectionString = "Server=192.168.2.22;Database=CEKA_APP;User Id=sa;Password=DOibg544;TrustServerCertificate=True;";
        //private static readonly string developmentConnectionString = "Server=IFS-DEVELOPER\\SQLEXPRESS;Database=CEKA_APP;Integrated Security=True;TrustServerCertificate=True;";
        private static readonly string developmentConnectionString = "Server=.\\SQLSERVER_2022;Database=CEKA_APP;Integrated Security=True;TrustServerCertificate=True;";
        private static readonly string connectionString = IsPublished() ? productionConnectionString : developmentConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        private static bool IsPublished()
        {
            try
            {
                return ApplicationDeployment.IsNetworkDeployed;
            }
            catch
            {
                return false;
            }
        }
        public static void ProjeDurumunuGuncelle(int projeId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var cmd = new SqlCommand("sp_UpdateProjeKutukDurum", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@projeId", projeId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Proje durumunu güncellerken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}