using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TrackableEntities;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [Authorize]
  [RoutePrefix("RoleMenus")]
  public class RoleMenusController : Controller
    {
        private ApplicationUserManager userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<RoleMenu>, Repository<RoleMenu>>();
        //container.RegisterType<IRoleMenuService, RoleMenuService>();

        //private TmsAppContext db = new TmsAppContext();
        private readonly IRoleMenuService _roleMenuService;
        private readonly IMenuItemService _menuItemService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private ApplicationRoleManager _roleManager;
        public RoleMenusController(IRoleMenuService roleMenuService, IUnitOfWorkAsync unitOfWork, IMenuItemService menuItemService)
        {
            _roleMenuService = roleMenuService;
            _menuItemService = menuItemService;
           
            _unitOfWork = unitOfWork;
        }

    // GET: RoleMenus/Index
    [Route("Index", Name = "授权管理", Order = 1)]
    public async Task<ActionResult> Index()
        {

            var rolemenus =await _roleMenuService.Queryable().Include(r => r.MenuItem).ToListAsync();
            var menus =await _menuItemService.Queryable().Include(x => x.SubMenus).Where(x => x.IsEnabled && x.Parent == null).ToListAsync();
            var roles = this.RoleManager.Roles;
            var roleview = new List<RoleView>();
            foreach (var role in roles)
            {
                var mymenus =await _roleMenuService.GetByRoleNameAsync(role.Name);
                var r = new RoleView();
                r.RoleName = role.Name;
                r.Count = mymenus.Count();
                roleview.Add(r);
            }
            ViewBag.Menus = menus;
            ViewBag.Roles = roleview;
            return View(rolemenus);
        }

        [ChildActionOnly]
        [OutputCache(Duration =10)]
        public ActionResult RenderMenus()
        {
      var roles = UserManager.GetRoles(this.User.Identity.GetUserId());
      //var roles = new string[] { "admin" };
      var menus = _roleMenuService.RenderMenus(roles.ToArray());
            return PartialView("_navMenuBar", menus);
        }
        public async Task<ActionResult> GetMenuList()
        {
            var menus = _menuItemService.Queryable().Include(x => x.SubMenus).Where(x => x.IsEnabled).OrderBy(y=>y.Code);
            var totalCount = menus.Count();
            var datarows =await menus.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Code = x.Code,
                _parentId = x.ParentId,
                Url = x.Url,
                Create = true,
                Edit = true,
                Delete = true,
                Import = true,
                Export = true,
                FunctionPoint1=false,
                FunctionPoint2=false,
                FunctionPoint3=false
            }).OrderBy(x=>x._parentId).ThenBy(x=>x.Code).ToListAsync();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetMenus(string roleName)
        {

            var rolemenus =await _roleMenuService.GetByRoleNameAsync(roleName);
            //var all = _roleMenuService.RenderMenus(roleName);
            return Json(rolemenus, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<ActionResult> Submit(RoleMenusView[] selectmenus)
        {

           await _roleMenuService.AuthorizeAsync(selectmenus);
      await _unitOfWork.SaveChangesAsync();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // Get :RoleMenus/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit + 1;
            var rolemenus = _roleMenuService.Query(new RoleMenuQuery().WithAnySearch(search)).Include(r => r.MenuItem).OrderBy(n => n.OrderBy(sort, order)).SelectPage(pagenum, limit, out totalCount);

            var rows = rolemenus.Select(n => new { MenuItemTitle = (n.MenuItem == null ? "" : n.MenuItem.Title), Id = n.Id, RoleName = n.RoleName, MenuId = n.MenuId, IsEnabled = n.IsEnabled }).ToList();
            var pagelist = new { total = totalCount, rows = rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }


        // GET: RoleMenus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMenu roleMenu = _roleMenuService.Find(id);
            if (roleMenu == null)
            {
                return HttpNotFound();
            }
            return View(roleMenu);
        }


        // GET: RoleMenus/Create
        public ActionResult Create()
        {
            RoleMenu roleMenu = new RoleMenu();
            //set default value
            var menuitemRepository = _unitOfWork.Repository<MenuItem>();
            ViewBag.MenuId = new SelectList(menuitemRepository.Queryable(), "Id", "Title");
            return View(roleMenu);
        }

        // POST: RoleMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuItem,Id,RoleName,MenuId,IsEnabled")] RoleMenu roleMenu)
        {
            if (ModelState.IsValid)
            {
                _roleMenuService.Insert(roleMenu);
                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a RoleMenu record");
                return RedirectToAction("Index");
            }

            var menuitemRepository = _unitOfWork.Repository<MenuItem>();
            ViewBag.MenuId = new SelectList(menuitemRepository.Queryable(), "Id", "Title", roleMenu.MenuId);
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(roleMenu);
        }

        // GET: RoleMenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMenu roleMenu = _roleMenuService.Find(id);
            if (roleMenu == null)
            {
                return HttpNotFound();
            }
            var menuitemRepository = _unitOfWork.Repository<MenuItem>();
            ViewBag.MenuId = new SelectList(menuitemRepository.Queryable(), "Id", "Title", roleMenu.MenuId);
            return View(roleMenu);
        }

        // POST: RoleMenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuItem,Id,RoleName,MenuId,IsEnabled")] RoleMenu roleMenu)
        {
            if (ModelState.IsValid)
            {
                roleMenu.TrackingState = TrackingState.Modified;
                _roleMenuService.Update(roleMenu);

                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a RoleMenu record");
                return RedirectToAction("Index");
            }
            var menuitemRepository = _unitOfWork.Repository<MenuItem>();
            ViewBag.MenuId = new SelectList(menuitemRepository.Queryable(), "Id", "Title", roleMenu.MenuId);
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(roleMenu);
        }

        // GET: RoleMenus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMenu roleMenu = _roleMenuService.Find(id);
            if (roleMenu == null)
            {
                return HttpNotFound();
            }
            return View(roleMenu);
        }

        // POST: RoleMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleMenu roleMenu = _roleMenuService.Find(id);
            _roleMenuService.Delete(roleMenu);
            _unitOfWork.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a RoleMenu record");
            return RedirectToAction("Index");
        }






        private void DisplaySuccessMessage(string msgText)
        {
            TempData["SuccessMessage"] = msgText;
        }

        private void DisplayErrorMessage()
        {
            TempData["ErrorMessage"] = "Save changes was unsuccessful.";
        }

         



    }
}
