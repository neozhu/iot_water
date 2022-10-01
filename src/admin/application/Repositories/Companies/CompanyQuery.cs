/// <summary>
/// File: CompanyQuery.cs
/// Purpose: easyui datagrid filter query 
/// Date: 2018/11/9 15:32:01
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 neo.zhu and Contributors
/// License: GNU General Public License v3.See license.txt
/// </summary>
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Web.WebPages;
using Repository.Pattern.Ef6;
using WebApp.Models;

namespace WebApp.Repositories
{
  public class CompanyQuery : QueryObject<Company>
  {
    public CompanyQuery WithfilterQ(string q)
    {
      if (!string.IsNullOrEmpty(q))
      {

        this.And(x => x.Code.Contains(q) || x.Name.Contains(q));

      }
      return this;
    }
    public CompanyQuery Withfilter(IEnumerable<filterRule> filters)
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
                this.And(x => x.Id == val);
                break;
              case "notequal":
                this.And(x => x.Id != val);
                break;
              case "less":
                this.And(x => x.Id < val);
                break;
              case "lessorequal":
                this.And(x => x.Id <= val);
                break;
              case "greater":
                this.And(x => x.Id > val);
                break;
              case "greaterorequal":
                this.And(x => x.Id >= val);
                break;
              default:
                this.And(x => x.Id == val);
                break;
            }
          }




          if (rule.field == "Name" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Name.Contains(rule.value));
          }





          if (rule.field == "Code" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Code.Contains(rule.value));
          }





          if (rule.field == "Address" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Address.Contains(rule.value));
          }





          if (rule.field == "Contect" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Contect.Contains(rule.value));
          }





          if (rule.field == "PhoneNumber" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.PhoneNumber.Contains(rule.value));
          }








          if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              this.And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
              this.And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
            }
          }





          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              this.And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              this.And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }


          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.CreatedBy.Contains(rule.value));
          }








          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              this.And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              this.And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }


          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.LastModifiedBy.Contains(rule.value));
          }






        }
      }
      return this;
    }



  }
}
