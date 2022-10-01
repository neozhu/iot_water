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
  /// File: ZoneWaterMeterService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/28/2020 6:45:19 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class ZoneWaterMeterService : Service<ZoneWaterMeter>, IZoneWaterMeterService
  {
    private readonly IRepositoryAsync<ZoneWaterMeter> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly NLog.ILogger logger;
    public ZoneWaterMeterService(
      IRepositoryAsync<ZoneWaterMeter> repository,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
    }
    public async Task<IEnumerable<ZoneWaterMeter>> GetByZoneIdAsync(int zoneid) => await repository.GetByZoneIdAsync(zoneid);



    private async Task<int> getZoneIdByNameAsync(string name)
    {
      var zoneRepository = this.repository.GetRepositoryAsync<Zone>();
      var zone = await zoneRepository.Queryable().Where(x => x.Name == name).FirstOrDefaultAsync();
      if (zone == null)
      {
        throw new Exception("not found ForeignKey:ZoneId with " + name);
      }
      else
      {
        return zone.Id;
      }
    }
    public async Task ImportDataTableAsync(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "ZoneWaterMeter" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到ZoneWaterMeter对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null || !row.IsNull(requiredfield))
        {
          var item = new ZoneWaterMeter();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var zonewatermetertype = item.GetType();
              var propertyInfo = zonewatermetertype.GetProperty(field.FieldName);
              //关联外键查询获取Id
              switch (field.FieldName)
              {
                case "ZoneId":
                  var name = row[field.SourceFieldName].ToString();
                  var zoneid = await this.getZoneIdByNameAsync(name);
                  propertyInfo.SetValue(item, Convert.ChangeType(zoneid, propertyInfo.PropertyType), null);
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
              var zonewatermetertype = item.GetType();
              var propertyInfo = zonewatermetertype.GetProperty(field.FieldName);
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
             .Where(x => x.EntitySetName == "ZoneWaterMeter" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo = x.LineNo
             }).ToArrayAsync();

      var zonewatermeters = await this.Query(
        new ZoneWaterMeterQuery()
        .Withfilter(filters))
  .Include(p => p.Zone)
  .OrderBy(n => n.OrderBy(sort, order))
  .SelectAsync();


      return await NPOIHelper.ExportExcelAsync("zonewatermeters", zonewatermeters, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        this.Delete(item);
      }

    }

    public override void Update(ZoneWaterMeter entity)
    {
      var rep = repository.GetRepositoryAsync<WaterMeter>();
      var meter = rep.Queryable().Where(x => x.meterId == entity.meterId).FirstOrDefault();
      if (meter != null)
      {
        meter.ZoneName = entity.ZoneName;
        rep.Update(meter);
      }
      base.Update(entity);
    }
    public override void Insert(ZoneWaterMeter entity)
    {
      var rep = repository.GetRepositoryAsync<WaterMeter>();
      var meter = rep.Queryable().Where(x => x.meterId == entity.meterId).FirstOrDefault();
      if (meter != null)
      {
        meter.ZoneName = entity.ZoneName;
        rep.Update(meter);
      }
      base.Insert(entity);
    }
  }
}



