namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Month_WaterManualReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterManualReports", "Month", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterManualReports", "Month");
        }
    }
}
