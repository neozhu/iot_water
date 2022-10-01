using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //水表历史记录
  public partial class WaterMeterSumByDay : Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "日期", Description = "日期")]
    public DateTime datetime { get; set; }
    [Display(Name = "表号", Description = "表号")]
    [MaxLength(20)]
    public string meterId { get; set; }
    [Display(Name = "水表当前读数", Description = "水表当前读数(吨)")]
    public decimal water { get; set; }
    [Display(Name = "累计流量", Description = "累计流量")]
    public decimal reverseWater { get; set; }
    [Display(Name = "用户编号", Description = "用户编号")]
    [MaxLength(20)]
    public string userId { get; set; }
    [Display(Name = "IMEI", Description = "IMEI")]
    [MaxLength(50)]
    public string imei { get; set; }
    [Display(Name = "所属单位", Description = "所属单位")]
    [MaxLength(80)]
    public string OrgName { get; set; }
  }
}