namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_customerrecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerWaterRecords", "CustomerName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerWaterRecords", "CustomerName");
        }
    }
}
