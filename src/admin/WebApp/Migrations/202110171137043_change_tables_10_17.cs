namespace WebApp.Migrations
{
  using System;
  using System.Data.Entity.Migrations;

  public partial class change_tables_10_17 : DbMigration
  {
    public override void Up()
    {
      AddColumn("dbo.WaterManualReports", "meterType", c => c.String(maxLength: 20));
      AddColumn("dbo.WaterMeterRecords", "meterType", c => c.String(maxLength: 20));
      AddColumn("dbo.WaterMeters", "longitude", c => c.Decimal(precision: 18, scale: 6));
      AddColumn("dbo.WaterMeters", "latitude", c => c.Decimal(precision: 18, scale: 6));
    }

    public override void Down()
    {
      DropColumn("dbo.WaterMeters", "latitude");
      DropColumn("dbo.WaterMeters", "longitude");
      DropColumn("dbo.WaterMeterRecords", "meterType");
      DropColumn("dbo.WaterManualReports", "meterType");
    }
  }
}
