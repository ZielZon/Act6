﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarDBEntities : DbContext
    {
        public CarDBEntities()
            : base("name=CarDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarPrice> CarPrices { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<OnlineSale> OnlineSales { get; set; }
        public DbSet<OnsiteSale> OnsiteSales { get; set; }
        public DbSet<Province> Provinces { get; set; }
    }
}