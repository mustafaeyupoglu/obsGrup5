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
    
    public partial class Fakulte
    {
        public Fakulte()
        {
            this.Bolum = new HashSet<Bolum>();
        }
    
        public int Id { get; set; }
        public string FakulteAdi { get; set; }
        public string FakulteKodu { get; set; }
        public string Adres { get; set; }
        public string Fax { get; set; }
        public string Telefon { get; set; }
    
        public virtual ICollection<Bolum> Bolum { get; set; }
    }
}
