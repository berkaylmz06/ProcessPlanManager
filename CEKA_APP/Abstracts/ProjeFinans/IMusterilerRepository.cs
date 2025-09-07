using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IMusterilerRepository
    {
        void MusteriKaydet(Musteriler musteri, SqlTransaction transaction);
        bool MusteriNoVarMi(string musteriNo, SqlTransaction transaction);
        Musteriler GetMusteriByMusteriNo(string musteriNo);
        List<Musteriler> GetMusteriler();
        void TumMusterileriSil(SqlTransaction transaction);
        DataTable FiltreleMusteriBilgileri(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid);
        string NormalizeColumnName(string columnName);
    }
}
