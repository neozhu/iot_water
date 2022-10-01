namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_customerMeter_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerWaterMeters", "meterName", c => c.String(maxLength: 50));
            AddColumn("dbo.CustomerWaterMeters", "Negitive", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerWaterMeters", "Negitive");
            DropColumn("dbo.CustomerWaterMeters", "meterName");
        }
    }
}
