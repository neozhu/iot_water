using System;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using System.Linq.Dynamic.Core;
using Service.Pattern;
using System.Text.RegularExpressions;
using WebApp.Models;
using WebApp.Repositories;
using ClosedXML.Report;
using Spire.Xls;
using System.Net.Mail;
using System.Net;
using WebApp.Models.ViewModel;

namespace WebApp.Services
{
  /// <summary>
  /// File: BillHeaderService.cs
  /// Purpose: Within the service layer, you define and implement 
  /// the service interface and the data contracts (or message types).
  /// One of the more important concepts to keep in mind is that a service
  /// should never expose details of the internal processes or 
  /// the business entities used within the application. 
  /// Created Date: 2/19/2021 8:29:53 PM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class BillHeaderService : Service<BillHeader>, IBillHeaderService
  {
    private readonly IRepositoryAsync<BillHeader> repository;
    private readonly IBillDetailService billDetailService;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly ICustomerService customerService;
    private readonly IWaterMeterService meterService;
    private readonly IWaterMeterRecordService recordService;
    private readonly IWaterManualReportService waterManualReportService;
    private readonly NLog.ILogger logger;
    public BillHeaderService(
      IWaterManualReportService waterManualReportService,
      IBillDetailService billDetailService,
      IWaterMeterService meterService,
      IWaterMeterRecordService recordService,
       ICustomerService customerService,
      IRepositoryAsync<BillHeader> repository,
      IDataTableImportMappingService mappingservice,
      NLog.ILogger logger
      )
        : base(repository)
    {
      this.waterManualReportService = waterManualReportService;
      this.billDetailService = billDetailService;
      this.meterService = meterService;
      this.recordService = recordService;
      this.customerService = customerService;
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
    }
    public async Task<IEnumerable<BillHeader>> GetByCustomerId(int customerid) => await repository.GetByCustomerId(customerid);
    public async Task<IEnumerable<BillDetail>> GetBillDetailsByBillId(int billid) => await repository.GetBillDetailsByBillId(billid);



    private async Task<int> getCustomerIdByName(string name)
    {
      var customerRepository = this.repository.GetRepositoryAsync<Customer>();
      var customer = await customerRepository.Queryable().Where(x => x.Name == name).FirstOrDefaultAsync();
      if (customer == null)
      {
        throw new Exception("not found ForeignKey:CustomerId with " + name);
      }
      else
      {
        return customer.Id;
      }
    }
    public async Task ImportDataTable(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "BillHeader" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到BillHeader对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null ||
              ( !row.IsNull(requiredfield) &&
               !string.IsNullOrEmpty(row[requiredfield].ToString())
              )
            )
        {
          var item = new BillHeader();
          var billheadertype = item.GetType();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain &&
                           !row.IsNull(field.SourceFieldName) &&
                           !string.IsNullOrEmpty(row[field.SourceFieldName].ToString())
                        )
            {
              var propertyInfo = billheadertype.GetProperty(field.FieldName);
              //关联外键查询获取Id
              switch (field.FieldName)
              {
                case "CustomerId":
                  var customer_name = row[field.SourceFieldName].ToString();
                  var customerid = await this.getCustomerIdByName(customer_name);
                  propertyInfo.SetValue(item, Convert.ChangeType(customerid, propertyInfo.PropertyType), null);
                  break;
                default:
                  var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                  var safeValue = Convert.ChangeType(row[field.SourceFieldName], safetype);
                  propertyInfo.SetValue(item, safeValue, null);
                  break;
              }
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var propertyInfo = billheadertype.GetProperty(field.FieldName);
              if (string.Equals(defval, "now", StringComparison.OrdinalIgnoreCase) && ( propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(Nullable<DateTime>) ))
              {
                var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var safeValue = Convert.ChangeType(DateTime.Now, safetype);
                propertyInfo.SetValue(item, safeValue, null);
              }
              else if (string.Equals(defval, "guid", StringComparison.OrdinalIgnoreCase))
              {
                propertyInfo.SetValue(item, Guid.NewGuid().ToString(), null);
              }
              else if (string.Equals(defval, "user", StringComparison.OrdinalIgnoreCase))
              {
                propertyInfo.SetValue(item, username, null);
              }
              else
              {
                var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var safeValue = Convert.ChangeType(defval, safetype);
                propertyInfo.SetValue(item, safeValue, null);
              }
            }
          }
          this.Insert(item);
        }
      }
    }
    public async Task<Stream> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var expcolopts = await this.mappingservice.Queryable()
             .Where(x => x.EntitySetName == "BillHeader" && x.IgnoredColumn)
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName,
               LineNo=x.LineNo
             }).ToArrayAsync();

      var billheaders = await this.Query(
        new BillHeaderQuery().Withfilter(filters))
        .Include(p => p.Customer)
        .OrderBy(n => n.OrderBy($"{sort} {order}"))
        .SelectAsync();

       
      return await NPOIHelper.ExportExcelAsync("账单信息", billheaders, expcolopts);
    }
    public async Task Delete(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        var reports =await this.waterManualReportService.Queryable().Where(x => x.BillNo == item.BillNo).ToListAsync();
        foreach (var report in reports)
        {
          this.waterManualReportService.Delete(report);
        }

        this.Delete(item);
      }

    }

    public async Task GenerateBills(string inputmonth,int day)
    {
      var year = Convert.ToInt32(inputmonth.Split('-')[0]);
      var month = Convert.ToInt32(inputmonth.Split('-')[1]);
      var endate = new DateTime(year, month, day);
      var startdate = endate.AddMonths(-1);
      var customers = await this.customerService.Queryable()
        .Where(x=>x.WaterMeters.Any(y=>y.IsDelete==false) && x.Status!="停用")
        .Include(x => x.WaterMeters)
        .ToListAsync();
      var reportlist = new List<WaterManualReport>();
      foreach (var customer in customers)
      {
        
        var yearmonth = $"{year}年{month}月";
        var any =await this.Queryable().Where(x => x.CustomerId == customer.Id && x.Month == yearmonth).AnyAsync();
        if (!any)
        {
          var head = new BillHeader
          {
            BillNo = KeyGenerator.GetNextBillNo(),
            CustomerId = customer.Id,
            CustomerName = customer.Name,
            CustomerCode=customer.Code,
            BillDate = DateTime.Now,
            PaymentDate=DateTime.Now.AddMonths(1),
            Category = customer.Category,
            WaterPrice = customer.WaterPrice,
            ServicePrice = customer.ServicePrice,
            Discount = customer.Discount,
            Month = yearmonth,
            Status = "待确认"
          };
         
          var details = new List<BillDetail>();
          
          foreach (var meter in customer.WaterMeters)
          {
          
            var meterdata = await this.meterService.Find(meter.meterId);
            if (meterdata == null || meterdata.Status== "停用")
            {
              this.logger.Warn($"创建{yearmonth}账单时:{meter.meterId} 不存在.");
              continue;
              //throw new Exception($"{meter.meterId} 不存在.");
            }
            var endwater = await this.recordService.GetEndWaterRecord(startdate, endate, meter.meterId);
            if (endwater == null)
            {
              var rate = meterdata.Rate ?? 1;
              details.Add(new BillDetail()
              {
                BillHeader = head,
                MeterId = meter.meterId,
                ActualWater = 0,
                LastWater = null,
                PerCent = null,
                LineNo = meterdata.LineNo,
                MeterName1 = meterdata.Name1,
                MeterName = meter.meterName,
                MeterPoint = meter.Remark,
                Negitive = meter.Negitive,
                Remark = "当月没有用水记录",
                Ratio = meter.Ratio,
                Rate = rate,
                Water = 0
              });
            }
            else
            {
              var startw = 0m;
              decimal?  lastcal = 0m;
              decimal? onyear = 0m;
              var startd = new DateTime(1990,1,1);
              var startwater = await this.waterManualReportService.Find(meter.meterId, startdate.Year, startdate.Month);
              if (startwater == null)
              {
                var startwater2 = await this.recordService.GetStartWaterRecord(startdate, endate, meter.meterId);
                if (startwater2 != null)
                {
                  startw = startwater2.water;
                  startd = startwater2.datetime;
                }
                lastcal = 0;
              }
              else
              {
                startw = startwater.Water;
                startd = startwater.RecordDate;
                lastcal = startwater.CalWater;
                //找去年同期的用水量
                var onyearw = await this.waterManualReportService.Find(meter.meterId, startdate.Year - 1, startdate.Month);
                if (onyearw != null)
                {
                  onyear = onyearw.CalWater;
                }
              }
              var rate = meterdata.Rate ?? 1;
              var water = ( endwater.water - startw ) * rate;
              var existreport =await this.waterManualReportService.Queryable()
                .Where(x => x.meterId == meterdata.meterId && x.Month == head.Month)
                .FirstOrDefaultAsync();
              if (existreport != null && !reportlist.Any(x => x.meterId == meterdata.meterId && x.Month == head.Month))
              {
                 
                  existreport.CalWater =Math.Round(endwater.water - startw,0, MidpointRounding.AwayFromZero);
                existreport.LastWater = Math.Round(startw,0, MidpointRounding.AwayFromZero);
                existreport.LastRecordDate = startd;
                existreport.RecordDate = endwater.datetime;
                existreport.Water = Math.Round(endwater.water,0, MidpointRounding.AwayFromZero);
                existreport.LastCalWater = Math.Round(lastcal??0,0, MidpointRounding.AwayFromZero);
                existreport.OnYearCalWater = Math.Round(onyear??0,0, MidpointRounding.AwayFromZero);
                existreport.BillNo = head.BillNo;
                if (existreport.OnYearCalWater > 0)
                {
                  existreport.YearRatio = (existreport.CalWater - existreport.OnYearCalWater ) / existreport.OnYearCalWater * 100;
                }
                if (existreport.LastCalWater > 0)
                {
                  existreport.LastRatio = (existreport.CalWater - existreport.LastCalWater ) / existreport.LastCalWater * 100;
                }
                this.waterManualReportService.Update(existreport);
                reportlist.Add(existreport);

              }
              else
              {
                if (!reportlist.Any(x => x.meterId == meterdata.meterId && x.Month == head.Month))
                {
                  var report = new WaterManualReport()
                  {
                    CalWater = Math.Round(endwater.water - startw, 0, MidpointRounding.AwayFromZero),
                    LastWater = Math.Round(startw, 0, MidpointRounding.AwayFromZero),
                    LastRecordDate = startd,
                    LineNo = meterdata.LineNo,
                    meterId = meter.meterId,
                    Name = meterdata.Name,
                    Name1 = meterdata.Name1,
                    meterType = meterdata.meterType,
                    RecordDate = endwater.datetime,
                    Water = Math.Round(endwater.water, 0, MidpointRounding.AwayFromZero),
                    LastCalWater = Math.Round(lastcal ?? 0, 0, MidpointRounding.AwayFromZero),
                    OnYearCalWater = Math.Round(onyear ?? 0, 0, MidpointRounding.AwayFromZero),
                    BillNo = head.BillNo,
                    Month = head.Month
                  };
                  if (report.OnYearCalWater > 0)
                  {
                    report.YearRatio = ( report.CalWater - report.OnYearCalWater ) / report.OnYearCalWater * 100;
                  }
                  if (report.LastCalWater > 0)
                  {
                    report.LastRatio = ( report.CalWater - report.LastCalWater ) / report.LastCalWater * 100;
                  }
                  this.waterManualReportService.Insert(report);
                  reportlist.Add(report);
                }
              }
              var ratio = meter.Ratio;
              var negitive = meter.Negitive;
              var actwater = water;
              if (negitive == "负项")
              {
                actwater = water * -ratio;
              }
              else
              {
                actwater = water * ratio;
              }

              details.Add(new BillDetail()
              {
                BillHeader = head,
                MeterId = meter.meterId,
                ActualWater = actwater,
                LastWater = null,
                PerCent = null,
                LineNo = meterdata.LineNo,
                MeterName1 = meterdata.Name1,
                MeterName = meter.meterName,
                MeterPoint = meter.Remark,
                Negitive = meter.Negitive,
                Rate = rate,
                Remark = "",
                Ratio = meter.Ratio,
                Water = Math.Round(water,0, MidpointRounding.AwayFromZero),
                WaterDt1 = startd,
                Water1 = Math.Round(startw,0, MidpointRounding.AwayFromZero),
                WaterDt2 = endwater.datetime,
                Water2 = Math.Round(endwater.water,0, MidpointRounding.AwayFromZero),
              });
            }
            head.BillDetails = details;
            head.TotalWater = Math.Round(details.Sum(x => x.ActualWater),0, MidpointRounding.AwayFromZero);
            head.TotalWaterAmount = head.TotalWater * head.WaterPrice;
            head.TotalServiceAmount = head.TotalWater * head.ServicePrice;
            head.TotalAmount = head.TotalWaterAmount + head.TotalServiceAmount;
            head.TotalReceivableAmount = head.TotalAmount * head.Discount;
            head.TotalChineseAmount = head.TotalAmount.ChineseMoney();
            this.Insert(head);
          }

          var lastitem = await this.GetAdnUpdateLasterWater(head.BillNo, yearmonth, customer.Id);
          if (lastitem != null)
          {
            head.LastTotalWater = Math.Round(lastitem.TotalWater,0, MidpointRounding.AwayFromZero);
            if (head.LastTotalWater > 0)
            {
              var percent = ( head.TotalWater - head.LastTotalWater ) / head.LastTotalWater * 100;
              head.PerCent = percent;
            }
          }
        }
        
      }
    }

    public async Task<BillHeader> GetAdnUpdateLasterWater(string billid, string month="",int customerId=0)
    {
      var item = await this.Queryable().Where(x => x.BillNo == billid).FirstOrDefaultAsync();
      if (item != null)
      {
        var billdate = DateTime.Parse(item.Month + "1日");
        var lastbilldate = billdate.AddMonths(-1);
        var lastbillmonth = $"{lastbilldate.Year}年{lastbilldate.Month}月";
        var customerid = item?.CustomerId;
        var lastitem = await this.Queryable().Where(x => x.CustomerId == customerid && x.Month == lastbillmonth).FirstOrDefaultAsync();

        item.LastTotalWater = lastitem.TotalWater;
        if (item?.LastTotalWater > 0)
        {
          var percent = ( item.TotalWater - item.LastTotalWater ) / item.LastTotalWater * 100;
          item.PerCent = percent;
        }
        this.Update(item);

        return lastitem;
      }
      else
      {
        var billdate = DateTime.Parse(month + "1日");
        var lastbilldate = billdate.AddMonths(-1);
        var lastbillmonth = $"{lastbilldate.Year}年{lastbilldate.Month}月";
        var customerid = customerId;
        var lastitem = await this.Queryable().Where(x => x.CustomerId == customerid && x.Month == lastbillmonth).FirstOrDefaultAsync();
        return lastitem;
      }
    }

    public async Task Confirm(int[] id)
    {
      var items = await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach(var item in items)
      {
        item.Status = "已确认";
        item.TotalChineseAmount = item.TotalAmount.ChineseMoney();
        this.Update(item);
      }
    }
    public async Task SendToCustomer(int[] id,string path)
    {
      foreach (var i in id)
      {
        var model = await this.Queryable().Where(x => x.Id == i)
          .Include(x => x.BillDetails).FirstAsync();
        var customer =await this.customerService.FindAsync(model.CustomerId);
        var templatePath = Path.Combine(path, "ExcelTemplate\\billtemplate.xlsx");
        var output = Path.Combine(path,"UploadFiles\\BillDocs");
        var outputexcel = Path.Combine(output, model.BillNo + ".xlsx");
        var outputpdf = Path.Combine(output, model.BillNo + ".pdf");
        if (!Directory.Exists(output))
        {
          Directory.CreateDirectory(output);
        }
        var template = new XLTemplate(templatePath);
        template.AddVariable(model);
        template.Generate();
        template.SaveAs(outputexcel);

        var workbook = new Workbook();
        workbook.LoadFromFile(outputexcel, ExcelVersion.Version2010);
        workbook.SaveToFile(outputpdf, Spire.Xls.FileFormat.PDF);

        SendToMail(model.Month,model.BillNo, outputpdf,customer.ContactInfo);
        model.HasSend = true;
        model.SendDateTime = DateTime.Now;
        this.logger.Info($"电子账单:[{model.BillNo}]发送成功,To:{customer.ContactInfo}");
      }
    }

    private void SendToMail(string month, string billno, string file, string to) {

      using (var stream = new FileStream(file, FileMode.Open))
      {
        try
        {
          var attachment = new Attachment(stream, billno + ".pdf", System.Net.Mime.MediaTypeNames.Application.Pdf);
          var smtpClient = new SmtpClient();

          //var networkCredential = new NetworkCredential("new163@163.com", "BEVHMTHMXIRBHRXR");
          //smtpClient.Credentials = networkCredential;
          var from = ( smtpClient.Credentials as NetworkCredential ).UserName;
           var message = new MailMessage(from, to, month + "电子账单", "电子账单");
          message.Attachments.Add(attachment);
          smtpClient.Send(message);
        }catch(Exception e)
        {
          stream.Close();
          throw e;
        }
      }

    }

    public async Task<Stream> PrintBill(string path,IEnumerable<int> selectid) {
      var bills = await this.Queryable().Where(x => selectid.Contains(x.Id))
        .Include(x => x.BillDetails).ToListAsync();
  
      var stream = new MemoryStream();
      var templatePath = Path.Combine(path, "ExcelTemplate\\billtemplate2.xlsx");

      var template = new XLTemplate(templatePath);
      template.AddVariable(new {
        Headers = bills
        });
      template.Generate();
      template.SaveAs(stream);
      return stream;
    }
  }
}