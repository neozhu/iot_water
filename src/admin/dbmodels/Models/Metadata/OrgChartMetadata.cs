using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="OrgChartMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2020/11/3 19:40:08 </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(OrgChartMetadata))]
    public partial class OrgChart
    {
    }

    public partial class OrgChartMetadata
    {
        [Display(Name = "Parent",Description ="父级",Prompt = "父级",ResourceType = typeof(resource.OrgChart))]
        public OrgChart Parent { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.OrgChart))]
        public int Id { get; set; }

        [Display(Name = "No",Description ="总序号",Prompt = "总序号",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(3)]
        public string No { get; set; }

        [Required(ErrorMessage = "Please enter : 层级")]
        [Display(Name = "Level",Description ="层级",Prompt = "层级",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(16)]
        public string Level { get; set; }

        [Required(ErrorMessage = "Please enter : 层级序号")]
        [Display(Name = "LevelNo",Description ="层级序号",Prompt = "层级序号",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(3)]
        public string LevelNo { get; set; }

        [Display(Name = "Location",Description ="位置",Prompt = "位置",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(128)]
        public string Location { get; set; }

        [Display(Name = "Precision",Description ="精度",Prompt = "精度",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(8)]
        public string Precision { get; set; }

        [Display(Name = "DN",Description ="口径",Prompt = "口径",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(8)]
        public string DN { get; set; }

        [Display(Name = "Year",Description ="年份",Prompt = "年份",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(8)]
        public string Year { get; set; }

        [Display(Name = "Remark",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(128)]
        public string Remark { get; set; }

        [Display(Name = "ParentId",Description ="父级",Prompt = "父级",ResourceType = typeof(resource.OrgChart))]
        public int ParentId { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.OrgChart))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.OrgChart))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.OrgChart))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.OrgChart))]
        public int TenantId { get; set; }

    }

}
