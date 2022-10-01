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
  /// File: CustomerWaterRecordQuery.cs
  /// Purpose: easyui datagrid filter query 
  /// Created Date: 3/25/2020 10:40:48 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class CustomerWaterRecordQuery : QueryObject<CustomerWaterRecord>
  {
    public CustomerWaterRecordQuery Withfilter(IEnumerable<filterRule> filters)
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
          if (rule.field == "meterId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.meterId.Contains(rule.value));
          }
          if (rule.field == "previousDate" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.previousDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.previousDate) <= 0);
            }
          }
          if (rule.field == "previousWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.previousWater == val);
                break;
              case "notequal":
                And(x => x.previousWater != val);
                break;
              case "less":
                And(x => x.previousWater < val);
                break;
              case "lessorequal":
                And(x => x.previousWater <= val);
                break;
              case "greater":
                And(x => x.previousWater > val);
                break;
              case "greaterorequal":
                And(x => x.previousWater >= val);
                break;
              default:
                And(x => x.previousWater == val);
                break;
            }
          }
          if (rule.field == "lastWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.lastWater == val);
                break;
              case "notequal":
                And(x => x.lastWater != val);
                break;
              case "less":
                And(x => x.lastWater < val);
                break;
              case "lessorequal":
                And(x => x.lastWater <= val);
                break;
              case "greater":
                And(x => x.lastWater > val);
                break;
              case "greaterorequal":
                And(x => x.lastWater >= val);
                break;
              default:
                And(x => x.lastWater == val);
                break;
            }
          }
          if (rule.field == "water" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.water == val);
                break;
              case "notequal":
                And(x => x.water != val);
                break;
              case "less":
                And(x => x.water < val);
                break;
              case "lessorequal":
                And(x => x.water <= val);
                break;
              case "greater":
                And(x => x.water > val);
                break;
              case "greaterorequal":
                And(x => x.water >= val);
                break;
              default:
                And(x => x.water == val);
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
          if (rule.field == "User" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.User.Contains(rule.value));
          }
          if (rule.field == "Type" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Type.Contains(rule.value));
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
    public CustomerWaterRecordQuery ByCustomerIdWithfilter(int customerid, IEnumerable<filterRule> filters)
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
          if (rule.field == "meterId" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "equal")
            {
              And(x => x.meterId == rule.value);
            }
            else
            {
              And(x => x.meterId.Contains(rule.value));
            }
          }
          if (rule.field == "previousDate" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.previousDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.previousDate) <= 0);
            }
          }
          if (rule.field == "previousWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.previousWater == val);
                break;
              case "notequal":
                And(x => x.previousWater != val);
                break;
              case "less":
                And(x => x.previousWater < val);
                break;
              case "lessorequal":
                And(x => x.previousWater <= val);
                break;
              case "greater":
                And(x => x.previousWater > val);
                break;
              case "greaterorequal":
                And(x => x.previousWater >= val);
                break;
              default:
                And(x => x.previousWater == val);
                break;
            }
          }
          if (rule.field == "lastWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.lastWater == val);
                break;
              case "notequal":
                And(x => x.lastWater != val);
                break;
              case "less":
                And(x => x.lastWater < val);
                break;
              case "lessorequal":
                And(x => x.lastWater <= val);
                break;
              case "greater":
                And(x => x.lastWater > val);
                break;
              case "greaterorequal":
                And(x => x.lastWater >= val);
                break;
              default:
                And(x => x.lastWater == val);
                break;
            }
          }
          if (rule.field == "water" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.water == val);
                break;
              case "notequal":
                And(x => x.water != val);
                break;
              case "less":
                And(x => x.water < val);
                break;
              case "lessorequal":
                And(x => x.water <= val);
                break;
              case "greater":
                And(x => x.water > val);
                break;
              case "greaterorequal":
                And(x => x.water >= val);
                break;
              default:
                And(x => x.water == val);
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
          if (rule.field == "User" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "equal")
            {
              And(x => x.User == rule.value);
            }
            else
            {
              And(x => x.User.Contains(rule.value));
            }
          }
          if (rule.field == "Type" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "equal")
            {
              And(x => x.Type == rule.value);
            }
            else
            {
              And(x => x.Type.Contains(rule.value));
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
