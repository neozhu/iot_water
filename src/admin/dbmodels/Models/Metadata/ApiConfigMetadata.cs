using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="ApiConfigMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/25/2020 10:46:21 AM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(ApiConfigMetadata))]
    public partial class ApiConfig
    {
    }

    public partial class ApiConfigMetadata
    {
        [Display(Name = "Company",Description ="所属企业",Prompt = "所属企业",ResourceType = typeof(resource.ApiConfig))]
        public Company Company { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.ApiConfig))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 服务名")]
        [Display(Name = "ServiceName",Description ="服务名",Prompt = "服务名",ResourceType = typeof(resource.ApiConfig))]
        [MaxLength(50)]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Please enter : 主机地址")]
        [Display(Name = "Host",Description ="主机地址",Prompt = "主机地址",ResourceType = typeof(resource.ApiConfig))]
        [MaxLength(50)]
        public string Host { get; set; }

        [Display(Name = "SecretAccessKey",Description ="安全代码",Prompt = "安全代码",ResourceType = typeof(resource.ApiConfig))]
        [MaxLength(250)]
        public string SecretAccessKey { get; set; }

        [Display(Name = "AccessKeyId",Description ="访问代码",Prompt = "访问代码",ResourceType = typeof(resource.ApiConfig))]
        [MaxLength(250)]
        public string AccessKeyId { get; set; }

        [Display(Name = "UserId",Description ="登录用户名",Prompt = "登录用户名",ResourceType = typeof(resource.ApiConfig))]
        [MaxLength(50)]
        public string UserId { get; set; }

        [Display(Name = "Password",Description ="登录密码",Prompt = "登录密码",ResourceType = typeof(resource.ApiConfig))]
        [MaxLength(50)]
        public string Password { get; set; }

        [Display(Name = "Description",Description ="描述",Prompt = "描述",ResourceType = typeof(resource.ApiConfig))]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter : 所属企业")]
        [Display(Name = "CompanyId",Description ="所属企业",Prompt = "所属企业",ResourceType = typeof(resource.ApiConfig))]
        public int CompanyId { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.ApiConfig))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.ApiConfig))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.ApiConfig))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.ApiConfig))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.ApiConfig))]
        public int TenantId { get; set; }

    }

}
