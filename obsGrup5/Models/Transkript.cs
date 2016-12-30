using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace obsGrup5.Models
{
    public class Transkript
    {
        public int? AkademisyenId { get; set; } 
        public int? OgrenciNo { get; set; }
        public string DersAdi { get; set; }
        public string DersKodu { get; set; }
        public string YariYil { get; set; }
        public int? YilDers { get; set; }
        public int? Kredi { get; set; }
        public int? AKTS { get; set; }
        public double? YilNot { get; set; }
        public string HarfNotu { get; set; }

        public Transkript()
        {
            AkademisyenId = 0;
            OgrenciNo = 0;
            DersAdi = "";
             DersKodu ="";
               YariYil ="";
          YilDers =0;
          Kredi =0;
         AKTS =0;
         YilNot =0;
         HarfNotu = "";
        }
    }
}