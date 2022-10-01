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
  public partial class CustomerWaterRecord:Entity
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
    [Display(Name = "表号", Description = "表号")]
    [MaxLength(20)]
    public string meterId { get; set; }
    [Display(Name = "上期抄表日期", Description = "上期抄表日期")]
    public DateTime? previousDate { get; set; }
    [Display(Name = "上期表见", Description = "上期表见")]
    public decimal previousWater { get; set; }
    [Display(Name = "本期表见", Description = "本期表见")]
    public decimal lastWater { get; set; }
    [Display(Name = "本期用水量", Description = "本期用水量")]
    public decimal water { get; set; }
    [Display(Name = "抄表日期", Description = "抄表日期")]
    [DefaultValue("now")]
    public DateTime RecordDate { get; set; }
    [Display(Name = "抄表人", Description = "抄表人")]
    [MaxLength(20)]
    public string User { get; set; }
    [Display(Name = "抄表方式", Description = "抄表方式")]
    [MaxLength(10)]
    [DefaultValue("人工")]
    public string Type { get; set; }
    [Display(Name = "是否删除", Description = "是否删除")]
    public bool IsDelete { get; set; }


  }
}