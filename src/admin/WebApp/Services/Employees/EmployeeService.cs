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
  /// File: EmployeeService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 12/26/2019 9:29:16 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class EmployeeService : Service<Employee>, IEmployeeService
  {
    private readonly IRepositoryAsync<Employee> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly NLog.ILogger logger;
    public EmployeeService(
      IRepositoryAsync<Employee> repository,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
    }
    public async Task<IEnumerable<Employee>> GetByCompanyIdAsync(int companyid) => await repository.GetByCompanyIdAsync(companyid);
    public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentid) => await repository.GetByDepartmentIdAsync(departmentid);



    private async Task<int> getCompanyIdByNameAsync(string name)
    {
      var companyRepository = this.repository.GetRepositoryAsync<Company>();
      var company = await companyRepository.Queryable().Where(x => x.Name == name).FirstOrDefaultAsync();
      if (company == null)
      {
        throw new Exception("not found ForeignKey:CompanyId with " + name);
      }
      else
      {
        return company.Id;
      }
    }
    private async Task<int> getDepartmentIdByNameAsync(string name)
    {
      var departmentRepository = this.repository.GetRepositoryAsync<Department>();
      var department = await departmentRepository.Queryable().Where(x => x.Name == name).FirstOrDefaultAsync();
      if (department == null)
      {
        throw new Exception("not found ForeignKey:DepartmentId with " + name);
      }
      else
      {
        return department.Id;
      }
    }
    public async Task ImportDataTableAsync(DataTable datatable)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "Employee" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到Employee对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null && !row.IsNull(requiredfield))
        {
          var item = new Employee();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var employeetype = item.GetType();
              var propertyInfo = employeetype.GetProperty(field.FieldName);
              //关联外键查询获取Id
              switch (field.FieldName)
              {
                case "CompanyId":
                  var companyname = row[field.SourceFieldName].ToString();
                  var companyid = await this.getCompanyIdByNameAsync(companyname);
                  propertyInfo.SetValue(item, Convert.ChangeType(companyid, propertyInfo.PropertyType), null);
                  break;
                case "DepartmentId":
                  var name = row[field.SourceFieldName].ToString();
                  var departmentid = await this.getDepartmentIdByNameAsync(name);
                  propertyInfo.SetValue(item, Convert.ChangeType(departmentid, propertyInfo.PropertyType), null);
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
              var employeetype = item.GetType();
              var propertyInfo = employeetype.GetProperty(field.FieldName);
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
             .Where(x => x.EntitySetName == "Employee")
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName
             }).ToArrayAsync();

      var employees = await this.Query(new EmployeeQuery().Withfilter(filters)).Include(p => p.Company).Include(p => p.Department).OrderBy(n => n.OrderBy(sort, order)).SelectAsync();

      var datarows = employees.Select(n => new
      {

        CompanyName = n.Company?.Name,
        DepartmentName = n.Department?.Name,
        Id = n.Id,
        Name = n.Name,
        Title = n.Title,
        PhoneNumber = n.PhoneNumber,
        WX = n.WX,
        Sex = n.Sex,
        Age = n.Age,
        Brithday = n.Brithday.ToString("yyyy-MM-dd HH:mm:ss"),
        EntryDate = n.EntryDate.ToString("yyyy-MM-dd HH:mm:ss"),
        IsDeleted = n.IsDeleted,
        LeaveDate = n.LeaveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
        CompanyId = n.CompanyId,
        DepartmentId = n.DepartmentId
      }).ToList();
      return await NPOIHelper.ExportExcelAsync(typeof(Employee), datarows, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items =await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        this.Delete(item);
      }

    }
  }
}



