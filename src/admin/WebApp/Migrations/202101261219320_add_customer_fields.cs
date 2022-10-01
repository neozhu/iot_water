namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_customer_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Code", c => c.String(maxLength: 50));
            AddColumn("dbo.Customers", "Category", c => c.String(maxLength: 50));
            AddColumn("dbo.Customers", "Status", c => c.String(maxLength: 10));
      //CreateIndex("dbo.Customers", "Code", unique: true);
      var indexName = "IX_Code";
      var tableName = "dbo.Customers";
      var columnName = "Code";
      Sql(string.Format(@"
    CREATE UNIQUE NONCLUSTERED INDEX {0}
    ON {1}({2}) 
    WHERE {2} IS NOT NULL;",
indexName, tableName, columnName));
    }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", new[] { "Code" });
            DropColumn("dbo.Customers", "Status");
            DropColumn("dbo.Customers", "Category");
            DropColumn("dbo.Customers", "Code");
        }
    }
}
