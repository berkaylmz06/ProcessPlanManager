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
    public interface ITeminatMektuplariRepository
    {
        bool TeminatMektubuKaydet(TeminatMektuplari mektup, SqlTransaction transaction);
        void MektupGuncelle(string eskiMektupNo, TeminatMektuplari guncelMektup, SqlTransaction transaction);
        void MektupSil(string mektupNo, SqlTransaction transaction);
        bool MektupNoVarMi(string mektupNo);
        List<TeminatMektuplari> GetTeminatMektuplari();
        DataTable FiltreleTeminatMektuplari(Dictionary<string, TextBox> filtreKutulari, DataGridView dataGrid);
        string NormalizeColumnName(string columnName);
    }
}
