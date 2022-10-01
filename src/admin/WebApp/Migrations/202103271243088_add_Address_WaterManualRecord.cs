namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Address_WaterManualRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterManualRecords", "Address", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterManualRecords", "Address");
        }
    }
}
