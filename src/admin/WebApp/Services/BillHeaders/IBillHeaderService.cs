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
  /// File: IBillHeaderService.cs
  /// Purpose: Service interfaces. Services expose a service interface
  /// to which all inbound messages are sent. You can think of a service interface
  /// as a façade that exposes the business logic implemented in the application
  /// Created Date: 2/19/2021 8:29:52 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public interface IBillHeaderService : IService<BillHeader>
  {
    Task<IEnumerable<BillHeader>> GetByCustomerId(int customerid);
    Task<IEnumerable<BillDetail>> GetBillDetailsByBillId(int billid);

    Task ImportDataTable(DataTable datatable, string username = "");
    Task<Stream> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc");
    Task Delete(int[] id);
    Task GenerateBills(string month, int day);
    Task<BillHeader> GetAdnUpdateLasterWater(string billid, string month="", int customerId=0);
    Task Confirm(int[] id);

    Task SendToCustomer(int[] id, string path);
    Task<Stream> PrintBill(string path,IEnumerable<int> selectid);
  }
}