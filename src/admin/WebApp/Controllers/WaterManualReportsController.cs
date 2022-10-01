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
  /// File: WaterManualReportsController.cs
  /// Purpose:收费管理/水表月度汇总记录
  /// Created Date: 3/6/2021 5:31:32 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<WaterManualReport>, Repository<WaterManualReport>>();
  ///    container.RegisterType<IWaterManualReportService, WaterManualReportService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("WaterManualReports")]
  public class WaterManualReportsController : Controller
  {
    private readonly IWaterManualReportService waterManualReportService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    public WaterManualReportsController(
          IWaterManualReportService waterManualReportService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.waterManualReportService = waterManualReportService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
    //GET: WaterManualReports/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "水表月度汇总记录", Order = 1)]
    public ActionResult Index() => this.View();


    public JsonResult GetMonthDataSource()
    {
      var result = this.waterManualReportService.Queryable()
        .Select(x => x.Month)
        .Distinct()
        .Select(x => new { value = x, text = x })
        .ToList();
      result.Insert(0, new { value = "", text = "All" });
      return Json(result, JsonRequestBehavior.AllowGet);
    }
    //Get :WaterManualReports/GetData
    //For Index View datagrid datasource url

    [HttpPost]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = PredicateBuilder.From<WaterManualReport>(filterRules);
      var pagerows = ( await this.waterManualReportService
                           .Query(filters)
                         .OrderBy(n => n.OrderBy($"{sort} {order}"))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         Month=n.Month,
                                         meterId = n.meterId,
                                         Name = n.Name,
                                         LineNo = n.LineNo,
                                         Name1 = n.Name1,
                                         Water = n.Water,
                                         n.meterType,
                                         n.YearRatio,
                                         n.LastRatio,
                                         RecordDate = n.RecordDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         LastWater = n.LastWater,
                                         LastRecordDate = n.LastRecordDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         CalWater = n.CalWater,
                                         Remark = n.Remark,
                                         LastCalWater=n.LastCalWater,
                                         OnYearCalWater=n.OnYearCalWater,
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> AcceptChanges(WaterManualReport[] watermanualreports)
    {
      try
      {
        this.waterManualReportService.ApplyChanges(watermanualreports);
        var result = await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }

    //GET: WaterManualReports/Details/:id
    public ActionResult Details(int id)
    {

      var waterManualReport = this.waterManualReportService.Find(id);
      if (waterManualReport == null)
      {
        return HttpNotFound();
      }
      return View(waterManualReport);
    }
    //GET: WaterManualReports/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var waterManualReport = await this.waterManualReportService.FindAsync(id);
      return Json(waterManualReport, JsonRequestBehavior.AllowGet);
    }
    //GET: WaterManualReports/Create
    public ActionResult Create()
    {
      var waterManualReport = new WaterManualReport();
      //set default value
      return View(waterManualReport);
    }
    //POST: WaterManualReports/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(WaterManualReport waterManualReport)
    {
      if (ModelState.IsValid)
      {
        try
        {
          this.waterManualReportService.Insert(waterManualReport);
          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
        }
        //DisplaySuccessMessage("Has update a waterManualReport record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(waterManualReport);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var waterManualReport = await Task.Run(() =>
      {
        return new WaterManualReport();
      });
      return Json(waterManualReport, JsonRequestBehavior.AllowGet);
    }


    //GET: WaterManualReports/Edit/:id
    public ActionResult Edit(int id)
    {
      var waterManualReport = this.waterManualReportService.Find(id);
      if (waterManualReport == null)
      {
        return HttpNotFound();
      }
      return View(waterManualReport);
    }
    //POST: WaterManualReports/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(WaterManualReport waterManualReport)
    {
      if (ModelState.IsValid)
      {
        waterManualReport.TrackingState = TrackingState.Modified;
        try
        {
          this.waterManualReportService.Update(waterManualReport);

          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
        }

        //DisplaySuccessMessage("Has update a WaterManualReport record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(waterManualReport);
    }
    //删除当前记录
    //GET: WaterManualReports/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.waterManualReportService.Delete(new int[] { id });
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
        await this.waterManualReportService.Delete(id);
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
      var fileName = "watermanualreports_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.waterManualReportService.ExportExcel(filterRules, sort, order);
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
        await this.waterManualReportService.ImportDataTable(data, givenname);
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
