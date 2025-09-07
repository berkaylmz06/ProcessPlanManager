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
    public interface ITeminatMektuplariService
    {
        bool TeminatMektubuKaydet(TeminatMektuplari mektup);
        void MektupGuncelle(string eskiMektupNo, TeminatMektuplari guncelMektup);
        void MektupSil(string mektupNo);
        bool MektupNoVarMi(string mektupNo);
        List<TeminatMektuplari> GetTeminatMektuplari();
        DataTable FiltreleTeminatMektuplari(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid);
        string NormalizeColumnName(string columnName);
    }
}
