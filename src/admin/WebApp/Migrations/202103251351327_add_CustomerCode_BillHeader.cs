namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_CustomerCode_BillHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillHeaders", "CustomerCode", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillHeaders", "CustomerCode");
        }
    }
}
