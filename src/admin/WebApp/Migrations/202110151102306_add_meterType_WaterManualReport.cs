namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_meterType_WaterManualReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterManualReports", "meterType", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterManualReports", "meterType");
        }
    }
}
