using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Concretes.ProjeFinans;
using CEKA_APP.DataBase;
using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Services.ProjeFinans
{
    public class ProjeIliskiService : IProjeIliskiService
    {
        private readonly IProjeIliskiRepository _projeIliskiRepository;

        public ProjeIliskiService(IProjeIliskiRepository projeIliskiRepository)
        {
            _projeIliskiRepository = projeIliskiRepository ?? throw new ArgumentNullException(nameof(projeIliskiRepository));
        }
        public bool AltProjeEkle(int ustProjeId, int altProjeId)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        bool sonuc = _projeIliskiRepository.AltProjeEkle(ustProjeId, altProjeId, transaction);
                        transaction.Commit();
                        return sonuc;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool CheckAltProje(int projeId)
        {
            try
            {
                return _projeIliskiRepository.CheckAltProje(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public List<int> GetAltProjeler(int projeId)
        {
            try
            {
                return _projeIliskiRepository.GetAltProjeler(projeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }

        public int? GetUstProjeId(int altProjeId)
        {
            try
            {
                return _projeIliskiRepository.GetUstProjeId(altProjeId);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Proje ID alınırken hata oluştu.", ex);
            }
        }
    }
}
