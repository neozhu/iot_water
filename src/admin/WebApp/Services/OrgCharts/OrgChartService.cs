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
using System.Text.RegularExpressions;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
  /// <summary>
  /// File: OrgChartService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 2020/11/3 19:40:02
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class OrgChartService : Service<OrgChart>, IOrgChartService
  {
    private readonly IRepositoryAsync<OrgChart> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly NLog.ILogger logger;
    public OrgChartService(
      IRepositoryAsync<OrgChart> repository,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
    }
    public async Task<IEnumerable<OrgChart>> GetByParentId(int parentid) => await repository.GetByParentId(parentid);



    private async Task<int> getParentIdByNo(string no)
    {
      var orgchartRepository = this.repository.GetRepositoryAsync<OrgChart>();
      var orgchart = await orgchartRepository.Queryable().Where(x => x.No == no).FirstOrDefaultAsync();
      if (orgchart == null)
      {
        throw new Exception("not found ForeignKey:ParentId with " + no);
      }
      else
      {
        return orgchart.Id;
      }
    }
    public async Task ImportDataTable(DataTable datatable, string username)
    {
      var list = new List<OrgChart>();
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "OrgChart" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到OrgChart对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
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
          var item = new OrgChart();
          var orgcharttype = item.GetType();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain &&
                           !row.IsNull(field.SourceFieldName) &&
                           !string.IsNullOrEmpty(row[field.SourceFieldName].ToString())
                        )
            {
              var propertyInfo = orgcharttype.GetProperty(field.FieldName);
             
               
                  var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                  var safeValue = Convert.ChangeType(row[field.SourceFieldName], safetype);
                  propertyInfo.SetValue(item, safeValue, null);
                  
             
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var propertyInfo = orgcharttype.GetProperty(field.FieldName);
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
          list.Add(item);

        }
      }

      if (list.Count > 0)
      {
        foreach (var item in list)
        {
          if (item.ParentId != null && item.ParentId > 0)
          {
            var parent = list.Find(x => x.No == item.ParentId.ToString());
            item.Parent = parent;
            item.ParentId = parent.Id;
              }
        }
        this.InsertRange(list);
      }
    }
    public async Task<Stream> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var expcolopts = await this.mappingservice.Queryable()
             .Where(x => x.EntitySetName == "OrgChart")
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName
             }).ToArrayAsync();

      var orgcharts = await this.Query(new OrgChartQuery().Withfilter(filters)).Include(p => p.Parent).OrderBy(n => n.OrderBy(sort, order)).SelectAsync();

      var datarows = orgcharts.Select(n => new
      {

        ParentNo = n.Parent?.No,
        Id = n.Id,
        No = n.No,
        Level = n.Level,
        LevelNo = n.LevelNo,
        Location = n.Location,
        Precision = n.Precision,
        DN = n.DN,
        Year = n.Year,
        Remark = n.Remark,
        ParentId = n.ParentId
      }).ToList();
      return await NPOIHelper.ExportExcelAsync(typeof(OrgChart), datarows, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        this.Delete(item);
      }

    }
  }
}