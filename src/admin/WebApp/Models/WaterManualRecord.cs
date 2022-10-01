using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  public partial class WaterManualRecord:Entity
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
    [Display(Name = "安装位置", Description = "安装位置")]
    [MaxLength(50)]
    public string Address { get; set; }
    [Display(Name = "本期读数", Description = "本期读数")]
    public decimal Water { get; set; }
    [Display(Name = "抄表日期", Description = "抄表日期")]
    public DateTime RecordDate { get; set; }
 
    [Display(Name = "备注", Description = "备注")]
    [MaxLength(250)]
    public string Remark { get; set; }
  }
}