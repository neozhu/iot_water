using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="CustomerWaterRecordMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/25/2020 10:41:38 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(CustomerWaterRecordMetadata))]
    public partial class CustomerWaterRecord
    {
    }

    public partial class CustomerWaterRecordMetadata
    {
        [Display(Name = "Customer",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.CustomerWaterRecord))]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.CustomerWaterRecord))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 用水单位")]
        [Display(Name = "CustomerId",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.CustomerWaterRecord))]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please enter : 年份")]
        [Display(Name = "Year",Description ="年份",Prompt = "年份",ResourceType = typeof(resource.CustomerWaterRecord))]
        public int Year { get; set; }

        [Required(ErrorMessage = "Please enter : 月份")]
        [Display(Name = "Month",Description ="月份",Prompt = "月份",ResourceType = typeof(resource.CustomerWaterRecord))]
        public int Month { get; set; }

        [Display(Name = "meterId",Description ="表号",Prompt = "表号",ResourceType = typeof(resource.CustomerWaterRecord))]
        [MaxLength(20)]
        public string meterId { get; set; }

        [Required(ErrorMessage = "Please enter : 上期抄表日期")]
        [Display(Name = "previousDate",Description ="上期抄表日期",Prompt = "上期抄表日期",ResourceType = typeof(resource.CustomerWaterRecord))]
        public decimal previousDate { get; set; }

        [Required(ErrorMessage = "Please enter : 上期表见")]
        [Display(Name = "previousWater",Description ="上期表见",Prompt = "上期表见",ResourceType = typeof(resource.CustomerWaterRecord))]
        public decimal previousWater { get; set; }

        [Required(ErrorMessage = "Please enter : 本期表见")]
        [Display(Name = "lastWater",Description ="本期表见",Prompt = "本期表见",ResourceType = typeof(resource.CustomerWaterRecord))]
        public decimal lastWater { get; set; }

        [Required(ErrorMessage = "Please enter : 本期用水量")]
        [Display(Name = "water",Description ="本期用水量",Prompt = "本期用水量",ResourceType = typeof(resource.CustomerWaterRecord))]
        public decimal water { get; set; }

        [Required(ErrorMessage = "Please enter : 抄表日期")]
        [Display(Name = "RecordDate",Description ="抄表日期",Prompt = "抄表日期",ResourceType = typeof(resource.CustomerWaterRecord))]
        public DateTime RecordDate { get; set; }

        [Display(Name = "User",Description ="抄表人",Prompt = "抄表人",ResourceType = typeof(resource.CustomerWaterRecord))]
        [MaxLength(20)]
        public string User { get; set; }

        [Display(Name = "Type",Description ="抄表方式",Prompt = "抄表方式",ResourceType = typeof(resource.CustomerWaterRecord))]
        [MaxLength(10)]
        public string Type { get; set; }

        [Required(ErrorMessage = "Please enter : 是否删除")]
        [Display(Name = "IsDelete",Description ="是否删除",Prompt = "是否删除",ResourceType = typeof(resource.CustomerWaterRecord))]
        public bool IsDelete { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.CustomerWaterRecord))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.CustomerWaterRecord))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.CustomerWaterRecord))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.CustomerWaterRecord))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.CustomerWaterRecord))]
        public int TenantId { get; set; }

    }

}
