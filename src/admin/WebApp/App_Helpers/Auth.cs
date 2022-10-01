using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LazyCache;
using WebApp.Models;

namespace WebApp
{
  public static class Auth
  {
    private static readonly IAppCache cache = new CachingService();
    private static readonly SqlSugar.ISqlSugarClient db = SqlSugarFactory.CreateSqlSugarClient();
    /// <summary>
    /// 获取当前登录用户名
    /// </summary>
    public static string CurrentUserName
    {

      get
      {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
          return HttpContext.Current.User.Identity.Name;
        }
        else
        {
          return "Unauthenticated";
        }
      }
    }

    public static string GetCompanyCode(string username = null) => cache.GetOrAdd($"Identity.CompanyCode.{username ?? HttpContext.Current.User.Identity.Name}", () =>
    {
      username = string.IsNullOrEmpty(username) ? HttpContext.Current.User.Identity.Name : username;
      var companyCode = db.Ado.GetString("select [CompanyCode] from [dbo].[AspNetUsers] where [username]=@username", new { username });
      return companyCode;
    });
    public static string GetCompanyName(string username = null) => cache.GetOrAdd($"Identity.CompanyName.{username ?? HttpContext.Current.User.Identity.Name}", () =>
    {
      username = string.IsNullOrEmpty(username) ? HttpContext.Current.User.Identity.Name : username;
      var companyCode = db.Ado.GetString("select [CompanyName] from [dbo].[AspNetUsers] where [username]=@username", new { username });
      return companyCode;
    });
    public static ApplicationUser CurrentApplicationUser
    {
      get
      {
        var username = HttpContext.Current.User.Identity.Name;
        var user = db.Ado.SqlQuery<ApplicationUser>("select * from [dbo].[AspNetUsers] where [username]=@username", new { username });
        return user.FirstOrDefault();
      }
    }

    public static string GetFullName(string username = null) => cache.GetOrAdd($"Identity.FullName.{username ?? HttpContext.Current.User.Identity.Name}", () =>
    {
      username = string.IsNullOrEmpty(username) ? HttpContext.Current.User.Identity.Name : username;
      var userid = db.Ado.GetString("select [fullname] from [dbo].[AspNetUsers] where [username]=@username", new { username });
      return userid;
    });
    public static int GetTenantId(string username = null) => cache.GetOrAdd($"Identity.TenantId.{username ?? HttpContext.Current.User.Identity.Name}", () =>
    {
      username = string.IsNullOrEmpty(username) ? HttpContext.Current.User.Identity.Name : username;
      var tenantid = db.Ado.GetInt("select [TenantId] from [dbo].[AspNetUsers] where [username]=@username", new { username });
      return tenantid;
    });
    public static string GetUserId(string username = null) => cache.GetOrAdd($"Identity.Id.{username ?? HttpContext.Current.User.Identity.Name}", () =>
    {
      username = string.IsNullOrEmpty(username) ? HttpContext.Current.User.Identity.Name : username;
      var userid = db.Ado.GetString("select [id] from [dbo].[AspNetUsers] where [username]=@username", new { username });
      return userid;
    });
    public static async Task SetOnlineAsync(string id)
    {
      var sql = "update [dbo].[AspNetUsers]  set [IsOnline]=1 where [Id]=@id";
      await db.Ado.ExecuteCommandAsync(sql, new { id = id });
    }
    public static async Task SetOfflineAsync(string id)
    {
      var sql = "update [dbo].[AspNetUsers]  set [IsOnline]=0 where [Id]=@id";
      await db.Ado.ExecuteCommandAsync(sql, new { id = id });
    }
  
  }
}