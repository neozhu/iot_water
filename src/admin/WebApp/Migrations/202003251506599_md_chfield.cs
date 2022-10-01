namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_chfield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomerWaterRecords", "previousDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerWaterRecords", "previousDate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
