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
    
    public partial class Car
    {
        public Car()
        {
            this.CarPrices = new HashSet<CarPrice>();
        }
    
        public int CarID { get; set; }
        public string CarName { get; set; }
        public Nullable<int> CarModel { get; set; }
        public string CarMake { get; set; }
    
        public virtual ICollection<CarPrice> CarPrices { get; set; }
    }
}