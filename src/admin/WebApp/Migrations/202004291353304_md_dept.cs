namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_dept : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerWaterMeters", "Dept", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerWaterMeters", "Dept");
        }
    }
}
