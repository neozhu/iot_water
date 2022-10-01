namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_Precision_3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BillHeaders", "WaterPrice", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.BillHeaders", "ServicePrice", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.Customers", "WaterPrice", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AlterColumn("dbo.Customers", "ServicePrice", c => c.Decimal(nullable: false, precision: 18, scale: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "ServicePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Customers", "WaterPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BillHeaders", "ServicePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BillHeaders", "WaterPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
