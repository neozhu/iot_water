using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="CustomerWaterQuotaMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/25/2020 10:36:21 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(CustomerWaterQuotaMetadata))]
    public partial class CustomerWaterQuota
    {
    }

    public partial class CustomerWaterQuotaMetadata
    {
        [Display(Name = "Customer",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.CustomerWaterQuota))]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.CustomerWaterQuota))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 用水单位")]
        [Display(Name = "CustomerId",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.CustomerWaterQuota))]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please enter : 年份")]
        [Display(Name = "Year",Description ="年份",Prompt = "年份",ResourceType = typeof(resource.CustomerWaterQuota))]
        public int Year { get; set; }

        [Required(ErrorMessage = "Please enter : 月份")]
        [Display(Name = "Month",Description ="月份",Prompt = "月份",ResourceType = typeof(resource.CustomerWaterQuota))]
        public int Month { get; set; }

        [Required(ErrorMessage = "Please enter : 用水定额(月)")]
        [Display(Name = "Quota",Description ="用水定额(月)",Prompt = "用水定额(月)",ResourceType = typeof(resource.CustomerWaterQuota))]
        public decimal Quota { get; set; }

        [Required(ErrorMessage = "Please enter : 已用水(吨)")]
        [Display(Name = "Water",Description ="已用水(吨)",Prompt = "已用水(吨)",ResourceType = typeof(resource.CustomerWaterQuota))]
        public decimal Water { get; set; }

        [Required(ErrorMessage = "Please enter : 预测用水量(吨)")]
        [Display(Name = "ForecastWater",Description ="预测用水量(吨)",Prompt = "预测用水量(吨)",ResourceType = typeof(resource.CustomerWaterQuota))]
        public decimal ForecastWater { get; set; }

        [Required(ErrorMessage = "Please enter : 计算日期")]
        [Display(Name = "RecordDate",Description ="计算日期",Prompt = "计算日期",ResourceType = typeof(resource.CustomerWaterQuota))]
        public DateTime RecordDate { get; set; }

        [Display(Name = "CustomerName",Description ="单位名称",Prompt = "单位名称",ResourceType = typeof(resource.CustomerWaterQuota))]
        [MaxLength(50)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please enter : 是否删除")]
        [Display(Name = "IsDelete",Description ="是否删除",Prompt = "是否删除",ResourceType = typeof(resource.CustomerWaterQuota))]
        public bool IsDelete { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.CustomerWaterQuota))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.CustomerWaterQuota))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.CustomerWaterQuota))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.CustomerWaterQuota))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.CustomerWaterQuota))]
        public int TenantId { get; set; }

    }

}
