using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Repository.Pattern.UnitOfWork;
using WebApp.Models;

namespace WebApp.Controllers
{
  [Authorize]
  public class ManagementController : Controller
  {
    private ApplicationUserManager userManager;
    private ApplicationRoleManager roleManager;
    private readonly IUnitOfWorkAsync _unitOfWork;
    private ApplicationDbContext dbContext => HttpContext.GetOwinContext().Get<ApplicationDbContext>();
    public ApplicationUserManager UserManager
    {
      get => this.userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
      private set => this.userManager = value;
    }
    public ApplicationRoleManager RoleManager
    {
      get => this.roleManager ?? this.HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
      private set => this.roleManager = value;
    }

    public ManagementController(
            IUnitOfWorkAsync _unitOfWork
         ) => this._unitOfWork = _unitOfWork;

    private async Task<ActionResult> _Index()
    {
      //set default value
      var companyRepository = this._unitOfWork.RepositoryAsync<Company>();
      this.ViewBag.CompanySelectList = await companyRepository.Queryable().Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).ToArrayAsync();

      this.ViewBag.RoleSelectList = await this.RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToArrayAsync();
      try
      {
        this.ViewBag.MaxRolesCount = this.UserManager.Users.Max(u => u.Roles.Count);
      }
      catch (InvalidOperationException ex)
      {
        this.ViewBag.MaxRolesCount = 0;
        Console.WriteLine(ex);
      }
      return this.View("Index", new ManagementViewModel
      {
        Users = await this.UserManager.Users.OrderBy(x => x.UserName).ToListAsync(),
        Roles = await this.RoleManager.Roles.OrderBy(x => x.Name).ToListAsync()
      });
    }
    [Route("management/index", Name = "角色管理", Order = 1)]
    public async Task<ActionResult> Index()
    {
      if (this.RoleManager.Roles.Count() == 0)
      {
        await this.RoleManager.CreateAsync(new ApplicationRole() { Name = "admin" });

      }

      return await this._Index();
    }



    [HttpPost]
    [Route("management/roles")]
    public async Task<ActionResult> AddRole(RoleViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        var result = await this.RoleManager.CreateAsync(new ApplicationRole { Name = model.Name });
        if (result.Succeeded)
        {
          //RedirectToAction("Index");
          return await this._Index();
        }
        else
        {
          this.AddErrors(result);
        }
      }
      return await this._Index();
    }
    [HttpGet]
    [Route("management/roles/{id}")]
    public async Task<ActionResult> DeleteRole(string id)
    {
      var role = await this.RoleManager.FindByIdAsync(id);
      if (role != null)
      {
        await this.RoleManager.DeleteAsync(role);
      }
      return this.RedirectToAction("Index");
    }
    [HttpPost]
    [Button("Attach")]
    [Route("management/account/roles")]
    public async Task<ActionResult> AttachRole(AttachRoleViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        var result = await this.UserManager.AddToRoleAsync(model.UserId, model.RoleName);
        if (result.Succeeded)
        {
          return this.RedirectToAction("Index");
        }
        else
        {
          this.AddErrors(result);
        }
      }
      return await this._Index();
    }
    [HttpPost]
    [Button("Purge")]
    [Route("management/account/roles")]
    public async Task<ActionResult> PurgeRole(AttachRoleViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        var result = await this.UserManager.RemoveFromRoleAsync(model.UserId, model.RoleName);
        if (result.Succeeded)
        {
          return this.RedirectToAction("Index");
        }
        else
        {
          this.AddErrors(result);
        }
      }
      return await this._Index();
    }
    [HttpPost]
    [Route("management/account")]
    public async Task<ActionResult> AddAccount(AccountViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        var tenant = await this.dbContext.Tenants.Where(x => x.Id == model.TenantId).FirstOrDefaultAsync();
        var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, TenantDb = tenant.ConnectionStrings, TenantName = tenant.Name };
        var result = await this.UserManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          result = await this.UserManager.AddToRoleAsync(user.Id, model.Role);
          //return RedirectToAction("Index");
        }
        else
        {
          this.AddErrors(result);
        }
      }
      return await this._Index();
    }
    [HttpGet]
    [Route("management/account/{id}")]
    public async Task<ActionResult> DeleteAccount(string id)
    {
      var user = await this.UserManager.FindByIdAsync(id);
      if (user != null)
      {
        await this.UserManager.DeleteAsync(user);
      }
      return this.RedirectToAction("Index");
    }
    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        this.ModelState.AddModelError("", error);
      }
    }
  }
}