using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="CustomerWaterMeterMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/25/2020 10:27:45 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(CustomerWaterMeterMetadata))]
    public partial class CustomerWaterMeter
    {
    }

    public partial class CustomerWaterMeterMetadata
    {
        [Display(Name = "Customer",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.CustomerWaterMeter))]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.CustomerWaterMeter))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 用水单位")]
        [Display(Name = "CustomerId",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.CustomerWaterMeter))]
        public int CustomerId { get; set; }

        [Display(Name = "meterId",Description ="表号",Prompt = "表号",ResourceType = typeof(resource.CustomerWaterMeter))]
        [MaxLength(20)]
        public string meterId { get; set; }

        [Required(ErrorMessage = "Please enter : 用水定额(月)")]
        [Display(Name = "Quota",Description ="用水定额(月)",Prompt = "用水定额(月)",ResourceType = typeof(resource.CustomerWaterMeter))]
        public decimal Quota { get; set; }

        [Required(ErrorMessage = "Please enter : 是否计费")]
        [Display(Name = "IsFee",Description ="是否计费",Prompt = "是否计费",ResourceType = typeof(resource.CustomerWaterMeter))]
        public bool IsFee { get; set; }

        [Required(ErrorMessage = "Please enter : 折扣")]
        [Display(Name = "Discount",Description ="折扣",Prompt = "折扣",ResourceType = typeof(resource.CustomerWaterMeter))]
        public decimal Discount { get; set; }

        [Display(Name = "Remark",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.CustomerWaterMeter))]
        [MaxLength(150)]
        public string Remark { get; set; }

        [Display(Name = "CustomerName",Description ="单位名称",Prompt = "单位名称",ResourceType = typeof(resource.CustomerWaterMeter))]
        [MaxLength(50)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please enter : 是否删除")]
        [Display(Name = "IsDelete",Description ="是否删除",Prompt = "是否删除",ResourceType = typeof(resource.CustomerWaterMeter))]
        public bool IsDelete { get; set; }

        [Required(ErrorMessage = "Please enter : 注册日期")]
        [Display(Name = "RegisterDate",Description ="注册日期",Prompt = "注册日期",ResourceType = typeof(resource.CustomerWaterMeter))]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "DeleteDate",Description ="截止日期",Prompt = "截止日期",ResourceType = typeof(resource.CustomerWaterMeter))]
        public DateTime DeleteDate { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.CustomerWaterMeter))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.CustomerWaterMeter))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.CustomerWaterMeter))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.CustomerWaterMeter))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.CustomerWaterMeter))]
        public int TenantId { get; set; }

    }

}
