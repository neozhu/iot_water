namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_BillHeaders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BillId = c.Int(nullable: false),
                        MeterName = c.String(maxLength: 50),
                        LineNo = c.String(maxLength: 50),
                        MeterId = c.String(maxLength: 20),
                        MeterName1 = c.String(maxLength: 50),
                        MeterPoint = c.String(maxLength: 150),
                        Negitive = c.String(maxLength: 10),
                        Ratio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Water = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastWater = c.Decimal(precision: 18, scale: 2),
                        PerCent = c.Decimal(precision: 18, scale: 2),
                        ActualWater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WaterDt1 = c.DateTime(nullable: false),
                        Water1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WaterDt2 = c.DateTime(nullable: false),
                        Water2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                        BillHeader_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BillHeaders", t => t.BillHeader_Id)
                .Index(t => t.TenantId)
                .Index(t => t.BillHeader_Id);
            
            CreateTable(
                "dbo.BillHeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BillNo = c.String(nullable: false, maxLength: 12),
                        CustomerId = c.Int(nullable: false),
                        CustomerName = c.String(maxLength: 50),
                        Category = c.String(maxLength: 50),
                        WaterPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServicePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillDate = c.DateTime(nullable: false),
                        ReceiptDate = c.DateTime(),
                        Month = c.String(maxLength: 12),
                        TotalWater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastTotalWater = c.Decimal(precision: 18, scale: 2),
                        PerCent = c.Decimal(precision: 18, scale: 2),
                        TotalWaterAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalServiceAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdjustWater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdjustWaterAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdjustServiceAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalReceivableAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(nullable: false, maxLength: 20),
                        Remark = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.TenantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BillHeaders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.BillDetails", "BillHeader_Id", "dbo.BillHeaders");
            DropIndex("dbo.BillHeaders", new[] { "TenantId" });
            DropIndex("dbo.BillHeaders", new[] { "CustomerId" });
            DropIndex("dbo.BillDetails", new[] { "BillHeader_Id" });
            DropIndex("dbo.BillDetails", new[] { "TenantId" });
            DropTable("dbo.BillHeaders");
            DropTable("dbo.BillDetails");
        }
    }
}
