using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IOdemeSartlariRepository
    {
        void SaveOrUpdateOdemeBilgi(OdemeSartlari odemeSartlari, SqlTransaction transaction);
        string GetFaturaNo(int projeId, int kilometreTasiId);
        List<OdemeSartlari> GetOdemeBilgileri();
        List<OdemeSartlari> GetOdemeBilgileriByProjeId(int projeId);
        OdemeSartlari GetOdemeBilgi(string projeNo, int kilometreTasiId);
        OdemeSartlari GetOdemeBilgiByOdemeId(int odemeId);
        void DeleteOdemeBilgi(int projeId, int kilometreTasiId, SqlTransaction transaction);
        bool UpdateFaturaNo(int odemeId, string faturaNo, SqlTransaction transaction);
        bool OdemeSartlariSil(int projeId, SqlTransaction transaction);
        DataTable FiltreleOdemeBilgileri(Dictionary<string, TextBox> filtreKriterleri, DataGridView dataGrid);
        DataTable ToDataTableWithOdemeSapmasi(List<OdemeSartlari> data);
        string NormalizeColumnName(string columnName);
    }
}
