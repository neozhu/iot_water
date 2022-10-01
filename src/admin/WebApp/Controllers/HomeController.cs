using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using LazyCache;
using SqlSugar;

namespace WebApp.Controllers
{
  [Authorize]
  [RoutePrefix("Home")]
  public class HomeController : Controller
  {
    private readonly IAppCache cache;
    private readonly IMapper mapper;
    private readonly ISqlSugarClient db;
    private readonly NLog.ILogger logger;
 

    public HomeController(
      NLog.ILogger logger,
      IAppCache cache, IMapper mapper,
      ISqlSugarClient db) {

      this.cache = cache;
      this.mapper = mapper;
      this.db = db;
      this.logger = logger;

  
    }

    public async Task<ActionResult> Index()
    {
      //await  WaterApi.findQueryDetailDataByDatetime();
      //await WaterApi.UpdateQuota();
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var date1 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
      var date2= DateTime.Now.AddDays(-2).Date.ToString("yyyy-MM-dd");
      var hour = DateTime.Now.Hour;
      var bydatesql1 = @"select isnull(sum(water),0) from  [dbo].[summarywaterbyhour] 
  where date = @date ";
      var bymonthsql2 = @"SELECT isnull(sum(water),0)
  FROM  [dbo].[summarywaterbydate]
  where date >=@date1 and date <=@date2";

      var result1 = await this.cache.GetOrAddAsync("result1", async () =>
      {
        var result = await this.db.Ado.SqlQueryAsync<decimal>(bydatesql1, new { date = date1, hour = hour });
        return result;
      },new DateTimeOffset(DateTime.Now.AddHours(12)));
      var result2 = await this.cache.GetOrAddAsync("result2", async () =>
      {
        var result = await this.db.Ado.SqlQueryAsync<decimal>(bydatesql1, new { date = date2, hour = hour });
        return result;
      }, new DateTimeOffset(DateTime.Now.AddHours(12)));

      var day1= result1[0];
      var day2 = result2[0];

     
      var dayRatio = 0m;
      if (day2 > 0)
      {
        dayRatio = ( day1 - day2 ) / day2 * 100;
      }
      var m1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      var m2 = DateTime.Now;
      var result3 = await this.cache.GetOrAddAsync("result3", async () =>
      {
        return await this.db.Ado.SqlQueryAsync<decimal>(bymonthsql2, new { date1 = m1.ToString("yyyy-MM-dd"), date2 = m2.ToString("yyyy-MM-dd") });
      }
      , new DateTimeOffset(DateTime.Now.AddHours(24))
      );
      var result4 = await this.cache.GetOrAddAsync("result4", async () =>
      {
        return await this.db.Ado.SqlQueryAsync<decimal>(bymonthsql2, new { date1 = m1.AddMonths(-1).ToString("yyyy-MM-dd"), date2 = m2.AddMonths(-1).ToString("yyyy-MM-dd") });

      }, new DateTimeOffset(DateTime.Now.AddHours(24)));
      var month1 = result3[0];
      var month2 = result4[0];
      var monthRatio = 0m;
      if (month2 > 0)
      {
        monthRatio = ( month1 - month2 ) / month2 * 100;
      }


      var y1 = new DateTime(DateTime.Now.Year, 1, 1);
      var y2 = DateTime.Now;
      var result5 = await this.cache.GetOrAddAsync("result5", async () =>
      {
        return await this.db.Ado.SqlQueryAsync<decimal>(bymonthsql2, new { date1 = y1.ToString("yyyy-MM-dd"), date2 = y2.ToString("yyyy-MM-dd") });
      }, new DateTimeOffset(DateTime.Now.AddHours(24)));

      var result6 = await this.cache.GetOrAddAsync("result6", async () =>
      {
        return await this.db.Ado.SqlQueryAsync<decimal>(bymonthsql2, new { date1 = y1.AddYears(-1).ToString("yyyy-MM-dd"), date2 = y2.AddYears(-1).ToString("yyyy-MM-dd") });
      }, new DateTimeOffset(DateTime.Now.AddHours(24)));
          var year1 = result5[0];
      var year2 = result6[0];
      var yearRatio = 0m;
      if (year2 > 0)
      {
        yearRatio = ( year1 - year2 ) / year2 * 100;
      }


      ViewBag.Day1 = day1;
      ViewBag.Day2 = day2;
      ViewBag.DayRatio = dayRatio;

      ViewBag.Month1 = month1;
      ViewBag.Month2 = month2;
      ViewBag.MonthRatio = monthRatio;

      ViewBag.Year1 = year1;
      ViewBag.Year2 = year2;
      ViewBag.YearRatio = yearRatio;

      return this.View();
    }

    public ActionResult About()
    {
      this.ViewBag.Message = "Your application description page.";

      return this.View();
    }

    public ActionResult GetTime() =>
        //ViewBag.Message = "Your application description page.";

        this.View();
    public ActionResult BlankPage() => this.View();
    public ActionResult AgileBoard() => this.View();


    public ActionResult Contact()
    {
      this.ViewBag.Message = "Your contact page.";

      return this.View();
    }
    public ActionResult Chat() => this.View();




  }
}