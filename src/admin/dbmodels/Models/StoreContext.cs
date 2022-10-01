using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Repository.Pattern.Ef6;
using Z.EntityFramework.Plus;

namespace WebApp.Models
{
  public class StoreContext : DbContext
  {


   public StoreContext()
        : base("Name=DefaultConnection") {
      //获取登录用户信息,tenantid
      //QueryFilterManager.AllowPropertyFilter = true;
      var claimsidentity = (ClaimsIdentity)HttpContext.Current?.User.Identity;
      var tenantclaim = claimsidentity?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid");
      var tenantid = Convert.ToInt32(tenantclaim?.Value);
      //设置当对Work对象进行查询时默认添加过滤条件
      //QueryDbSetFilterManager.Filter<Work>(q => q.Where(x => x.TenantId == tenantid));
      //this.Filter<Work>(q => q.Where(x => x.TenantId == tenantid));
      //设置当对Order对象进行查询时默认添加过滤条件
      //QueryDbSetFilterManager.Filter<Order>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<Customer>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<CustomerWaterMeter>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<CustomerWaterQuota>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<CustomerWaterRecord>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<CustomerBill>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<WaterMeter>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<WaterMeterRecord>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<Zone>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<ZoneWaterMeter>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<AlarmLog>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<ApiConfig>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<MainMeter>(q => q.Where(x => x.TenantId == tenantid));
      this.Filter<Customer>(q => q.Where(x => x.TenantId == tenantid));
      //QueryDbSetFilterManager.InitilizeGlobalFilter(this);
       
    }
    #region 系统必要的表
    public System.Data.Entity.DbSet<WebApp.Models.Company> Companies { get; set; }
    public System.Data.Entity.DbSet<WebApp.Models.Department> Departments { get; set; }
    public System.Data.Entity.DbSet<WebApp.Models.Employee> Employees { get; set; }
    public System.Data.Entity.DbSet<WebApp.Models.CodeItem> CodeItems { get; set; }
    public System.Data.Entity.DbSet<WebApp.Models.MenuItem> MenuItems { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<RoleMenu> RoleMenus { get; set; }
    public DbSet<DataTableImportMapping> DataTableImportMappings { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    #endregion

    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerWaterMeter> CustomerWaterMeters { get; set; }
    public DbSet<CustomerWaterQuota> CustomerWaterQuotas { get; set; }
    public DbSet<CustomerWaterRecord> CustomerWaterRecords { get; set; }
    public DbSet<CustomerBill> CustomerBills { get; set; }
    public DbSet<WaterMeter> WaterMeters { get; set; }
    public DbSet<WaterMeterRecord> WaterMeterRecords { get; set; }
    public DbSet<Zone> Zones { get; set; }
    public DbSet<ZoneWaterMeter> ZoneWaterMeters { get; set; }
    public DbSet<AlarmLog> AlarmLogs { get; set; }
       
    public DbSet<ApiConfig> ApiConfigs { get; set; }
    public DbSet<MainMeter> MainMeters { get; set; }
    public DbSet<OrgChart> OrgCharts { get; set; }

    public DbSet<BillHeader> BillHeaders { get; set; }
    public DbSet<BillDetail> BillDetails { get; set; }

    public DbSet<WaterManualRecord> WaterManualRecords { get; set; }
    public DbSet<WaterManualReport>  WaterManualReports { get; set; }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
      var currentDateTime = DateTime.Now;
      var claimsidentity = (ClaimsIdentity)HttpContext.Current?.User.Identity;
      var tenantclaim = claimsidentity?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid");
      var tenantid = Convert.ToInt32(tenantclaim?.Value);
      foreach (var auditableEntity in this.ChangeTracker.Entries<Entity>())
      {
        if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
        {
          //auditableEntity.Entity.LastModifiedDate = currentDateTime;
          switch (auditableEntity.State)
          {
            case EntityState.Added:
              auditableEntity.Property("LastModifiedDate").IsModified = false;
              auditableEntity.Property("LastModifiedBy").IsModified = false;
              auditableEntity.Entity.CreatedDate = currentDateTime;
              auditableEntity.Entity.CreatedBy = claimsidentity.Name;
              auditableEntity.Entity.TenantId = tenantid;
              break;
            case EntityState.Modified:
              auditableEntity.Property("CreatedDate").IsModified = false;
              auditableEntity.Property("CreatedBy").IsModified = false;
              auditableEntity.Entity.LastModifiedDate = currentDateTime;
              auditableEntity.Entity.LastModifiedBy = claimsidentity.Name;
              auditableEntity.Entity.TenantId = tenantid;
              //if (auditableEntity.Property(p => p.Created).IsModified || auditableEntity.Property(p => p.CreatedBy).IsModified)
              //{
              //    throw new DbEntityValidationException(string.Format("Attempt to change created audit trails on a modified {0}", auditableEntity.Entity.GetType().FullName));
              //}
              break;
          }
        }
      }
      return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
      var currentDateTime = DateTime.Now;
      var claimsidentity =(ClaimsIdentity)HttpContext.Current?.User.Identity;
      var tenantclaim = claimsidentity?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid");
      var tenantid = Convert.ToInt32(tenantclaim?.Value);
      foreach (var auditableEntity in this.ChangeTracker.Entries<Entity>())
      {
        if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
        {
          auditableEntity.Entity.LastModifiedDate = currentDateTime;
          switch (auditableEntity.State)
          {
            case EntityState.Added:
              auditableEntity.Property("LastModifiedDate").IsModified = false;
              auditableEntity.Property("LastModifiedBy").IsModified = false;
              auditableEntity.Entity.CreatedDate = currentDateTime;
              auditableEntity.Entity.CreatedBy = claimsidentity.Name;
              auditableEntity.Entity.TenantId = tenantid;
              break;
            case EntityState.Modified:
              auditableEntity.Property("CreatedDate").IsModified = false;
              auditableEntity.Property("CreatedBy").IsModified = false;
              auditableEntity.Entity.LastModifiedDate = currentDateTime;
              auditableEntity.Entity.LastModifiedBy = claimsidentity.Name;
              auditableEntity.Entity.TenantId = tenantid;
              break;
          }
        }
      }
      return base.SaveChanges();
    }


    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      //Oracle 表所有者，（SQL 改成 dbo(默认)，也可删除此设置）
      //this.ApplyAllConventionsIfOracle(modelBuilder);
      //modelBuilder.HasDefaultSchema(DbSchema);
      ////将数据列转换成大写
      //modelBuilder.Conventions.Add<UpperCaseColumnNameConvention>();
      ////将TableName转大写,TableName 指定的除外
      //modelBuilder.Conventions.Add<UpperCaseTableNameConvention>();

      #region 设置特殊格式以及浮点型长度
      //统一设置Decimal 长度（数据库实际位数可以缩短）
      //modelBuilder.Properties<decimal>().Configure(c => c.HasPrecision(18, 5));
      modelBuilder.Entity<ZoneWaterMeter>().Property(x => x.latitude).HasPrecision(18, 6);
      modelBuilder.Entity<ZoneWaterMeter>().Property(x => x.longitude).HasPrecision(18, 6);
      modelBuilder.Entity<WaterMeter>().Property(x => x.latitude).HasPrecision(18, 6);
      modelBuilder.Entity<WaterMeter>().Property(x => x.longitude).HasPrecision(18, 6);
      modelBuilder.Entity<Customer>().Property(x => x.WaterPrice).HasPrecision(18, 3);
      modelBuilder.Entity<BillHeader>().Property(x => x.WaterPrice).HasPrecision(18, 3);
      modelBuilder.Entity<Customer>().Property(x => x.ServicePrice).HasPrecision(18, 3);
      modelBuilder.Entity<BillHeader>().Property(x => x.ServicePrice).HasPrecision(18, 3);
      //modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasPrecision(18, 3);



      #endregion

      ////关闭一对多的级联删除。
      //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
      ////关闭多对多的级联删除。
      //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
      ////移除EF的表名公约  
      //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      ////移除对MetaData表的查询验证，否则每次都要访问Metadata这个表
      //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

      ////数据库不存在时重新创建数据库
      //Database.SetInitializer<StoreContext>(new CreateDatabaseIfNotExists<WebdbContext>());
      ////每次启动应用程序时创建数据库
      //Database.SetInitializer<StoreContext>(new DropCreateDatabaseAlways<WebdbContext>());
      ////模型更改时重新创建数据库
      //Database.SetInitializer<StoreContext>(new DropCreateDatabaseIfModelChanges<WebdbContext>());
      ////从不创建数据库
      //Database.SetInitializer<StoreContext>(null);




      ;

      base.OnModelCreating(modelBuilder);
    }


  }



}