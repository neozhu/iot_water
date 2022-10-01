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
  /// File: CustomerBillService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 3/28/2020 3:02:42 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class CustomerBillService : Service<CustomerBill>, ICustomerBillService
  {
    private readonly IRepositoryAsync<CustomerBill> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly NLog.ILogger logger;
    private readonly SqlSugar.ISqlSugarClient db;
    public CustomerBillService(
      IRepositoryAsync<CustomerBill> repository,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger,
      SqlSugar.ISqlSugarClient db
      )
        : base(repository)
    {
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
      this.db = db;
    }
    public async Task<IEnumerable<CustomerBill>> GetByCustomerIdAsync(int customerid) => await repository.GetByCustomerIdAsync(customerid);



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
                        .Where(x => x.EntitySetName == "CustomerBill" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到CustomerBill对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null || !row.IsNull(requiredfield))
        {
          var item = new CustomerBill();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var customerbilltype = item.GetType();
              var propertyInfo = customerbilltype.GetProperty(field.FieldName);
              //关联外键查询获取Id
              switch (field.FieldName)
              {
                case "CustomerId":
                  var name = row[field.SourceFieldName].ToString();
                  var customerid = await this.getCustomerIdByNameAsync(name);
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
              var customerbilltype = item.GetType();
              var propertyInfo = customerbilltype.GetProperty(field.FieldName);
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
             .Where(x => x.EntitySetName == "CustomerBill" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo=x.LineNo
             }).ToArrayAsync();

      var customerbills = await this.Query(
        new CustomerBillQuery()
        .Withfilter(filters))
        .Include(p => p.Customer)
        .OrderBy(n => n.OrderBy(sort, order))
        .SelectAsync();

     
      return await NPOIHelper.ExportExcelAsync("customerbills", customerbills, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        this.Delete(item);
      }

    }

    private async Task<Customer> getCustomer(int customerid) {
      var rep = this.repository.GetRepositoryAsync<Customer>();
      var customer =await rep.FindAsync(customerid);
      return customer;

    }
    public async Task CreateBills(string user) {
      //获取水价
      var sql = "select code from dbo.codeitems where codetype='water'";
      var price =await this.db.Ado.GetDecimalAsync(sql);
      var month = DateTime.Now.Month;
      var year = DateTime.Now.Year;
      var now = DateTime.Now;
      if (month == 1)
      {
        month = 12;
        year = year - 1;
      }
      else
      {
        month = month - 1;
      }
      var records =await this.repository
        .GetRepositoryAsync<CustomerWaterRecord>()
        .Queryable()
        .Where(x=>x.Year<=year && x.Month<=month)
        .GroupBy(x=>new {x.CustomerId,x.Year,x.Month })
        .Select(x => new { x.Key, water = x.Sum(y => y.water) })
        .ToListAsync();
      foreach (var rec in records)
      {
        var any =await this.Queryable().Where(x => x.CustomerId == rec.Key.CustomerId &&
                                x.Year == rec.Key.Year &&
                                x.Month == rec.Key.Month).AnyAsync();
  
        if (!any)
        {
          var customer =await this.getCustomer(rec.Key.CustomerId);
          var bill = new CustomerBill()
          {
            Amount = rec.water * price * ((customer==null)?1:customer.Discount),
             BillDate=now,
              Discount= ( ( customer == null ) ? 1 : customer.Discount ),
               CustomerId=rec.Key.CustomerId,
                CustomerName= customer?.Name,
                 Month=rec.Key.Month,
                  Price=price,
                   Status="待确认",
                    water=Math.Round(rec.water,0),
                     Year=rec.Key.Year,
                      
           
          };
          this.Insert(bill);
        }
      }

    }
  }
}



