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
using ClosedXML.Report;
using FastReport;
using FastReport.Export.Html;
using System.Text;
using FastReport.Data;
using FastReport.Data.JsonConnection;

namespace WebApp.Controllers
{
  /// <summary>
  /// File: BillHeadersController.cs
  /// Purpose:账单管理/账单信息
  /// Created Date: 2/19/2021 8:29:57 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<BillHeader>, Repository<BillHeader>>();
  ///    container.RegisterType<IBillHeaderService, BillHeaderService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("BillHeaders")]
  public class BillHeadersController : Controller
  {
    private readonly IBillHeaderService billHeaderService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    private readonly ICodeItemService codeItemService;
    public BillHeadersController(
      ICodeItemService codeItemService,
          IBillHeaderService billHeaderService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.codeItemService = codeItemService;
      this.billHeaderService = billHeaderService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
    //发送账单
    [HttpPost]
    public async Task<JsonResult> SendToCustomer(int[] id) {
      try
      {
        var path= Server.MapPath("\\");
        await this.billHeaderService.SendToCustomer(id,path);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }
    //批量打印,生产excel
    [HttpPost]
    public async Task<ActionResult> PrintBill() {
      try
      {
        var path = Server.MapPath("\\");
        var selectid = this.Request.Form["id"].Split(',').Select(x=>Convert.ToInt32(x));
        var stream = await this.billHeaderService.PrintBill(path,selectid);
        stream.Position = 0;
        this.logger.Info($"账单生成成功");
        var filename = $"printinfo_{DateTime.Now.ToString("yyyyMMdd")}.xlsx";
        return File(stream, "application/vnd.ms-excel", filename);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }

 
    public async Task<ActionResult> PrintPreview(string selectid="")
    {
  
        var path = Server.MapPath("\\");
        var id = selectid.Split(',').Select(x => Convert.ToInt32(x));
        var data =await this.billHeaderService.Queryable().Where(x => id.Contains(x.Id))
          .Include(x => x.BillDetails).ToListAsync();

        var jsonString = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
       
        var report = new Report();

      report.Load(System.IO.Path.Combine(path, "ExcelTemplate", "printpreview.frx"));
      var conn = report.Dictionary.Connections[0] as JsonDataSourceConnection;
      var connbuilder = new JsonDataSourceConnectionStringBuilder();
      connbuilder.Json = jsonString;
      conn.ConnectionString = connbuilder.ConnectionString;
      report.Prepare();
        var export = new HTMLExport();
        export.Layers = true;
        using (var ms = new MemoryStream())
        {
          export.EmbedPictures = true;
          export.Export(report, ms);
          ms.Flush();
          var str = Encoding.UTF8.GetString(ms.ToArray());
          ViewData["report"] = str;
        }

        return View();
     
    }
    //生成账单
    [HttpGet]
    public async Task<JsonResult> GenerateBills(string month,int day)
    {
      try
      {
        await this.billHeaderService.GenerateBills(month,day);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch(Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }

    public JsonResult GetMonthDataSource() {
      var result = this.billHeaderService.Queryable()
        .Select(x => x.Month)
        .Distinct()
        .Select(x => new { value = x, text = x })
        .ToList();
      result.Insert(0, new { value = "", text = "All" });
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    //GET: BillHeaders/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "账单信息", Order = 1)]
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
    //Get :BillHeaders/GetData
    //For Index View datagrid datasource url

    [HttpPost]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = PredicateBuilder.From<BillHeader>(filterRules);
 
      var pagerows = ( await this.billHeaderService
                           .Query(filters).Include(b => b.Customer)
                         .OrderBy(n => n.OrderBy($"{sort} {order}"))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         CustomerName = n.Customer?.Name,
                                         CustomerCode=n.CustomerCode,
                                         Id = n.Id,
                                         BillNo = n.BillNo,
                                         CustomerId = n.CustomerId,
                                         Category = n.Category,
                                         WaterPrice = n.WaterPrice,
                                         ServicePrice = n.ServicePrice,
                                         Discount = n.Discount,
                                         BillDate = n.BillDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PaymentDate = n.PaymentDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ReceiptDate = n.ReceiptDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         SendDateTime = n.SendDateTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         Month = n.Month,
                                         TotalWater = n.TotalWater,
                                         LastTotalWater = n.LastTotalWater,
                                         PerCent = n.PerCent,
                                         TotalWaterAmount = n.TotalWaterAmount,
                                         TotalServiceAmount = n.TotalServiceAmount,
                                         AdjustWater = n.AdjustWater,
                                         AdjustWaterAmount = n.AdjustWaterAmount,
                                         AdjustServiceAmount = n.AdjustServiceAmount,
                                         TotalAmount = n.TotalAmount,
                                         TotalReceivableAmount = n.TotalReceivableAmount,
                                         HasSend=n.HasSend,
                                         Status = n.Status,
                                         Remark = n.Remark
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    [HttpPost]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetDataByCustomerId(int customerid, int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.billHeaderService
                       .Query(new BillHeaderQuery().ByCustomerIdWithfilter(customerid, filters)).Include(b => b.Customer)
                     .OrderBy(n => n.OrderBy($"{sort} {order}"))
                     .SelectPageAsync(page, rows, out var totalCount) )
                                   .Select(n => new
                                   {

                                     CustomerName = n.Customer?.Name,
                                     Id = n.Id,
                                     BillNo = n.BillNo,
                                     CustomerId = n.CustomerId,
                                     Category = n.Category,
                                     WaterPrice = n.WaterPrice,
                                     ServicePrice = n.ServicePrice,
                                     Discount = n.Discount,
                                     BillDate = n.BillDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                     PaymentDate = n.PaymentDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                     ReceiptDate = n.ReceiptDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                     SendDateTime = n.SendDateTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                                     Month = n.Month,
                                     TotalWater = n.TotalWater,
                                     LastTotalWater = n.LastTotalWater,
                                     PerCent = n.PerCent,
                                     TotalWaterAmount = n.TotalWaterAmount,
                                     TotalServiceAmount = n.TotalServiceAmount,
                                     AdjustWater = n.AdjustWater,
                                     AdjustWaterAmount = n.AdjustWaterAmount,
                                     AdjustServiceAmount = n.AdjustServiceAmount,
                                     TotalAmount = n.TotalAmount,
                                     TotalReceivableAmount = n.TotalReceivableAmount,
                                     Status = n.Status,
                                     HasSend = n.HasSend,
                                     Remark = n.Remark
                                   }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> AcceptChanges(BillHeader[] billheaders)
    {
      try
      {
        this.billHeaderService.ApplyChanges(billheaders);
        var result = await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
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
                            .Select(n => new { Id = n.Id, Name = n.Name,Code=n.Code })
                            .ToListAsync();
      return Json(rows, JsonRequestBehavior.AllowGet);
    }


    //GET: BillHeaders/Details/:id
    public ActionResult Details(int id)
    {

      var billHeader = this.billHeaderService.Queryable().Where(x=>x.Id==id)
        .Include(x=>x.BillDetails).First();
      if (billHeader == null)
      {
        return HttpNotFound();
      }
      return View(billHeader);
    }
    //GET: BillHeaders/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var billHeader = await this.billHeaderService.FindAsync(id);
      return Json(billHeader, JsonRequestBehavior.AllowGet);
    }
    //GET: BillHeaders/Create
    public ActionResult Create()
    {
      var billHeader = new BillHeader();
      //set default value
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(customerRepository.Queryable().OrderBy(n => n.Name), "Id", "Name");
      return View(billHeader);
    }
    //POST: BillHeaders/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(BillHeader billHeader)
    {
      if (ModelState.IsValid)
      {
        billHeader.TrackingState = TrackingState.Added;
        foreach (var item in billHeader.BillDetails)
        {
          item.BillId = billHeader.Id;
          item.TrackingState = TrackingState.Added;
        }
        try
        {
          this.billHeaderService.ApplyChanges(billHeader);
          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
        }
        //DisplaySuccessMessage("Has update a billHeader record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      //ViewBag.CustomerId = new SelectList(await customerRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", billHeader.CustomerId);
      //return View(billHeader);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var billHeader = await Task.Run(() =>
      {
        return new BillHeader();
      });
      return Json(billHeader, JsonRequestBehavior.AllowGet);
    }


    //GET: BillHeaders/Edit/:id
    public ActionResult Edit(int id)
    {
      var billHeader = this.billHeaderService.Find(id);
      if (billHeader == null)
      {
        return HttpNotFound();
      }
      var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      ViewBag.CustomerId = new SelectList(customerRepository.Queryable().OrderBy(n => n.Name), "Id", "Name", billHeader.CustomerId);
      return View(billHeader);
    }
    //POST: BillHeaders/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(BillHeader billHeader)
    {
      if (ModelState.IsValid)
      {
        billHeader.TrackingState = TrackingState.Modified;
        foreach (var item in billHeader.BillDetails)
        {
          item.BillId = billHeader.Id;
        }

        try
        {
          this.billHeaderService.ApplyChanges(billHeader);

          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
        }

        //DisplaySuccessMessage("Has update a BillHeader record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //var customerRepository = this.unitOfWork.RepositoryAsync<Customer>();
      //return View(billHeader);
    }
    //删除当前记录
    //GET: BillHeaders/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.billHeaderService.Delete(new int[] { id });
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }

    //Get Detail Row By Id For Edit
    //Get : BillHeaders/EditBillDetail/:id
    [HttpGet]
    public async Task<ActionResult> EditBillDetail(int id)
    {
      var billdetailRepository = this.unitOfWork.RepositoryAsync<BillDetail>();
      var billdetail = await billdetailRepository.FindAsync(id);
      var billheaderRepository = this.unitOfWork.RepositoryAsync<BillHeader>();
      if (billdetail == null)
      {
        ViewBag.BillId = new SelectList(await billheaderRepository.Queryable().OrderBy(n => n.BillNo).ToListAsync(), "Id", "BillNo");
        //return HttpNotFound();
        return PartialView("_BillDetailEditForm", new BillDetail());
      }
      else
      {
        ViewBag.BillId = new SelectList(await billheaderRepository.Queryable().ToListAsync(), "Id", "BillNo", billdetail.BillId);
      }
      return PartialView("_BillDetailEditForm", billdetail);
    }
    //Get Create Row By Id For Edit
    //Get : BillHeaders/CreateBillDetail
    [HttpGet]
    public async Task<ActionResult> CreateBillDetail(int billid)
    {
      var billheaderRepository = this.unitOfWork.RepositoryAsync<BillHeader>();
      ViewBag.BillId = new SelectList(await billheaderRepository.Queryable().OrderBy(n => n.BillNo).ToListAsync(), "Id", "BillNo");
      return PartialView("_BillDetailEditForm");
    }
    //Post Delete Detail Row By Id
    //Get : BillHeaders/DeleteBillDetail/:id
    [HttpGet]
    public async Task<ActionResult> DeleteBillDetail(int id)
    {
      try
      {
        var billdetailRepository = this.unitOfWork.RepositoryAsync<BillDetail>();
        billdetailRepository.Delete(id);
        var result = await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }

    //Get : BillHeaders/GetBillDetailsByBillId/:id
    [HttpGet]
    public async Task<JsonResult> GetBillDetailsByBillId(int id)
    {
      var billdetails = await this.billHeaderService.GetBillDetailsByBillId(id);
      var rows = billdetails.Select(n => new
      {

        BillHeaderBillNo = n.BillHeader?.BillNo,
        Id = n.Id,
        BillId = n.BillId,
        MeterName = n.MeterName,
        LineNo = n.LineNo,
        MeterId = n.MeterId,
        MeterName1 = n.MeterName1,
        MeterPoint = n.MeterPoint,
        Negitive = n.Negitive,
        Ratio = n.Ratio,
        Water = n.Water,
        LastWater = n.LastWater,
        PerCent = n.PerCent,
        ActualWater = n.ActualWater,
        WaterDt1 = n.WaterDt1?.ToString("yyyy-MM-dd HH:mm:ss"),
        Water1 = n.Water1,
        WaterDt2 = n.WaterDt2?.ToString("yyyy-MM-dd HH:mm:ss"),
        Water2 = n.Water2,
        Remark = n.Remark
      });
      return Json(rows, JsonRequestBehavior.AllowGet);

    }
    //账单确认
    public async Task<JsonResult> Confirm(int[] id)
    {
      try
      {
        await this.billHeaderService.Confirm(id);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }
    //获取并更新上个月的用水量
    public async Task<JsonResult> GetAdnUpdateLasterWater(string billid)
    {
      try
      {
        var item= await this.billHeaderService.GetAdnUpdateLasterWater(billid);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new{success=true, item }, JsonRequestBehavior.AllowGet);
      }
      catch(Exception e)
      {
        return Json(new { success = false,err=e.GetMessage() }, JsonRequestBehavior.AllowGet);
      }
    }

    //删除选中的记录
    [HttpPost]
    public async Task<JsonResult> DeleteChecked(int[] id)
    {
      try
      {
        await this.billHeaderService.Delete(id);
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
      var fileName = "billheaders_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.billHeaderService.ExportExcel(filterRules, sort, order);
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
        await this.billHeaderService.ImportDataTable(data, givenname);
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
