using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="ZoneMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/28/2020 6:17:49 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(ZoneMetadata))]
    public partial class Zone
    {
    }

    public partial class ZoneMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.Zone))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 区域名称")]
        [Display(Name = "Name",Description ="区域名称",Prompt = "区域名称",ResourceType = typeof(resource.Zone))]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Description",Description ="描述",Prompt = "描述",ResourceType = typeof(resource.Zone))]
        [MaxLength(150)]
        public string Description { get; set; }

        [Display(Name = "GeoJSON",Description ="geo数据",Prompt = "geo数据",ResourceType = typeof(resource.Zone))]
        [MaxLength(50)]
        public string GeoJSON { get; set; }

    }

}
