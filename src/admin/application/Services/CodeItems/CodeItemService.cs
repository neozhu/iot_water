using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LazyCache;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;


namespace WebApp.Services
{
  public class CodeItemService : Service<CodeItem>, ICodeItemService
  {
    private readonly IAppCache cache;
    private readonly IRepositoryAsync<CodeItem> _repository;
    private readonly IDataTableImportMappingService _mappingservice;
 
    public CodeItemService(
      IAppCache cache,
      IRepositoryAsync<CodeItem> repository,
      IDataTableImportMappingService mappingservice)
        : base(repository)
    {
      this._repository = repository;
      this._mappingservice = mappingservice;
      this.cache = cache;
    }





    public async Task ImportDataTableAsync(System.Data.DataTable datatable)
    {
      var mapping = await this._mappingservice.Queryable().Where(x => x.EntitySetName == "CodeItem" && ( ( x.IsEnabled == true ) || ( x.IsEnabled == false && !( x.DefaultValue == null || x.DefaultValue.Equals(string.Empty) ) ) )).ToListAsync();
      if (mapping == null || mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到CodeItem对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {
        var item = new CodeItem();
        var requiredfield = mapping.Where(x => x.IsRequired == true).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null && !row.IsNull(requiredfield) && row[requiredfield] != DBNull.Value && Convert.ToString(row[requiredfield]).Trim() != string.Empty)
        {
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName) && row[field.SourceFieldName] != DBNull.Value && row[field.SourceFieldName].ToString() != string.Empty)
            {
              var partnertype = item.GetType();
              var propertyInfo = partnertype.GetProperty(field.FieldName);
              var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
              var safeValue = ( row[field.SourceFieldName] == null ) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
              propertyInfo.SetValue(item, safeValue, null);
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var codeitemtype = item.GetType();
              var propertyInfo = codeitemtype.GetProperty(field.FieldName);
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
          var has = await this.Queryable().Where(x => x.CodeType == item.CodeType && x.Code == item.Code).AnyAsync();
          if (has)
          {
            //this.Queryable().Where(x => x.CodeType == item.CodeType && x.Code == item.Code).UpdateFromQuery(u => new CodeItem()
            //{
            //    Text = item.Text
            //});
          }
          else
          {
            this.Insert(item);
          }
        }

      }
    }

    public async Task<Stream> ExportExcelAsync(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);

      var codeitems = await this.Query(new CodeItemQuery().Withfilter(filters)).OrderBy(n => n.OrderBy(sort, order)).SelectAsync();

      var datarows = codeitems.Select(n => new { Id = n.Id, Code = n.Code, Text = n.Text, Description = n.Description, IsDisabled = n.IsDisabled }).ToList();

      return await ExcelHelper.ExportExcelAsync(typeof(CodeItem), datarows);

    }

    public async Task UpdateJavascriptAsync(string filename)
    {
      var sw = new StringWriter();
      var list = await this.GetCodeItemsAsync();
      foreach (var item in list.Select(x => new { CodeType = x.CodeType, Description = x.Description }).Distinct()
          )
      {
        var multiple = list.Where(x => x.CodeType == item.CodeType && x.Multiple == true).Any().ToString().ToLower();
        sw.WriteLine($"//-------{item.Description}---------//");
        var filtername = item.CodeType.ToLower() + "filtersource";
        var datasourcename = item.CodeType.ToLower() + "datasource";
        var codetype = item.CodeType.ToLower();
        var description = item.Description;
        sw.WriteLine($"var {filtername} = [{{ value: '', text: 'All'}}];");
        sw.WriteLine($"var {datasourcename} = [];");
        foreach (var data in list.Where(x => x.CodeType == item.CodeType))
        {
          sw.WriteLine($"{filtername}.push({{ value: '{data.Code}',text:'{data.Text}'  }});");
          sw.WriteLine($"{datasourcename}.push({{ value: '{data.Code}',text:'{data.Text}'  }});");
        }
        sw.WriteLine($"//for datagrid {item.CodeType} field  formatter");
        sw.WriteLine($"function {item.CodeType.ToLower()}formatter(value, row, index) {{ ");
        sw.WriteLine($"     let multiple = {multiple}; ");
        sw.WriteLine($"     if (value === null || value === '' || value === undefined) ");
        sw.WriteLine($"     {{ ");
        sw.WriteLine($"         return \"\";");
        sw.WriteLine($"     }} ");
        sw.WriteLine($"     if (multiple) {{ ");
        sw.WriteLine($"         let valarray = value.split(','); ");
        sw.WriteLine($"         let result = { datasourcename }.filter(item => valarray.includes(item.value));");
        sw.WriteLine($"         let textarray = result.map(x => x.text);");
        sw.WriteLine($"         if (textarray.length > 0)");
        sw.WriteLine($"             return textarray.join(\",\");");
        sw.WriteLine($"         else ");
        sw.WriteLine($"             return value;");
        sw.WriteLine($"      }} else {{ ");
        sw.WriteLine($"         let result = { datasourcename }.filter(x => x.value == value);");
        sw.WriteLine($"               if (result.length > 0)");
        sw.WriteLine($"                    return result[0].text;");
        sw.WriteLine($"               else");
        sw.WriteLine($"                    return value;");
        sw.WriteLine($"       }} ");
        sw.WriteLine($" }} ");

        sw.WriteLine($"//for datagrid   {item.CodeType}  field filter ");
        sw.WriteLine($"$.extend($.fn.datagrid.defaults.filters, {{");
        sw.WriteLine($"{ item.CodeType.ToLower() }filter: {{");
        sw.WriteLine($"     init: function(container, options) {{");
        sw.WriteLine($"        var input = $('<select class=\"easyui-combobox\" >').appendTo(container);");
        sw.WriteLine($"        var myoptions = {{");
        sw.WriteLine($"             panelHeight: 'auto',");
        sw.WriteLine($"             editable: false,");
        sw.WriteLine($"             data: {filtername} ,");
        sw.WriteLine($"             onChange: function () {{");
        sw.WriteLine($"                input.trigger('combobox.filter');");
        sw.WriteLine($"             }}");
        sw.WriteLine($"         }};");
        sw.WriteLine($"         $.extend(options, myoptions);");
        sw.WriteLine($"         input.combobox(options);");
        sw.WriteLine($"         input.combobox('textbox').bind('keydown', function (e) {{   ");
        sw.WriteLine($"            if (e.keyCode === 13) {{");
        sw.WriteLine($"              $(e.target).emulateTab();");
        sw.WriteLine($"            }}");
        sw.WriteLine($"          }});  ");
        sw.WriteLine($"         return input;");
        sw.WriteLine($"      }},");
        sw.WriteLine($"     destroy: function(target) {{");
        sw.WriteLine($"                  ");
        sw.WriteLine($"     }},");
        sw.WriteLine($"     getValue: function(target) {{");
        sw.WriteLine($"         return $(target).combobox('getValue');");
        sw.WriteLine($"     }},");
        sw.WriteLine($"     setValue: function(target, value) {{");
        sw.WriteLine($"         $(target).combobox('setValue', value);");
        sw.WriteLine($"     }},");
        sw.WriteLine($"     resize: function(target, width) {{");
        sw.WriteLine($"         $(target).combobox('resize', width);");
        sw.WriteLine($"     }}");
        sw.WriteLine($"   }}");
        sw.WriteLine($"}});");

        sw.WriteLine($"//for datagrid   { item.CodeType }   field  editor ");
        sw.WriteLine($"$.extend($.fn.datagrid.defaults.editors, {{");
        sw.WriteLine($"{item.CodeType.ToLower()}editor: {{");
        sw.WriteLine($"     init: function(container, options) {{");
        sw.WriteLine($"        var input = $('<input type=\"text\">').appendTo(container);");
        sw.WriteLine($"        var myoptions = {{");
        sw.WriteLine($"         panelHeight: 'auto',");
        sw.WriteLine($"         editable: false,");
        sw.WriteLine($"         data: {datasourcename},");
        sw.WriteLine($"         multiple: {multiple},");
        sw.WriteLine($"         valueField: 'value',");
        sw.WriteLine($"         textField: 'text'");
        sw.WriteLine($"     }};");
        sw.WriteLine($"    $.extend(options, myoptions);");
        sw.WriteLine($"           input.combobox(options);");
        sw.WriteLine($"         input.combobox('textbox').bind('keydown', function (e) {{   ");
        sw.WriteLine($"            if (e.keyCode === 13) {{");
        sw.WriteLine($"              $(e.target).emulateTab();");
        sw.WriteLine($"            }}");
        sw.WriteLine($"          }});  ");
        sw.WriteLine($"           return input;");
        sw.WriteLine($"       }},");
        sw.WriteLine($"     destroy: function(target) {{");
        sw.WriteLine($"         $(target).combobox('destroy');");
        sw.WriteLine($"        }},");
        sw.WriteLine($"     getValue: function(target) {{");
        sw.WriteLine($"        let opts = $(target).combobox('options');");
        sw.WriteLine($"        if (opts.multiple) {{");
        sw.WriteLine($"           return $(target).combobox('getValues').join(opts.separator);");
        sw.WriteLine($"         }} else {{");
        sw.WriteLine($"            return $(target).combobox('getValue');");
        sw.WriteLine($"         }}");
        sw.WriteLine($"        }},");
        sw.WriteLine($"     setValue: function(target, value) {{");
        sw.WriteLine($"         let opts = $(target).combobox('options');");
        sw.WriteLine($"         if (opts.multiple) {{");
        sw.WriteLine($"             if (value == '' || value == null) {{ ");
        sw.WriteLine($"                 $(target).combobox('clear'); ");
        sw.WriteLine($"              }} else {{ ");
        sw.WriteLine($"                  $(target).combobox('setValues', value.split(opts.separator));");
        sw.WriteLine($"               }}");
        sw.WriteLine($"          }}");
        sw.WriteLine($"          else {{");
        sw.WriteLine($"             $(target).combobox('setValue', value);");
        sw.WriteLine($"           }}");
        sw.WriteLine($"         }},");
        sw.WriteLine($"     resize: function(target, width) {{");
        sw.WriteLine($"         $(target).combobox('resize', width);");
        sw.WriteLine($"        }}");
        sw.WriteLine($"  }}  ");
        sw.WriteLine($"}});");


      }

      File.WriteAllText(filename, sw.ToString());
    }
    private async Task<IEnumerable<CodeItem>> GetCodeItemsAsync() => await this.Queryable().Where(x => x.IsDisabled == 0).OrderBy(x => x.CodeType).ThenBy(x => x.Code).ToListAsync();
    public async Task<string> Code2Text(string type, string code) {
      var items =await  this.cache.GetOrAddAsync(type,  async () =>
      {
        var list = await this.Queryable().Where(x => x.CodeType == type).ToListAsync();
        return list;
      });
      return items.Where(x=>x.Code==code).FirstOrDefault()?.Text;

      }
    public async Task<string> Text2Code(string type, string text) {
      var items = await this.cache.GetOrAddAsync(type, async () =>
      {
        var list = await this.Queryable().Where(x => x.CodeType == type).ToListAsync();
        return list;
      });
      return items.Where(x => x.Text == text).FirstOrDefault()?.Code;
    }
  }
}



