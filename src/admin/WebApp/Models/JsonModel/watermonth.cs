using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.JsonModel
{
  public class waterpoint {
    public string meterId { get; set; }
    public string points { get; set; }
    public decimal water { get; set; }
  }
  public class watermonth
  {
    public string meterId { get; set; }
    public string year { get; set; }
    public string month { get; set; }
    public decimal water { get; set; }

  }

  public class customermonth
  {
    public string customer { get; set; }
    public string year { get; set; }
    public string month { get; set; }
    public decimal water { get; set; }

  }
  public class customerquotamonth {
    public string customer { get; set; }
    public string year { get; set; }
    public string month{ get; set; }
    public decimal water { get; set; }
    public decimal quota { get; set; }
    public decimal forecast { get; set; }
    public decimal percent { get; set; }
    }
  public class alarmcount {
    public string level { get; set; }
    public int count { get; set; }
    }
}