using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{

    /// <summary>
    /// 获取问答广场最新问题请求Dto
    /// </summary>
    public class GetLatestFAQsPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
    }

    /// <summary>
    /// 获取问答广场最新问题响应Dto
    /// </summary>
    public class GetLatestFAQsPageListResponseDto : BasePageResponseDto<GetLatestFAQsPageListItemDto>
    {

    }

    /// <summary>
    /// 获取问答广场最新问题 Item Dto
    /// </summary>
    public class GetLatestFAQsPageListItemDto : BaseDto
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
