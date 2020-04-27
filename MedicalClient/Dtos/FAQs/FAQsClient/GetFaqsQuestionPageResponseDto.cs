using GD.Common.Base;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 搜索问题 请求
    /// </summary>
    public class GetFaqsQuestionPageRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public SortByEnum SortBy { get; set; }

        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 最近N天,等于0表示不限制时间
        /// </summary>
        [Required(ErrorMessage = "最近天数必填")]
        public int LatestDay { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public enum SortByEnum
        {
            /// <summary>
            /// 热门 
            /// </summary>
            Popular = 1,
            /// <summary>
            /// 最新
            /// </summary>
            Newest
        }
    }
    /// <summary>
    /// 搜索问题 响应
    /// </summary>
    public class GetFaqsQuestionPageResponseDto : BasePageResponseDto<GetFaqsQuestionPageItemDto>
    {
    }
    /// <summary>
    /// 搜索问题 项
    /// </summary>
    public class GetFaqsQuestionPageItemDto : BaseDto
    {
        ///<summary>
        ///提问主键
        ///</summary>
        public string QuestionGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        public string UserGuid { get; set; }

        ///<summary>
        ///悬赏积分
        ///</summary>
        public int RewardIntergral { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        public string Status { get; set; }

        ///<summary>
        ///已抢答数
        ///</summary>
        public int AnswerNum { get; set; }

        ///<summary>
        ///内容
        ///</summary>
        public string Content { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 点赞
        /// </summary>
        public int LikeCount { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 浏览
        /// </summary>
        public int VisitCount { get; set; }
    }
}
