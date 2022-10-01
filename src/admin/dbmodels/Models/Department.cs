using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Department:Entity
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "部门名称",Description = "部门名称")]
        [MaxLength(10)]
        [MinLength(1)]
        public string Name { get; set; }
        [Display(Name = "部门主管", Description = "部门主管")]
        [MaxLength(10)]
        public string Manager { get; set; }
        [Display(Name = "所在公司")]
        public int CompanyId { get; set; }
         [ForeignKey("CompanyId")]
         [Display(Name = "所在公司")]
        public Company Company { get;set;}
    }

    public partial class Employee:Entity
    {
        [Key]
        public int Id { get; set; }
 
    [MaxLength(20)]
        [Required]
        [Index(IsUnique =true)]
        [Display(Name = "姓名", Description = "姓名")]
        public string Name { get; set; }
        [MaxLength(30)]
        [Display(Name = "职位", Description = "职位")]
    [DefaultValue("员工")]
    public string Title { get; set; }
    [MaxLength(30)]
    [Display(Name = "联系电话", Description = "联系电话")]
    public string PhoneNumber { get; set; }
    [MaxLength(30)]
    [Display(Name = "微信", Description = "微信")]
    public string WX { get; set; }
    [MaxLength(10)]
        [Required]
        [Display(Name = "性别", Description = "性别")]
        [DefaultValue("男")]
        public string Sex { get; set; }
        [Display(Name = "年龄", Description = "年龄")]
    [DefaultValue("25")]
    public int Age { get; set; }
        [Display(Name = "出生日期", Description = "出生日期")]
    [DefaultValue("1995/10/10")]
    public DateTime Brithday { get; set; }
    [Display(Name = "入职日期", Description = "入职日期")]
    [DefaultValue("now")]
    public DateTime EntryDate { get; set; }
    [Display(Name = "是否已离职", Description = "是否已离职")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    [Display(Name = "离职日期", Description = "离职日期")]
    [DefaultValue(null)]
    public DateTime? LeaveDate { get; set; }
    [Display(Name = "公司", Description = "公司")]
        [DefaultValue(1)]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [Display(Name = "公司信息", Description = "公司信息")]
        public Company Company { get; set; }

    [Display(Name = "部门", Description = "部门")]
    [DefaultValue(null)]
    public int DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    [Display(Name = "部门信息", Description = "部门信息")]
    public Department Department { get; set; }

  }
}