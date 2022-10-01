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
  public partial class BillHeader:Entity
  {
    [Display(Name = "账单编号", Description = "账单编号")]
    [MaxLength(12)]
    [Required]
    public string BillNo { get; set; }

    [NotMapped]
    public string Year { get {
      return this.Month.Substring(0, 4);
        } }

    [NotMapped]
    public string Month2
    {
      get
      {
        var m = this.Month.Split(new string[] { "年" }, StringSplitOptions.RemoveEmptyEntries)[1];
        return m.Substring(0,m.Length-1);
      }
    }
    [NotMapped]
    public string PaymentDate1 {
      get
      {
        return this.PaymentDate?.ToString("yyyy-MM-dd");
      }

      }
    [NotMapped]
    public string CreatedDate1
    {
      get
      {
        return this.CreatedDate?.ToString("yyyy-MM-dd");
      }

    }
    [Display(Name = "用水单位", Description = "用水单位")]
    [DefaultValue(null)]
    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    [Display(Name = "用水单位", Description = "用水单位")]
    public Customer Customer { get; set; }
    [Display(Name = "单位名称", Description = "单位名称")]
    [MaxLength(50)]
    [DefaultValue(null)]
    public string CustomerName { get; set; }
    [Display(Name = "单位编号", Description = "单位编号")]
    [MaxLength(50)]
    [DefaultValue(null)]
    public string CustomerCode { get; set; }
    [Display(Name = "客户类型", Description = "客户类型")]
    [MaxLength(50)]
    public string Category { get; set; }

    [Display(Name = "水单价(元/m3)", Description = "水单价(元/m3)")]
    [DefaultValue(0)]
    public decimal WaterPrice { get; set; }
    [Display(Name = "水服务费单价(元/m3)", Description = "水服务费单价(元/m3)")]
    [DefaultValue(0)]
    public decimal ServicePrice { get; set; }
    [Display(Name = "折扣", Description = "折扣")]
    [DefaultValue(0)]
    public decimal Discount { get; set; }
    [Display(Name = "账单日期", Description = "账单日期")]
    [DefaultValue("now")]
    public DateTime BillDate { get; set; }
    [Display(Name = "缴费截至日期", Description = "缴费截至日期")]
    [DefaultValue("now")]
    public DateTime? PaymentDate { get; set; }
  

    [Display(Name = "收费日期", Description = "收费日期")]
    [DefaultValue(null)]
    public DateTime? ReceiptDate { get; set; }
    [Display(Name = "月份", Description = "月份")]
    [MaxLength(12)]
    public string Month { get; set; }
    [Display(Name = "总水量(m3)", Description = "总水量(m3)")]
    [DefaultValue(0)]
    public decimal TotalWater { get; set; }
    [Display(Name = "上个月总用水量(m3)", Description = "上个月总用水量(m3)")]
    public decimal? LastTotalWater { get; set; }
    [Display(Name = "环比(%)", Description = "环比(%)")]
    public decimal? PerCent { get; set; }
    [Display(Name = "总水价(元)", Description = "总水价(元)")]
    [DefaultValue(0)]
    public decimal TotalWaterAmount { get; set; }
   
    [Display(Name = "总水服务费(元)", Description = "总水服务费(元)")]
    [DefaultValue(0)]
    public decimal TotalServiceAmount { get; set; }
    [Display(Name = "补扣水量(m3)", Description = "补扣水量(m3)")]
    [DefaultValue(0)]
    public decimal AdjustWater { get; set; }
    [Display(Name = "补扣水费(元)", Description = "补扣水费(元)")]
    [DefaultValue(0)]
    public decimal AdjustWaterAmount { get; set; }
    [Display(Name = "补扣服务费(元)", Description = "补扣服务费(元)")]
    [DefaultValue(0)]
    public decimal AdjustServiceAmount { get; set; }

    [Display(Name = "总费用(元)", Description = "总费用(元)")]
    [DefaultValue(0)]
    public decimal TotalAmount { get; set; }
    [Display(Name = "大写金额", Description = "大写金额")]
   [MaxLength(50)]
    public string TotalChineseAmount { get; set; }
    [Display(Name = "总应收费用(元)", Description = "总应收费用(元)")]
    [DefaultValue(0)]
    public decimal TotalReceivableAmount { get; set; }
    [Display(Name = "状态", Description = "状态")]
    [MaxLength(20)]
    [Required]
    public string Status { get; set; }
    [Display(Name = "是否发送账单", Description = "是否发送账单")]
    public bool HasSend { get; set; }
    [Display(Name = "发送日期", Description = "发送日期")]
    public DateTime? SendDateTime { get; set; }
    [Display(Name = "备注", Description = "备注")]
    [MaxLength(250)]
    public string Remark { get; set; }

    public BillHeader()
    {
      BillDetails = new HashSet<BillDetail>();
    }
    public virtual ICollection<BillDetail> BillDetails { get; set; }
  }
}