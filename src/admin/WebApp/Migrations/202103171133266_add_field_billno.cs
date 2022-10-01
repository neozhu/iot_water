namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_field_billno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterManualReports", "BillNo", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterManualReports", "BillNo");
        }
    }
}
