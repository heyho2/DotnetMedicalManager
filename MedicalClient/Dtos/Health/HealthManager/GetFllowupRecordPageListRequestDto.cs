using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    /// <summary>
    /// 获取随访记录分页列表请求dto
    /// </summary>
    public class GetFllowupRecordPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 记录人Id
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 会员guid
        /// </summary>
        [Required(ErrorMessage = "会员id必填")]
        public string ConsumerGuid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetFollowupRecordPageListResponseDto : BasePageResponseDto<GetFollowupRecordPageListItemDto>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class GetFollowupRecordPageListItemDto : BaseDto
    {
        /// <summary>
        /// 随访记录id
        /// </summary>
        public string FollowupGuid { get; set; }

        /// <summary>
        /// 随访日期
        /// </summary>
        public DateTime FollowupTime { get; set; }

        /// <summary>
        /// 记录人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 随访记录
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 随访建议
        /// </summary>
        public string Suggestion { get; set; }
    }
}
