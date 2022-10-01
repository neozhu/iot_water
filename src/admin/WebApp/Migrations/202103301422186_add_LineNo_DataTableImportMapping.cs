namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_LineNo_DataTableImportMapping : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataTableImportMappings", "LineNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataTableImportMappings", "LineNo");
        }
    }
}
