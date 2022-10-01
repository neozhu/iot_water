namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chagne_optional_dt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BillDetails", "WaterDt1", c => c.DateTime());
            AlterColumn("dbo.BillDetails", "Water1", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.BillDetails", "WaterDt2", c => c.DateTime());
            AlterColumn("dbo.BillDetails", "Water2", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BillDetails", "Water2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BillDetails", "WaterDt2", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BillDetails", "Water1", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BillDetails", "WaterDt1", c => c.DateTime(nullable: false));
        }
    }
}
