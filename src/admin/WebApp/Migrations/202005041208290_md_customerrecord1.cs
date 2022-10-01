namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_customerrecord1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerWaterRecords", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CustomerWaterRecords", new[] { "CustomerId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.CustomerWaterRecords", "CustomerId");
            AddForeignKey("dbo.CustomerWaterRecords", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
    }
}
