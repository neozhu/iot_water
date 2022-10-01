namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addopeningdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterMeters", "OpeningDate", c => c.DateTime());
            AddColumn("dbo.WaterMeters", "ClosedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterMeters", "ClosedDate");
            DropColumn("dbo.WaterMeters", "OpeningDate");
        }
    }
}
