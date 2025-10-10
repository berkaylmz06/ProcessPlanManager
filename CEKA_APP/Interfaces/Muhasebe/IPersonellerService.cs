using CEKA_APP.Entitys.Muhasebe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Interfaces.Muhasebe
{
    public interface IPersonellerService
    {
        void PersonelEkle(string adSoyad, string kullaniciAdi, string departman, string telefon, bool aktif);
        DataTable GetAllPersonel();
        void UpdatePersonel(int personelId, string adSoyad, string kullaniciAdi, string departman, string telefon, bool aktif);
        void PersonelSil(int personelId);
        List<Personeller> GetPersonelOperator();
    }
}
