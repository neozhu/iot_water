using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="CustomerMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/25/2020 9:58:43 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
    }

    public partial class CustomerMetadata
    {
        [Display(Name = "CustomerWaterRecords",Description ="CustomerWaterRecords",Prompt = "CustomerWaterRecords",ResourceType = typeof(resource.Customer))]
        public CustomerWaterRecord CustomerWaterRecords { get; set; }

        [Display(Name = "WaterMeters",Description ="WaterMeters",Prompt = "WaterMeters",ResourceType = typeof(resource.Customer))]
        public CustomerWaterMeter WaterMeters { get; set; }

        [Display(Name = "WaterQuotas",Description ="WaterQuotas",Prompt = "WaterQuotas",ResourceType = typeof(resource.Customer))]
        public CustomerWaterQuota WaterQuotas { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.Customer))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 单位名称")]
        [Display(Name = "Name",Description ="单位名称",Prompt = "单位名称",ResourceType = typeof(resource.Customer))]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Level",Description ="单位层级",Prompt = "单位层级",ResourceType = typeof(resource.Customer))]
        [MaxLength(50)]
        public string Level { get; set; }

        [Display(Name = "Dept",Description ="管理部门",Prompt = "管理部门",ResourceType = typeof(resource.Customer))]
        [MaxLength(20)]
        public string Dept { get; set; }

        [Display(Name = "Contact",Description ="主要联系人",Prompt = "主要联系人",ResourceType = typeof(resource.Customer))]
        [MaxLength(50)]
        public string Contact { get; set; }

        [Display(Name = "ContactInfo",Description ="联系方式",Prompt = "联系方式",ResourceType = typeof(resource.Customer))]
        [MaxLength(50)]
        public string ContactInfo { get; set; }

        [Display(Name = "MobilePhone",Description ="短信通知手机号",Prompt = "短信通知手机号",ResourceType = typeof(resource.Customer))]
        [MaxLength(50)]
        public string MobilePhone { get; set; }

        [Required(ErrorMessage = "Please enter : 用水定额(月)")]
        [Display(Name = "Quota",Description ="用水定额(月)",Prompt = "用水定额(月)",ResourceType = typeof(resource.Customer))]
        public decimal Quota { get; set; }

        [Required(ErrorMessage = "Please enter : 预警阈值")]
        [Display(Name = "Threshold",Description ="预警阈值",Prompt = "预警阈值",ResourceType = typeof(resource.Customer))]
        public decimal Threshold { get; set; }

        [Required(ErrorMessage = "Please enter : 是否计费")]
        [Display(Name = "IsFee",Description ="是否计费",Prompt = "是否计费",ResourceType = typeof(resource.Customer))]
        public bool IsFee { get; set; }

        [Required(ErrorMessage = "Please enter : 折扣")]
        [Display(Name = "Discount",Description ="折扣",Prompt = "折扣",ResourceType = typeof(resource.Customer))]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Please enter : 注册日期")]
        [Display(Name = "RegisterDate",Description ="注册日期",Prompt = "注册日期",ResourceType = typeof(resource.Customer))]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "Remark",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.Customer))]
        [MaxLength(150)]
        public string Remark { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.Customer))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.Customer))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.Customer))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.Customer))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.Customer))]
        public int TenantId { get; set; }

    }

}
