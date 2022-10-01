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
  /// File: CustomerWaterMeterService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/25/2020 10:27:37 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class CustomerWaterMeterService : Service<CustomerWaterMeter>, ICustomerWaterMeterService
  {
    private readonly IRepositoryAsync<CustomerWaterMeter> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly NLog.ILogger logger;
    private readonly IRepositoryAsync<WaterMeter> waterrepository;
    public CustomerWaterMeterService(
      IRepositoryAsync<CustomerWaterMeter> repository,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
      this.waterrepository = this.repository.GetRepositoryAsync<WaterMeter>();
    }
    public async Task<IEnumerable<CustomerWaterMeter>> GetByCustomerIdAsync(int customerid) => await repository.GetByCustomerIdAsync(customerid);



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
                        .Where(x => x.EntitySetName == "CustomerWaterMeter" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到CustomerWaterMeter对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null || !row.IsNull(requiredfield))
        {
          var item = new CustomerWaterMeter();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var customerwatermetertype = item.GetType();
              var propertyInfo = customerwatermetertype.GetProperty(field.FieldName);
              //关联外键查询获取Id
              switch (field.FieldName)
              {
                case "CustomerId":
                  var name = row[field.SourceFieldName].ToString();
                  var customerid = await this.getCustomerIdByNameAsync(name);
                  item.CustomerName = name;
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
              var customerwatermetertype = item.GetType();
              var propertyInfo = customerwatermetertype.GetProperty(field.FieldName);
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

          var water =await this.waterrepository.Queryable().Where(x => x.meterId == item.meterId).FirstOrDefaultAsync();
          if (water != null)
          {
            water.CustomerId = item.CustomerId;
            water.CustomerName = item.CustomerName;
            this.waterrepository.Update(water);
          }
        }
      }
    }
    public async Task<Stream> ExportExcelAsync(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var expcolopts = await this.mappingservice.Queryable()
             .Where(x => x.EntitySetName == "CustomerWaterMeter" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo=x.LineNo
             }).ToArrayAsync();

      var customerwatermeters = await this
        .Query(new CustomerWaterMeterQuery()
        .Withfilter(filters))
        .Include(p => p.Customer)
        .OrderBy(n => n.OrderBy(sort, order))
        .SelectAsync();

       
      return await NPOIHelper.ExportExcelAsync("customerwatermeters", customerwatermeters, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var wrepostiry = this.repository.GetRepositoryAsync<WaterMeter>();
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        var meter =await wrepostiry.Queryable().Where(x => x.meterId == item.meterId).FirstOrDefaultAsync();
        if (meter != null)
        {
          meter.CustomerName = null;
          meter.CustomerId = -1;
          wrepostiry.Update(meter);
         }
        item.IsDelete = true;
        this.Update(item);
      }

    }

    public async Task Stop(int[] id)
    {
      var wrepostiry = this.repository.GetRepositoryAsync<WaterMeter>();
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        var meter = await wrepostiry.Queryable().Where(x => x.meterId == item.meterId).FirstOrDefaultAsync();
        if (meter != null)
        {
          meter.CustomerName = item.CustomerName;
          meter.CustomerId = item.CustomerId;
          meter.Status = "停用";
          wrepostiry.Update(meter);
        }
        item.IsDelete = true;
        item.DeleteDate = DateTime.Now;
        this.Update(item);
      }
    }

    public async Task Resume(int[] id)
    {
      var wrepostiry = this.repository.GetRepositoryAsync<WaterMeter>();
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        var meter = await wrepostiry.Queryable().Where(x => x.meterId == item.meterId).FirstOrDefaultAsync();
        if (meter != null)
        {
          meter.CustomerName = item.CustomerName;
          meter.CustomerId = item.CustomerId;
          meter.Status = "正常";
          wrepostiry.Update(meter);
        }
        item.IsDelete = false;
        item.DeleteDate = null;
        this.Update(item);
      }
    }
  }
}



