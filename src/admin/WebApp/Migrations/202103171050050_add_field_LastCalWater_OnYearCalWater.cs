namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_field_LastCalWater_OnYearCalWater : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterManualReports", "LastCalWater", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.WaterManualReports", "OnYearCalWater", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterManualReports", "OnYearCalWater");
            DropColumn("dbo.WaterManualReports", "LastCalWater");
        }
    }
}
