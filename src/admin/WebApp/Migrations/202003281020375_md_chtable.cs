namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_chtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Zones", "GeoJSON", c => c.String());
            AddColumn("dbo.Zones", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.Zones", "CreatedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.Zones", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.Zones", "LastModifiedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.Zones", "TenantId", c => c.Int(nullable: false));
            AddColumn("dbo.ZoneWaterMeters", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ZoneWaterMeters", "CreatedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.ZoneWaterMeters", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.ZoneWaterMeters", "LastModifiedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.ZoneWaterMeters", "TenantId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ZoneWaterMeters", "TenantId");
            DropColumn("dbo.ZoneWaterMeters", "LastModifiedBy");
            DropColumn("dbo.ZoneWaterMeters", "LastModifiedDate");
            DropColumn("dbo.ZoneWaterMeters", "CreatedBy");
            DropColumn("dbo.ZoneWaterMeters", "CreatedDate");
            DropColumn("dbo.Zones", "TenantId");
            DropColumn("dbo.Zones", "LastModifiedBy");
            DropColumn("dbo.Zones", "LastModifiedDate");
            DropColumn("dbo.Zones", "CreatedBy");
            DropColumn("dbo.Zones", "CreatedDate");
            DropColumn("dbo.Zones", "GeoJSON");
        }
    }
}
