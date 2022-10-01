using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using LazyCache;
using OfficeOpenXml;
using SqlSugar;
using WebApp.Models.JsonModel;


namespace WebApp.Controllers.api
{
  
  [System.Web.Mvc.Authorize]

  public class WaterChartController : Controller
  {
    private readonly IAppCache cache;
    private readonly IMapper mapper;
    private readonly NLog.ILogger logger;
    private readonly ISqlSugarClient db;
    public WaterChartController(NLog.ILogger logger,
      IAppCache cache, IMapper mapper)
    {

      this.cache = cache;
      this.mapper = mapper;
      this.logger = logger;
      this.db = SqlSugarFactory.CreateSqlSugarClient();
    }
    //获取环比水表用水量
    public async Task<IEnumerable<ChainWaterMeterDataModel>> GetChainMeterData(string meterid)
    {
      var sql = @"select  meterId,[year],[Month],sum(water) [water] from [dbo].[CustomerWaterRecords]
            where meterId=@meterid
            group by meterId,[year],[Month]
            order by meterId,[year],[Month]";
      var result = await this.db.Ado.SqlQueryAsync<ChainWaterMeterDataModel>(sql, new { meterid });
      return result;
    }
    //获取环比单位用水量
    public async Task<IEnumerable<ChainCustomerWaterMeterDataModel>> GetChainCustomerMeterData(string customerid)
    {
      var sql = @"select  CustomerId [customer],[year],[Month],sum(water) [water] from [dbo].[CustomerWaterRecords]
            where CustomerId=@customerid
            group by CustomerId,[year],[Month]
            order by CustomerId,[year],[Month]";
      var result = await this.db.Ado.SqlQueryAsync<ChainCustomerWaterMeterDataModel>(sql, new { customerid });
      return result;
    }
    //获取单一水表汇总数据 20分钟
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByMinute(string meterid,DateTime dt1, DateTime dt2) {
      var sql = @"select meterId,[datetime],[increment] from [dbo].[waterrecordincrement]
      where meterId=@meterid and [datetime]>=@dt1 and  [datetime]<=@dt2";
      var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid, dt1, dt2 });
      return result;
    }
    public async Task<JsonResult> GetSMeterDataByMinute(string meterid, DateTime dt1, DateTime dt2)
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var array = meterid.Split(',');
      var sql = @"select [datetime],[increment] from [dbo].[waterrecordincrement]
      where tenantid=@tenantid and meterId in (@meterid) and [datetime]>=@dt1 and  [datetime]<=@dt2
      -- group by [datetime]
      order by [datetime]
      ";
      var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid= array, dt1, dt2 , tenantid });
      return Json(result.Select(x=>new { datetime=x.datetime.ToString("yyyy-MM-dd HH:mm:ss"),x.increment}), JsonRequestBehavior.AllowGet);
    }
    //[HttpPost]
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByMinuteIn(string meterid, DateTime dt1, DateTime dt2)
    {
      var array = meterid.Split(',');
      var sql = $@"select meterId,[datetime],[increment] from [dbo].[waterrecordincrement]
      where meterId in({array.ToJoinSqlInVals()}) and [datetime]>=@dt1 and  [datetime]<=@dt2";
      var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { dt1, dt2 });
      return result;
    }
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByMinute(string customerid, DateTime dt1, DateTime dt2)
    {
      //      var sql = @"select t2.customerId,t1.[datetime],sum(t1.[increment]) [increment]
      //from [dbo].[waterrecordincrement] t1
      //inner join [dbo].[CustomerWaterMeters]  t2 on t1.meterId=t2.meterId
      //where t2.[CustomerId]=@customerid and t1.[datetime]>=@dt1 and t1.[datetime]<=@dt2
      //group by t2.CustomerId,t1.[datetime]
      //order by t2.CustomerId,t1.[datetime]";
      var sql = @"select min(customerName) [customerId],[datetime],sum(increment)  [increment]
from [dbo].[customerwaterrecordincrement]
where [customerId]=@customerid and [datetime]>=@dt1 and [datetime]<=@dt2
group by CustomerId,[datetime]
order by CustomerId,[datetime]";
      var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new { customerid, dt1, dt2 });
      return result;
    }
    //[HttpPost]
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByMinuteIn(string customerid, DateTime dt1, DateTime dt2)
    {
      var array = customerid.Split(',');
      //      var sql = $@"select t2.CustomerName as [customerId],t1.[datetime],sum(t1.[increment]) [increment]
      //from [dbo].[waterrecordincrement] t1
      //inner join [dbo].[CustomerWaterMeters]  t2 on t1.meterId=t2.meterId
      //where t2.[CustomerId] in ({array.ToJoinSqlInVals()}) and t1.[datetime]>=@dt1 and t1.[datetime]<=@dt2
      //group by t2.CustomerName,t1.[datetime]
      //order by t2.CustomerName,t1.[datetime]";
      var sql = $@"select min(customerName) [customerId],[datetime],sum(increment)  [increment]
from [dbo].[customerwaterrecordincrement]
where [customerId] in ({array.ToJoinSqlInVals()}) and [datetime]>=@dt1 and [datetime]<=@dt2
group by CustomerId,[datetime]
order by CustomerId,[datetime]";
      var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new { dt1, dt2 });
      return result;
    }
    //获取单一水表汇总数据 1小时
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByHour(string meterid, DateTime dt1, DateTime dt2)
    {
      try
      {
        var sql = @"select 
meterId,
CAST((CAST([date] as varchar) + ' ' + CAST([hour] as varchar) +':00') as datetime)    [datetime],
[increment]
from (
select meterId,
CAST([datetime] as date) [date],
DATEPART(Hour, [datetime]) as [hour],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where meterId=@meterid and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY meterId,CAST([datetime] as date),
       DATEPART(HOUR,[datetime])
) t
order by t.meterId,t.[date],t.[hour]";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid, dt1, dt2 });
        return result;
      }
      catch (Exception e) {
        throw e;
        }
    }
    public async Task<JsonResult> GetSMeterDataByHour(string meterid, DateTime dt1, DateTime dt2)
    {
      try
      {
        var tenantid = Auth.GetTenantId(this.User.Identity.Name);
        var array = meterid.Split(',');
        var sql = @"select 

CAST((CAST([date] as varchar) + ' ' + CAST([hour] as varchar) +':00') as datetime)    [datetime],
[increment]
from (
select 
CAST([datetime] as date) [date],
DATEPART(Hour, [datetime]) as [hour],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where tenantid=@tenantid and  meterId in (@meterid) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY CAST([datetime] as date),
       DATEPART(HOUR,[datetime])
) t
order by t.[date],t.[hour]";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid=array, dt1, dt2, tenantid });
        return Json(result.Select(x=>new { datetime=x.datetime.ToString("yyyy-MM-dd HH:mm:ss"),x.increment,x.meterId }),JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //[HttpPost]
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByHourIn(string meterid, DateTime dt1, DateTime dt2)
    {
      var array = meterid.Split(',');
      try
      {
        var sql = $@"select 
meterId,
CAST((CAST([date] as varchar) + ' ' + CAST([hour] as varchar) +':00') as datetime)    [datetime],
[increment]
from (
select meterId,
CAST([datetime] as date) [date],
DATEPART(Hour, [datetime]) as [hour],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where meterId in ({array.ToJoinSqlInVals()}) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY meterId,CAST([datetime] as date),
       DATEPART(HOUR,[datetime])
) t
order by t.meterId,t.[date],t.[hour]";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new {  dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByHour(string customerid, DateTime dt1, DateTime dt2)
    {
      try
      {
        //        var sql = @"select 
        //customerId,
        //CAST((CAST([date] as varchar) + ' ' + CAST([hour] as varchar) +':00') as datetime)    [datetime],
        //[increment]
        //from (
        //select t2.CustomerId,
        //CAST(t1.[datetime] as date) [date],
        //DATEPART(Hour, t1.[datetime]) as [hour],
        //sum(t1.[increment]) [increment] from [dbo].[waterrecordincrement] t1
        //inner join [dbo].[CustomerWaterMeters] t2 on t1.meterId=t2.meterId
        //where t2.CustomerId=@customerid and t1.[datetime]>=@dt1 and  t1.[datetime]<=@dt2
        //GROUP BY t2.CustomerId,CAST(t1.[datetime] as date),
        //       DATEPART(HOUR,t1.[datetime])
        //) t
        //order by t.CustomerId,t.[date],t.[hour]";
        var sql = @"select 
customerId,
CAST((CAST([date] as varchar) + ' ' + CAST([hour] as varchar) +':00') as datetime)    [datetime],
[increment]
from (
select min(CustomerName) CustomerId,
CAST([datetime] as date) [date],
DATEPART(Hour, [datetime]) as [hour],
sum([increment]) [increment] from [dbo].[customerwaterrecordincrement] 
where customerId = @customerid and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY CustomerId,CAST([datetime] as date),
       DATEPART(HOUR,[datetime])
) t
order by t.customerId,t.[date],t.[hour]";
        var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new { customerid, dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //[HttpPost]
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByHourIn(string customerid, DateTime dt1, DateTime dt2)
    {
      var array = customerid.Split(',');
      try
      {
        //        var sql = $@"select 
        //CustomerName as [customerId],
        //CAST((CAST([date] as varchar) + ' ' + CAST([hour] as varchar) +':00') as datetime)    [datetime],
        //[increment]
        //from (
        //select t2.CustomerName,
        //CAST(t1.[datetime] as date) [date],
        //DATEPART(Hour, t1.[datetime]) as [hour],
        //sum(t1.[increment]) [increment] from [dbo].[waterrecordincrement] t1
        //inner join [dbo].[CustomerWaterMeters] t2 on t1.meterId=t2.meterId
        //where t2.CustomerId in ({array.ToJoinSqlInVals()}) and t1.[datetime]>=@dt1 and  t1.[datetime]<=@dt2
        //GROUP BY t2.CustomerName,CAST(t1.[datetime] as date),
        //       DATEPART(HOUR,t1.[datetime])
        //) t
        //order by t.CustomerName,t.[date],t.[hour]";
        var sql = $@"select 
customerId,
CAST((CAST([date] as varchar) + ' ' + CAST([hour] as varchar) +':00') as datetime)    [datetime],
[increment]
from (
select min(CustomerName) CustomerId,
CAST([datetime] as date) [date],
DATEPART(Hour, [datetime]) as [hour],
sum([increment]) [increment] from [dbo].[customerwaterrecordincrement] 
where customerId in({array.ToJoinSqlInVals()}) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY CustomerId,CAST([datetime] as date),
       DATEPART(HOUR,[datetime])
) t
order by t.customerId,t.[date],t.[hour]";
        var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new {  dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //获取单一水表汇总数据 天
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByDay(string meterid, DateTime dt1, DateTime dt2)
    {
      try
      {
        var sql = @"select meterId,
CAST([datetime] as date) [datetime],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where meterId=@meterid and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY meterId,CAST([datetime] as date)
order by  meterId,CAST([datetime] as date) ";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid, dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<JsonResult> GetSMeterDataByDay(string meterid, DateTime dt1, DateTime dt2)
    {
      try
      {
        var tenantid = Auth.GetTenantId(this.User.Identity.Name);
        var array = meterid.Split(',');
        var sql = @"select 
CAST([datetime] as date) [datetime],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where tenantid=@tenantid and meterId In (@meterid) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY CAST([datetime] as date)
order by CAST([datetime] as date) ";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid= array, dt1, dt2, tenantid });
        return Json(result.Select(x=>new { datetime=x.datetime.ToString("yyyy-MM-dd HH:mm:ss"),x.increment}),JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //[HttpPost]
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByDayIn(string meterid, DateTime dt1, DateTime dt2)
    {
      var array = meterid.Split(',');
      try
      {
        var sql = $@"select meterId,
CAST([datetime] as date) [datetime],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where meterId in ({array.ToJoinSqlInVals()}) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY meterId,CAST([datetime] as date)
order by  meterId,CAST([datetime] as date) ";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new {  dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByDay(string customerid, DateTime dt1, DateTime dt2)
    {
      try
      {
        //        var sql = @"select t2.customerId,
        //CAST(t1.[datetime] as date) [datetime],
        //sum(t1.[increment]) [increment] from [dbo].[waterrecordincrement]  t1
        //inner join [dbo].[CustomerWaterMeters] t2 on t1.meterId=t2.meterId
        //where t2.customerId=@customerid and t1.[datetime]>=@dt1 and  t1.[datetime]<=@dt2
        //GROUP BY t2.customerId,CAST(t1.[datetime] as date)
        //order by  t2.customerId,CAST(t1.[datetime] as date) ";
        var sql = @"select min(CustomerName) customerId,
CAST([datetime] as date) [datetime],
sum([increment]) [increment] from [dbo].[customerwaterrecordincrement] 
where customerId=@customerid and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY customerId,CAST([datetime] as date)
order by  customerId,CAST([datetime] as date) ";
        var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new { customerid, dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //[HttpPost]
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByDayIn(string customerid, DateTime dt1, DateTime dt2)
    {
      var array = customerid.Split(',');
      try
      {
        //        var sql = $@"select t2.customerName as [customerId],
        //CAST(t1.[datetime] as date) [datetime],
        //sum(t1.[increment]) [increment] from [dbo].[waterrecordincrement]  t1
        //inner join [dbo].[CustomerWaterMeters] t2 on t1.meterId=t2.meterId
        //where t2.customerId in ({array.ToJoinSqlInVals()}) and t1.[datetime]>=@dt1 and  t1.[datetime]<=@dt2
        //GROUP BY t2.customerName,CAST(t1.[datetime] as date)
        //order by  t2.customerName,CAST(t1.[datetime] as date) ";
        var sql = $@"select min(CustomerName) customerId,
CAST([datetime] as date) [datetime],
sum([increment]) [increment] from [dbo].[customerwaterrecordincrement] 
where customerId in ({array.ToJoinSqlInVals()}) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY customerId,CAST([datetime] as date)
order by  customerId,CAST([datetime] as date) ";
        var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new {  dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //获取单一水表汇总数据 周
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByWeek(string meterid, DateTime dt1, DateTime dt2)
    {
      try
      {
        var sql = @"select meterId,
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where meterId=@meterid and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY meterId,DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime]) 
order by meterId,DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime]) ";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid, dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<JsonResult> GetSMeterDataByWeek(string meterid, DateTime dt1, DateTime dt2)
    {
      try
      {
        var tenantid = Auth.GetTenantId(this.User.Identity.Name);
        var array = meterid.Split(',');
        var sql = @"select 
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where tenantid=@tenantid and meterId in (@meterid) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime]) 
order by DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime]) ";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid= array, dt1, dt2, tenantid });
        return Json(result.Select(x=>new {datetime=x.datetime.ToString("yyyy-MM-dd HH:mm:ss"),x.increment }), JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //[HttpPost]
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByWeekIn(string meterid, DateTime dt1, DateTime dt2)
    {
      var array = meterid.Split(',');
      try
      {
        var sql = $@"select meterId,
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where meterId in ({array.ToJoinSqlInVals()}) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY meterId,DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime]) 
order by meterId,DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime]) ";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new {  dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByWeek(string customerid, DateTime dt1, DateTime dt2)
    {
      try
      {
        //        var sql = @"select t2.customerId,
        //min(t1.[datetime]) [datetime],
        //sum(t1.[increment]) [increment] from [dbo].[waterrecordincrement] t1
        //inner join [dbo].[CustomerWaterMeters] t2 on t1.meterId=t2.meterId
        //where t2.customerId=@customerid and t1.[datetime]>=@dt1 and  t1.[datetime]<=@dt2
        //GROUP BY t2.customerId,DATEPART(YEAR, t1.[datetime]),DATEPART(WEEK, t1.[datetime]) 
        //order by t2.customerId,DATEPART(YEAR, t1.[datetime]),DATEPART(WEEK, t1.[datetime]) ";
        var sql = @"select min(customerName) customerId,
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[customerwaterrecordincrement] 
where customerId=@customerid and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY customerId,DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime]) 
order by customerId,DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime])";
        var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new { customerid, dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //[HttpPost]
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByWeekIn(string customerid, DateTime dt1, DateTime dt2)
    {
      var array = customerid.Split(',');
      try
      {
        //        var sql = $@"select t2.customerName as [customerId],
        //min(t1.[datetime]) [datetime],
        //sum(t1.[increment]) [increment] from [dbo].[waterrecordincrement] t1
        //inner join [dbo].[CustomerWaterMeters] t2 on t1.meterId=t2.meterId
        //where t2.customerId in ({array.ToJoinSqlInVals()}) and t1.[datetime]>=@dt1 and  t1.[datetime]<=@dt2
        //GROUP BY t2.customerName,DATEPART(YEAR, t1.[datetime]),DATEPART(WEEK, t1.[datetime]) 
        //order by t2.customerName,DATEPART(YEAR, t1.[datetime]),DATEPART(WEEK, t1.[datetime]) ";
        var sql = $@"select min(customerName) customerId,
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[customerwaterrecordincrement] 
where customerId in ({array.ToJoinSqlInVals()}) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY customerId,DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime]) 
order by customerId,DATEPART(YEAR, [datetime]),DATEPART(WEEK, [datetime])";
        var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new { dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //获取单一水表汇总数据 月
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByMonth(string meterid, DateTime dt1, DateTime dt2)
    {
      try
      {
        var sql = @"select meterId,
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where meterId=@meterid and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY meterId,DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) 
order by meterId,DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) ";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid, dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<JsonResult> GetSMeterDataByMonth(string meterid, DateTime dt1, DateTime dt2)
    {
      try
      {
        var tenantid = Auth.GetTenantId(this.User.Identity.Name);
        var array= meterid.Split(',');
        var sql = @"select 
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where tenantid=@tenantid and  meterId in (@meterid) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) 
order by DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) ";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { meterid= array, dt1, dt2 , tenantid });
        return Json(result.Select(x=>new {datetime=x.datetime.ToString("yyyy-MM-dd HH:mm:ss"),x.increment }), JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //[HttpPost]
    public async Task<IEnumerable<WaterLineChartModel>> GetMeterDataByMonthIn(string meterid, DateTime dt1, DateTime dt2)
    {
      var array = meterid.Split(',');
      try
      {
        var sql = $@"select meterId,
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where meterId in ({array.ToJoinSqlInVals()} ) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY meterId,DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) 
order by meterId,DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) ";
        var result = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new {  dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByMonth(string customerid, DateTime dt1, DateTime dt2)
    {
      try
      {
        var sql = @"select min(CustomerName) customerId,
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[customerwaterrecordincrement] 
where customerId=@customerid and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY customerId,DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) 
order by customerId,DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) ";
        var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new { customerid, dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //[HttpPost]
    public async Task<IEnumerable<CustomerLineChartModel>> GetCustomerDataByMonthIn(string customerid, DateTime dt1, DateTime dt2)
    {
      var array = customerid.Split(',');
      try
      {
        //        var sql = $@"select t2.customerName as [customerId],
        //min(t1.[datetime]) [datetime],
        //sum(t1.[increment]) [increment] from [dbo].[waterrecordincrement] t1
        //inner join [dbo].[CustomerWaterMeters] t2 on t1.meterId=t2.meterId
        //where t2.CustomerId in ({array.ToJoinSqlInVals()}) and t1.[datetime]>=@dt1 and  t1.[datetime]<=@dt2
        //GROUP BY t2.customerName,DATEPART(YEAR, t1.[datetime]),DATEPART(month, t1.[datetime]) 
        //order by t2.customerName,DATEPART(YEAR, t1.[datetime]),DATEPART(month, t1.[datetime]) ";
        var sql = $@"select min(CustomerName) customerId,
min([datetime]) [datetime],
sum([increment]) [increment] from [dbo].[customerwaterrecordincrement] 
where customerId in ({array.ToJoinSqlInVals()}) and [datetime]>=@dt1 and  [datetime]<=@dt2
GROUP BY customerId,DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) 
order by customerId,DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) ";
        var result = await this.db.Ado.SqlQueryAsync<CustomerLineChartModel>(sql, new {  dt1, dt2 });
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //======================================
    public async Task<IEnumerable<customermonth>> GetCustomerMonth()
    {
      try
      {
        //        var sql = $@"select t2.[CustomerName] customer,
        //DATEPART(year, [datetime]) [year],
        //DATEPART(month, [datetime]) [month] ,
        //sum(increment) [water]
        //from [dbo].[waterrecordincrement] as t1,
        //[dbo].[WaterMeters] as t2
        //where t1.meterId=t2.meterid and DATEPART(year, [datetime])={DateTime.Now.Year}
        //group by t2.CustomerName, DATEPART(year, [datetime]), DATEPART(month, [datetime])
        // ";
        var sql = $@"select min(customerName) customer,
DATEPART(year, [datetime])[year],
DATEPART(month, [datetime])[month] ,
sum(increment)[water]
from[dbo].[customerwaterrecordincrement]
where DATEPART(year, [datetime])= DATEPART(year, GETDATE())
group by customerId, DATEPART(year, [datetime]), DATEPART(month, [datetime])";
        var result = await this.db.SqlQueryable<customermonth>(sql).ToListAsync();
        return result.OrderBy(x => x.customer).ThenBy(x => x.year).ThenBy(x => x.month).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<IEnumerable<watermonth>> GetWaterMonth()
    {
      try
      {
        var sql = $@"select [meterId],
DATEPART(year, [datetime]) [year],
DATEPART(month, [datetime]) [month] ,
sum(increment) [water]
from [dbo].[waterrecordincrement] as [watermonth]
where DATEPART(year, [datetime])={DateTime.Now.Year}
group by[meterId], DATEPART(year, [datetime]), DATEPART(month, [datetime])
 ";
        var result = await this.db.SqlQueryable<watermonth>(sql).ToListAsync();
        return result.OrderBy(x => x.meterId).ThenBy(x => x.year).ThenBy(x => x.month).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<IEnumerable<WaterLineChartModel>> GetDayLineChartData(int day = 0)
    {
      var dt = DateTime.Now.AddDays(day).ToString("yyyy-MM-dd");
      var sql = "select meterId,[datetime],increment from [dbo].[waterrecordincrement]  where CONVERT(date, [datetime]) >= @datetime ";
      var data = await this.db.Ado.SqlQueryAsync<WaterLineChartModel>(sql, new { datetime = dt });
      return data;
    }
    public async Task<IEnumerable<WaterDayLineChartModel>> GetLineChartData()
    {
      var sql = @"select meterId,
CAST([datetime] AS DATE) [date],
max(water) [water] from[dbo].[WaterMeterRecords]
    group by meterId,
CAST([datetime] AS DATE)
order by meterId, CAST([datetime] AS DATE)";
      var data = await this.db.Ado.SqlQueryAsync<WaterDayLineChartModel>(sql);
      return data;
    }
    public async Task<IEnumerable<WaterDayLineChartModel>> GetWaterLineChartData()
    {
      var sql = @"select meterId,
      CAST([datetime] AS DATE) [date],
      sum(increment) [water] from[dbo].[waterrecordincrement]
          group by meterId,
      CAST([datetime] AS DATE)
      order by CAST([datetime] AS DATE)";

      var data = await this.db.Ado.SqlQueryAsync<WaterDayLineChartModel>(sql);
      return data;
    }
    public async Task<IEnumerable<MeterWaterModel>> GetMeterWater()
    {
      var sql = "select meterId,water from [dbo].[WaterMeters]";
      var data = await this.db.Ado.SqlQueryAsync<MeterWaterModel>(sql, new { });
      return data;
    }

    public async Task<IEnumerable<customerquotamonth>> GetCustomerQuotaMonth() {
      var year = DateTime.Now.Year;
      var month = DateTime.Now.Month;
      var sql = @"select customerName [customer],[year],[month],water,quota [quota], ForecastWater  [forecast], Convert(decimal(18,2),(water/(select sum(Water) from  [dbo].[CustomerWaterQuotas] 
where [year]=@year and [month]=@month)))   [percent]
from[dbo].[CustomerWaterQuotas]
where[year] = @year and[month] = @month";
      var data = await this.db.Ado.SqlQueryAsync<customerquotamonth>(sql, new { year, month });
      return data;
    }

    public async Task<IEnumerable<alarmcount>> GetAlarmCount() {
      var sql = @"select [level],count(1) [count] from [dbo].[AlarmLogs]
where Status = N'待处理'
group by[level]";
      var data = await this.db.Ado.SqlQueryAsync<alarmcount>(sql);
      return data;
     }

    public async Task<IEnumerable<zonesummary>> GetZoneSummay(string tag)
    {
      var sql = "";
      if (tag == "1") //当日
      {
        sql = @"select t2.ZoneName [zone],sum(t1.increment) water,
convert(decimal(10,0),SUM(t1.increment) * 100 / SUM(SUM(t1.increment)) OVER ()) AS [percent]
 from dbo.customerwaterrecordincrement t1
inner join dbo.CustomerWaterMeters t2 on t1.customerId = t2.CustomerId
where Convert(date, t1.[datetime]) = Convert(date, GETDATE())
group by t2.ZoneName";
      }
      else if (tag == "2") //当周
      {
        sql = @"select t2.ZoneName [zone],sum(t1.increment) water,
convert(decimal(10,0),SUM(t1.increment) * 100 / SUM(SUM(t1.increment)) OVER ()) AS [percent]
from dbo.customerwaterrecordincrement t1
inner join dbo.CustomerWaterMeters t2 on t1.customerId=t2.CustomerId
where DATEPART ( WEEK , t1.[datetime] )   = DATEPART ( WEEK , GETDATE() ) and DATEPART ( YEAR , t1.[datetime] ) = DATEPART ( YEAR , GETDATE() )
group by t2.ZoneName";
      }
      else if (tag == "3") //当月
      {
        sql = @"select t2.ZoneName [zone],sum(t1.increment) water,
convert(decimal(10,0),SUM(t1.increment) * 100 / SUM(SUM(t1.increment)) OVER ()) AS [percent]
from dbo.customerwaterrecordincrement t1
inner join dbo.CustomerWaterMeters t2 on t1.customerId=t2.CustomerId
where DATEPART ( MONTH , t1.[datetime] )   = DATEPART ( MONTH , GETDATE() ) and DATEPART ( YEAR , t1.[datetime] ) = DATEPART ( YEAR , GETDATE() )
group by t2.ZoneName";
       }
      if (tag == "4") //当年
      {
        sql = @"select t2.ZoneName [zone],sum(t1.increment) water,
convert(decimal(10,0),SUM(t1.increment) * 100 / SUM(SUM(t1.increment)) OVER ()) AS [percent]
from dbo.customerwaterrecordincrement t1
inner join dbo.CustomerWaterMeters t2 on t1.customerId=t2.CustomerId
where   DATEPART ( YEAR , t1.[datetime] ) = DATEPART ( YEAR , GETDATE() )
group by t2.ZoneName";
      }

      var data = await this.db.Ado.SqlQueryAsync<zonesummary>(sql);
      return data;
    }
    public async Task<IEnumerable<customertypesummary>> GetCustomerTypeSummay(string tag)
    {
      var sql = "";
      if (tag == "1") //当日
      {
        sql = @"select t2.[type] [type],sum(t1.increment) water,
convert(decimal(10,0),SUM(t1.increment) * 100 / SUM(SUM(t1.increment)) OVER ()) AS [percent]
from dbo.customerwaterrecordincrement t1
inner join dbo.Customers t2 on t1.customerId = t2.Id
where Convert(date, t1.[datetime]) = Convert(date, GETDATE())
group by t2.[Type]";
      }
      else if (tag == "2") //当周
      {
        sql = @"select t2.[type] [type],sum(t1.increment) water,
convert(decimal(10,0),SUM(t1.increment) * 100 / SUM(SUM(t1.increment)) OVER ()) AS [percent]
from dbo.customerwaterrecordincrement t1
inner join dbo.CustomerWaterMeters t2 on t1.customerId=t2.Id
where DATEPART ( WEEK , t1.[datetime] )   = DATEPART ( WEEK , GETDATE() ) and DATEPART ( YEAR , t1.[datetime] ) = DATEPART ( YEAR , GETDATE() )
group by t2.[type]";
      }
      else if (tag == "3") //当月
      {
        sql = @"select  t2.[type] [type],sum(t1.increment) water,
convert(decimal(10,0),SUM(t1.increment) * 100 / SUM(SUM(t1.increment)) OVER ()) AS [percent]
from dbo.customerwaterrecordincrement t1
inner join dbo.Customers t2 on t1.customerId=t2.Id
where DATEPART ( MONTH , t1.[datetime] )   = DATEPART ( MONTH , GETDATE() ) and DATEPART ( YEAR , t1.[datetime] ) = DATEPART ( YEAR , GETDATE() )
group by t2.[type]";
      }
      if (tag == "4") //当年
      {
        sql = @"select t2.[type] [type],sum(t1.increment) water,
convert(decimal(10,0),SUM(t1.increment) * 100 / SUM(SUM(t1.increment)) OVER ()) AS [percent]
from dbo.customerwaterrecordincrement t1
inner join dbo.Customers t2 on t1.customerId=t2.Id
where   DATEPART ( YEAR , t1.[datetime] ) = DATEPART ( YEAR , GETDATE() )
group by t2.[type]";
      }

      var data = await this.db.Ado.SqlQueryAsync<customertypesummary>(sql);
      return data;
    }

    //查询总进出水量
    public async Task<JsonResult> GetMainWaterDaily(DateTime dt1, DateTime dt2) {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = "select CAST([date] AS DATE) [date] ,involume inval,outvolume outval from dbo.MainMeters where tenantid=@tenantid and [date]>=@dt1 and [date]<=@dt2 ";
      var data = await this.db.Ado.SqlQueryAsync<mainwaterdaily>(sql,new { tenantid, dt1,dt2});
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    //查询本月总进出水
    public async Task<JsonResult> GetTotalMainWaterMonth() {
      
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = "select sum(involume) totalin, sum(outvolume) totalout,0 totalstd  from dbo.MainMeters where tenantid=@tenantid and DATEPART(YEAR,[date])=DATEPART(YEAR,GETDATE()) and DATEPART(MONTH,[date])=DATEPART(MONTH,GETDATE())";
      var data = await this.db.Ado.SqlQueryAsync<totalmainwatermonth>(sql,new { tenantid });
      return Json(data.FirstOrDefault(), JsonRequestBehavior.AllowGet);
    }
    //首页按业态汇总
    public async Task<JsonResult> GetTypeWater(DateTime dt1, DateTime dt2)
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"select t2.Type [type],sum(t1.increment) [water]
 from [dbo].[customerwaterrecordincrement]  t1
inner join[dbo].[Customers] t2 on t1.customerId = t2.Id
 where t1.datetime >= @dt1 and t1.datetime <= @dt2 and t1.tenantid=@tenantid
 group by t2.[Type]  ";
      var data = await this.db.Ado.SqlQueryAsync<typewater>(sql, new { dt1, dt2, tenantid });
      return Json(data, JsonRequestBehavior.AllowGet);
    }

    //首页区域态汇总
    public async Task<JsonResult> GetZoneWater(DateTime dt1, DateTime dt2)
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"  select isnull(t2.ZoneName,N'其它') [zone],sum(t1.increment) [water]
 from [dbo].[customerwaterrecordincrement]  t1
 inner join [dbo].[CustomerWaterMeters] t2 on t1.meterId=t2.meterId
 where t1.datetime>= @dt1 and t1.datetime <= @dt2 and t1.tenantid=@tenantid
 group by t2.ZoneName";
      var data = await this.db.Ado.SqlQueryAsync<zonewater>(sql, new { dt1, dt2, tenantid });
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    //查询汇总水量
    public async Task<JsonResult> GetSummaryWater(string meterid, DateTime dt1, DateTime dt2) {
      try
      {
        var array = meterid.Split(',');
        var tenantid = Auth.GetTenantId();
        var sql = @"select 
sum([increment]) [increment] from [dbo].[waterrecordincrement] 
where meterId in (@meterid) and [datetime]>=@dt1 and  [datetime]<=@dt2 and tenantid=@tenantid
 ";
        var result = await this.db.Ado.GetDecimalAsync(sql, new { meterid = array, dt1, dt2, tenantid });
        return Json(result,JsonRequestBehavior.AllowGet);
      }
      catch (Exception e) {
        return Json(0, JsonRequestBehavior.AllowGet);
      }
    }

    [System.Web.Mvc.HttpPost]
    public async Task<JsonResult> QueryCompareMonthData(comparequeryrequestdto[] dto, string years)
    {
      var result = new List<ChainWaterMeterDataModel>();
      if (dto != null)
      {
        var tenantid = Auth.GetTenantId(this.User.Identity.Name);
        var yarray = years.Split(',');
        foreach (var para in dto)
        {
          var marray = para.data.Split(',');
          var sql = $@"select
N'{para.text}' meterId,
DATEPART(YEAR, [datetime]) [year],DATEPART(month, [datetime]) [month] ,
sum([increment]) [water] from [dbo].[waterrecordincrement] 
where tenantid=@tenantid and  meterId in (@meterid) and DATEPART(YEAR, [datetime]) in(@years)
GROUP BY DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) 
order by DATEPART(YEAR, [datetime]),DATEPART(month, [datetime])  ";
          var data = await this.db.Ado.SqlQueryAsync<ChainWaterMeterDataModel>(sql, new { meterid = marray, tenantid, years = yarray });
          //var list = new List<ChainWaterMeterDataModel>();
          foreach (var year in yarray)
          {
            for (var i = 1; i <= 12; i++)
            {
              var item = data.Where(x => x.year == year && x.month == i.ToString()).FirstOrDefault();
              if (item != null)
              {
                item.year = $"{year}-{para.text}";
                result.Add(item);
              }
              else
              {
                result.Add(new ChainWaterMeterDataModel()
                {
                  water = 0,
                  meterId = para.text,
                  month = i.ToString(),
                  year = $"{year}-{para.text}"
                });
              }
            }
          }
        }
      }

      return Json(result, JsonRequestBehavior.AllowGet);

    }
    //年-月同比
    public async Task<JsonResult> GetComparedMonthData(string meterid, string years,string label)
    {
      try
      {
        var tenantid = Auth.GetTenantId(this.User.Identity.Name);
        var marray = meterid.Split(',');
        var yarray = years.Split(',');
        var sql = $@"select
N'{label}' meterId,
DATEPART(YEAR, [datetime]) [year],DATEPART(month, [datetime]) [month] ,
sum([increment]) [water] from [dbo].[waterrecordincrement] 
where tenantid=@tenantid and  meterId in (@meterid) and DATEPART(YEAR, [datetime]) in(@years)
GROUP BY DATEPART(YEAR, [datetime]),DATEPART(month, [datetime]) 
order by DATEPART(YEAR, [datetime]),DATEPART(month, [datetime])  ";
        var result = await this.db.Ado.SqlQueryAsync<ChainWaterMeterDataModel>(sql, new { meterid = marray, tenantid,years= yarray });
        var list = new List<ChainWaterMeterDataModel>();
        foreach (var year in yarray)
        {
          for (var i = 1; i <= 12; i++)
          {
            var item = result.Where(x => x.year == year && x.month == i.ToString()).FirstOrDefault();
            if (item != null)
            {
              item.year = $"{year}-{label}";
              list.Add(item);
            }
            else
            {
              list.Add(new ChainWaterMeterDataModel()
              {
                 water=0,
                  meterId=label,
                   month=i.ToString(),
                    year= $"{year}-{label}"
            });
             }
          }
        }
        return Json(list, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    //获取区域汇总的饼图
    public async Task<JsonResult> GetZonePieData(DateTime dt1, DateTime dt2) {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"
select
t2.ZoneName [type],
count(t3.increment) total,
count(t3.increment)*100/ (Select count(t3.increment) From  dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId=t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId=t2.meterId and t3.TenantId=t2.TenantId
where t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
) as [percent]
from dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId = t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId = t2.meterId and t3.TenantId = t2.TenantId
where t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
group by t2.ZoneName";
      var result =await this.db.Ado.SqlQueryAsync<summarypiemodel>(sql, new { tenantid, dt1, dt2 });
      return Json(result, JsonRequestBehavior.AllowGet);

      }
    public async Task<JsonResult> GetZonePointPieData(string zonename,DateTime dt1, DateTime dt2)
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"
select
t2.Points [type],
count(t3.increment) total,
count(t3.increment)*100/ (Select count(t3.increment) From  dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId=t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId=t2.meterId and t3.TenantId=t2.TenantId
where t2.ZoneName=@zonename and t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
) as [percent]
from dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId = t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId = t2.meterId and t3.TenantId = t2.TenantId
where t2.ZoneName=@zonename and t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
group by t2.Points";
      var result = await this.db.Ado.SqlQueryAsync<summarypiemodel>(sql, new { zonename, tenantid, dt1, dt2 });
      return Json(result, JsonRequestBehavior.AllowGet);

    }
    //获取业态汇总的饼图
    public async Task<JsonResult> GetCustomerTypePieData(DateTime dt1, DateTime dt2)
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"
select
t1.[type] [type],
count(t3.increment) total,
count(t3.increment)*100/ (Select count(t3.increment) From  dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId=t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId=t2.meterId and t3.TenantId=t2.TenantId
where t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
) as [percent]
from dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId = t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId = t2.meterId and t3.TenantId = t2.TenantId
where t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
group by t1.[Type]";
      var result = await this.db.Ado.SqlQueryAsync<summarypiemodel>(sql, new { tenantid, dt1, dt2 });
      return Json(result, JsonRequestBehavior.AllowGet);

    }
    //获取业态汇-单位总的饼图
    public async Task<JsonResult> GetCustomerPieData(string type,DateTime dt1, DateTime dt2)
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"
select
t1.[name] [type],
count(t3.increment) total,
count(t3.increment)*100/ (Select count(t3.increment) From  dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId=t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId=t2.meterId and t3.TenantId=t2.TenantId
where t1.[type]=@type and t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
) as [percent]
from dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId = t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId = t2.meterId and t3.TenantId = t2.TenantId
where t1.[type]=@type and t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
group by t1.[name]";
      var result = await this.db.Ado.SqlQueryAsync<summarypiemodel>(sql, new {type, tenantid, dt1, dt2 });
      return Json(result, JsonRequestBehavior.AllowGet);

    }

    //获取业态-单位-部门汇总的饼图
    public async Task<JsonResult> GetCustomerDeptPieData(string type,string name, DateTime dt1, DateTime dt2)
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"
select
t2.[Dept] [type],
count(t3.increment) total,
count(t3.increment)*100/ (Select count(t3.increment) From  dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId=t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId=t2.meterId and t3.TenantId=t2.TenantId
where t1.[type]=@type and t1.[Name]=@name and t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
) as [percent]
from dbo.Customers t1
inner join dbo.CustomerWaterMeters t2 on t1.Id = t2.CustomerId and t1.TenantId = t2.TenantId
inner join dbo.[waterrecordincrement] t3 on t3.meterId = t2.meterId and t3.TenantId = t2.TenantId
where t1.[type]=@type and t1.[Name]=@name and t1.tenantid = @tenantid and t3.[datetime]>= @dt1 and t3.[datetime]<= @dt2
group by t2.[Dept]";
      var result = await this.db.Ado.SqlQueryAsync<summarypiemodel>(sql, new { type,name, tenantid, dt1, dt2 });
      return Json(result, JsonRequestBehavior.AllowGet);

    }


    //首页按小时汇总图表
    [OutputCache(Duration =43200,VaryByParam = "dt1,dt2")]
    public async Task<JsonResult> GetSummarywaterbyhour(DateTime dt1,DateTime dt2) {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"SELECT *
  FROM [dbo].[summarywaterbyhour]
  where[date] >= @dt1 and[date] <= @dt2 and tenantid=@tenantid
order by[date],[hour]";
      var result = await this.db.Ado.SqlQueryAsync<summarywaterbyhour>(sql, new { dt1, dt2, tenantid });
      return Json(result.Select(x => new { date = $"{x.date.ToString("yyyy-MM-dd")} {x.hour}:0", x.water }), JsonRequestBehavior.AllowGet);
     }
    [OutputCache(Duration = 60, VaryByParam = "dt1,dt2")]
    public async Task<JsonResult> GetSummarywaterbyday(DateTime dt1, DateTime dt2)
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"SELECT *
  FROM [dbo].[summarywaterbydate]
  where[date] >= @dt1 and[date] <= @dt2 and tenantid=@tenantid
order by[date]";
      var result = await this.db.Ado.SqlQueryAsync<summarywaterbyday>(sql, new { dt1, dt2, tenantid });
      return Json(result.Select(x => new { date = $"{x.date.ToString("yyyy-MM-dd")}", x.water }), JsonRequestBehavior.AllowGet);
    }

    [OutputCache(Duration = 120, VaryByParam = "year")]
    public async Task<JsonResult> GetSummarywaterbymonth(string year="2021")
    {
      try
      {
        var tenantid = Auth.GetTenantId(this.User.Identity.Name);
        var sql = $@"SELECT *
  FROM [dbo].[summarywaterbymonthrate]
  where [month] like '{year}%' and tenantid=@tenantid
order by [month]";
        var result = await this.db.Ado.SqlQueryAsync<summarywaterbymonth>(sql, new { tenantid });
        return Json(result.Select(x => new { x.month, x.water, ratio = x.vs_previous_water }), JsonRequestBehavior.AllowGet);
      }
      catch (Exception e) {
        throw e;
        }
     
      
    }

    [OutputCache(Duration = 120)]
    public async Task<JsonResult> GetSummarywaterbyyear()
    {
      var tenantid = Auth.GetTenantId(this.User.Identity.Name);
      var sql = @"SELECT *
  FROM [dbo].[summarywaterbyyearrate]
  where  tenantid=@tenantid
order by [year]";
      var result = await this.db.Ado.SqlQueryAsync<summarywaterbyyear>(sql, new { tenantid });
      return Json(result.Select(x => new { x.year, x.water, ratio = x.vs_previous_water }), JsonRequestBehavior.AllowGet);
    }
  }
}
