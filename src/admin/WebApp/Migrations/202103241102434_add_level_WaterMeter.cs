namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_level_WaterMeter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterMeters", "Level", c => c.String(maxLength: 10));
            AddColumn("dbo.WaterMeters", "ZoneName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterMeters", "ZoneName");
            DropColumn("dbo.WaterMeters", "Level");
        }
    }
}
