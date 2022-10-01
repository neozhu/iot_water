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
  /// File: CustomerWaterRecordsController.cs
  /// Purpose:水务管理中心/单位抄表记录
  /// Created Date: 3/25/2020 10:41:26 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<CustomerWaterRecord>, Repository<CustomerWaterRecord>>();
  ///    container.RegisterType<ICustomerWaterRecordService, CustomerWaterRecordService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("CustomerWaterRecords")]
  public class CustomerWaterRecordsController : Controller
  {
    private readonly ICustomerWaterRecordService customerWaterRecordService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    public CustomerWaterRecordsController(
          ICustomerWaterRecordService customerWaterRecordService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.customerWaterRecordService = customerWaterRecordService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
    //自动抄表
    public async Task<JsonResult> AutoRecordMonth(int year=0,int month=0) {
      try
      {
        if (year == 0 || month == 0)
        {
          year = DateTime.Now.Year;
          month = DateTime.Now.Month;
          for (var i = 1; i <= month; i++)
          {
            await this.customerWaterRecordService.AutoRecordMonth(year, i, ViewBag.FullName);
          }
        }
        else
        {
          await this.customerWaterRecordService.AutoRecordMonth(year, month, ViewBag.FullName);
        }
        var result = await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //手工抄表
    public async Task<JsonResult> AddRecord(CustomerWaterRecord item) {
      try
      {
        await this.customerWaterRecordService.AddRecord(item);
  
      var result = await this.unitOfWork.SaveChangesAsync();
      return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
    }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message
  }, JsonRequestBehavior.AllowGet);
      }
      }
    //GET: CustomerWaterRecords/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "单位抄表记录", Order = 1)]
    public ActionResult Index() => this.View();

    //Get :CustomerWaterRecords/GetData
    //For Index View datagrid datasource url
    //获取上期抄表记录
    public async Task<JsonResult> GetPreviousData(string meterid, int customerid)
    {
      var data = await this.customerWaterRecordService.Queryable()
        .Where(x => x.meterId == meterid && x.CustomerId == customerid)
        .OrderByDescending(x => x.Id)
        .FirstOrDefaultAsync();
      return Json(data, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.customerWaterRecordService
                           .Query(new CustomerWaterRecordQuery().Withfilter(filters)).Include(c => c.Customer)
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         CustomerName = n.Customer?.Name,
                                         Id = n.Id,
                                         CustomerId = n.CustomerId,
                                         Year = n.Year,
                                         Month = n.Month,
                                         meterId = n.meterId,
                                         previousDate = n.previousDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         previousWater = n.previousWater,
                                         lastWater = n.lastWater,
                                         water = n.water,
                                         RecordDate = n.RecordDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         User = n.User,
                                         Type = n.Type,
                                      
                                         IsDelete = n.IsDelete
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetDataByCustomerId(int customerid, int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.customerWaterRecordService
                       .Query(new CustomerWaterRecordQuery().ByCustomerIdWithfilter(customerid, filters)).Include(c => c.Customer)
                     .OrderBy(n => n.OrderBy(sort, order))
                     .SelectPageAsync(page, rows, out var totalCount) )
                                   .Select(n => new
                                   {

                                     CustomerName = n.Customer?.Name,
                                     Id = n.Id,
                                     CustomerId = n.CustomerId,
                                     Year = n.Year,
                                     Month = n.Month,
                                     meterId = n.meterId,
                                     previousDate = n.previousDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                     previousWater = n.previousWater,
                                     lastWater = n.lastWater,
                                     water = n.water,
                                     RecordDate = n.RecordDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                     User = n.User,
                                     Type = n.Type,
                                     IsDelete = n.IsDelete
                                   }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveData(CustomerWaterRecord[] customerwaterrecords)
    {
      if (customerwaterrecords == null)
      {
        throw new ArgumentNullException(nameof(customerwaterrecords));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in customerwaterrecords)
          {
            this.customerWaterRecordService.ApplyChanges(item);
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
    public async Task<JsonResult> GetCustomers(string q = "")
    {
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      var rows = await customerRepository
                            .Queryable()
                            .Where(n => n.Name.Contains(q))
                            .OrderBy(n => n.Name)
                            .Select(n => new { Id = n.Id, Name = n.Name })
                            .ToListAsync();
      return Json(rows, JsonRequestBehavior.AllowGet);
    }
    //GET: CustomerWaterRecords/Details/:id
    public ActionResult Details(int id)
    {

      var customerWaterRecord = this.customerWaterRecordService.Find(id);
      if (customerWaterRecord == null)
      {
        return HttpNotFound();
      }
      return View(customerWaterRecord);
    }
    //GET: CustomerWaterRecords/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var customerWaterRecord = await this.customerWaterRecordService.FindAsync(id);
      return Json(customerWaterRecord, JsonRequestBehavior.AllowGet);
    }
    //GET: CustomerWaterRecords/Create
    public ActionResult Create()
    {
      var customerWaterRecord = new CustomerWaterRecord();
      //set default value
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(customerRepository.Queryable().OrderBy(n => n.Name), "Id", "Name");
      return View(customerWaterRecord);
    }
    //POST: CustomerWaterRecords/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CustomerWaterRecord customerWaterRecord)
    {
      if (customerWaterRecord == null)
      {
        throw new ArgumentNullException(nameof(customerWaterRecord));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.customerWaterRecordService.Insert(customerWaterRecord);
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
        //DisplaySuccessMessage("Has update a customerWaterRecord record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      //ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", customerWaterRecord.CustomerId);
      //return View(customerWaterRecord);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var customerWaterRecord = await Task.Run(() =>
      {
        return new CustomerWaterRecord();
      });
      return Json(customerWaterRecord, JsonRequestBehavior.AllowGet);
    }


    //GET: CustomerWaterRecords/Edit/:id
    public ActionResult Edit(int id)
    {
      var customerWaterRecord = this.customerWaterRecordService.Find(id);
      if (customerWaterRecord == null)
      {
        return HttpNotFound();
      }
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(customerRepository.Queryable().OrderBy(n => n.Name), "Id", "Name", customerWaterRecord.CustomerId);
      return View(customerWaterRecord);
    }
    //POST: CustomerWaterRecords/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(CustomerWaterRecord customerWaterRecord)
    {
      if (customerWaterRecord == null)
      {
        throw new ArgumentNullException(nameof(customerWaterRecord));
      }
      if (ModelState.IsValid)
      {
        customerWaterRecord.TrackingState = TrackingState.Modified;
        try
        {
          this.customerWaterRecordService.Update(customerWaterRecord);

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

        //DisplaySuccessMessage("Has update a CustomerWaterRecord record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      //return View(customerWaterRecord);
    }
    //删除当前记录
    //GET: CustomerWaterRecords/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.customerWaterRecordService.Queryable().Where(x => x.Id == id).DeleteAsync();
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

    //停用水表

    [HttpPost]
    public async Task<JsonResult> Stop(int[] id)
    {
      try
      {
        await this.customerWaterRecordService.Stop(id);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
        await this.customerWaterRecordService.Delete(id);
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
      var fileName = "customerwaterrecords_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.customerWaterRecordService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
