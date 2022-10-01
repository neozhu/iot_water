using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //区域
  public partial class Zone:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "区域名称", Description = "区域名称")]
    [MaxLength(50)]
    [DefaultValue("")]
    [Required]
    [Index(IsUnique =true)]
    public string Name{ get; set; }
    [Display(Name = "描述", Description = "描述")]
    [MaxLength(150)]
    public string Description { get; set; }
    [Display(Name = "geo数据", Description = "geo数据")]
    public string GeoJSON { get; set; }





  }
}