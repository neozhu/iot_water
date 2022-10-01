namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_chdt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomerWaterQuotas", "RecordDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerWaterQuotas", "RecordDate", c => c.DateTime(nullable: false));
        }
    }
}
