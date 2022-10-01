using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApp.Models;
using Unity;
using Microsoft.AspNet.SignalR;

namespace WebApp
{
  public class MvcApplication : System.Web.HttpApplication
  {
    private readonly NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
    protected void Application_Start()
    {
      // Make long polling connections wait a maximum of 110 seconds for a
      // response. When that time expires, trigger a timeout command and
      // make the client reconnect.
      GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

      // Wait a maximum of 30 seconds after a transport connection is lost
      // before raising the Disconnected event to terminate the SignalR connection.
      GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);

      // For transports other than long polling, send a keepalive packet every
      // 10 seconds. 
      // This value must be no more than 1/3 of the DisconnectTimeout value.
      GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);

 

      //AreaRegistration.RegisterAllAreas();
      GlobalConfiguration.Configure(WebApiConfig.Register);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
      this.logger.Info("网站启动");
   
    }
    protected void Application_Error()
    {
      var exception = this.Server.GetLastError();

       this.logger.Fatal(exception, exception.GetBaseException().Message);

       
    }
  }
}
