using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.JsonModel
{
  //提供指定时间用户水表末次水量及状态数据
  public class findQueryLastDataByDatetimeRequest
  {
    public int count { get; set; }
    public int type { get; set; }
    public string[] data { get; set; }
    public string datetime { get; set; }
    public string checkCode { get; set; }
  }

  //public class LastData
  //{
  //  public string meterStatus { get; set; }
  //  public string datetime { get; set; }
  //  public string meterId { get; set; }
  //  public decimal reverseWater { get; set; }
  //  public decimal temperature { get; set; }
  //  public decimal flowrate { get; set; }
  //  public decimal pressure { get; set; }
  //  public string userId { get; set; }
  //  public string valveStatus { get; set; }
  //  public decimal water { get; set; }
  //  public string voltage { get; set; }
  //}

  public class findQueryLastDataByDatetimeResponse
  {
    public string result { get; set; }
    public IEnumerable<MeterData> data { get; set; }
    public string message { get; set; }
    public int totalCount { get; set; }
  }
}