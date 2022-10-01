namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_temperature_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WaterMeters", "temperature", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WaterMeters", "temperature", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
