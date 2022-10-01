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
using System.Linq.Dynamic.Core;
using Service.Pattern;
using System.Text.RegularExpressions;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Models.Dto;

namespace WebApp.Services
{
  /// <summary>
  /// File: WaterManualReportService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/6/2021 5:31:32 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class WaterManualReportService : Service<WaterManualReport>, IWaterManualReportService
  {
    private readonly IWaterMeterService meterService;
    private readonly IWaterMeterRecordService recordService;
    private readonly IRepositoryAsync<WaterManualReport> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly NLog.ILogger logger;
    public WaterManualReportService(
      IWaterMeterService meterService,
      IWaterMeterRecordService recordService,
      IRepositoryAsync<WaterManualReport> repository,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.meterService = meterService;
      this.recordService = recordService;
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
    }



    public async Task ImportDataTable(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "WaterManualReport" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到WaterManualReport对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null ||
              ( !row.IsNull(requiredfield) &&
               !string.IsNullOrEmpty(row[requiredfield].ToString())
              )
            )
        {
          var item = new WaterManualReport();
          var watermanualreporttype = item.GetType();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain &&
                           !row.IsNull(field.SourceFieldName) &&
                           !string.IsNullOrEmpty(row[field.SourceFieldName].ToString())
                        )
            {
              var propertyInfo = watermanualreporttype.GetProperty(field.FieldName);
              var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
              var safeValue = Convert.ChangeType(row[field.SourceFieldName], safetype);
              if (!string.IsNullOrEmpty(field.RegularExpression))
              {
                var isValid = Regex.IsMatch(safeValue.ToString(), field.RegularExpression);
                if (!isValid)
                {
                  throw new Exception($"{field.SourceFieldName}:{safeValue} 表达式验证不匹配:{field.RegularExpression}");
                }
              }
              propertyInfo.SetValue(item, safeValue, null);
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var propertyInfo = watermanualreporttype.GetProperty(field.FieldName);
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
          if (string.IsNullOrEmpty(item.meterType)){
            item.meterType = "机械表";
          }
          this.Insert(item);
        }
      }
    }
    public async Task<Stream> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var expcolopts = await this.mappingservice.Queryable()
             .Where(x => x.EntitySetName == "WaterManualReport" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo=x.LineNo
             }).ToArrayAsync();

      var watermanualreports = await this.Query(
        new WaterManualReportQuery()
        .Withfilter(filters))
        .OrderBy(n => n.OrderBy($"{sort} {order}"))
        .SelectAsync();
     
      return await NPOIHelper.ExportExcelAsync("水表月度汇总记录", watermanualreports, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        this.Delete(item);
      }

    }

    public async Task<WaterManualReport> Find(string meterid, int year ,int month) {
      var item = await this.Queryable().Where(x => x.meterId == meterid &&
      x.RecordDate.Year == year && x.RecordDate.Month == month)
        .OrderBy(x => x.Id)
        .FirstOrDefaultAsync();
      return item;
    }

    public async Task CreateReport(List<CreateReportDto> request) {
      foreach (var input in request)
      {
        var meterid = input.meterId;
        var month = input.Month;
        var day = input.Day;
        var meter =await this.meterService.Find(meterid);

        var endate = DateTime.Parse($"{month}{day}日");
        var startdate = endate.AddMonths(-1);

        if (meter == null)
        {
          this.logger.Warn($"创建{month}账单时:{meterid} 不存在.");
          continue;
          //throw new Exception($"{meter.meterId} 不存在.");
        }
        var endwater = await this.recordService.GetEndWaterRecord(startdate, endate, meter.meterId);
        if (endwater == null)
        {
          this.logger.Warn($"{meterid} 没有找到 {month} 用水记录.");
          continue;
        }
        else
        {
          var startw = 0m;
          decimal? lastcal = 0m;
          decimal? onyear = 0m;
          var startd = new DateTime(1990, 1, 1);
          var startwater = await this.Find(meter.meterId, startdate.Year, startdate.Month);
          if (startwater == null)
          {
            var startwater2 = await this.recordService.GetStartWaterRecord(startdate, endate, meter.meterId);
            if (startwater2 != null)
            {
              startw = startwater2.water;
              startd = startwater2.datetime;

            }
            lastcal = 0;
          }
          else
          {
            startw = startwater.Water;
            startd = startwater.RecordDate;
            lastcal = startwater.CalWater;
            //找去年同期的用水量
            var onyearw = await this.Find(meter.meterId, startdate.Year - 1, startdate.Month);
            if (onyearw != null)
            {
              onyear = onyearw.CalWater;
            }
          }
          //判断一下是否已经导入过当月的抄表数据，有就删掉，重新导
          var existreport =await this.Queryable().Where(x => x.Month == month && x.meterId == meter.meterId).FirstOrDefaultAsync();
          if (existreport != null)
          {
            this.Delete(existreport);
          }
          var rate = meter.Rate ?? 1;
          var water = ( endwater.water - startw ) * rate;
          var report = new WaterManualReport()
          {
            CalWater = endwater.water - startw,
            LastWater = startw,
            LastRecordDate = startd,
            LineNo = meter.LineNo,
            meterId = meter.meterId,
            Name = meter.Name,
            Name1 = meter.Name1,
            RecordDate = endwater.datetime,
            Water = endwater.water,
            LastCalWater = lastcal,
            OnYearCalWater = onyear,
            BillNo = null,
            Month = month
          };
          if (report.OnYearCalWater > 0)
          {
            report.YearRatio = ( report.CalWater - report.OnYearCalWater ) / report.OnYearCalWater * 100;
          }
          if (report.LastCalWater > 0)
          {
            report.LastRatio = ( report.CalWater - report.LastCalWater ) / report.LastCalWater * 100;
          }
          this.Insert(report);
          
        }
      }

    }
  }
}