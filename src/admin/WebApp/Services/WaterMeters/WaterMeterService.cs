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
using AutoMapper;
using RestClient.Net;
using RestClient.Net.Abstractions.Extensions;
using WebApp.Models.JsonModel;

namespace WebApp.Services
{
  /// <summary>
  /// File: WaterMeterService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/25/2020 9:40:44 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class WaterMeterService : Service<WaterMeter>, IWaterMeterService
  {
    private readonly IRepositoryAsync<WaterMeter> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly IApiConfigService apiConfigService;
    private readonly ICustomerWaterMeterService customerWaterMeterService;
    private readonly NLog.ILogger logger;
    private readonly IMapper mapper;
    private readonly IRepositoryAsync<Customer> customerrepository;
    public WaterMeterService(
      IRepositoryAsync<WaterMeter> repository,
      ICustomerWaterMeterService customerWaterMeterService,
      IDataTableImportMappingService mappingservice,
      IApiConfigService apiConfigService,
      NLog.ILogger logger,
      IMapper mapper
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.apiConfigService = apiConfigService;
      this.customerWaterMeterService = customerWaterMeterService;
      this.logger = logger;
      this.mapper = mapper;
      this.customerrepository = this.repository.GetRepositoryAsync<Customer>();
    }



    public async Task ImportDataTableAsync(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "WaterMeter" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到WaterMeter对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield == null || !row.IsNull(requiredfield))
        {
          var item = new WaterMeter();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var watermetertype = item.GetType();
              var propertyInfo = watermetertype.GetProperty(field.FieldName);
              var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
              var safeValue = ( row[field.SourceFieldName] == null ) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
              propertyInfo.SetValue(item, safeValue, null);
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var watermetertype = item.GetType();
              var propertyInfo = watermetertype.GetProperty(field.FieldName);
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
          var customer = await this.getCustomer(item.CustomerName);
          if (customer != null)
          {
            item.CustomerId = customer.Id;
            item.CustomerName = customer.Name;
          }
          this.Insert(item);
          
        }
      }
    }
    private async Task<Customer> getCustomer(string customername) {
      var customer =await this.customerrepository.Queryable().Where(x => x.Name == customername).FirstOrDefaultAsync();
      return customer;
    }
    
    public async Task<Stream> ExportExcelAsync(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var expcolopts = await this.mappingservice.Queryable()
             .Where(x => x.EntitySetName == "WaterMeter" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo=x.LineNo
             }).ToArrayAsync();

      var watermeters =await this.Query(new WaterMeterQuery().Withfilter(filters))
        .OrderBy(n => n.OrderBy(sort, order)).SelectAsync();
      return await NPOIHelper.ExportExcelAsync("WaterMeter", watermeters, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var rep = this.repository.GetRepositoryAsync<CustomerWaterMeter>();
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        var any =await rep.Queryable().Where(x => x.meterId == item.meterId).AnyAsync();
        if (any)
        {
          throw new Exception($"该表号{item.meterId}已分配给单位使用不允许删除");
        }
        this.Delete(item);
      }

    }

    public async Task SyncQueryUserInfo(int tenantid)
    {
      var config = await this.apiConfigService.GetWaterMeterService(tenantid);
      var method = "interface/findQueryUserInfo.do";
      var url = $"{config.Host}{method}";
      var client = new Client(new NewtonsoftSerializationAdapter(), new Uri(url));
      client.SetJsonContentTypeHeader();
      var result = await client.PostAsync<findQueryUserInfoResponse, findQueryUserInfoRequest>(new findQueryUserInfoRequest { checkCode = config.SecretAccessKey, startNO = 0, endNo = 99 });
      if (result.IsSuccess && result.Body.result == "success")
      {
        foreach (var item in result.Body.data)
        {
          var meter = this.mapper.Map<WaterMeter>(item);
          var exist = await this.Queryable().Where(x => x.meterId == meter.meterId).FirstOrDefaultAsync();
          var lastdata = await this.findQueryLastData(config, meter.meterId);
          meter.flowrate = lastdata.data.First().flowrate;
          meter.pressure = lastdata.data.First().pressure;
          meter.Status = lastdata.data.First().meterStatus;
          meter.temperature = lastdata.data.First().temperature;
          meter.valveStatus = lastdata.data.First().valveStatus;
          meter.voltage = lastdata.data.First().voltage;
          meter.water = lastdata.data.First().water;
          meter.meterType = "智能表";
          meter.Status = "使用中";
          meter.address = null;
          if (exist == null)
          {
            //meter.Status = "正常";
            this.Insert(meter);
          }
          else
          {
            exist.flowrate = lastdata.data.First().flowrate;
            exist.pressure = lastdata.data.First().pressure;
            exist.Status = lastdata.data.First().meterStatus;
            exist.temperature = lastdata.data.First().temperature;
            exist.valveStatus = lastdata.data.First().valveStatus;
            exist.voltage = lastdata.data.First().voltage;
            exist.water = lastdata.data.First().water;
            exist.address = meter.address;
            exist.imei = meter.imei;
            exist.meterSize = meter.meterSize;
            exist.userCode = meter.userCode;
            exist.userName = meter.userName;
            exist.valveStatus = meter.valveStatus;
            exist.meterType = "智能表";
            exist.Status = "使用中";
            this.Update(exist);
          }
        }
      }
    }
    public async Task OperateValveStatus(int tenantid, int id, int state)
    {
      var config = await this.apiConfigService.GetWaterMeterService(tenantid);
      var method = "interface/updateOperateValveStatus.do";
      var url = $"{config.Host}{method}";
      var meterid = await this.Queryable().Where(x => x.Id == id)
        .Select(x => x.meterId).ToArrayAsync();
      var client = new Client(new NewtonsoftSerializationAdapter(), new Uri(url));
      client.SetJsonContentTypeHeader();
      var result = await client.PostAsync<updateOperateValveStatusResponse, updateOperateValveStatusRequest>(
        new updateOperateValveStatusRequest
        {
          checkCode = config.SecretAccessKey,
          count = meterid.Length,
          type = 0,
          valve = state,
          data = meterid
        }
        );
      if (result.IsSuccess)
      {
        var res = result.Body;
        if (res.result == "fail")
        {
          throw new Exception(res.message);
        }
        else
        {
          var mid = res.data.First().meterId;
          var meter = this.Queryable().Where(x => x.meterId == mid).First();
          meter.valveStatus = res.data.First().valveStatus;
          this.Update(meter);
        }
      }
    }
    private async Task<findQueryLastDataResponse> findQueryLastData(ApiConfig config, string meterId)
    {
      var method = "interface/findQueryLastData.do";
      var url = $"{config.Host}{method}";
      var client = new Client(new NewtonsoftSerializationAdapter(), new Uri(url));
      client.SetJsonContentTypeHeader();
      var result = await client.PostAsync<findQueryLastDataResponse, findQueryLastDataRequest>(
        new findQueryLastDataRequest
        {
          checkCode = config.SecretAccessKey,
          count = 1,
          type = 0,
          data = new string[] { meterId }
        });
      return result.Body;
    }

    public async Task<WaterMeter> Find(string meterid) {
      return await this.Queryable().Where(x => x.meterId == meterid).FirstOrDefaultAsync();
      }

    public async Task Stop(int[] id) {
      var customermeter = this.repository.GetRepositoryAsync<CustomerWaterMeter>();
      var meters =await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach(var item in meters)
      {
        item.Status = "停用";
        item.ClosedDate = DateTime.Now;
        var meterrelation =await customermeter.Queryable().Where(x => x.meterId == item.meterId).ToListAsync();
        foreach(var rel in meterrelation)
        {
          rel.IsDelete = true;
          rel.DeleteDate = DateTime.Now;
          customermeter.Update(rel);
        }
        this.Update(item);
        this.logger.Info($"{item.meterId} 被停用");
      }
      }
    public async Task Enable(int[] id)
    {
      var customermeter = this.repository.GetRepositoryAsync<CustomerWaterMeter>();
      var meters = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in meters)
      {
        item.Status = "使用中";
        item.ClosedDate = null;
        item.OpeningDate = DateTime.Now;
        var meterrelation = await customermeter.Queryable().Where(x => x.meterId == item.meterId).ToListAsync();
        foreach (var rel in meterrelation)
        {
          rel.IsDelete = false;
          rel.DeleteDate = null;
          customermeter.Update(rel);
        }
        this.Update(item);
        this.logger.Info($"{item.meterId} 被启用");
      }
    }
  }
}



