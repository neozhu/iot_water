using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using System.Threading.Tasks;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
using System.Data;
using System.IO;
namespace WebApp.Services
{
  /// <summary>
  /// File: ICustomerService.cs
  /// Purpose: Service interfaces. Services expose a service interface
  /// to which all inbound messages are sent. You can think of a service interface
  /// as a façade that exposes the business logic implemented in the application
  /// Created Date: 3/25/2020 9:58:04 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public interface ICustomerService : IService<Customer>
  {
    Task<IEnumerable<CustomerWaterRecord>> GetCustomerWaterRecordsByCustomerIdAsync(int customerid);
    Task<IEnumerable<CustomerWaterMeter>> GetWaterMetersByCustomerIdAsync(int customerid);
    Task<IEnumerable<CustomerWaterQuota>> GetWaterQuotasByCustomerIdAsync(int customerid);

    Task ImportDataTableAsync(DataTable datatable, string username = "");
    Task<Stream> ExportExcelAsync(string filterRules = "", string sort = "Id", string order = "asc");
    Task Delete(int[] id);

    Task CreateCustomerWaterMeters(CustomerWaterMeter[] items);
    Task UpdateCustomerWaterMeters(CustomerWaterMeter[] items);
    Task CreateCustomerWaterQuotas(int customerid, int year);
    Task EnableSelected(int[] id);
    Task DisableSelected(int[] id);
    Task ChangeWatePrice(decimal value);
  }
}