using CEKA_APP.UsrControl.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.UsrControl.Interfaces
{
    public interface IUserControlFactory
    {
        ctlOdemeSartlari CreateOdemeSartlariControl();
        ctlOdemeSartlariListe CreateOdemeSartlariListeControl();

        ctlProjeKutuk CreateProjeKutukControl();
        ctlProjeFiyatlandirma CreateProjeFiyatlandirmaControl();
        ctlSevkiyat CreateSevkiyatControl();
        ctlMusteriler CreateMusterilerControl();
        ctlTeminatMektuplari CreateTeminatMektuplariControl();
        ctlProjeBilgileri CreateProjeBilgileriControl();
    }
}
