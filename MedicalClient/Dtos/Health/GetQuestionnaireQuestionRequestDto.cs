using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 问卷问题Dto
    /// </summary>
    public class GetQuestionnaireQuestionRequestDto
    {
        /// <summary>
        /// 问卷Id
        /// </summary>
        [Required(ErrorMessage = "问卷Id必填")]
        public string QuestionnaireGuid { get; set; }
        /// <summary>
        /// 问题Id
        /// </summary>
        public string QuestionGuid { get; set; }
        /// <summary>
        /// null:进入问卷 false:上一题 true:下一题
        /// </summary>
        public bool? NextQuestion { get; set; }
        /// <summary>
        /// 回答结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 答案数组Id对象
        /// </summary>
        public List<string> AnswerGuids { get; set; }
    }
    /// <summary>
    /// 题目查询Dto
    /// </summary>
    public class GetQuestionnaireQuestionResponseDto
    {
        /// <summary>
        /// 是否完成答题 false:未完成 true:完成
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 依赖当前提交题目答案(第一次做问卷或者继续答题如果下一题是依赖当前做的题目答案选择值)
        /// </summary>
        public List<string> DependLastAnswer { get; set; } = new List<string>();
        /// <summary>
        /// 提示
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 题目
        /// </summary>
        public GetQuestionnaireQuestionDto QuestionnaireQuestionDto { get; set; } = new GetQuestionnaireQuestionDto();
    }

    /// <summary>
    /// 问卷问题响应Dto
    /// </summary>
    public class GetQuestionnaireQuestionDto
    {
        /// <summary>
        /// 题目Id
        /// </summary>
        public string QuestionGuid { get; set; }
        /// <summary>
        /// 题目序号
        /// </summary>
        public int QuestionNumber { get; set; }
        /// <summary>
        /// 所属问卷guid
        /// </summary>
        public string QuestionnaireGuid { get; set; }
        /// <summary>
        /// 题目名称
        /// </summary>
        public string QuestionName { get; set; }
        /// <summary>
        /// 题目类型：单选、判断、数值、文本、多选 'Enum','Bool','Decimal','String','Array'
        /// </summary>
        public string QuestionType { get; set; }
        /// <summary>
        /// 问题单位：仅数值问题有此项
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 提示文字：仅问答题由此项
        /// </summary>
        public string PromptText { get; set; }
        /// <summary>
        /// 答案列表
        /// </summary>
        public List<QuestionnaireAnswerDto> QuestionnaireAnswerDtoList { get; set; }
        /// <summary>
        /// 回答结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 用户回答的答案数组Id对象
        /// </summary>
        public List<string> AnswerGuids { get; set; }

        /// <summary>
        /// 是否有下一题
        /// </summary>
        public bool HasNext { get; set; } = false;
    }
    /// <summary>
    /// 问题答案列表
    /// </summary>
    public class QuestionnaireAnswerDto
    {
        /// <summary>
        /// 问题答案主键
        /// </summary>
        public string AnswerGuid { get; set; }
        /// <summary>
        /// 答案名称
        /// </summary>
        public string AnswerLabel { get; set; }

        /// <summary>
        /// 是否是默认选项
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 答案顺序序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 选择此答案后是否存在下一题
        /// </summary>
        public bool HasNext { get; set; } = false;
    }
}
