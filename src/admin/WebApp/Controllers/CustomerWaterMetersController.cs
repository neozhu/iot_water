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
using System.Web.Helpers;
using WebApp.Models.JsonModel;
using Org.BouncyCastle.Math.EC.Multiplier;

namespace WebApp.Controllers
{
  /// <summary>
  /// File: CustomerWaterMetersController.cs
  /// Purpose:水务管理中心/单位水表关系
  /// Created Date: 3/25/2020 10:27:42 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<CustomerWaterMeter>, Repository<CustomerWaterMeter>>();
  ///    container.RegisterType<ICustomerWaterMeterService, CustomerWaterMeterService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("CustomerWaterMeters")]
  public class CustomerWaterMetersController : Controller
  {
    private readonly ICustomerWaterMeterService customerWaterMeterService;
    private readonly ICustomerService customerService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    private readonly SqlSugar.ISqlSugarClient db;
    public CustomerWaterMetersController(
      SqlSugar.ISqlSugarClient db,
          ICustomerWaterMeterService customerWaterMeterService,
          ICustomerService customerService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.customerWaterMeterService = customerWaterMeterService;
      this.customerService = customerService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
      this.db = db;
    }

    //获取水表树状结构
    public async Task<JsonResult> GetCategoryTreeData(int isdelete=0)
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = "";
      if (isdelete == 0)
      {
         sql = @"select distinct 
t1.Category,
t1.Name,
t2.Dept,
t2.[Points],
t2.Id
from dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId
where t1.tenantid=@tenantid and t2.IsDelete=0";
      }
      else
      {
        sql = @"select distinct 
t1.Category,
t1.Name,
t2.Dept,
t2.[Points],
t2.Id
from dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId
where t1.tenantid=@tenantid ";
      }
      var data = await this.db.Ado.SqlQueryAsync<categorydata>(sql, new { tenantid });
      var list = new List<treenode>();
      foreach (var z in data.Where(x => !string.IsNullOrEmpty(x.Category)).Select(x => x.Category).Distinct()) //客户类型
      {
        var m1 = data.Where(x => x.Category == z).Select(x => x.Id).ToArray();
        var n1 = new treenode() { label = z, text = $"{z}({m1.Length})", icon = "", data = string.Join(",", m1) };
        var l1 = new List<treenode>();
        foreach (var p in data.Where(x => x.Category == z).Select(x => x.Name).Distinct()) //客户.Distinct())
        {
          var m2 = data.Where(x => x.Category == z && x.Name == p).Select(x => x.Id).ToArray();
          var n2 = new treenode() { label = p, text = $"{p}({m2.Length})", icon = "", data = string.Join(",", m2) };
          var l2 = new List<treenode>();
          foreach (var c in data.Where(x => x.Category == z && x.Name == p).Select(x => x.Dept).Distinct()) //部门
          {
            var m3 = data.Where(x => x.Category == z && x.Name == p && x.Dept == c).Select(x => x.Id).ToArray();
            var n3 = new treenode() { label = c, text = $"{c}({m3.Length})", icon = "", data = string.Join(",", m3) };
            var l3 = new List<treenode>();
            foreach (var a in data.Where(x => x.Category == z && x.Name == p && x.Dept == c).Select(x => x.Points).Distinct()) //用水点
            {
              var m4 = data.Where(x => x.Category == z && x.Name == p && x.Dept == c && x.Points == a).Select(x => x.Id).ToArray();
              var n4 = new treenode() { label = a, text = $"{a}({m4.Length})", icon = "", data = string.Join(",", m4) };
              l3.Add(n4);
            }
            n3.nodes = l3.ToArray();
            l2.Add(n3);
          }

          n2.nodes = l2.ToArray();
          l1.Add(n2);
        }



        n1.nodes = l1.ToArray();
        list.Add(n1);
      }
      
      return Json(new { list }, JsonRequestBehavior.AllowGet);
    }

    //获取水表树状结构
    public async Task<JsonResult> GetZoneTreeData()
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"select distinct 
t2.ZoneName,
t2.Dept [ManageDept],
t1.[Name],
t2.[Points],
t2.meterId
from dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId
where t1.tenantid=@tenantid --and t2.IsDelete=0";
      var data = await this.db.Ado.SqlQueryAsync<groupdetail>(sql,new { tenantid });
      var list = new List<treenode>();
      foreach (var z in data.Where(x => !string.IsNullOrEmpty(x.ZoneName)).Select(x => x.ZoneName).Distinct()) //区域
      {
        var m1 = data.Where(x => x.ZoneName == z).Select(x => x.meterId).Distinct().ToArray();
        var n1 = new treenode() {label=z, text = $"{z}({m1.Length})", icon = "", data = string.Join(",", m1) };
        var l1 = new List<treenode>();
              foreach (var p in data.Where(x => x.ZoneName == z ).Select(x => x.Points).Distinct()) //用水点.Distinct())
              {
                var m4 = data.Where(x => x.ZoneName == z && x.Points==p).Select(x => x.meterId).Distinct().ToArray();
                var n4 = new treenode() {label=p, text = $"{p}({m4.Length})", icon = "", data = string.Join(",", m4) };
                l1.Add(n4);
              }
         
            
       
        n1.nodes = l1.ToArray();
        list.Add(n1);
      }
      //foreach (var z in data.Where(x=>!string.IsNullOrEmpty(x.ZoneName)).Select(x => x.ZoneName).Distinct()) //区域
      //{
      //  var m1 = data.Where(x => x.ZoneName == z).Select(x => x.meterId).ToArray();
      //  var n1 = new treenode() { text = $"{z}({m1.Length})", icon = "", data = string.Join(",", m1) };
      //  var l1 = new List<treenode>();
      //  foreach (var d in data.Where(x => x.ZoneName == z).Select(x => x.Name).Distinct()) //单位
      //  {
      //    var m2 = data.Where(x => x.ZoneName == z && x.Name == d).Select(x => x.meterId).ToArray();
      //    var n2 = new treenode() { text = $"{d}({m2.Length})", icon = "", data = string.Join(",", m2) };
      //    var l2 = new List<treenode>();
      //    if (data.Where(x => x.ZoneName == z && x.Name == d).Select(x => x.ManageDept).Distinct().Count() > 1)
      //    {
      //      foreach (var c in data.Where(x => x.ZoneName == z && x.Name == d).Select(x => x.ManageDept).Distinct()) //部门
      //      {
      //        var m3 = data.Where(x => x.ZoneName == z && x.Name == d && x.ManageDept == c).Select(x => x.meterId).ToArray();
      //        var n3 = new treenode() { text = $"{c}({m3.Length})", icon = "", data = string.Join(",", m3) };
      //        var l3 = new List<treenode>();
      //        foreach (var p in data.Where(x => x.ZoneName == z && x.ManageDept == c && x.Name == d).Select(x => x.Points).Distinct()) //用水点.Distinct())
      //        {
      //          var m4 = data.Where(x => x.ZoneName == z && x.Name == d && x.ManageDept == c && x.Points == p).Select(x => x.meterId).ToArray();
      //          var n4 = new treenode() { text = $"{p}({m4.Length})", icon = "", data = string.Join(",", m4) };
      //          l3.Add(n4);
      //        }
      //        n3.nodes = l3.ToArray();
      //        l2.Add(n3);
      //      }
      //    }
      //    else
      //    {
      //      foreach (var p in data.Where(x => x.ZoneName == z &&  x.Name == d).Select(x => x.Points).Distinct()) //用水点.Distinct())
      //      {
      //        var m4 = data.Where(x => x.ZoneName == z && x.Name == d && x.Points == p).Select(x => x.meterId).ToArray();
      //        var n4 = new treenode() { text = $"{p}({m4.Length})", icon = "", data = string.Join(",", m4) };
      //        l2.Add(n4);
      //      }
      //    }
      //    n2.nodes = l2.ToArray();
      //    l1.Add(n2);
      //  }
      //  n1.nodes = l1.ToArray();
      //  list.Add(n1);
      //}
      //foreach (var z in data.Where(x => string.IsNullOrEmpty(x.ZoneName)).Select(x => x.ZoneName).Distinct()) //区域
      //{

      //  foreach (var d in data.Where(x => string.IsNullOrEmpty(x.ZoneName)).Select(x => x.Name).Distinct()) //单位
      //  {
      //    var m2 = data.Where(x => string.IsNullOrEmpty(x.ZoneName) && x.Name == d).Select(x => x.meterId).ToArray();
      //    var n2 = new treenode() { text = $"{d}({m2.Length})", icon = "", data = string.Join(",", m2) };
      //    var l2 = new List<treenode>();
      //    if (data.Where(x => string.IsNullOrEmpty(x.ZoneName) && x.Name == d).Select(x => x.ManageDept).Distinct().Count() > 1)
      //    {
      //      foreach (var c in data.Where(x => string.IsNullOrEmpty(x.ZoneName) && x.Name == d).Select(x => x.ManageDept).Distinct()) //部门
      //      {
      //        var m3 = data.Where(x => string.IsNullOrEmpty(x.ZoneName) && x.Name == d && x.ManageDept == c).Select(x => x.meterId).ToArray();
      //        var n3 = new treenode() { text = $"{c}({m3.Length})", icon = "", data = string.Join(",", m3) };
      //        var l3 = new List<treenode>();
      //        foreach (var p in data.Where(x => string.IsNullOrEmpty(x.ZoneName) && x.ManageDept == c && x.Name == d).Select(x => x.Points).Distinct()) //用水点.Distinct())
      //        {
      //          var m4 = data.Where(x => string.IsNullOrEmpty(x.ZoneName) && x.Name == d && x.ManageDept == c && x.Points == p).Select(x => x.meterId).ToArray();
      //          var n4 = new treenode() { text = $"{p}({m4.Length})", icon = "", data = string.Join(",", m4) };
      //          l3.Add(n4);
      //        }
      //        n3.nodes = l3.ToArray();
      //        l2.Add(n3);
      //      }
      //    }
      //    else
      //    {
      //      foreach (var p in data.Where(x => string.IsNullOrEmpty(x.ZoneName) && x.Name == d).Select(x => x.Points).Distinct()) //用水点.Distinct())
      //      {
      //        var m4 = data.Where(x => string.IsNullOrEmpty(x.ZoneName) && x.Name == d && x.Points == p).Select(x => x.meterId).ToArray();
      //        var n4 = new treenode() { text = $"{p}({m4.Length})", icon = "", data = string.Join(",", m4) };
      //        l2.Add(n4);
      //      }
      //    }
      //    n2.nodes = l2.ToArray();
      //    list.Add(n2);
      //  }


      //}
      return Json(new { list }, JsonRequestBehavior.AllowGet);
    }

    //获取水表树状结构
    public async Task<JsonResult> GetCTypeTreeData()
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
 var sql = @"select distinct 
t1.Type [ZoneName],
t2.Dept [ManageDept],
t1.[Name],
t2.[Points],
t2.meterId
from dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId where t1.tenantid=@tenantid --and t2.IsDelete=0 ";
      var data = await this.db.Ado.SqlQueryAsync<groupdetail>(sql,new { tenantid });
      var list = new List<treenode>();
      foreach (var z in data.Select(x => x.ZoneName).Distinct()) //业态
      {
        var m1 = data.Where(x => x.ZoneName == z).Select(x => x.meterId).ToArray();
        var n1 = new treenode() {label=z, text = $"{z}({m1.Length})", icon = "", data = string.Join(",", m1) };
        var l1 = new List<treenode>();
        foreach (var d in data.Where(x => x.ZoneName == z).Select(x => x.Name).Distinct()) //单位
        {
          var m2 = data.Where(x => x.ZoneName == z && x.Name == d).Select(x => x.meterId).ToArray();
          var n2 = new treenode() { label=d, text =$"{d}({m2.Length})", icon = "", data = string.Join(",", m2) };
          var l2 = new List<treenode>();
          if (data.Where(x => x.ZoneName == z && x.Name == d).Select(x => x.ManageDept).Distinct().Count() > 1)
          {
            foreach (var c in data.Where(x => x.ZoneName == z && x.Name == d).Select(x => x.ManageDept).Distinct()) //部门
            {
              var m3 = data.Where(x => x.ZoneName == z && x.Name == d && x.ManageDept == c).Select(x => x.meterId).ToArray();
              var n3 = new treenode() { label=c, text = $"{c}({m3.Length})", icon = "", data = string.Join(",", m3) };
              var l3 = new List<treenode>();
              foreach (var p in data.Where(x => x.ZoneName == z && x.ManageDept == c && x.Name == d).Select(x => x.Points).Distinct()) //用水点.Distinct())
              {
                var m4 = data.Where(x => x.ZoneName == z && x.Name == d && x.ManageDept == c && x.Points == p).Select(x => x.meterId).ToArray();
                var n4 = new treenode() { label=p, text = $"{p}({m4.Length})", icon = "", data = string.Join(",", m4) };
                l3.Add(n4);
              }
              n3.nodes = l3.ToArray();
              l2.Add(n3);
            }
          }
          else
          {
            foreach (var p in data.Where(x => x.ZoneName == z  && x.Name == d).Select(x => x.Points).Distinct()) //用水点.Distinct())
            {
              var m4 = data.Where(x => x.ZoneName == z && x.Name == d && x.Points == p).Select(x => x.meterId).ToArray();
              var n4 = new treenode() {label=p, text = $"{p}({m4.Length})", icon = "", data = string.Join(",", m4) };
              l2.Add(n4);
            }
          }
          n2.nodes = l2.ToArray();
          l1.Add(n2);
        }
        n1.nodes = l1.ToArray();
        list.Add(n1);
      }
      return Json(new { list }, JsonRequestBehavior.AllowGet);
    }
    //GET: CustomerWaterMeters/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "单位水表关系", Order = 1)]
    public ActionResult Index() => this.View();

    //Get :CustomerWaterMeters/GetData
    //For Index View datagrid datasource url

    [HttpPost]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = PredicateBuilder.From<CustomerWaterMeter>(filterRules);
      var pagerows = ( await this.customerWaterMeterService
                           .Query(filters)
                           .Include(c => c.Customer)
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {
                                         CustomerCode=n.Customer?.Code,
                                         Id = n.Id,
                                         CustomerId = n.CustomerId,
                                         meterId = n.meterId,
                                         meterName=n.meterName,
                                         n.Ratio,
                                         n.Negitive,
                                         Quota = n.Quota,
                                         IsFee = n.IsFee,
                                         Dept=n.Dept,
                                         n.Points,
                                         n.ZoneName,
                                         n.Level,
                                         Discount = n.Discount,
                                         Remark = n.Remark,
                                         CustomerName = n.CustomerName,
                                         IsDelete = n.IsDelete,
                                         RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         DeleteDate = n.DeleteDate?.ToString("yyyy-MM-dd HH:mm:ss")
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetDataByCustomerId(int customerid, int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.customerWaterMeterService
                       .Query(new CustomerWaterMeterQuery()
                       .ByCustomerIdWithfilter(customerid, filters))
                       .Include(c => c.Customer)
                     .OrderBy(n => n.OrderBy(sort, order))
                     .SelectPageAsync(page, rows, out var totalCount) )
                                   .Select(n => new
                                   {
                                     CustomerCode=n.Customer?.Code,
                                     Id = n.Id,
                                     CustomerId = n.CustomerId,
                                     n.meterName,
                                      n.Ratio,
                                     n.Negitive,
                                     meterId = n.meterId,
                                     Dept = n.Dept,
                                     Quota = n.Quota,
                                     IsFee = n.IsFee,
                                     n.Points,
                                     n.ZoneName,
                                     n.Level,
                                     Discount = n.Discount,
                                     Remark = n.Remark,
                                     CustomerName = n.CustomerName,
                                     IsDelete = n.IsDelete,
                                     RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                     DeleteDate = n.DeleteDate?.ToString("yyyy-MM-dd HH:mm:ss")
                                   }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveData(CustomerWaterMeter[] customerwatermeters)
    {
      if (customerwatermeters == null)
      {
        throw new ArgumentNullException(nameof(customerwatermeters));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in customerwatermeters)
          {
            this.customerWaterMeterService.ApplyChanges(item);
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
    //GET: CustomerWaterMeters/Details/:id
    public ActionResult Details(int id)
    {

      var customerWaterMeter = this.customerWaterMeterService.Find(id);
      if (customerWaterMeter == null)
      {
        return HttpNotFound();
      }
      return View(customerWaterMeter);
    }
    //GET: CustomerWaterMeters/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var customerWaterMeter = await this.customerWaterMeterService.FindAsync(id);
      return Json(customerWaterMeter, JsonRequestBehavior.AllowGet);
    }
    //GET: CustomerWaterMeters/Create
    public ActionResult Create()
    {
      var customerWaterMeter = new CustomerWaterMeter();
      //set default value
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(customerRepository.Queryable().OrderBy(n => n.Name), "Id", "Name");
      return View(customerWaterMeter);
    }
    //POST: CustomerWaterMeters/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CustomerWaterMeter customerWaterMeter)
    {
      if (customerWaterMeter == null)
      {
        throw new ArgumentNullException(nameof(customerWaterMeter));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.customerWaterMeterService.Insert(customerWaterMeter);
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
        //DisplaySuccessMessage("Has update a customerWaterMeter record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      //ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", customerWaterMeter.CustomerId);
      //return View(customerWaterMeter);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var customerWaterMeter = await Task.Run(() =>
      {
        return new CustomerWaterMeter();
      });
      return Json(customerWaterMeter, JsonRequestBehavior.AllowGet);
    }


    //GET: CustomerWaterMeters/Edit/:id
    public ActionResult Edit(int id)
    {
      var customerWaterMeter = this.customerWaterMeterService.Find(id);
      if (customerWaterMeter == null)
      {
        return HttpNotFound();
      }
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(customerRepository.Queryable().OrderBy(n => n.Name), "Id", "Name", customerWaterMeter.CustomerId);
      return View(customerWaterMeter);
    }
    //POST: CustomerWaterMeters/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(CustomerWaterMeter customerWaterMeter)
    {
      if (customerWaterMeter == null)
      {
        throw new ArgumentNullException(nameof(customerWaterMeter));
      }
      if (ModelState.IsValid)
      {
        customerWaterMeter.TrackingState = TrackingState.Modified;
        try
        {
          this.customerWaterMeterService.Update(customerWaterMeter);

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

        //DisplaySuccessMessage("Has update a CustomerWaterMeter record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      //return View(customerWaterMeter);
    }
    //删除当前记录
    //GET: CustomerWaterMeters/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.customerWaterMeterService.Queryable().Where(x => x.Id == id).DeleteAsync();
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
        await this.customerWaterMeterService.Stop(id);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //恢复水表
    [HttpPost]
    public async Task<JsonResult> Resume(int[] id)
    {

      try
      {
        await this.customerWaterMeterService.Resume(id);
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
        await this.customerWaterMeterService.Delete(id);
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
      var fileName = "customerwatermeters_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.customerWaterMeterService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
