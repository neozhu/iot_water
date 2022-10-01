using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  public partial class Log:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name ="主机名",Description = "主机名")]
    [MaxLength(50)]
    public string MachineName { get; set; }
    [Display(Name = "时间", Description = "时间")]
    public DateTime? Logged { get; set; }
    [Display(Name = "级别", Description = "级别")]
    [MaxLength(5)]
    public string Level { get; set; }
    [Display(Name = "信息", Description = "信息")]
    public string Message { get; set; }
    [Display(Name = "异常信息", Description = "异常信息")]
    public string Exception { get; set; }
    [Display(Name = "客户端IP", Description = "客户端IP")]
    public string ClientIP { get; set; }
    [Display(Name = "事件属性", Description = "事件属性")]
    public string Properties { get; set; }
    [Display(Name = "使用账号", Description = "使用账号")]
    [MaxLength(50)]
    public string User { get; set; }
    [Display(Name = "日志", Description = "日志")]
    [MaxLength(300)]
    public string Logger { get; set; }
    [Display(Name = "站点", Description = "站点")]
    [MaxLength(300)]
    public string Callsite { get; set; }
    [Display(Name = "已处理", Description = "已处理")]
    public bool Resolved { get; set; }
  }
}