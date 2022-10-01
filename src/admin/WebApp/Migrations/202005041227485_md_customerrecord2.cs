namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_customerrecord2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CustomerWaterRecords", "CustomerId");
            AddForeignKey("dbo.CustomerWaterRecords", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            DropColumn("dbo.CustomerWaterRecords", "CustomerName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomerWaterRecords", "CustomerName", c => c.String(maxLength: 50));
            DropForeignKey("dbo.CustomerWaterRecords", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CustomerWaterRecords", new[] { "CustomerId" });
        }
    }
}
