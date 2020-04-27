using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    /// <summary>
    /// 新增随访记录请求dto
    /// </summary>
    public class AddFollowupRecordRequestDto : BaseDto
    {
        /// <summary>
        /// 随访日期
        /// </summary>
        [Required(ErrorMessage ="随访日期必填")]
        public DateTime FollowupTime { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        [Required(ErrorMessage = "会员id必填")]
        public string ConsumerGuid { get; set; }

        /// <summary>
        /// 随访记录
        /// </summary>
        [Required(ErrorMessage = "随访记录必填")]
        public string Content { get; set; }

        /// <summary>
        /// 随访建议
        /// </summary>
        [Required(ErrorMessage = "随访建议必填")]
        public string Suggestion { get; set; }
    }
}
