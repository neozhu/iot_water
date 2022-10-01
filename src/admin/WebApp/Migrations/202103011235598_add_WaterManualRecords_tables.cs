namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_WaterManualRecords_tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WaterManualRecords",
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
                        LastRecordDate = c.DateTime(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.WaterManualRecords", new[] { "TenantId" });
            DropTable("dbo.WaterManualRecords");
        }
    }
}
