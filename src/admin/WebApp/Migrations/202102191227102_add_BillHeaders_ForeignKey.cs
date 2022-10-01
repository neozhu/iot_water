namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_BillHeaders_ForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BillDetails", "BillHeader_Id", "dbo.BillHeaders");
            DropIndex("dbo.BillDetails", new[] { "BillHeader_Id" });
            DropColumn("dbo.BillDetails", "BillId");
            RenameColumn(table: "dbo.BillDetails", name: "BillHeader_Id", newName: "BillId");
            AlterColumn("dbo.BillDetails", "BillId", c => c.Int(nullable: false));
            CreateIndex("dbo.BillDetails", "BillId");
            AddForeignKey("dbo.BillDetails", "BillId", "dbo.BillHeaders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BillDetails", "BillId", "dbo.BillHeaders");
            DropIndex("dbo.BillDetails", new[] { "BillId" });
            AlterColumn("dbo.BillDetails", "BillId", c => c.Int());
            RenameColumn(table: "dbo.BillDetails", name: "BillId", newName: "BillHeader_Id");
            AddColumn("dbo.BillDetails", "BillId", c => c.Int(nullable: false));
            CreateIndex("dbo.BillDetails", "BillHeader_Id");
            AddForeignKey("dbo.BillDetails", "BillHeader_Id", "dbo.BillHeaders", "Id");
        }
    }
}
