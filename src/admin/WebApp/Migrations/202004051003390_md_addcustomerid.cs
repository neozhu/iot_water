namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addcustomerid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterMeterRecords", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.WaterMeterRecords", "CustomerName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterMeterRecords", "CustomerName");
            DropColumn("dbo.WaterMeterRecords", "CustomerId");
        }
    }
}
