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
  /// File: CustomerBillsController.cs
  /// Purpose:收费管理/收费台账
  /// Created Date: 3/28/2020 3:02:46 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<CustomerBill>, Repository<CustomerBill>>();
  ///    container.RegisterType<ICustomerBillService, CustomerBillService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("CustomerBills")]
  public class CustomerBillsController : Controller
  {
    private readonly ICustomerBillService customerBillService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    public CustomerBillsController(
          ICustomerBillService customerBillService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.customerBillService = customerBillService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
    //GET: CustomerBills/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "收费台账", Order = 1)]
    public ActionResult Index() => this.View();
    //生成台账
    public async Task<JsonResult> CreateBills()
    {
      try
      {
        await this.customerBillService.CreateBills(ViewBag.FullName);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //Get :CustomerBills/GetData
    //For Index View datagrid datasource url

    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.customerBillService
                           .Query(new CustomerBillQuery().Withfilter(filters)).Include(c => c.Customer)
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CustomerId = n.CustomerId,
                                         Year = n.Year,
                                         Month = n.Month,
                                         Status = n.Status,
                                         water = n.water,
                                         Price = n.Price,
                                         Discount = n.Discount,
                                         Amount = n.Amount,
                                         BillDate = n.BillDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         Remark = n.Remark,
                                         CustomerName = n.CustomerName
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetDataByCustomerId(int customerid, int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.customerBillService
                       .Query(new CustomerBillQuery().ByCustomerIdWithfilter(customerid, filters)).Include(c => c.Customer)
                     .OrderBy(n => n.OrderBy(sort, order))
                     .SelectPageAsync(page, rows, out var totalCount) )
                                   .Select(n => new
                                   {

                                     Id = n.Id,
                                     CustomerId = n.CustomerId,
                                     Year = n.Year,
                                     Month = n.Month,
                                     Status = n.Status,
                                     water = n.water,
                                     Price = n.Price,
                                     Discount = n.Discount,
                                     Amount = n.Amount,
                                     BillDate = n.BillDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                     Remark = n.Remark,
                                     CustomerName = n.CustomerName
                                   }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveData(CustomerBill[] customerbills)
    {
      if (customerbills == null)
      {
        throw new ArgumentNullException(nameof(customerbills));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in customerbills)
          {
            this.customerBillService.ApplyChanges(item);
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
    //GET: CustomerBills/Details/:id
    public ActionResult Details(int id)
    {

      var customerBill = this.customerBillService.Find(id);
      if (customerBill == null)
      {
        return HttpNotFound();
      }
      return View(customerBill);
    }
    //GET: CustomerBills/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var customerBill = await this.customerBillService.FindAsync(id);
      return Json(customerBill, JsonRequestBehavior.AllowGet);
    }
    //GET: CustomerBills/Create
    public ActionResult Create()
    {
      var customerBill = new CustomerBill();
      //set default value
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(customerRepository.Queryable().OrderBy(n => n.Name), "Id", "Name");
      return View(customerBill);
    }
    //POST: CustomerBills/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CustomerBill customerBill)
    {
      if (customerBill == null)
      {
        throw new ArgumentNullException(nameof(customerBill));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.customerBillService.Insert(customerBill);
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
        //DisplaySuccessMessage("Has update a customerBill record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      //ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", customerBill.CustomerId);
      //return View(customerBill);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var customerBill = await Task.Run(() =>
      {
        return new CustomerBill();
      });
      return Json(customerBill, JsonRequestBehavior.AllowGet);
    }


    //GET: CustomerBills/Edit/:id
    public ActionResult Edit(int id)
    {
      var customerBill = this.customerBillService.Find(id);
      if (customerBill == null)
      {
        return HttpNotFound();
      }
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(customerRepository.Queryable().OrderBy(n => n.Name), "Id", "Name", customerBill.CustomerId);
      return View(customerBill);
    }
    //POST: CustomerBills/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(CustomerBill customerBill)
    {
      if (customerBill == null)
      {
        throw new ArgumentNullException(nameof(customerBill));
      }
      if (ModelState.IsValid)
      {
        customerBill.TrackingState = TrackingState.Modified;
        try
        {
          this.customerBillService.Update(customerBill);

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

        //DisplaySuccessMessage("Has update a CustomerBill record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      //return View(customerBill);
    }
    //删除当前记录
    //GET: CustomerBills/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.customerBillService.Queryable().Where(x => x.Id == id).DeleteAsync();
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
        await this.customerBillService.Delete(id);
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
      var fileName = "customerbills_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.customerBillService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
