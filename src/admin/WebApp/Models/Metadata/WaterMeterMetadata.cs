using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="WaterMeterMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/25/2020 9:40:58 AM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(WaterMeterMetadata))]
    public partial class WaterMeter
    {
    }

    public partial class WaterMeterMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.WaterMeter))]
        public int Id { get; set; }

        [Display(Name = "meterId",Description ="表号",Prompt = "表号",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(20)]
        public string meterId { get; set; }

        [Display(Name = "Status",Description ="水表状态",Prompt = "水表状态",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(10)]
        public string Status { get; set; }

        [Display(Name = "valveStatus",Description ="阀门状态",Prompt = "阀门状态",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(20)]
        public string valveStatus { get; set; }

        [Display(Name = "address",Description ="小区/楼栋/单元/门牌号",Prompt = "小区/楼栋/单元/门牌号",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(200)]
        public string address { get; set; }

        [Display(Name = "meterSize",Description ="口径大小",Prompt = "口径大小",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(50)]
        public string meterSize { get; set; }

        [Display(Name = "meterType",Description ="水表类型",Prompt = "水表类型",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(20)]
        public string meterType { get; set; }

        [Display(Name = "vender",Description ="厂家",Prompt = "厂家",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(50)]
        public string vender { get; set; }

        [Required(ErrorMessage = "Please enter : 检修周期")]
        [Display(Name = "repairCycle",Description ="检修周期",Prompt = "检修周期",ResourceType = typeof(resource.WaterMeter))]
        public int repairCycle { get; set; }

        [Required(ErrorMessage = "Please enter : 水表精度")]
        [Display(Name = "precision",Description ="水表精度",Prompt = "水表精度",ResourceType = typeof(resource.WaterMeter))]
        public decimal precision { get; set; }

        [Display(Name = "produced",Description ="生产日期",Prompt = "生产日期",ResourceType = typeof(resource.WaterMeter))]
        public DateTime produced { get; set; }

        [Display(Name = "period",Description ="有效日期",Prompt = "有效日期",ResourceType = typeof(resource.WaterMeter))]
        public DateTime period { get; set; }

        [Display(Name = "sealNumber1",Description ="鉴定铅封号",Prompt = "鉴定铅封号",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(30)]
        public string sealNumber1 { get; set; }

        [Display(Name = "sealNumber2",Description ="监管铅封号",Prompt = "监管铅封号",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(30)]
        public string sealNumber2 { get; set; }

        [Display(Name = "sealNumber3",Description ="售水铅封号",Prompt = "售水铅封号",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(30)]
        public string sealNumber3 { get; set; }

        [Display(Name = "imei",Description ="IMEI",Prompt = "IMEI",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(50)]
        public string imei { get; set; }

        [Required(ErrorMessage = "Please enter : 水表当前读数(吨)")]
        [Display(Name = "water",Description ="水表当前读数(吨)",Prompt = "水表当前读数(吨)",ResourceType = typeof(resource.WaterMeter))]
        public decimal water { get; set; }

        [Required(ErrorMessage = "Please enter : 累计流量")]
        [Display(Name = "reverseWater",Description ="累计流量",Prompt = "累计流量",ResourceType = typeof(resource.WaterMeter))]
        public decimal reverseWater { get; set; }

        [Required(ErrorMessage = "Please enter : 温度")]
        [Display(Name = "temperature",Description ="温度",Prompt = "温度",ResourceType = typeof(resource.WaterMeter))]
        public decimal temperature { get; set; }

        [Required(ErrorMessage = "Please enter : 瞬时流量")]
        [Display(Name = "flowrate",Description ="瞬时流量",Prompt = "瞬时流量",ResourceType = typeof(resource.WaterMeter))]
        public decimal flowrate { get; set; }

        [Required(ErrorMessage = "Please enter : 压力 (bar)")]
        [Display(Name = "pressure",Description ="压力 (bar)",Prompt = "压力 (bar)",ResourceType = typeof(resource.WaterMeter))]
        public decimal pressure { get; set; }

        [Required(ErrorMessage = "Please enter : 电压")]
        [Display(Name = "voltage",Description ="电压",Prompt = "电压",ResourceType = typeof(resource.WaterMeter))]
        public decimal voltage { get; set; }

        [Required(ErrorMessage = "Please enter : 水压预警阈值")]
        [Display(Name = "threshold1",Description ="水压预警阈值",Prompt = "水压预警阈值",ResourceType = typeof(resource.WaterMeter))]
        public decimal threshold1 { get; set; }

        [Required(ErrorMessage = "Please enter : 流量预警阈值")]
        [Display(Name = "threshold2",Description ="流量预警阈值",Prompt = "流量预警阈值",ResourceType = typeof(resource.WaterMeter))]
        public decimal threshold2 { get; set; }

        [Required(ErrorMessage = "Please enter : 温度预警阈值")]
        [Display(Name = "threshold3",Description ="温度预警阈值",Prompt = "温度预警阈值",ResourceType = typeof(resource.WaterMeter))]
        public decimal threshold3 { get; set; }

        [Required(ErrorMessage = "Please enter : 电压预警阈值")]
        [Display(Name = "threshold4",Description ="电压预警阈值",Prompt = "电压预警阈值",ResourceType = typeof(resource.WaterMeter))]
        public decimal threshold4 { get; set; }

        [Display(Name = "userCode",Description ="用户编码",Prompt = "用户编码",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(20)]
        public string userCode { get; set; }

        [Display(Name = "userName",Description ="用户名",Prompt = "用户名",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(20)]
        public string userName { get; set; }

        [Display(Name = "Remark",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(280)]
        public string Remark { get; set; }

        [Required(ErrorMessage = "Please enter : 单位ID")]
        [Display(Name = "CustomerId",Description ="单位ID",Prompt = "单位ID",ResourceType = typeof(resource.WaterMeter))]
        public int CustomerId { get; set; }

        [Display(Name = "CustomerName",Description ="单位名称",Prompt = "单位名称",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(50)]
        public string CustomerName { get; set; }

        [Display(Name = "OrgName",Description ="所属租户",Prompt = "所属租户",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(80)]
        public string OrgName { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.WaterMeter))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.WaterMeter))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.WaterMeter))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.WaterMeter))]
        public int TenantId { get; set; }

    }

}
