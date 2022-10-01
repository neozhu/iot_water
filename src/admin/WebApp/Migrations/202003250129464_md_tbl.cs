namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlarmLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceId = c.String(nullable: false, maxLength: 50),
                        Status = c.String(maxLength: 30),
                        Type = c.String(nullable: false, maxLength: 30),
                        Level = c.String(nullable: false, maxLength: 30),
                        Content = c.String(maxLength: 300),
                        InitDateTime = c.DateTime(nullable: false),
                        User = c.String(maxLength: 20),
                        ToUser = c.String(maxLength: 20),
                        BeginDateTime = c.DateTime(),
                        CompletedDateTime = c.DateTime(),
                        Result = c.String(maxLength: 300),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Level = c.String(),
                        Dept = c.String(maxLength: 20),
                        Contact = c.String(maxLength: 50),
                        ContactInfo = c.String(maxLength: 50),
                        MobilePhone = c.String(maxLength: 50),
                        Quota = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Threshold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsFee = c.Boolean(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegisterDate = c.DateTime(nullable: false),
                        Remark = c.String(maxLength: 150),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.CustomerWaterRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        meterId = c.String(maxLength: 20),
                        previousDate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        previousWater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        lastWater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        water = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecordDate = c.DateTime(nullable: false),
                        User = c.String(maxLength: 20),
                        Type = c.String(maxLength: 10),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.CustomerWaterMeters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        meterId = c.String(maxLength: 20),
                        Quota = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsFee = c.Boolean(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(maxLength: 150),
                        CustomerName = c.String(maxLength: 50),
                        IsDelete = c.Boolean(nullable: false),
                        RegisterDate = c.DateTime(nullable: false),
                        DeleteDate = c.DateTime(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.CustomerWaterQuotas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Quota = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Water = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForecastWater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecordDate = c.DateTime(nullable: false),
                        CustomerName = c.String(maxLength: 50),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.WaterMeterRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        meterStatus = c.String(maxLength: 10),
                        datetime = c.DateTime(nullable: false),
                        meterId = c.String(maxLength: 20),
                        water = c.Decimal(nullable: false, precision: 18, scale: 2),
                        reverseWater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        temperature = c.Decimal(nullable: false, precision: 18, scale: 2),
                        flowrate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pressure = c.Decimal(nullable: false, precision: 18, scale: 2),
                        voltage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valveStatus = c.String(maxLength: 20),
                        userId = c.String(maxLength: 20),
                        imei = c.String(maxLength: 50),
                        OrgName = c.String(maxLength: 80),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WaterMeters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        meterId = c.String(maxLength: 20),
                        Status = c.String(maxLength: 10),
                        valveStatus = c.String(maxLength: 20),
                        address = c.String(maxLength: 200),
                        meterSize = c.String(maxLength: 50),
                        meterType = c.String(maxLength: 20),
                        vender = c.String(maxLength: 50),
                        repairCycle = c.Int(nullable: false),
                        precision = c.Decimal(nullable: false, precision: 18, scale: 2),
                        produced = c.DateTime(),
                        period = c.DateTime(),
                        sealNumber1 = c.String(maxLength: 30),
                        sealNumber2 = c.String(maxLength: 30),
                        sealNumber3 = c.String(maxLength: 30),
                        imei = c.String(maxLength: 50),
                        water = c.Decimal(nullable: false, precision: 18, scale: 2),
                        reverseWater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        temperature = c.Decimal(nullable: false, precision: 18, scale: 2),
                        flowrate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        pressure = c.Decimal(nullable: false, precision: 18, scale: 2),
                        voltage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        threshold1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        threshold2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        threshold3 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        threshold4 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        userCode = c.String(maxLength: 20),
                        userName = c.String(maxLength: 20),
                        Remark = c.String(maxLength: 280),
                        CustomerId = c.Int(nullable: false),
                        CustomerName = c.String(maxLength: 50),
                        OrgName = c.String(maxLength: 80),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.ZoneWaterMeters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ZoneId = c.Int(nullable: false),
                        Direct = c.Int(nullable: false),
                        meterId = c.String(maxLength: 20),
                        ZoneName = c.String(maxLength: 50),
                        longitude = c.Decimal(precision: 18, scale: 2),
                        latitude = c.Decimal(precision: 18, scale: 2),
                        Detail = c.String(maxLength: 80),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZoneWaterMeters", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.CustomerWaterQuotas", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerWaterMeters", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerWaterRecords", "CustomerId", "dbo.Customers");
            DropIndex("dbo.ZoneWaterMeters", new[] { "ZoneId" });
            DropIndex("dbo.Zones", new[] { "Name" });
            DropIndex("dbo.CustomerWaterQuotas", new[] { "CustomerId" });
            DropIndex("dbo.CustomerWaterMeters", new[] { "CustomerId" });
            DropIndex("dbo.CustomerWaterRecords", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "Name" });
            DropTable("dbo.ZoneWaterMeters");
            DropTable("dbo.Zones");
            DropTable("dbo.WaterMeters");
            DropTable("dbo.WaterMeterRecords");
            DropTable("dbo.CustomerWaterQuotas");
            DropTable("dbo.CustomerWaterMeters");
            DropTable("dbo.CustomerWaterRecords");
            DropTable("dbo.Customers");
            DropTable("dbo.AlarmLogs");
        }
    }
}
