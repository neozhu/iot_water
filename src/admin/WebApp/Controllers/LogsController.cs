using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using SqlSugar;
using TrackableEntities;
using WebApp.Models;
using WebApp.Models.JsonModel;
using WebApp.Repositories;
using WebApp.Services;
using Z.EntityFramework.Plus;
namespace WebApp.Controllers
{
  /// <summary>
  /// File: LogsController.cs
  /// Purpose:系统管理/系统日志
  /// Created Date: 9/19/2019 8:51:53 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<Log>, Repository<Log>>();
  ///    container.RegisterType<ILogService, LogService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("Logs")]
  public class LogsController : Controller
  {
    private readonly ILogService logService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    private readonly ISqlSugarClient db;
    public LogsController(
      ILogService logService,
      IUnitOfWorkAsync unitOfWork,
      NLog.ILogger logger,
      ISqlSugarClient db
      )
    {
      this.logService = logService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
      this.db = db;
    }
    //GET: Logs/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "系统日志", Order = 1)]
    public async Task<ActionResult> Index()
    {




      return View();
    }

    public async Task<JsonResult> GetChartData()
    {

      var sql = @"SELECT CONVERT(Datetime,format(min(Logged),'yyyy-MM-dd HH:00:00')) AS [time],
       COUNT(*) AS total
FROM Logs
where DATEDIFF(D, GETDATE(), Logged)> -3
GROUP BY CAST(Logged as date),
       DATEPART(hour, Logged)
order by  CAST(Logged as date),
       DATEPART(hour, Logged)";
      var data = await this.db.Ado.SqlQueryAsync<logtotal>(sql);
      var date = DateTime.Now.AddDays(-2).Date;
      var today = DateTime.Now.AddDays(1).Date;
      var list = new List<dynamic>();
      while (( date = date.AddHours(1) ) < today)
      {
        var item = data.Where(x => x.time == date).FirstOrDefault();
        if (item != null)
        {
          list.Add(new { time = date.ToString("yyyy-MM-dd HH:mm"), total = item.total });
        }
        else
        {
          list.Add(new { time = date.ToString("yyyy-MM-dd HH:mm"), total = 0 });

        }

      }
      var sql1 = @"select Level [level],count(*) total
FROM Logs
where DATEDIFF(D, GETDATE(), Logged)> -3
group by Level";
      var array = await this.db.Ado.SqlQueryAsync<leveltotal>(sql1);
      var levels = new string[] { "Info", "Trace", "Debug", "Warn", "Error", "Fatal" };
      var group = new List<dynamic>();
      foreach (var level in levels)
      {
        var item = array.Where(x => x.level == level).FirstOrDefault();
        if (item != null)
        {
          group.Add(new { level, item.total });
        }
        else
        {
          group.Add(new { level, total = 0 });

        }
      }

      return Json(new { list = list, group = group }, JsonRequestBehavior.AllowGet);
    }
    //Get :Logs/GetData
    //For Index View datagrid datasource url
    //更新日志状态
    [HttpGet]
    public async Task<JsonResult> SetLogState(int id)
    {

      var item = await this.logService.Queryable().Where(x => x.Id == id)
        .UpdateAsync(x => new Log()
        { Resolved = true });
      return Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }
    [HttpGet]
    public ActionResult Notify() => this.PartialView("_notifications");
    [HttpGet]
    [OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.logService
                                 .Query(new LogQuery().Withfilter(filters))
                                 .OrderBy(n => n.OrderBy(sort, order))
                                 .SelectPageAsync(page, rows, out var totalCount) )
                                 .Select(n => new
                                 {

                                   Id = n.Id,
                                   MachineName = n.MachineName,
                                   Logged = n.Logged?.ToString("yyyy-MM-dd HH:mm:ss"),
                                   Level = n.Level,
                                   ClientIP = n.ClientIP,
                                   Message = n.Message,
                                   Exception = n.Exception,
                                   Properties = n.Properties,
                                   User = n.User,
                                   Logger = n.Logger,
                                   Callsite = n.Callsite,
                                   Resolved = n.Resolved
                                 }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return this.Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveDataAsync(Log[] logs)
    {
      if (logs == null)
      {
        throw new ArgumentNullException(nameof(logs));
      }
      if (this.ModelState.IsValid)
      {
        try
        {
          foreach (var item in logs)
          {
            this.logService.ApplyChanges(item);
          }
          var result = await this.unitOfWork.SaveChangesAsync();
          return this.Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
      }

    }
    //GET: Logs/Details/:id
    public ActionResult Details(int id)
    {

      var log = this.logService.Find(id);
      if (log == null)
      {
        return this.HttpNotFound();
      }
      return this.View(log);
    }
    //GET: Logs/GetItemAsync/:id
    [HttpGet]
    public async Task<JsonResult> GetItemAsync(int id)
    {
      var log = await this.logService.FindAsync(id);
      return this.Json(log, JsonRequestBehavior.AllowGet);
    }
    //GET: Logs/Create
    public ActionResult Create()
    {
      var log = new Log();
      //set default value
      return this.View(log);
    }
    //POST: Logs/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateAsync(Log log)
    {
      if (log == null)
      {
        throw new ArgumentNullException(nameof(log));
      }
      if (this.ModelState.IsValid)
      {
        try
        {
          this.logService.Insert(log);
          var result = await this.unitOfWork.SaveChangesAsync();
          return this.Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
        //DisplaySuccessMessage("Has update a log record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(log);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItemAsync()
    {
      var log = await Task.Run(() =>
      {
        return new Log();
      });
      return this.Json(log, JsonRequestBehavior.AllowGet);
    }


    //GET: Logs/Edit/:id
    public ActionResult Edit(int id)
    {
      var log = this.logService.Find(id);
      if (log == null)
      {
        return this.HttpNotFound();
      }
      return this.View(log);
    }
    //POST: Logs/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditAsync(Log log)
    {
      if (log == null)
      {
        throw new ArgumentNullException(nameof(log));
      }
      if (this.ModelState.IsValid)
      {
        log.TrackingState = TrackingState.Modified;
        try
        {
          this.logService.Update(log);

          var result = await this.unitOfWork.SaveChangesAsync();
          return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }

        //DisplaySuccessMessage("Has update a Log record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(log);
    }
    //删除当前记录
    //GET: Logs/Delete/:id
    [HttpGet]
    public async Task<ActionResult> DeleteAsync(int id)
    {
      try
      {
        await this.logService.Queryable().Where(x => x.Id == id).DeleteAsync();
        return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
        return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }




    //删除选中的记录
    [HttpPost]
    public async Task<JsonResult> DeleteCheckedAsync(int[] id)
    {
      if (id == null)
      {
        throw new ArgumentNullException(nameof(id));
      }
      try
      {
        this.logService.Delete(id);
        await this.unitOfWork.SaveChangesAsync();
        return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
        return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //导出Excel
    [HttpPost]
    public async Task<ActionResult> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var fileName = "logs_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.logService.ExportExcelAsync(filterRules, sort, order);
      return this.File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => this.TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => this.TempData["ErrorMessage"] = msgText;

  }
}
