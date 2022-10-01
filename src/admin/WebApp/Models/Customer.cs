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
  //单位客户信息
  public partial class Customer:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "客户编码", Description = "客户编码")]
    [MaxLength(50)]
    [Index(IsUnique = true)]
    public string Code { get; set; }
    [Display(Name = "单位名称", Description = "单位名称")]
    [MaxLength(50)]
    [Required]
    [Index(IsUnique =true)]
    public string Name { get; set; }
 
    [Display(Name = "客户类型", Description = "客户类型")]
    [MaxLength(50)]
    public string Category { get; set; }
    [Display(Name = "状态", Description = "状态")]
    [MaxLength(10)]
    public string Status { get; set; }
    [Display(Name = "业态", Description = "业态")]
    [MaxLength(20)]
    [DefaultValue("自用")]
    public string Type { get; set; }
    [Display(Name = "单位层级", Description = "单位层级")]
    public string Level { get; set; }
    [Display(Name = "主管部门", Description = "主管部门")]
    [MaxLength(20)]
    public string ManageDept { get; set; }
    [Display(Name = "管水部门", Description = "管水部门")]
    [MaxLength(20)]
    public string Dept { get; set; }

    [Display(Name = "主要联系人", Description = "主要联系人")]
    [MaxLength(50)]
    public string Contact { get; set; }
    [Display(Name = "电子邮件", Description = "电子邮件")]
    [MaxLength(50)]
    public string ContactInfo { get; set; }
    [Display(Name = "短信通知手机号", Description = "短信通知手机号")]
    [MaxLength(50)]
    public string MobilePhone { get; set; }

    [Display(Name = "用水定额", Description = "用水定额(月)")]
    [DefaultValue(0)]
    public decimal Quota { get; set; }
    [Display(Name = "预警阈值", Description = "预警阈值")]
    [DefaultValue(0)]
    public decimal Threshold { get; set; }
    [Display(Name = "水单价(元/m3)", Description = "水单价(元/m3)")]
    [DefaultValue(0)]
    public decimal WaterPrice { get; set; }
    [Display(Name = "水服务费单价(元/m3)", Description = "水服务费单价(元/m3)")]
    [DefaultValue(0)]
    public decimal ServicePrice { get; set; }

    [Display(Name = "是否计费", Description = "是否计费")]
    [DefaultValue(false)]
    public bool IsFee { get; set; }
    [Display(Name = "折扣", Description = "折扣")]
    [DefaultValue(0)]
    public decimal Discount { get; set; }
    [Display(Name = "注册日期", Description = "注册日期")]
    [DefaultValue("now")]
    public DateTime RegisterDate { get; set; }
    [Display(Name = "备注", Description = "备注")]
    [MaxLength(150)]
    public string Remark { get; set; }
   

    public Customer()
    {
      this.WaterMeters = new HashSet<CustomerWaterMeter>();
      this.WaterQuotas = new HashSet<CustomerWaterQuota>();
      this.CustomerWaterRecords = new HashSet<CustomerWaterRecord>();
    }

    public virtual ICollection<CustomerWaterMeter> WaterMeters { get; set; }
    public virtual ICollection<CustomerWaterQuota> WaterQuotas { get; set; }

    public virtual ICollection<CustomerWaterRecord> CustomerWaterRecords { get; set; }
  }
}