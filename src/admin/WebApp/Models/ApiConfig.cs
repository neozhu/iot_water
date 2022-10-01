using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //调用第三方平台接口参数
  public partial class ApiConfig : Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "服务名", Description = "服务名")]
    [MaxLength(50)]
    [Required]
    public string ServiceName { get; set; }
    [Display(Name = "主机地址", Description = "主机地址")]
    [MaxLength(50)]
    [Required]
    public string Host { get; set; }
    [Display(Name = "安全代码", Description = "安全代码")]
    [MaxLength(250)]
    public string SecretAccessKey { get; set; }
    [Display(Name = "访问代码", Description = "访问代码")]
    [MaxLength(250)]
    public string AccessKeyId { get; set; }
    [Display(Name = "登录用户名", Description = "登录用户名")]
    [MaxLength(50)]
    public string UserId { get; set; }
    [Display(Name = "登录密码", Description = "登录密码")]
    [MaxLength(50)]
    public string Password { get; set; }
    [Display(Name = "描述", Description = "描述")]
    [MaxLength(250)]
    public string Description { get; set; }

    [Display(Name = "所属企业", Description = "所属企业")]
    public int CompanyId { get; set; }
    [ForeignKey("CompanyId")]
    [Display(Name = "所属企业", Description = "所属企业")]
    public Company Company { get; set; }


  }
}