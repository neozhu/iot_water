namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_CustomerCode_CustomerWaterMeter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerWaterMeters", "CustomerCode", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerWaterMeters", "CustomerCode");
        }
    }
}
