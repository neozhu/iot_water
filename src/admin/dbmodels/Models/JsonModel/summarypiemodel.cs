using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.JsonModel
{
  public class summarypiemodel
  {
    public string type { get; set; }
    public decimal total { get; set; }
    public decimal percent { get; set; }
  }


  public class summarywaterbyhour {

    public DateTime date { get; set; }
    public int hour { get; set; }
    public decimal water { get; set; }
    }

  public class summarywaterbyday
  {

    public DateTime date { get; set; }
    public decimal water { get; set; }
  }

  public class summarywaterbymonth
  {

    public string month { get; set; }
    public decimal water { get; set; }
    public decimal previous_water { get; set; }
    public decimal? vs_previous_water { get; set; }
  }
  public class summarywaterbyyear
  {

    public string year { get; set; }
    public decimal water { get; set; }
    public decimal previous_water { get; set; }
    public decimal? vs_previous_water { get; set; }
  }
}