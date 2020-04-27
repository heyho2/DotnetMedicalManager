using GD.Common.Base;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace GD.Dtos.Faqs
{
    /// <summary>
    /// 搜索问题 请求
    /// </summary>
    public class SearchFaqsQuestionRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public RewardTypeEnum? RewardType { get; set; }
        /// <summary>
        /// 用户/手机
        /// </summary>
        public string Phone { get; set; }


        /// <summary>
        /// 悬赏类型枚举
        /// </summary>
        public enum RewardTypeEnum
        {
            /// <summary>
            /// 积分
            /// </summary>
            [Description("积分")]
            Intergral = 1,
            /// <summary>
            /// 人民币
            /// </summary>
            [Description("人民币")]
            Money

        }
    }
    /// <summary>
    /// 搜索问题 响应
    /// </summary>
    public class SearchFaqsQuestionResponseDto : BasePageResponseDto<SearchFaqsQuestionItemDto>
    {
    }
    /// <summary>
    /// 搜索问题 项
    /// </summary>
    public class SearchFaqsQuestionItemDto : BaseDto
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

        ///<summary>
        ///附件GUID，数据库为json格式：["Guid1","Guid2",……]
        ///</summary>
        [JsonIgnore]
        public string AttachmentGuidList { get; set; }
        /// <summary>
        /// 附件GUID
        /// </summary>
        [JsonIgnore]
        public string[] AttachmentGuidList2 { get; set; }
        /// <summary>
        ///附件GUID
        /// </summary>
        public Attachment[] Attachments { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 点赞
        /// </summary>
        public int LikeCount { get; set; }
        /// <summary>
        /// 收藏
        /// </summary>
        public int CollectionCount { get; set; }
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
        
        ///<summary>
        ///悬赏类型
        ///</summary>
        public string RewardType { get; set; }

        /// <summary>
        /// tupian
        /// </summary>
        public class Attachment
        {
            /// <summary>
            /// url
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// guid
            /// </summary>
            public string Guid { get; set; }

        }

    }
}
