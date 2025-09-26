using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Interfaces.Genel;
using System;
using System.Data.SqlClient;

namespace CEKA_APP.Services.Genel
{
    public class DataBaseService : IDataBaseService
    {
        private readonly IDataBaseRepository _dataBaseRepository;

        public DataBaseService(IDataBaseRepository dataBaseRepository)
        {
            _dataBaseRepository = dataBaseRepository ?? throw new ArgumentNullException(nameof(dataBaseRepository));
        }
        public SqlConnection GetConnection()
        {
            try
            {
                return _dataBaseRepository.GetConnection();

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Beklenmeyen bir hata oluştu.", ex);
            }
        }
    }
}
