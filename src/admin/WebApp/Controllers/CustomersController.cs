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
  /// File: CustomersController.cs
  /// Purpose:水务管理中心/单位信息
  /// Created Date: 3/25/2020 9:58:38 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<Customer>, Repository<Customer>>();
  ///    container.RegisterType<ICustomerService, CustomerService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("Customers")]
  public class CustomersController : Controller
  {
    private readonly ICustomerService customerService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    public CustomersController(
          ICustomerService customerService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.customerService = customerService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
    //GET: Customers/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "单位信息", Order = 1)]
    public ActionResult Index() => this.View();

    //统一调价功能
    public async Task<JsonResult> ChangeWatePrice(decimal value)
    {
      try
      {
        await this.customerService.ChangeWatePrice(value);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }

    //添加客户表关系
    public async Task<JsonResult> CreateCustomerWaterMeters(CustomerWaterMeter[] meters) {
      try
      {
       await  this.customerService.CreateCustomerWaterMeters(meters);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //修改客户表关系
    public async Task<JsonResult> UpdateCustomerWaterMeters(CustomerWaterMeter[] meters)
    {
      try
      {
        await this.customerService.UpdateCustomerWaterMeters(meters);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //生产配额
    public async Task<JsonResult> CreateCustomerWaterQuotas(int customerid,int year)
    {
      try
      {
        await this.customerService.CreateCustomerWaterQuotas( customerid,  year);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //Get :Customers/GetData
    //For Index View datagrid datasource url

    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.customerService
                           .Query(new CustomerQuery().Withfilter(filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {
                                         n.Category,
                                         n.Code,
                                         n.Status,
                                         n.ServicePrice,
                                         n.WaterPrice,
                                         Id = n.Id,
                                         Name = n.Name,
                                         Type=n.Type,
                                         ManageDept = n.ManageDept,
                                         Level = n.Level,
                                         Dept = n.Dept,
                                         Contact = n.Contact,
                                         ContactInfo = n.ContactInfo,
                                         MobilePhone = n.MobilePhone,
                                         Quota = n.Quota,
                                         Threshold = n.Threshold,
                                         IsFee = n.IsFee,
                                         Discount = n.Discount,
                                         RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         Remark = n.Remark
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveData(Customer[] customers)
    {
      if (customers == null)
      {
        throw new ArgumentNullException(nameof(customers));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in customers)
          {
            this.customerService.ApplyChanges(item);
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
    //GET: Customers/Details/:id
    public ActionResult Details(int id)
    {

      var customer = this.customerService.Find(id);
      if (customer == null)
      {
        return HttpNotFound();
      }
      return View(customer);
    }
    //GET: Customers/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var customer = await this.customerService.FindAsync(id);
      return Json(customer, JsonRequestBehavior.AllowGet);
    }
    //GET: Customers/Create
    public ActionResult Create()
    {
      var customer = new Customer();
      //set default value
      return View(customer);
    }
    //POST: Customers/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Customer customer)
    {
      if (customer == null)
      {
        throw new ArgumentNullException(nameof(customer));
      }
      if (ModelState.IsValid)
      {
        customer.TrackingState = TrackingState.Added;
        foreach (var item in customer.CustomerWaterRecords)
        {
          item.CustomerId = customer.Id;
          item.TrackingState = TrackingState.Added;
        }
        foreach (var item in customer.WaterMeters)
        {
          item.CustomerId = customer.Id;
          item.TrackingState = TrackingState.Added;
        }
        foreach (var item in customer.WaterQuotas)
        {
          item.CustomerId = customer.Id;
          item.TrackingState = TrackingState.Added;
        }
        try
        {
          this.customerService.ApplyChanges(customer);
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
        //DisplaySuccessMessage("Has update a customer record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(customer);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var customer = await Task.Run(() =>
      {
        return new Customer();
      });
      return Json(customer, JsonRequestBehavior.AllowGet);
    }


    //GET: Customers/Edit/:id
    public ActionResult Edit(int id)
    {
      var customer = this.customerService.Find(id);
      if (customer == null)
      {
        return HttpNotFound();
      }
      return View(customer);
    }
    //POST: Customers/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(Customer customer)
    {
      if (customer == null)
      {
        throw new ArgumentNullException(nameof(customer));
      }
      if (ModelState.IsValid)
      {
        customer.TrackingState = TrackingState.Modified;
        foreach (var item in customer.CustomerWaterRecords)
        {
          item.CustomerId = customer.Id;
        }
        foreach (var item in customer.WaterMeters)
        {
          item.CustomerId = customer.Id;
        }
        foreach (var item in customer.WaterQuotas)
        {
          item.CustomerId = customer.Id;
        }

        try
        {
          this.customerService.ApplyChanges(customer);

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

        //DisplaySuccessMessage("Has update a Customer record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(customer);
    }
    //删除当前记录
    //GET: Customers/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.customerService.Delete(new int[] { id });
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

    //Get Detail Row By Id For Edit
    //Get : Customers/EditCustomerWaterRecord/:id
    [HttpGet]
    public async Task<ActionResult> EditCustomerWaterRecord(int id)
    {
      var customerwaterrecordRepository = this.unitOfWork.RepositoryAsync<CustomerWaterRecord>();
      var customerwaterrecord = await customerwaterrecordRepository.FindAsync(id);
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      if (customerwaterrecord == null)
      {
        ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
        //return HttpNotFound();
        return PartialView("_CustomerWaterRecordEditForm", new CustomerWaterRecord());
      }
      else
      {
        ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().ToListAsync(), "Id", "Name", customerwaterrecord.CustomerId);
      }
      return PartialView("_CustomerWaterRecordEditForm", customerwaterrecord);
    }
    //Get Create Row By Id For Edit
    //Get : Customers/CreateCustomerWaterRecord
    [HttpGet]
    public async Task<ActionResult> CreateCustomerWaterRecord(int customerid)
    {
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
      return PartialView("_CustomerWaterRecordEditForm");
    }
    //Post Delete Detail Row By Id
    //Get : Customers/DeleteCustomerWaterRecord/:id
    [HttpGet]
    public async Task<ActionResult> DeleteCustomerWaterRecord(int id)
    {
      try
      {
        var customerwaterrecordRepository = this.unitOfWork.RepositoryAsync<CustomerWaterRecord>();
        customerwaterrecordRepository.Delete(id);
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
    //Get Detail Row By Id For Edit
    //Get : Customers/EditCustomerWaterMeter/:id
    [HttpGet]
    public async Task<ActionResult> EditCustomerWaterMeter(int id)
    {
      var customerwatermeterRepository = this.unitOfWork.RepositoryAsync<CustomerWaterMeter>();
      var customerwatermeter = await customerwatermeterRepository.FindAsync(id);
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      if (customerwatermeter == null)
      {
        ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
        //return HttpNotFound();
        return PartialView("_CustomerWaterMeterEditForm", new CustomerWaterMeter());
      }
      else
      {
        ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().ToListAsync(), "Id", "Name", customerwatermeter.CustomerId);
      }
      return PartialView("_CustomerWaterMeterEditForm", customerwatermeter);
    }
    //Get Create Row By Id For Edit
    //Get : Customers/CreateCustomerWaterMeter
    [HttpGet]
    public async Task<ActionResult> CreateCustomerWaterMeter(int customerid)
    {
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
      return PartialView("_CustomerWaterMeterEditForm");
    }
    //Post Delete Detail Row By Id
    //Get : Customers/DeleteCustomerWaterMeter/:id
    [HttpGet]
    public async Task<ActionResult> DeleteCustomerWaterMeter(int id)
    {
      try
      {
        var customerwatermeterRepository = this.unitOfWork.RepositoryAsync<CustomerWaterMeter>();
        customerwatermeterRepository.Delete(id);
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
    //Get Detail Row By Id For Edit
    //Get : Customers/EditCustomerWaterQuota/:id
    [HttpGet]
    public async Task<ActionResult> EditCustomerWaterQuota(int id)
    {
      var customerwaterquotaRepository = this.unitOfWork.RepositoryAsync<CustomerWaterQuota>();
      var customerwaterquota = await customerwaterquotaRepository.FindAsync(id);
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      if (customerwaterquota == null)
      {
        ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
        //return HttpNotFound();
        return PartialView("_CustomerWaterQuotaEditForm", new CustomerWaterQuota());
      }
      else
      {
        ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().ToListAsync(), "Id", "Name", customerwaterquota.CustomerId);
      }
      return PartialView("_CustomerWaterQuotaEditForm", customerwaterquota);
    }
    //Get Create Row By Id For Edit
    //Get : Customers/CreateCustomerWaterQuota
    [HttpGet]
    public async Task<ActionResult> CreateCustomerWaterQuota(int customerid)
    {
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
      return PartialView("_CustomerWaterQuotaEditForm");
    }
    //Post Delete Detail Row By Id
    //Get : Customers/DeleteCustomerWaterQuota/:id
    [HttpGet]
    public async Task<ActionResult> DeleteCustomerWaterQuota(int id)
    {
      try
      {
        var customerwaterquotaRepository = this.unitOfWork.RepositoryAsync<CustomerWaterQuota>();
        customerwaterquotaRepository.Delete(id);
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

    //Get : Customers/GetCustomerWaterRecordsByCustomerId/:id
    [HttpGet]
    public async Task<JsonResult> GetCustomerWaterRecordsByCustomerId(int id)
    {
      var customerwaterrecords = await this.customerService.GetCustomerWaterRecordsByCustomerIdAsync(id);
      var rows = customerwaterrecords.Select(n => new
      {

        CustomerName = n.Customer?.Name,
        Id = n.Id,
        CustomerId = n.CustomerId,
        Year = n.Year,
        Month = n.Month,
        meterId = n.meterId,
        previousDate = n.previousDate,
        previousWater = n.previousWater,
        lastWater = n.lastWater,
        water = n.water,
        RecordDate = n.RecordDate.ToString("yyyy-MM-dd HH:mm:ss"),
        User = n.User,
        Type = n.Type,
        IsDelete = n.IsDelete
      });
      return Json(rows, JsonRequestBehavior.AllowGet);

    }
    //Get : Customers/GetWaterMetersByCustomerId/:id
    [HttpGet]
    public async Task<JsonResult> GetWaterMetersByCustomerId(int id)
    {
      var watermeters = await this.customerService.GetWaterMetersByCustomerIdAsync(id);
      var rows = watermeters.Select(n => new
      {

        Id = n.Id,
        CustomerId = n.CustomerId,
        meterId = n.meterId,
        Quota = n.Quota,
        IsFee = n.IsFee,
        Discount = n.Discount,
        Remark = n.Remark,
        CustomerName = n.CustomerName,
        IsDelete = n.IsDelete,
        RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
        DeleteDate = n.DeleteDate?.ToString("yyyy-MM-dd HH:mm:ss")
      });
      return Json(rows, JsonRequestBehavior.AllowGet);

    }
    //Get : Customers/GetWaterQuotasByCustomerId/:id
    [HttpGet]
    public async Task<JsonResult> GetWaterQuotasByCustomerId(int id)
    {
      var waterquotas = await this.customerService.GetWaterQuotasByCustomerIdAsync(id);
      var rows = waterquotas.Select(n => new
      {

        Id = n.Id,
        CustomerId = n.CustomerId,
        Year = n.Year,
        Month = n.Month,
        Quota = n.Quota,
        Water = n.Water,
        ForecastWater = n.ForecastWater,
        RecordDate = n.RecordDate?.ToString("yyyy-MM-dd HH:mm:ss"),
        CustomerName = n.CustomerName,
        IsDelete = n.IsDelete
      });
      return Json(rows, JsonRequestBehavior.AllowGet);

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
        await this.customerService.Delete(id);
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


    [HttpPost]
    public async Task<JsonResult> EnableSelected(int[] id)
    {

      try
      {
        await this.customerService.EnableSelected(id);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    [HttpPost]
    public async Task<JsonResult> DisableSelected(int[] id)
    {

      try
      {
        await this.customerService.DisableSelected(id);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
      var fileName = "customers_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.customerService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
