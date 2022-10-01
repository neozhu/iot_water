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
  //单位客户用水定额记录表
  public partial class CustomerWaterQuota:Entity
  {
    [Key]
    public int Id { get; set; }

    [Display(Name = "用水单位", Description = "用水单位")]
    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    [Display(Name = "用水单位", Description = "用水单位")]
    public Customer Customer { get; set; }
    [Display(Name = "年份", Description = "年份")]
    public int Year { get; set; }
    [Display(Name = "月份", Description = "月份")]
    public int Month { get; set; }
    [Display(Name = "用水定额", Description = "用水定额(月)")]
    [DefaultValue(0)]
    public decimal Quota { get; set; }
    [Display(Name = "已用水(吨)", Description = "已用水(吨)")]
    [DefaultValue(0)]
    public decimal Water { get; set; }
    [Display(Name = "预测用水量(吨)", Description = "预测用水量(吨)")]
    [DefaultValue(0)]
    public decimal ForecastWater { get; set; }

    [Display(Name = "计算日期", Description = "计算日期")]
    [DefaultValue(null)]
    public DateTime? RecordDate { get; set; }
    [Display(Name = "单位名称", Description = "单位名称")]
    [MaxLength(50)]
    public string CustomerName { get; set; }

    [Display(Name = "是否删除", Description = "是否删除")]
    public bool IsDelete { get; set; }

  }
}