using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  public partial class OrgChart : Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name ="总序号",Description = "总序号")]
    [MaxLength(3)]
    [Index(IsUnique =true)]
    public string No { get; set; }
    [Display(Name = "层级", Description = "层级")]
    [MaxLength(16)]
    [Required]
    public string Level { get; set; }
    [Display(Name = "层级序号", Description = "层级序号")]
    [MaxLength(3)]
    [Required]
    public string LevelNo { get; set; }
    [Display(Name = "位置", Description = "位置")]
    [MaxLength(128)]
    public string Location { get; set; }
    [Display(Name = "精度", Description = "精度")]
    [MaxLength(8)]
    public string Precision { get; set; }
    [Display(Name = "口径", Description = "口径")]
    [MaxLength(8)]
    public string DN { get; set; }
    [Display(Name = "年份", Description = "年份")]
    [MaxLength(8)]
    public string Year { get; set; }
    [Display(Name = "备注", Description = "备注")]
    [MaxLength(128)]
    public string Remark { get; set; }
    [Display(Name = "父级", Description = "父级")]
    public int? ParentId { get; set; }
    [Display(Name = "父级", Description = "父级")]
    [ForeignKey("ParentId")]
    public OrgChart Parent { get; set; }
  }
}