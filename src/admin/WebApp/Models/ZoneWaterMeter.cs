using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //区域内水表关系
  public partial class ZoneWaterMeter:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "所在区域", Description = "所在区域")]
    public int ZoneId { get; set; }
    [ForeignKey("ZoneId")]
    [Display(Name = "所在区域", Description = "所在区域")]
    public Zone Zone { get; set; }
    [Display(Name = "关系", Description = "关系")]
    [DefaultValue(1)]
    public int Direct { get; set; }
    [Display(Name = "表号", Description = "表号")]
    [MaxLength(20)]
    public string meterId { get; set; }
    [Display(Name = "区域", Description = "区域")]
    [MaxLength(50)]
    public string ZoneName { get; set; }

    [Display(Name = "经度", Description = "经度")]
    [DefaultValue(null)]
    public decimal? longitude { get; set; }
    [Display(Name = "维度", Description = "维度")]
    [DefaultValue(null)]
    public decimal? latitude { get; set; }
    [Display(Name = "安装位置", Description = "安装位置")]
    [MaxLength(80)]
    public string Detail { get; set; }

  }
}