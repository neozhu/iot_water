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
  //客户水费账单
  public partial class CustomerBill : Entity
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
    [Display(Name = "状态", Description = "状态")]
    [MaxLength(10)]
    [DefaultValue("待确认")]
    public string Status { get; set; }
    [Display(Name = "本期用水量", Description = "本期用水量")]
    public decimal water { get; set; }
    [Display(Name = "水价", Description = "水价")]
    public decimal Price { get; set; }
    [Display(Name = "折扣", Description = "折扣")]

    public decimal Discount { get; set; }
    [Display(Name = "金额", Description = "金额")]
    public decimal Amount { get; set; }
    [Display(Name = "账单日期", Description = "账单日期")]
    public DateTime BillDate { get; set; }
    [Display(Name = "备注", Description = "备注")]
    [MaxLength(200)]
    public string Remark { get; set; }
    [Display(Name = "单位名称", Description = "单位名称")]
    [MaxLength(50)]
    public string CustomerName { get; set; }



  }
}