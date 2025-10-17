using CEKA_APP.Abstracts.KesimTakip;
using CEKA_APP.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.Concretes.KesimTakip
{
    public class KesimTamamlanmisRepository : IKesimTamamlanmisRepository
    {
        public int TablodanKesimTamamlanmisEkleme(SqlConnection connection, SqlTransaction transaction, string kesimYapan, string kesimId, int kesilmisPlanSayisi, string kesilenLot, int kullanilanMalzemeEn, int kullanilanMalzemeBoy)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            int existingId = 0;

            using (var checkCommand = new SqlCommand("SELECT [id] FROM [KesimTamamlanmisPaket] WHERE [kesimId] = @kesimId", connection, transaction))
            {
                checkCommand.Parameters.AddWithValue("@kesimId", kesimId ?? (object)DBNull.Value);
                var result = checkCommand.ExecuteScalar();
                if (result != null)
                {
                    existingId = (int)result;
                }
            }

            if (existingId > 0)
            {
                using (var updateCommand = new SqlCommand(@"
            UPDATE [KesimTamamlanmisPaket]
            SET 
                [kesilmisPlanSayisi] = [kesilmisPlanSayisi] + @kesilmisPlanSayisi,
                [kesimYapan] = @kesimYapan,
                [kesilenLot] = @kesilenLot,
                [kullanilanMalzemeEn] = @kullanilanMalzemeEn,
                [kullanilanMalzemeBoy] = @kullanilanMalzemeBoy
            WHERE [id] = @id;
            SELECT @id;", connection, transaction))
                {
                    updateCommand.Parameters.AddWithValue("@id", existingId);
                    updateCommand.Parameters.AddWithValue("@kesilmisPlanSayisi", kesilmisPlanSayisi);
                    updateCommand.Parameters.AddWithValue("@kesimYapan", kesimYapan ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@kesilenLot", kesilenLot ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@kullanilanMalzemeEn", kullanilanMalzemeEn);
                    updateCommand.Parameters.AddWithValue("@kullanilanMalzemeBoy", kullanilanMalzemeBoy);

                    updateCommand.ExecuteNonQuery();
                    return existingId;
                }
            }
            else
            {
                using (var insertCommand = new SqlCommand(@"
            INSERT INTO [KesimTamamlanmisPaket] 
                ([kesimYapan], [kesimId], [kesilmisPlanSayisi], [kesilenLot], [kullanilanMalzemeEn], [kullanilanMalzemeBoy])
            VALUES 
                (@kesimYapan, @kesimId, @kesilmisPlanSayisi, @kesilenLot, @kullanilanMalzemeEn, @kullanilanMalzemeBoy);
            SELECT SCOPE_IDENTITY();", connection, transaction))
                {
                    insertCommand.Parameters.AddWithValue("@kesimYapan", kesimYapan ?? (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@kesimId", kesimId ?? (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@kesilmisPlanSayisi", kesilmisPlanSayisi);
                    insertCommand.Parameters.AddWithValue("@kesilenLot", kesilenLot ?? (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@kullanilanMalzemeEn", kullanilanMalzemeEn);
                    insertCommand.Parameters.AddWithValue("@kullanilanMalzemeBoy", kullanilanMalzemeBoy);

                    object newId = insertCommand.ExecuteScalar();
                    return newId != null ? Convert.ToInt32(newId) : 0;
                }
            }
        }
        public bool YanUrunDetayEkleme(SqlConnection connection, SqlTransaction transaction, int kesimTamamlanmisId, int yanUrunEn, int yanUrunBoy, int adet)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            using (var command = new SqlCommand(@"
                INSERT INTO [KesimYanUrun] 
                    ([kesimTamamlanmisId], [yanUrunEn], [yanUrunBoy], [adet])
                VALUES 
                    (@kesimTamamlanmisId, @yanUrunEn, @yanUrunBoy, @adet)", connection, transaction))
            {
                command.Parameters.AddWithValue("@kesimTamamlanmisId", kesimTamamlanmisId);
                command.Parameters.AddWithValue("@yanUrunEn", yanUrunEn);
                command.Parameters.AddWithValue("@yanUrunBoy", yanUrunBoy);
                command.Parameters.AddWithValue("@adet", adet);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public string GetKesimListesTamamlanmisQuery()
        {
            return @"SELECT kesimYapan, kesimId, kesilmisPlanSayisi, kesilenLot, kullanilanMalzemeEn, kullanilanMalzemeBoy, yanUrunVar FROM KesimTamamlanmisPaket";
        }
        public DataTable GetKesimListesTamamlanmis(SqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            using (var adapter = new SqlDataAdapter(GetKesimListesTamamlanmisQuery(), connection))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}