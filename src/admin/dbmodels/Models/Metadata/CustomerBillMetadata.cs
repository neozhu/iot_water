using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="CustomerBillMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/28/2020 3:02:50 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(CustomerBillMetadata))]
    public partial class CustomerBill
    {
    }

    public partial class CustomerBillMetadata
    {
        [Display(Name = "Customer",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.CustomerBill))]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.CustomerBill))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 用水单位")]
        [Display(Name = "CustomerId",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.CustomerBill))]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please enter : 年份")]
        [Display(Name = "Year",Description ="年份",Prompt = "年份",ResourceType = typeof(resource.CustomerBill))]
        public int Year { get; set; }

        [Required(ErrorMessage = "Please enter : 月份")]
        [Display(Name = "Month",Description ="月份",Prompt = "月份",ResourceType = typeof(resource.CustomerBill))]
        public int Month { get; set; }

        [Display(Name = "Status",Description ="状态",Prompt = "状态",ResourceType = typeof(resource.CustomerBill))]
        [MaxLength(10)]
        public string Status { get; set; }

        [Required(ErrorMessage = "Please enter : 本期用水量")]
        [Display(Name = "water",Description ="本期用水量",Prompt = "本期用水量",ResourceType = typeof(resource.CustomerBill))]
        public decimal water { get; set; }

        [Required(ErrorMessage = "Please enter : 水价")]
        [Display(Name = "Price",Description ="水价",Prompt = "水价",ResourceType = typeof(resource.CustomerBill))]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter : 折扣")]
        [Display(Name = "Discount",Description ="折扣",Prompt = "折扣",ResourceType = typeof(resource.CustomerBill))]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Please enter : 金额")]
        [Display(Name = "Amount",Description ="金额",Prompt = "金额",ResourceType = typeof(resource.CustomerBill))]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please enter : 账单日期")]
        [Display(Name = "BillDate",Description ="账单日期",Prompt = "账单日期",ResourceType = typeof(resource.CustomerBill))]
        public DateTime BillDate { get; set; }

        [Display(Name = "Remark",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.CustomerBill))]
        [MaxLength(200)]
        public string Remark { get; set; }

        [Display(Name = "CustomerName",Description ="单位名称",Prompt = "单位名称",ResourceType = typeof(resource.CustomerBill))]
        [MaxLength(50)]
        public string CustomerName { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.CustomerBill))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.CustomerBill))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.CustomerBill))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.CustomerBill))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.CustomerBill))]
        public int TenantId { get; set; }

    }

}
