namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_longitude_18_6_to_WaterMeter : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WaterMeters", "longitude", c => c.Decimal(precision: 18, scale: 6));
            AlterColumn("dbo.WaterMeters", "latitude", c => c.Decimal(precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WaterMeters", "latitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.WaterMeters", "longitude", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
