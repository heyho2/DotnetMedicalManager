using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 获取问题详情
    /// </summary>
    public class GetFAQsDetailResponseDto : BaseDto
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 悬赏数额
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// 悬赏类型
        /// </summary>
        public string RewardType { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 提问日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 回答人数
        /// </summary>
        public int AnswerNum { get; set; }

        /// <summary>
        /// 查看者是否是本人
        /// </summary>
        public bool IsSelf { get; set; } = false;

        /// <summary>
        /// 附件图片地址
        /// </summary>
        public List<string> AttachedPictures { get; set; }


    }
}
