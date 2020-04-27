
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Health
{
    /// <summary>
    /// 健康管理师随访记录表
    /// </summary>
    [Table("t_health_manager_followup_record")]
    public class FollowupRecordModel : BaseModel
    {
        /// <summary>
        /// 随访记录主键
        /// </summary>
        [Column("followup_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "随访记录主键")]
        public string FollowupGuid { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        [Column("consumer_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "会员id")]
        public string ConsumerGuid { get; set; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        [Column("name"), Required(ErrorMessage = "{0}必填"), Display(Name = "会员姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        [Column("phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "会员手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 健康管理师guid
        /// </summary>
        [Column("health_manager_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "健康管理师guid")]
        public string HealthManagerGuid { get; set; }

        /// <summary>
        /// 随访时间
        /// </summary>
        [Column("followup_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "随访时间")]
        public DateTime FollowupTime { get; set; }

        /// <summary>
        /// 随访记录
        /// </summary>
        [Column("content")]
        public string Content { get; set; }

        /// <summary>
        /// 随访建议
        /// </summary>
        [Column("suggestion")]
        public string Suggestion { get; set; }
    }
}



