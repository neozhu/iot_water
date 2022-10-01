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
  //单位客户用表关系
  public partial class CustomerWaterMeter:Entity
  {

    [Display(Name ="用水单位",Description = "用水单位")]
    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    [Display(Name = "用水单位", Description = "用水单位")]
    public Customer Customer { get; set; }

    [Display(Name = "部门", Description = "部门")]
    [MaxLength(20)]
    public string Dept { get; set; }


    [Display(Name = "表号", Description = "表号")]
    [MaxLength(20)]
    public string meterId { get; set; }
    [Display(Name = "表名", Description = "表名")]
    [MaxLength(50)]
    public string meterName { get; set; }
    [Display(Name = "用水定额", Description = "用水定额(月)")]
    [DefaultValue(0)]
    public decimal Quota { get; set; }
    [Display(Name = "是否总表", Description = "是否总表")]
    [DefaultValue(false)]
    public bool IsFee { get; set; }
    [Display(Name = "层级", Description = "层级")]
    [DefaultValue(null)]
    [MaxLength(10)]
    public string Level { get; set; }
    [Display(Name = "折扣", Description = "折扣")]
    [DefaultValue(0)]
    public decimal Discount { get; set; }
    [Display(Name = "正负项", Description = "正负项")]
    [MaxLength(10)]
    public string Negitive { get; set; }
    [Display(Name = "占比", Description = "占比")]
    [DefaultValue(1)]
    public decimal Ratio { get; set; }
    [Display(Name = "安装位置", Description = "安装位置")]
    [MaxLength(150)]
    public string Remark { get; set; }
    [Display(Name = "出线名称", Description = "出线名称")]
    public string Points { get; set; }
    [Display(Name = "区域", Description = "区域")]
    [MaxLength(50)]
    public string ZoneName { get; set; }

    [Display(Name = "单位名称", Description = "单位名称")]
    [MaxLength(50)]
    public string CustomerName { get; set; }
    [Display(Name = "单位编号", Description = "单位编号")]
    [MaxLength(50)]
    public string CustomerCode { get; set; }
    [Display(Name = "是否停用", Description = "是否停用")]
    public bool IsDelete { get; set; }
    [Display(Name = "注册日期", Description = "注册日期")]
    [DefaultValue("now")]
    public DateTime RegisterDate { get; set; }
    [Display(Name = "停用日期", Description = "停用日期")]
    public DateTime? DeleteDate { get; set; }
  }
}