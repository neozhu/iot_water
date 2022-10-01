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

namespace WebApp.Services
{
  /// <summary>
  /// File: CustomerWaterRecordService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/25/2020 10:41:13 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class CustomerWaterRecordService : Service<CustomerWaterRecord>, ICustomerWaterRecordService
  {
    private readonly IRepositoryAsync<CustomerWaterRecord> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly NLog.ILogger logger;
    private readonly SqlSugar.ISqlSugarClient db;
    public CustomerWaterRecordService(
      IRepositoryAsync<CustomerWaterRecord> repository,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger,
      SqlSugar.ISqlSugarClient db
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
      this.db = db;
    }
    public async Task<IEnumerable<CustomerWaterRecord>> GetByCustomerIdAsync(int customerid) => await repository.GetByCustomerIdAsync(customerid);



    private async Task<int> getCustomerIdByNameAsync(string name)
    {
      var customerRepository = this.repository.GetRepositoryAsync<Customer>();
      var customer = await customerRepository.Queryable().Where(x => x.Name == name).FirstOrDefaultAsync();
      if (customer == null)
      {
        throw new Exception($"{name}没有找到." );
      }
      else
      {
        return customer.Id;
      }
    }
    private async Task<int> GetCustomerId(string customerName) {
      var customer =await this.repository.GetRepositoryAsync<Customer>()
        .Queryable()
        .Where(x => x.Name == customerName)
        .FirstOrDefaultAsync();
      if (customer == null)
      {
        throw new Exception($"{customerName} 不存在");
      }
      return customer.Id;
    }
    public async Task ImportDataTableAsync(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "CustomerWaterRecord" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到CustomerWaterRecord对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null || !row.IsNull(requiredfield))
        {
          var item = new CustomerWaterRecord();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var customerwaterrecordtype = item.GetType();
              var propertyInfo = customerwaterrecordtype.GetProperty(field.FieldName);
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
              var customerwaterrecordtype = item.GetType();
              var propertyInfo = customerwaterrecordtype.GetProperty(field.FieldName);
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
             .Where(x => x.EntitySetName == "CustomerWaterRecord" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo=x.LineNo
             }).ToArrayAsync();

      var customerwaterrecords = await this.Query(
        new CustomerWaterRecordQuery()
        .Withfilter(filters))
        .OrderBy(n => n.OrderBy(sort, order))
        .SelectAsync();

      
      return await NPOIHelper.ExportExcelAsync("customerwaterrecords", customerwaterrecords, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        this.Delete(item);
      }

    }

    public async Task AutoRecordMonth(int year, int month, string user) {
      var now = DateTime.Now;
      var sql = $@"select t2.customerid,
t2.customername, 
t1.meterid,
DATEPART(yy, t1.[datetime])[year] ,
DATEPART(mm, t1.[datetime])[month],
min(t1.[datetime])[startdate],
min(t1.[water])[start] ,
max(t1.[datetime])[enddate],
max(t1.[water])[end],
max(t1.[water]) - min(t1.[water])[water] from[dbo].[WaterMeterRecords] t1
inner join[dbo].[WaterMeters] t2 on t1.[meterId] = t2.[meterId]
where t2.meterType=N'智能表' and t1.CustomerId > 0 and DATEPART(yy, t1.[datetime])=@year and DATEPART(mm, t1.[datetime])=@month
group by t2.CustomerId,t2.CustomerName, t1.meterid,DATEPART(yy, t1.[datetime]) ,DATEPART(mm, t1.[datetime])
order by t2.CustomerId,t2.CustomerName, t1.meterid,DATEPART(yy, t1.[datetime]) ,DATEPART(mm, t1.[datetime])";
      var data = await this.db.Ado.SqlQueryAsync<dynamic>(sql, new { year, month });
      
      foreach (var item in data)
      {
        var meterId =(string)item.meterid;
        var customerId =(int)item.customerid;
        var customername = (string)item.customername;
        var record =await this.Queryable()
          .Where(x => x.Year == year && x.Month == month && x.CustomerId == customerId && x.meterId == meterId)
          .FirstOrDefaultAsync();
        
        if (record == null)
        {
          var exist =await this.hasCustomer(customerId);
          if (exist)
          {
            var previousData = await this.getpreviousDate(year, month, meterId, customerId);


            record = new CustomerWaterRecord()
            {
              meterId = meterId,
              // CustomerName= customername,
              CustomerId = customerId,
              Year = year,
              Month = month,
              lastWater = (decimal)item.end,
              Type = "自动",
              User = user,
              RecordDate = now,
            };
            if (previousData == null)
            {
              record.previousDate = (DateTime)item.startdate;
              record.previousWater = (decimal)item.start;
            }
            else
            {
              record.previousDate = (DateTime)previousData.enddate;
              record.previousWater = (decimal)previousData.end;
            }
            record.water = record.lastWater - record.previousWater;
            this.Insert(record);
          }
        }
        else
        {
          record.lastWater = (decimal)item.end;
          record.RecordDate = now;
          record.water = record.lastWater - record.previousWater;
          this.Update(record);
        }
         
      }

      //更新最新表读数
      var updatesql = "update [dbo].[WaterMeters] SET water = b.water FROM [dbo].[WaterMeters] a ,(select meterId,max(water) [water] from [dbo].[WaterMeterRecords] group by meterId) b  WHERE  a.meterId = b.meterId  ";
      await db.Ado.ExecuteCommandAsync(updatesql);
    }
    private async Task<bool> hasCustomer(int customerid) {
      var sql = "select count(1) from dbo.Customers where Id=@customerid";
      var result =await this.db.Ado.GetIntAsync(sql, new { customerid });
      if (result > 0)
      {
        return true;
      }
        
      else
      {
        return false;
      }
     }
    private async Task<dynamic> getpreviousDate(int year, int month, string meterid,int customerid) {
      if (month == 12)
      {
        year = year - 1;
        month = 1;
      }
      else
      {
        month = month - 1;
       }
      var sql = @" select t2.CustomerId,
t2.CustomerName, 
t1.meterid,
DATEPART(yy, t1.[datetime]) [year] ,
DATEPART(mm, t1.[datetime]) [month],
max(t1.[datetime]) [enddate],
max(t1.[water]) [end]
 from [dbo].[WaterMeterRecords] t1 
inner join [dbo].[WaterMeters] t2 on t1.[meterId]=t2.[meterId]
where DATEPART(yy, t1.[datetime])=@year
and DATEPART(mm, t1.[datetime])=@month
and t1.[meterId] = @meterid
and t2.[customerId]=@customerid
group by t2.CustomerId,t2.CustomerName, t1.meterid,DATEPART(yy, t1.[datetime]) ,DATEPART(mm, t1.[datetime])
order by t2.CustomerId,t2.CustomerName, t1.meterid,DATEPART(yy, t1.[datetime]) ,DATEPART(mm, t1.[datetime])";
      var data = await this.db.Ado.SqlQueryAsync<dynamic>(sql, new { year, month,meterid,customerid });
      return data.FirstOrDefault();
    }

    public async Task AddRecord(CustomerWaterRecord item)
    {
      this.Insert(item);
      var meter = await this.repository.GetRepositoryAsync<WaterMeter>()
        .Queryable().Where(x => x.meterId == item.meterId).FirstOrDefaultAsync();
      meter.water = item.lastWater;
      meter.LastWater = item.lastWater;
      meter.LastWaterDate = item.RecordDate;
      this.repository.GetRepositoryAsync<WaterMeter>().Update(meter);

    }

    public async Task Stop(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        item.IsDelete = true;
        this.Update(item);
      }
    }
  }
}



