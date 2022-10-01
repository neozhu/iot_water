using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="BillDetailMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2021 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2/20/2021 9:15:34 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(BillDetailMetadata))]
    public partial class BillDetail
    {
    }

    public partial class BillDetailMetadata
    {
        [Display(Name = "BillHeader",Description ="BillHeader",Prompt = "BillHeader",ResourceType = typeof(resource.BillDetail))]
        public BillHeader BillHeader { get; set; }

        [Required(ErrorMessage = "Please enter : 系统主键")]
        [Display(Name = "Id",Description ="系统主键",Prompt = "系统主键",ResourceType = typeof(resource.BillDetail))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : BillId")]
        [Display(Name = "BillId",Description ="BillId",Prompt = "BillId",ResourceType = typeof(resource.BillDetail))]
        public int BillId { get; set; }

        [Display(Name = "MeterName",Description ="表名",Prompt = "表名",ResourceType = typeof(resource.BillDetail))]
        [MaxLength(50)]
        public string MeterName { get; set; }

        [Display(Name = "LineNo",Description ="表序号",Prompt = "表序号",ResourceType = typeof(resource.BillDetail))]
        [MaxLength(50)]
        public string LineNo { get; set; }

        [Display(Name = "MeterId",Description ="表号",Prompt = "表号",ResourceType = typeof(resource.BillDetail))]
        [MaxLength(20)]
        public string MeterId { get; set; }

        [Display(Name = "MeterName1",Description ="出线名称",Prompt = "出线名称",ResourceType = typeof(resource.BillDetail))]
        [MaxLength(50)]
        public string MeterName1 { get; set; }

        [Display(Name = "MeterPoint",Description ="安装位置",Prompt = "安装位置",ResourceType = typeof(resource.BillDetail))]
        [MaxLength(150)]
        public string MeterPoint { get; set; }

        [Display(Name = "Negitive",Description ="正负项",Prompt = "正负项",ResourceType = typeof(resource.BillDetail))]
        [MaxLength(10)]
        public string Negitive { get; set; }

        [Required(ErrorMessage = "Please enter : 占比")]
        [Display(Name = "Ratio",Description ="占比",Prompt = "占比",ResourceType = typeof(resource.BillDetail))]
        public decimal Ratio { get; set; }

        [Required(ErrorMessage = "Please enter : 用水量(m3)")]
        [Display(Name = "Water",Description ="用水量(m3)",Prompt = "用水量(m3)",ResourceType = typeof(resource.BillDetail))]
        public decimal Water { get; set; }

        [Display(Name = "LastWater",Description ="上个月用水量(m3)",Prompt = "上个月用水量(m3)",ResourceType = typeof(resource.BillDetail))]
        public decimal LastWater { get; set; }

        [Display(Name = "PerCent",Description ="环比(%)",Prompt = "环比(%)",ResourceType = typeof(resource.BillDetail))]
        public decimal PerCent { get; set; }

        [Required(ErrorMessage = "Please enter : 实际用水量(m3)")]
        [Display(Name = "ActualWater",Description ="实际用水量(m3)",Prompt = "实际用水量(m3)",ResourceType = typeof(resource.BillDetail))]
        public decimal ActualWater { get; set; }

        [Required(ErrorMessage = "Please enter : 期初日期")]
        [Display(Name = "WaterDt1",Description ="期初日期",Prompt = "期初日期",ResourceType = typeof(resource.BillDetail))]
        public DateTime WaterDt1 { get; set; }

        [Required(ErrorMessage = "Please enter : 期初水量(m3)")]
        [Display(Name = "Water1",Description ="期初水量(m3)",Prompt = "期初水量(m3)",ResourceType = typeof(resource.BillDetail))]
        public decimal Water1 { get; set; }

        [Required(ErrorMessage = "Please enter : 期末日期")]
        [Display(Name = "WaterDt2",Description ="期末日期",Prompt = "期末日期",ResourceType = typeof(resource.BillDetail))]
        public DateTime WaterDt2 { get; set; }

        [Required(ErrorMessage = "Please enter : 期末水量(m3)")]
        [Display(Name = "Water2",Description ="期末水量(m3)",Prompt = "期末水量(m3)",ResourceType = typeof(resource.BillDetail))]
        public decimal Water2 { get; set; }

        [Display(Name = "Remark",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.BillDetail))]
        [MaxLength(250)]
        public string Remark { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.BillDetail))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.BillDetail))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.BillDetail))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.BillDetail))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : 租户主键")]
        [Display(Name = "TenantId",Description ="租户主键",Prompt = "租户主键",ResourceType = typeof(resource.BillDetail))]
        public int TenantId { get; set; }

    }

}
