namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_WaterManualReports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WaterManualReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        meterId = c.String(maxLength: 20),
                        Name = c.String(maxLength: 50),
                        LineNo = c.String(maxLength: 50),
                        Name1 = c.String(maxLength: 50),
                        Water = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecordDate = c.DateTime(nullable: false),
                        LastWater = c.Decimal(precision: 18, scale: 2),
                        LastRecordDate = c.DateTime(),
                        CalWater = c.Decimal(precision: 18, scale: 2),
                        Remark = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.TenantId);
            
            DropColumn("dbo.WaterManualRecords", "LastWater");
            DropColumn("dbo.WaterManualRecords", "LastRecordDate");
            DropColumn("dbo.WaterManualRecords", "CalWater");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WaterManualRecords", "CalWater", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.WaterManualRecords", "LastRecordDate", c => c.DateTime());
            AddColumn("dbo.WaterManualRecords", "LastWater", c => c.Decimal(precision: 18, scale: 2));
            DropIndex("dbo.WaterManualReports", new[] { "TenantId" });
            DropTable("dbo.WaterManualReports");
        }
    }
}
