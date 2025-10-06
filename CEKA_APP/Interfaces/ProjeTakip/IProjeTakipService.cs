using CEKA_APP.Entitys.ProjeTakip;

namespace CEKA_APP.Interfaces.ProjeTakip
{
    public interface IProjeTakipService
    {
        bool ProjeKartiEkle(ProjeKarti projeKarti);
        ProjeKarti ProjeKartiAra(int projeKartId);
        bool ProjeKartiSil(int projeKartId);
    }
}
