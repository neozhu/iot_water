using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace WebApp.Models.ViewModel
{

  [SugarTable("meterwaterminanalyse")]
  public class meterwaterminanalyse
  {
    public string id { get; set; }
    public string meterid { get; set; }
    public string name { get; set; }
    public string name1 { get; set; }
    public DateTime date { get; set; }
    public int times { get; set; }
    public decimal min {get;set;}
    public decimal max { get; set; }
    public decimal avg { get; set; }
    public decimal? per { get; set; }
  }
}