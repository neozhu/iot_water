using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.JsonModel
{
  public class Meter
  {
    public string address { get; set; }
    public string meterId { get; set; }
    public string imei { get; set; }
    public string meterSize { get; set; }
    public string userCode { get; set; }
    public string username { get; set; }
  }

  public class findQueryUserInfoResponse
  {
    public string result { get; set; }
    public IEnumerable<Meter> data { get; set; }
    public string message { get; set; }
    public int totalCount { get; set; }
  }

  public class findQueryUserInfoRequest
  {
    public int startNO { get; set; }
    public int endNo { get; set; }
    public string checkCode { get; set; }
  }


}