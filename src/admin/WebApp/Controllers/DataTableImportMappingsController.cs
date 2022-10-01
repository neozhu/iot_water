using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using Z.EntityFramework.Plus;
using TrackableEntities;
using System.Threading.Tasks;
using System.IO;

namespace WebApp.Controllers
{
  [Authorize]
  [RoutePrefix("DataTableImportMappings")]
  public class DataTableImportMappingsController : Controller
  {

    //Please RegisterType UnityConfig.cs
    //container.RegisterType<IRepositoryAsync<DataTableImportMapping>, Repository<DataTableImportMapping>>();
    //container.RegisterType<IDataTableImportMappingService, DataTableImportMappingService>();

    //private ImsDbContext db = new ImsDbContext();
    private readonly IDataTableImportMappingService _dataTableImportMappingService;
    private readonly IUnitOfWorkAsync _unitOfWork;

    public DataTableImportMappingsController(IDataTableImportMappingService dataTableImportMappingService, IUnitOfWorkAsync unitOfWork)
    {
      _dataTableImportMappingService = dataTableImportMappingService;
      _unitOfWork = unitOfWork;
    }

    // GET: DataTableImportMappings/Index
    [Route("Index", Name = "Excel导入导出配置", Order = 1)]
    public ActionResult Index()
    {

      //var datatableimportmappings  = _dataTableImportMappingService.Queryable().AsQueryable();
      //return View(datatableimportmappings  );
      return View();
    }

    // Get :DataTableImportMappings/PageList
    // For Index View Boostrap-Table load  data 
    [HttpGet]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "desc", string filterRules = "")
    {
      try
      {


        var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
        var datatableimportmappings = await this._dataTableImportMappingService
          .Query(new DataTableImportMappingQuery().Withfilter(filters))
          .OrderBy(n => n.OrderBy(sort, order).ThenBy(x=>x.LineNo))
          .SelectPageAsync(page, rows, out var totalCount);
        var datarows = datatableimportmappings.Select(n => new
        {
          Id = n.Id,
          EntitySetName = n.EntitySetName,
          DefaultValue = n.DefaultValue,
          FieldName = n.FieldName,
          IsRequired = n.IsRequired,
          TypeName = n.TypeName,
          SourceFieldName = n.SourceFieldName,
          IsEnabled = n.IsEnabled,
          IgnoredColumn = n.IgnoredColumn,
          n.LineNo,
          RegularExpression = n.RegularExpression
        }).ToList();
        var pagelist = new { total = totalCount, rows = datarows };
        return Json(pagelist, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e) {
        throw e;
        }
    }

    [HttpPost]
    public async Task<ActionResult> SaveData(DataTableImportMapping[] datatableimportmappings)
    {
      if (datatableimportmappings == null)
      {
        throw new ArgumentNullException(nameof(datatableimportmappings));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in datatableimportmappings)
          {
            this._dataTableImportMappingService.ApplyChanges(item);
          }
          var result = await this._unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
      }
      else
      {
        var modelStateErrors = string.Join(",", ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
      }

      
    }


    [HttpGet]
    public async Task<ActionResult> GetAllEntites()
    {
      var list =await _dataTableImportMappingService.GetAssemblyEntitiesAsync();
      var rows = list.Select(x => new { Name = x.EntitySetName, Value = x.EntitySetName }).Distinct();
      return Json(rows, JsonRequestBehavior.AllowGet);

    }
    [HttpPost]
    public async Task<ActionResult> Generate(string entityname)
    {
      await _dataTableImportMappingService.GenerateByEnityNameAsync(entityname);
      await _unitOfWork.SaveChangesAsync();
      return Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }
    public async Task<ActionResult> CreateExcelTemplate(string entityname)
    {

      var count = await this._dataTableImportMappingService.Queryable().Where(x => x.EntitySetName == entityname && x.IsEnabled == true).AnyAsync();
      if (count)
      {
        var filename = Server.MapPath("~/ExcelTemplate/" + entityname + ".xlsx");
       await  _dataTableImportMappingService.CreateExcelTemplateAsync(entityname, filename);

        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      else
      {
        return Json(new { success = false, message = "没有找到[" + entityname + "]配置信息请,先执行【生成】mapping关系" }, JsonRequestBehavior.AllowGet);
      }
    }

    // GET: DataTableImportMappings/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var dataTableImportMapping = _dataTableImportMappingService.Find(id);
      if (dataTableImportMapping == null)
      {
        return HttpNotFound();
      }
      return View(dataTableImportMapping);
    }


    // GET: DataTableImportMappings/Create
    public ActionResult Create()
    {
      var dataTableImportMapping = new DataTableImportMapping();
      //set default value
      return View(dataTableImportMapping);
    }

    // POST: DataTableImportMappings/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    //[ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,EntitySetName,FieldName,TypeName,SourceFieldName,DefaultValue,IsEnabled,RegularExpression")] DataTableImportMapping dataTableImportMapping)
    {
      if (ModelState.IsValid)
      {
        _dataTableImportMappingService.Insert(dataTableImportMapping);
        _unitOfWork.SaveChanges();
        if (Request.IsAjaxRequest())
        {
          return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        DisplaySuccessMessage("Has append a DataTableImportMapping record");
        return RedirectToAction("Index");
      }

      if (Request.IsAjaxRequest())
      {
        var modelStateErrors = string.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
      }
      DisplayErrorMessage();
      return View(dataTableImportMapping);
    }

    // GET: DataTableImportMappings/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var dataTableImportMapping = _dataTableImportMappingService.Find(id);
      if (dataTableImportMapping == null)
      {
        return HttpNotFound();
      }
      return View(dataTableImportMapping);
    }

    // POST: DataTableImportMappings/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    //[ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,EntitySetName,FieldName,TypeName,SourceFieldName,DefaultValue,IsEnabled,RegularExpression")] DataTableImportMapping dataTableImportMapping)
    {
      if (ModelState.IsValid)
      {
        dataTableImportMapping.TrackingState = TrackingState.Modified;
        _dataTableImportMappingService.Update(dataTableImportMapping);

        _unitOfWork.SaveChanges();
        if (Request.IsAjaxRequest())
        {
          return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        DisplaySuccessMessage("Has update a DataTableImportMapping record");
        return RedirectToAction("Index");
      }
      if (Request.IsAjaxRequest())
      {
        var modelStateErrors = string.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
      }
      DisplayErrorMessage();
      return View(dataTableImportMapping);
    }

    // GET: DataTableImportMappings/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var dataTableImportMapping = _dataTableImportMappingService.Find(id);
      if (dataTableImportMapping == null)
      {
        return HttpNotFound();
      }
      return View(dataTableImportMapping);
    }

    // POST: DataTableImportMappings/Delete/5
    [HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      var dataTableImportMapping = _dataTableImportMappingService.Find(id);
      _dataTableImportMappingService.Delete(dataTableImportMapping);
      _unitOfWork.SaveChanges();
      if (Request.IsAjaxRequest())
      {
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      DisplaySuccessMessage("Has delete a DataTableImportMapping record");
      return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<ActionResult> ImportConfig()
    {
      if (Request.Files.Count > 0)
      {
        for (var i = 0; i < this.Request.Files.Count; i++)
        {
          var label = Request.Form["label"];
          var file = Request.Files[i];
          if (file != null && file.ContentLength > 0)
          {
            var filename = file.FileName;
            var contenttype = file.ContentType;
            var size = file.ContentLength;
            var ext = Path.GetExtension(filename);

            var folder = this.Server.MapPath("~/UploadFiles");
            if (!Directory.Exists(folder))
            {
              Directory.CreateDirectory(folder);
            }
            var virtualPath = Path.Combine(folder, filename);
            // 文件系统不能使用虚拟路径
            //string path = this.Server.MapPath(virtualPath);
           
            var datatable = await NPOIHelper.GetDataTableFromExcelAsync(file.InputStream, ext);
            await this._dataTableImportMappingService.ImportDataTableAsync(datatable);
            await this._unitOfWork.SaveChangesAsync();
            file.InputStream.Position = 0;
            file.SaveAs(virtualPath);
            
            return Content(filename);
          }
        }
        
      }

      return Content(null);
    }
    //删除选中的记录
    [HttpPost]
    public async Task<JsonResult> DeleteChecked(int[] id)
    {
      if (id == null)
      {
        throw new ArgumentNullException(nameof(id));
      }
      try
      {
        this._dataTableImportMappingService.Delete(id);
        await this._unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
        return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }

    [HttpDelete]
    public JsonResult Revert()
    {
      var req = Request.InputStream;
      var filename = new StreamReader(req).ReadToEnd();
      if (!string.IsNullOrEmpty(filename))
      {
        var folder = this.Server.MapPath("~/UploadFiles");
        var path = Path.Combine(folder, filename);
        if (System.IO.File.Exists(path))
        {
          System.IO.File.Delete(path);
        }
      }
      return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public async Task<ActionResult> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var fileName = "excelconfiguration_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this._dataTableImportMappingService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }

    private void DisplaySuccessMessage(string msgText)
    {
      TempData["SuccessMessage"] = msgText;
    }

    private void DisplayErrorMessage()
    {
      TempData["ErrorMessage"] = "Save changes was unsuccessful.";
    }


  }
}
