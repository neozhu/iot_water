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
using System.Web.UI.WebControls;
using WebApp.Models.JsonModel;
using WebApp.Models.ViewModel;
using SqlSugar;

namespace WebApp.Controllers
{
  /// <summary>
  /// File: WaterMeterRecordsController.cs
  /// Purpose:水表信息/水表采集数据
  /// Created Date: 3/17/2020 10:29:04 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<WaterMeterRecord>, Repository<WaterMeterRecord>>();
  ///    container.RegisterType<IWaterMeterRecordService, WaterMeterRecordService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("WaterMeterRecords")]
  public class WaterMeterRecordsController : Controller
  {
    private readonly IWaterMeterRecordService waterMeterRecordService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    private readonly SqlSugar.ISqlSugarClient db;
    public WaterMeterRecordsController(
          IWaterMeterRecordService waterMeterRecordService,
          IUnitOfWorkAsync unitOfWork,
          SqlSugar.ISqlSugarClient db,
          NLog.ILogger logger
          )
    {
      this.waterMeterRecordService = waterMeterRecordService;
      this.unitOfWork = unitOfWork;
      this.db = db;
      this.logger = logger;
    }
    //查询时间段汇总用水情况
    public async Task<ActionResult> QuerySummaryReport(string meterid,DateTime start, DateTime end)
    {
      var day = ( end - start ).Days;
      var end1 = start.AddMinutes(-1);
      var start1 = start.AddDays(-day-1);
      end = end.AddDays(1).AddMinutes(-1);
      var sql = $@"select t.meterId,
t.Name,
t.Name1,
t.[LineNo],
min(t.[start1]) [start1],
max(t.[minwater]) [minwater],
max(t.[end1]) [end1],
max(t.[maxwater]) [maxwater],
sum(water1) water1,
sum(reverseWater1) reverseWater1,
min(t.[start2]) [start2],
max(t.[end2]) [end2],
sum(water2) water2,
sum(reverseWater2) reverseWater2,
CASE 
 when sum(water2) >0 then (sum(water1)-sum(water2))/sum(water2) * 100
 ELSE NULL
END as rate
from(
select t1.meterId, t2.Name, t2.Name1, t2.[LineNo], min(t1.[datetime])[start1],min(t1.water) [minwater], max(t1.[datetime])[end1],max(t1.water) maxwater, ( MAX(t1.water) - min(t1.water) ) as water1, ( max(t1.reverseWater) - min(t1.reverseWater) ) as reverseWater1, null start2, null end2, null water2, null reverseWater2  from[dbo].[WaterMeterRecords] t1
inner join[dbo].[WaterMeters] t2 on t1.meterId = t2.meterId
where  t1.[datetime] >= @start and t1.[datetime] <=  @end
and t1.meterId like '{meterid}%'
group by t1.meterId, t2.Name, t2.Name1, t2.[LineNo]
union all
select t1.meterId, t2.Name, t2.Name1, t2.[LineNo], null start1 ,0 [minwater], null end1,0 [maxwater], null water1, null reverseWater1, min(t1.[datetime])[start2], max(t1.[datetime])[end2], ( MAX(t1.water) - min(t1.water) ) as water2, ( max(t1.reverseWater) - min(t1.reverseWater) ) as reverseWater2  from[dbo].[WaterMeterRecords] t1
inner join[dbo].[WaterMeters] t2 on t1.meterId = t2.meterId
where t1.[datetime] >= @start1 and t1.[datetime] <= @end1
and t1.meterId like '{meterid}%'
group by t1.meterId, t2.Name, t2.Name1, t2.[LineNo]
) t
group by t.meterId,t.Name,t.Name1,t.[LineNo]";

      var result =await this.db.Ado.SqlQueryAsync<summaryreportdto>(sql, new { start, end, start1, end1 });
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    public async Task<ActionResult> QuerySummaryReport2( DateTime start, DateTime end)
    {

      end = end.AddDays(1).AddMinutes(-1);
      var sql = $@"select 
t2.zoneName,sum(
(case when t2.Direct=1 then t1.increment else 0 end)) as  inwater,
sum(
(case when t2.Direct=-1 then t1.increment else 0 end)) as  outwater,
(sum(
(case when t2.Direct=1 then t1.increment else 0 end)) - sum(
(case when t2.Direct=-1 then t1.increment else 0 end))) as water,
case when sum(
(case when t2.Direct=1 then t1.increment else 0 end)) > 0 then
(sum(
(case when t2.Direct=-1 then t1.increment else 0 end))
/sum(
(case when t2.Direct=1 then t1.increment else 0 end)))
else 0 end as rate
from [dbo].[waterrecordincrement] t1
inner join [dbo].[ZoneWaterMeters] t2 on t1.meterId = t2.meterId
where t1.[datetime] >=@dt1 and t1.[datetime] <=@dt2
group by t2.ZoneName";

      var result = await this.db.Ado.SqlQueryAsync<summaryreportdto2>(sql, new { dt1=start, dt2=end });
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    //GET: WaterMeterRecords/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("meterwaterminanalyse", Name = "水表每天集数据分析", Order = 11)]
    public ActionResult meterwaterminanalyse() => this.View();

    public async Task<JsonResult> GetMinAnalyseData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "") {
      var filter = PredicateBuilder.From<meterwaterminanalyse>(filterRules);
      var sql = "select * from  dbo.meterwaterminanalyse";
      RefAsync<int> total = 0;
      var data = await this.db.Queryable<meterwaterminanalyse>().Where(filter).OrderBy($"{sort} {order}").ToPageListAsync(page, rows,  total);
      var pagelist = new { total = total.Value, rows = data };
      return Json(pagelist, JsonRequestBehavior.AllowGet);

    }

    [Route("Index", Name = "水表采集数据", Order = 1)]
    public ActionResult Index() => this.View();

    public async Task<ActionResult> ChartView()
    {
      var sql = "select meterId from [dbo].[WaterMeters] ";
      var list = await this.db.Ado.SqlQueryAsync<dynamic>(sql);
      ViewBag.meterlist = list.Select(x => new SelectListItem { Text = (string)x.meterId, Value = (string)x.meterId }).ToList();
      return View();
    }
    public async Task<ActionResult> ChainChartView()
    {
      var sql = "select meterId from [dbo].[WaterMeters] ";
      var list = await this.db.Ado.SqlQueryAsync<dynamic>(sql);
      ViewBag.meterlist = list.Select(x => new SelectListItem { Text = (string)x.meterId, Value = (string)x.meterId }).ToList();
      return View();
    }
    public ActionResult SummaryQuery() {
      return View();
      }
    public ActionResult SummaryQuery2()
    {
      return View();
    }
    public ActionResult SummaryChartView() {

      return View();
    }

    public ActionResult ComparedMonth() {

      return View();
     }
    public async Task<ActionResult> CustomerChartView()
    {
      var sql = "select id,[name] from dbo.[Customers] ";
      var list = await this.db.Ado.SqlQueryAsync<dynamic>(sql);
      ViewBag.customerlist = list.Select(x => new SelectListItem { Text = (string)x.name, Value = x.id.ToString() }).ToList();
      return View();
    }
    public async Task<ActionResult> ChainCustomerChartView()
    {
      var sql = "select id,[name] from dbo.[Customers] ";
      var list = await this.db.Ado.SqlQueryAsync<dynamic>(sql);
      ViewBag.customerlist = list.Select(x => new SelectListItem { Text = (string)x.name, Value = x.id.ToString() }).ToList();
      return View();
    }
    //同步获取云端水表记录信息
    public async Task<JsonResult> SyncLastDataByDatetime()
    {
      try
      {
        await this.waterMeterRecordService.SyncLastDataByDatetime(Auth.GetTenantId());
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }

    //同步下载历史水表记录信息
    public async Task<JsonResult> DownloadDetailDataByDatetime(string[] meterid,DateTime dt1, DateTime dt2)
    {
      try
      {
        await this.waterMeterRecordService.DownloadDetailDataByDatetime(Auth.GetTenantId(), meterid, dt1, dt2);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }

    //Get :WaterMeterRecords/GetData
    //For Index View datagrid datasource url

    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.waterMeterRecordService
                           .Query(new WaterMeterRecordQuery().Withfilter(filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         meterStatus = n.meterStatus,
                                         datetime = n.datetime.ToString("yyyy-MM-dd HH:mm:ss"),
                                         meterId = n.meterId,
                                         water = n.water,
                                         reverseWater = n.reverseWater,
                                         temperature = n.temperature,
                                         flowrate = n.flowrate,
                                         pressure = n.pressure,
                                         voltage = n.voltage,
                                         valveStatus = n.valveStatus,
                                         userId = n.userId,
                                         imei = n.imei,
                                         OrgName = n.OrgName
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetRecordData(string meterid,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.waterMeterRecordService
                           .Query(new WaterMeterRecordQuery().WithfilterWithMeterId(meterid,filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         meterStatus = n.meterStatus,
                                         datetime = n.datetime.ToString("yyyy-MM-dd HH:mm:ss"),
                                         meterId = n.meterId,
                                         water = n.water,
                                         reverseWater = n.reverseWater,
                                         temperature = n.temperature,
                                         flowrate = n.flowrate,
                                         pressure = n.pressure,
                                         voltage = n.voltage,
                                         valveStatus = n.valveStatus,
                                         userId = n.userId,
                                         imei = n.imei,
                                         OrgName = n.OrgName,
                                         openingdate=this.getopeningdate(n.meterId)

                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    private string getopeningdate(string meterid) {
      var rep = this.unitOfWork.RepositoryAsync<WaterMeter>();
      return rep.Queryable().Where(x => x.meterId == meterid).FirstOrDefault()?.OpeningDate?.ToString("yyyy-MM-dd");
      }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveData(WaterMeterRecord[] watermeterrecords)
    {
      if (watermeterrecords == null)
      {
        throw new ArgumentNullException(nameof(watermeterrecords));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in watermeterrecords)
          {
            this.waterMeterRecordService.ApplyChanges(item);
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
    //GET: WaterMeterRecords/Details/:id
    public ActionResult Details(int id)
    {

      var waterMeterRecord = this.waterMeterRecordService.Find(id);
      if (waterMeterRecord == null)
      {
        return HttpNotFound();
      }
      return View(waterMeterRecord);
    }
    //GET: WaterMeterRecords/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var waterMeterRecord = await this.waterMeterRecordService.FindAsync(id);
      return Json(waterMeterRecord, JsonRequestBehavior.AllowGet);
    }
    //GET: WaterMeterRecords/Create
    public ActionResult Create()
    {
      var waterMeterRecord = new WaterMeterRecord();
      //set default value
      return View(waterMeterRecord);
    }
    //POST: WaterMeterRecords/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(WaterMeterRecord waterMeterRecord)
    {
      if (waterMeterRecord == null)
      {
        throw new ArgumentNullException(nameof(waterMeterRecord));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.waterMeterRecordService.Insert(waterMeterRecord);
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
        //DisplaySuccessMessage("Has update a waterMeterRecord record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(waterMeterRecord);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var waterMeterRecord = await Task.Run(() =>
      {
        return new WaterMeterRecord();
      });
      return Json(waterMeterRecord, JsonRequestBehavior.AllowGet);
    }


    //GET: WaterMeterRecords/Edit/:id
    public ActionResult Edit(int id)
    {
      var waterMeterRecord = this.waterMeterRecordService.Find(id);
      if (waterMeterRecord == null)
      {
        return HttpNotFound();
      }
      return View(waterMeterRecord);
    }
    //POST: WaterMeterRecords/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(WaterMeterRecord waterMeterRecord)
    {
      if (waterMeterRecord == null)
      {
        throw new ArgumentNullException(nameof(waterMeterRecord));
      }
      if (ModelState.IsValid)
      {
        waterMeterRecord.TrackingState = TrackingState.Modified;
        try
        {
          this.waterMeterRecordService.Update(waterMeterRecord);

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

        //DisplaySuccessMessage("Has update a WaterMeterRecord record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(waterMeterRecord);
    }
    //删除当前记录
    //GET: WaterMeterRecords/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.waterMeterRecordService.Queryable().Where(x => x.Id == id).DeleteAsync();
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
        this.waterMeterRecordService.Delete(id);
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
      try
      {
        var fileName = "watermeterrecords_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        var stream = await this.waterMeterRecordService.ExportExcelAsync(filterRules, sort, order);
        return File(stream, "text/csv", fileName);
      }catch(Exception e)
      {
        throw e;
      }
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
