using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="DepartmentMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2019 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>10/22/2019 10:39:56 AM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(DepartmentMetadata))]
    public partial class Department
    {
    }

    public partial class DepartmentMetadata
    {
        [Display(Name = "Company",Description ="所在公司",Prompt = "所在公司",ResourceType = typeof(resource.Department))]
        public Company Company { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.Department))]
        public int Id { get; set; }

        [Display(Name = "Name",Description ="部门名称",Prompt = "部门名称",ResourceType = typeof(resource.Department))]
        [MaxLength(10)]
        public string Name { get; set; }

        [Display(Name = "Manager",Description ="部门主管",Prompt = "部门主管",ResourceType = typeof(resource.Department))]
        [MaxLength(10)]
        public string Manager { get; set; }

        [Required(ErrorMessage = "Please enter : 所在公司")]
        [Display(Name = "CompanyId",Description ="所在公司",Prompt = "所在公司",ResourceType = typeof(resource.Department))]
        public int CompanyId { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.Department))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.Department))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.Department))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.Department))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.Department))]
        public int TenantId { get; set; }

    }

}
