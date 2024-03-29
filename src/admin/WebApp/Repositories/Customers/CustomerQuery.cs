﻿using System;
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
  /// File: CustomerQuery.cs
  /// Purpose: easyui datagrid filter query 
  /// Created Date: 3/25/2020 9:57:54 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class CustomerQuery : QueryObject<Customer>
  {
    public CustomerQuery Withfilter(IEnumerable<filterRule> filters)
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
          if (rule.field == "Name" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Name.Contains(rule.value));
          }
          if (rule.field == "Level" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Level.Contains(rule.value));
          }
          if (rule.field == "Status" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Status.Contains(rule.value));
          }
          if (rule.field == "Code" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Code.Contains(rule.value));
          }
          if (rule.field == "Category" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Category.Contains(rule.value));
          }
          if (rule.field == "Dept" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Dept.Contains(rule.value));
          }
          if (rule.field == "Contact" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Contact.Contains(rule.value));
          }
          if (rule.field == "ContactInfo" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ContactInfo.Contains(rule.value));
          }
          if (rule.field == "MobilePhone" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.MobilePhone.Contains(rule.value));
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
          if (rule.field == "Threshold" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Threshold == val);
                break;
              case "notequal":
                And(x => x.Threshold != val);
                break;
              case "less":
                And(x => x.Threshold < val);
                break;
              case "lessorequal":
                And(x => x.Threshold <= val);
                break;
              case "greater":
                And(x => x.Threshold > val);
                break;
              case "greaterorequal":
                And(x => x.Threshold >= val);
                break;
              default:
                And(x => x.Threshold == val);
                break;
            }
          }
          if (rule.field == "IsFee" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
          {
            var boolval = Convert.ToBoolean(rule.value);
            And(x => x.IsFee == boolval);
          }
          if (rule.field == "Discount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Discount == val);
                break;
              case "notequal":
                And(x => x.Discount != val);
                break;
              case "less":
                And(x => x.Discount < val);
                break;
              case "lessorequal":
                And(x => x.Discount <= val);
                break;
              case "greater":
                And(x => x.Discount > val);
                break;
              case "greaterorequal":
                And(x => x.Discount >= val);
                break;
              default:
                And(x => x.Discount == val);
                break;
            }
          }
          if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
            }
          }
          if (rule.field == "Remark" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Remark.Contains(rule.value));
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
  }
}
