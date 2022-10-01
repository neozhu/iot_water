using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="ZoneWaterMeterMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/28/2020 6:45:25 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(ZoneWaterMeterMetadata))]
    public partial class ZoneWaterMeter
    {
    }

    public partial class ZoneWaterMeterMetadata
    {
        [Display(Name = "Zone",Description ="所在区域",Prompt = "所在区域",ResourceType = typeof(resource.ZoneWaterMeter))]
        public Zone Zone { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.ZoneWaterMeter))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 所在区域")]
        [Display(Name = "ZoneId",Description ="所在区域",Prompt = "所在区域",ResourceType = typeof(resource.ZoneWaterMeter))]
        public int ZoneId { get; set; }

        [Required(ErrorMessage = "Please enter : 关系")]
        [Display(Name = "Direct",Description ="关系",Prompt = "关系",ResourceType = typeof(resource.ZoneWaterMeter))]
        public int Direct { get; set; }

        [Display(Name = "meterId",Description ="表号",Prompt = "表号",ResourceType = typeof(resource.ZoneWaterMeter))]
        [MaxLength(20)]
        public string meterId { get; set; }

        [Display(Name = "ZoneName",Description ="区域",Prompt = "区域",ResourceType = typeof(resource.ZoneWaterMeter))]
        [MaxLength(50)]
        public string ZoneName { get; set; }

        [Display(Name = "longitude",Description ="经度",Prompt = "经度",ResourceType = typeof(resource.ZoneWaterMeter))]
        public decimal longitude { get; set; }

        [Display(Name = "latitude",Description ="维度",Prompt = "维度",ResourceType = typeof(resource.ZoneWaterMeter))]
        public decimal latitude { get; set; }

        [Display(Name = "Detail",Description ="安装位置",Prompt = "安装位置",ResourceType = typeof(resource.ZoneWaterMeter))]
        [MaxLength(80)]
        public string Detail { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.ZoneWaterMeter))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.ZoneWaterMeter))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.ZoneWaterMeter))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.ZoneWaterMeter))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.ZoneWaterMeter))]
        public int TenantId { get; set; }

    }

}
