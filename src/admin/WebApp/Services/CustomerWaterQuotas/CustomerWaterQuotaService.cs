using System;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Repository.Pattern.Infrastructure;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
using System.Data.Entity.SqlServer;

namespace WebApp.Services
{
  /// <summary>
  /// File: CustomerWaterQuotaService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/25/2020 10:35:58 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class CustomerWaterQuotaService : Service<CustomerWaterQuota>, ICustomerWaterQuotaService
  {
    private readonly IRepositoryAsync<CustomerWaterQuota> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly NLog.ILogger logger;
    private readonly SqlSugar.ISqlSugarClient db;
    private readonly IAlarmLogService alarmLogService;
    public CustomerWaterQuotaService(
      IRepositoryAsync<CustomerWaterQuota> repository,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger,
      SqlSugar.ISqlSugarClient db,
      IAlarmLogService alarmLogService
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
      this.db = db;
      this.alarmLogService = alarmLogService;
    }
    public async Task<IEnumerable<CustomerWaterQuota>> GetByCustomerIdAsync(int customerid) => await repository.GetByCustomerIdAsync(customerid);



    private async Task<int> getCustomerIdByNameAsync(string name)
    {
      var customerRepository = this.repository.GetRepositoryAsync<Customer>();
      var customer = await customerRepository.Queryable().Where(x => x.Name == name).FirstOrDefaultAsync();
      if (customer == null)
      {
        throw new Exception("not found ForeignKey:CustomerId with " + name);
      }
      else
      {
        return customer.Id;
      }
    }
    public async Task ImportDataTableAsync(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "CustomerWaterQuota" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到CustomerWaterQuota对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null || !row.IsNull(requiredfield))
        {
          var item = new CustomerWaterQuota();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var customerwaterquotatype = item.GetType();
              var propertyInfo = customerwaterquotatype.GetProperty(field.FieldName);
              //关联外键查询获取Id
              switch (field.FieldName)
              {
                case "CustomerId":
                  var name = row[field.SourceFieldName].ToString();
                  var customerid = await this.getCustomerIdByNameAsync(name);
                  propertyInfo.SetValue(item, Convert.ChangeType(customerid, propertyInfo.PropertyType), null);
                  break;
                default:
                  var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                  var safeValue = ( row[field.SourceFieldName] == null ) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
                  propertyInfo.SetValue(item, safeValue, null);
                  break;
              }
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var customerwaterquotatype = item.GetType();
              var propertyInfo = customerwaterquotatype.GetProperty(field.FieldName);
              if (string.Equals(defval, "now", StringComparison.OrdinalIgnoreCase) && ( propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(Nullable<DateTime>) ))
              {
                var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var safeValue = Convert.ChangeType(DateTime.Now, safetype);
                propertyInfo.SetValue(item, safeValue, null);
              }
              else if (string.Equals(defval, "guid", StringComparison.OrdinalIgnoreCase))
              {
                propertyInfo.SetValue(item, Guid.NewGuid().ToString(), null);
              }
              else if (string.Equals(defval, "user", StringComparison.OrdinalIgnoreCase))
              {
                propertyInfo.SetValue(item, username, null);
              }
              else
              {
                var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var safeValue = Convert.ChangeType(defval, safetype);
                propertyInfo.SetValue(item, safeValue, null);
              }
            }
          }
          this.Insert(item);
        }
      }
    }
    public async Task<Stream> ExportExcelAsync(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var expcolopts = await this.mappingservice.Queryable()
             .Where(x => x.EntitySetName == "CustomerWaterQuota" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo=x.LineNo
             }).ToArrayAsync();

      var customerwaterquotas = await this.Query(
        new CustomerWaterQuotaQuery()
        .Withfilter(filters))
        .Include(p => p.Customer)
        .OrderBy(n => n.OrderBy(sort, order))
        .SelectAsync();

      
      return await NPOIHelper.ExportExcelAsync("customerwaterquotas", customerwaterquotas, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        this.Delete(item);
      }

    }

    public async Task UpdateMonthWater() {
      var sql = @"select t.customerid,
t.customername, 
t.[year],
t.[month],
sum(t.water) [water] from (
select t2.CustomerId,
t2.CustomerName, 
t1.meterid,
DATEPART(yy, t1.[datetime]) [year] ,
DATEPART(mm, t1.[datetime]) [month],
min(t1.[datetime]) [startdate],
min(t1.[water]) [start] ,
max(t1.[datetime]) [enddate],
max(t1.[water]) [end],
max(t1.[water]) - min(t1.[water]) [water] from [dbo].[WaterMeterRecords] t1 
inner join [dbo].[WaterMeters] t2 on t1.[meterId]=t2.[meterId]
--where DATEPART(yy, t1.[datetime])=2020 and DATEPART(mm, t1.[datetime])=2
group by t2.CustomerId,t2.CustomerName, t1.meterid,DATEPART(yy, t1.[datetime]) ,DATEPART(mm, t1.[datetime])
) t
group by t.CustomerId,t.CustomerName,t.[year],t.[month]";
      var data =await this.db.Ado.SqlQueryAsync<dynamic>(sql);
      var now = DateTime.Now;
      foreach (var item in data)
      {
        var customerId = (int)item.customerid;
        var customerName = (string)item.customername;
        var year = (int)item.year;
        var month = (int)item.month;
        var quota = this.Queryable().Where(x => x.CustomerId == customerId && x.Year == year && x.Month == month).FirstOrDefault();
        if (quota != null)
        {
          quota.Water = (decimal)item.water;
          quota.RecordDate = now;
          var days = DateTime.DaysInMonth(year, month);
          if (DateTime.Now.Month <= month)
          {
            quota.ForecastWater = quota.Water * days / DateTime.Now.Day;
          }
          else
          {
            quota.ForecastWater = quota.Water;
          }
          this.Update(quota);

          //判断用水量是否超过定额，超过预警.


          var content = "";
          var level = "一级";
          if (quota.ForecastWater > quota.Quota)
          {
            content = $"预测用水量:{quota.ForecastWater.ToString("0.0")} > 定额:{quota.Quota.ToString("0.0")} 报警。";
            if (quota.Water > quota.Quota)
            {
              content += $"实际用水量:{quota.Water.ToString("0.0")} 》定额:{quota.Quota.ToString("0.0")} 报警。";
              level = "二级";
            }
            var any =await this.alarmLogService.Queryable().Where(x => x.DeviceId == customerName &&
              SqlFunctions.DateDiff("d", now, x.InitDateTime) == 0 && x.Status == "待处理").AnyAsync();
            if (!any)
            {
              var insertsql = @"INSERT INTO [dbo].[AlarmLogs]
           ([DeviceId]
           ,[Status]
           ,[Type]
           ,[Level]
           ,[Content]
           ,[InitDateTime]
           ,[User]
       ,[TenantId]
           )
     VALUES
           (@DeviceId
           , @Status
           , @Type
           , @Level
           , @Content
           , @InitDateTime
       , @User
           , @TenantId)";
              await db.Ado.ExecuteCommandAsync(insertsql, new
              {
                DeviceId = customerName,
                Status = "待处理",
                Type = "阈值",
                Level = level,
                Content = content,
                InitDateTime = now,
                User = "sys",
                TenantId = 1
              });
            }
          }
        }


      }

    }
  }
}



