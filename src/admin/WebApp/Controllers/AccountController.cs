#region Using

using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApp.Models;
using WebApp.Services;

#endregion

namespace WebApp.Controllers
{
  [Authorize]
  public class AccountController : Controller
  {
    // TODO: This should be moved to the constructor of the controller in combination with a DependencyResolver setup
    // NOTE: You can use NuGet to find a strategy for the various IoC packages out there (i.e. StructureMap.MVC5)
    //private readonly UserManager _manager = UserManager.Create();

    public ApplicationUserManager UserManager => this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
    private ApplicationDbContext dbContext => HttpContext.GetOwinContext().Get<ApplicationDbContext>();
    private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
    public ApplicationSignInManager SignInManager => this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
    private readonly NLog.ILogger logger;
    public AccountController(
      NLog.ILogger logger
   )
    {

      this.logger = logger;
    }

    // GET: /account/forgotpassword
    [AllowAnonymous]
    public ActionResult ForgotPassword()
    {
      // We do not want to use any existing identity information
      this.EnsureLoggedOut();

      return this.View();
    }

    // GET: /account/login
    [AllowAnonymous]

    public ActionResult Login(string returnUrl)
    {
      // We do not want to use any existing identity information
      this.EnsureLoggedOut();

      // Store the originating URL so we can attach it to a form field

      return this.View(new AccountLoginModel { ReturnUrl = returnUrl });
    }

    // POST: /account/login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(AccountLoginModel viewModel)
    {
      // Ensure we have a valid viewModel to work with
      if (!this.ModelState.IsValid)
      {
        return this.View(viewModel);
      }

      // Verify if a user exists with the provided identity information

      // If a user was found
      var user = await this.UserManager.FindByNameOrEmailAsync(viewModel.Email);
      if (user != null)
      {
        switch (await this.SignInManager.PasswordSignInAsync(user.UserName, viewModel.Password, viewModel.RememberMe, shouldLockout: true))
        {
          case SignInStatus.Success:
            // Then create an identity for it and sign it in
            await this.SignInAsync(user, viewModel.RememberMe);
            // If the user came from a specific page, redirect back to it
            this.logger.Info($"{user.UserName}:登录成功");
            return this.RedirectToLocal(viewModel.ReturnUrl);
          case SignInStatus.LockedOut:
            //return this.View("Lockout");
            this.ModelState.AddModelError("", "账号被锁定,30分钟后再试");
            this.logger.Warn($"{user.UserName}:账号被锁定");
            return this.View(viewModel);
          case SignInStatus.RequiresVerification:
          case SignInStatus.Failure:
          default:
            this.ModelState.AddModelError("", "用户名或密码不准确.");
            this.logger.Warn($"{user.UserName}:密码不准确");
            return this.View(viewModel);
        }
      }

      // No existing user was found that matched the given criteria
      this.ModelState.AddModelError("", "账号不存在.");
      this.logger.Warn($"{viewModel.Email}:账号不存在");
      // If we got this far, something failed, redisplay form
      return this.View(viewModel);
    }

    // GET: /account/error
    [AllowAnonymous]
    public ActionResult Error()
    {
      // We do not want to use any existing identity information
      this.EnsureLoggedOut();

      return this.View();
    }

    // GET: /account/error
    [AllowAnonymous]
    public ActionResult Error404() => this.View();

    // GET: /account/register
    [AllowAnonymous]

    public async Task<ActionResult> Register()
    {
      // We do not want to use any existing identity information
      this.EnsureLoggedOut();
      var data = await this.dbContext.Tenants
        .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
        .ToListAsync();
      ViewBag.TenantId = data;
      var model = new AccountRegistrationModel
      {
        TenantId = Convert.ToInt32(data.FirstOrDefault().Value),
        TenantName = data.FirstOrDefault().Text
      };

      return this.View(model);
    }
    public ActionResult Lockout() => View();
    // POST: /account/register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(AccountRegistrationModel viewModel)
    {
      var data = await this.dbContext.Tenants
        .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
        .ToListAsync();
      ViewBag.TenantId = data;
      var tenant = await this.dbContext.Tenants.FindAsync(viewModel.TenantId);
      // Ensure we have a valid viewModel to work with
      if (!this.ModelState.IsValid)
      {
        return this.View(viewModel);
      }

      // Try to create a user with the given identity
      try
      {
        // Prepare the identity with the provided information
        var user = new ApplicationUser
        {
          UserName = viewModel.Username,
          FullName = viewModel.FullName,
          TenantDb = tenant.ConnectionStrings,
          TenantName = tenant.Name,
          TenantId = viewModel.TenantId,
          Gender = viewModel.Gender,
          Email = viewModel.Email,
          PhoneNumber = viewModel.PhoneNumber,
          AccountType = 0,
          Avatars = "ng.jpg",
          AvatarsX120 = "ng.jpg",

        };
        var result = await this.UserManager.CreateAsync(user, viewModel.Password);

        // If the user could not be created
        if (!result.Succeeded)
        {
          // Add all errors to the page so they can be used to display what went wrong
          this.AddErrors(result);

          return this.View(viewModel);
        }
        this.logger.Info($"{user.UserName}:注册成功");
        //默认角色
        await this.UserManager.AddToRoleAsync(user.Id, "users");
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("http://schemas.microsoft.com/identity/claims/tenantid", user.TenantId.ToString()));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.GivenName, string.IsNullOrEmpty(user.FullName) ? "" : user.FullName));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("http://schemas.microsoft.com/identity/claims/tenantname", string.IsNullOrEmpty(user.TenantName) ? "" : user.TenantName));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("http://schemas.microsoft.com/identity/claims/tenantdb", string.IsNullOrEmpty(user.TenantDb) ? "" : user.TenantDb));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("http://schemas.microsoft.com/identity/claims/avatars", user.Avatars));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.MobilePhone, string.IsNullOrEmpty(user.PhoneNumber) ? "" : user.PhoneNumber));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Country, "zh-cn"));
        // If the user was able to be created we can sign it in immediately
        // Note: Consider using the email verification proces
        await this.SignInAsync(user, true);

        return this.RedirectToLocal();
      }
      catch (DbEntityValidationException ex)
      {
        // Add all errors to the page so they can be used to display what went wrong
        this.AddErrors(ex);

        return this.View(viewModel);
      }
    }
    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="username"></param>
    /// <param name="currpwd"></param>
    /// <param name="pwd"></param>
    /// <returns></returns>
    public async Task<JsonResult> ChangePassword(string username, string currpwd, string pwd)
    {
      var user = await this.UserManager.FindByNameAsync(username);
      var result = await this.UserManager.ChangePasswordAsync(user.Id, currpwd, pwd);
      if (result.Succeeded)
      {
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      else
      {
        return Json(new { success = false, err = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
      }
    }
    // POST: /account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Logout()
    {
      // First we clean the authentication ticket like always
      FormsAuthentication.SignOut();
      this.SignInManager.SignOut();
      this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.TwoFactorCookie);
      // Second we clear the principal to ensure the user does not retain any authentication
      this.HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

      // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
      // this clears the Request.IsAuthenticated flag since this triggers a new request
      return this.RedirectToLocal();
    }
    public new ActionResult Profile() => this.View();
    private ActionResult RedirectToLocal(string returnUrl = "")
    {
      // If the return url starts with a slash "/" we assume it belongs to our site
      // so we will redirect to this "action"
      if (!returnUrl.IsNullOrWhiteSpace() && this.Url.IsLocalUrl(returnUrl))
      {
        return this.Redirect(returnUrl);
      }

      // If we cannot verify if the url is local to our host we redirect to a default location
      return this.RedirectToAction("index", "home");
    }

    private void AddErrors(DbEntityValidationException exc)
    {
      foreach (var error in exc.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors.Select(validationError => validationError.ErrorMessage)))
      {
        this.ModelState.AddModelError("", error);
      }
    }

    private void AddErrors(IdentityResult result)
    {
      // Add all errors that were returned to the page error collection
      foreach (var error in result.Errors)
      {
        this.ModelState.AddModelError("", error);
      }
    }

    private void EnsureLoggedOut()
    {
      // If the request is (still) marked as authenticated we send the user to the logout action
      if (this.Request.IsAuthenticated)
      {
        this.Logout();
      }
    }
    private async Task SignInAsync(ApplicationUser user, bool isPersistent)
    {

      // Write the authentication cookie
      //FormsAuthentication.SetAuthCookie(( await this.SignInManager.CreateUserIdentityAsync(user) ).Name, isPersistent);
      var rememberBrowserIdentity =
               AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(user.Id);
      AuthenticationManager.SignIn(
          new AuthenticationProperties { IsPersistent = isPersistent },
        await user.GenerateUserIdentityAsync(this.UserManager),
          rememberBrowserIdentity);
    }

    [HttpGet]
    public ActionResult SetCulture(string lang)
    {
      switch (lang.Trim())
      {
        case "en":
          CultureInfo.CurrentCulture = new CultureInfo("en-US");
          CultureInfo.CurrentUICulture = new CultureInfo("en-US");
          break;
        case "cn":
          CultureInfo.CurrentCulture = new CultureInfo("zh-CN");
          CultureInfo.CurrentUICulture = new CultureInfo("zh-CN");
          break;
        case "tw":
          CultureInfo.CurrentCulture = new CultureInfo("zh-TW");
          CultureInfo.CurrentUICulture = new CultureInfo("zh-TW");
          break;
      }

      var cookie = new HttpCookie("culture", lang)
      {
        Expires = DateTime.Now.AddYears(1)
      };
      this.Response.Cookies.Add(cookie);
      return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }

    // GET: /account/lock
    [AllowAnonymous]
    public ActionResult Lock() => this.View();
  }
}