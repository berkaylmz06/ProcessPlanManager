using CEKA_APP.Abstracts.ProjeTakip;
using CEKA_APP.Entitys.ProjeTakip;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CEKA_APP.Concretes.ProjeTakip
{
    public class UrunGruplariRepository : IUrunGruplariRepository
    {
        public bool UrunGrubuEkle(SqlConnection connection, SqlTransaction transaction, string urunGrubu, string urunGrubuAdi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            using (SqlCommand komut = new SqlCommand(@"INSERT INTO ProjeTakip_UrunGruplari (urunGrubu, urunGrubuAdi, eklemeTarihi) Values (@urunGrubu, @urunGrubuAdi, GETDATE())", connection, transaction))
            {
                komut.Parameters.AddWithValue("@urunGrubu", urunGrubu);
                komut.Parameters.AddWithValue("@urunGrubuAdi", urunGrubuAdi);
                komut.ExecuteNonQuery();

                return true;
            }
        }
        public bool UrunGrubuGuncelle(SqlConnection connection, SqlTransaction transaction, int urunGrubuId, string urunGrubu, string urunGrubuAdi, out bool degisiklikVar)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            List<UrunGruplari> mevcutListe = GetUrunGrubuBilgileri(connection, transaction);

            UrunGruplari mevcutBilgi = mevcutListe.FirstOrDefault(u => u.urunGrubuId == urunGrubuId);
            if (mevcutBilgi == null)
            {
                degisiklikVar = false;
                return false;
            }

            degisiklikVar =
                (mevcutBilgi.urunGrubu ?? "") != (urunGrubu ?? "") ||
                (mevcutBilgi.urunGrubuAdi ?? "") != (urunGrubuAdi ?? "");

            if (!degisiklikVar)
                return true;

            string sorgu = @"UPDATE ProjeTakip_UrunGruplari SET urunGrubu = @urunGrubu, urunGrubuAdi = @urunGrubuAdi WHERE urunGrubuId = @urunGrubuId";

            using (SqlCommand komut = new SqlCommand(sorgu, connection, transaction))
            {
                komut.Parameters.AddWithValue("@urunGrubuId", urunGrubuId);
                komut.Parameters.AddWithValue("@urunGrubu", urunGrubu);
                komut.Parameters.AddWithValue("@urunGrubuAdi", urunGrubuAdi);
                komut.ExecuteNonQuery();
                return true;
            }
        }

        public bool UrunGrubuSil(SqlConnection connection, SqlTransaction transaction, int urunGrubuId)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            using (SqlCommand komut = new SqlCommand("Delete From ProjeTakip_UrunGruplari Where urunGrubuId = @urunGrubuId", connection, transaction))
            {
                komut.Parameters.AddWithValue("@urunGrubuId", urunGrubuId);
                int etkilenenSatirSayisi = komut.ExecuteNonQuery();
                return etkilenenSatirSayisi > 0;
            }
        }
        public string GetUrunGrubuBilgileriQuery()
        {
            return "Select urunGrubuId, urunGrubu, urunGrubuAdi From ProjeTakip_UrunGruplari";
        }

        public List<UrunGruplari> GetUrunGrubuBilgileri(SqlConnection connection, SqlTransaction transaction)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var liste = new List<UrunGruplari>();

            using (SqlCommand komut = new SqlCommand(GetUrunGrubuBilgileriQuery(), connection, transaction))
            using (SqlDataReader reader = komut.ExecuteReader())
            {
                while (reader.Read())
                {
                    liste.Add(new UrunGruplari
                    {
                        urunGrubuId = reader.GetInt32(0),
                        urunGrubu = reader.IsDBNull(1) ? null : reader.GetString(1),
                        urunGrubuAdi = reader.IsDBNull(2) ? null : reader.GetString(2),
                    });
                }
            }

            return liste;
        }
    }
}


