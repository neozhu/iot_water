// <copyright file="MenuItemQuery.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>9/26/2018 4:20:16 PM </date>
// <summary>
// 配合 easyui datagrid filter 组件使用,实现对datagrid 所有字段筛选功能
// 也可以对特定的业务逻辑查询进行封装
//  
//  
// </summary>

using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Web.WebPages;
using Repository.Pattern.Ef6;
using WebApp.Models;


namespace WebApp.Repositories
{
  public class MenuItemQuery : QueryObject<MenuItem>
  {
    public MenuItemQuery WithAnySearch(string search)
    {
      if (!string.IsNullOrEmpty(search))
        this.And(x => x.Id.ToString().Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Code.Contains(search) || x.Url.Contains(search) || x.Controller.Contains(search) || x.Action.Contains(search) || x.IconCls.Contains(search) || x.ParentId.ToString().Contains(search));
      return this;
    }


    public MenuItemQuery Withfilter(IEnumerable<filterRule> filters)
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




          if (rule.field == "Title" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Title.Contains(rule.value));
          }





          if (rule.field == "Description" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Description.Contains(rule.value));
          }





          if (rule.field == "Code" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Code.Contains(rule.value));
          }





          if (rule.field == "Url" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Url.Contains(rule.value));
          }





          if (rule.field == "Controller" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Controller.Contains(rule.value));
          }





          if (rule.field == "Action" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Action.Contains(rule.value));
          }





          if (rule.field == "IconCls" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.IconCls.Contains(rule.value));
          }









          if (rule.field == "IsEnabled" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
          {
            var boolval = Convert.ToBoolean(rule.value);
            this.And(x => x.IsEnabled == boolval);
          }


          if (rule.field == "ParentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            this.And(x => x.ParentId == val);
               
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



    public MenuItemQuery ByParentIdWithfilter(int parentid, IEnumerable<filterRule> filters)
    {
      this.And(x => x.ParentId == parentid);

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




          if (rule.field == "Title" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Title.Contains(rule.value));
          }





          if (rule.field == "Description" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Description.Contains(rule.value));
          }





          if (rule.field == "Code" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Code.Contains(rule.value));
          }





          if (rule.field == "Url" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Url.Contains(rule.value));
          }





          if (rule.field == "Controller" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Controller.Contains(rule.value));
          }





          if (rule.field == "Action" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.Action.Contains(rule.value));
          }





          if (rule.field == "IconCls" && !string.IsNullOrEmpty(rule.value))
          {
            this.And(x => x.IconCls.Contains(rule.value));
          }









          if (rule.field == "IsEnabled" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
          {
            var boolval = Convert.ToBoolean(rule.value);
            this.And(x => x.IsEnabled == boolval);
          }


          if (rule.field == "ParentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                this.And(x => x.ParentId == val);
                break;
              case "notequal":
                this.And(x => x.ParentId != val);
                break;
              case "less":
                this.And(x => x.ParentId < val);
                break;
              case "lessorequal":
                this.And(x => x.ParentId <= val);
                break;
              case "greater":
                this.And(x => x.ParentId > val);
                break;
              case "greaterorequal":
                this.And(x => x.ParentId >= val);
                break;
              default:
                this.And(x => x.ParentId == val);
                break;
            }
          }




        }
      }
      return this;
    }

  }
}



