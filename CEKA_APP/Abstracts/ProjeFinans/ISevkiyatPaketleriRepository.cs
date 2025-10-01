using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface ISevkiyatPaketleriRepository
    {
        List<(int Id, string Adi, DateTime Tarih)> GetPaketler(SqlConnection connection);
        int PaketEkle(SqlConnection connection, SqlTransaction transaction, string paketAdi);
        int GetPaketIdByAdi(SqlConnection connection, string paketAdi);
    }
}
