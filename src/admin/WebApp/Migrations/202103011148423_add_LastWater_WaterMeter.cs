namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_LastWater_WaterMeter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterMeters", "LastWater", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.WaterMeters", "LastWaterDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterMeters", "LastWaterDate");
            DropColumn("dbo.WaterMeters", "LastWater");
        }
    }
}
