using CEKA_APP.Abstracts.Genel;
using System.Data.SqlClient;
using System.Deployment.Application;

namespace CEKA_APP.Concretes.Genel
{
    public class DataBaseRepository : IDataBaseRepository
    {
        private readonly string connectionString;
        private readonly string productionConnectionString = "Server=192.168.2.22;Database=CEKA_APP;User Id=sa;Password=DOibg544;TrustServerCertificate=True;";
        private static readonly string developmentConnectionString = "Server=IFS-DEVELOPER\\SQLEXPRESS;Database=CEKA_APP;Integrated Security=True;TrustServerCertificate=True;";
        //private readonly string developmentConnectionString = "Server=.\\SQLSERVER_2022;Database=CEKA_APP;Integrated Security=True;TrustServerCertificate=True;";
        

        public DataBaseRepository()
        {
            connectionString = IsPublished() ? productionConnectionString : developmentConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        private bool IsPublished()
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
    }
}
