//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API_Act6.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OnlineSale
    {
        public int CityID { get; set; }
        public string link { get; set; }
    
        public virtual City City { get; set; }
    }
}
