using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="EmployeeMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/22/2020 6:03:02 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
    }

    public partial class EmployeeMetadata
    {
        [Display(Name = "Company",Description ="公司信息",Prompt = "公司信息",ResourceType = typeof(resource.Employee))]
        public Company Company { get; set; }

        [Display(Name = "Department",Description ="部门信息",Prompt = "部门信息",ResourceType = typeof(resource.Employee))]
        public Department Department { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.Employee))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 姓名")]
        [Display(Name = "Name",Description ="姓名",Prompt = "姓名",ResourceType = typeof(resource.Employee))]
        [MaxLength(20)]
        public string Name { get; set; }

        [Display(Name = "Title",Description ="职位",Prompt = "职位",ResourceType = typeof(resource.Employee))]
        [MaxLength(30)]
        public string Title { get; set; }

        [Display(Name = "PhoneNumber",Description ="联系电话",Prompt = "联系电话",ResourceType = typeof(resource.Employee))]
        [MaxLength(30)]
        public string PhoneNumber { get; set; }

        [Display(Name = "WX",Description ="微信",Prompt = "微信",ResourceType = typeof(resource.Employee))]
        [MaxLength(30)]
        public string WX { get; set; }

        [Required(ErrorMessage = "Please enter : 性别")]
        [Display(Name = "Sex",Description ="性别",Prompt = "性别",ResourceType = typeof(resource.Employee))]
        [MaxLength(10)]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Please enter : 年龄")]
        [Display(Name = "Age",Description ="年龄",Prompt = "年龄",ResourceType = typeof(resource.Employee))]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please enter : 出生日期")]
        [Display(Name = "Brithday",Description ="出生日期",Prompt = "出生日期",ResourceType = typeof(resource.Employee))]
        public DateTime Brithday { get; set; }

        [Required(ErrorMessage = "Please enter : 入职日期")]
        [Display(Name = "EntryDate",Description ="入职日期",Prompt = "入职日期",ResourceType = typeof(resource.Employee))]
        public DateTime EntryDate { get; set; }

        [Required(ErrorMessage = "Please enter : 是否已离职")]
        [Display(Name = "IsDeleted",Description ="是否已离职",Prompt = "是否已离职",ResourceType = typeof(resource.Employee))]
        public bool IsDeleted { get; set; }

        [Display(Name = "LeaveDate",Description ="离职日期",Prompt = "离职日期",ResourceType = typeof(resource.Employee))]
        public DateTime LeaveDate { get; set; }

        [Required(ErrorMessage = "Please enter : 公司")]
        [Display(Name = "CompanyId",Description ="公司",Prompt = "公司",ResourceType = typeof(resource.Employee))]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Please enter : 部门")]
        [Display(Name = "DepartmentId",Description ="部门",Prompt = "部门",ResourceType = typeof(resource.Employee))]
        public int DepartmentId { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.Employee))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.Employee))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.Employee))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.Employee))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.Employee))]
        public int TenantId { get; set; }

    }

}
