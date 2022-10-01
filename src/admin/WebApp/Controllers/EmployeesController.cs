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
/// File: EmployeesController.cs
/// Purpose:Home/Employee
/// Created Date: 3/22/2020 6:03:02 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
/// <![CDATA[
///    container.RegisterType<IRepositoryAsync<Employee>, Repository<Employee>>();
///    container.RegisterType<IEmployeeService, EmployeeService>();
/// ]]>
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
    [Authorize]
    [RoutePrefix("Employees")]
	public class EmployeesController : Controller
	{
		private readonly IEmployeeService  employeeService;
		private readonly IUnitOfWorkAsync unitOfWork;
        private readonly NLog.ILogger logger;
		public EmployeesController (
          IEmployeeService  employeeService, 
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
		{
			this.employeeService  = employeeService;
			this.unitOfWork = unitOfWork;
            this.logger = logger;
		}
        		//GET: Employees/Index
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        [Route("Index", Name = "Employee", Order = 1)]
		public ActionResult Index() => this.View();

		//Get :Employees/GetData
		//For Index View datagrid datasource url
        
		[HttpGet]
        //[OutputCache(Duration = 10, VaryByParam = "*")]
		 public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var pagerows  = (await this.employeeService
						               .Query(new EmployeeQuery().Withfilter(filters)).Include(e => e.Company).Include(e => e.Department)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    CompanyName = n.Company?.Name,
    DepartmentName = n.Department?.Name,
    Id = n.Id,
    Name = n.Name,
    Title = n.Title,
    PhoneNumber = n.PhoneNumber,
    WX = n.WX,
    Sex = n.Sex,
    Age = n.Age,
    Brithday = n.Brithday.ToString("yyyy-MM-dd HH:mm:ss"),
    EntryDate = n.EntryDate.ToString("yyyy-MM-dd HH:mm:ss"),
    IsDeleted = n.IsDeleted,
    LeaveDate = n.LeaveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
    CompanyId = n.CompanyId,
    DepartmentId = n.DepartmentId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
        [HttpGet]
        //[OutputCache(Duration = 10, VaryByParam = "*")]
        public async Task<JsonResult> GetDataByCompanyId (int  companyid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			    var pagerows = (await this.employeeService
						               .Query(new EmployeeQuery().ByCompanyIdWithfilter(companyid,filters)).Include(e => e.Company).Include(e => e.Department)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    CompanyName = n.Company?.Name,
    DepartmentName = n.Department?.Name,
    Id = n.Id,
    Name = n.Name,
    Title = n.Title,
    PhoneNumber = n.PhoneNumber,
    WX = n.WX,
    Sex = n.Sex,
    Age = n.Age,
    Brithday = n.Brithday.ToString("yyyy-MM-dd HH:mm:ss"),
    EntryDate = n.EntryDate.ToString("yyyy-MM-dd HH:mm:ss"),
    IsDeleted = n.IsDeleted,
    LeaveDate = n.LeaveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
    CompanyId = n.CompanyId,
    DepartmentId = n.DepartmentId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        //[OutputCache(Duration = 10, VaryByParam = "*")]
        public async Task<JsonResult> GetDataByDepartmentId (int  departmentid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			    var pagerows = (await this.employeeService
						               .Query(new EmployeeQuery().ByDepartmentIdWithfilter(departmentid,filters)).Include(e => e.Company).Include(e => e.Department)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    CompanyName = n.Company?.Name,
    DepartmentName = n.Department?.Name,
    Id = n.Id,
    Name = n.Name,
    Title = n.Title,
    PhoneNumber = n.PhoneNumber,
    WX = n.WX,
    Sex = n.Sex,
    Age = n.Age,
    Brithday = n.Brithday.ToString("yyyy-MM-dd HH:mm:ss"),
    EntryDate = n.EntryDate.ToString("yyyy-MM-dd HH:mm:ss"),
    IsDeleted = n.IsDeleted,
    LeaveDate = n.LeaveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
    CompanyId = n.CompanyId,
    DepartmentId = n.DepartmentId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        //easyui datagrid post acceptChanges 
		[HttpPost]
		public async Task<JsonResult> SaveData(Employee[] employees)
		{
            if (employees == null)
            {
                throw new ArgumentNullException(nameof(employees));
            }
            if (ModelState.IsValid)
			{
            try{
               foreach (var item in employees)
               {
                 this.employeeService.ApplyChanges(item);
               }
			   var result = await this.unitOfWork.SaveChangesAsync();
			   return Json(new {success=true,result}, JsonRequestBehavior.AllowGet);
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
		public async Task<JsonResult> GetCompanies(string q="")
		{
			var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
			var rows = await companyRepository
                            .Queryable()
                            .Where(n=>n.Name.Contains(q))
                            .OrderBy(n=>n.Name)
                            .Select(n => new { Id = n.Id, Name = n.Name })
                            .ToListAsync();
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
						        //[OutputCache(Duration = 10, VaryByParam = "q")]
		public async Task<JsonResult> GetDepartments(string q="")
		{
			var departmentRepository = this.unitOfWork.RepositoryAsync<Department>();
			var rows = await departmentRepository
                            .Queryable()
                            .Where(n=>n.Name.Contains(q))
                            .OrderBy(n=>n.Name)
                            .Select(n => new { Id = n.Id, Name = n.Name })
                            .ToListAsync();
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
								//GET: Employees/Details/:id
		public ActionResult Details(int id)
		{
			
			var employee = this.employeeService.Find(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			return View(employee);
		}
        //GET: Employees/GetItem/:id
        [HttpGet]
        public async Task<JsonResult> GetItem(int id) {
            var  employee = await this.employeeService.FindAsync(id);
            return Json(employee,JsonRequestBehavior.AllowGet);
        }
		//GET: Employees/Create
        		public ActionResult Create()
				{
			var employee = new Employee();
			//set default value
			var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
		   			ViewBag.CompanyId = new SelectList(companyRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name");
		   			var departmentRepository = this.unitOfWork.RepositoryAsync<Department>();
		   			ViewBag.DepartmentId = new SelectList(departmentRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name");
		   			return View(employee);
		}
		//POST: Employees/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(Employee employee)
		{
			if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            } 
            if (ModelState.IsValid)
			{
                try{ 
				this.employeeService.Insert(employee);
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result }, JsonRequestBehavior.AllowGet);
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
			    //DisplaySuccessMessage("Has update a employee record");
			}
			else {
			   var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			   //DisplayErrorMessage(modelStateErrors);
			}
			//var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
			//ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", employee.CompanyId);
			//var departmentRepository = this.unitOfWork.RepositoryAsync<Department>();
			//ViewBag.DepartmentId = new SelectList(await departmentRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", employee.DepartmentId);
			//return View(employee);
		}

        //新增对象初始化
        [HttpGet]
        public async Task<JsonResult> NewItem() {
            var employee = await Task.Run(() => {
                return new Employee();
                });
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

         
		//GET: Employees/Edit/:id
		public ActionResult Edit(int id)
		{
			var employee = this.employeeService.Find(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
			ViewBag.CompanyId = new SelectList(companyRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name", employee.CompanyId);
			var departmentRepository = this.unitOfWork.RepositoryAsync<Department>();
			ViewBag.DepartmentId = new SelectList(departmentRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name", employee.DepartmentId);
			return View(employee);
		}
		//POST: Employees/Edit/:id
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(Employee employee)
		{
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
			if (ModelState.IsValid)
			{
				employee.TrackingState = TrackingState.Modified;
				                try{
				this.employeeService.Update(employee);
				                
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result = result }, JsonRequestBehavior.AllowGet);
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
				
				//DisplaySuccessMessage("Has update a Employee record");
				//return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			//DisplayErrorMessage(modelStateErrors);
			}
						//var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
												//var departmentRepository = this.unitOfWork.RepositoryAsync<Department>();
												//return View(employee);
		}
        //删除当前记录
		//GET: Employees/Delete/:id
        [HttpGet]
		public async Task<ActionResult> Delete(int id)
		{
          try{
               await this.employeeService.Queryable().Where(x => x.Id == id).DeleteAsync();
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
        public async Task<JsonResult> DeleteChecked(int[] id) {
           if (id == null)
           {
                throw new ArgumentNullException(nameof(id));
           }
           try{
               await this.employeeService.Delete(id);
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
		public async Task<ActionResult> ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "employees_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream = await this.employeeService.ExportExcelAsync(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;
		 
	}
}
