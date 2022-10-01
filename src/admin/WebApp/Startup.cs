using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{

  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      this.ConfigureAuth(app);
      this.ConfigureHangfire(app);
      //var config = new HubConfiguration()
      //{
      //  Resolver = new HubDependencyResolver(HubUnityConfig.Container)
      //};
      //var unityHubActivator = new UnityHubActivator(HubUnityConfig.Container);
      //GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => unityHubActivator);
      //app.MapSignalR();
    
    }
  }
}
