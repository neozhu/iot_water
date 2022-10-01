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
using WebApp.Models.JsonModel;
using RestClient.Net.Abstractions.Extensions;
using System.Globalization;
using CsvHelper;
using Polly.Extensions.Http;
using Polly;

namespace WebApp.Services
{
  /// <summary>
  /// File: WaterMeterRecordService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/17/2020 10:29:03 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class WaterMeterRecordService : Service<WaterMeterRecord>, IWaterMeterRecordService
  {
    private readonly IRepositoryAsync<WaterMeterRecord> repository;
    private readonly IRepositoryAsync<WaterMeter> meterRepository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly IApiConfigService apiConfigService;
    private readonly NLog.ILogger logger;
    private readonly IMapper mapper;
    private readonly ICustomerWaterMeterService customerWaterMeterService;
    public WaterMeterRecordService(
      ICustomerWaterMeterService customerWaterMeterService,
      IRepositoryAsync<WaterMeter> meterRepository,
      IRepositoryAsync<WaterMeterRecord> repository,
      IDataTableImportMappingService mappingservice,
      IApiConfigService apiConfigService,
      IMapper mapper,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
      this.apiConfigService = apiConfigService;
      this.mapper = mapper;
      this.meterRepository = meterRepository;
      this.customerWaterMeterService = customerWaterMeterService;
    }



    public async Task ImportDataTableAsync(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "WaterMeterRecord" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到WaterMeterRecord对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null || !row.IsNull(requiredfield))
        {
          var item = new WaterMeterRecord();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var watermeterrecordtype = item.GetType();
              var propertyInfo = watermeterrecordtype.GetProperty(field.FieldName);
              var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
              var safeValue = ( row[field.SourceFieldName] == null ) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
              propertyInfo.SetValue(item, safeValue, null);
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var watermeterrecordtype = item.GetType();
              var propertyInfo = watermeterrecordtype.GetProperty(field.FieldName);
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

          var exist = await this.Queryable()
           .Where(x => x.meterId == item.meterId &&
           x.datetime == item.datetime)
           .AnyAsync();
          if (!exist)
          {
            var customerwater = await this.customerWaterMeterService.Queryable().Where(x => x.meterId == item.meterId).FirstOrDefaultAsync();
            if (customerwater != null)
            {
              item.CustomerId = customerwater.CustomerId;
              item.CustomerName = customerwater.CustomerName;
            }
            this.Insert(item);
          }
        }
      }
    }
    public async Task<Stream> ExportExcelAsync(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);


      var watermeterrecords = await this.Query(
        new WaterMeterRecordQuery()
        .Withfilter(filters))
        .OrderBy(n => n.OrderBy(sort, order))
        .SelectAsync();
      byte[] result = null;
      using (var stream = new MemoryStream())
      {
        using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8))
        {
          using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
          {
            await csv.WriteRecordsAsync(watermeterrecords);

          }
        }
        result = stream.ToArray();
      }

      return new MemoryStream(result);
    }

    public async Task<Stream> ExportExcelAsyncv(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var expcolopts = await this.mappingservice.Queryable()
             .Where(x => x.EntitySetName == "WaterMeterRecord")
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName
             }).ToArrayAsync();

      var watermeterrecords = this.Query(new WaterMeterRecordQuery().Withfilter(filters)).OrderBy(n => n.OrderBy(sort, order)).Select().ToList();
      var datarows = watermeterrecords.Select(n => new
      {

        Id = n.Id,
        meterStatus = n.meterStatus,
        datetime = n.datetime.ToString("yyyy-MM-dd HH:mm:ss"),
        meterId = n.meterId,
        water = n.water,
        reverseWater = n.reverseWater,
        temperature = n.temperature,
        flowrate = n.flowrate,
        pressure = n.pressure,
        voltage = n.voltage,
        valveStatus = n.valveStatus,
        userId = n.userId,
        imei = n.imei,
        OrgName = n.OrgName
      }).ToList();
      return await NPOIHelper.ExportExcelAsync(typeof(WaterMeterRecord), datarows, expcolopts);
    }
    public void Delete(int[] id)
    {
      var items = this.Queryable().Where(x => id.Contains(x.Id));
      foreach (var item in items)
      {
        this.Delete(item);
      }

    }

    public async Task SyncLastDataByDatetime(int tenantid)
    {
      var config = await this.apiConfigService.GetWaterMeterService(tenantid);
      var method = "interface/findQueryLastDataByDatetime.do";
      var url = $"{config.Host}{method}";
      var meterid = await this.meterRepository.Queryable()
        .Where(x => x.TenantId == tenantid &&
        x.meterType == "智能表")
        .Select(x => x.meterId).ToArrayAsync();
      var client = new Client(new NewtonsoftSerializationAdapter(), new Uri(url));
      client.SetJsonContentTypeHeader();
      var result = await client
        .PostAsync<findQueryLastDataByDatetimeResponse, findQueryLastDataByDatetimeRequest>(
        new findQueryLastDataByDatetimeRequest
        {
          checkCode = config.SecretAccessKey,
          count = 99,
          data = meterid,
          datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
      if (result.IsSuccess && result.Body.result == "success")
      {
        var distinctarray = result.Body.data.Select(x => new { x.datetime, x.meterId, x.water }).Distinct().ToList();
        foreach (var dist in distinctarray)
        {
          var item = result.Body.data.Where(x => x.datetime == dist.datetime && x.meterId == dist.meterId).First();
          var customerwater = await this.customerWaterMeterService.Queryable().Where(x => x.meterId == item.meterId).FirstOrDefaultAsync();
          var record = this.mapper.Map<WaterMeterRecord>(item);
          record.CustomerId = customerwater?.CustomerId;
          record.CustomerName = customerwater?.CustomerName;
          var exist = await this.Queryable()
            .Where(x => x.meterId == record.meterId &&
            x.datetime == record.datetime)
            .AnyAsync();
          if (!exist)
          {
            this.Insert(record);
          }
        }
      }
    }

    public async Task DownloadDetailDataByDatetime(int tenantid, string[] meteridarray, DateTime dt1, DateTime dt2)
    {
      var config = await this.apiConfigService.GetWaterMeterService(tenantid);
      var method = "interface/findQueryDetailDataByDatetime.do";
      var url = $"{config.Host}{method}";
      //var meteridarray = await this.meterRepository.Queryable()
      //  .Where(x => x.TenantId == tenantid &&
      //   x.meterType == "智能表")
      //  .Select(x => x.meterId).ToArrayAsync();
      var policy = HttpPolicyExtensions
   .HandleTransientHttpError()
   .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
   .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
      while (dt1.AddDays(1) <= dt2)
      {
        foreach (var meterid in meteridarray)
        {
          
          var tries = 0;
          var client = new Client(new NewtonsoftSerializationAdapter(),
              null,
              new Uri(url),
              null,
              createHttpClient: (httpClient) =>
              {
                return new SingletonHttpClientFactory().CreateClient(httpClient);
              },
             sendHttpRequestFunc: (httpClient, httpRequestMessageFunc, logger, cancellationToken) =>
             {
               return policy.ExecuteAsync(() =>
               {
                 var httpRequestMessage = httpRequestMessageFunc.Invoke();

                 //On the third try change the Url to a the correct one
                 tries++;
                 return httpClient.SendAsync(httpRequestMessage, cancellationToken);
               });
             });
          client.SetJsonContentTypeHeader();
          var result = await client
            .PostAsync<findQueryDetailDataByDatetimeResponse, findQueryDetailDataByDatetimeRequest>(
            new findQueryDetailDataByDatetimeRequest
            {
              checkCode = config.SecretAccessKey,
              count = 1,
              data = new string[] { meterid },
              datetime = dt1.ToString("yyyy-MM-dd")
            });
          if (result.IsSuccess && result.Body.result == "success")
          {
            var distinctarray = result.Body.data.Select(x => new { x.datetime, x.meterId, x.water }).Distinct().ToList();
            foreach (var dist in distinctarray)
            {
              var item = result.Body.data.Where(x => x.datetime == dist.datetime && x.meterId == dist.meterId).First();
              var customerwater = await this.customerWaterMeterService.Queryable().Where(x => x.meterId == item.meterId).FirstOrDefaultAsync();
              var record = this.mapper.Map<WaterMeterRecord>(item);
              record.CustomerId = customerwater?.CustomerId;
              record.CustomerName = customerwater?.CustomerName;
              var exist = await this.Queryable()
                .Where(x => x.meterId == record.meterId &&
                x.datetime == record.datetime)
                .AnyAsync();
              if (!exist)
              {
                this.Insert(record);
              }
            }
          }
          else
          {
            throw new Exception(result.Body.message);
          }
        }
        dt1 = dt1.AddDays(1);
      }
    }

    public async Task<IEnumerable<WaterMeterRecord>> GetRangeWaterRecord(int year, int month, string meterid)
    {
      var count = await this.Queryable().Where(x => x.meterId == meterid && x.datetime.Year == year && x.datetime.Month == month).CountAsync();
      if (count >= 2)
      {
        var first = await this.Queryable().Where(x => x.meterId == meterid && x.datetime.Year == year && x.datetime.Month == month)
          .OrderBy(x => x.datetime).FirstAsync();
        var end = await this.Queryable().Where(x => x.meterId == meterid && x.datetime.Year == year && x.datetime.Month == month)
          .OrderByDescending(x => x.datetime).FirstAsync();
        return new WaterMeterRecord[] {
          first,end
          };
      }
      else if (count == 1)
      {

        var end = await this.Queryable().Where(x => x.meterId == meterid && x.datetime.Year == year && x.datetime.Month == month)
         .OrderByDescending(x => x.datetime).FirstAsync();
        var first = await this.Queryable().Where(x => x.meterId == meterid && x.datetime < end.datetime)
         .OrderByDescending(x => x.datetime).FirstOrDefaultAsync();
        if (first == null)
        {
          return new WaterMeterRecord[] {
          end
          };
        }
        return new WaterMeterRecord[] {
          first,end
          };
      }
      else
      {
        return null;
      }

    }

    public async Task<WaterMeterRecord> GetEndWaterRecord(DateTime dt1, DateTime dt2, string meterid)
    {
      var item = await this.Queryable().Where(x => x.datetime >= dt1 && x.datetime <= dt2 && x.meterId == meterid)
        .OrderByDescending(x => x.datetime).FirstOrDefaultAsync();
      return item;

    }
    public async Task<WaterMeterRecord> GetStartWaterRecord(DateTime dt1, DateTime dt2, string meterid)
    {

      var item = await this.Queryable().Where(x => x.datetime <= dt1 && x.meterId == meterid)
       .OrderByDescending(x => x.datetime).FirstOrDefaultAsync();
      return item;
    }
  }
}



