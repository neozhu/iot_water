     
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public interface ICodeItemService:IService<CodeItem>
    {


    Task<string> Code2Text(string type, string code);
    Task<string> Text2Code(string type, string text);
    Task UpdateJavascriptAsync(string filename);

        Task ImportDataTableAsync(DataTable datatable);
		Task<Stream> ExportExcelAsync( string filterRules = "",string sort = "Id", string order = "asc");
	}
}