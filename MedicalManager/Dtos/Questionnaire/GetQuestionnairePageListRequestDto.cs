using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Questionnaire
{
    /// <summary>
    /// 获取问卷分页列表请求Dto
    /// </summary>
    public class GetQuestionnairePageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 问卷名称筛选条件
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 获取问卷分页列表响应Dto
    /// </summary>
    public class GetQuestionnairePageListResponseDto : BasePageResponseDto<GetQuestionnairePageListItemDto>
    {

    }

    /// <summary>
    /// 获取问卷分页列表响应详情Dto
    /// </summary>
    public class GetQuestionnairePageListItemDto : BaseDto
    {
        /// <summary>
        /// 问卷guid
        /// </summary>
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 问卷名称
        /// </summary>
        public string QuestionnaireName { get; set; }

        /// <summary>
        /// 创建者姓名
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 发放时间
        /// </summary>
        public DateTime? IssuingDate { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// 是否已发放
        /// </summary>
        public bool IssuingStatus { get; set; }
    }
}
