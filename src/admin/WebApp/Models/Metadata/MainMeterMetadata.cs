using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="MainMeterMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>4/25/2020 11:14:00 AM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(MainMeterMetadata))]
    public partial class MainMeter
    {
    }

    public partial class MainMeterMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.MainMeter))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 日期")]
        [Display(Name = "date",Description ="日期",Prompt = "日期",ResourceType = typeof(resource.MainMeter))]
        public DateTime date { get; set; }

        [Required(ErrorMessage = "Please enter : 进水表现")]
        [Display(Name = "inwater",Description ="进水表现",Prompt = "进水表现",ResourceType = typeof(resource.MainMeter))]
        public decimal inwater { get; set; }

        [Required(ErrorMessage = "Please enter : 当天进水量")]
        [Display(Name = "involume",Description ="当天进水量",Prompt = "当天进水量",ResourceType = typeof(resource.MainMeter))]
        public decimal involume { get; set; }

        [Required(ErrorMessage = "Please enter : 出水表现")]
        [Display(Name = "outwater",Description ="出水表现",Prompt = "出水表现",ResourceType = typeof(resource.MainMeter))]
        public decimal outwater { get; set; }

        [Required(ErrorMessage = "Please enter : 当天用水量")]
        [Display(Name = "outvolume",Description ="当天用水量",Prompt = "当天用水量",ResourceType = typeof(resource.MainMeter))]
        public decimal outvolume { get; set; }

        [Display(Name = "remark",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.MainMeter))]
        [MaxLength(120)]
        public string remark { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.MainMeter))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.MainMeter))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.MainMeter))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.MainMeter))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.MainMeter))]
        public int TenantId { get; set; }

    }

}
