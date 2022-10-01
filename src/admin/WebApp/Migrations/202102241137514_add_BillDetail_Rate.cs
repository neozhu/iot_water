namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_BillDetail_Rate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillDetails", "Rate", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillDetails", "Rate");
        }
    }
}
