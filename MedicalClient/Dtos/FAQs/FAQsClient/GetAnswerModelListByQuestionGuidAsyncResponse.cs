using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// AnswerModel + 点赞
    /// </summary>
    public class GetAnswerModelListByQuestionGuidAsyncResponse
    {

        ///<summary>
        ///主键
        ///</summary>
        public string AnswerGuid { get; set; }

        ///<summary>
        ///问题主键
        ///</summary>
        public string QuestionGuid { get; set; }

        ///<summary>
        ///用户
        ///</summary>
        public string UserGuid { get; set; }

        ///<summary>
        ///是否主解答
        ///</summary>
        public bool MainAnswer { get; set; }

        ///<summary>
        ///已获数额
        ///</summary>
        public int RewardIntergral { get; set; } = 0;
        ///<summary>
        ///悬赏类型
        ///</summary>
        public string ReceiveType { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreationDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string LastUpdatedBy { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string OrgGuid { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeCount { get; set; }


    }
}
