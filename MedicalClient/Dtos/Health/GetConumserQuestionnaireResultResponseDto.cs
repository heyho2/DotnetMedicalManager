using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 获取用户问卷结果
    /// </summary>
    public class GetConumserQuestionnaireResultResponseDto : BaseDto
    {
        /// <summary>
        /// 问卷名称
        /// </summary>
        public string QuestionnaireName { get; set; }

        /// <summary>
        /// 问卷副标题
        /// </summary>
        public string Subhead { get; set; }

        /// <summary>
        /// 评论建议
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 评论建议时间
        /// </summary>
        public DateTime CommentDate { get; set; }
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
            /// 问题内容
            /// </summary>
            public string QuestionName { get; set; }

            /// <summary>
            /// 问题类型
            /// </summary>
            public string QuestionType { get; set; }

            /// <summary>
            /// 答题序号
            /// </summary>
            public int Sort { get; set; }

            /// <summary>
            /// 问题单位：仅数值型问题有
            /// </summary>
            public string Unit { get; set; }

            /// <summary>
            /// 提示文字：仅问答题有
            /// </summary>
            public string PromptText { get; set; }

            /// <summary>
            /// 问卷已选答案选项集合
            /// </summary>
            public IList<Answer> Answers { get; set; }

            /// <summary>
            /// 答题结果(填空题)
            /// </summary>
            public string Result { get; set; }

        }

        /// <summary>
        /// 问卷答案选项
        /// </summary>
        public class Answer
        {
            /// <summary>
            /// 答案选项值
            /// </summary>
            public string AnswerLabel { get; set; }

            /// <summary>
            /// 是否已选择
            /// </summary>
            public bool IsSelected { get; set; }

        }
    }
}
