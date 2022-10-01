namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_changetb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerWaterRecords", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.CustomerWaterRecords", "CreatedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.CustomerWaterRecords", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.CustomerWaterRecords", "LastModifiedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.CustomerWaterRecords", "TenantId", c => c.Int(nullable: false));
            AddColumn("dbo.CustomerWaterQuotas", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.CustomerWaterQuotas", "CreatedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.CustomerWaterQuotas", "LastModifiedDate", c => c.DateTime());
            AddColumn("dbo.CustomerWaterQuotas", "LastModifiedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.CustomerWaterQuotas", "TenantId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerWaterQuotas", "TenantId");
            DropColumn("dbo.CustomerWaterQuotas", "LastModifiedBy");
            DropColumn("dbo.CustomerWaterQuotas", "LastModifiedDate");
            DropColumn("dbo.CustomerWaterQuotas", "CreatedBy");
            DropColumn("dbo.CustomerWaterQuotas", "CreatedDate");
            DropColumn("dbo.CustomerWaterRecords", "TenantId");
            DropColumn("dbo.CustomerWaterRecords", "LastModifiedBy");
            DropColumn("dbo.CustomerWaterRecords", "LastModifiedDate");
            DropColumn("dbo.CustomerWaterRecords", "CreatedBy");
            DropColumn("dbo.CustomerWaterRecords", "CreatedDate");
        }
    }
}
