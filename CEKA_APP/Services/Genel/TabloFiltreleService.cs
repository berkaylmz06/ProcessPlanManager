using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Interfaces.Genel;
using System;
using System.Collections.Generic;
using System.Data;

namespace CEKA_APP.Services.Genel
{
    public class TabloFiltreleService : ITabloFiltreleService
    {
        private readonly ITabloFiltreleRepository _tabloFiltreleRepository;
        private readonly IDataBaseService _dataBaseService;
        public TabloFiltreleService(ITabloFiltreleRepository tabloFiltreleRepository, IDataBaseService dataBaseService)
        {
            _tabloFiltreleRepository = tabloFiltreleRepository ?? throw new ArgumentNullException(nameof(tabloFiltreleRepository));
            _dataBaseService = dataBaseService ?? throw new ArgumentNullException(nameof(dataBaseService));
        }

        public DataTable GetFilteredData(string baseSql, Dictionary<string, object> filters)
        {
            try
            {
                using (var connection = _dataBaseService.GetConnection())
                {
                    connection.Open();
                    return _tabloFiltreleRepository.GetFilteredData(connection, baseSql, filters);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Veriler filtrelenirken hata oluştu.", ex);
            }
        }
    }
}
