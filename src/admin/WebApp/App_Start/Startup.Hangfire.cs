using System;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using WebApp.App_Helpers.third_party.api;

namespace WebApp
{
  public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
  {
    public bool Authorize([NotNull] DashboardContext context)
    {
      // In case you need an OWIN context, use the next line, `OwinContext` class
      // is the part of the `Microsoft.Owin` package.
      var owinContext = new OwinContext(context.GetOwinEnvironment());

      // Allow all authenticated users to see the Dashboard (potentially dangerous).
      return owinContext.Authentication.User.Identity.IsAuthenticated;
    }
  }

  public partial class Startup
  {
    // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
    public void ConfigureHangfire(IAppBuilder app)
    {
      GlobalConfiguration.Configuration
         .UseSimpleAssemblyNameTypeSerializer()
         .UseRecommendedSerializerSettings()
         .UseColouredConsoleLogProvider()
         .UseNLogLogProvider()
         .UseSqlServerStorage(
              "DefaultConnection",
              new SqlServerStorageOptions
              {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                UsePageLocksOnDequeue = true,
                DisableGlobalLocks = true,
                PrepareSchemaIfNecessary = false, // Default value: true
                EnableHeavyMigrations = false      // Default value: false
              });



      app.UseHangfireDashboard("/hangfire", new DashboardOptions()
      {
        Authorization = new[] { new HangFireAuthorizationFilter() }
      });
      app.UseHangfireServer();
      //每10分钟执行一个方法
      //RecurringJob.AddOrUpdate(
      //         () => WaterApi.findQueryDetailDataByDatetime(),
      //     Cron.Daily(2,0));
      //RecurringJob.AddOrUpdate(
      //        () => WaterApi.UpdateQuota(),
      //    Cron.Daily(3,30));
    }

    //public void ExecuteProcess() => Console.WriteLine("{ DateTime.Now }:do something......");
  }
}