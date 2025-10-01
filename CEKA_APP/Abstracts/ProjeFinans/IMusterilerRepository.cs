using CEKA_APP.Entitys.ProjeFinans;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IMusterilerRepository
    {
        void MusteriKaydet(SqlConnection connection, SqlTransaction transaction, Musteriler musteri);
        bool MusteriNoVarMi(SqlConnection connection, string musteriNo);
        Musteriler GetMusteriByMusteriNo(SqlConnection connection, string musteriNo);
        List<Musteriler> GetMusteriler(SqlConnection connection);
        void TumMusterileriSil(SqlConnection connection, SqlTransaction transaction);
        string GetMusterilerQuery();
        List<Musteriler> GetMusterilerAraFormu(SqlConnection connection);
        string GetMusterilerAraFormuQuery();
    }
}
