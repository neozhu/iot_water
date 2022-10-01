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
  /// File: CustomerService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/25/2020 9:58:11 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class CustomerService : Service<Customer>, ICustomerService
  {
    private readonly IRepositoryAsync<Customer> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly ICustomerWaterMeterService customerWaterMeterService;
    private readonly ICustomerWaterQuotaService customerWaterQuotaService;
    private readonly ICustomerWaterRecordService customerWaterRecordService;
    private readonly IWaterMeterService waterMeterService;
    private readonly NLog.ILogger logger;
    public CustomerService(
      IRepositoryAsync<Customer> repository,
      ICustomerWaterMeterService customerWaterMeterService,
      ICustomerWaterQuotaService customerWaterQuotaService,
      ICustomerWaterRecordService customerWaterRecordService,
      IWaterMeterService waterMeterService,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
      this.customerWaterMeterService = customerWaterMeterService;
      this.customerWaterQuotaService = customerWaterQuotaService;
      this.customerWaterRecordService = customerWaterRecordService;
      this.waterMeterService = waterMeterService;
    }
    public async Task<IEnumerable<CustomerWaterRecord>> GetCustomerWaterRecordsByCustomerIdAsync(int customerid) => await repository.GetCustomerWaterRecordsByCustomerIdAsync(customerid);
    public async Task<IEnumerable<CustomerWaterMeter>> GetWaterMetersByCustomerIdAsync(int customerid) => await repository.GetWaterMetersByCustomerIdAsync(customerid);
    public async Task<IEnumerable<CustomerWaterQuota>> GetWaterQuotasByCustomerIdAsync(int customerid) => await repository.GetWaterQuotasByCustomerIdAsync(customerid);



    public async Task ImportDataTableAsync(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "Customer" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到Customer对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null || !row.IsNull(requiredfield))
        {
          var item = new Customer();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var customertype = item.GetType();
              var propertyInfo = customertype.GetProperty(field.FieldName);
              var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
              var safeValue = ( row[field.SourceFieldName] == null ) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
              propertyInfo.SetValue(item, safeValue, null);
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var customertype = item.GetType();
              var propertyInfo = customertype.GetProperty(field.FieldName);
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
             .Where(x => x.EntitySetName == "Customer" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo=x.LineNo
             }).ToArrayAsync();

      var customers =await this.Query(
        new CustomerQuery()
        .Withfilter(filters))
        .OrderBy(n => n.OrderBy(sort, order))
        .SelectAsync();
      
      return await NPOIHelper.ExportExcelAsync("customers", customers, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable()
        .Where(x => id.Contains(x.Id))
        .Include(x=>x.CustomerWaterRecords)
        .ToListAsync();
      foreach (var item in items)
      {
        if (await this.customerWaterMeterService.Queryable().Where(x=>x.CustomerId==item.Id).AnyAsync())
        {
          throw new Exception($"该单位:{item.Name}已维护有水表信息不允许删除");
        }

        this.Delete(item);
      }

    }

    public async Task CreateCustomerWaterMeters(CustomerWaterMeter[] items)
    {
      foreach (var item in items)
      {
        item.RegisterDate = DateTime.Now;
        this.customerWaterMeterService.Insert(item);
        var meter =await this.waterMeterService.Queryable().Where(x => x.meterId == item.meterId).FirstAsync();
        meter.CustomerId = item.CustomerId;
        meter.CustomerName = item.CustomerName;
        meter.address = item.Remark;
        meter.Name1 = item.Points;
        meter.Level = item.Level;
        meter.ZoneName = item.ZoneName;
        meter.OpeningDate = DateTime.Now;
        this.waterMeterService.Update(meter);
      }
    }

    public async Task UpdateCustomerWaterMeters(CustomerWaterMeter[] items)
    {
      foreach (var item in items)
      {
        item.RegisterDate = DateTime.Now;
        var customermeter =await this.customerWaterMeterService.FindAsync(item.Id);
        if (customermeter.meterId != item.meterId)
        {
          var originalmeter = await this.waterMeterService.Queryable().Where(x => x.meterId == customermeter.meterId).FirstAsync();
          originalmeter.CustomerId = 0;
          originalmeter.CustomerName = "";
          originalmeter.address = "";
          originalmeter.OpeningDate = null;
          this.waterMeterService.Update(originalmeter);
        }

        customermeter.meterId = item.meterId;
        customermeter.CustomerId = item.CustomerId;
        customermeter.CustomerName = item.CustomerName;
        customermeter.Dept = item.Dept;
        customermeter.Discount = item.Discount;
        customermeter.Level = item.Level;
        customermeter.Points = item.Points;
        customermeter.Quota = item.Quota;
        customermeter.Remark = item.Remark;
        customermeter.ZoneName = item.ZoneName;
        customermeter.Negitive = item.Negitive;
        customermeter.Ratio = item.Ratio;
        customermeter.meterName = item.meterName;
        this.customerWaterMeterService.Update(customermeter);
        var meter = await this.waterMeterService.Queryable().Where(x => x.meterId == item.meterId).FirstAsync();
        meter.CustomerId = item.CustomerId;
        meter.CustomerName = item.CustomerName;
        meter.address = item.Remark;
        meter.Name1 = item.Points;
        meter.Level = item.Level;
        meter.ZoneName = item.ZoneName;
        meter.OpeningDate = DateTime.Now;
        this.waterMeterService.Update(meter);

      }
    }
    public async Task CreateCustomerWaterQuotas(int customerid, int year) {
      var customer =await this.FindAsync(customerid);
      for (var i = 1; i <= 12; i++)
      {
        var any = await this.customerWaterQuotaService.Queryable().Where(x => x.CustomerId == customer.Id && x.Year == year && x.Month == i).AnyAsync();
        if (!any)
        {
          var quota = new CustomerWaterQuota()
          {
            CustomerId = customer.Id,
            CustomerName = customer.Name,
            Quota = customer.Quota,
            Year = year,
            Month = i,
            RecordDate=null,
            Water=0

          };
          this.customerWaterQuotaService.Insert(quota);
        }
      }
    }

    public async Task EnableSelected(int[] id) {
      var items =await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        item.Status = "启用";
        this.Update(item);
      }
      }
    public async Task DisableSelected(int[] id) {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        item.Status = "停用";
        this.Update(item);
      }
    }


    public async Task ChangeWatePrice(decimal value)
    {
      var items = await this.Queryable().ToListAsync();
      foreach(var item in items)
      {
        item.WaterPrice = item.WaterPrice + value;
        this.Update(item);
      }
      this.logger.Info($"统一调整水价:{value}");
    }
  }
}



