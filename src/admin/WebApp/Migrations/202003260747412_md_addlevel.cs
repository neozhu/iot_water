namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addlevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerWaterMeters", "Level", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerWaterMeters", "Level");
        }
    }
}
