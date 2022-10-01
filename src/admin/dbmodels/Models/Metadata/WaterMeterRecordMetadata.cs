using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="WaterMeterRecordMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/25/2020 8:18:06 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(WaterMeterRecordMetadata))]
    public partial class WaterMeterRecord
    {
    }

    public partial class WaterMeterRecordMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.WaterMeterRecord))]
        public int Id { get; set; }

        [Display(Name = "meterStatus",Description ="水表状态",Prompt = "水表状态",ResourceType = typeof(resource.WaterMeterRecord))]
        [MaxLength(10)]
        public string meterStatus { get; set; }

        [Required(ErrorMessage = "Please enter : 采集时间")]
        [Display(Name = "datetime",Description ="采集时间",Prompt = "采集时间",ResourceType = typeof(resource.WaterMeterRecord))]
        public DateTime datetime { get; set; }

        [Display(Name = "meterId",Description ="表号",Prompt = "表号",ResourceType = typeof(resource.WaterMeterRecord))]
        [MaxLength(20)]
        public string meterId { get; set; }

        [Required(ErrorMessage = "Please enter : 水表当前读数(吨)")]
        [Display(Name = "water",Description ="水表当前读数(吨)",Prompt = "水表当前读数(吨)",ResourceType = typeof(resource.WaterMeterRecord))]
        public decimal water { get; set; }

        [Required(ErrorMessage = "Please enter : 累计流量")]
        [Display(Name = "reverseWater",Description ="累计流量",Prompt = "累计流量",ResourceType = typeof(resource.WaterMeterRecord))]
        public decimal reverseWater { get; set; }

        [Required(ErrorMessage = "Please enter : 温度")]
        [Display(Name = "temperature",Description ="温度",Prompt = "温度",ResourceType = typeof(resource.WaterMeterRecord))]
        public decimal temperature { get; set; }

        [Required(ErrorMessage = "Please enter : 瞬时流量")]
        [Display(Name = "flowrate",Description ="瞬时流量",Prompt = "瞬时流量",ResourceType = typeof(resource.WaterMeterRecord))]
        public decimal flowrate { get; set; }

        [Required(ErrorMessage = "Please enter : 压力 (bar)")]
        [Display(Name = "pressure",Description ="压力 (bar)",Prompt = "压力 (bar)",ResourceType = typeof(resource.WaterMeterRecord))]
        public decimal pressure { get; set; }

        [Required(ErrorMessage = "Please enter : 电压")]
        [Display(Name = "voltage",Description ="电压",Prompt = "电压",ResourceType = typeof(resource.WaterMeterRecord))]
        public decimal voltage { get; set; }

        [Display(Name = "valveStatus",Description ="阀门状态",Prompt = "阀门状态",ResourceType = typeof(resource.WaterMeterRecord))]
        [MaxLength(20)]
        public string valveStatus { get; set; }

        [Display(Name = "userId",Description ="用户编号",Prompt = "用户编号",ResourceType = typeof(resource.WaterMeterRecord))]
        [MaxLength(20)]
        public string userId { get; set; }

        [Display(Name = "imei",Description ="IMEI",Prompt = "IMEI",ResourceType = typeof(resource.WaterMeterRecord))]
        [MaxLength(50)]
        public string imei { get; set; }

        [Display(Name = "OrgName",Description ="所属单位",Prompt = "所属单位",ResourceType = typeof(resource.WaterMeterRecord))]
        [MaxLength(80)]
        public string OrgName { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.WaterMeterRecord))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.WaterMeterRecord))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.WaterMeterRecord))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.WaterMeterRecord))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.WaterMeterRecord))]
        public int TenantId { get; set; }

    }

}
