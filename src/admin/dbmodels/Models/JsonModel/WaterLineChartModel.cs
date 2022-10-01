using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.JsonModel
{
  public class ChainWaterMeterDataModel {
    public string year { get; set; }
    public string month { get; set; }
    public string meterId { get; set; }
    public decimal water { get; set; }
  }
  public class ChainCustomerWaterMeterDataModel
  {
    public string year { get; set; }
    public string month { get; set; }
    public string customer { get; set; }
    public decimal water { get; set; }
  }
  public class WaterLineChartModel
  {
    public string meterId { get; set; }
    public DateTime datetime { get; set; }
    public decimal increment { get; set; }
  }
  public class CustomerLineChartModel
  {
    public string customerId { get; set; }
    public DateTime datetime { get; set; }
    public decimal increment { get; set; }
  }
  public class WaterDayLineChartModel
  {
    public string meterId { get; set; }
    public string date { get; set; }
    public decimal water { get; set; }
  }

  public class MeterWaterModel
  {
    public string meterId { get; set; }
    public decimal water { get; set; }
  }
  public class zonesummary {
    public string zone { get; set; }
    public string date { get; set; }
    public decimal water { get; set; }
    public decimal percent { get; set; }
  }
  public class customertypesummary
  {
    public string type { get; set; }
    public string date { get; set; }
    public decimal water { get; set; }
    public decimal percent { get; set; }
  }
  public class mainwaterdaily
  {
    public string date { get; set; }
    public decimal inval { get; set; }
    public decimal outval { get; set; }
  }

  public class totalmainwatermonth {
    public decimal totalin { get; set; }
    public decimal totalout { get; set; }
    public decimal totalstd { get; set; }

    }
  public class typewater {
    public string type { get; set; }
    public decimal water { get; set; }
    }
  public class zonewater {
    public string zone { get; set; }
    public decimal water { get; set; }
    }
}