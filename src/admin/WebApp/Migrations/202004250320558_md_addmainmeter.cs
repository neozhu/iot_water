namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md_addmainmeter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MainMeters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false),
                        inwater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        involume = c.Decimal(nullable: false, precision: 18, scale: 2),
                        outwater = c.Decimal(nullable: false, precision: 18, scale: 2),
                        outvolume = c.Decimal(nullable: false, precision: 18, scale: 2),
                        remark = c.String(maxLength: 120),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 20),
                        LastModifiedDate = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 20),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MainMeters");
        }
    }
}
