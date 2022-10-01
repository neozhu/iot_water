using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.SqlServer;
using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using System.Web.WebPages;
using WebApp.Models;

namespace WebApp.Repositories
{
  /// <summary>
  /// File: CustomerWaterQuotaQuery.cs
  /// Purpose: easyui datagrid filter query 
  /// Created Date: 3/25/2020 10:34:06 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class CustomerWaterQuotaQuery : QueryObject<CustomerWaterQuota>
  {
    public CustomerWaterQuotaQuery WithLastfilter(IEnumerable<filterRule> filters)
    {
      var year = DateTime.Now.Year;
      var month = DateTime.Now.Month;
      this.And(x => (x.Year < year)  || (x.Year >=year  && x.Month <= month));
      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CustomerId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.CustomerId == val);
                break;
              case "notequal":
                And(x => x.CustomerId != val);
                break;
              case "less":
                And(x => x.CustomerId < val);
                break;
              case "lessorequal":
                And(x => x.CustomerId <= val);
                break;
              case "greater":
                And(x => x.CustomerId > val);
                break;
              case "greaterorequal":
                And(x => x.CustomerId >= val);
                break;
              default:
                And(x => x.CustomerId == val);
                break;
            }
          }
          if (rule.field == "Year" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Year == val);
                break;
              case "notequal":
                And(x => x.Year != val);
                break;
              case "less":
                And(x => x.Year < val);
                break;
              case "lessorequal":
                And(x => x.Year <= val);
                break;
              case "greater":
                And(x => x.Year > val);
                break;
              case "greaterorequal":
                And(x => x.Year >= val);
                break;
              default:
                And(x => x.Year == val);
                break;
            }
          }
          if (rule.field == "Month" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Month == val);
                break;
              case "notequal":
                And(x => x.Month != val);
                break;
              case "less":
                And(x => x.Month < val);
                break;
              case "lessorequal":
                And(x => x.Month <= val);
                break;
              case "greater":
                And(x => x.Month > val);
                break;
              case "greaterorequal":
                And(x => x.Month >= val);
                break;
              default:
                And(x => x.Month == val);
                break;
            }
          }
          if (rule.field == "Quota" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Quota == val);
                break;
              case "notequal":
                And(x => x.Quota != val);
                break;
              case "less":
                And(x => x.Quota < val);
                break;
              case "lessorequal":
                And(x => x.Quota <= val);
                break;
              case "greater":
                And(x => x.Quota > val);
                break;
              case "greaterorequal":
                And(x => x.Quota >= val);
                break;
              default:
                And(x => x.Quota == val);
                break;
            }
          }
          if (rule.field == "Water" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Water == val);
                break;
              case "notequal":
                And(x => x.Water != val);
                break;
              case "less":
                And(x => x.Water < val);
                break;
              case "lessorequal":
                And(x => x.Water <= val);
                break;
              case "greater":
                And(x => x.Water > val);
                break;
              case "greaterorequal":
                And(x => x.Water >= val);
                break;
              default:
                And(x => x.Water == val);
                break;
            }
          }
          if (rule.field == "ForecastWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.ForecastWater == val);
                break;
              case "notequal":
                And(x => x.ForecastWater != val);
                break;
              case "less":
                And(x => x.ForecastWater < val);
                break;
              case "lessorequal":
                And(x => x.ForecastWater <= val);
                break;
              case "greater":
                And(x => x.ForecastWater > val);
                break;
              case "greaterorequal":
                And(x => x.ForecastWater >= val);
                break;
              default:
                And(x => x.ForecastWater == val);
                break;
            }
          }
          if (rule.field == "RecordDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RecordDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RecordDate) <= 0);
            }
          }
          if (rule.field == "CustomerName" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CustomerName.Contains(rule.value));
          }
          if (rule.field == "IsDelete" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
          {
            var boolval = Convert.ToBoolean(rule.value);
            And(x => x.IsDelete == boolval);
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
    public CustomerWaterQuotaQuery Withfilter(IEnumerable<filterRule> filters)
    {
      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CustomerId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.CustomerId == val);
                break;
              case "notequal":
                And(x => x.CustomerId != val);
                break;
              case "less":
                And(x => x.CustomerId < val);
                break;
              case "lessorequal":
                And(x => x.CustomerId <= val);
                break;
              case "greater":
                And(x => x.CustomerId > val);
                break;
              case "greaterorequal":
                And(x => x.CustomerId >= val);
                break;
              default:
                And(x => x.CustomerId == val);
                break;
            }
          }
          if (rule.field == "Year" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Year == val);
                break;
              case "notequal":
                And(x => x.Year != val);
                break;
              case "less":
                And(x => x.Year < val);
                break;
              case "lessorequal":
                And(x => x.Year <= val);
                break;
              case "greater":
                And(x => x.Year > val);
                break;
              case "greaterorequal":
                And(x => x.Year >= val);
                break;
              default:
                And(x => x.Year == val);
                break;
            }
          }
          if (rule.field == "Month" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Month == val);
                break;
              case "notequal":
                And(x => x.Month != val);
                break;
              case "less":
                And(x => x.Month < val);
                break;
              case "lessorequal":
                And(x => x.Month <= val);
                break;
              case "greater":
                And(x => x.Month > val);
                break;
              case "greaterorequal":
                And(x => x.Month >= val);
                break;
              default:
                And(x => x.Month == val);
                break;
            }
          }
          if (rule.field == "Quota" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Quota == val);
                break;
              case "notequal":
                And(x => x.Quota != val);
                break;
              case "less":
                And(x => x.Quota < val);
                break;
              case "lessorequal":
                And(x => x.Quota <= val);
                break;
              case "greater":
                And(x => x.Quota > val);
                break;
              case "greaterorequal":
                And(x => x.Quota >= val);
                break;
              default:
                And(x => x.Quota == val);
                break;
            }
          }
          if (rule.field == "Water" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Water == val);
                break;
              case "notequal":
                And(x => x.Water != val);
                break;
              case "less":
                And(x => x.Water < val);
                break;
              case "lessorequal":
                And(x => x.Water <= val);
                break;
              case "greater":
                And(x => x.Water > val);
                break;
              case "greaterorequal":
                And(x => x.Water >= val);
                break;
              default:
                And(x => x.Water == val);
                break;
            }
          }
          if (rule.field == "ForecastWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.ForecastWater == val);
                break;
              case "notequal":
                And(x => x.ForecastWater != val);
                break;
              case "less":
                And(x => x.ForecastWater < val);
                break;
              case "lessorequal":
                And(x => x.ForecastWater <= val);
                break;
              case "greater":
                And(x => x.ForecastWater > val);
                break;
              case "greaterorequal":
                And(x => x.ForecastWater >= val);
                break;
              default:
                And(x => x.ForecastWater == val);
                break;
            }
          }
          if (rule.field == "RecordDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RecordDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RecordDate) <= 0);
            }
          }
          if (rule.field == "CustomerName" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CustomerName.Contains(rule.value));
          }
          if (rule.field == "IsDelete" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
          {
            var boolval = Convert.ToBoolean(rule.value);
            And(x => x.IsDelete == boolval);
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
    public CustomerWaterQuotaQuery ByCustomerIdWithfilter(int customerid, IEnumerable<filterRule> filters)
    {
      And(x => x.CustomerId == customerid);
      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CustomerId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.CustomerId == val);
                break;
              case "notequal":
                And(x => x.CustomerId != val);
                break;
              case "less":
                And(x => x.CustomerId < val);
                break;
              case "lessorequal":
                And(x => x.CustomerId <= val);
                break;
              case "greater":
                And(x => x.CustomerId > val);
                break;
              case "greaterorequal":
                And(x => x.CustomerId >= val);
                break;
              default:
                And(x => x.CustomerId == val);
                break;
            }
          }
          if (rule.field == "Year" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Year == val);
                break;
              case "notequal":
                And(x => x.Year != val);
                break;
              case "less":
                And(x => x.Year < val);
                break;
              case "lessorequal":
                And(x => x.Year <= val);
                break;
              case "greater":
                And(x => x.Year > val);
                break;
              case "greaterorequal":
                And(x => x.Year >= val);
                break;
              default:
                And(x => x.Year == val);
                break;
            }
          }
          if (rule.field == "Month" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Month == val);
                break;
              case "notequal":
                And(x => x.Month != val);
                break;
              case "less":
                And(x => x.Month < val);
                break;
              case "lessorequal":
                And(x => x.Month <= val);
                break;
              case "greater":
                And(x => x.Month > val);
                break;
              case "greaterorequal":
                And(x => x.Month >= val);
                break;
              default:
                And(x => x.Month == val);
                break;
            }
          }
          if (rule.field == "Quota" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Quota == val);
                break;
              case "notequal":
                And(x => x.Quota != val);
                break;
              case "less":
                And(x => x.Quota < val);
                break;
              case "lessorequal":
                And(x => x.Quota <= val);
                break;
              case "greater":
                And(x => x.Quota > val);
                break;
              case "greaterorequal":
                And(x => x.Quota >= val);
                break;
              default:
                And(x => x.Quota == val);
                break;
            }
          }
          if (rule.field == "Water" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Water == val);
                break;
              case "notequal":
                And(x => x.Water != val);
                break;
              case "less":
                And(x => x.Water < val);
                break;
              case "lessorequal":
                And(x => x.Water <= val);
                break;
              case "greater":
                And(x => x.Water > val);
                break;
              case "greaterorequal":
                And(x => x.Water >= val);
                break;
              default:
                And(x => x.Water == val);
                break;
            }
          }
          if (rule.field == "ForecastWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.ForecastWater == val);
                break;
              case "notequal":
                And(x => x.ForecastWater != val);
                break;
              case "less":
                And(x => x.ForecastWater < val);
                break;
              case "lessorequal":
                And(x => x.ForecastWater <= val);
                break;
              case "greater":
                And(x => x.ForecastWater > val);
                break;
              case "greaterorequal":
                And(x => x.ForecastWater >= val);
                break;
              default:
                And(x => x.ForecastWater == val);
                break;
            }
          }
          if (rule.field == "RecordDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RecordDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RecordDate) <= 0);
            }
          }
          if (rule.field == "CustomerName" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "equal")
            {
              And(x => x.CustomerName == rule.value);
            }
            else
            {
              And(x => x.CustomerName.Contains(rule.value));
            }
          }
          if (rule.field == "IsDelete" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
          {
            var boolval = Convert.ToBoolean(rule.value);
            And(x => x.IsDelete == boolval);
          }
        }
      }
      return this;
    }
  }
}
