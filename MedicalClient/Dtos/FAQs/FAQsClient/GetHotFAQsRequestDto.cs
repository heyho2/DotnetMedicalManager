using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 获取问答广场热门问题请求Dto
    /// </summary>
    public class GetHotFAQsRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 最近N天,等于0表示不限制时间
        /// </summary>
        [Required(ErrorMessage = "最近天数必填")]
        public int LatestDay { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
    }

    /// <summary>
    /// 获取问答广场热门问题响应Dto
    /// </summary>
    public class GetHotFAQsResponseDto : BasePageResponseDto<GetHotFAQsItemDto>
    {

    }

    /// <summary>
    /// 获取问答广场热门问题 Item Dto
    /// </summary>
    public class GetHotFAQsItemDto : BaseDto
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
