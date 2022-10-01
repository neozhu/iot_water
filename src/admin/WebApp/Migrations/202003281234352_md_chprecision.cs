namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_chprecision : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ZoneWaterMeters", "longitude", c => c.Decimal(precision: 18, scale: 6));
            AlterColumn("dbo.ZoneWaterMeters", "latitude", c => c.Decimal(precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ZoneWaterMeters", "latitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ZoneWaterMeters", "longitude", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
