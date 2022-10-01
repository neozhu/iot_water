using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //总表进出日报
  public partial class MeterWaterDailyRecord:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "表号", Description = "表号")]
    [MaxLength(20)]
    public string meterId { get; set; }
    public DateTime date { get; set; }
    public decimal waterin { get; set; }
    public decimal waterout { get; set; }
  }
}