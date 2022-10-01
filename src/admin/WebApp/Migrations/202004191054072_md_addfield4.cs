namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addfield4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Type", c => c.String(maxLength: 20));
            AddColumn("dbo.Customers", "ManageDept", c => c.String(maxLength: 20));
            AddColumn("dbo.CustomerWaterMeters", "Points", c => c.String());
            AddColumn("dbo.CustomerWaterMeters", "ZoneName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerWaterMeters", "ZoneName");
            DropColumn("dbo.CustomerWaterMeters", "Points");
            DropColumn("dbo.Customers", "ManageDept");
            DropColumn("dbo.Customers", "Type");
        }
    }
}
