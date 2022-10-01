namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_longitude_to_WaterMeter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterMeters", "longitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.WaterMeters", "latitude", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterMeters", "latitude");
            DropColumn("dbo.WaterMeters", "longitude");
        }
    }
}
