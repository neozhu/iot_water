using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Polly;
using Polly.Extensions.Http;
using RestClient.Net;
using RestClient.Net.Abstractions.Extensions;
using SqlSugar;
using WebApp.Models;
using WebApp.Models.JsonModel;

namespace WebApp.App_Helpers.third_party.api
{
  public class meter
  {
    public string meterId { get; set; }
    public int TenantId { get; set; }
  }
  public class configpara
  {
    public string Host { get; set; }
    public string SecretAccessKey { get; set; }
  }
  public class WaterApi
  {
    private static SqlSugarClient getInstance() => new SqlSugarClient(new ConnectionConfig()
    {
      ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
      DbType = DbType.SqlServer,
      IsAutoCloseConnection = false,
      InitKeyType = InitKeyType.Attribute
    });
    public static async Task findQueryDetailDataByDatetime()
    {
      try
      {
        var policy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var db = getInstance();
        var sql = "SELECT distinct [meterId],[TenantId] FROM [dbo].[WaterMeters] where meterType=N'智能表'";
        var array = await db.SqlQueryable<meter>(sql).ToListAsync();
        foreach (var meter in array)
        {
          try
          {
            var tries = 0;
            sql = "SELECT distinct [Host] ,[SecretAccessKey] FROM [dbo].[ApiConfigs] where [TenantId]=@tid and [ServiceName]=N'水表服务'";
            var config = await db.Ado.SqlQueryAsync<configpara>(sql, new { tid = meter.TenantId });
            var method = "interface/findQueryDetailDataByDatetime.do";
            var url = $"{config.First().Host}{method}";
            var client = new Client(new NewtonsoftSerializationAdapter(),
              null,
              new Uri(url),
              null,
              createHttpClient:(httpClient) => {
                return new SingletonHttpClientFactory().CreateClient(httpClient);
                },
             sendHttpRequestFunc: (httpClient, httpRequestMessageFunc, logger, cancellationToken) =>
             {
               return policy.ExecuteAsync(() =>
               {
                 var httpRequestMessage = httpRequestMessageFunc.Invoke();

                 //On the third try change the Url to a the correct one
                 tries++;
                 return httpClient.SendAsync(httpRequestMessage, cancellationToken);
               });
             } );
            client.SetJsonContentTypeHeader();
            var result = await client
              .PostAsync<findQueryDetailDataByDatetimeResponse, findQueryDetailDataByDatetimeRequest>(
              new findQueryDetailDataByDatetimeRequest
              {
                checkCode = config.First().SecretAccessKey,
                count = 1,
                data = new string[] { meter.meterId },
                datetime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
              });
            if (result.IsSuccess && result.Body.result == "success")
            {
              foreach (var item in result.Body.data)
              {
                await db.Ado.UseStoredProcedure().ExecuteCommandAsync("[dbo].[SP_InsertWaterMeterRecord]", new
                {
                  meterStatus = item.meterStatus,
                  datetime = Convert.ToDateTime(item.datetime),
                  meterId = item.meterId,
                  water = item.water,
                  reverseWater = item.reverseWater,
                  temperature = item.temperature,
                  flowrate = item.flowrate,
                  pressure = item.pressure,
                  voltage = item.voltage,
                  valveStatus = item.valveStatus,
                  userId = item.userId,
                  imei = item.imei,
                  TenantId = meter.TenantId
                });

              }
            }
            else
            {
              NLog.LogManager.GetCurrentClassLogger().Error($"同步数据异常,表号:{meter.meterId}");
            }
          }catch(Exception e)
          {
            NLog.LogManager.GetCurrentClassLogger().Error($"同步数据异常,表号:{meter.meterId},{e.Message}");
          }
        }

        //更新最新表读数
        var updatesql = "update [dbo].[WaterMeters] SET water = b.water FROM [dbo].[WaterMeters] a ,(select meterId,max(water) [water] from [dbo].[WaterMeterRecords] group by meterId) b  WHERE  a.meterId = b.meterId  and a.meterType=N'智能表' ";
        await db.Ado.ExecuteCommandAsync(updatesql);

        NLog.LogManager.GetCurrentClassLogger().Info($"同步完成：{DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")} 水表读数");
      }
      catch (Exception e) {
        throw e;
        }
    }

    public static async Task UpdateQuota() {
       var db = getInstance();
      var now = DateTime.Now;
      var month = now.Month;
      var year = now.Year;
      var sql = @"select t.customerid,
t.customername, 
t.[year],
t.[month],
sum(t.water) [water] from (
select t2.CustomerId,
t2.CustomerName, 
t1.meterid,
DATEPART(yy, t1.[datetime]) [year] ,
DATEPART(mm, t1.[datetime]) [month],
min(t1.[datetime]) [startdate],
min(t1.[water]) [start] ,
max(t1.[datetime]) [enddate],
max(t1.[water]) [end],
max(t1.[water]) - min(t1.[water]) [water] from [dbo].[WaterMeterRecords] t1 
inner join [dbo].[WaterMeters] t2 on t1.[meterId]=t2.[meterId]
where t1.CustomerId > 0  and DATEPART(yy, t1.[datetime])=@year and DATEPART(mm, t1.[datetime])=@month
group by t2.CustomerId,t2.CustomerName, t1.meterid,DATEPART(yy, t1.[datetime]) ,DATEPART(mm, t1.[datetime])
) t
group by t.CustomerId,t.CustomerName,t.[year],t.[month]";
      var data = await db.Ado.SqlQueryAsync<dynamic>(sql,new { year,month});
      foreach (var item in data)
      {
        var customerId = (int)item.customerid;
        var customerName = (string)item.customername;
        var water = (decimal)item.water;
        var days = DateTime.DaysInMonth(year, month);
        var forecast = water * days / now.Day;
        var update = " update [dbo].[CustomerWaterQuotas] set [Water]=@water, [ForecastWater]=@forecast,[RecordDate]=@now  where year=@year and month=@month and customerid=@customerId";
        await db.Ado.ExecuteCommandAsync(update, new { water, forecast, now, year, month, customerId });
        //判断用水量是否超过定额，超过预警.
       
        var selectsql = "select quota from [dbo].[CustomerWaterQuotas]  where year=@year and month=@month and customerid=@customerId";
        var quota = await db.Ado.GetDecimalAsync(selectsql, new { year, month, customerId });
        var content = "";
        var level = "一级";
        if (forecast > quota)
        {
          content = $"预测用水量:{forecast.ToString("0.0")} > 定额:{quota.ToString("0.0")} 报警。";
          if (water > quota)
          {
            content += $"实际用水量:{water.ToString("0.0")} 》定额:{quota.ToString("0.0")} 报警。";
            level = "二级";
          }
          var countsql = "select count(1) from [dbo].[AlarmLogs] where DeviceId=@customerName and CAST(InitDateTime AS date)  =@date and [status]=N'待处理'";
          var count = await db.Ado.GetIntAsync(countsql, new { customerName, date=now.ToString("yyyy-MM-dd") });
          if (count == 0 && !string.IsNullOrEmpty(customerName))
          {
            var insertsql = @"INSERT INTO [dbo].[AlarmLogs]
           ([DeviceId]
           ,[Status]
           ,[Type]
           ,[Level]
           ,[Content]
           ,[InitDateTime]
           ,[User]
       ,[TenantId]
           )
     VALUES
           (@DeviceId
           , @Status
           , @Type
           , @Level
           , @Content
           , @InitDateTime
       , @User
           , @TenantId)";
            await db.Ado.ExecuteCommandAsync(insertsql, new
            {
              DeviceId = customerName,
              Status = "待处理",
              Type = "阈值",
              Level = level,
              Content = content,
              InitDateTime = now,
              User = "sys",
              TenantId = 1
            });
          }
        }
        

      }
      NLog.LogManager.GetCurrentClassLogger().Info($"更新水表配额");


    }
  }
}