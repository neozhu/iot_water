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
using WebApp.Models.JsonModel;
using WebApp.Models.ViewModel;
using System.Web.UI;

namespace WebApp.Controllers
{
  /// <summary>
  /// File: WaterMetersController.cs
  /// Purpose:水务中心/水表信息
  /// Created Date: 3/25/2020 9:40:47 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<WaterMeter>, Repository<WaterMeter>>();
  ///    container.RegisterType<IWaterMeterService, WaterMeterService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("WaterMeters")]
  public class WaterMetersController : Controller
  {
    private readonly IWaterMeterService waterMeterService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    private readonly SqlSugar.ISqlSugarClient db;
    public WaterMetersController(
          IWaterMeterService waterMeterService,
          IUnitOfWorkAsync unitOfWork,
          SqlSugar.ISqlSugarClient db,
          NLog.ILogger logger
          )
    {
      this.waterMeterService = waterMeterService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
      this.db = db;
    }
    //停用水表
    public async Task<JsonResult> StopMeter(int[] id)
    {
      try
      {
        await this.waterMeterService.Stop(id);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
      //启用水表
      public async Task<JsonResult> EnableMeter(int[] id)
      {
      try
      {
        await this.waterMeterService.Enable(id);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }

      //GET: WaterMeters/Index
      //[OutputCache(Duration = 60, VaryByParam = "none")]
      [Route("Index", Name = "水表信息", Order = 1)]
    public ActionResult Index() => this.View();
    //获取当前水表的读数
    public async Task<JsonResult> GetWater()
    {
      var tenantid = Auth.GetTenantId();
      var data = await this.waterMeterService.Queryable()
          .Where(x => x.TenantId == tenantid)
          .Select(x => new { x.meterId, x.water })
          .OrderByDescending(x => x.water)
          .Take(4)
          .ToListAsync();
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    //同步获取云端水表信息
    public async Task<JsonResult> SyncQueryUserInfo()
    {
      try
      {
        await this.waterMeterService.SyncQueryUserInfo(Auth.GetTenantId());
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //同步获取云端水表信息
    public async Task<JsonResult> OperateValveStatus(int[] id, int state)
    {
      try
      {
        foreach (var i in id)
        {
          await this.waterMeterService.OperateValveStatus(Auth.GetTenantId(), i, state);
        }
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //获取水表号，用于客户单位绑定,避免重复绑定多家客户
    public async Task<JsonResult> GetMetersWithCustomer(string q = "", int customerid = 0)
    {
      var data = await this.waterMeterService.Queryable().Where(x => x.meterId.Contains(q) && x.CustomerId <= 0)
        .Select(x => new { x.meterId, meterSize = x.meterSize ?? "", Remark = x.Remark ?? "" }).ToListAsync();
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    public async Task<JsonResult> GetMetersWithNoCustomer(string q = "")
    {
      var sql = $@"select  x.meterId,x.meterSize,x.Remark from dbo.WaterMeters x
where not exists(select y.meterId from[dbo].[CustomerWaterMeters] y
where x.meterId = y.meterId and y.IsDelete=0)
and x.meterId like '%{q}'";
      var data = await this.db.Ado.SqlQueryAsync<meterwithnocustomer>(sql);
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    //获取已经分配客户的表
    public async Task<JsonResult> GetWaterMeters()
    {
      var data = await this.waterMeterService.Queryable().Where(x => x.CustomerId > 0)
        .Select(x => new { x.meterId, x.CustomerId, x.CustomerName }).ToListAsync();
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    //获取智能表号
    public async Task<JsonResult> GetMeterIdList(string search="")
    {
      var result =await this.waterMeterService.Queryable().Where(x => x.meterId.Contains(search) && x.meterType=="智能表")
        .Select(x =>new { id = x.meterId, text = x.meterId })
        .ToListAsync();

      return Json(result, JsonRequestBehavior.AllowGet);
    }

    //获取水表绑定区域
    public async Task<JsonResult> GetWaterLoc(string meterid = "")
    {
      //var data = await this.waterMeterService.Queryable().Where(x =>x.meterId.Contains(meterid) && x.address ==null)
      //  .Select(x => new { x.meterId,x.water }).ToListAsync();
      var sql = $@"select t1.meterId,t1.water,t3.Points points from [dbo].[WaterMeters] t1 
left join[dbo].[CustomerWaterMeters] t3 on t1.meterId=t3.meterId
where t1.longitude is null and t1.meterid like '{meterid}%'";
      var data = await this.db.Ado.SqlQueryAsync<waterpoint>(sql);
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    //获取已经分配客户机械表的表
    public async Task<JsonResult> GetMechWaterMeters()
    {
      var data = await this.waterMeterService.Queryable().Where(x => x.CustomerId > 0 && x.meterType == "机械表")
        .Select(x => new { x.meterId, x.CustomerId, x.CustomerName }).ToListAsync();
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    //获取水表
    public async Task<JsonResult> GetComboData()
    {
      var data = await this.waterMeterService.Queryable().Where(x => x.Status == "使用中")
        .Select(x => new { x.meterId, x.meterType }).ToListAsync();
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    //Get :WaterMeters/GetData
    //For Index View datagrid datasource url

    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "", string allmeterid = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.waterMeterService
                           .Query(new WaterMeterQuery().Withfilter(filters, allmeterid))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                     .ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetComboGridData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string q="")
    {
      var pagerows = ( await this.waterMeterService
                           .Query(x=>x.Name.Contains(q) || x.meterId.Contains(q))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                     .ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }

    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveData(WaterMeter[] watermeters)
    {
      if (watermeters == null)
      {
        throw new ArgumentNullException(nameof(watermeters));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in watermeters)
          {
            this.waterMeterService.ApplyChanges(item);
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
    //GET: WaterMeters/Details/:id
    public ActionResult Details(int id)
    {

      var waterMeter = this.waterMeterService.Find(id);
      if (waterMeter == null)
      {
        return HttpNotFound();
      }
      return View(waterMeter);
    }
    //GET: WaterMeters/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var waterMeter = await this.waterMeterService.FindAsync(id);
      return Json(waterMeter, JsonRequestBehavior.AllowGet);
    }
    [HttpGet]
    public async Task<JsonResult> GetMeterWith(string meterid)
    {
      var waterMeter = await this.waterMeterService.Queryable().Where(x => x.meterId == meterid).FirstOrDefaultAsync();
      return Json(waterMeter, JsonRequestBehavior.AllowGet);
    }
    //GET: WaterMeters/Create
    public ActionResult Create()
    {
      var waterMeter = new WaterMeter();
      //set default value
      return View(waterMeter);
    }
    //POST: WaterMeters/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(WaterMeter waterMeter)
    {
      if (waterMeter == null)
      {
        throw new ArgumentNullException(nameof(waterMeter));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.waterMeterService.Insert(waterMeter);
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
        //DisplaySuccessMessage("Has update a waterMeter record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(waterMeter);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var waterMeter = await Task.Run(() =>
      {
        return new WaterMeter();
      });
      return Json(waterMeter, JsonRequestBehavior.AllowGet);
    }


    //GET: WaterMeters/Edit/:id
    public ActionResult Edit(int id)
    {
      var waterMeter = this.waterMeterService.Find(id);
      if (waterMeter == null)
      {
        return HttpNotFound();
      }
      return View(waterMeter);
    }
    //POST: WaterMeters/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(WaterMeter waterMeter)
    {
      if (waterMeter == null)
      {
        throw new ArgumentNullException(nameof(waterMeter));
      }
      if (ModelState.IsValid)
      {
        waterMeter.TrackingState = TrackingState.Modified;
        try
        {
          this.waterMeterService.Update(waterMeter);

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

        //DisplaySuccessMessage("Has update a WaterMeter record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(waterMeter);
    }
    //删除当前记录
    //GET: WaterMeters/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.waterMeterService.Delete(new int[] { id });
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
    //通过表号获取口径
    [HttpPost]
    [OutputCache(Duration = 300, VaryByParam = "*")]
    public async Task<JsonResult> MappingMeter(string[] meterid) {
      var result = await this.waterMeterService.Queryable()
        .Where(x => meterid.Contains(x.meterId))
        .Select(x => new { x.meterId, x.meterSize })
        .ToListAsync();
      return Json(result, JsonRequestBehavior.AllowGet);

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
        await this.waterMeterService.Delete(id);
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
      var fileName = "watermeters_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.waterMeterService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }


  }
}
