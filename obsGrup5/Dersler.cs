//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace obsGrup5
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dersler
    {
        public string DersKodu { get; set; }
        public string DersAdi { get; set; }
        public string Tur { get; set; }
        public Nullable<int> Kredi { get; set; }
        public Nullable<int> AKTS { get; set; }
        public Nullable<int> TeoriDersSaati { get; set; }
        public Nullable<int> UygulamaDersSaati { get; set; }
        public string Donem { get; set; }
    
        public virtual Donemler Donemler { get; set; }
    }
}