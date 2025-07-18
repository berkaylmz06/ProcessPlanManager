using CEKA_APP.Entitys;
using CEKA_APP.Entitys.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_TeminatMektuplariData
    {
        public bool TeminatMektubuKaydet(TeminatMektuplari mektup) 
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();

                    string sql = @"
                        INSERT INTO ProjeFinans_TeminatMektuplari
                        (mektupNo, musteriNo, musteriAdi, paraBirimi, banka, mektupTuru, tutar, vadeTarihi, iadeTarihi, komisyonTutari, komisyonOrani, komisyonVadesi)
                        VALUES
                        (@mektupNo, @musteriNo, @musteriAdi, @paraBirimi, @banka, @mektupTuru, @tutar, @vadeTarihi, @iadeTarihi, @komisyonTutari, @komisyonOrani, @komisyonVadesi)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@mektupNo", mektup.mektupNo ?? "");
                        command.Parameters.AddWithValue("@musteriNo", mektup.musteriNo ?? "");
                        command.Parameters.AddWithValue("@musteriAdi", mektup.musteriAdi ?? "");
                        command.Parameters.AddWithValue("@paraBirimi", mektup.paraBirimi ?? "");
                        command.Parameters.AddWithValue("@banka", mektup.banka ?? "");
                        command.Parameters.AddWithValue("@mektupTuru", mektup.mektupTuru ?? "");
                        command.Parameters.AddWithValue("@tutar", mektup.tutar);
                        command.Parameters.AddWithValue("@vadeTarihi", mektup.vadeTarihi);
                        command.Parameters.AddWithValue("@iadeTarihi", mektup.iadeTarihi);
                        command.Parameters.AddWithValue("@komisyonTutari", mektup.komisyonTutari);
                        command.Parameters.AddWithValue("@komisyonOrani", mektup.komisyonOrani);
                        command.Parameters.AddWithValue("@komisyonVadesi", mektup.komisyonVadesi);

                        command.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kayıt eklenemedi: " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        public List<TeminatMektuplari> GetTeminatMektuplari()
        {
            List<TeminatMektuplari> teminatMektuplari = new List<TeminatMektuplari>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT mektupNo, musteriNo, musteriAdi, paraBirimi, banka, mektupTuru, tutar, vadeTarihi, iadeTarihi, komisyonTutari, komisyonOrani, komisyonVadesi FROM ProjeFinans_TeminatMektuplari";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TeminatMektuplari mektup = new TeminatMektuplari();
                                mektup.mektupNo = reader["mektupNo"] as string;
                                mektup.musteriNo = reader["musteriNo"] as string;
                                mektup.musteriAdi = reader["musteriAdi"] as string;
                                mektup.paraBirimi = reader["paraBirimi"] as string;
                                mektup.banka = reader["banka"] as string;
                                mektup.mektupTuru = reader["mektupTuru"] as string;
                                mektup.tutar = reader.GetDecimal(reader.GetOrdinal("tutar"));
                                mektup.vadeTarihi = reader.GetDateTime(reader.GetOrdinal("vadeTarihi"));
                                mektup.iadeTarihi = reader.GetDateTime(reader.GetOrdinal("iadeTarihi"));
                                mektup.komisyonTutari = reader.GetDecimal(reader.GetOrdinal("komisyonTutari"));
                                mektup.komisyonOrani = reader.GetDecimal(reader.GetOrdinal("komisyonOrani"));
                                mektup.komisyonVadesi = reader.GetInt32(reader.GetOrdinal("komisyonVadesi"));
                                teminatMektuplari.Add(mektup);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Teminat mektupları getirilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
            return teminatMektuplari;
        }
        public bool MektupNoVarMi(string mektupNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM ProjeFinans_TeminatMektuplari WHERE mektupNo = @mektupNo";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@mektupNo", mektupNo);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Mektup numarası kontrol edilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
        public void MektupGuncelle(string eskiMektupNo, TeminatMektuplari guncelMektup)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE ProjeFinans_TeminatMektuplari
                        SET musteriNo = @yeniMusteriNo,
                            musteriAdi = @musteriAdi,
                            paraBirimi = @paraBirimi,
                            banka = @banka,
                            mektupTuru = @mektupTuru,
                            tutar = @tutar,
                            vadeTarihi = @vadeTarihi,
                            iadeTarihi = @iadeTarihi,
                            komisyonTutari = @komisyonTutari,
                            komisyonOrani = @komisyonOrani,
                            komisyonVadesi = @komisyonVadesi
                        WHERE mektupNo = @eskiMektupNo";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@yeniMusteriNo", guncelMektup.musteriNo);
                        cmd.Parameters.AddWithValue("@musteriAdi", guncelMektup.musteriAdi);
                        cmd.Parameters.AddWithValue("@paraBirimi", guncelMektup.paraBirimi);
                        cmd.Parameters.AddWithValue("@banka", guncelMektup.banka);
                        cmd.Parameters.AddWithValue("@mektupTuru", guncelMektup.mektupTuru);
                        cmd.Parameters.AddWithValue("@tutar", guncelMektup.tutar);
                        cmd.Parameters.AddWithValue("@vadeTarihi", guncelMektup.vadeTarihi);
                        cmd.Parameters.AddWithValue("@iadeTarihi", guncelMektup.iadeTarihi);
                        cmd.Parameters.AddWithValue("@komisyonTutari", guncelMektup.komisyonTutari);
                        cmd.Parameters.AddWithValue("@komisyonOrani", guncelMektup.komisyonOrani);
                        cmd.Parameters.AddWithValue("@komisyonVadesi", guncelMektup.komisyonVadesi);
                        cmd.Parameters.AddWithValue("@eskiMusteriNo", eskiMektupNo);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Mektup güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
        public void MektupSil(string mektupNo)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM ProjeFinans_TeminatMektuplari WHERE mektupNo = @mektupNo";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@mektupNo", mektupNo);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Mektup silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
    }
}
