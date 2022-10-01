namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addapitb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(nullable: false, maxLength: 50),
                        Host = c.String(nullable: false, maxLength: 50),
                        SecretAccessKey = c.String(maxLength: 250),
                        AccessKeyId = c.String(maxLength: 250),
                        UserId = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        Description = c.String(maxLength: 250),
                        CompanyId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApiConfigs", "CompanyId", "dbo.Companies");
            DropIndex("dbo.ApiConfigs", new[] { "CompanyId" });
            DropTable("dbo.ApiConfigs");
        }
    }
}
