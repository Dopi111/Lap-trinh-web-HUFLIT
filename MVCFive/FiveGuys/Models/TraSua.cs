//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FiveGuys.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TraSua
    {
        public int Id { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public string Loai { get; set; }
        public DateTime NgayThem { get; set; }
        public Nullable<decimal> GiaTien { get; set; }
        public string HinhAnhText { get; set; }
        public string GiaTienFormatted { get; set; }
    }
}
