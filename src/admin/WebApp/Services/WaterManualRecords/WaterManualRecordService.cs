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
using ClosedXML.Excel;
using WebApp.Models.Dto;

namespace WebApp.Services
{
  /// <summary>
  /// File: WaterManualRecordService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/6/2021 5:29:06 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class WaterManualRecordService : Service<WaterManualRecord>, IWaterManualRecordService
  {
    private readonly IRepositoryAsync<WaterManualRecord> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly ICustomerWaterMeterService customerWaterMeterService;
    private readonly NLog.ILogger logger;
    private readonly IWaterMeterService waterMeterService;
    private readonly IWaterMeterRecordService waterMeterRecordService;
    private readonly ICodeItemService codeItemService;
    private readonly SqlSugar.ISqlSugarClient db;
    public WaterManualRecordService(
      ICodeItemService codeItemService,
      IWaterMeterService waterMeterService,
      IWaterMeterRecordService waterMeterRecordService,
      IRepositoryAsync<WaterManualRecord> repository,
      IDataTableImportMappingService mappingservice,
      ICustomerWaterMeterService customerWaterMeterService,
      SqlSugar.ISqlSugarClient db,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.db = db;
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.customerWaterMeterService = customerWaterMeterService;
      this.logger = logger;
      this.waterMeterService = waterMeterService;
      this.waterMeterRecordService = waterMeterRecordService;
      this.codeItemService = codeItemService;
    }



    public async Task<List<CreateReportDto>> ImportDataTable(DataTable datatable, string username)
    {
      var result = new List<CreateReportDto>();
      var day =this.codeItemService.Queryable()
         .Where(x => x.CodeType == "endday").First().Code;
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "WaterManualRecord" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到WaterManualRecord对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
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
          var item = new WaterManualRecord();
          var watermanualrecordtype = item.GetType();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain &&
                           !row.IsNull(field.SourceFieldName) &&
                           !string.IsNullOrEmpty(row[field.SourceFieldName].ToString())
                        )
            {
              var propertyInfo = watermanualrecordtype.GetProperty(field.FieldName);
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
              var propertyInfo = watermanualrecordtype.GetProperty(field.FieldName);
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
          var meter =await this.waterMeterService.Queryable().Where(x => x.meterId == item.meterId).FirstOrDefaultAsync();
          if (meter == null)
          {
            throw new Exception($"表号:{item.meterId},不存在,请先维护水表基础数据后再导入.");
          }
          meter.water = item.Water;
          this.waterMeterService.Update(meter);
          var any =await this.Queryable().Where(x => x.meterId == item.meterId && x.RecordDate == item.RecordDate).FirstOrDefaultAsync();
          if (any == null)
          {
            
            item.Name1 = meter.Name1;
            item.Name = meter.Name;
            item.LineNo = meter.LineNo;
            item.Address = meter.address;
            var history =await this.waterMeterRecordService.Queryable().Where(x => x.meterId == item.meterId && x.datetime == item.RecordDate).FirstOrDefaultAsync();
            if (history != null)
            {
              history.water = item.Water;
              this.waterMeterRecordService.Update(history);
            }
            this.Insert(item);
          }
          else
          {
            var history =await this.waterMeterRecordService.Queryable().Where(x => x.meterId == item.meterId && x.datetime == item.RecordDate).FirstOrDefaultAsync();
            if (history != null)
            {
              history.water = item.Water;
              this.waterMeterRecordService.Update(history);
            }
            else
            {
              var rec = new WaterMeterRecord()
              {
                meterId = item.meterId,
                water = item.Water,
                datetime = item.RecordDate
              };
              this.waterMeterRecordService.Insert(rec);
            }
            any.Water = item.Water;
            this.Update(any);
          }
          var month =$"{item.RecordDate.Year}年{item.RecordDate.Month}月";
          var exist = result.Any(x => x.meterId == item.meterId && x.Month == month);
          if (!exist)
          {
            result.Add(new CreateReportDto()
            {
              Day = Convert.ToInt32(day),
              meterId = item.meterId,
              Month = month

            });
            ;
          }
        }
      }

      return result;
    }
    public async Task<Stream> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var expcolopts = await this.mappingservice.Queryable()
             .Where(x => x.EntitySetName == "WaterManualRecord" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo = x.LineNo
             }).ToArrayAsync();

      var watermanualrecords =await this.Query(
        new WaterManualRecordQuery().Withfilter(filters)
        )
        .OrderBy(n => n.OrderBy($"{sort} {order}"))
        .SelectAsync();
      
      return await NPOIHelper.ExportExcelAsync("抄表记录", watermanualrecords, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        var records = await this.waterMeterRecordService.Queryable().Where(x => x.meterId == item.meterId && x.datetime == item.RecordDate).ToListAsync();
        foreach(var rec in records)
        {
          this.waterMeterRecordService.Delete(rec);
        }
        this.Delete(item);
        

      }

    }
    public override void Insert(WaterManualRecord entity)
    {
      var meter = this.waterMeterService.Queryable().Where(x => x.meterId == entity.meterId).First();
      meter.water = entity.Water;
      meter.LastWater = entity.Water;
      meter.LastWaterDate = entity.RecordDate;
      this.waterMeterService.Update(meter);
      var customer =this.customerWaterMeterService.Queryable().Where(x => x.meterId == entity.meterId).FirstOrDefault();
      var record = new WaterMeterRecord()
      {
        water = entity.Water,
        meterId = entity.meterId,
        datetime = entity.RecordDate,
        CustomerId=customer?.Id,
        CustomerName= customer?.CustomerName
      };
      this.waterMeterRecordService.Insert(record);
      base.Insert(entity);
    }

    public async Task<Stream> CreateImportTemplate() {
      var sql = @"SELECT 
[LineNo] as '表序号'
,[Name] as '表名'
,[meterId] as '表号'
,[Name1] as '出线名称'
,[address] as '具体位置'
,[Rate] as '倍率'
,[CustomerName] as '使用单位'
,[meterType] as '水表类型'
,[Status] as '水表状态'
,[water] as '水表当前读数'
,'' as '本期读数'
,'' as '抄表日期'
  FROM [dbo].[WaterMeters]
  where meterType = N'机械表' and Status=N'使用中'";
      var dt =await this.db.Ado.GetDataTableAsync(sql);
      var wb = new XLWorkbook();
      wb.Worksheets.Add(dt);
      var stream = new MemoryStream();
      wb.SaveAs(stream);
      stream.Position = 0;
      return stream;
    }
  }
}