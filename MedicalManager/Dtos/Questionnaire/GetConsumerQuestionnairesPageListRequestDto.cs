using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;
using GD.Dtos.Enum.Questionnaire;

namespace GD.Dtos.Questionnaire
{
    /// <summary>
    /// 获取某一个问卷下消费者答题结果分页列表
    /// </summary>
    public class GetConsumerQuestionnairesPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 问卷guid
        /// </summary>
        [Required(ErrorMessage = "问卷guid必填")]
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 是否已提交
        /// </summary>
        public bool? FillStatus { get; set; }

        /// <summary>
        /// 是否已评价
        /// </summary>
        public bool? Commented { get; set; }


    }

    /// <summary>
    /// 获取某一个问卷下消费者答题结果分页列表响应
    /// </summary>
    public class GetConsumerQuestionnairesPageListResponseDto : BasePageResponseDto<GetConsumerQuestionnairesPageListItemDto>
    {

    }

    /// <summary>
    /// 获取某一个问卷下消费者答题结果分页列表响应详情
    /// </summary>
    public class GetConsumerQuestionnairesPageListItemDto : BaseDto
    {
        /// <summary>
        /// 用户答卷结果Guid
        /// </summary>
        public string ResultGuid { get; set; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 会员年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 填写状态
        /// </summary>
        public bool FillStatus { get; set; }

        /// <summary>
        /// 是否已评价
        /// </summary>
        public bool Commented { get; set; }
    }
}
