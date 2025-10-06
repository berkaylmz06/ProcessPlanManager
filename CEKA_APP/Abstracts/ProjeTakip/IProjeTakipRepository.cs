using CEKA_APP.Entitys.ProjeTakip;
using System.Data.SqlClient;

namespace CEKA_APP.Abstracts.ProjeTakip
{
    public interface IProjeTakipRepository
    {
        bool ProjeKartiEkle(SqlConnection connection, SqlTransaction transaction, ProjeKarti projeKarti);
        ProjeKarti ProjeKartiAra(SqlConnection connection, int projeKartId);
        bool ProjeKartiSil(SqlConnection connection, SqlTransaction transaction, int projeKartId);
    }
}
