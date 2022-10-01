using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using TrackableEntities;
using System.Collections.Generic;
#nullable enable
namespace Repository.Pattern.Ef6
{
    public abstract class Entity : ITrackable, IAuditable, IMustHaveTenant
    {
        [Key]
        [Display(Name = "系统主键", Description = "系统主键")]
        public virtual int Id { get; set; }

        [NotMapped]
        public virtual TrackingState TrackingState { get; set; }
        [NotMapped]
        public virtual ICollection<string>? ModifiedProperties { get; set; }
        [Display(Name = "创建时间",Description = "创建时间")]
        [ScaffoldColumn(false)]
        public virtual DateTime? CreatedDate { get; set ; }
        [Display(Name = "创建用户", Description = "创建用户")]
        [MaxLength(20)]
        [ScaffoldColumn(false)]
        public virtual string CreatedBy { get; set; } = string.Empty;
        [Display(Name = "最后更新时间",Description = "最后更新时间")]
        [ScaffoldColumn(false)]
        public virtual DateTime? LastModifiedDate { get ; set ; }
        [Display(Name = "最后更新用户",Description = "最后更新用户")]
        [MaxLength(20)]
        [ScaffoldColumn(false)]
        public virtual string LastModifiedBy { get ; set; } = string.Empty;
        [Display(Name = "租户主键", Description = "租户主键")]
        [ScaffoldColumn(false)]
        [Index]
        public virtual int TenantId { get; set; }
    }
}