using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 健康问卷Dto
    /// </summary>
    public class GetHealthQuestionnairePageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 非用户端登录需要传Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 问卷状态 fasle:未提交 true:已提交
        /// </summary>
        public bool FillStatus { get; set; }
    }
    /// <summary>
    /// 健康问卷响应Dto
    /// </summary>
    public class GetHealthQuestionnairePageListResponseDto : BasePageResponseDto<GetHealthQuestionnairePageListItemDto>
    {

    }
    /// <summary>
    /// 列表
    /// </summary>
    public class GetHealthQuestionnairePageListItemDto : BaseDto
    {
        /// <summary>
        /// 问卷id
        /// </summary>
        public string QuestionnaireGuid { get; set; }
        /// <summary>
        /// 用户问卷id
        /// </summary>
        public string ResultGuid { get; set; }
        /// <summary>
        /// 问卷名称
        /// </summary>
        public string QuestionnaireName { get; set; }
        /// <summary>
        /// 问卷副标题
        /// </summary>
        public string Subhead { get; set; }
        /// <summary>
        /// 问卷包含问题是否存在关联 false:不存在 ture:存在
        /// </summary>
        public bool HasDepend { get; set; }
        /// <summary>
        /// 填写状态:是否已提交
        /// </summary>
        public bool? FillStatus { get; set; }
        /// <summary>
        /// 是否已评论
        /// </summary>
        public bool Commented { get; set; }
        /// <summary>
        /// 评论建议
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 问卷不存在依赖问题的题目总数量
        /// </summary>
        public int QuestionCount { get; set; }
        /// <summary>
        /// 用户已答题数量
        /// </summary>
        public int UseranswerCount { get; set; }
    }
}
