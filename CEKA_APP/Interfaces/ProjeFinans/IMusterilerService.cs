using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Interfaces.ProjeFinans
{
    public interface IMusterilerService
    {
        void MusteriKaydet(Musteriler musteri);
        bool MusteriNoVarMi(string musteriNo);
        Musteriler GetMusteriByMusteriNo(string musteriNo);
        List<Musteriler> GetMusteriler();
        void TumMusterileriSil();
        DataTable FiltreleMusteriBilgileri(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid);
        string NormalizeColumnName(string columnName);
    }
}
