using CEKA_APP.Entitys.ProjeFinans;
using System.Collections.Generic;
using System.Data;
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
        string GetTeminatMektuplariQuery();
    }
}
