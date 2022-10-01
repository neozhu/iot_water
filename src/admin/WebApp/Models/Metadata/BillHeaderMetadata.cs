using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="BillHeaderMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2021 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2/19/2021 8:30:00 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(BillHeaderMetadata))]
    public partial class BillHeader
    {
    }

    public partial class BillHeaderMetadata
    {
        [Display(Name = "BillDetails",Description ="BillDetails",Prompt = "BillDetails",ResourceType = typeof(resource.BillHeader))]
        public BillDetail BillDetails { get; set; }

        [Display(Name = "Customer",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.BillHeader))]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Please enter : 系统主键")]
        [Display(Name = "Id",Description ="系统主键",Prompt = "系统主键",ResourceType = typeof(resource.BillHeader))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 账单编号")]
        [Display(Name = "BillNo",Description ="账单编号",Prompt = "账单编号",ResourceType = typeof(resource.BillHeader))]
        [MaxLength(12)]
        public string BillNo { get; set; }

        [Required(ErrorMessage = "Please enter : 用水单位")]
        [Display(Name = "CustomerId",Description ="用水单位",Prompt = "用水单位",ResourceType = typeof(resource.BillHeader))]
        public int CustomerId { get; set; }

        [Display(Name = "CustomerName",Description ="单位名称",Prompt = "单位名称",ResourceType = typeof(resource.BillHeader))]
        [MaxLength(50)]
        public string CustomerName { get; set; }

        [Display(Name = "Category",Description ="客户类型",Prompt = "客户类型",ResourceType = typeof(resource.BillHeader))]
        [MaxLength(50)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please enter : 水单价(元/m3)")]
        [Display(Name = "WaterPrice",Description ="水单价(元/m3)",Prompt = "水单价(元/m3)",ResourceType = typeof(resource.BillHeader))]
        public decimal WaterPrice { get; set; }

        [Required(ErrorMessage = "Please enter : 水服务费单价(元/m3)")]
        [Display(Name = "ServicePrice",Description ="水服务费单价(元/m3)",Prompt = "水服务费单价(元/m3)",ResourceType = typeof(resource.BillHeader))]
        public decimal ServicePrice { get; set; }

        [Required(ErrorMessage = "Please enter : 折扣")]
        [Display(Name = "Discount",Description ="折扣",Prompt = "折扣",ResourceType = typeof(resource.BillHeader))]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Please enter : 账单日期")]
        [Display(Name = "BillDate",Description ="账单日期",Prompt = "账单日期",ResourceType = typeof(resource.BillHeader))]
        public DateTime BillDate { get; set; }

        [Display(Name = "ReceiptDate",Description ="收费日期",Prompt = "收费日期",ResourceType = typeof(resource.BillHeader))]
        public DateTime ReceiptDate { get; set; }

        [Display(Name = "Month",Description ="月份",Prompt = "月份",ResourceType = typeof(resource.BillHeader))]
        [MaxLength(12)]
        public string Month { get; set; }

        [Required(ErrorMessage = "Please enter : 总水量(m3)")]
        [Display(Name = "TotalWater",Description ="总水量(m3)",Prompt = "总水量(m3)",ResourceType = typeof(resource.BillHeader))]
        public decimal TotalWater { get; set; }

        [Display(Name = "LastTotalWater",Description ="上个月总用水量(m3)",Prompt = "上个月总用水量(m3)",ResourceType = typeof(resource.BillHeader))]
        public decimal LastTotalWater { get; set; }

        [Display(Name = "PerCent",Description ="环比(%)",Prompt = "环比(%)",ResourceType = typeof(resource.BillHeader))]
        public decimal PerCent { get; set; }

        [Required(ErrorMessage = "Please enter : 总水价(元)")]
        [Display(Name = "TotalWaterAmount",Description ="总水价(元)",Prompt = "总水价(元)",ResourceType = typeof(resource.BillHeader))]
        public decimal TotalWaterAmount { get; set; }

        [Required(ErrorMessage = "Please enter : 总水服务费(元)")]
        [Display(Name = "TotalServiceAmount",Description ="总水服务费(元)",Prompt = "总水服务费(元)",ResourceType = typeof(resource.BillHeader))]
        public decimal TotalServiceAmount { get; set; }

        [Required(ErrorMessage = "Please enter : 补扣水量(m3)")]
        [Display(Name = "AdjustWater",Description ="补扣水量(m3)",Prompt = "补扣水量(m3)",ResourceType = typeof(resource.BillHeader))]
        public decimal AdjustWater { get; set; }

        [Required(ErrorMessage = "Please enter : 补扣水费(元)")]
        [Display(Name = "AdjustWaterAmount",Description ="补扣水费(元)",Prompt = "补扣水费(元)",ResourceType = typeof(resource.BillHeader))]
        public decimal AdjustWaterAmount { get; set; }

        [Required(ErrorMessage = "Please enter : 补扣服务费(元)")]
        [Display(Name = "AdjustServiceAmount",Description ="补扣服务费(元)",Prompt = "补扣服务费(元)",ResourceType = typeof(resource.BillHeader))]
        public decimal AdjustServiceAmount { get; set; }

        [Required(ErrorMessage = "Please enter : 总费用(元)")]
        [Display(Name = "TotalAmount",Description ="总费用(元)",Prompt = "总费用(元)",ResourceType = typeof(resource.BillHeader))]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Please enter : 总应收费用(元)")]
        [Display(Name = "TotalReceivableAmount",Description ="总应收费用(元)",Prompt = "总应收费用(元)",ResourceType = typeof(resource.BillHeader))]
        public decimal TotalReceivableAmount { get; set; }

        [Required(ErrorMessage = "Please enter : 状态")]
        [Display(Name = "Status",Description ="状态",Prompt = "状态",ResourceType = typeof(resource.BillHeader))]
        [MaxLength(20)]
        public string Status { get; set; }

        [Display(Name = "Remark",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.BillHeader))]
        [MaxLength(250)]
        public string Remark { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.BillHeader))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.BillHeader))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.BillHeader))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.BillHeader))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : 租户主键")]
        [Display(Name = "TenantId",Description ="租户主键",Prompt = "租户主键",ResourceType = typeof(resource.BillHeader))]
        public int TenantId { get; set; }

    }

}
