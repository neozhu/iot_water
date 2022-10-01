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
  /// File: ZoneService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/28/2020 6:17:43 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class ZoneService : Service<Zone>, IZoneService
  {
    private readonly IRepositoryAsync<Zone> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly IZoneWaterMeterService zoneWaterMeterService;
    private readonly IWaterMeterService waterMeterService;
    private readonly NLog.ILogger logger;
    public ZoneService(
      IRepositoryAsync<Zone> repository,
      IZoneWaterMeterService zoneWaterMeterService,
      IWaterMeterService waterMeterService,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
      this.waterMeterService = waterMeterService;
      this.zoneWaterMeterService = zoneWaterMeterService;
    }



    public async Task ImportDataTableAsync(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "Zone" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到Zone对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null || !row.IsNull(requiredfield))
        {
          var item = new Zone();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var zonetype = item.GetType();
              var propertyInfo = zonetype.GetProperty(field.FieldName);
              var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
              var safeValue = ( row[field.SourceFieldName] == null ) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
              propertyInfo.SetValue(item, safeValue, null);
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var zonetype = item.GetType();
              var propertyInfo = zonetype.GetProperty(field.FieldName);
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
             .Where(x => x.EntitySetName == "Zone" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo=x.LineNo
             }).ToArrayAsync();

      var zones = await this.Query(new ZoneQuery()
        .Withfilter(filters))
        .OrderBy(n => n.OrderBy(sort, order))
        .SelectAsync();
      
      return await NPOIHelper.ExportExcelAsync("zones", zones, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        this.Delete(item);
      }

    }

    public async Task AddWaterMeter(int zoneid, string meterid, decimal lng, decimal lat)
    {
      var meter = await this.waterMeterService.Queryable().Where(x => x.meterId == meterid).FirstAsync();
      meter.latitude = lat;
      meter.longitude = lng;
      this.waterMeterService.Update(meter);
      //var zone = await this.FindAsync(zoneid);
      //meter.address = zone.Name;
      //this.waterMeterService.Update(meter);
      //var zonewater = await this.zoneWaterMeterService.Queryable()
      //  .Where(x => x.meterId == meterid && x.ZoneId == zoneid).FirstOrDefaultAsync();
      //if (zonewater != null)
      //{
      //  zonewater.latitude = lat;
      //  zonewater.longitude = lng;
      //  this.zoneWaterMeterService.Update(zonewater);
      //}
      //else
      //{
      //  var waterzone = new ZoneWaterMeter()
      //  {
      //     Direct=1,
      //      latitude=lat,
      //       longitude=lng,
      //        meterId=meterid,
      //         TenantId=meter.TenantId,
      //          ZoneId=zoneid,
      //           ZoneName=zone.Name

      //  };
      //  this.zoneWaterMeterService.Insert(waterzone);
      //}
    }

    public async Task UpdateMeterLoc(int zoneid, string meterid, decimal lng, decimal lat)
    {
      var meter = await this.waterMeterService.Queryable().Where(x => x.meterId == meterid).FirstAsync();
      meter.longitude = lng;
      meter.latitude = lat;
      this.waterMeterService.Update(meter);
    }

    public async Task DeleteWaterMeter(int zoneid, string meterid)
    {
      var meter = await this.waterMeterService.Queryable().Where(x => x.meterId == meterid).FirstAsync();
      meter.longitude = null;
      meter.latitude = null;
      this.waterMeterService.Update(meter);
      
    }
    public async Task ChangeDirect(int zoneid, string meterid, int direct) {
      var zonewater = await this.zoneWaterMeterService.Queryable()
       .Where(x => x.meterId == meterid && x.ZoneId == zoneid).FirstOrDefaultAsync();
      if (zonewater != null)
      {
        zonewater.Direct = direct;
        this.zoneWaterMeterService.Update(zonewater);
      }

    }
  }
}



