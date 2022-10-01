namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_meter_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerWaterMeters", "Ratio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.WaterMeters", "Name", c => c.String(maxLength: 50));
            AddColumn("dbo.WaterMeters", "LineNo", c => c.String(maxLength: 50));
            AddColumn("dbo.WaterMeters", "Name1", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterMeters", "Name1");
            DropColumn("dbo.WaterMeters", "LineNo");
            DropColumn("dbo.WaterMeters", "Name");
            DropColumn("dbo.CustomerWaterMeters", "Ratio");
        }
    }
}
