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
        public bool TablodanKesimTamamlanmisEkleme(SqlConnection connection, SqlTransaction transaction, string kesimYapan, string kesimId, int kesilmisPlanSayisi, DateTime kesimTarihi, TimeSpan kesimSaati, string kesilenLot)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            using (var command = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM [KesimTamamlanmisPaket] WHERE [kesimId] = @kesimId)
BEGIN
    UPDATE [KesimTamamlanmisPaket]
    SET 
        [kesilmisPlanSayisi] = [kesilmisPlanSayisi] + @kesilmisPlanSayisi,
        [kesimYapan] = @kesimYapan,
        [kesimTarihi] = @kesimTarihi,
        [kesimSaati] = @kesimSaati,
        [kesilenLot] = @kesilenLot
    WHERE [kesimId] = @kesimId
END
ELSE
BEGIN
    INSERT INTO [KesimTamamlanmisPaket] 
        ([kesimYapan], [kesimId], [kesilmisPlanSayisi], [kesimTarihi], [kesimSaati], [kesilenLot])
    VALUES 
        (@kesimYapan, @kesimId, @kesilmisPlanSayisi, @kesimTarihi, @kesimSaati, @kesilenLot)
END", connection, transaction))
            {
                command.Parameters.AddWithValue("@kesimYapan", kesimYapan ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@kesimId", kesimId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@kesilmisPlanSayisi", kesilmisPlanSayisi);
                command.Parameters.AddWithValue("@kesimTarihi", kesimTarihi);
                command.Parameters.AddWithValue("@kesimSaati", kesimSaati);
                command.Parameters.AddWithValue("@kesilenLot", kesilenLot ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }

            return true;
        }

        public string GetKesimListesTamamlanmisQuery()
        {
            return @"SELECT kesimYapan, kesimId, kesilmisPlanSayisi, kesilenLot, kesimTarihi, kesimSaati FROM KesimTamamlanmisPaket";
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
