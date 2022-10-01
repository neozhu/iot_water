using System.Linq;
using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.Ajax.Utilities;
using WebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq.Expressions;
using System.Globalization;


namespace WebApp
{
  public static class HtmlHelperExtensions
  {
    private static string _displayVersion = string.Empty;
    /// <summary>
    /// 多语言切换
    /// </summary>
    /// <param name="helper"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static HtmlString L(this HtmlHelper helper, string name)
    {

      var resource = new System.Resources.ResourceManager(typeof(WebApp.resource.Global));
      var text = resource.GetString(name);
      return new HtmlString(text ?? name);
    }
    public static HtmlString LangScriptTag(this HtmlHelper helper, string defaultsrc)
    {
      var src = defaultsrc;
      var lang = CultureInfo.CurrentCulture.Name;
      switch (lang)
      {
        case "en-US":
          src = "/Scripts/easyui/locale/easyui-lang-en.js";
          break;
        case "zh-CN":
          src = "/Scripts/easyui/locale/easyui-lang-zh_CN.js";
          break;
        case "zh-TW":
          src = "/Scripts/easyui/locale/easyui-lang-zh_TW.js";
          break;
        default:
          src = defaultsrc;
          break;
      }
      return new HtmlString($"<script src=\"{ src }\"></script>");
    }
    /// <summary>
    ///     Retrieves a non-HTML encoded string containing the assembly version as a formatted string.
    ///     <para>If a project name is specified in the application configuration settings it will be prefixed to this value.</para>
    ///     <para>
    ///         e.g.
    ///         <code>1.0 (build 100)</code>
    ///     </para>
    ///     <para>
    ///         e.g.
    ///         <code>ProjectName 1.0 (build 100)</code>
    ///     </para>
    /// </summary>
    /// <param name="helper"></param>
    /// <returns></returns>
    public static IHtmlString AssemblyVersion(this HtmlHelper helper)
    {
      if (_displayVersion.IsNullOrWhiteSpace())
      {
        SetDisplayVersion();
      }

      return helper.Raw(_displayVersion);
    }

    /// <summary>
    ///     Compares the requested route with the given <paramref name="value" /> value, if a match is found the
    ///     <paramref name="attribute" /> value is returned.
    /// </summary>
    /// <param name="helper"></param>
    /// <param name="value">The action value to compare to the requested route action.</param>
    /// <param name="attribute">The attribute value to return in the current action matches the given action value.</param>
    /// <returns>A HtmlString containing the given attribute value; otherwise an empty string.</returns>
    public static IHtmlString RouteIf(this HtmlHelper helper, string[] values, string attribute)
    {
      //var currentController =
      //    ( helper.ViewContext.RequestContext.RouteData.Values["controller"] ?? string.Empty ).ToString().UnDash();
      //var currentAction =
      //    ( helper.ViewContext.RequestContext.RouteData.Values["action"] ?? string.Empty ).ToString().UnDash();
      var url = helper.ViewContext.HttpContext.Request.Url.LocalPath;
      var exist = values.Contains(url);
      //var currentController =
      //   ( helper.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["controller"] ?? string.Empty ).ToString().UnDash();
      //var currentAction =
      //    ( helper.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["action"] ?? string.Empty ).ToString().UnDash();

      //var hasController = values.Contains(currentController, StringComparer.InvariantCultureIgnoreCase);
      //var hasAction = values.Contains(currentAction, StringComparer.InvariantCultureIgnoreCase);
      return exist ? new HtmlString(attribute) : new HtmlString(string.Empty);
      //return hasAction || hasController ? new HtmlString(attribute) : new HtmlString(string.Empty);
    }

    /// <summary>
    ///     Renders the specified partial view with the parent's view data and model if the given setting entry is found and
    ///     represents the equivalent of true.
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="partialViewName">The name of the partial view.</param>
    /// <param name="appSetting">The key value of the entry point to look for.</param>
    public static void RenderPartialIf(this HtmlHelper htmlHelper, string partialViewName, string appSetting)
    {
      var setting = Settings.GetValue<bool>(appSetting);

      htmlHelper.RenderPartialIf(partialViewName, setting);
    }

    /// <summary>
    ///     Renders the specified partial view with the parent's view data and model if the given setting entry is found and
    ///     represents the equivalent of true.
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="partialViewName">The name of the partial view.</param>
    /// <param name="condition">The boolean value that determines if the partial view should be rendered.</param>
    public static void RenderPartialIf(this HtmlHelper htmlHelper, string partialViewName, bool condition)
    {
      if (!condition)
      {
        return;
      }

      htmlHelper.RenderPartial(partialViewName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="html"></param>
    /// <param name="controller"></param>
    /// <param name="action"></param>
    /// <param name="cssClass"></param>
    /// <returns></returns>
    public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
    {

      if (string.IsNullOrEmpty(cssClass))
      {
        cssClass = "active";
      }

      var currentAction = (string)html.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["action"];
      var currentController = (string)html.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["controller"];

      if (string.IsNullOrEmpty(controller))
      {
        controller = currentController;
      }

      //if (string.IsNullOrEmpty(action))
      //{
      //  action = currentAction;
      //} 

      var ctrs = controller.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      if (ctrs.Length >= 1 && string.IsNullOrEmpty(action))
      {
        return ctrs.Contains(currentController, StringComparer.OrdinalIgnoreCase) ? cssClass : string.Empty;
      }
      else
      {
        return controller == currentController && action.Equals(currentAction, StringComparison.OrdinalIgnoreCase) ? cssClass : string.Empty;
      }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="html"></param>
    /// <param name="menu"></param>
    /// <returns></returns>
    public static bool IsAuthorize(this HtmlHelper html, string menu)
    {
      var userid = html.ViewContext.HttpContext.User.Identity.GetUserId();
      var currentAction = (string)html.ViewContext.RouteData.Values["action"];
      var currentController = (string)html.ViewContext.RouteData.Values["controller"];
      var key = userid + currentAction + currentController;
      // var data= html.ViewContext.HttpContext.Cache.Data<IList<WebApp.Models.RoleMenu>>(key,10000,()=>{
      var rolemanager = html.ViewContext.HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
      var usermanager = html.ViewContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

      var roles = usermanager.GetRoles(userid);
      var db = new StoreContext();
      var authorize = db.RoleMenus.Where(x => roles.Contains(x.RoleName) && x.MenuItem.Action == currentAction && x.MenuItem.Controller == currentController).ToList();
      //return authorize;
      //});
      var data = authorize;
      switch (menu)
      {
        case "Create":
          return data.Where(x => x.Create == true).Any();
        case "Edit":
          return data.Where(x => x.Edit == true).Any();
        case "Delete":
          return data.Where(x => x.Delete == true).Any();
        case "Import":
          return data.Where(x => x.Import == true).Any();
        default:
          return false;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="html"></param>
    /// <param name="linkText"></param>
    /// <param name="action"></param>
    /// <param name="controllerName"></param>
    /// <param name="iconClass"></param>
    /// <returns></returns>
    public static MvcHtmlString ActionButton(this HtmlHelper html, string linkText, string action, string controllerName, string iconClass)
    {
      //<a href="/@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText"><i class="@lLink.IconClass"></i><span class="">@lLink.LinkText</span></a>
      var lURL = new UrlHelper(html.ViewContext.RequestContext);

      // build the <span class="">@lLink.LinkText</span> tag
      var lSpanBuilder = new TagBuilder("span");
      lSpanBuilder.MergeAttribute("class", "");
      lSpanBuilder.SetInnerText(linkText);
      var lSpanHtml = lSpanBuilder.ToString(TagRenderMode.Normal);

      // build the <i class="@lLink.IconClass"></i> tag
      var lIconBuilder = new TagBuilder("i");
      lIconBuilder.MergeAttribute("class", iconClass);
      var lIconHtml = lIconBuilder.ToString(TagRenderMode.Normal);

      // build the <a href="@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText">...</a> tag
      var lAnchorBuilder = new TagBuilder("a");
      lAnchorBuilder.MergeAttribute("href", lURL.Action(action, controllerName));
      lAnchorBuilder.InnerHtml = lIconHtml + lSpanHtml; // include the <i> and <span> tags inside
      var lAnchorHtml = lAnchorBuilder.ToString(TagRenderMode.Normal);

      return MvcHtmlString.Create(lAnchorHtml);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string PageClass(this HtmlHelper html)
    {
      var currentAction = (string)html.ViewContext.RouteData.Values["action"];
      return currentAction;
    }


    /// <summary>
    ///     Retrieves a non-HTML encoded string containing the assembly version and the application copyright as a formatted
    ///     string.
    ///     <para>If a company name is specified in the application configuration settings it will be suffixed to this value.</para>
    ///     <para>
    ///         e.g.
    ///         <code>1.0 (build 100) © 2015</code>
    ///     </para>
    ///     <para>
    ///         e.g.
    ///         <code>1.0 (build 100) © 2015 CompanyName</code>
    ///     </para>
    /// </summary>
    /// <param name="helper"></param>
    /// <returns></returns>
    public static IHtmlString Copyright(this HtmlHelper helper)
    {
      var copyright =
          string.Format("{0} &copy; {1} {2}", helper.AssemblyVersion(), DateTime.Now.Year, Settings.Company)
              .Trim();

      return helper.Raw(copyright);
    }

    private static void SetDisplayVersion()
    {
      var version = Assembly.GetExecutingAssembly().GetName().Version;

      _displayVersion =
          string.Format("{4} {0}.{1}.{2} (build {3})", version.Major, version.Minor, version.Build,
              version.Revision, Settings.Project).Trim();
    }

    /// <summary>
    ///     Returns an unordered list (ul element) of validation messages that utilizes bootstrap markup and styling.
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="alertType">The alert type styling rule to apply to the summary element.</param>
    /// <param name="heading">The optional value for the heading of the summary element.</param>
    /// <returns></returns>
    public static HtmlString ValidationBootstrap(this HtmlHelper htmlHelper, string alertType = "primary",
        string heading = "")
    {
      if (htmlHelper.ViewData.ModelState.IsValid)
      {
        return new HtmlString(string.Empty);
      }

      var sb = new StringBuilder();

      sb.AppendFormat("<div class=\"alert alert-{0} text-dark validation-summary-errors\" data-valmsg-summary=\"true\">", alertType);
      if (!heading.IsNullOrWhiteSpace())
      {
        sb.AppendFormat("<span class=\"h4\">{0}</span>", heading);
      }
      sb.Append("<ul>");
      foreach (var modelError in htmlHelper.ViewData.ModelState.SelectMany(keyValuePair => keyValuePair.Value.Errors))
      {
        sb.AppendFormat("<li>{0}</li>", modelError.ErrorMessage);
      }
      sb.Append("</ul>");
      sb.Append("</div>");

      return new HtmlString(sb.ToString());
    }


    /// <summary>
    /// 输出 Description
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
      var description = metadata.Description;
      return MvcHtmlString.Create(description);
    }



  }
}