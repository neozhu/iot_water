using System.Security.Claims;
using System.Web.Mvc;

namespace WebApp
{
  public class ViewBagFilter : IActionFilter
  {
    private static readonly string Enabled = "Enabled";
    private static readonly string Disabled = string.Empty;

    public void OnActionExecuting(ActionExecutingContext context)
    {
      if (context.Controller is Controller controller)
      {
        var claimsidentity = (ClaimsIdentity)controller.User.Identity;
        var username = claimsidentity.FindFirst(ClaimTypes.Name)?.Value;
        var givenname = claimsidentity.FindFirst(ClaimTypes.GivenName)?.Value;
        var email = claimsidentity.FindFirst(ClaimTypes.Email)?.Value;
        var mobilephone = claimsidentity.FindFirst(ClaimTypes.MobilePhone)?.Value;
        var avatars = claimsidentity.FindFirst("http://schemas.microsoft.com/identity/claims/avatars")?.Value;
        var tenantid = claimsidentity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
        var tenantname = claimsidentity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantname")?.Value;
        var tenantdb = claimsidentity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantdb")?.Value;
        var culture = claimsidentity.FindFirst(ClaimTypes.Country)?.Value;
        var role = claimsidentity.FindFirst(ClaimTypes.Role)?.Value;
        // SmartAdmin Toggle Features
        controller.ViewBag.AppSidebar = Enabled;
        controller.ViewBag.AppHeader = Enabled;
        controller.ViewBag.AppLayoutShortcut = Enabled;
        controller.ViewBag.AppFooter = Enabled;
        controller.ViewBag.ShortcutMenu = Enabled;
        controller.ViewBag.ChatInterface = Enabled;
        controller.ViewBag.LayoutSettings = Enabled;

        // SmartAdmin Default Settings
        controller.ViewBag.App = "Water";
        controller.ViewBag.AppName = Settings.Project;
        controller.ViewBag.AppFlavor = "";
        controller.ViewBag.AppFlavorSubscript = "";
        controller.ViewBag.User = username;
        controller.ViewBag.Role = role;
        controller.ViewBag.Email = email;
        controller.ViewBag.MobilePhone = mobilephone;
        controller.ViewBag.Twitter = email;
        controller.ViewBag.Avatar = "ng.jpg";
        controller.ViewBag.FullName = givenname;
        controller.ViewBag.TenantId = tenantid;
        controller.ViewBag.TenantName = tenantname;
        controller.ViewBag.TenantDb = tenantdb;
        controller.ViewBag.Version = "4.0.2";
        controller.ViewBag.Bs4v = "4.3";
        controller.ViewBag.Logo = "logo.png";
        controller.ViewBag.LogoM = "logo.png";
        //备案信息 <a href='http://beian.miit.gov.cn/' class='text-primary fw-500' title='gotbootstrap.com' target='_blank'>工业和信息化部备案管理系统网站 粤ICP备00000000号 00000000000000</a>
        controller.ViewBag.Copyright = $"2020 © 智慧水务综合管理平台 &nbsp; {Settings.Company} ";
        controller.ViewBag.CopyrightInverse = $"2020 © 智慧水务综合管理平台 &nbsp;{Settings.Company} ";
      }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
  }
}
