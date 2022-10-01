namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_meterType_WaterMeterRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterMeterRecords", "meterType", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterMeterRecords", "meterType");
        }
    }
}
