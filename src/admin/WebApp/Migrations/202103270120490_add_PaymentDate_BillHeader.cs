namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_PaymentDate_BillHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillHeaders", "PaymentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillHeaders", "PaymentDate");
        }
    }
}
