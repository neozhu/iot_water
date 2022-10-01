namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addcustomerid_null : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WaterMeterRecords", "CustomerId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WaterMeterRecords", "CustomerId", c => c.Int(nullable: false));
        }
    }
}
