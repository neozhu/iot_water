namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_HasSend_Billhead : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillHeaders", "HasSend", c => c.Boolean(nullable: false));
            AddColumn("dbo.BillHeaders", "SendDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillHeaders", "SendDateTime");
            DropColumn("dbo.BillHeaders", "HasSend");
        }
    }
}
