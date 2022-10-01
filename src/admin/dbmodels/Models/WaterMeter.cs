using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //水表
  public partial class WaterMeter : Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "表名", Description = "表名")]
    [MaxLength(50)]
    public string Name { get; set; }
    [Display(Name = "表序号", Description = "表序号")]
    [MaxLength(50)]
    public string LineNo { get; set; }
    [Display(Name = "表号", Description = "表号")]
    [MaxLength(20)]
    public string meterId { get; set; }
    [Display(Name = "出线名称", Description = "出线名称")]
    [MaxLength(50)]
    public string Name1 { get; set; }
    [Display(Name = "倍率", Description = "倍率")]
    public decimal? Rate { get; set; }
    [Display(Name = "水表状态", Description = "水表状态")]
    [MaxLength(10)]
    [DefaultValue("使用中")]
    public string Status { get; set; }
    [Display(Name = "阀门状态", Description = "阀门状态")]
    [MaxLength(20)]
    [DefaultValue(null)]
    public string valveStatus { get; set; }
    
    
    [Display(Name = "安装位置", Description = "安装位置")]
    [MaxLength(200)]
    public string address { get; set; }
    [Display(Name = "口径大小", Description = "口径大小")]
    [MaxLength(50)]
    public string meterSize { get; set; }
    [Display(Name = "水表类型", Description = "水表类型")]
    [MaxLength(20)]
    [DefaultValue("")]
    public string meterType { get; set; }
    [Display(Name = "厂家", Description = "厂家")]
    [MaxLength(50)]
    public string vender { get; set; }
    [Display(Name = "检修周期", Description = "检修周期")]
    public int repairCycle { get; set; }

    [Display(Name = "水表精度", Description = "水表精度")]
    public decimal precision { get; set; }
    [Display(Name = "生产日期", Description = "生产日期")]
    [DefaultValue(null)]
    public DateTime? produced { get; set; }
    [Display(Name = "有效日期", Description = "有效日期")]
    [DefaultValue(null)]
    public DateTime? period { get; set; }
    [Display(Name = "鉴定铅封号", Description = "鉴定铅封号")]
    [MaxLength(30)]
    public string sealNumber1 { get; set; }
    [Display(Name = "监管铅封号", Description = "监管铅封号")]
    [MaxLength(30)]
    public string sealNumber2 { get; set; }
    [Display(Name = "售水铅封号", Description = "售水铅封号")]
    [MaxLength(30)]
    public string sealNumber3 { get; set; }
    [Display(Name = "IMEI", Description = "IMEI")]
    [MaxLength(50)]
    public string imei { get; set; }
    [Display(Name = "当前读数", Description = "水表当前读数(吨)")]
    public decimal water { get; set; }
    [Display(Name = "累计流量", Description = "累计流量")]
    public decimal reverseWater { get; set; }
    [Display(Name = "温度", Description = "温度")]
    public decimal? temperature { get; set; }
    [Display(Name = "瞬时流量", Description = "瞬时流量")]
    public decimal flowrate { get; set; }
    [Display(Name = "压力", Description = "压力 (bar)")]
    public decimal pressure { get; set; }
    [Display(Name = "电压", Description = "电压")]
    public decimal voltage { get; set; }
    [Display(Name = "水压预警阈值", Description = "水压预警阈值")]
    public decimal threshold1 { get; set; }
    [Display(Name = "流量预警阈值", Description = "流量预警阈值")]
    public decimal threshold2 { get; set; }
    [Display(Name = "温度预警阈值", Description = "温度预警阈值")]
    public decimal threshold3 { get; set; }
    [Display(Name = "电压预警阈值", Description = "电压预警阈值")]
    public decimal threshold4 { get; set; }
    [Display(Name = "用户编码", Description = "用户编码")]
    [MaxLength(20)]
    public string userCode { get; set; }
    [Display(Name = "用户名", Description = "用户名")]
    [MaxLength(20)]
    public string userName { get; set; }
   
    [Display(Name = "备注", Description = "备注")]
    [MaxLength(280)]
    public string Remark { get; set; }
    [Display(Name = "单位ID", Description = "单位ID")]
    public int CustomerId { get; set; }
    [Display(Name = "使用单位", Description = "使用单位")]
    [MaxLength(50)]
    public string CustomerName { get; set; }
    [Display(Name = "最近一次抄表读数", Description = "最近一次抄表读数")]
    public decimal? LastWater { get; set; }
    [Display(Name = "最近一次抄表日期", Description = "最近一次抄表日期")]
    public DateTime? LastWaterDate { get; set; }

    [Display(Name = "所属租户", Description = "所属租户")]
    [MaxLength(80)]
    public string OrgName { get; set; }
    [Display(Name = "开通日期", Description = "开通日期")]
    public DateTime? OpeningDate { get; set; }
    [Display(Name = "停用日期", Description = "停用日期")]
    public DateTime? ClosedDate { get; set; }


    [Display(Name = "层级", Description = "层级")]
    [DefaultValue(null)]
    [MaxLength(10)]
    public string Level { get; set; }

    [Display(Name = "区域", Description = "区域")]
    [MaxLength(50)]
    public string ZoneName { get; set; }

    [Display(Name = "经度", Description = "经度")]
    [DefaultValue(null)]
    public decimal? longitude { get; set; }
    [Display(Name = "维度", Description = "维度")]
    [DefaultValue(null)]
    public decimal? latitude { get; set; }
  }
}