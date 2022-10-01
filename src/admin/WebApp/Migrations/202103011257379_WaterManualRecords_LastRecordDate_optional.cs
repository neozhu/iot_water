namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WaterManualRecords_LastRecordDate_optional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WaterManualRecords", "LastRecordDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WaterManualRecords", "LastRecordDate", c => c.DateTime(nullable: false));
        }
    }
}
