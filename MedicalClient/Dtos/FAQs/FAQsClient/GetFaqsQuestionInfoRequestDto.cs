using GD.Common.Base;
using System;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 获取问题详细 请求
    /// </summary>
    public class GetFaqsQuestionInfoRequestDto : BaseDto
    {
        /// <summary>
        /// 问题guid
        /// </summary>
        public string QuestionGuid { get; set; }
    }
    /// <summary>
    /// 获取问题详细 响应
    /// </summary>
    public class GetFaqsQuestionInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 问题guid
        /// </summary>
        public string QuestionGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        public string UserGuid { get; set; }

        ///<summary>
        ///悬赏数额
        ///</summary>
        public decimal RewardIntergral { get; set; }

        ///<summary>
        ///悬赏类型
        ///</summary>
        public string RewardType { get; set; }

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
        /// <summary>
        /// 图片附件
        /// </summary>
        public Attachment[] AttachedPictures { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Portrait { get; set; }

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
