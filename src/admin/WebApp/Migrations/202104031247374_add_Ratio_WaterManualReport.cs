namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Ratio_WaterManualReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterManualReports", "YearRatio", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.WaterManualReports", "LastRatio", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterManualReports", "LastRatio");
            DropColumn("dbo.WaterManualReports", "YearRatio");
        }
    }
}
