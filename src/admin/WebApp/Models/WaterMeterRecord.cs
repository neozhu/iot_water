using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CsvHelper.Configuration.Attributes;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //水表历史记录
  public partial class WaterMeterRecord:Entity
  {
    [Key]
    [Ignore]
    public int Id { get; set; }
    [Display(Name = "水表状态", Description = "水表状态")]
    [MaxLength(10)]
    [Name("水表状态")]
    public string meterStatus { get; set; }
    [Display(Name = "采集时间", Description = "采集时间")]
    //[Index("IX_WaterMeterRecord", IsUnique = true, Order = 3)]
    [Name("采集时间")]
    public DateTime datetime { get; set; }

    [Display(Name = "表号", Description = "表号")]
    [MaxLength(20)]
    [Name("表号")]
    //[Index("IX_WaterMeterRecord",IsUnique =true,Order =1)]
    public string meterId { get; set; }
    [Display(Name = "累计流量(m3)", Description = "累计流量(m3)")]
    //[Index("IX_WaterMeterRecord", IsUnique = true, Order = 2)]
    [Name("累计流量(m3)")]
    public decimal water { get; set; }
    [Display(Name = "反向流量(m3)", Description = "反向流量(m3)")]
    [Name("反向流量(m3)")]
    public decimal reverseWater { get; set; }
    [Display(Name = "温度", Description = "温度")]
    [Ignore]
    public decimal temperature { get; set; }
    
    [Display(Name = "瞬时流量(m3/h)", Description = "瞬时流量(m3/h)")]
    [Name("瞬时流量(m3/h)")]
    public decimal flowrate { get; set; }
    [Display(Name = "压力(bar)", Description = "压力 (bar)")]
    [Ignore]
    public decimal pressure { get; set; }

    [Display(Name = "电压", Description = "电压")]
    [Ignore]
    public decimal voltage { get; set; }
    [Display(Name = "阀门状态", Description = "阀门状态")]
    [MaxLength(20)]
    [Name("阀门状态")]
    public string valveStatus { get; set; }
    [Display(Name = "用户编号", Description = "用户编号")]
    [MaxLength(20)]
    [Ignore]
    public string userId { get; set; }
    [Display(Name = "IMEI", Description = "IMEI")]
    [MaxLength(50)]
    [Name("IMEI")]
    public string imei { get; set; }
    [Display(Name = "所属单位", Description = "所属单位")]
    [MaxLength(80)]
    [Ignore]
    public string OrgName { get; set; }
    [Display(Name = "客户ID", Description = "客户ID")]
    [Ignore]
    public int? CustomerId { get; set; }
    [Display(Name = "单位名称", Description = "单位名称")]
    [MaxLength(50)]
    [Name("单位名称")]
    public string CustomerName { get; set; }
    [Display(Name = "水表类型", Description = "水表类型")]
    [MaxLength(20)]
    public string meterType { get; set; }
  }
}