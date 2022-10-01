using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //设备报警记录
  public partial class AlarmLog:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "发生时间", Description = "发生时间")]
    [DefaultValue("now")]
    public DateTime InitDateTime { get; set; }
    [Display(Name = "对象", Description = "对象")]
    [MaxLength(50)]
    [Required]
    public string DeviceId { get; set; }
    [Display(Name = "报警内容", Description = "报警内容")]
    [MaxLength(300)]
    public string Content { get; set; }
    [Display(Name = "状态", Description = "状态")]
    [MaxLength(30)]
    public string Status { get; set; }
    [Display(Name = "类型", Description = "类型")]
    [MaxLength(30)]
    [Required]
    public string Type { get; set; }

    [Display(Name ="报警等级", Description = "报警等级")]
    [MaxLength(30)]
    [Required]
    public string Level { get; set; }

    [Display(Name = "提交人", Description = "提交人")]
    [MaxLength(20)]
    public string User { get; set; }
    [Display(Name = "处理人", Description = "处理人")]
    [MaxLength(20)]
    public string ToUser { get; set; }
    [Display(Name = "开始处理时间", Description = "开始处理时间")]
    [DefaultValue(null)]
    public DateTime? BeginDateTime { get; set; }
    [Display(Name = "完成时间", Description = "完成时间")]
    [DefaultValue(null)]
    public DateTime? CompletedDateTime { get; set; }
    [Display(Name = "处理结果", Description = "处理结果")]
    [MaxLength(300)]
    public string Result { get; set; }

  }
}