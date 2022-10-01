using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="LogMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2019 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>9/19/2019 8:51:55 AM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(LogMetadata))]
    public partial class Log
    {
    }

    public partial class LogMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.Log))]
        public int Id { get; set; }

        [Display(Name = "MachineName",Description ="主机名",Prompt = "主机名",ResourceType = typeof(resource.Log))]
        [MaxLength(50)]
        public string MachineName { get; set; }

        [Display(Name = "Logged",Description ="时间",Prompt = "时间",ResourceType = typeof(resource.Log))]
        public DateTime Logged { get; set; }

        [Display(Name = "Level",Description ="级别",Prompt = "级别",ResourceType = typeof(resource.Log))]
        [MaxLength(5)]
        public string Level { get; set; }

        [Display(Name = "Message",Description ="信息",Prompt = "信息",ResourceType = typeof(resource.Log))]
        [MaxLength(50)]
        public string Message { get; set; }

        [Display(Name = "Exception",Description ="异常信息",Prompt = "异常信息",ResourceType = typeof(resource.Log))]
        [MaxLength(50)]
        public string Exception { get; set; }

        [Display(Name = "Properties",Description ="事件属性",Prompt = "事件属性",ResourceType = typeof(resource.Log))]
        [MaxLength(50)]
        public string Properties { get; set; }

        [Display(Name = "User",Description ="使用账号",Prompt = "使用账号",ResourceType = typeof(resource.Log))]
        [MaxLength(50)]
        public string User { get; set; }

        [Display(Name = "Logger",Description ="日志",Prompt = "日志",ResourceType = typeof(resource.Log))]
        [MaxLength(30)]
        public string Logger { get; set; }

        [Display(Name = "Callsite",Description ="站点",Prompt = "站点",ResourceType = typeof(resource.Log))]
        [MaxLength(150)]
        public string Callsite { get; set; }

        [Required(ErrorMessage = "Please enter : 已处理")]
        [Display(Name = "Resolved",Description ="已处理",Prompt = "已处理",ResourceType = typeof(resource.Log))]
        public bool Resolved { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.Log))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.Log))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.Log))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.Log))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.Log))]
        public int TenantId { get; set; }

    }

}
