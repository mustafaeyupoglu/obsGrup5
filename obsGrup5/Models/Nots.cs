using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace obsGrup5.Models
{
    public class Nots
    {
        public int? AkademisyenId { get; set; } 
        public int? OgrenciNo { get; set; }
        public string DersAdi { get; set; }
        public string DersKodu { get; set; }
        public string YariYil { get; set; }
        public int? YilDers { get; set; }
        public int? Vize { get; set; }
        public int? Final { get; set; }
        public int? But { get; set; }
        public double? YilNot { get; set; }
        public string HarfNotu { get; set; }

        public Nots()
        {
            AkademisyenId = 0;
            OgrenciNo = 0;
            DersAdi = "";
             DersKodu ="";
               YariYil ="";
          YilDers =0;
          Vize =0;
         Final =0;
         But =0;
         YilNot =0;
         HarfNotu = "";
        }
        
    }
}