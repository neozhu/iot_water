namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_TotalChineseAmount_BillHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillHeaders", "TotalChineseAmount", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillHeaders", "TotalChineseAmount");
        }
    }
}
