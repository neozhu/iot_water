using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.JsonModel
{
  public class summaryreportdto
  {
    public string meterId { get; set; }
    public string Name { get; set; }
    public string Name1 { get; set; }
    public string LineNo { get; set; }
    public string start1 { get; set; }
    public decimal? minwater { get; set; }
    public string end1 { get; set; }
    public decimal? maxwater { get; set; }
    public decimal? water1 { get; set; }
    public decimal? reverseWater1 { get; set; }

    public string start2 { get; set; }
    public string end2 { get; set; }
    public decimal? water2 { get; set; }
    public decimal? reverseWater2 { get; set; }
    public decimal? rate { get; set; }

  }
  public class summaryreportdto2
  {
   public string zoneName { get; set; }
    public decimal? inwater { get; set; }
    public decimal? outwater { get; set; }
    public decimal? water { get; set; }
    public decimal? rate { get; set; }

  }
}