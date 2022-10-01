using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
    public partial class Notification:Entity
    {
        public Notification()
        {

        }
        [Key]
        public int Id { get; set; }
        [Display( Name ="主题",Description = "主题")]
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Display(Name = "消息内容", Description = "消息内容")]
        [MaxLength(255)]
        public string Content { get; set; }
        [Display(Name = "链接",Description = "链接")]
        [MaxLength(255)]
        public string Link { get; set; }
        [Display(Name = "已读",Description = "已读")]
    [DefaultValue(false)]
    public bool Read { get; set; }
        [Display(Name = "From", Description = "From")]
    [DefaultValue("user")]
    public string From { get; set; }
        [Display(Name = "To", Description = "From")]
    [DefaultValue("user")]
    public string To { get; set; }
        [Display(Name = "类型", Description = "类型")]
        public int Group { get; set; }
    [Display(Name = "发出时间", Description = "发出时间")]
    [DefaultValue("now")]
    public DateTime Created { get; set; }
 
       

    }
}