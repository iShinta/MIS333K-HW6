namespace Ho_MinhTri_HW6.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Ho_MinhTri_HW6.DAL.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Ho_MinhTri_HW6.DAL.AppDbContext context)
       {
            //call method to add frequencies
            AddCustomers.AddFrequencies();

            //call method to add customers
            AddCustomers.SeedCustomers();
       }

    }
}
