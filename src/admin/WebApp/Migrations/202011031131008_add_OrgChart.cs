namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_OrgChart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrgCharts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        No = c.String(maxLength: 3),
                        Level = c.String(nullable: false, maxLength: 16),
                        LevelNo = c.String(nullable: false, maxLength: 3),
                        Location = c.String(maxLength: 128),
                        Precision = c.String(maxLength: 8),
                        DN = c.String(maxLength: 8),
                        Year = c.String(maxLength: 8),
                        Remark = c.String(maxLength: 128),
                        ParentId = c.Int(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrgCharts", t => t.ParentId)
                .Index(t => t.No, unique: true)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrgCharts", "ParentId", "dbo.OrgCharts");
            DropIndex("dbo.OrgCharts", new[] { "ParentId" });
            DropIndex("dbo.OrgCharts", new[] { "No" });
            DropTable("dbo.OrgCharts");
        }
    }
}
