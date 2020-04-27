using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Questionnaire
{
    /// <summary>
    /// 获取当前问题可依赖的问题列表请求dto
    /// </summary>
    public class GetQuestionListCanDependRequestDto : BaseDto
    {
        /// <summary>
        /// 问卷guid
        /// </summary>
        [Required(ErrorMessage = "问卷guid必填")]
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 当前问题序号
        /// </summary>
        [Required(ErrorMessage = "当前问题序号必填")]
        public int Sort { get; set; }
    }

    /// <summary>
    /// 获取当前问题可依赖的问题列表响应Dto
    /// </summary>
    public class GetQuestionListCanDependResponseDto : BaseDto
    {
        /// <summary>
        /// 问题guid
        /// </summary>
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 题目名称
        /// </summary>
        public string QuestionName { get; set; }

        /// <summary>
        /// 问题序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 答案列表
        /// </summary>
        public List<Answer> Answers { get; set; }

        /// <summary>
        /// 问题答案选项
        /// </summary>
        public class Answer
        {
            /// <summary>
            /// 答案guid
            /// </summary>
            public string AnswerGuid { get; set; }

            /// <summary>
            /// 答案名称
            /// </summary>
            public string AnswerLabel { get; set; }

            /// <summary>
            /// 答案序号
            /// </summary>
            public int Sort { get; set; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetQuestionListCanDependItemDto : BaseDto
    {
        /// <summary>
        /// 问题guid
        /// </summary>
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 题目名称
        /// </summary>
        public string QuestionName { get; set; }

        /// <summary>
        /// 问题序号
        /// </summary>
        public int QuestionSort { get; set; }

        /// <summary>
        /// 答案guid
        /// </summary>
        public string AnswerGuid { get; set; }

        /// <summary>
        /// 答案名称
        /// </summary>
        public string AnswerLabel { get; set; }

        /// <summary>
        /// 答案序号
        /// </summary>
        public int AnswerSort { get; set; }

    }
}
