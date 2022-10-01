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
using Repository.Pattern.Infrastructure;
using Z.EntityFramework.Plus;
using TrackableEntities;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
namespace WebApp.Controllers
{
/// <summary>
/// File: OrgChartsController.cs
/// Purpose:设备管理/拓扑结构
/// Created Date: 2020/11/3 19:40:03
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
/// <![CDATA[
///    container.RegisterType<IRepositoryAsync<OrgChart>, Repository<OrgChart>>();
///    container.RegisterType<IOrgChartService, OrgChartService>();
/// ]]>
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
    [Authorize]
    [RoutePrefix("OrgCharts")]
	public class OrgChartsController : Controller
	{
		private readonly IOrgChartService  orgChartService;
		private readonly IUnitOfWorkAsync unitOfWork;
        private readonly NLog.ILogger logger;
		public OrgChartsController (
          IOrgChartService  orgChartService, 
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
		{
			this.orgChartService  = orgChartService;
			this.unitOfWork = unitOfWork;
            this.logger = logger;
		}
        		//GET: OrgCharts/Index
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        [Route("Index", Name = "拓扑结构", Order = 1)]
		public ActionResult Index() => this.View();
    [Route("OrgView", Name = "拓扑结构图", Order = 2)]
    public ActionResult OrgView() {

      var list = this.orgChartService.Queryable().ToList();
      return View(list);
      }

		//Get :OrgCharts/GetData
		//For Index View datagrid datasource url
        
		[HttpGet]
        //[OutputCache(Duration = 10, VaryByParam = "*")]
		 public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var pagerows  = (await this.orgChartService
						               .Query(new OrgChartQuery().Withfilter(filters)).Include(o => o.Parent)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    ParentNo = n.Parent?.No,
    Id = n.Id,
    No = n.No,
    Level = n.Level,
    LevelNo = n.LevelNo,
    Location = n.Location,
    Precision = n.Precision,
    DN = n.DN,
    Year = n.Year,
    Remark = n.Remark,
    ParentId = n.ParentId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
        [HttpGet]
        //[OutputCache(Duration = 10, VaryByParam = "*")]
        public async Task<JsonResult> GetDataByParentId (int  parentid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			    var pagerows = (await this.orgChartService
						               .Query(new OrgChartQuery().ByParentIdWithfilter(parentid,filters)).Include(o => o.Parent)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    ParentNo = n.Parent?.No,
    Id = n.Id,
    No = n.No,
    Level = n.Level,
    LevelNo = n.LevelNo,
    Location = n.Location,
    Precision = n.Precision,
    DN = n.DN,
    Year = n.Year,
    Remark = n.Remark,
    ParentId = n.ParentId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        //easyui datagrid post acceptChanges 
		[HttpPost]
		public async Task<JsonResult> AcceptChanges(OrgChart[] orgcharts)
		{
            try{
               this.orgChartService.ApplyChanges( orgcharts);
               var result = await this.unitOfWork.SaveChangesAsync();
			   return Json(new {success=true,result}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
            }
        }
				//[OutputCache(Duration = 10, VaryByParam = "q")]
		public async Task<JsonResult> GetOrgCharts(string q="")
		{
			var orgchartRepository = this.unitOfWork.RepositoryAsync<OrgChart>();
			var rows = await orgchartRepository
                            .Queryable()
                            .Where(n=>n.No.Contains(q))
                            .OrderBy(n=>n.No)
                            .Select(n => new { Id = n.Id, No = n.No })
                            .ToListAsync();
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
		 
				
		//GET: OrgCharts/Details/:id
		public ActionResult Details(int id)
		{
			
			var orgChart = this.orgChartService.Find(id);
			if (orgChart == null)
			{
				return HttpNotFound();
			}
			return View(orgChart);
		}
        //GET: OrgCharts/GetItem/:id
        [HttpGet]
        public async Task<JsonResult> GetItem(int id) {
            var  orgChart = await this.orgChartService.FindAsync(id);
            return Json(orgChart,JsonRequestBehavior.AllowGet);
        }
		//GET: OrgCharts/Create
        		public ActionResult Create()
				{
			var orgChart = new OrgChart();
			//set default value
			var orgchartRepository = this.unitOfWork.RepositoryAsync<OrgChart>();
		   			ViewBag.ParentId = new SelectList(orgchartRepository.Queryable().OrderBy(n=>n.No), "Id", "No");
		   			return View(orgChart);
		}
		//POST: OrgCharts/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(OrgChart orgChart)
		{
            if (ModelState.IsValid)
			{
                try{ 
				this.orgChartService.Insert(orgChart);
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
			    //DisplaySuccessMessage("Has update a orgChart record");
			}
			else {
			   var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			   //DisplayErrorMessage(modelStateErrors);
			}
			//var orgchartRepository = this.unitOfWork.RepositoryAsync<OrgChart>();
			//ViewBag.ParentId = new SelectList(await orgchartRepository.Queryable().OrderBy(n=>n.No).ToListAsync(), "Id", "No", orgChart.ParentId);
			//return View(orgChart);
		}

        //新增对象初始化
        [HttpGet]
        public async Task<JsonResult> NewItem() {
            var orgChart = await Task.Run(() => {
                return new OrgChart();
                });
            return Json(orgChart, JsonRequestBehavior.AllowGet);
        }

         
		//GET: OrgCharts/Edit/:id
		public ActionResult Edit(int id)
		{
			var orgChart = this.orgChartService.Find(id);
			if (orgChart == null)
			{
				return HttpNotFound();
			}
			var orgchartRepository = this.unitOfWork.RepositoryAsync<OrgChart>();
			ViewBag.ParentId = new SelectList(orgchartRepository.Queryable().OrderBy(n=>n.No), "Id", "No", orgChart.ParentId);
			return View(orgChart);
		}
		//POST: OrgCharts/Edit/:id
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(OrgChart orgChart)
		{
			if (ModelState.IsValid)
			{
				orgChart.TrackingState = TrackingState.Modified;
				                try{
				this.orgChartService.Update(orgChart);
				                
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
				
				//DisplaySuccessMessage("Has update a OrgChart record");
				//return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			//DisplayErrorMessage(modelStateErrors);
			}
						//var orgchartRepository = this.unitOfWork.RepositoryAsync<OrgChart>();
												//return View(orgChart);
		}
        //删除当前记录
		//GET: OrgCharts/Delete/:id
        [HttpGet]
		public async Task<ActionResult> Delete(int id)
		{
          try{
               await this.orgChartService.Delete(new int[] { id });
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
        public async Task<JsonResult> DeleteChecked(int[] id) {
           try{
               await this.orgChartService.Delete(id);
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
		public async Task<ActionResult> ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "orgcharts_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream = await this.orgChartService.ExportExcel(filterRules,sort, order );
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
      try
      {

        var ext = Path.GetExtension(uploadfilename);
        var newfileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}_{uploadfile.FileName.Replace(ext, "")}{ext}";//重组成新的文件名
        var stream = new MemoryStream();
        await uploadfile.InputStream.CopyToAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);
        uploadfile.InputStream.Seek(0, SeekOrigin.Begin);
        var data = await NPOIHelper.GetDataTableFromExcelAsync(stream, ext);
        await this.orgChartService.ImportDataTable(data, Auth.GetFullName());
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
        return Json(new { success = true, filename = newfileName, elapsedTime = elapsedTime }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        var message = e.GetBaseException().Message;
        this.logger.Error(e, $"导入失败,文件名:{uploadfilename}");
        return this.Json(new { success = false, filename = uploadfilename, message = message }, JsonRequestBehavior.AllowGet);
      }
    }
		 
	}
}
