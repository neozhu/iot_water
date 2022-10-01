// <copyright file="MenuItemService.cs" company="neozhu/SmartCode-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>9/26/2018 4:20:25 PM </date>
// <summary>
//  根据需求定义实现具体的业务逻辑,通过依赖注入降低模块之间的耦合度
//   
//  
//  
// </summary>
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
  public class MenuItemService : Service<MenuItem>, IMenuItemService
  {
    private readonly IRepositoryAsync<MenuItem> _repository;
    private readonly IDataTableImportMappingService _mappingservice;
    public MenuItemService(IRepositoryAsync<MenuItem> repository, IDataTableImportMappingService mappingservice)
        : base(repository)
    {
      this._repository = repository;
      this._mappingservice = mappingservice;
    }

    public IEnumerable<MenuItem> GetByParentId(int parentid) => this._repository.GetByParentId(parentid);
    public IEnumerable<MenuItem> GetSubMenusByParentId(int parentid) => this._repository.GetSubMenusByParentId(parentid);



    private int getParentIdByTitle(string title)
    {
      var menuitemRepository = this._repository.GetRepository<MenuItem>();
      var menuitem = menuitemRepository.Queryable().Where(x => x.Title == title).FirstOrDefault();
      if (menuitem == null)
      {
        throw new Exception("not found ForeignKey:ParentId with " + title);
      }
      else
      {
        return menuitem.Id;
      }
    }

    public async Task ImportDataTableAsync(System.Data.DataTable datatable)
    {
      var mapping = await this._mappingservice.Queryable().Where(x => x.EntitySetName == "MenuItem" && ( ( x.IsEnabled == true ) || ( x.IsEnabled == false && !( x.DefaultValue == null || x.DefaultValue.Equals(string.Empty) ) ) )).ToListAsync();
      foreach (DataRow row in datatable.Rows)
      {
        var item = new MenuItem();
      
        var requiredfield = mapping.Where(x => x.IsRequired == true).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null && !row.IsNull(requiredfield) && row[requiredfield] != DBNull.Value && Convert.ToString(row[requiredfield]).Trim() != string.Empty)
        {
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contation = datatable.Columns.Contains(( field.SourceFieldName == null ? "" : field.SourceFieldName ));
            if (contation && !row.IsNull(field.SourceFieldName) && row[field.SourceFieldName] != DBNull.Value && row[field.SourceFieldName].ToString() != string.Empty)
            {
              var menuitemtype = item.GetType();
              var propertyInfo = menuitemtype.GetProperty(field.FieldName);
              //关联外键查询获取Id
              switch (field.FieldName)
              {
                case "ParentId":
                  var title = row[field.SourceFieldName].ToString();
                  var parentid = this.getParentIdByTitle(title);
                  propertyInfo.SetValue(item, Convert.ChangeType(parentid, propertyInfo.PropertyType), null);
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
              var menuitemtype = item.GetType();
              PropertyInfo propertyInfo = menuitemtype.GetProperty(field.FieldName);
              if (defval.ToLower() == "now" && propertyInfo.PropertyType == typeof(DateTime))
              {
                propertyInfo.SetValue(item, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
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

      var menuitems = await this.Query(new MenuItemQuery().Withfilter(filters)).Include(p => p.Parent).OrderBy(n => n.OrderBy(sort, order)).SelectAsync();

      var datarows = menuitems.Select(n => new
      {

        ParentTitle = ( n.Parent == null ? "" : n.Parent.Title ),
        Id = n.Id,
        Title = n.Title,
        Description = n.Description,
        Code = n.Code,
        Url = n.Url,
        Controller = n.Controller,
        Action = n.Action,
        IconCls = n.IconCls,
        IsEnabled = n.IsEnabled,
        ParentId = n.ParentId
      }).ToList();

      return await ExcelHelper.ExportExcelAsync(typeof(MenuItem), datarows);

    }

    public async Task<IEnumerable<MenuItem>> CreateWithControllerAsync()
    {
      var list = new List<MenuItem>();

      var asm = Assembly.GetExecutingAssembly();

      var controlleractionlist = asm.GetTypes()
              .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
              .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
              .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
              .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = x.GetCustomAttributes().Where(y => y.GetType() == typeof(RouteAttribute)).Select(rx=>( RouteAttribute )rx).FirstOrDefault() })
              .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
      var i = 1000;
      var filter = new string[] { "Index", "AddRole" };
      foreach (var item in controlleractionlist.Where(x => x.Attributes!=null).OrderBy(x=>x.Controller).ThenBy(x=>x.Attributes.Order))
      {
        var routename = ( item.Attributes as RouteAttribute ).Name;
        if (!string.IsNullOrEmpty(routename))
        {
          var menu = new MenuItem();
          menu.Code = ( i++ ).ToString("0000");
          menu.Description = routename;
          menu.Title = routename;
          menu.Url = "/" + item.Controller.Replace("Controller", "") + "/" + item.Action;
          menu.Action = item.Action;
          menu.Controller = item.Controller.Replace("Controller", "");
          menu.IsEnabled = true;
          if (!await this.Queryable().Where(x => x.Url == menu.Url).AnyAsync())
          {
            this.Insert(menu);

          }

          list.Add(menu);
        }
      }

      return list;
    }


    public async Task<IEnumerable<MenuItem>> ReBuildMenusAsync()
    {
      foreach (var item in this.Queryable().ToList())
      {
        this.Delete(item);
      }
      return await this.CreateWithControllerAsync();
    }
  }
}



