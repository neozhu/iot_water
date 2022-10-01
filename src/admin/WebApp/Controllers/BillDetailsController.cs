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
/// File: BillDetailsController.cs
/// Purpose:Home/BillDetail
/// Created Date: 2/20/2021 9:15:34 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
/// <![CDATA[
///    container.RegisterType<IRepositoryAsync<BillDetail>, Repository<BillDetail>>();
///    container.RegisterType<IBillDetailService, BillDetailService>();
/// ]]>
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
    [Authorize]
    [RoutePrefix("BillDetails")]
	public class BillDetailsController : Controller
	{
		private readonly IBillDetailService  billDetailService;
		private readonly IUnitOfWorkAsync unitOfWork;
        private readonly NLog.ILogger logger;
		public BillDetailsController (
          IBillDetailService  billDetailService, 
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
		{
			this.billDetailService  = billDetailService;
			this.unitOfWork = unitOfWork;
            this.logger = logger;
		}
        		//GET: BillDetails/Index
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        [Route("Index", Name = "BillDetail", Order = 1)]
		public ActionResult Index() => this.View();

		//Get :BillDetails/GetData
		//For Index View datagrid datasource url
        
		[HttpPost]
        //[OutputCache(Duration = 10, VaryByParam = "*")]
		 public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = PredicateBuilder.From<BillDetail>(filterRules);
			var pagerows  = (await this.billDetailService
						               .Query(filters).Include(b => b.BillHeader)
							           .OrderBy(n=>n.OrderBy($"{sort} {order}"))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    BillHeaderBillNo = n.BillHeader?.BillNo,
    Id = n.Id,
    BillId = n.BillId,
    MeterName = n.MeterName,
    LineNo = n.LineNo,
    MeterId = n.MeterId,
    MeterName1 = n.MeterName1,
    MeterPoint = n.MeterPoint,
    Negitive = n.Negitive,
                                         n.Rate,
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
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        //[OutputCache(Duration = 10, VaryByParam = "*")]
        public async Task<JsonResult> GetDataByBillId (int  billid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			    var pagerows = (await this.billDetailService
						               .Query(new BillDetailQuery().ByBillIdWithfilter(billid,filters)).Include(b => b.BillHeader)
							           .OrderBy(n=>n.OrderBy($"{sort} {order}"))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    BillHeaderBillNo = n.BillHeader?.BillNo,
    Id = n.Id,
    BillId = n.BillId,
    MeterName = n.MeterName,
    LineNo = n.LineNo,
    MeterId = n.MeterId,
    n.Rate,
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
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        //easyui datagrid post acceptChanges 
		[HttpPost]
		public async Task<JsonResult> AcceptChanges(BillDetail[] billdetails)
		{
            try{
               this.billDetailService.ApplyChanges( billdetails);
               var result = await this.unitOfWork.SaveChangesAsync();
			   return Json(new {success=true,result}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
            }
        }
				//[OutputCache(Duration = 10, VaryByParam = "q")]
		public async Task<JsonResult> GetBillHeaders(string q="")
		{
			var billheaderRepository = this.unitOfWork.RepositoryAsync<BillHeader>();
			var rows = await billheaderRepository
                            .Queryable()
                            .Where(n=>n.BillNo.Contains(q))
                            .OrderBy(n=>n.BillNo)
                            .Select(n => new { Id = n.Id, BillNo = n.BillNo })
                            .ToListAsync();
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
		 
				
		//GET: BillDetails/Details/:id
		public ActionResult Details(int id)
		{
			
			var billDetail = this.billDetailService.Find(id);
			if (billDetail == null)
			{
				return HttpNotFound();
			}
			return View(billDetail);
		}
        //GET: BillDetails/GetItem/:id
        [HttpGet]
        public async Task<JsonResult> GetItem(int id) {
            var  billDetail = await this.billDetailService.FindAsync(id);
            return Json(billDetail,JsonRequestBehavior.AllowGet);
        }
		//GET: BillDetails/Create
        		public ActionResult Create()
				{
			var billDetail = new BillDetail();
			//set default value
			var billheaderRepository = this.unitOfWork.RepositoryAsync<BillHeader>();
		   			ViewBag.BillId = new SelectList(billheaderRepository.Queryable().OrderBy(n=>n.BillNo), "Id", "BillNo");
		   			return View(billDetail);
		}
		//POST: BillDetails/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(BillDetail billDetail)
		{
            if (ModelState.IsValid)
			{
                try{ 
				this.billDetailService.Insert(billDetail);
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
                }
			    //DisplaySuccessMessage("Has update a billDetail record");
			}
			else {
			   var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			   //DisplayErrorMessage(modelStateErrors);
			}
			//var billheaderRepository = this.unitOfWork.RepositoryAsync<BillHeader>();
			//ViewBag.BillId = new SelectList(await billheaderRepository.Queryable().OrderBy(n=>n.BillNo).ToListAsync(), "Id", "BillNo", billDetail.BillId);
			//return View(billDetail);
		}

        //新增对象初始化
        [HttpGet]
        public async Task<JsonResult> NewItem() {
            var billDetail = await Task.Run(() => {
                return new BillDetail();
                });
            return Json(billDetail, JsonRequestBehavior.AllowGet);
        }

         
		//GET: BillDetails/Edit/:id
		public ActionResult Edit(int id)
		{
			var billDetail = this.billDetailService.Find(id);
			if (billDetail == null)
			{
				return HttpNotFound();
			}
			var billheaderRepository = this.unitOfWork.RepositoryAsync<BillHeader>();
			ViewBag.BillId = new SelectList(billheaderRepository.Queryable().OrderBy(n=>n.BillNo), "Id", "BillNo", billDetail.BillId);
			return View(billDetail);
		}
		//POST: BillDetails/Edit/:id
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(BillDetail billDetail)
		{
			if (ModelState.IsValid)
			{
				billDetail.TrackingState = TrackingState.Modified;
				                try{
				this.billDetailService.Update(billDetail);
				                
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetMessage() }, JsonRequestBehavior.AllowGet);
                }
				
				//DisplaySuccessMessage("Has update a BillDetail record");
				//return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			//DisplayErrorMessage(modelStateErrors);
			}
						//var billheaderRepository = this.unitOfWork.RepositoryAsync<BillHeader>();
												//return View(billDetail);
		}
        //删除当前记录
		//GET: BillDetails/Delete/:id
        [HttpGet]
		public async Task<ActionResult> Delete(int id)
		{
          try{
               await this.billDetailService.Delete(new int[] { id });
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
        public async Task<JsonResult> DeleteChecked(int[] id) {
           try{
               await this.billDetailService.Delete(id);
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
		public async Task<ActionResult> ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "billdetails_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream = await this.billDetailService.ExportExcel(filterRules,sort, order );
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
        await this.billDetailService.ImportDataTable(data, givenname);
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
