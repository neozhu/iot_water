namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_ : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AlarmLogs", "TenantId");
            CreateIndex("dbo.ApiConfigs", "TenantId");
            CreateIndex("dbo.Companies", "TenantId");
            CreateIndex("dbo.Departments", "TenantId");
            CreateIndex("dbo.Employees", "TenantId");
            CreateIndex("dbo.CodeItems", "TenantId");
            CreateIndex("dbo.CustomerBills", "TenantId");
            CreateIndex("dbo.Customers", "TenantId");
            CreateIndex("dbo.CustomerWaterRecords", "TenantId");
            CreateIndex("dbo.CustomerWaterMeters", "TenantId");
            CreateIndex("dbo.CustomerWaterQuotas", "TenantId");
            CreateIndex("dbo.DataTableImportMappings", "TenantId");
            CreateIndex("dbo.Logs", "TenantId");
            CreateIndex("dbo.MainMeters", "TenantId");
            CreateIndex("dbo.MenuItems", "TenantId");
            CreateIndex("dbo.Notifications", "TenantId");
            CreateIndex("dbo.OrgCharts", "TenantId");
            CreateIndex("dbo.RoleMenus", "TenantId");
            CreateIndex("dbo.WaterMeterRecords", "TenantId");
            CreateIndex("dbo.WaterMeters", "TenantId");
            CreateIndex("dbo.Zones", "TenantId");
            CreateIndex("dbo.ZoneWaterMeters", "TenantId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ZoneWaterMeters", new[] { "TenantId" });
            DropIndex("dbo.Zones", new[] { "TenantId" });
            DropIndex("dbo.WaterMeters", new[] { "TenantId" });
            DropIndex("dbo.WaterMeterRecords", new[] { "TenantId" });
            DropIndex("dbo.RoleMenus", new[] { "TenantId" });
            DropIndex("dbo.OrgCharts", new[] { "TenantId" });
            DropIndex("dbo.Notifications", new[] { "TenantId" });
            DropIndex("dbo.MenuItems", new[] { "TenantId" });
            DropIndex("dbo.MainMeters", new[] { "TenantId" });
            DropIndex("dbo.Logs", new[] { "TenantId" });
            DropIndex("dbo.DataTableImportMappings", new[] { "TenantId" });
            DropIndex("dbo.CustomerWaterQuotas", new[] { "TenantId" });
            DropIndex("dbo.CustomerWaterMeters", new[] { "TenantId" });
            DropIndex("dbo.CustomerWaterRecords", new[] { "TenantId" });
            DropIndex("dbo.Customers", new[] { "TenantId" });
            DropIndex("dbo.CustomerBills", new[] { "TenantId" });
            DropIndex("dbo.CodeItems", new[] { "TenantId" });
            DropIndex("dbo.Employees", new[] { "TenantId" });
            DropIndex("dbo.Departments", new[] { "TenantId" });
            DropIndex("dbo.Companies", new[] { "TenantId" });
            DropIndex("dbo.ApiConfigs", new[] { "TenantId" });
            DropIndex("dbo.AlarmLogs", new[] { "TenantId" });
        }
    }
}
