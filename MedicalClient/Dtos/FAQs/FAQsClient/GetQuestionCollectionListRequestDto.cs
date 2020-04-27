using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 获取用户收藏的问题列表
    /// </summary>
    public class GetQuestionCollectionListRequestDto : BasePageRequestDto
    {

    }

    /// <summary>
    /// 获取用户收藏的问题列表响应Dto
    /// </summary>
    public class GetQuestionCollectionListResponseDto : BasePageResponseDto<GetQuestionCollectionListItemDto>
    {

    }

    /// <summary>
    /// 获取用户收藏的问题列表 Item Dto
    /// </summary>
    public class GetQuestionCollectionListItemDto : BaseDto
    {
        /// <summary>
        /// 问题guid
        /// </summary>
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 提问日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int VisitCount { get; set; }

        /// <summary>
        /// 回答人数
        /// </summary>
        public int AnswerNum { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public string Status { get; set; }
    }
}
