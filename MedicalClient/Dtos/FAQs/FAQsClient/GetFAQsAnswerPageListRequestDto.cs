using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 获取问题的回答列表请求dto
    /// </summary>
    public class GetFAQsAnswerPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 问题guid
        /// </summary>
        [Required(ErrorMessage = "问题guid必填")]
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 当前登录用户guid(用于判断用户是否点赞了指定的回答)
        /// </summary>
        public string UserId { get; set; }
    }


    /// <summary>
    /// 获取问题的回答列表响应dto
    /// </summary>
    public class GetFAQsAnswerPageListResponseDto : BasePageResponseDto<GetFAQsAnswerPageListItemDto>
    {

    }

    /// <summary>
    /// 获取问题的回答列表item dto
    /// </summary>
    public class GetFAQsAnswerPageListItemDto : BaseDto
    {
        /// <summary>
        /// 医生Guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 回答记录Guid
        /// </summary>
        public string AnswerGuid { get; set; }

        /// <summary>
        /// 是否最佳答案
        /// </summary>
        public bool MainAnswer { get; set; }

        /// <summary>
        /// 回答内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 医生头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 医生职称名称
        /// </summary>
        public string TitleName { get; set; }

        /// <summary>
        /// 数额
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 悬赏类型
        /// </summary>
        public string ReceiveType { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// 回答时间
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 如果用户一登录，则表示当前用户是否点赞了回答记录
        /// </summary>
        public bool IsLike { get; set; }
    }
}
