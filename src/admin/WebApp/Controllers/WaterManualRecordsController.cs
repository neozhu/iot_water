using System;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using System.Linq.Dynamic.Core;
using Z.EntityFramework.Plus;
using TrackableEntities;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
namespace WebApp.Controllers
{
  /// <summary>
  /// File: WaterManualRecordsController.cs
  /// Purpose:收费管理/抄表记录
  /// Created Date: 3/6/2021 5:29:07 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<WaterManualRecord>, Repository<WaterManualRecord>>();
  ///    container.RegisterType<IWaterManualRecordService, WaterManualRecordService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("WaterManualRecords")]
  public class WaterManualRecordsController : Controller
  {
    private readonly IWaterManualRecordService waterManualRecordService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly ICodeItemService codeItemService;
    private readonly NLog.ILogger logger;
    public WaterManualRecordsController(
      ICodeItemService codeItemService,
          IWaterManualRecordService waterManualRecordService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.codeItemService = codeItemService;
      this.waterManualRecordService = waterManualRecordService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
    //GET: WaterManualRecords/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "抄表记录", Order = 1)]
    public ActionResult Index()
    {
      var list = this.codeItemService.Queryable()
         .Where(x => x.CodeType == "endday")
         .Select(x => new SelectListItem
         {
           Text = x.Text,
           Value = x.Code
         })
         .ToList();
      ViewBag.endday = list;
      return View();
    }

    //生成抄表模板(机械表)
    public async Task<ActionResult> DownloadTemplate() {
      var fileName = DateTime.Now.Month + "月抄表模板" + ".xlsx";
      var stream = await this.waterManualRecordService.CreateImportTemplate();
      this.logger.Info($"下载模板,文件名:{fileName}");
      return File(stream, "application/vnd.ms-excel", fileName);

    }

    //Get :WaterManualRecords/GetData
    //For Index View datagrid datasource url

    [HttpPost]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = PredicateBuilder.From<WaterManualRecord>(filterRules);
      var pagerows = ( await this.waterManualRecordService
                           .Query(filters)
                         .OrderBy(n => n.OrderBy($"{sort} {order}"))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         meterId = n.meterId,
                                         Name = n.Name,
                                         LineNo = n.LineNo,
                                         Name1 = n.Name1,
                                         Address=n.Address,
                                         Water = n.Water,
                                         RecordDate = n.RecordDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         Remark = n.Remark
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> AcceptChanges(WaterManualRecord[] watermanualrecords)
    {
      try
      {
        this.waterManualRecordService.ApplyChanges(watermanualrecords);
        var result = await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }

    //GET: WaterManualRecords/Details/:id
    public ActionResult Details(int id)
    {

      var waterManualRecord = this.waterManualRecordService.Find(id);
      if (waterManualRecord == null)
      {
        return HttpNotFound();
      }
      return View(waterManualRecord);
    }
    //GET: WaterManualRecords/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var waterManualRecord = await this.waterManualRecordService.FindAsync(id);
      return Json(waterManualRecord, JsonRequestBehavior.AllowGet);
    }
    //GET: WaterManualRecords/Create
    public ActionResult Create()
    {
      var waterManualRecord = new WaterManualRecord();
      //set default value
      return View(waterManualRecord);
    }
    //POST: WaterManualRecords/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(WaterManualRecord waterManualRecord)
    {
      if (ModelState.IsValid)
      {
        try
        {
          this.waterManualRecordService.Insert(waterManualRecord);
          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
        }
        //DisplaySuccessMessage("Has update a waterManualRecord record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(waterManualRecord);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var waterManualRecord = await Task.Run(() =>
      {
        return new WaterManualRecord();
      });
      return Json(waterManualRecord, JsonRequestBehavior.AllowGet);
    }


    //GET: WaterManualRecords/Edit/:id
    public ActionResult Edit(int id)
    {
      var waterManualRecord = this.waterManualRecordService.Find(id);
      if (waterManualRecord == null)
      {
        return HttpNotFound();
      }
      return View(waterManualRecord);
    }
    //POST: WaterManualRecords/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(WaterManualRecord waterManualRecord)
    {
      if (ModelState.IsValid)
      {
        waterManualRecord.TrackingState = TrackingState.Modified;
        try
        {
          this.waterManualRecordService.Update(waterManualRecord);

          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
        }

        //DisplaySuccessMessage("Has update a WaterManualRecord record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(waterManualRecord);
    }
    //删除当前记录
    //GET: WaterManualRecords/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.waterManualRecordService.Delete(new int[] { id });
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }




    //删除选中的记录
    [HttpPost]
    public async Task<JsonResult> DeleteChecked(int[] id)
    {
      try
      {
        await this.waterManualRecordService.Delete(id);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }
    //导出Excel
    [HttpPost]
    public async Task<ActionResult> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var fileName = "watermanualrecords_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.waterManualRecordService.ExportExcel(filterRules, sort, order);
      this.logger.Info($"导出完成,文件名:{fileName}");
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    //导入数据
    [HttpPost]
    public async Task<JsonResult> ImportData()
    {
      var watch = new Stopwatch();
      watch.Start();
      var uploadfile = this.Request.Files[0];
      var uploadfilename = uploadfile.FileName;
      var model = this.Request.Form["model"] ?? "model";
      var autosave = Convert.ToBoolean(this.Request.Form["autosave"] ?? "false");
      var givenname = (string)ViewBag.GivenName;
      try
      {

        var ext = Path.GetExtension(uploadfilename);
        var newfileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}_{uploadfile.FileName.Replace(ext, "")}{ext}";//重组成新的文件名
        var stream = new MemoryStream();
        await uploadfile.InputStream.CopyToAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);
        uploadfile.InputStream.Seek(0, SeekOrigin.Begin);
        var data = await NPOIHelper.GetDataTableFromExcelAsync(stream, ext);
        await this.waterManualRecordService.ImportDataTable(data, givenname);
        await this.unitOfWork.SaveChangesAsync();
        if (autosave)
        {
          var folder = this.Server.MapPath($"/UploadFiles/{model}");
          if (!Directory.Exists(folder))
          {
            Directory.CreateDirectory(folder);
          }
          var savepath = Path.Combine(folder, newfileName);
          uploadfile.SaveAs(savepath);
        }
        watch.Stop();
        //获取当前实例测量得出的总运行时间（以毫秒为单位）
        var elapsedTime = watch.ElapsedMilliseconds.ToString();
        this.logger.Info($"导入成功,文件名:{uploadfilename},耗时:{elapsedTime}");
        return Json(new { success = true, filename = newfileName, elapsedTime = elapsedTime }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        var message = e.GetMessage();
        this.logger.Error(e, $"导入失败,文件名:{uploadfilename}");
        return this.Json(new { success = false, filename = uploadfilename, message = message }, JsonRequestBehavior.AllowGet);
      }
    }

  }
}
