using CEKA_APP.Entitys.Muhasebe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Abstracts.Muhasebe
{
    public interface IPersonellerRepository
    {
        void PersonelEkle(SqlConnection connection, SqlTransaction transaction, string adSoyad, string kullaniciAdi, string departman, string telefon, bool aktif);
        DataTable GetAllPersonel(SqlConnection connection, SqlTransaction transaction);
        void UpdatePersonel(SqlConnection connection, SqlTransaction transaction, int personelId, string adSoyad, string kullaniciAdi, string departman, string telefon, bool aktif);
        void PersonelSil(SqlConnection connection, SqlTransaction transaction, int personelId);
        List<Personeller> GetPersonelOperator(SqlConnection connection, SqlTransaction transaction);
    }
}
