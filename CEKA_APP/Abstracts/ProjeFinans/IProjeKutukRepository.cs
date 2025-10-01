using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.ProjeFinans
{
    public interface IProjeKutukRepository
    {
        bool ProjeKutukEkle(SqlConnection connection, SqlTransaction transaction, ProjeKutuk kutuk);
        bool ProjeFiyatlandirmaEkle(SqlConnection connection, SqlTransaction transaction, string projeNo, decimal fiyat);
        bool ProjeEkleProjeFinans(SqlConnection connection, SqlTransaction transaction, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi);
        bool UpdateProjeFinans(SqlConnection connection, SqlTransaction transaction, string projeNo, string aciklama, string projeAdi, DateTime olusturmaTarihi);
        ProjeBilgi GetProjeBilgileri(SqlConnection connection, string projeNo);
        ProjeKutuk ProjeKutukAra(SqlConnection connection, int projeId);
        void UpdateToplamBedel(SqlConnection connection, SqlTransaction transaction, string projeNo, decimal toplamBedel);
        bool IsFaturalamaSekliTekil(SqlConnection connection, int projeId);
        (bool HasRelated, List<string> Details) HasRelatedRecords(SqlConnection connection, int projeId, List<int> altProjeler);
        bool ProjeKutukGuncelle(SqlConnection connection, SqlTransaction transaction, ProjeKutuk yeniKutuk);
        bool ProjeKutukSil(SqlConnection connection, SqlTransaction transaction, int projeId, List<int> altProjeIds);
        string GetProjeParaBirimi(SqlConnection connection, int projeId);
        ProjeKutuk GetProjeKutukStatus(SqlConnection connection, int projeId);
        bool UpdateProjeKutukDurum(SqlConnection connection, SqlTransaction transaction, int projeId, bool? montajTamamlandiMi);
        bool ProjeNoKontrol(SqlConnection connection, string projeNo);
    }
}
