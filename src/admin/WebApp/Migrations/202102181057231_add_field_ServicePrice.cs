namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_field_ServicePrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "WaterPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Customers", "ServicePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "ServicePrice");
            DropColumn("dbo.Customers", "WaterPrice");
        }
    }
}
