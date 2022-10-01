using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  public partial class BillDetail:Entity
  {
    public int BillId { get; set; }
    [ForeignKey("BillId")]
    public BillHeader BillHeader { get; set; }
    [Display(Name = "表名", Description = "表名")]
    [MaxLength(50)]
    public string MeterName { get; set; }
    [Display(Name = "表序号", Description = "表序号")]
    [MaxLength(50)]
    public string LineNo { get; set; }
    [Display(Name = "表号", Description = "表号")]
    [MaxLength(20)]
    public string MeterId { get; set; }
    [Display(Name = "出线名称", Description = "出线名称")]
    [MaxLength(50)]
    public string MeterName1 { get; set; }
    [Display(Name = "安装位置", Description = "安装位置")]
    [MaxLength(150)]
    public string MeterPoint { get; set; }

    [Display(Name = "正负项", Description = "正负项")]
    [MaxLength(10)]
    public string Negitive { get; set; }
    [Display(Name = "占比", Description = "占比")]
    public decimal Ratio { get; set; }
    [Display(Name = "倍率", Description = "倍率")]
    public decimal? Rate { get; set; }

    [Display(Name = "用水量(m3)", Description = "用水量(m3)")]
    public decimal Water { get; set; }
    [Display(Name = "上个月用水量(m3)", Description = "上个月用水量(m3)")]
    public decimal? LastWater { get; set; }
    [Display(Name = "环比(%)", Description = "环比(%)")]
    public decimal? PerCent { get; set; }

    [Display(Name = "实际用水量(m3)", Description = "实际用水量(m3)")]
    public decimal ActualWater { get; set; }

    [Display(Name = "期初日期", Description = "期初日期")]
    public DateTime? WaterDt1 { get; set; }
    [Display(Name = "期初水量(m3)", Description = "期初水量(m3)")]
    public decimal? Water1 { get; set; }
    [Display(Name = "期末日期", Description = "期末日期")]
    public DateTime? WaterDt2 { get; set; }
    [Display(Name = "期末水量(m3)", Description = "期末水量(m3)")]
    public decimal? Water2 { get; set; }
    [Display(Name = "备注", Description = "备注")]
    [MaxLength(250)]
    public string Remark { get; set; }
  }
}