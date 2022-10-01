using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //总水表每日登记表
  public partial class MainMeter : Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name ="日期", Description = "日期")]
    [DefaultValue("now")]
    public DateTime date { get; set; }
    [Display(Name = "进水表现", Description = "进水表现")]
    public decimal inwater{get;set;}
    [Display(Name = "当天进水量", Description = "当天进水量")]
    public decimal involume { get; set; }

    [Display(Name = "出水表现", Description = "出水表现")]
    public decimal outwater { get; set; }
    [Display(Name = "当天用水量", Description = "当天用水量")]
    public decimal outvolume { get; set; }
    [Display(Name = "备注", Description = "备注")]
    [MaxLength(120)]
    public string remark { get; set; }
  }
}