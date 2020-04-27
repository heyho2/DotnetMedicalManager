using GD.Common.Base;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerFllowupRecordsRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 会员Guid
        /// </summary>
        [Required(ErrorMessage = "指定会员参数未提供")]
        public string ConsumerGuid { get; set; }
        /// <summary>
        /// 姓名/手机号码
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 随访时间
        /// </summary>
        public DateTime? FollowUpTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerFllowupRecordsResponseDto : BasePageResponseDto<GetHealthManagerFllowupRecordsItem>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerFllowupRecordsItem : BaseDto
    {
        /// <summary>
        /// 健康管理师姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 健康管理师手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 随访记录
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 随访建议
        /// </summary>
        public string Suggestion { get; set; }
        /// <summary>
        ///随访时间
        /// </summary>
        public DateTime FollowUpTime { get; set; }
    }
}
