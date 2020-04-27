using GD.Common.Base;
using GD.Dtos.Enum.Questionnaire;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Questionnaire
{
    /// <summary>
    /// 获取问卷详情
    /// </summary>
    public class GetQuestionnaireInfoResponseDto : BaseDto
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
        /// 问卷副标题
        /// </summary>
        public string Subhead { get; set; }

        /// <summary>
        /// 问题列表
        /// </summary>
        public List<Question> Questions { get; set; }

        /// <summary>
        /// 问卷问题
        /// </summary>
        public class Question
        {
            /// <summary>
            /// 问题guid
            /// </summary>
            public string QuestionGuid { get; set; }

            /// <summary>
            /// 问题内容
            /// </summary>
            public string QuestionName { get; set; }

            /// <summary>
            /// 问题类型
            /// </summary>
            public QuestionnaireQuestionTypeEnum QuestionType { get; set; }

            /// <summary>
            /// 是否依赖
            /// </summary>
            public bool IsDepend { get; set; }

            /// <summary>
            /// 依赖的问题答案guid
            /// </summary>
            public string DependAnswer { get; set; }

            /// <summary>
            /// 依赖答案所属问题guid
            /// </summary>
            public string DependQuestion { get; set; }

            /// <summary>
            /// 依赖描述
            /// </summary>
            public string DependDescription { get; set; }

            /// <summary>
            /// 问题序号
            /// </summary>
            public int Sort { get; set; }

            /// <summary>
            /// 问题单位：仅数值问题有
            /// </summary>
            public string Unit { get; set; }

            /// <summary>
            /// 问题提示：仅问答题有
            /// </summary>
            public string PromptText { get; set; }

            /// <summary>
            /// 问卷答案选项集合
            /// </summary>
            public IList<Answer> Answers { get; set; }

        }

        /// <summary>
        /// 问卷答案选项
        /// </summary>
        public class Answer
        {
            /// <summary>
            /// 答案项guid
            /// </summary>
            public string AnswerGuid { get; set; }

            /// <summary>
            /// 答案选项值
            /// </summary>
            public string AnswerLabel { get; set; }

            /// <summary>
            /// 是否是默认选项
            /// </summary>
            public bool IsDefault { get; set; }

        }
    }
}
