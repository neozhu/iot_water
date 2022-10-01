using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  public partial class WaterManualReport:Entity
  {
    [Display(Name = "表号", Description = "表号")]
    [MaxLength(20)]
    public string meterId { get; set; }

    [Display(Name = "表名", Description = "表名")]
    [MaxLength(50)]
    public string Name { get; set; }
    [Display(Name = "表序号", Description = "表序号")]
    [MaxLength(50)]
    public string LineNo { get; set; }

    [Display(Name = "出线名称", Description = "出线名称")]
    [MaxLength(50)]
    public string Name1 { get; set; }
    [Display(Name = "本期读数", Description = "本期读数")]
    public decimal Water { get; set; }
    [Display(Name = "抄表日期", Description = "抄表日期")]
    public DateTime RecordDate { get; set; }

    [Display(Name = "上期读数", Description = "上期读数")]
    public decimal?  LastWater { get; set; }
    [Display(Name = "上期抄表日期", Description = "上期抄表日期")]
    public DateTime? LastRecordDate { get; set; }
    [Display(Name = "本期用水量", Description = "本期用水量")]
    public decimal? CalWater { get; set; }
    [Display(Name = "上期用水量", Description = "上期用水量")]
    public decimal? LastCalWater { get; set; }
    [Display(Name = "同期用水量", Description = "同期用水量")]
    public decimal? OnYearCalWater { get; set; }
    [Display(Name = "抄表月份", Description = "抄表月份")]
    [MaxLength(12)]
    public string Month { get; set; }
    [Display(Name = "账单编号", Description = "账单编号")]
    [MaxLength(30)]
    public string BillNo { get; set; }
    //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Display(Name = "同比(%)", Description = "同比(%)")]
    public decimal? YearRatio {
      get;set;
      }
    //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Display(Name = "环比(%)", Description = "环比(%)")]
    public decimal? LastRatio
    {
      get;set;
    }
    [Display(Name = "备注", Description = "备注")]
    [MaxLength(250)]
    public string Remark { get; set; }

    [Display(Name = "水表类型", Description = "水表类型")]
    [MaxLength(20)]
    public string meterType { get; set; }
  }
}