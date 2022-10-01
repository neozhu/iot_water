using System;
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
using Repository.Pattern.Infrastructure;
using Z.EntityFramework.Plus;
using TrackableEntities;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
namespace WebApp.Controllers
{
  /// <summary>
  /// File: ZoneWaterMetersController.cs
  /// Purpose:分区记录/水表分区配置
  /// Created Date: 3/28/2020 6:45:22 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<ZoneWaterMeter>, Repository<ZoneWaterMeter>>();
  ///    container.RegisterType<IZoneWaterMeterService, ZoneWaterMeterService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("ZoneWaterMeters")]
  public class ZoneWaterMetersController : Controller
  {
    private readonly IZoneWaterMeterService zoneWaterMeterService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    private readonly SqlSugar.ISqlSugarClient db;
    public ZoneWaterMetersController(
          IZoneWaterMeterService zoneWaterMeterService,
          SqlSugar.ISqlSugarClient db,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.db = db;
      this.zoneWaterMeterService = zoneWaterMeterService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
    //GET: ZoneWaterMeters/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "水表分区配置", Order = 1)]
    public ActionResult Index() => this.View();

    //Get :ZoneWaterMeters/GetData
    //For Index View datagrid datasource url
    public async Task<JsonResult> GetWaterMeterLoc() {
      var sql = @"select t1.meterId,t1.water,t3.Points points,'1' direct,t1.longitude,t1.latitude from [dbo].[WaterMeters] t1
left join [dbo].[CustomerWaterMeters] t3 on t1.meterId=t3.meterId
where  t1.longitude is not null and t1.latitude is not null ";
      var data = await this.db.Ado.SqlQueryAsync<dynamic>(sql);
      var list = data.Select(x => new { meterId=(string)x.meterId, direct=1, water =(decimal)x.water, points = (string)x.points, longitude=(decimal)x.longitude, latitude=(decimal)x.latitude }).ToList();
      return Json(list, JsonRequestBehavior.AllowGet);

    }
    public async Task<JsonResult> GetAllWaterMeterLoc()
    {
      var sql = @"select t1.meterId,
t1.status,
t1.water,
convert(decimal(18,2),(t1.water/( select sum(t3.water)
from [dbo].[WaterMeters] t3 ))) [percent],
t3.Points zoneName,
t1.customerName,
t1.longitude,
t1.latitude 
from [dbo].[WaterMeters] t1
left join [dbo].[CustomerWaterMeters] t3 on t1.meterId=t3.meterId
where t1.longitude is not null
 ";
      var data = await this.db.Ado.SqlQueryAsync<dynamic>(sql);
      var list = data.Select(x => new {
        status=(string)x.status,
        zoneName =(string)x.zoneName,
        customerName=(string)x.customerName,
        meterId = (string)x.meterId,
        water = (decimal)x.water,
        percent=(decimal)x.percent,
        longitude = (decimal)x.longitude,
        latitude = (decimal)x.latitude }).ToList();
      return Json(list.Distinct().ToArray(), JsonRequestBehavior.AllowGet);

    }

    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.zoneWaterMeterService
                           .Query(new ZoneWaterMeterQuery().Withfilter(filters)).Include(z => z.Zone)
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {
                                         Id = n.Id,
                                         ZoneId = n.ZoneId,
                                         Direct = n.Direct,
                                         meterId = n.meterId,
                                         ZoneName = n.ZoneName,
                                         longitude = n.longitude,
                                         latitude = n.latitude,
                                         Detail = n.Detail
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetDataByZoneId(int zoneid, int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.zoneWaterMeterService
                       .Query(new ZoneWaterMeterQuery().ByZoneIdWithfilter(zoneid, filters)).Include(z => z.Zone)
                     .OrderBy(n => n.OrderBy(sort, order))
                     .SelectPageAsync(page, rows, out var totalCount) )
                                   .Select(n => new
                                   {
                                     Id = n.Id,
                                     ZoneId = n.ZoneId,
                                     Direct = n.Direct,
                                     meterId = n.meterId,
                                     ZoneName = n.ZoneName,
                                     longitude = n.longitude,
                                     latitude = n.latitude,
                                     Detail = n.Detail
                                   }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveData(ZoneWaterMeter[] zonewatermeters)
    {
      if (zonewatermeters == null)
      {
        throw new ArgumentNullException(nameof(zonewatermeters));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in zonewatermeters)
          {
            this.zoneWaterMeterService.ApplyChanges(item);
          }
          var result = await this.unitOfWork.SaveChangesAsync();
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
    //[OutputCache(Duration = 10, VaryByParam = "q")]
    public async Task<JsonResult> GetZones(string q = "")
    {
      var zoneRepository = this.unitOfWork.RepositoryAsync<Zone>();
      var rows = await zoneRepository
                            .Queryable()
                            .Where(n => n.Name.Contains(q))
                            .OrderBy(n => n.Name)
                            .Select(n => new { Id = n.Id, Name = n.Name })
                            .ToListAsync();
      return Json(rows, JsonRequestBehavior.AllowGet);
    }
    //GET: ZoneWaterMeters/Details/:id
    public ActionResult Details(int id)
    {

      var zoneWaterMeter = this.zoneWaterMeterService.Find(id);
      if (zoneWaterMeter == null)
      {
        return HttpNotFound();
      }
      return View(zoneWaterMeter);
    }
    //GET: ZoneWaterMeters/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var zoneWaterMeter = await this.zoneWaterMeterService.FindAsync(id);
      return Json(zoneWaterMeter, JsonRequestBehavior.AllowGet);
    }
    //GET: ZoneWaterMeters/Create
    public ActionResult Create()
    {
      var zoneWaterMeter = new ZoneWaterMeter();
      //set default value
      var zoneRepository = this.unitOfWork.RepositoryAsync<Zone>();
      ViewBag.ZoneId = new SelectList(zoneRepository.Queryable().OrderBy(n => n.Name), "Id", "Name");
      return View(zoneWaterMeter);
    }
    //POST: ZoneWaterMeters/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ZoneWaterMeter zoneWaterMeter)
    {
      if (zoneWaterMeter == null)
      {
        throw new ArgumentNullException(nameof(zoneWaterMeter));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.zoneWaterMeterService.Insert(zoneWaterMeter);
          var result = await this.unitOfWork.SaveChangesAsync();
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
        //DisplaySuccessMessage("Has update a zoneWaterMeter record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var zoneRepository = this.unitOfWork.RepositoryAsync<Zone>();
      //ViewBag.ZoneId = new SelectList(await zoneRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", zoneWaterMeter.ZoneId);
      //return View(zoneWaterMeter);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var zoneWaterMeter = await Task.Run(() =>
      {
        return new ZoneWaterMeter();
      });
      return Json(zoneWaterMeter, JsonRequestBehavior.AllowGet);
    }


    //GET: ZoneWaterMeters/Edit/:id
    public ActionResult Edit(int id)
    {
      var zoneWaterMeter = this.zoneWaterMeterService.Find(id);
      if (zoneWaterMeter == null)
      {
        return HttpNotFound();
      }
      var zoneRepository = this.unitOfWork.RepositoryAsync<Zone>();
      ViewBag.ZoneId = new SelectList(zoneRepository.Queryable().OrderBy(n => n.Name), "Id", "Name", zoneWaterMeter.ZoneId);
      return View(zoneWaterMeter);
    }
    //POST: ZoneWaterMeters/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ZoneWaterMeter zoneWaterMeter)
    {
      if (zoneWaterMeter == null)
      {
        throw new ArgumentNullException(nameof(zoneWaterMeter));
      }
      if (ModelState.IsValid)
      {
        zoneWaterMeter.TrackingState = TrackingState.Modified;
        try
        {
          this.zoneWaterMeterService.Update(zoneWaterMeter);

          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
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

        //DisplaySuccessMessage("Has update a ZoneWaterMeter record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var zoneRepository = this.unitOfWork.RepositoryAsync<Zone>();
      //return View(zoneWaterMeter);
    }
    //删除当前记录
    //GET: ZoneWaterMeters/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.zoneWaterMeterService.Queryable().Where(x => x.Id == id).DeleteAsync();
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
        await this.zoneWaterMeterService.Delete(id);
        await this.unitOfWork.SaveChangesAsync();
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
    //导出Excel
    [HttpPost]
    public async Task<ActionResult> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var fileName = "zonewatermeters_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.zoneWaterMeterService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
