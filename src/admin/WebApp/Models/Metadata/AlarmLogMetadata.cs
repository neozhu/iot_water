using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="AlarmLogMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/29/2020 5:57:19 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(AlarmLogMetadata))]
    public partial class AlarmLog
    {
    }

    public partial class AlarmLogMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.AlarmLog))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 发生时间")]
        [Display(Name = "InitDateTime",Description ="发生时间",Prompt = "发生时间",ResourceType = typeof(resource.AlarmLog))]
        public DateTime InitDateTime { get; set; }

        [Required(ErrorMessage = "Please enter : 对象")]
        [Display(Name = "DeviceId",Description ="对象",Prompt = "对象",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(50)]
        public string DeviceId { get; set; }

        [Display(Name = "Content",Description ="报警内容",Prompt = "报警内容",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(300)]
        public string Content { get; set; }

        [Display(Name = "Status",Description ="状态",Prompt = "状态",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(30)]
        public string Status { get; set; }

        [Required(ErrorMessage = "Please enter : 类型")]
        [Display(Name = "Type",Description ="类型",Prompt = "类型",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(30)]
        public string Type { get; set; }

        [Required(ErrorMessage = "Please enter : 报警等级")]
        [Display(Name = "Level",Description ="报警等级",Prompt = "报警等级",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(30)]
        public string Level { get; set; }

        [Display(Name = "User",Description ="提交人",Prompt = "提交人",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(20)]
        public string User { get; set; }

        [Display(Name = "ToUser",Description ="处理人",Prompt = "处理人",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(20)]
        public string ToUser { get; set; }

        [Display(Name = "BeginDateTime",Description ="开始处理时间",Prompt = "开始处理时间",ResourceType = typeof(resource.AlarmLog))]
        public DateTime BeginDateTime { get; set; }

        [Display(Name = "CompletedDateTime",Description ="完成时间",Prompt = "完成时间",ResourceType = typeof(resource.AlarmLog))]
        public DateTime CompletedDateTime { get; set; }

        [Display(Name = "Result",Description ="处理结果",Prompt = "处理结果",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(300)]
        public string Result { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.AlarmLog))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.AlarmLog))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.AlarmLog))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.AlarmLog))]
        public int TenantId { get; set; }

    }

}
