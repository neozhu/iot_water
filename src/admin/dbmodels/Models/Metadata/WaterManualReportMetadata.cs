using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="WaterManualReportMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2021 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/6/2021 5:31:32 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(WaterManualReportMetadata))]
    public partial class WaterManualReport
    {
    }

    public partial class WaterManualReportMetadata
    {
        [Required(ErrorMessage = "Please enter : 系统主键")]
        [Display(Name = "Id",Description ="系统主键",Prompt = "系统主键",ResourceType = typeof(resource.WaterManualReport))]
        public int Id { get; set; }

        [Display(Name = "meterId",Description ="表号",Prompt = "表号",ResourceType = typeof(resource.WaterManualReport))]
        [MaxLength(20)]
        public string meterId { get; set; }

        [Display(Name = "Name",Description ="表名",Prompt = "表名",ResourceType = typeof(resource.WaterManualReport))]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "LineNo",Description ="表序号",Prompt = "表序号",ResourceType = typeof(resource.WaterManualReport))]
        [MaxLength(50)]
        public string LineNo { get; set; }

        [Display(Name = "Name1",Description ="出线名称",Prompt = "出线名称",ResourceType = typeof(resource.WaterManualReport))]
        [MaxLength(50)]
        public string Name1 { get; set; }

        [Required(ErrorMessage = "Please enter : 本期读数")]
        [Display(Name = "Water",Description ="本期读数",Prompt = "本期读数",ResourceType = typeof(resource.WaterManualReport))]
        public decimal Water { get; set; }

        [Required(ErrorMessage = "Please enter : 抄表日期")]
        [Display(Name = "RecordDate",Description ="抄表日期",Prompt = "抄表日期",ResourceType = typeof(resource.WaterManualReport))]
        public DateTime RecordDate { get; set; }

        [Display(Name = "LastWater",Description ="上期读数",Prompt = "上期读数",ResourceType = typeof(resource.WaterManualReport))]
        public decimal LastWater { get; set; }

        [Display(Name = "LastRecordDate",Description ="上期抄表日期",Prompt = "上期抄表日期",ResourceType = typeof(resource.WaterManualReport))]
        public DateTime LastRecordDate { get; set; }

        [Display(Name = "CalWater",Description ="本期用水量",Prompt = "本期用水量",ResourceType = typeof(resource.WaterManualReport))]
        public decimal CalWater { get; set; }

        [Display(Name = "Remark",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.WaterManualReport))]
        [MaxLength(250)]
        public string Remark { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.WaterManualReport))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.WaterManualReport))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.WaterManualReport))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.WaterManualReport))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : 租户主键")]
        [Display(Name = "TenantId",Description ="租户主键",Prompt = "租户主键",ResourceType = typeof(resource.WaterManualReport))]
        public int TenantId { get; set; }

    }

}
