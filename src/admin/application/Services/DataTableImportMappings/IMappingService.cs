



using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
  public interface IDataTableImportMappingService : IService<DataTableImportMapping>
  {

    Task<IEnumerable<EntityInfo>> GetAssemblyEntitiesAsync();
    Task GenerateByEnityNameAsync(string entityName);

    Task<DataTableImportMapping> FindMappingAsync(string entitySetName, string sourceFieldName);
    Task CreateExcelTemplateAsync(string entityname, string filename);
    Task ImportDataTableAsync(DataTable datatable);
    void Delete(int[] id);
    Task<Stream> ExportExcelAsync(string filterRules = "", string sort = "Id", string order = "asc");
  }
}