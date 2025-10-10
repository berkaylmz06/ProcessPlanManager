using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Entitys.Muhasebe
{
    public class Personeller
    {
        public int personelId { get; set; }          
        public string adSoyad { get; set; }            
        public string kullaniciAdi { get; set; }      
        public string departman { get; set; }        
        public string telefon { get; set; }            
        public bool aktif { get; set; }             
        public DateTime kayitTarihi { get; set; }      
    }
}
