using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.JsonModel
{
  //用户水表开关阀接口
  public class updateOperateValveStatusRequest
  {
    public int count { get; set; }
    public int type { get; set; }
    public string[] data { get; set; }
    public int valve { get; set; }
    public string checkCode { get; set; }
  }

  public class ValveStatus
  {
    public string result { get; set; }
    public string message { get; set; }
    public string valveStatus { get; set; }
    public string meterId { get; set; }
    public string userId { get; set; }
  }

  public class updateOperateValveStatusResponse
  {
    public string result { get; set; }
    public IEnumerable<ValveStatus> data { get; set; }
    public string message { get; set; }
  }
}