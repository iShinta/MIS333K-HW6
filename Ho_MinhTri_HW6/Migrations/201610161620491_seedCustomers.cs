namespace Ho_MinhTri_HW6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedCustomers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        Email = c.String(),
                        AverageSale = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReferredFrom = c.String(),
                        Frequency_FrequencyID = c.Int(),
                    })
                .PrimaryKey(t => t.CustomerID)
                .ForeignKey("dbo.Frequencies", t => t.Frequency_FrequencyID)
                .Index(t => t.Frequency_FrequencyID);
            
            CreateTable(
                "dbo.Frequencies",
                c => new
                    {
                        FrequencyID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.FrequencyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "Frequency_FrequencyID", "dbo.Frequencies");
            DropIndex("dbo.Customers", new[] { "Frequency_FrequencyID" });
            DropTable("dbo.Frequencies");
            DropTable("dbo.Customers");
        }
    }
}
